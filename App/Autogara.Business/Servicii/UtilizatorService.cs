using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Business.Securitate;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

/// <summary>Administrarea conturilor (doar Admin).</summary>
public sealed class UtilizatorService(BazaDeDate bd, Sesiune sesiune)
{
    public Task<List<UtilizatorRand>> ListeazaAsync(bool doarActivi = false)
    {
        sesiune.CereAdmin();

        return bd.CitesteAsync(async db =>
        {
            var randuri = await db.Utilizatori
                .Where(u => !doarActivi || u.Activ)
                .OrderBy(u => u.Nume).ThenBy(u => u.Prenume)
                .Select(u => new { u, Rol = u.Rol!.Denumire })
                .ToListAsync();

            return randuri.Select(r => new UtilizatorRand(
                r.u.UtilizatorID, r.u.NumeUtilizator, r.u.Nume, r.u.Prenume, r.u.Email, r.u.Telefon,
                ValoriDb.FromDb<RolTip>(r.Rol), r.u.Activ, r.u.CreatLa, r.u.RowVersion)).ToList();
        });
    }

    public Task<Guid> AdaugaAsync(UtilizatorEditare date, string parola)
    {
        var admin = sesiune.CereAdmin();

        var erori = Valideaza(date);
        erori.AddRange(ParolaHasher.ValideazaParolaNoua(parola, date.NumeUtilizator));
        ValidareException.AruncaDaca(erori);

        return bd.ScrieAsync(db => db.AdaugaUtilizatorAsync(
            date.NumeUtilizator.Trim().ToLowerInvariant(), ParolaHasher.Hash(parola), date.Nume.Trim(), date.Prenume.Trim(),
            Validari.Curata(date.Email), Validari.Curata(date.Telefon), date.Rol, admin.UtilizatorID));
    }

    /// <param name="rowVersion">Valoarea citita la incarcarea formularului (detecteaza modificari concurente).</param>
    public Task ActualizeazaAsync(Guid utilizatorId, UtilizatorEditare date, byte[] rowVersion)
    {
        var admin = sesiune.CereAdmin();
        ValidareException.AruncaDaca(Valideaza(date));

        return bd.InTranzactieAsync(async db =>
        {
            var u = await db.Utilizatori.Include(x => x.Rol).FirstOrDefaultAsync(x => x.UtilizatorID == utilizatorId)
                    ?? throw new NegasitException("Utilizatorul nu există.");

            db.Entry(u).Property(x => x.RowVersion).OriginalValue = rowVersion;

            var rolVechi = ValoriDb.FromDb<RolTip>(u.Rol!.Denumire);
            if (rolVechi == RolTip.Admin && date.Rol != RolTip.Admin)
            {
                if (utilizatorId == admin.UtilizatorID)
                    throw new RegulaException("Nu vă puteți retrage singur rolul de administrator.");
                await VerificaAltAdminActivAsync(db, utilizatorId);
            }

            var numeNou = date.NumeUtilizator.Trim().ToLowerInvariant();
            if (numeNou != u.NumeUtilizator && ParolaHasher.DepindeDeNumeleUtilizator(u.ParolaHash))
                throw new RegulaException(
                    "Contul are încă parola din seed, legată de numele vechi. Resetați parola înainte de redenumire.");

            var rolNou = ValoriDb.ToDb(date.Rol);
            u.RolID = await db.Roluri.Where(r => r.Denumire == rolNou).Select(r => r.RolID).SingleAsync();
            u.NumeUtilizator = numeNou;
            u.Nume = date.Nume.Trim();
            u.Prenume = date.Prenume.Trim();
            u.Email = Validari.Curata(date.Email);
            u.Telefon = Validari.Curata(date.Telefon);

            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Modificare utilizator {u.NumeUtilizator}", "Utilizatori", u.UtilizatorID.ToString());
        });
    }

    public Task DezactiveazaAsync(Guid utilizatorId)
    {
        var admin = sesiune.CereAdmin();
        return bd.ScrieAsync(db => db.DezactiveazaUtilizatorAsync(utilizatorId, admin.UtilizatorID));
    }

    public Task ReactiveazaAsync(Guid utilizatorId)
    {
        var admin = sesiune.CereAdmin();

        return bd.ScrieAsync(async db =>
        {
            var u = await db.Utilizatori.FirstOrDefaultAsync(x => x.UtilizatorID == utilizatorId)
                    ?? throw new NegasitException("Utilizatorul nu există.");
            if (u.Activ)
                throw new RegulaException("Utilizatorul este deja activ.");

            u.Activ = true;
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Reactivare utilizator {u.NumeUtilizator}", "Utilizatori", u.UtilizatorID.ToString());
        });
    }

    private static async Task VerificaAltAdminActivAsync(AutogaraDbContext db, Guid exceptand)
    {
        var admin = ValoriDb.ToDb(RolTip.Admin);
        var altii = await db.Utilizatori.CountAsync(u => u.Activ && u.Rol!.Denumire == admin && u.UtilizatorID != exceptand);
        if (altii == 0)
            throw new RegulaException("Trebuie să rămână cel puțin un administrator activ.");
    }

    private static List<string> Valideaza(UtilizatorEditare d)
    {
        var erori = new List<string>();
        Validari.Obligatoriu(erori, d.NumeUtilizator, "Numele de utilizator", 50);
        if (d.NumeUtilizator?.Trim() is { Length: > 0 and < 3 })
            erori.Add("Numele de utilizator trebuie să aibă cel puțin 3 caractere.");
        if (d.NumeUtilizator?.Trim().Any(char.IsWhiteSpace) == true)
            erori.Add("Numele de utilizator nu poate conține spații.");
        Validari.Obligatoriu(erori, d.Nume, "Numele", 100);
        Validari.Obligatoriu(erori, d.Prenume, "Prenumele", 100);
        Validari.Email(erori, d.Email);
        Validari.Telefon(erori, d.Telefon);
        return erori;
    }
}
