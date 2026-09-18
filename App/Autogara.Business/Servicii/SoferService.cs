using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Entitati;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

public sealed class SoferService(BazaDeDate bd, Sesiune sesiune)
{
    public Task<List<SoferRand>> ListeazaAsync(bool includeInactive = false)
    {
        sesiune.CerePersonal();

        return bd.CitesteAsync(db =>
            db.Soferi
                .Where(s => includeInactive || s.Activ)
                .OrderBy(s => s.Nume).ThenBy(s => s.Prenume)
                .Select(s => new SoferRand(s.SoferID, s.Nume, s.Prenume, s.NrPermis, s.Telefon, s.Activ))
                .ToListAsync());
    }

    public Task<int> AdaugaAsync(SoferEditare date)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(Valideaza(date));

        return bd.ScrieAsync(async db =>
        {
            var s = new Sofer { Activ = true };
            Aplica(s, date);
            db.Soferi.Add(s);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Adăugare șofer {s.NumeComplet}", "Soferi", s.SoferID.ToString());
            return s.SoferID;
        });
    }

    public Task ActualizeazaAsync(int soferId, SoferEditare date)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(Valideaza(date));

        return bd.ScrieAsync(async db =>
        {
            var s = await db.Soferi.FirstOrDefaultAsync(x => x.SoferID == soferId)
                    ?? throw new NegasitException("Șoferul nu există.");
            Aplica(s, date);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Modificare șofer {s.NumeComplet}", "Soferi", s.SoferID.ToString());
        });
    }

    public Task SeteazaActivAsync(int soferId, bool activ)
    {
        var admin = sesiune.CereAdmin();

        return bd.ScrieAsync(async db =>
        {
            var s = await db.Soferi.FirstOrDefaultAsync(x => x.SoferID == soferId)
                    ?? throw new NegasitException("Șoferul nu există.");

            if (!activ && await db.Curse.AnyAsync(c => c.SoferID == soferId && c.Status == StatusCursa.Planificata && c.DataCursa >= OraLocala.Azi))
                throw new RegulaException("Șoferul are curse planificate. Alocați alt șofer acelor curse înainte de dezactivare.");

            s.Activ = activ;
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID,
                $"{(activ ? "Reactivare" : "Dezactivare")} șofer {s.NumeComplet}", "Soferi", s.SoferID.ToString());
        });
    }

    private static void Aplica(Sofer s, SoferEditare d)
    {
        s.Nume = d.Nume.Trim();
        s.Prenume = d.Prenume.Trim();
        s.NrPermis = d.NrPermis.Trim().ToUpperInvariant();
        s.Telefon = Validari.Curata(d.Telefon);
    }

    private static List<string> Valideaza(SoferEditare d)
    {
        var erori = new List<string>();
        Validari.Obligatoriu(erori, d.Nume, "Numele", 100);
        Validari.Obligatoriu(erori, d.Prenume, "Prenumele", 100);
        Validari.Obligatoriu(erori, d.NrPermis, "Numărul permisului", 20);
        Validari.Telefon(erori, d.Telefon);
        return erori;
    }
}
