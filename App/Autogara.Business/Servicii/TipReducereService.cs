using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Entitati;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

public sealed class TipReducereService(BazaDeDate bd, CacheLocalService cache, Sesiune sesiune)
{
    public Task<List<TipReducereRand>> ListeazaAsync(bool includeInactive = false)
    {
        sesiune.CereAutentificare();

        return cache.ObtineAsync(includeInactive ? "reduceri-toate" : "reduceri", () => bd.CitesteAsync(db =>
            db.TipuriReducere
                .Where(t => includeInactive || t.Activ)
                .OrderBy(t => t.Denumire)
                .Select(t => new TipReducereRand(t.TipReducereID, t.Denumire, t.ProcentReducere, t.Activ))
                .ToListAsync()));
    }

    public Task<int> AdaugaAsync(TipReducereEditare date)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(Valideaza(date));

        return bd.ScrieAsync(async db =>
        {
            var t = new TipReducere { Denumire = date.Denumire.Trim(), ProcentReducere = date.ProcentReducere, Activ = true };
            db.TipuriReducere.Add(t);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Adăugare reducere {t.Denumire} ({t.ProcentReducere}%)", "TipuriReducere", t.TipReducereID.ToString());
            return t.TipReducereID;
        });
    }

    /// <summary>Biletele deja vandute pastreaza pretul de la vanzare; noul procent conteaza doar de acum.</summary>
    public Task ActualizeazaAsync(int tipReducereId, TipReducereEditare date, bool activ)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(Valideaza(date));

        return bd.ScrieAsync(async db =>
        {
            var t = await db.TipuriReducere.FirstOrDefaultAsync(x => x.TipReducereID == tipReducereId)
                    ?? throw new NegasitException("Tipul de reducere nu există.");

            t.Denumire = date.Denumire.Trim();
            t.ProcentReducere = date.ProcentReducere;
            t.Activ = activ;
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID,
                $"Modificare reducere {t.Denumire} ({t.ProcentReducere}%, {(activ ? "activă" : "inactivă")})", "TipuriReducere", t.TipReducereID.ToString());
        });
    }

    private static List<string> Valideaza(TipReducereEditare d)
    {
        var erori = new List<string>();
        Validari.Obligatoriu(erori, d.Denumire, "Denumirea", 50);
        if (d.ProcentReducere is < 0 or > 100)
            erori.Add("Procentul de reducere trebuie să fie între 0 și 100.");
        if (decimal.Round(d.ProcentReducere, 2) != d.ProcentReducere)
            erori.Add("Procentul poate avea cel mult două zecimale.");
        return erori;
    }
}
