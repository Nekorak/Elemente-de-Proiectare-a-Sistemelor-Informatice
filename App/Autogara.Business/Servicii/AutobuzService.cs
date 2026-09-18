using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Entitati;
using Autogara.Domain.Enumerari;
using Autogara.FileServer;
using Autogara.FileServer.Modele;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

/// <summary>
/// Autobuzele: datele in SQL, asezarea locurilor in Autobuze/autobuz_{id}.json pe file server.
/// Numarul de locuri din JSON trebuie sa fie mereu egal cu Autobuze.CapacitateLocuri.
/// </summary>
public sealed class AutobuzService(BazaDeDate bd, JsonAutobuzRepository fisiere, CacheLocalService cache, Sesiune sesiune)
{
    public Task<List<AutobuzRand>> ListeazaAsync(bool includeInactive = false)
    {
        sesiune.CereAutentificare();

        return bd.CitesteAsync(db =>
            db.Autobuze
                .Where(a => includeInactive || a.Activ)
                .OrderBy(a => a.NrInmatriculare)
                .Select(a => new AutobuzRand(a.AutobuzID, a.NrInmatriculare, a.Model, a.CapacitateLocuri, a.Status,
                    a.CaleFisierJSON, a.DataExpirareITP, a.Activ))
                .ToListAsync());
    }

    /// <param name="structura">Asezarea locurilor din editor; null = grila implicita (4 pe rand, culoar la mijloc).</param>
    public Task<int> CreeazaAsync(AutobuzEditare date, int capacitate, AutobuzStructura? structura = null)
    {
        var admin = sesiune.CereAdmin();

        structura ??= StructuraImplicita(capacitate);
        var erori = Valideaza(date);
        if (capacitate is < 1 or > 999) erori.Add("Capacitatea trebuie să fie între 1 și 999 de locuri.");
        erori.AddRange(Validare.Autobuz(structura, capacitate));
        ValidareException.AruncaDaca(erori);

        return bd.InTranzactieAsync(async db =>
        {
            var a = new Autobuz
            {
                NrInmatriculare = NormalizeazaNr(date.NrInmatriculare),
                Model = date.Model.Trim(),
                CapacitateLocuri = (short)capacitate,
                Status = date.Status,
                DataExpirareITP = date.DataExpirareITP,
                Activ = date.Status != StatusAutobuz.ScosDinUz,
            };
            db.Autobuze.Add(a);
            await db.SaveChangesAsync();

            // Fisierul se scrie inainte de commit: daca file server-ul nu raspunde, autobuzul nu ramane fara structura.
            a.CaleFisierJSON = JsonAutobuzRepository.CaleImplicita(a.AutobuzID);
            structura.AutobuzId = a.AutobuzID;
            await fisiere.WriteAsync(a.CaleFisierJSON, structura, capacitate);

            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Creare autobuz {a.NrInmatriculare}", "Autobuze", a.AutobuzID.ToString());
            return a.AutobuzID;
        });
    }

    public Task ActualizeazaAsync(int autobuzId, AutobuzEditare date)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(Valideaza(date));

