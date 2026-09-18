using Autogara.Business.Dto;
using Autogara.Business.Securitate;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Autogara.Business.Servicii;

/// <summary>
/// Autentificarea, schimbarea si resetarea parolei. Incercarile esuate se numara in LogAudit
/// (nu local), deci limita se aplica pe toate statiile de lucru deodata.
/// </summary>
public sealed class AuthService(BazaDeDate bd, Sesiune sesiune)
{
    public const int IncercariPermise = 5;
    public const int MinuteBlocare = 15;

    private const string EntitateAutentificare = "Autentificare";
    private const string ActiuneReusita = "Autentificare reușită";
    private const string ActiuneEsuata = "Autentificare eșuată";
    private const string ActiuneCerereResetare = "Cerere resetare parolă";
    private const string ActiuneResetare = "Resetare parolă de către administrator";

    public async Task<UtilizatorAutentificat> AutentificaAsync(string numeUtilizator, string parola)
    {
        if (string.IsNullOrWhiteSpace(numeUtilizator) || string.IsNullOrEmpty(parola))
            throw new ValidareException("Introduceți numele de utilizator și parola.");

        var cheie = Cheie(numeUtilizator);

        return await bd.ScrieAsync(async db =>
        {
            if (await IncercariEsuateRecenteAsync(db, cheie) >= IncercariPermise)
                throw new RegulaException(
                    $"Prea multe încercări eșuate. Contul este blocat temporar; încercați din nou peste {MinuteBlocare} minute.");

            var u = await db.Utilizatori.Include(x => x.Rol).FirstOrDefaultAsync(x => x.NumeUtilizator == cheie);
            var (corecta, trebuieRehash) = u is null ? (false, false) : ParolaHasher.Verifica(u.ParolaHash, u.NumeUtilizator, parola);

            if (!corecta)
            {
                await db.InregistreazaLogAuditAsync(u?.UtilizatorID, ActiuneEsuata, EntitateAutentificare, cheie);
                Log.Warning("Autentificare eșuată pentru {Utilizator}", cheie);
                throw new RegulaException("Nume de utilizator sau parolă incorectă.");
            }

            if (!u!.Activ)
                throw new RegulaException("Contul este dezactivat. Contactați administratorul.");

            if (trebuieRehash)
            {
                u.ParolaHash = ParolaHasher.Hash(parola);
                await db.SaveChangesAsync();
            }

            await db.InregistreazaLogAuditAsync(u.UtilizatorID, ActiuneReusita, EntitateAutentificare, cheie);

            var autentificat = new UtilizatorAutentificat(
                u.UtilizatorID, u.NumeUtilizator, u.Nume, u.Prenume, u.Email, ValoriDb.FromDb<RolTip>(u.Rol!.Denumire));

            sesiune.Porneste(autentificat);
            Log.Information("Autentificat {Utilizator} ({Rol})", autentificat.NumeUtilizator, autentificat.Rol);
            return autentificat;
        });
    }

    public async Task DeconecteazaAsync()
    {
        if (sesiune.Utilizator is not { } u)
            return;

        try
        {
            await bd.ScrieAsync(db => db.InregistreazaLogAuditAsync(u.UtilizatorID, "Deconectare", EntitateAutentificare, u.NumeUtilizator));
        }
        catch (ConexiuneException)
        {
            // Deconectarea locala merge si fara server.
        }

        sesiune.Inchide();
    }

    public Task SchimbaParolaAsync(string parolaVeche, string parolaNoua, string confirmare)
    {
        var curent = sesiune.CereAutentificare();

        var erori = ParolaHasher.ValideazaParolaNoua(parolaNoua, curent.NumeUtilizator);
        if (parolaNoua != confirmare) erori.Add("Confirmarea nu coincide cu parola nouă.");
        if (parolaNoua == parolaVeche) erori.Add("Parola nouă trebuie să fie diferită de cea veche.");
        ValidareException.AruncaDaca(erori);

        return bd.ScrieAsync(async db =>
        {
            var u = await db.Utilizatori.FirstOrDefaultAsync(x => x.UtilizatorID == curent.UtilizatorID)
                    ?? throw new NegasitException("Utilizatorul nu mai există.");

            if (!ParolaHasher.Verifica(u.ParolaHash, u.NumeUtilizator, parolaVeche).Corecta)
                throw new RegulaException("Parola actuală este incorectă.");

            u.ParolaHash = ParolaHasher.Hash(parolaNoua);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(u.UtilizatorID, "Schimbare parolă", "Utilizatori", u.UtilizatorID.ToString());
        });
    }

