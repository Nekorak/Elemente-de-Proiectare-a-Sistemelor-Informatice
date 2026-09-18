using System.Data;
using System.Data.Common;
using Autogara.Business;
using Autogara.Business.Dto;
using Autogara.Business.Servicii;
using Autogara.Common.Configurare;
using Autogara.DataAccess;
using Autogara.Domain.Enumerari;
using Autogara.FileServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit.Abstractions;

namespace Autogara.Tests;

/// <summary>
/// Verifica fara server ca interogarile LINQ din servicii se pot traduce in SQL: conexiunea si
/// executia sunt suprimate de un interceptor, care intoarce rezultate goale si retine SQL-ul generat.
/// O eroare de traducere EF ("could not be translated") pica testul; restul erorilor (ex. "nu exista",
/// "Sequence contains no elements") sunt normale pe o baza goala.
/// </summary>
public class TraducereLinqTests(ITestOutputHelper output)
{
    private sealed class FaraServer : DbConnectionInterceptor
    {
        public override InterceptionResult ConnectionOpening(DbConnection c, ConnectionEventData e, InterceptionResult r) =>
            InterceptionResult.Suppress();

        public override ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection c, ConnectionEventData e, InterceptionResult r, CancellationToken ct = default) =>
            ValueTask.FromResult(InterceptionResult.Suppress());
    }

    private sealed class RezultateGoale(List<string> sql) : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand c, CommandEventData e, InterceptionResult<DbDataReader> r, CancellationToken ct = default)
        {
            sql.Add(c.CommandText);
            return ValueTask.FromResult(InterceptionResult<DbDataReader>.SuppressWithResult(Rezultat(c.CommandText)));
        }

        // COUNT/MAX intorc mereu un rand (0, respectiv NULL); restul interogarilor — niciun rand.
        private static DbDataReader Rezultat(string sql)
        {
            var tabel = new DataTable();
            var text = sql.TrimStart();
            if (text.StartsWith("SELECT COUNT(*)", StringComparison.OrdinalIgnoreCase))
            {
                tabel.Columns.Add("c", typeof(int));
                tabel.Rows.Add(0);
            }
            else if (text.StartsWith("SELECT MAX(", StringComparison.OrdinalIgnoreCase))
            {
                tabel.Columns.Add("m", typeof(object));
                tabel.Rows.Add(DBNull.Value);
            }
            return tabel.CreateDataReader();
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand c, CommandEventData e, InterceptionResult<int> r, CancellationToken ct = default)
        {
            sql.Add(c.CommandText);
            return ValueTask.FromResult(InterceptionResult<int>.SuppressWithResult(0));
        }

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand c, CommandEventData e, InterceptionResult<object> r, CancellationToken ct = default)
        {
            sql.Add(c.CommandText);
            return ValueTask.FromResult(InterceptionResult<object>.SuppressWithResult(DBNull.Value));
        }
    }

    public static TheoryData<string> Operatii => new(Apeluri.Keys);

    private static readonly Dictionary<string, Func<Servicii, Task>> Apeluri = new()
    {
        ["Auth.Autentifica"] = s => s.Auth.AutentificaAsync("tatiana.rusu", "Test2026"),
        ["Auth.CereriResetare"] = s => s.Auth.CereriResetareAsync(),
        ["Audit.Listeaza"] = s => s.Audit.ListeazaAsync(new FiltruAudit
            { DeLa = new DateOnly(2026, 9, 1), PanaLa = new DateOnly(2026, 9, 30), Text = "Vânzare", UtilizatorID = Guid.NewGuid() }),
        ["Utilizatori.Listeaza"] = s => s.Utilizatori.ListeazaAsync(),
        ["Harta.Incarca"] = s => s.Harta.IncarcaHartaAsync(),
        ["Statii.Listeaza"] = s => s.Statii.ListeazaAsync(),
        ["Trasee.Listeaza"] = s => s.Trasee.ListeazaAsync(includeInactive: true),
        ["Trasee.Detalii"] = s => s.Trasee.DetaliiAsync(1),
        ["Autobuze.Listeaza"] = s => s.Autobuze.ListeazaAsync(),
        ["Soferi.Listeaza"] = s => s.Soferi.ListeazaAsync(),
        ["Mentenanta.Listeaza"] = s => s.Mentenanta.ListeazaAsync(1),
        ["Mentenanta.Itp"] = s => s.Mentenanta.AutobuzeCuItpApropiatAsync(),
        ["Reduceri.Listeaza"] = s => s.Reduceri.ListeazaAsync(),
        ["Curse.CautaIntreStatii"] = s => s.Curse.CautaCurseAsync(1, 4, new DateOnly(2026, 9, 20)),
        ["Curse.CautaDinStatie"] = s => s.Curse.CautaCurseAsync(1, null, new DateOnly(2026, 9, 20)),
        ["Curse.Active"] = s => s.Curse.CurseActiveAsync(),
        ["Curse.Listeaza"] = s => s.Curse.ListeazaAsync(new DateOnly(2026, 9, 1), new DateOnly(2026, 9, 30), 1),
        ["Curse.HartaLocuri"] = s => s.Curse.HartaLocuriAsync(1),
        ["Rezervari.Mele"] = s => s.Rezervari.RezervarileMeleAsync(),
        ["Bilete.CalculeazaPret"] = s => s.Bilete.CalculeazaPretAsync(1, 1),
        ["Bilete.EstimeazaAnulare"] = s => s.Bilete.EstimeazaAnulareAsync(Guid.NewGuid()),
        ["Bilete.CautaDupaCod"] = s => s.Bilete.CautaDupaCodAsync("ag260920-0a1b2c3d"),
        ["Bilete.Mele"] = s => s.Bilete.BileteleMeleAsync(),
        ["Bilete.Cursa"] = s => s.Bilete.BileteCursaAsync(1),
        ["Rapoarte.VanzariZilnice"] = s => s.Rapoarte.VanzariZilniceAsync(new DateOnly(2026, 9, 18)),
        ["Rapoarte.VanzariInterval"] = s => s.Rapoarte.VanzariPeIntervalAsync(new DateOnly(2026, 9, 1), new DateOnly(2026, 9, 30)),
        ["Rapoarte.Ocupare"] = s => s.Rapoarte.OcupareCurseAsync(new DateOnly(2026, 9, 1), new DateOnly(2026, 9, 30), 1),
        ["Rapoarte.Comparative"] = s => s.Rapoarte.RapoarteComparativeAsync(2026, 1),
        ["Dashboard.DateLive"] = s => s.Dashboard.DateLiveAsync(),
    };

    [Theory]
    [MemberData(nameof(Operatii))]
    public async Task Interogarea_SeTraduceInSql(string operatie)
    {
        var sql = new List<string>();
        var s = new Servicii(sql);

        try
        {
            await Apeluri[operatie](s);
        }
        catch (Exception ex) when (!EsteEroareDeTraducere(ex))
        {
            output.WriteLine($"(oprit la: {ex.GetType().Name}: {ex.Message})");
        }

        foreach (var comanda in sql)
            output.WriteLine(comanda + Environment.NewLine + "----");

        Assert.NotEmpty(sql);
    }

    private static bool EsteEroareDeTraducere(Exception ex) =>
        ex.ToString().Contains("could not be translated", StringComparison.OrdinalIgnoreCase)
        || ex is NotSupportedException or NotImplementedException;

    private sealed class Servicii
    {
        public Servicii(List<string> sql)
        {
            var optiuni = new DbContextOptionsBuilder<AutogaraDbContext>()
                .UseSqlServer("Server=127.0.0.1,1;Database=autogara;User Id=x;Password=y;")
                .AddInterceptors(new FaraServer(), new RezultateGoale(sql))
                .Options;

            var bd = new BazaDeDate(optiuni);
            var sesiune = Ajutor.SesiuneCu(RolTip.Admin);
            var cache = new CacheLocalService(Ajutor.FolderTemporar());
            var sftp = new SftpFileClient(SftpSetari.Din(new SetariFileServer { Host = "127.0.0.1", Port = 1, Utilizator = "x", Parola = "y", SecundeTimeout = 1 }));

            Auth = new AuthService(bd, sesiune);
            Audit = new AuditService(bd, sesiune);
            Utilizatori = new UtilizatorService(bd, sesiune);
            Harta = new HartaService(bd, new JsonHartaRepository(sftp), cache, sesiune);
            Statii = new StatieService(bd, cache, sesiune);
            Trasee = new TraseuService(bd, sesiune);
            Autobuze = new AutobuzService(bd, new JsonAutobuzRepository(sftp), cache, sesiune);
            Soferi = new SoferService(bd, sesiune);
            Mentenanta = new MentenantaService(bd, sesiune);
            Reduceri = new TipReducereService(bd, cache, sesiune);
            Curse = new CursaService(bd, Autobuze, cache, sesiune);
            Rezervari = new RezervareService(bd, new SetariAplicatie(), sesiune);
            Bilete = new BiletService(bd, Rezervari, sesiune);
            Rapoarte = new RaportService(bd, sftp, sesiune);
            Dashboard = new DashboardService(bd, sesiune);
        }

        public AuthService Auth { get; }
        public AuditService Audit { get; }
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
    }
}
