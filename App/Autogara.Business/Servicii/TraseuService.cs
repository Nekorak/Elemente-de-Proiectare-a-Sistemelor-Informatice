using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Entitati;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

public sealed class TraseuService(BazaDeDate bd, Sesiune sesiune)
{
    public Task<List<TraseuRand>> ListeazaAsync(bool includeInactive = false)
    {
        sesiune.CereAutentificare();

        return bd.CitesteAsync(db =>
            db.Trasee
                .Where(t => includeInactive || t.Activ)
                .OrderBy(t => t.Denumire)
                .Select(t => new TraseuRand(
                    t.TraseuID, t.Denumire, t.Activ, t.Opriri.Count,
                    t.Opriri.OrderBy(o => o.Ordine).Select(o => o.Statie!.Nod!.Nume).FirstOrDefault(),
                    t.Opriri.OrderByDescending(o => o.Ordine).Select(o => o.Statie!.Nod!.Nume).FirstOrDefault()))
                .ToListAsync());
    }

    /// <summary>Opririle in ordine, cu distanta pe harta fata de oprirea precedenta.</summary>
    public Task<TraseuDetalii> DetaliiAsync(int traseuId)
    {
        sesiune.CereAutentificare();

        return bd.CitesteAsync(async db =>
        {
            var t = await db.Trasee.FirstOrDefaultAsync(x => x.TraseuID == traseuId)
                    ?? throw new NegasitException("Traseul nu există.");

            var opriri = await db.TraseuOpriri
                .Where(o => o.TraseuID == traseuId)
                .OrderBy(o => o.Ordine)
                .Select(o => new { o.Ordine, o.StatieID, o.Statie!.NodID, Nume = o.Statie.Nod!.Nume })
                .ToListAsync();

            var harta = await HartaService.CitesteDinBazaAsync(db);
            var muchii = HartaService.Muchii(harta).ToList();

            var randuri = new List<OprireRand>();
            decimal? total = 0;
            for (var i = 0; i < opriri.Count; i++)
            {
                decimal? distanta = i == 0 ? null : Rute.CelMaiScurt(muchii, opriri[i - 1].NodID, opriri[i].NodID)?.DistantaKm;
                if (i > 0) total = distanta is null ? null : total + distanta;
                randuri.Add(new OprireRand(opriri[i].Ordine, opriri[i].StatieID, opriri[i].Nume, distanta));
            }

            return new TraseuDetalii(t.TraseuID, t.Denumire, t.Activ, randuri, opriri.Count > 1 ? total : null);
        });
    }

    public Task<int> CreeazaAsync(string denumire, IReadOnlyList<int> statiiInOrdine)
    {
        var admin = sesiune.CereAdmin();

        return bd.InTranzactieAsync(async db =>
        {
            await ValideazaAsync(db, denumire, statiiInOrdine, traseuId: null);

            var traseu = new Traseu { Denumire = denumire.Trim(), Activ = true };
            for (var i = 0; i < statiiInOrdine.Count; i++)
                traseu.Opriri.Add(new TraseuOprire { StatieID = statiiInOrdine[i], Ordine = (short)(i + 1) });

            db.Trasee.Add(traseu);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Creare traseu {traseu.Denumire}", "Trasee", traseu.TraseuID.ToString());
            return traseu.TraseuID;
        });
    }

    public Task ActualizeazaAsync(int traseuId, string denumire, IReadOnlyList<int> statiiInOrdine)
    {
        var admin = sesiune.CereAdmin();

        return bd.InTranzactieAsync(async db =>
        {
            var t = await db.Trasee.Include(x => x.Opriri).FirstOrDefaultAsync(x => x.TraseuID == traseuId)
                    ?? throw new NegasitException("Traseul nu există.");

            await ValideazaAsync(db, denumire, statiiInOrdine, traseuId);

            t.Denumire = denumire.Trim();
            db.TraseuOpriri.RemoveRange(t.Opriri);
            await db.SaveChangesAsync(); // intai stergerea, altfel UQ_TraseuOpriri_Ordine intra in conflict

            for (var i = 0; i < statiiInOrdine.Count; i++)
                db.TraseuOpriri.Add(new TraseuOprire { TraseuID = t.TraseuID, StatieID = statiiInOrdine[i], Ordine = (short)(i + 1) });

            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Modificare traseu {t.Denumire}", "Trasee", t.TraseuID.ToString());
        });
    }

    public Task SeteazaActivAsync(int traseuId, bool activ)
    {
        var admin = sesiune.CereAdmin();

        return bd.ScrieAsync(async db =>
        {
            var t = await db.Trasee.FirstOrDefaultAsync(x => x.TraseuID == traseuId)
                    ?? throw new NegasitException("Traseul nu există.");

            if (!activ && await db.Curse.AnyAsync(c => c.TraseuID == traseuId
                                                      && c.Status == Domain.Enumerari.StatusCursa.Planificata
                                                      && c.DataCursa >= OraLocala.Azi))
                throw new RegulaException("Traseul are curse planificate. Anulați-le sau mutați-le înainte de dezactivare.");

            t.Activ = activ;
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID,
                $"{(activ ? "Reactivare" : "Dezactivare")} traseu {t.Denumire}", "Trasee", t.TraseuID.ToString());
        });
    }

    private static async Task ValideazaAsync(AutogaraDbContext db, string denumire, IReadOnlyList<int> statii, int? traseuId)
    {
        var erori = new List<string>();
        Validari.Obligatoriu(erori, denumire, "Denumirea traseului", 150);

        if (statii.Count < 2)
            erori.Add("Traseul trebuie să aibă cel puțin două stații.");
        if (statii.Distinct().Count() != statii.Count)
            erori.Add("O stație nu poate apărea de două ori în același traseu.");

        if (!string.IsNullOrWhiteSpace(denumire))
        {
            var d = denumire.Trim();
            if (await db.Trasee.AnyAsync(t => t.Denumire == d && t.TraseuID != traseuId))
                erori.Add("Există deja un traseu cu această denumire.");
        }

        var gasite = await db.Statii.Where(s => statii.Contains(s.StatieID) && s.Nod!.Activ)
            .Select(s => new { s.StatieID, s.NodID, s.Nod!.Nume })
            .ToDictionaryAsync(s => s.StatieID);
        if (statii.Any(id => !gasite.ContainsKey(id)))
            erori.Add("Una dintre stații nu există sau nu mai este activă pe hartă.");

        if (erori.Count == 0)
        {
            var muchii = HartaService.Muchii(await HartaService.CitesteDinBazaAsync(db)).ToList();
            for (var i = 1; i < statii.Count; i++)
            {
                var (a, b) = (gasite[statii[i - 1]], gasite[statii[i]]);
                if (Rute.CelMaiScurt(muchii, a.NodID, b.NodID) is null)
                    erori.Add($"Pe hartă nu există drum de la „{a.Nume}” la „{b.Nume}”.");
            }
        }

        ValidareException.AruncaDaca(erori);
    }
}