    /// <summary>
    /// Recuperarea parolei din ecranul de login: cererea ajunge la administratori (lista
    /// <see cref="CereriResetareAsync"/>), care genereaza o parola temporara. Raspunsul e acelasi
    /// indiferent daca utilizatorul exista, ca sa nu se poata ghici conturi.
    /// </summary>
    public async Task<string> CerereResetareParolaAsync(string numeUtilizator, string? email)
    {
        const string raspuns = "Cererea a fost înregistrată. Un administrator vă va comunica o parolă temporară.";

        if (string.IsNullOrWhiteSpace(numeUtilizator))
            throw new ValidareException("Introduceți numele de utilizator.");

        var cheie = Cheie(numeUtilizator);

        await bd.ScrieAsync(async db =>
        {
            var u = await db.Utilizatori.AsNoTracking().FirstOrDefaultAsync(x => x.NumeUtilizator == cheie && x.Activ);
            if (u is null)
                return;
            if (!string.IsNullOrWhiteSpace(email) && !string.Equals(u.Email, email.Trim(), StringComparison.OrdinalIgnoreCase))
                return;

            var id = u.UtilizatorID.ToString();
            var dejaCerut = await db.LogAudit.AnyAsync(l =>
                l.Actiune == ActiuneCerereResetare && l.EntitateID == id && l.DataOra >= DateTime.UtcNow.AddMinutes(-10));

            if (!dejaCerut)
                await db.InregistreazaLogAuditAsync(u.UtilizatorID, ActiuneCerereResetare, "Utilizatori", id);
        });

        return raspuns;
    }

    /// <summary>Cererile de resetare din ultimele 7 zile, care nu au fost rezolvate inca.</summary>
    public Task<List<CerereResetareParola>> CereriResetareAsync()
    {
        sesiune.CereAdmin();

        return bd.CitesteAsync(async db =>
        {
            var cereri = await (
                from l in db.LogAudit
                where l.Actiune == ActiuneCerereResetare && l.DataOra >= DateTime.UtcNow.AddDays(-7)
                where !db.LogAudit.Any(r => r.Actiune == ActiuneResetare && r.EntitateID == l.EntitateID && r.DataOra > l.DataOra)
                join u in db.Utilizatori on l.UtilizatorID equals u.UtilizatorID
                orderby l.DataOra descending
                select new { u.UtilizatorID, u.NumeUtilizator, l.DataOra })
                .ToListAsync();

            return cereri
                .DistinctBy(c => c.UtilizatorID)
                .Select(c => new CerereResetareParola(c.UtilizatorID, c.NumeUtilizator, OraLocala.DinUtc(c.DataOra)))
                .ToList();
        });
    }

    /// <summary>Administratorul seteaza o parola temporara noua; o returneaza ca sa fie comunicata utilizatorului.</summary>
    public Task<string> ReseteazaParolaAsync(Guid utilizatorId)
    {
        var admin = sesiune.CereAdmin();
        var parolaTemporara = ParolaHasher.GenereazaParolaTemporara();

        return bd.ScrieAsync(async db =>
        {
            var u = await db.Utilizatori.FirstOrDefaultAsync(x => x.UtilizatorID == utilizatorId)
                    ?? throw new NegasitException("Utilizatorul nu există.");

            u.ParolaHash = ParolaHasher.Hash(parolaTemporara);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, ActiuneResetare, "Utilizatori", u.UtilizatorID.ToString());
            return parolaTemporara;
        });
    }

    private static string Cheie(string numeUtilizator) => numeUtilizator.Trim().ToLowerInvariant();

    private static async Task<int> IncercariEsuateRecenteAsync(AutogaraDbContext db, string cheie)
    {
        // DateTime.UtcNow din interogare se traduce in SYSUTCDATETIME() — conteaza ceasul serverului.
        var ultimaReusita = await db.LogAudit
            .Where(l => l.Entitate == EntitateAutentificare && l.EntitateID == cheie && l.Actiune == ActiuneReusita)
            .MaxAsync(l => (DateTime?)l.DataOra);

        return await db.LogAudit.CountAsync(l =>
            l.Entitate == EntitateAutentificare && l.EntitateID == cheie && l.Actiune == ActiuneEsuata
            && l.DataOra >= DateTime.UtcNow.AddMinutes(-MinuteBlocare)
            && (ultimaReusita == null || l.DataOra > ultimaReusita));
    }
}
