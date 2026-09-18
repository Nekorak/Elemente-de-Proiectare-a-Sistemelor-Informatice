using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

public sealed class AuditService(BazaDeDate bd, Sesiune sesiune)
{
    /// <summary>Inregistrare in LogAudit in numele utilizatorului curent (sau NULL, daca nu e nimeni autentificat).</summary>
    public Task InregistreazaAsync(string actiune, string? entitate = null, string? entitateId = null) =>
        bd.ScrieAsync(db => db.InregistreazaLogAuditAsync(sesiune.Utilizator?.UtilizatorID, actiune, entitate, entitateId));

    public Task<List<LogAuditRand>> ListeazaAsync(FiltruAudit filtru)
    {
        sesiune.CereAdmin();

        return bd.CitesteAsync(async db =>
        {
            var q = db.LogAudit.AsQueryable();

            if (filtru.DeLa is { } deLa)
            {
                var deLaUtc = OraLocala.LaUtc(deLa.ToDateTime(TimeOnly.MinValue));
                q = q.Where(l => l.DataOra >= deLaUtc);
            }
            if (filtru.PanaLa is { } panaLa)
            {
                var panaLaUtc = OraLocala.LaUtc(panaLa.AddDays(1).ToDateTime(TimeOnly.MinValue));
                q = q.Where(l => l.DataOra < panaLaUtc);
            }
            if (filtru.UtilizatorID is { } uid)
                q = q.Where(l => l.UtilizatorID == uid);
            if (!string.IsNullOrWhiteSpace(filtru.Text))
                q = q.Where(l => l.Actiune.Contains(filtru.Text) || l.EntitateID == filtru.Text);

            var randuri = await (
                from l in q
                join u in db.Utilizatori on l.UtilizatorID equals u.UtilizatorID into uj
                from u in uj.DefaultIfEmpty()
                orderby l.DataOra descending
                select new { l.LogID, l.DataOra, Utilizator = u == null ? null : u.NumeUtilizator, l.Actiune, l.Entitate, l.EntitateID })
                .Take(Math.Clamp(filtru.MaxRanduri, 1, 5000))
                .ToListAsync();

            return randuri
                .Select(r => new LogAuditRand(r.LogID, OraLocala.DinUtc(r.DataOra), r.Utilizator, r.Actiune, r.Entitate, r.EntitateID))
                .ToList();
        });
    }
}
