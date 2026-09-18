using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.DataAccess.Vederi;
using Autogara.Domain.Entitati;
using Autogara.Domain.Enumerari;
using Autogara.FileServer.Modele;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Autogara.Business.Servicii;

public sealed class CursaService(BazaDeDate bd, AutobuzService autobuze, CacheLocalService cache, Sesiune sesiune)
{
    /// <summary>
    /// Cursele dintr-o zi care trec prin statia de plecare si apoi prin cea de sosire
    /// (in aceasta ordine pe traseu). Oricare statie poate lipsi.
    /// </summary>
    public Task<List<CursaGasita>> CautaCurseAsync(int? statiePlecareId, int? statieSosireId, DateOnly data)
    {
        sesiune.CereAutentificare();
        if (statiePlecareId is not null && statiePlecareId == statieSosireId)
            throw new ValidareException("Stația de plecare și cea de sosire trebuie să fie diferite.");

        return bd.CitesteAsync(async db =>
        {
            var q = db.Curse.Where(c => c.DataCursa == data && c.Status != StatusCursa.Anulata);

            if (statiePlecareId is { } p && statieSosireId is { } s)
                q = q.Where(c => db.TraseuOpriri.Any(a => a.TraseuID == c.TraseuID && a.StatieID == p
                    && db.TraseuOpriri.Any(b => b.TraseuID == c.TraseuID && b.StatieID == s && b.Ordine > a.Ordine)));
            else if (statiePlecareId is { } doarP)
                q = q.Where(c => db.TraseuOpriri.Any(o => o.TraseuID == c.TraseuID && o.StatieID == doarP));
            else if (statieSosireId is { } doarS)
                q = q.Where(c => db.TraseuOpriri.Any(o => o.TraseuID == c.TraseuID && o.StatieID == doarS));

            var curse = await q
                .OrderBy(c => c.OraPlecare)
                .Select(c => new
                {
                    c.CursaID, c.DataCursa, c.OraPlecare, c.OraSosireEstimata, c.Pret, c.Status,
                    Traseu = c.Traseu!.Denumire,
                    c.Autobuz!.NrInmatriculare,
                    c.Autobuz.Model,
                    Libere = c.Locuri.Count(l => l.Status == StatusLoc.Liber),
                    Opriri = c.Traseu.Opriri.OrderBy(o => o.Ordine)
                        .Select(o => new { o.StatieID, o.Statie!.Nod!.Nume, o.Statie.Peron })
                        .ToList(),
                })
                .ToListAsync();

            var acum = OraLocala.Acum;
            return curse.Select(c =>
            {
                var urcare = c.Opriri.FirstOrDefault(o => o.StatieID == statiePlecareId) ?? c.Opriri.FirstOrDefault();
                var coborare = c.Opriri.FirstOrDefault(o => o.StatieID == statieSosireId) ?? c.Opriri.LastOrDefault();
                var disponibila = c.Status == StatusCursa.Planificata && c.DataCursa.ToDateTime(c.OraPlecare) > acum && c.Libere > 0;

                return new CursaGasita(c.CursaID, c.Traseu, c.DataCursa, c.OraPlecare, c.OraSosireEstimata, c.Pret, c.Status,
                    urcare?.Nume, urcare?.Peron, coborare?.Nume, c.NrInmatriculare, c.Model, c.Libere, disponibila);
            }).ToList();
        });
    }

    /// <summary>Cursele de azi si din viitor, planificate sau in desfasurare (vw_CurseActive).</summary>
    public Task<List<CursaActivaRand>> CurseActiveAsync()
    {
        sesiune.CerePersonal();

        return cache.ObtineAsync("curse-active", () => bd.CitesteAsync(db =>
            db.CurseActive.OrderBy(c => c.DataCursa).ThenBy(c => c.OraPlecare).ToListAsync()));
    }

    public Task<List<CursaRand>> ListeazaAsync(DateOnly deLa, DateOnly panaLa, int? traseuId = null)
    {
        sesiune.CerePersonal();
        if (panaLa < deLa)
            throw new ValidareException("Intervalul de date este invalid.");

        return bd.CitesteAsync(db =>
            db.Curse
                .Where(c => c.DataCursa >= deLa && c.DataCursa <= panaLa && (traseuId == null || c.TraseuID == traseuId))
                .OrderBy(c => c.DataCursa).ThenBy(c => c.OraPlecare)
                .Select(c => new CursaRand(
                    c.CursaID, c.DataCursa, c.OraPlecare, c.OraSosireEstimata, c.Pret, c.Status,
                    c.TraseuID, c.Traseu!.Denumire, c.AutobuzID, c.Autobuz!.NrInmatriculare,
                    c.SoferID, c.Sofer!.Prenume + " " + c.Sofer.Nume,
                    c.Locuri.Count(),
                    c.Locuri.Count(l => l.Status == StatusLoc.Liber),
                    c.Locuri.Count(l => l.Status == StatusLoc.Ocupat)))
                .ToListAsync());
    }