        return bd.ScrieAsync(async db =>
        {
            var a = await GasesteAsync(db, autobuzId);

            if (date.Status != StatusAutobuz.Activ && a.Status == StatusAutobuz.Activ && await AreCursePlanificateAsync(db, autobuzId))
                throw new RegulaException("Autobuzul are curse planificate. Alocați alt autobuz acelor curse înainte de a-l scoate din circulație.");

            a.NrInmatriculare = NormalizeazaNr(date.NrInmatriculare);
            a.Model = date.Model.Trim();
            a.DataExpirareITP = date.DataExpirareITP;
            a.Status = date.Status;
            a.Activ = date.Status != StatusAutobuz.ScosDinUz;

            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Modificare autobuz {a.NrInmatriculare}", "Autobuze", a.AutobuzID.ToString());
        });
    }

    /// <summary>Asezarea locurilor, citita de pe file server si verificata fata de capacitatea din SQL.</summary>
    public async Task<AutobuzStructura> IncarcaStructuraAsync(int autobuzId)
    {
        sesiune.CereAutentificare();

        return await cache.ObtineAsync($"autobuz-{autobuzId}", async () =>
        {
            var a = await bd.CitesteAsync(db => GasesteAsync(db, autobuzId));
            if (string.IsNullOrWhiteSpace(a.CaleFisierJSON))
                throw new NegasitException($"Autobuzul {a.NrInmatriculare} nu are încă o structură de locuri salvată.");
            return await fisiere.ReadAsync(a.CaleFisierJSON, a.CapacitateLocuri);
        });
    }

    /// <summary>
    /// Salveaza o asezare noua a locurilor (versiunea veche ajunge in Backup/Autobuze/AAAA-LL-ZZ).
    /// Daca se schimba numarul de locuri, se schimba si capacitatea — permis doar fara curse planificate.
    /// </summary>
    public Task ActualizeazaStructuraAsync(int autobuzId, AutobuzStructura structura)
    {
        var admin = sesiune.CereAdmin();
        structura.AutobuzId = autobuzId;
        ValidareException.AruncaDaca(Validare.Autobuz(structura));

        return bd.InTranzactieAsync(async db =>
        {
            var a = await GasesteAsync(db, autobuzId);
            var capacitateNoua = structura.Locuri.Count;

            if (capacitateNoua != a.CapacitateLocuri && await AreCursePlanificateAsync(db, autobuzId))
                throw new RegulaException(
                    $"Autobuzul are curse planificate cu {a.CapacitateLocuri} locuri; numărul de locuri nu se poate schimba acum.");

            a.CapacitateLocuri = (short)capacitateNoua;
            a.CaleFisierJSON ??= JsonAutobuzRepository.CaleImplicita(autobuzId);
            await db.SaveChangesAsync();

            await fisiere.WriteAsync(a.CaleFisierJSON, structura, capacitateNoua);
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID,
                $"Structură locuri autobuz {a.NrInmatriculare} ({capacitateNoua} locuri)", "Autobuze", a.AutobuzID.ToString());
        });
    }

    /// <summary>Grila implicita: randuri de cate <paramref name="coloane"/> locuri, culoar dupa coloana <paramref name="culoarDupaColoana"/>.</summary>
    public static AutobuzStructura StructuraImplicita(int capacitate, int coloane = 4, int culoarDupaColoana = 2)
    {
        if (capacitate < 1) throw new ArgumentOutOfRangeException(nameof(capacitate));
        if (coloane < 1) throw new ArgumentOutOfRangeException(nameof(coloane));

        var randuri = (capacitate + coloane - 1) / coloane;
        return new AutobuzStructura
        {
            Randuri = randuri,
            Coloane = coloane,
            CuloarDupaColoana = culoarDupaColoana < coloane ? culoarDupaColoana : 0,
            Locuri = Enumerable.Range(0, capacitate)
                .Select(i => new LocStructura { NumarLoc = i + 1, Rand = i / coloane + 1, Coloana = i % coloane + 1 })
                .ToList(),
        };
    }

    /// <summary>Numeroteaza locurile 1..n, pe randuri, de la stanga la dreapta (pentru editorul de autobuz).</summary>
    public static void Renumeroteaza(AutobuzStructura s)
    {
        var nr = 1;
        foreach (var l in s.Locuri.OrderBy(l => l.Rand).ThenBy(l => l.Coloana))
            l.NumarLoc = nr++;
        s.Locuri = s.Locuri.OrderBy(l => l.NumarLoc).ToList();
    }

    /// <summary>Adauga sau scoate locul de pe pozitia data, apoi renumeroteaza.</summary>
    public static void ComutaLoc(AutobuzStructura s, int rand, int coloana)
    {
        if (rand < 1 || rand > s.Randuri || coloana < 1 || coloana > s.Coloane)
            return;

        var existent = s.Locuri.FirstOrDefault(l => l.Rand == rand && l.Coloana == coloana);
        if (existent is null)
            s.Locuri.Add(new LocStructura { Rand = rand, Coloana = coloana });
        else
            s.Locuri.Remove(existent);

        Renumeroteaza(s);
    }

    /// <summary>
    /// Schimba grila: locurile care ies din grila noua dispar; daca grila creste, pozitiile noi
    /// se umplu cu locuri. Rezultatul e renumerotat.
    /// </summary>
    public static void Redimensioneaza(AutobuzStructura s, int randuri, int coloane, int culoarDupaColoana)
    {
        if (randuri < 1 || coloane < 1)
            throw new ValidareException("Numărul de rânduri și de coloane trebuie să fie cel puțin 1.");

        var randuriVechi = s.Randuri;
        var coloaneVechi = s.Coloane;

        s.Locuri = s.Locuri.Where(l => l.Rand <= randuri && l.Coloana <= coloane).ToList();
        for (var r = 1; r <= randuri; r++)
            for (var c = 1; c <= coloane; c++)
                if ((r > randuriVechi || c > coloaneVechi) && !s.Locuri.Any(l => l.Rand == r && l.Coloana == c))
                    s.Locuri.Add(new LocStructura { Rand = r, Coloana = c });

        s.Randuri = randuri;
        s.Coloane = coloane;
        s.CuloarDupaColoana = culoarDupaColoana is > 0 && culoarDupaColoana < coloane ? culoarDupaColoana : 0;
        Renumeroteaza(s);
    }

    private static async Task<Autobuz> GasesteAsync(AutogaraDbContext db, int autobuzId) =>
        await db.Autobuze.FirstOrDefaultAsync(a => a.AutobuzID == autobuzId)
        ?? throw new NegasitException("Autobuzul nu există.");

    private static Task<bool> AreCursePlanificateAsync(AutogaraDbContext db, int autobuzId) =>
        db.Curse.AnyAsync(c => c.AutobuzID == autobuzId && c.Status == StatusCursa.Planificata && c.DataCursa >= OraLocala.Azi);

    private static string NormalizeazaNr(string nr) => string.Join(' ', nr.Trim().ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));

    private static List<string> Valideaza(AutobuzEditare d)
    {
        var erori = new List<string>();
        Validari.Obligatoriu(erori, d.NrInmatriculare, "Numărul de înmatriculare", 15);
        Validari.Obligatoriu(erori, d.Model, "Modelul", 100);
        return erori;
    }
}
