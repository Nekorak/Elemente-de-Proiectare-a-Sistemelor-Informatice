using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.Common.Configurare;
using Autogara.DataAccess;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

/// <summary>
/// Rezervarea provizorie a unui loc cat timp casierul completeaza datele pasagerului.
/// Expira singura (jobul SQL Job_ElibereazaRezervariExpirate o elibereaza).
/// </summary>
public sealed class RezervareService(BazaDeDate bd, SetariAplicatie setari, Sesiune sesiune)
{
    public Task<RezervareDto> RezervaLocAsync(int locId, int? durataMinute = null)
    {
        var u = sesiune.CereAutentificare();
        var durata = durataMinute ?? setari.DurataRezervareMinute;
        if (durata is < 1 or > 60)
            throw new ValidareException("Durata rezervării trebuie să fie între 1 și 60 de minute.");

        return bd.ScrieAsync(async db =>
        {
            var (id, expira) = await db.RezervaLocAsync(locId, u.UtilizatorID, durata);
            return new RezervareDto(id, locId, expira, durata);
        });
    }

    /// <summary>Renuntarea la rezervare (casierul inchide fereastra sau alege alt loc).</summary>
    public Task ElibereazaRezervareAsync(Guid rezervareId)
    {
        var u = sesiune.CereAutentificare();

        return bd.InTranzactieAsync(async db =>
        {
            var r = await db.RezervariProvizorii.FirstOrDefaultAsync(x => x.RezervareID == rezervareId);
            if (r is null)
                return; // a expirat si a fost deja eliberata, sau a devenit bilet

            if (r.UtilizatorID != u.UtilizatorID && u.Rol != RolTip.Admin)
                throw new AccesInterzisException("Rezervarea aparține altui utilizator.");

            db.RezervariProvizorii.Remove(r);
            await db.SaveChangesAsync();
            await db.Locuri.Where(l => l.LocID == r.LocID && l.Status == StatusLoc.Rezervat)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.Status, StatusLoc.Liber));
        });
    }

    /// <summary>Rezervarile neexpirate ale utilizatorului curent (ex. dupa repornirea aplicatiei).</summary>
    public Task<List<RezervareDto>> RezervarileMeleAsync()
    {
        var u = sesiune.CereAutentificare();

        return bd.CitesteAsync(async db =>
        {
            var randuri = await db.RezervariProvizorii
                .Where(r => r.UtilizatorID == u.UtilizatorID && r.DataExpirare > DateTime.UtcNow)
                .OrderBy(r => r.DataExpirare)
                .Select(r => new { r.RezervareID, r.LocID, r.DataCreare, r.DataExpirare })
                .ToListAsync();

            return randuri.Select(r => new RezervareDto(r.RezervareID, r.LocID,
                DateTime.SpecifyKind(r.DataExpirare, DateTimeKind.Utc),
                (int)Math.Round((r.DataExpirare - r.DataCreare).TotalMinutes))).ToList();
        });
    }

    /// <summary>Acelasi lucru ca jobul SQL, la cerere (ex. daca SQL Server Agent e oprit).</summary>
    public Task<int> ElibereazaRezervariExpirateAsync()
    {
        sesiune.CereAdmin();
        return bd.ScrieAsync(db => db.ElibereazaRezervariExpirateAsync());
    }
}
