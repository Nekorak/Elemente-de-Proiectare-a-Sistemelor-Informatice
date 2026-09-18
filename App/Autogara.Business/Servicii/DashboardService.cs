using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

public sealed class DashboardService(BazaDeDate bd, Sesiune sesiune)
{
    /// <summary>Cifrele zilei curente; UI-ul le reciteste periodic (ex. la 30 de secunde).</summary>
    public Task<DashboardDto> DateLiveAsync(int nrUrmatoareleCurse = 10)
    {
        sesiune.CerePersonal();

        var acum = OraLocala.Acum;
        var azi = DateOnly.FromDateTime(acum);
        var oraAcum = TimeOnly.FromDateTime(acum);
        var deLaUtc = OraLocala.LaUtc(azi.ToDateTime(TimeOnly.MinValue));
        var panaLaUtc = OraLocala.LaUtc(azi.AddDays(1).ToDateTime(TimeOnly.MinValue));
        var limitaItp = azi.AddDays(30);

        return bd.CitesteAsync(async db =>
        {
            var curseAzi = await db.Curse.Where(c => c.DataCursa == azi && c.Status != StatusCursa.Anulata)
                .Select(c => new { c.Status, c.OraPlecare })
                .ToListAsync();

            var plati = await db.Plati.Where(p => p.DataPlata >= deLaUtc && p.DataPlata < panaLaUtc)
                .GroupBy(p => p.Status)
                .Select(g => new { Status = g.Key, Numar = g.Count(), Suma = g.Sum(p => p.Suma) })
                .ToListAsync();

            var vandute = plati.Where(p => p.Status == StatusPlata.Finalizata).Sum(p => p.Numar);
            var incasat = plati.Where(p => p.Status == StatusPlata.Finalizata).Sum(p => p.Suma);
            var rambursat = plati.Where(p => p.Status == StatusPlata.Rambursata).Sum(p => p.Suma);

            var anulate = await db.Bilete.CountAsync(b => b.Status != StatusBilet.Activ && b.ModificatLa >= deLaUtc && b.ModificatLa < panaLaUtc);
            var rezervari = await db.RezervariProvizorii.CountAsync(r => r.DataExpirare > DateTime.UtcNow);
            var itp = await db.Autobuze.CountAsync(a => a.Activ && a.DataExpirareITP != null && a.DataExpirareITP <= limitaItp);

            var urmatoarele = await db.CurseActive
                .Where(c => c.DataCursa > azi || (c.DataCursa == azi && c.OraPlecare >= oraAcum) || c.Status == "In desfasurare")
                .OrderBy(c => c.DataCursa).ThenBy(c => c.OraPlecare)
                .Take(nrUrmatoareleCurse)
                .ToListAsync();

            return new DashboardDto(acum,
                curseAzi.Count,
                curseAzi.Count(c => c.Status == StatusCursa.InDesfasurare),
                curseAzi.Count(c => c.Status == StatusCursa.Planificata && c.OraPlecare > oraAcum),
                vandute, anulate, incasat - rambursat, rezervari, itp, urmatoarele);
        });
    }
}
