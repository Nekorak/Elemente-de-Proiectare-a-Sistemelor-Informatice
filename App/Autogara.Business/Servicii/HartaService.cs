using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Entitati;
using Autogara.Domain.Enumerari;
using Autogara.FileServer;
using Autogara.FileServer.Modele;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Autogara.Business.Servicii;

/// <summary>
/// Harta statiilor: nodurile si conexiunile se salveaza in SQL (sursa de adevar), apoi se
/// rescrie Harta/harta.json pe file server ca oglinda.
/// </summary>
public sealed class HartaService(BazaDeDate bd, JsonHartaRepository fisier, CacheLocalService cache, Sesiune sesiune)
{
    public Task<HartaDto> IncarcaHartaAsync()
    {
        sesiune.CereAutentificare();
        return cache.ObtineAsync("harta", () => bd.CitesteAsync(CitesteDinBazaAsync));
    }

    /// <summary>
    /// Salveaza harta din editor. Nodurile cu NodID &lt;= 0 sunt noi (ID-ul negativ e doar o
    /// referinta temporara folosita in conexiuni). Nodurile care lipsesc se dezactiveaza
    /// (soft-delete); conexiunile care lipsesc se sterg.
    /// </summary>
    /// <param name="ambeleSensuri">Fiecare drum din editor se salveaza si in sens invers, ca in seed.</param>
    public async Task<RezultatSalvareHarta> SalveazaHartaAsync(HartaDto harta, bool ambeleSensuri = true)
    {
        var admin = sesiune.CereAdmin();

        var conexiuni = harta.Conexiuni.ToList();
        if (ambeleSensuri)
            conexiuni.AddRange(harta.Conexiuni
                .Where(c => !harta.Conexiuni.Any(x => x.NodPlecareID == c.NodSosireID && x.NodSosireID == c.NodPlecareID))
                .Select(c => new ConexiuneDto { NodPlecareID = c.NodSosireID, NodSosireID = c.NodPlecareID, DistantaKm = c.DistantaKm }));

        ValidareException.AruncaDaca(Validare.Harta(CaFisier(harta.Noduri, conexiuni)));

        var (noduriNoi, dezactivate) = await bd.InTranzactieAsync(async db =>
        {
            var existente = await db.Noduri.Include(n => n.Statie).Where(n => n.Activ).ToDictionaryAsync(n => n.NodID);
            var idNou = new Dictionary<int, int>();

            // 1. noduri noi si modificate
            var adaugate = new List<(int Temporar, Nod Nod)>();
            foreach (var n in harta.Noduri)
            {
                if (n.NodID <= 0)
                {
                    var nou = new Nod { Tip = n.Tip, Nume = n.Nume.Trim(), CoordX = n.X, CoordY = n.Y, Activ = true };
                    if (n.Tip == TipNod.Statie)
                        nou.Statie = new Statie();
                    db.Noduri.Add(nou);
                    adaugate.Add((n.NodID, nou));
                    continue;
                }

                if (!existente.TryGetValue(n.NodID, out var e))
                    throw new RegulaException($"Nodul {n.NodID} ({n.Nume}) nu mai există sau a fost dezactivat între timp. Reîncărcați harta.");

                if (e.Tip != n.Tip)
                {
                    if (n.Tip == TipNod.Intersectie && e.Statie is not null && await StatieFolositaAsync(db, e.Statie.StatieID))
                        throw new RegulaException($"„{e.Nume}” este stație în trasee existente și nu poate deveni intersecție.");
                    if (n.Tip == TipNod.Statie && e.Statie is null)
                        e.Statie = new Statie();
                }

                e.Tip = n.Tip;
                e.Nume = n.Nume.Trim();
                e.CoordX = n.X;
                e.CoordY = n.Y;
            }

            // 2. noduri sterse din editor -> dezactivate
            var pastrate = harta.Noduri.Where(n => n.NodID > 0).Select(n => n.NodID).ToHashSet();
            var deDezactivat = existente.Values.Where(e => !pastrate.Contains(e.NodID)).ToList();
            foreach (var e in deDezactivat)
            {
                if (e.Statie is not null && await StatieFolositaAsync(db, e.Statie.StatieID))
                    throw new RegulaException($"Stația „{e.Nume}” este folosită în trasee active și nu poate fi ștearsă de pe hartă.");
                e.Activ = false;
            }

            await db.SaveChangesAsync();
            foreach (var (temporar, nod) in adaugate)
                idNou[temporar] = nod.NodID;

            int Real(int id) => id <= 0 ? idNou[id] : id;

            // 3. conexiuni: se inlocuieste setul dintre nodurile active
            var active = pastrate.Concat(idNou.Values).ToHashSet();
            var dorite = conexiuni.ToDictionary(c => (Real(c.NodPlecareID), Real(c.NodSosireID)), c => c.DistantaKm);
            var actuale = await db.Conexiuni.ToListAsync();

            foreach (var c in actuale)
            {
                var implicaActive = active.Contains(c.NodPlecareID) || active.Contains(c.NodSosireID)
                                    || deDezactivat.Any(d => d.NodID == c.NodPlecareID || d.NodID == c.NodSosireID);
                if (!implicaActive)
                    continue;

                if (dorite.Remove((c.NodPlecareID, c.NodSosireID), out var distanta))
                    c.DistantaKm = distanta;
                else
                    db.Conexiuni.Remove(c);
            }

            foreach (var ((de, la), distanta) in dorite)
                db.Conexiuni.Add(new Conexiune { NodPlecareID = de, NodSosireID = la, DistantaKm = distanta });

            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID,
                $"Salvare hartă: {harta.Noduri.Count} noduri, {conexiuni.Count} conexiuni", "Noduri", null);

            return ((IReadOnlyDictionary<int, int>)idNou, deDezactivat.Count);
        });

