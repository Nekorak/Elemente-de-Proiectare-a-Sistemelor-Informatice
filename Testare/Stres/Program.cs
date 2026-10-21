using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Autogara.Business;
using Autogara.Business.Dto;
using Autogara.Business.Securitate;
using Autogara.Business.Servicii;
using Autogara.Common;
using Autogara.Common.Configurare;
using Autogara.DataAccess;
using Autogara.Domain.Enumerari;
using Microsoft.Data.SqlClient;

namespace Autogara.Stres;

/// <summary>
/// Testele de concurenta si de incarcare. Fiecare "statie" are sesiunea ei si serviciile ei,
/// exact ca o aplicatie WinForms deschisa pe alt calculator; toate lucreaza pe aceeasi baza.
/// </summary>
public static class Program
{
    private const string BazaTest = "autogara_test";

    public static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var optiuni = Optiuni.Din(args);

        // Configurarea statiei (parolele se decripteaza DPAPI); se schimba doar baza si, optional, serverul.
        var setari = new AppSettingsStore().Citeste();
        setari.Sql.BazaDeDate = BazaTest;
        if (optiuni.Server is not null) setari.Sql.Server = optiuni.Server;
        if (optiuni.Port is not null) setari.Sql.Port = optiuni.Port.Value;

        var csAdmin = new SqlConnectionStringBuilder
        {
            DataSource = optiuni.Instanta,
            InitialCatalog = BazaTest,
            IntegratedSecurity = true,
            TrustServerCertificate = true,
            ApplicationName = "Autogara.Stres",
        }.ConnectionString;

        var ctx = new Context(setari, csAdmin);
        await ctx.VerificaBazaDeTestAsync();

        Scris.Titlu($"Teste de concurență și încărcare — baza {BazaTest} pe {setari.Sql.Server},{setari.Sql.Port}");
        Scris.Linie($"Pornit la {DateTime.Now:dd.MM.yyyy HH:mm:ss}, {Environment.ProcessorCount} procesoare logice, .NET {Environment.Version}");

        await ctx.PregatesteConturileAsync(20);

        var scenarii = new (string Id, Func<Context, Task> Rulare)[]
        {
            ("S1", c => Scenarii.AcelasiLoc(c, 5, 10)),
            ("S1", c => Scenarii.AcelasiLoc(c, 10, 10)),
            ("S1", c => Scenarii.AcelasiLoc(c, 20, 10)),
            ("S2", c => Scenarii.VanzareInMasa(c, 20)),
            ("S3", c => Scenarii.DublaConfirmare(c, 10, 10)),
            ("S4", c => Scenarii.DublaAnulare(c, 10, 10)),
            ("S5", Scenarii.ExpirareRezervare),
            ("S6", Scenarii.BlocareConcurenta),
            ("S7", c => Scenarii.IncarcareCitire(c, 20, 10)),
            ("S8", Scenarii.DateNeobisnuite),
            ("S9", Scenarii.DrepturiSql),
            ("S10", Scenarii.DrepturiRoluri),
        };

        foreach (var (id, rulare) in scenarii)
        {
            if (optiuni.Doar is { } doar && !doar.Contains(id)) continue;
            try
            {
                await rulare(ctx);
            }
            catch (Exception ex)
            {
                ctx.Rezultat(id, "Scenariul s-a oprit cu o eroare neprevăzută", "", "fără erori", $"{ex.GetType().Name}: {ex.Message}", false, 0);
            }
        }

        var cale = Path.GetFullPath("rezultate.md");
        await File.WriteAllTextAsync(cale, ctx.Markdown());
        Scris.Titlu("Rezumat");
        ctx.AfiseazaRezumat();
        Scris.Linie($"Rezultatele au fost salvate în {cale}");
        return ctx.Rezultate.All(r => r.Trecut) ? 0 : 1;
    }
}

public sealed record Optiuni(string? Server, int? Port, string Instanta, HashSet<string>? Doar)
{
    public static Optiuni Din(string[] args)
    {
        string? Val(string nume)
        {
            var i = Array.IndexOf(args, nume);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }

        var doar = Val("--doar")?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet();
        return new Optiuni(Val("--server"), int.TryParse(Val("--port"), out var p) ? p : null, Val("--instanta") ?? @".\EPSI", doar);
    }
}

public sealed record RezultatScenariu(
    string Id, string Scenariu, string Parametri, string Asteptat, string Obtinut, bool Trecut, double Secunde);

/// <summary>O statie de lucru simulata: sesiunea proprie si serviciile ei.</summary>
public sealed class Statie
{
    public Statie(string utilizator, string parola, BazaDeDate bd, SetariAplicatie setari)
    {
        Utilizator = utilizator;
        Parola = parola;
        Sesiune = new Sesiune();
        Auth = new AuthService(bd, Sesiune);
        Rezervari = new RezervareService(bd, setari, Sesiune);
        Bilete = new BiletService(bd, Rezervari, Sesiune);
        Dashboard = new DashboardService(bd, Sesiune);
    }

