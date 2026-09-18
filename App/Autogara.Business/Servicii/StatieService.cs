using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

/// <summary>
/// Statiile sunt nodurile de tip Statie de pe harta; randul din Statii se creeaza la salvarea
/// hartii. Aici se completeaza adresa si peronul.
/// </summary>
public sealed class StatieService(BazaDeDate bd, CacheLocalService cache, Sesiune sesiune)
{
    /// <summary>Statiile active, pentru listele de selectie (cautare curse, trasee).</summary>
    public Task<List<StatieRand>> ListeazaAsync(bool includeInactive = false)
    {
        sesiune.CereAutentificare();

        return cache.ObtineAsync(includeInactive ? "statii-toate" : "statii", () => bd.CitesteAsync(db =>
            db.Statii
                .Where(s => includeInactive || s.Nod!.Activ)
                .OrderBy(s => s.Nod!.Nume)
                .Select(s => new StatieRand(s.StatieID, s.NodID, s.Nod!.Nume, s.Adresa, s.Peron, s.Nod.Activ))
                .ToListAsync()));
    }

    public Task ActualizeazaAsync(int statieId, string? adresa, string? peron)
    {
        var admin = sesiune.CereAdmin();

        var erori = new List<string>();
        Validari.Optional(erori, adresa, "Adresa", 200);
        Validari.Optional(erori, peron, "Peronul", 10);
        ValidareException.AruncaDaca(erori);

        return bd.ScrieAsync(async db =>
        {
            var s = await db.Statii.Include(x => x.Nod).FirstOrDefaultAsync(x => x.StatieID == statieId)
                    ?? throw new NegasitException("Stația nu există.");

            s.Adresa = Validari.Curata(adresa);
            s.Peron = Validari.Curata(peron);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Modificare stație {s.Nod!.Nume}", "Statii", s.StatieID.ToString());
        });
    }
}