        var (actualizat, avertisment) = await ExportaInFisierInternAsync();
        return new RezultatSalvareHarta(noduriNoi, dezactivate, actualizat, avertisment);
    }

    /// <summary>Rescrie harta.json din baza de date (ex. dupa o salvare in care file server-ul nu a raspuns).</summary>
    public async Task ExportaInFisierAsync()
    {
        sesiune.CereAdmin();
        var (actualizat, avertisment) = await ExportaInFisierInternAsync();
        if (!actualizat)
            throw new ConexiuneException(avertisment!);
    }

    /// <summary>Harta asa cum e in harta.json (copia de pe file server, nu baza de date).</summary>
    public async Task<HartaDto> CitesteDinFisierAsync()
    {
        sesiune.CereAutentificare();
        var f = await fisier.ReadAsync();
        return new HartaDto
        {
            Noduri = f.Noduri.Select(n => new NodDto
            {
                NodID = n.Id, Tip = ValoriDb.FromDb<TipNod>(n.Tip), Nume = n.Nume, X = n.X, Y = n.Y,
            }).ToList(),
            Conexiuni = f.Conexiuni.Select(c => new ConexiuneDto
            {
                NodPlecareID = c.NodPlecareId, NodSosireID = c.NodSosireId, DistantaKm = c.DistantaKm,
            }).ToList(),
        };
    }

    /// <summary>Cel mai scurt drum intre doua noduri (sau statii, prin NodID-ul lor).</summary>
    public async Task<RutaDto?> CeaMaiScurtaRutaAsync(int nodPlecareId, int nodSosireId)
    {
        var harta = await IncarcaHartaAsync();
        var drum = Rute.CelMaiScurt(Muchii(harta), nodPlecareId, nodSosireId);
        if (drum is null)
            return null;

        var nume = harta.Noduri.ToDictionary(n => n.NodID, n => n.Nume);
        return new RutaDto(drum.Noduri, drum.Noduri.Select(id => nume.GetValueOrDefault(id, $"#{id}")).ToList(), drum.DistantaKm);
    }

    /// <summary>
    /// Distanta propusa pentru o conexiune noua din editor: lungimea pe harta inmultita cu
    /// scara medie (km pe unitate) a conexiunilor existente, rotunjita la 0,5 km.
    /// </summary>
    public static decimal EstimeazaDistantaKm(HartaDto harta, int nodA, int nodB)
    {
        var noduri = harta.Noduri.ToDictionary(n => n.NodID);
        if (!noduri.TryGetValue(nodA, out var a) || !noduri.TryGetValue(nodB, out var b))
            return 1;

        double Lungime(NodDto x, NodDto y) => Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2));

        var masurate = harta.Conexiuni
            .Where(c => noduri.ContainsKey(c.NodPlecareID) && noduri.ContainsKey(c.NodSosireID))
            .Select(c => (Km: (double)c.DistantaKm, Unitati: Lungime(noduri[c.NodPlecareID], noduri[c.NodSosireID])))
            .Where(m => m.Unitati > 0)
            .ToList();

        var scara = masurate.Count > 0 ? masurate.Sum(m => m.Km) / masurate.Sum(m => m.Unitati) : 0.25;
        var km = Math.Round(Lungime(a, b) * scara * 2, MidpointRounding.AwayFromZero) / 2;
        return (decimal)Math.Clamp(km, 0.5, 9999);
    }

    internal static IEnumerable<Rute.Muchie> Muchii(HartaDto harta) =>
        harta.Conexiuni.Select(c => new Rute.Muchie(c.NodPlecareID, c.NodSosireID, c.DistantaKm));

    internal static async Task<HartaDto> CitesteDinBazaAsync(AutogaraDbContext db)
    {
        var noduri = await db.Noduri.Where(n => n.Activ)
            .OrderBy(n => n.NodID)
            .Select(n => new NodDto
            {
                NodID = n.NodID, Tip = n.Tip, Nume = n.Nume, X = n.CoordX, Y = n.CoordY,
                StatieID = n.Statie == null ? null : n.Statie.StatieID,
            })
            .ToListAsync();

        var conexiuni = await db.Conexiuni
            .Where(c => c.NodPlecare!.Activ && c.NodSosire!.Activ)
            .OrderBy(c => c.NodPlecareID).ThenBy(c => c.NodSosireID)
            .Select(c => new ConexiuneDto { NodPlecareID = c.NodPlecareID, NodSosireID = c.NodSosireID, DistantaKm = c.DistantaKm })
            .ToListAsync();

        return new HartaDto { Noduri = noduri, Conexiuni = conexiuni };
    }

    private async Task<(bool, string?)> ExportaInFisierInternAsync()
    {
        try
        {
            var harta = await bd.CitesteAsync(CitesteDinBazaAsync);
            await fisier.WriteAsync(CaFisier(harta.Noduri, harta.Conexiuni));
            await cache.SalveazaAsync("harta", harta);
            return (true, null);
        }
        catch (AutogaraException ex)
        {
            Log.Warning(ex, "Harta salvată în baza de date, dar harta.json nu a putut fi actualizat");
            return (false, $"Harta a fost salvată în baza de date, dar fișierul harta.json nu a putut fi actualizat: {ex.Message}");
        }
    }

    private static HartaFisier CaFisier(IEnumerable<NodDto> noduri, IEnumerable<ConexiuneDto> conexiuni) => new()
    {
        Noduri = noduri.Select(n => new NodHarta { Id = n.NodID, Tip = ValoriDb.ToDb(n.Tip), Nume = n.Nume, X = n.X, Y = n.Y }).ToList(),
        Conexiuni = conexiuni.Select(c => new ConexiuneHarta
        {
            NodPlecareId = c.NodPlecareID, NodSosireId = c.NodSosireID, DistantaKm = c.DistantaKm,
        }).ToList(),
    };

    private static Task<bool> StatieFolositaAsync(AutogaraDbContext db, int statieId) =>
        db.TraseuOpriri.AnyAsync(o => o.StatieID == statieId && o.Traseu!.Activ);
}