    public string Utilizator { get; }
    public string Parola { get; }
    public Sesiune Sesiune { get; }
    public AuthService Auth { get; }
    public RezervareService Rezervari { get; }
    public BiletService Bilete { get; }
    public DashboardService Dashboard { get; }
}

public sealed class Context(AppSettings setari, string csAdmin)
{
    private readonly HashSet<int> _curseFolosite = [];

    public AppSettings Setari { get; } = setari;
    public BazaDeDate Bd { get; } = new(setari);
    public string CsApp { get; } = setari.ConnectionString();
    public string CsAdmin { get; } = csAdmin;
    public List<Statie> Statii { get; } = [];
    public List<RezultatScenariu> Rezultate { get; } = [];

    public async Task VerificaBazaDeTestAsync()
    {
        // Siguranta: programul nu are voie sa scrie in baza reala.
        foreach (var cs in new[] { CsApp, CsAdmin })
        {
            var nume = await ScalarAsync<string>(cs, "SELECT DB_NAME()");
            if (!string.Equals(nume, "autogara_test", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Conexiunea indică baza „{nume}”, nu autogara_test. Oprit.");
        }
    }

    /// <summary>Conturile stres.casierNN (Casier) si stres.admin, stres.pasager, cu parole generate acum.</summary>
    public async Task PregatesteConturileAsync(int nrCasieri)
    {
        var sw = Stopwatch.StartNew();
        var conturi = Enumerable.Range(1, nrCasieri).Select(i => ($"stres.casier{i:00}", "Casier"))
            .Append(("stres.admin", "Admin")).Append(("stres.pasager", "Pasager")).ToList();

        var parole = new Dictionary<string, string>();
        foreach (var (nume, rol) in conturi)
        {
            var parola = ParolaGenerata();
            parole[nume] = parola;
            await CreeazaContAsync(nume, rol, parola);
        }

        foreach (var (nume, _) in conturi)
            Statii.Add(new Statie(nume, parole[nume], Bd, Setari.Aplicatie));

        // Autentificarea tuturor statiilor deodata este deja un test de concurenta.
        await Task.WhenAll(Statii.Select(s => s.Auth.AutentificaAsync(s.Utilizator, s.Parola)));
        Scris.Linie($"{Statii.Count} conturi de test pregătite și autentificate simultan în {sw.Elapsed.TotalSeconds:0.0} s");
    }

    public IEnumerable<Statie> Casieri(int n) => Statii.Where(s => s.Utilizator.StartsWith("stres.casier")).Take(n);
    public Statie Admin => Statii.First(s => s.Utilizator == "stres.admin");
    public Statie Pasager => Statii.First(s => s.Utilizator == "stres.pasager");

    public static string ParolaGenerata() => "St!" + Convert.ToHexString(RandomNumberGenerator.GetBytes(9)) + "a7";

    public async Task CreeazaContAsync(string nume, string rol, string parola)
    {
        const string sql = """
            DECLARE @RolID INT = (SELECT RolID FROM autogara.Roluri WHERE Denumire = @Rol);
            IF EXISTS (SELECT 1 FROM autogara.Utilizatori WHERE NumeUtilizator = @Nume)
                UPDATE autogara.Utilizatori SET ParolaHash = @Hash, RolID = @RolID, Activ = 1, ModificatLa = SYSUTCDATETIME()
                WHERE NumeUtilizator = @Nume;
            ELSE
                INSERT INTO autogara.Utilizatori (UtilizatorID, NumeUtilizator, ParolaHash, Nume, Prenume, Email, RolID, Activ, CreatLa)
                VALUES (NEWID(), @Nume, @Hash, N'Stres', @Nume, CONCAT(@Nume, N'@autogara.local'), @RolID, 1, SYSUTCDATETIME());
            """;
        await using var con = new SqlConnection(CsAdmin);
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Rol", rol);
        cmd.Parameters.AddWithValue("@Nume", nume);
        cmd.Parameters.AddWithValue("@Hash", ParolaHasher.Hash(parola));
        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>O cursa planificata, peste cel putin 2 zile (regula de 24 h nu intervine), cu cele mai multe locuri libere.</summary>
    public async Task<(int CursaID, List<int> Libere)> AlegeCursaAsync(int minimLibere)
    {
        var exclus = _curseFolosite.Count == 0 ? "0" : string.Join(",", _curseFolosite);
        var cursa = await ScalarAsync<int?>(CsAdmin, $"""
            SELECT TOP (1) c.CursaID
            FROM autogara.Curse AS c
            JOIN autogara.Locuri AS l ON l.CursaID = c.CursaID AND l.Status = N'Liber'
            WHERE c.Status = N'Planificata' AND c.DataCursa >= DATEADD(DAY, 2, CAST(GETDATE() AS date))
              AND c.CursaID NOT IN ({exclus})
            GROUP BY c.CursaID
            HAVING COUNT(*) >= {minimLibere}
            ORDER BY COUNT(*) DESC, c.CursaID
            """) ?? throw new InvalidOperationException($"Nu există o cursă planificată cu {minimLibere} locuri libere.");
        _curseFolosite.Add(cursa);
        var libere = await ListaAsync<int>(CsAdmin,
            $"SELECT LocID FROM autogara.Locuri WHERE CursaID = {cursa} AND Status = N'Liber' ORDER BY NumarLoc");
        return (cursa, libere);
    }

    public static async Task<T?> ScalarAsync<T>(string cs, string sql)
    {
        await using var con = new SqlConnection(cs);
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        var v = await cmd.ExecuteScalarAsync();
        if (v is null or DBNull) return default;
        var tinta = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        return (T)Convert.ChangeType(v, tinta);
    }

    public static async Task<List<T>> ListaAsync<T>(string cs, string sql)
    {
        var rez = new List<T>();
        await using var con = new SqlConnection(cs);
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            rez.Add((T)Convert.ChangeType(r.GetValue(0), typeof(T)));
        return rez;
    }

    public void Rezultat(string id, string scenariu, string parametri, string asteptat, string obtinut, bool trecut, double secunde)
    {
        var r = new RezultatScenariu(id, scenariu, parametri, asteptat, obtinut, trecut, secunde);
        Rezultate.Add(r);
        Scris.Rezultat(r);
    }

    public void AfiseazaRezumat()
    {
        foreach (var r in Rezultate)
            Scris.Rand(r);
        var trecute = Rezultate.Count(r => r.Trecut);
        Scris.Linie($"\nTOTAL: {Rezultate.Count} verificări, {trecute} trecute, {Rezultate.Count - trecute} eșuate");
    }

    public string Markdown()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# Rezultatele testelor de concurență ({DateTime.Now:dd.MM.yyyy HH:mm})").AppendLine();
        sb.AppendLine("| ID | Scenariu | Parametri | Rezultat așteptat | Rezultat obținut | Stare | Durata, s |");
        sb.AppendLine("|---|---|---|---|---|---|---|");
        foreach (var r in Rezultate)
            sb.AppendLine($"| {r.Id} | {r.Scenariu} | {r.Parametri} | {r.Asteptat} | {r.Obtinut.Replace("|", "/")} | {(r.Trecut ? "TRECUT" : "EȘUAT")} | {r.Secunde:0.0} |");
        return sb.ToString();
    }
}

/// <summary>Masurarea timpilor: mediana, percentila 95 si maximul, in milisecunde.</summary>
public sealed class Timpi
{
    private readonly ConcurrentBag<double> _ms = [];

    public async Task<T> MasoaraAsync<T>(Func<Task<T>> op)
    {
        var sw = Stopwatch.StartNew();
        try { return await op(); }
        finally { _ms.Add(sw.Elapsed.TotalMilliseconds); }
    }

    public int Numar => _ms.Count;

    public string Rezumat()
    {
        if (_ms.IsEmpty) return "—";
        var v = _ms.OrderBy(x => x).ToArray();
        double P(double p) => v[Math.Min(v.Length - 1, (int)Math.Ceiling(p * v.Length) - 1)];
        return $"mediana {P(0.5):0} ms, p95 {P(0.95):0} ms, max {v[^1]:0} ms";
    }
}

public static class Scris
{
    public static void Titlu(string t)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n=== {t} ===");
        Console.ResetColor();
    }

    public static void Linie(string t) => Console.WriteLine(t);

    public static void Rezultat(RezultatScenariu r)
    {
        Console.ForegroundColor = r.Trecut ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write(r.Trecut ? "[TRECUT] " : "[EȘUAT]  ");
        Console.ResetColor();
        Console.WriteLine($"{r.Id} {r.Scenariu} ({r.Parametri}): {r.Obtinut}  [{r.Secunde:0.0} s]");
    }

    public static void Rand(RezultatScenariu r)
    {
        Console.ForegroundColor = r.Trecut ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{(r.Trecut ? "TRECUT" : "EȘUAT "),-7}");
        Console.ResetColor();
        Console.WriteLine($" {r.Id,-4} {Taie(r.Scenariu, 44),-44} {Taie(r.Parametri, 26),-26} {Taie(r.Obtinut, 70)}");
    }

    private static string Taie(string s, int n) => s.Length <= n ? s : s[..(n - 1)] + "…";
}