    public Task<int> CreeazaCursaAsync(CursaEditare date)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(ValideazaDate(date));

        return bd.InTranzactieAsync(async db =>
        {
            await ValideazaResurseAsync(db, date, cursaId: null);

            var c = new Cursa
            {
                TraseuID = date.TraseuID,
                AutobuzID = date.AutobuzID,
                SoferID = date.SoferID,
                DataCursa = date.DataCursa,
                OraPlecare = date.OraPlecare,
                OraSosireEstimata = date.OraSosireEstimata,
                Pret = date.Pret,
                Status = StatusCursa.Planificata,
            };
            db.Curse.Add(c);
            await db.SaveChangesAsync();

            var locuri = await db.CreeazaLocuriPentruCursaAsync(c.CursaID);
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID,
                $"Creare cursă {date.DataCursa:dd.MM.yyyy} {date.OraPlecare:HH\\:mm} ({locuri} locuri)", "Curse", c.CursaID.ToString());
            return c.CursaID;
        });
    }

    /// <summary>
    /// Pretul, orele si soferul se pot schimba oricand cat timp cursa e planificata (biletele vandute
    /// isi pastreaza pretul). Traseul, data si autobuzul — doar daca nu s-a vandut niciun bilet.
    /// </summary>
    public Task ActualizeazaCursaAsync(int cursaId, CursaEditare date)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(ValideazaDate(date));

        return bd.InTranzactieAsync(async db =>
        {
            var c = await db.Curse.FirstOrDefaultAsync(x => x.CursaID == cursaId)
                    ?? throw new NegasitException("Cursa nu există.");

            if (c.Status != StatusCursa.Planificata)
                throw new RegulaException("Doar cursele planificate pot fi modificate.");

            var schimbareMajora = c.TraseuID != date.TraseuID || c.DataCursa != date.DataCursa || c.AutobuzID != date.AutobuzID;
            if (schimbareMajora)
            {
                if (await db.Bilete.AnyAsync(b => b.CursaID == cursaId))
                    throw new RegulaException("Pentru cursă s-au emis bilete: traseul, data și autobuzul nu mai pot fi schimbate. Anulați cursa și creați alta.");
                if (await db.RezervariProvizorii.AnyAsync(r => r.Loc!.CursaID == cursaId))
                    throw new RegulaException("Există rezervări în curs pe această cursă. Încercați din nou după finalizarea lor.");
            }

            await ValideazaResurseAsync(db, date, cursaId);

            var autobuzSchimbat = c.AutobuzID != date.AutobuzID;
            c.TraseuID = date.TraseuID;
            c.AutobuzID = date.AutobuzID;
            c.SoferID = date.SoferID;
            c.DataCursa = date.DataCursa;
            c.OraPlecare = date.OraPlecare;
            c.OraSosireEstimata = date.OraSosireEstimata;
            c.Pret = date.Pret;

            if (autobuzSchimbat)
                await db.Locuri.Where(l => l.CursaID == cursaId).ExecuteDeleteAsync();

            await db.SaveChangesAsync();

            if (autobuzSchimbat)
                await db.CreeazaLocuriPentruCursaAsync(cursaId);

            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Modificare cursă #{cursaId}", "Curse", cursaId.ToString());
        });
    }

    /// <summary>Planificata → In desfasurare → Finalizata. Anularea are metoda ei.</summary>
    public Task SchimbaStatusAsync(int cursaId, StatusCursa statusNou)
    {
        var u = sesiune.CerePersonal();

        return bd.InTranzactieAsync(async db =>
        {
            var c = await db.Curse.FirstOrDefaultAsync(x => x.CursaID == cursaId)
                    ?? throw new NegasitException("Cursa nu există.");

            var permis = (c.Status, statusNou) is (StatusCursa.Planificata, StatusCursa.InDesfasurare)
                                                or (StatusCursa.InDesfasurare, StatusCursa.Finalizata);
            if (statusNou == StatusCursa.Anulata)
                throw new RegulaException("Pentru anulare folosiți operația „Anulează cursa” (rambursează biletele).");
            if (!permis)
                throw new RegulaException($"Cursa nu poate trece din „{ValoriDb.ToDb(c.Status)}” în „{ValoriDb.ToDb(statusNou)}”.");

            c.Status = statusNou;
            await db.SaveChangesAsync();

            if (statusNou == StatusCursa.InDesfasurare)
                await ElibereazaRezervarileCurseiAsync(db, cursaId);

            await db.InregistreazaLogAuditAsync(u.UtilizatorID, $"Cursă #{cursaId}: {ValoriDb.ToDb(statusNou)}", "Curse", cursaId.ToString());
        });
    }

    /// <summary>Anuleaza cursa si ramburseaza integral toate biletele active (prin sp_AnuleazaBilet).</summary>
    public Task<RezultatAnulareCursa> AnuleazaCursaAsync(int cursaId, string motiv)
    {
        var admin = sesiune.CereAdmin();
        if (string.IsNullOrWhiteSpace(motiv))
            throw new ValidareException("Motivul anulării este obligatoriu.");

        return bd.InTranzactieAsync(async db =>
        {
            var c = await db.Curse.FirstOrDefaultAsync(x => x.CursaID == cursaId)
                    ?? throw new NegasitException("Cursa nu există.");
            if (c.Status != StatusCursa.Planificata)
                throw new RegulaException("Doar cursele planificate pot fi anulate.");

            c.Status = StatusCursa.Anulata;
            await db.SaveChangesAsync();

            var rezervari = await ElibereazaRezervarileCurseiAsync(db, cursaId);

            var bilete = await db.Bilete.Where(b => b.CursaID == cursaId && b.Status == StatusBilet.Activ)
                .Select(b => b.BiletID).ToListAsync();
            decimal total = 0;
            foreach (var biletId in bilete)
                total += (await db.AnuleazaBiletAsync(biletId, admin.UtilizatorID, rambursareIntegrala: true)).SumaRambursata;

            await db.InregistreazaLogAuditAsync(admin.UtilizatorID,
                $"Anulare cursă #{cursaId}: {motiv.Trim()} — {bilete.Count} bilete rambursate ({total} MDL)", "Curse", cursaId.ToString());

            Log.Information("Cursa {CursaID} anulată; {Bilete} bilete rambursate, {Suma} MDL", cursaId, bilete.Count, total);
            return new RezultatAnulareCursa(bilete.Count, total, rezervari);
        });
    }

    /// <summary>
    /// Harta locurilor pentru vanzare: pozitiile din JSON-ul autobuzului + statusul fiecarui loc
    /// din SQL. Rezervarile expirate (inca neeliberate de job) apar ca libere.
    /// </summary>
    public async Task<HartaLocuri> HartaLocuriAsync(int cursaId)
    {
        var u = sesiune.CereAutentificare();

        var date = await bd.CitesteAsync(async db =>
        {
            var c = await db.Curse.Where(x => x.CursaID == cursaId)
                        .Select(x => new { x.CursaID, x.AutobuzID, x.Autobuz!.CapacitateLocuri })
                        .FirstOrDefaultAsync()
                    ?? throw new NegasitException("Cursa nu există.");

            var locuri = await db.Locuri.Where(l => l.CursaID == cursaId)
                .OrderBy(l => l.NumarLoc)
                .Select(l => new { l.LocID, l.NumarLoc, l.Status })
                .ToListAsync();

            var rezervari = await db.RezervariProvizorii.Where(r => r.Loc!.CursaID == cursaId)
                .Select(r => new { r.LocID, r.UtilizatorID, Expirata = r.DataExpirare <= DateTime.UtcNow })
                .ToListAsync();

            return (c.AutobuzID, (int)c.CapacitateLocuri, locuri, rezervari.ToDictionary(r => r.LocID));
        });

        var (autobuzId, capacitate, locuriDb, rezervariDb) = date;

        AutobuzStructura structura;
        var dinFisier = true;
        try
        {
            structura = await autobuze.IncarcaStructuraAsync(autobuzId);
        }
        catch (AutogaraException ex)
        {
            Log.Warning(ex, "Structura autobuzului {AutobuzID} nu a putut fi citită; se folosește grila implicită", autobuzId);
            structura = AutobuzService.StructuraImplicita(Math.Max(capacitate, locuriDb.Count));
            dinFisier = false;
        }

        var pozitii = structura.Locuri.ToDictionary(l => l.NumarLoc);
        var randExtra = structura.Randuri;
        var coloanaExtra = 0;

        var locuri = locuriDb.Select(l =>
        {
            var status = l.Status;
            var aMea = false;
            if (rezervariDb.TryGetValue(l.LocID, out var r))
            {
                if (r.Expirata && status == StatusLoc.Rezervat)
                    status = StatusLoc.Liber;
                else
                    aMea = r.UtilizatorID == u.UtilizatorID;
            }

            // Loc fara pozitie in JSON (structura veche): se pune pe randuri suplimentare, la final.
            if (!pozitii.TryGetValue(l.NumarLoc, out var p))
            {
                if (coloanaExtra % structura.Coloane == 0) { randExtra++; coloanaExtra = 0; }
                p = new LocStructura { NumarLoc = l.NumarLoc, Rand = randExtra, Coloana = ++coloanaExtra };
            }

            return new LocHarta(l.LocID, l.NumarLoc, p.Rand, p.Coloana, status, aMea);
        }).ToList();

        return new HartaLocuri(cursaId, autobuzId, Math.Max(structura.Randuri, randExtra), structura.Coloane,
            structura.CuloarDupaColoana, locuri, dinFisier);
    }

    private static async Task<int> ElibereazaRezervarileCurseiAsync(AutogaraDbContext db, int cursaId)
    {
        var sterse = await db.RezervariProvizorii.Where(r => r.Loc!.CursaID == cursaId).ExecuteDeleteAsync();
        await db.Locuri.Where(l => l.CursaID == cursaId && l.Status == StatusLoc.Rezervat)
            .ExecuteUpdateAsync(s => s.SetProperty(l => l.Status, StatusLoc.Liber));
        return sterse;
    }

    private static List<string> ValideazaDate(CursaEditare d)
    {
        var erori = new List<string>();
        if (d.TraseuID <= 0) erori.Add("Alegeți traseul.");
        if (d.AutobuzID <= 0) erori.Add("Alegeți autobuzul.");
        if (d.SoferID <= 0) erori.Add("Alegeți șoferul.");
        if (d.Pret < 0 || d.Pret > 100_000) erori.Add("Prețul trebuie să fie între 0 și 100 000 MDL.");
        if (decimal.Round(d.Pret, 2) != d.Pret) erori.Add("Prețul poate avea cel mult două zecimale.");
        if (d.OraSosireEstimata <= d.OraPlecare)
            erori.Add("Ora de sosire trebuie să fie după ora de plecare (cursele nu pot trece peste miezul nopții).");
        if (d.DataCursa.ToDateTime(d.OraPlecare) <= OraLocala.Acum)
            erori.Add("Ora de plecare a trecut deja.");
        return erori;
    }

    private static async Task ValideazaResurseAsync(AutogaraDbContext db, CursaEditare d, int? cursaId)
    {
        var erori = new List<string>();

        var traseu = await db.Trasee.Where(t => t.TraseuID == d.TraseuID)
            .Select(t => new { t.Activ, Opriri = t.Opriri.Count }).FirstOrDefaultAsync();
        if (traseu is null || !traseu.Activ) erori.Add("Traseul nu există sau nu este activ.");
        else if (traseu.Opriri < 2) erori.Add("Traseul trebuie să aibă cel puțin două stații.");

        var autobuz = await db.Autobuze.FirstOrDefaultAsync(a => a.AutobuzID == d.AutobuzID);
        if (autobuz is null || !autobuz.Activ || autobuz.Status != StatusAutobuz.Activ)
            erori.Add("Autobuzul nu există sau nu este în circulație (service / scos din uz).");
        else if (autobuz.DataExpirareITP is { } itp && itp < d.DataCursa)
            erori.Add($"ITP-ul autobuzului {autobuz.NrInmatriculare} expiră pe {itp:dd.MM.yyyy}, înainte de data cursei.");

        if (!await db.Soferi.AnyAsync(s => s.SoferID == d.SoferID && s.Activ))
            erori.Add("Șoferul nu există sau nu este activ.");

        var suprapuneri = await db.Curse
            .Where(c => c.DataCursa == d.DataCursa && c.Status != StatusCursa.Anulata && c.CursaID != cursaId
                        && (c.AutobuzID == d.AutobuzID || c.SoferID == d.SoferID)
                        && c.OraPlecare < d.OraSosireEstimata && d.OraPlecare < c.OraSosireEstimata)
            .Select(c => new { c.CursaID, c.AutobuzID, c.SoferID, c.OraPlecare, c.OraSosireEstimata })
            .ToListAsync();
        foreach (var s in suprapuneri)
        {
            var cine = s.AutobuzID == d.AutobuzID ? "Autobuzul" : "Șoferul";
            erori.Add($"{cine} are deja cursa #{s.CursaID} în intervalul {s.OraPlecare:HH\\:mm}–{s.OraSosireEstimata:HH\\:mm}.");
        }

        ValidareException.AruncaDaca(erori);
    }
}
