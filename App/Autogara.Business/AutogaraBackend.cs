using Autogara.Business.Dto;
using Autogara.Business.Servicii;
using Autogara.Common;
using Autogara.Common.Configurare;
using Autogara.DataAccess;
using Autogara.FileServer;
using Serilog;

namespace Autogara.Business;

/// <summary>
/// Punctul de intrare pentru Frontend: construieste toate serviciile o singura data, la pornire.
/// <code>
/// var store = new AppSettingsStore();
/// if (!store.Exista) { /* FrmSetupWizard: AutogaraBackend.TesteazaConfigurareaAsync + store.Salveaza */ }
/// using var backend = AutogaraBackend.Creeaza(store.Citeste());
/// await backend.Auth.AutentificaAsync(user, parola);
/// var curse = await backend.Curse.CautaCurseAsync(plecare, sosire, data);
/// </code>
/// </summary>
public sealed class AutogaraBackend : IDisposable
{
    private readonly SftpFileClient _sftp;

    private AutogaraBackend(AppSettings setari)
    {
        Setari = setari;
        Sesiune = new Sesiune();

        var bd = new BazaDeDate(setari);
        _sftp = new SftpFileClient(SftpSetari.Din(setari.FileServer));
        var autobuzeJson = new JsonAutobuzRepository(_sftp);
        var hartaJson = new JsonHartaRepository(_sftp);

        Cache = new CacheLocalService();
        Audit = new AuditService(bd, Sesiune);
        Auth = new AuthService(bd, Sesiune);
        Utilizatori = new UtilizatorService(bd, Sesiune);
        Harta = new HartaService(bd, hartaJson, Cache, Sesiune);
        Statii = new StatieService(bd, Cache, Sesiune);
        Trasee = new TraseuService(bd, Sesiune);
        Autobuze = new AutobuzService(bd, autobuzeJson, Cache, Sesiune);
        Soferi = new SoferService(bd, Sesiune);
        Mentenanta = new MentenantaService(bd, Sesiune);
        Reduceri = new TipReducereService(bd, Cache, Sesiune);
        Curse = new CursaService(bd, Autobuze, Cache, Sesiune);
        Rezervari = new RezervareService(bd, setari.Aplicatie, Sesiune);
        Bilete = new BiletService(bd, Rezervari, Sesiune);
        Rapoarte = new RaportService(bd, _sftp, Sesiune);
        Dashboard = new DashboardService(bd, Sesiune);
        Actualizari = new ActualizareService(_sftp);
        Conexiune = new ConexiuneStareService(bd, _sftp, setari.Aplicatie.SecundeVerificareConexiune);

        // Sincronizarea cache-ului cand serverul revine dupa o intrerupere.
        Conexiune.ConexiuneSchimbata += async (_, stare) =>
        {
            if (stare.BazaDeDate && Sesiune.EsteAutentificat)
                await Cache.ReimprospateazaTotAsync();
        };
    }

    public AppSettings Setari { get; }
    public Sesiune Sesiune { get; }

    public AuthService Auth { get; }
    public UtilizatorService Utilizatori { get; }
    public HartaService Harta { get; }
    public StatieService Statii { get; }
    public TraseuService Trasee { get; }
    public AutobuzService Autobuze { get; }
    public SoferService Soferi { get; }
    public MentenantaService Mentenanta { get; }
    public TipReducereService Reduceri { get; }
    public CursaService Curse { get; }
    public RezervareService Rezervari { get; }
    public BiletService Bilete { get; }
    public RaportService Rapoarte { get; }
    public DashboardService Dashboard { get; }
    public AuditService Audit { get; }
    public ConexiuneStareService Conexiune { get; }
    public CacheLocalService Cache { get; }
    public ActualizareService Actualizari { get; }

    /// <summary>Configureaza log-ul si construieste serviciile. Nu se conecteaza inca la nimic.</summary>
    public static AutogaraBackend Creeaza(AppSettings setari)
    {
        var erori = setari.Valideaza();
        if (erori.Count > 0)
            throw new ValidareException(erori);

        Jurnal.Configureaza(setari.Aplicatie);
        Log.Information("Pornire Autogara pe {Statie}; SQL {Sql},{Port}; file server {Sftp}:{PortSftp}",
            setari.Aplicatie.NumeStatie, setari.Sql.Server, setari.Sql.Port, setari.FileServer.Host, setari.FileServer.Port);

        return new AutogaraBackend(setari);
    }

    /// <summary>Butonul „Testează conexiunea” din wizard: incearca SQL si SFTP cu setarile introduse.</summary>
    public static async Task<RezultatTestConfigurare> TesteazaConfigurareaAsync(AppSettings setari, CancellationToken ct = default)
    {
        var testSql = TesteazaAsync(() => new BazaDeDate(setari).TesteazaConexiuneAsync(ct), "Conectat la baza de date.");
        var testSftp = TesteazaAsync(async () =>
        {
            using var sftp = new SftpFileClient(SftpSetari.Din(setari.FileServer));
            if (!await sftp.TesteazaConexiuneAsync(ct))
                throw new NegasitException($"Conectat, dar folderul „{setari.FileServer.CaleBaza}” nu există pe file server.");
        }, "Conectat la file server.");

        var (sqlOk, sqlMesaj) = await testSql;
        var (sftpOk, sftpMesaj) = await testSftp;
        return new RezultatTestConfigurare(sqlOk, sqlMesaj, sftpOk, sftpMesaj);
    }

    private static async Task<(bool, string)> TesteazaAsync(Func<Task> test, string mesajSucces)
    {
        try
        {
            await test();
            return (true, mesajSucces);
        }
        catch (Exception ex)
        {
            return (false, ExceptionHandler.MesajPrietenos(ex));
        }
    }

    public void Dispose()
    {
        Conexiune.Dispose();
        _sftp.Dispose();
        Jurnal.Inchide();
    }
}
