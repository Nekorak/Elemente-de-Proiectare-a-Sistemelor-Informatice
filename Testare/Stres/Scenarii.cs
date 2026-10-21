using System.Collections.Concurrent;
using System.Diagnostics;
using Autogara.Business.Dto;
using Autogara.Business.Servicii;
using Autogara.Common;
using Autogara.Business;
using Autogara.Domain.Enumerari;
using Microsoft.Data.SqlClient;

namespace Autogara.Stres;

/// <summary>
/// Scenariile de test. Fiecare porneste toate statiile in acelasi moment (o "poarta" comuna),
/// apoi verifica direct in baza de date ca nu s-a stricat nimic: niciun loc vandut de doua ori,
/// plati egale cu preturile, nicio rezervare ramasa agatata.
/// </summary>
public static class Scenarii
{
    private static VanzareDto Vanzare(string nume, MetodaPlata metoda = MetodaPlata.Numerar) =>
        new() { NumePasager = nume, MetodaPlata = metoda };

    /// <summary>Porneste toate operatiile deodata si asteapta sa se termine.</summary>
    private static async Task Simultan<T>(IEnumerable<T> participanti, Func<T, Task> operatie)
    {
        var poarta = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var sarcini = participanti.Select(p => Task.Run(async () =>
        {
            await poarta.Task;
            await operatie(p);
        })).ToList();
        poarta.SetResult();
        await Task.WhenAll(sarcini);
    }

    private static string SqlBileteDuble(int cursa) => $"""
        SELECT COUNT(*) FROM (SELECT LocID FROM autogara.Bilete
                              WHERE CursaID = {cursa} AND Status = N'Activ'
                              GROUP BY LocID HAVING COUNT(*) > 1) AS x
        """;

    // ---------- S1: acelasi loc, mai multe statii ----------

    public static async Task AcelasiLoc(Context c, int nrStatii, int runde)
    {
        var sw = Stopwatch.StartNew();
        var (cursa, libere) = await c.AlegeCursaAsync(runde);
        var statii = c.Casieri(nrStatii).ToList();
        var timpi = new Timpi();
        int rundeCorecte = 0, refuzuri = 0, alteErori = 0;
        var mesaje = new ConcurrentBag<string>();

        foreach (var loc in libere.Take(runde))
        {
            var castigatori = new ConcurrentBag<(Statie S, RezervareDto R)>();
            var refuzRunda = 0;
            await Simultan(statii, async s =>
            {
                try
                {
                    var r = await timpi.MasoaraAsync(() => s.Rezervari.RezervaLocAsync(loc, 5));
                    castigatori.Add((s, r));
                }
                catch (RegulaException e) when (e.CodSql == 50023) { Interlocked.Increment(ref refuzRunda); }
                catch (Exception e) { Interlocked.Increment(ref alteErori); mesaje.Add(e.Message); }
            });

            var inBaza = await Context.ScalarAsync<int>(c.CsAdmin, $"SELECT COUNT(*) FROM autogara.RezervariProvizorii WHERE LocID = {loc}");
            if (castigatori.Count == 1 && inBaza == 1 && refuzRunda == nrStatii - 1) rundeCorecte++;
            refuzuri += refuzRunda;

            // Ca in aplicatie: casierul care a prins locul vinde biletul.
            if (castigatori.TryPeek(out var w))
                await w.S.Bilete.ConfirmaVanzareAsync(w.R.RezervareID, Vanzare("Test Concurență S1"));
        }

        var duble = await Context.ScalarAsync<int>(c.CsAdmin, SqlBileteDuble(cursa));
        c.Rezultat("S1", "Același loc rezervat simultan de mai multe stații", $"{nrStatii} stații × {runde} locuri, cursa {cursa}",
            $"o rezervare reușită pe loc, {nrStatii - 1} refuzuri",
            $"{rundeCorecte}/{runde} runde corecte; {refuzuri} refuzuri „locul nu mai este liber”; {alteErori} alte erori; bilete duble: {duble}; {timpi.Rezumat()}"
            + (mesaje.IsEmpty ? "" : $"; ex.: {mesaje.First()}"),
            rundeCorecte == runde && alteErori == 0 && duble == 0, sw.Elapsed.TotalSeconds);
    }

    // ---------- S2: vanzare in masa pana la epuizarea locurilor ----------

    public static async Task VanzareInMasa(Context c, int nrStatii)
    {
        var sw = Stopwatch.StartNew();
        var (cursa, libere) = await c.AlegeCursaAsync(20);
        var statii = c.Casieri(nrStatii).ToList();
        var timpi = new Timpi();
        var vandute = new ConcurrentBag<BiletEmis>();
        int conflicte = 0, blocajeReciproce = 0, alteErori = 0;
        var mesaje = new ConcurrentBag<string>();

        await Simultan(statii, async s =>
        {
            while (Volatile.Read(ref alteErori) <= 50)
            {
                // Fiecare statie isi reimprospateaza schema locurilor si alege un loc liber la intamplare.
                var lib = await Context.ListaAsync<int>(c.CsAdmin,
                    $"SELECT LocID FROM autogara.Locuri WHERE CursaID = {cursa} AND Status = N'Liber'");
                if (lib.Count == 0) break;
                var loc = lib[Random.Shared.Next(lib.Count)];
                try
                {
                    var metoda = Random.Shared.Next(2) == 0 ? MetodaPlata.Numerar : MetodaPlata.Card;
                    vandute.Add(await timpi.MasoaraAsync(() => s.Bilete.VanzareRapidaAsync(loc, Vanzare($"Pasager {s.Utilizator}", metoda))));
                }
                catch (RegulaException e) when (e.CodSql == 50023) { Interlocked.Increment(ref conflicte); }
                catch (RegulaException e) when (e.CodSql == 1205) { Interlocked.Increment(ref blocajeReciproce); }
                catch (Exception e) { Interlocked.Increment(ref alteErori); mesaje.Add(e.Message); }
            }
        });
        var secunde = sw.Elapsed.TotalSeconds;

        var bileteActive = await Context.ScalarAsync<int>(c.CsAdmin, $"SELECT COUNT(*) FROM autogara.Bilete WHERE CursaID = {cursa} AND Status = N'Activ'");
        var ocupate = await Context.ScalarAsync<int>(c.CsAdmin, $"SELECT COUNT(*) FROM autogara.Locuri WHERE CursaID = {cursa} AND Status = N'Ocupat'");
        var libereRamase = await Context.ScalarAsync<int>(c.CsAdmin, $"SELECT COUNT(*) FROM autogara.Locuri WHERE CursaID = {cursa} AND Status <> N'Ocupat'");
        var duble = await Context.ScalarAsync<int>(c.CsAdmin, SqlBileteDuble(cursa));
        var rezervari = await Context.ScalarAsync<int>(c.CsAdmin, $"""
            SELECT COUNT(*) FROM autogara.RezervariProvizorii AS r JOIN autogara.Locuri AS l ON l.LocID = r.LocID WHERE l.CursaID = {cursa}
            """);
        var platiGresite = await Context.ScalarAsync<int>(c.CsAdmin, $"""
            SELECT COUNT(*) FROM autogara.Bilete AS b
            WHERE b.CursaID = {cursa} AND b.Status = N'Activ'
              AND ISNULL((SELECT SUM(p.Suma) FROM autogara.Plati AS p WHERE p.BiletID = b.BiletID AND p.Status = N'Finalizata'), 0) <> b.Pret
            """);

        var ok = vandute.Count == libere.Count && bileteActive == ocupate && libereRamase == 0 && duble == 0
                 && rezervari == 0 && platiGresite == 0 && alteErori == 0;
        c.Rezultat("S2", "Vânzare în masă până la epuizarea locurilor", $"{nrStatii} stații, {libere.Count} locuri, cursa {cursa}",
            "toate locurile vândute o singură dată, plăți = prețuri",
            $"{vandute.Count} bilete în {secunde:0.0} s ({vandute.Count / secunde:0.0} bilete/s); {conflicte} conflicte rezolvate; "
            + $"{blocajeReciproce} blocaje reciproce (deadlock); {alteErori} alte erori; bilete active {bileteActive} = locuri ocupate {ocupate}; "
            + $"duble {duble}; rezervări rămase {rezervari}; plăți greșite {platiGresite}; {timpi.Rezumat()}"
            + (mesaje.IsEmpty ? "" : $"; ex.: {mesaje.First()}"),
            ok, secunde);
    }

    // ---------- S3: aceeasi rezervare confirmata de mai multe statii ----------

    public static async Task DublaConfirmare(Context c, int nrStatii, int runde)
    {
        var sw = Stopwatch.StartNew();
        var (cursa, libere) = await c.AlegeCursaAsync(runde);
        var statii = c.Casieri(nrStatii).ToList();
        var proprietar = statii[0];
        int rundeCorecte = 0, alteErori = 0, confirmateDeAltul = 0;
        var mesaje = new ConcurrentBag<string>();

        foreach (var loc in libere.Take(runde))
        {
            var rez = await proprietar.Rezervari.RezervaLocAsync(loc, 5);
            var reusite = new ConcurrentBag<Statie>();
            var refuz = 0;
            await Simultan(statii, async s =>
            {
                try
                {
                    await s.Bilete.ConfirmaVanzareAsync(rez.RezervareID, Vanzare("Test Concurență S3"));
                    reusite.Add(s);
                }
                catch (RegulaException e) when (e.CodSql is 50032 or 50036) { Interlocked.Increment(ref refuz); }
                catch (Exception e) { Interlocked.Increment(ref alteErori); mesaje.Add(e.Message); }
            });
            var bilete = await Context.ScalarAsync<int>(c.CsAdmin, $"SELECT COUNT(*) FROM autogara.Bilete WHERE LocID = {loc}");
            var plati = await Context.ScalarAsync<int>(c.CsAdmin,
                $"SELECT COUNT(*) FROM autogara.Plati AS p JOIN autogara.Bilete AS b ON b.BiletID = p.BiletID WHERE b.LocID = {loc}");
            if (reusite.Count == 1 && bilete == 1 && plati == 1) rundeCorecte++;
            if (reusite.TryPeek(out var w) && w != proprietar) confirmateDeAltul++;
        }

        c.Rezultat("S3", "Aceeași rezervare confirmată simultan (dublu clic, două stații)", $"{nrStatii} stații × {runde} rezervări",
            "un singur bilet și o singură plată pe rezervare",
            $"{rundeCorecte}/{runde} runde cu exact un bilet și o plată; {alteErori} alte erori; "
            + $"în {confirmateDeAltul} runde biletul a fost emis de alt casier decât cel care a rezervat locul"
            + (mesaje.IsEmpty ? "" : $"; ex.: {mesaje.First()}"),
            rundeCorecte == runde && alteErori == 0, sw.Elapsed.TotalSeconds);
    }

    // ---------- S4: acelasi bilet anulat de mai multe statii ----------

    public static async Task DublaAnulare(Context c, int nrStatii, int runde)
    {
        var sw = Stopwatch.StartNew();
        var (cursa, libere) = await c.AlegeCursaAsync(runde);
        var statii = c.Casieri(nrStatii).ToList();
        int rundeCorecte = 0, alteErori = 0;
        var mesaje = new ConcurrentBag<string>();

        foreach (var loc in libere.Take(runde))
        {
            var bilet = await statii[0].Bilete.VanzareRapidaAsync(loc, Vanzare("Test Concurență S4"));
            var reusite = new ConcurrentBag<RezultatAnulare>();
            var refuz = 0;
            await Simultan(statii, async s =>
            {
                try { reusite.Add(await s.Bilete.AnuleazaBiletAsync(bilet.BiletID)); }
                catch (RegulaException e) when (e.CodSql == 50041) { Interlocked.Increment(ref refuz); }
                catch (Exception e) { Interlocked.Increment(ref alteErori); mesaje.Add(e.Message); }
            });
            var rambursari = await Context.ScalarAsync<int>(c.CsAdmin,
                $"SELECT COUNT(*) FROM autogara.Plati WHERE BiletID = '{bilet.BiletID}' AND Status = N'Rambursata'");
            var locLiber = await Context.ScalarAsync<int>(c.CsAdmin,
                $"SELECT COUNT(*) FROM autogara.Locuri WHERE LocID = {loc} AND Status = N'Liber'");
            if (reusite.Count == 1 && refuz == nrStatii - 1 && rambursari <= 1 && locLiber == 1) rundeCorecte++;
        }

        c.Rezultat("S4", "Același bilet anulat simultan de mai multe stații", $"{nrStatii} stații × {runde} bilete",
            "o singură anulare și cel mult o rambursare, locul redevine liber",
            $"{rundeCorecte}/{runde} runde corecte; {alteErori} alte erori" + (mesaje.IsEmpty ? "" : $"; ex.: {mesaje.First()}"),
            rundeCorecte == runde && alteErori == 0, sw.Elapsed.TotalSeconds);
    }

    // ---------- S5: rezervarea expira in timp ce casierul completeaza datele ----------

    public static async Task ExpirareRezervare(Context c)
    {
        var sw = Stopwatch.StartNew();
        var (cursa, libere) = await c.AlegeCursaAsync(2);
        var a = c.Casieri(2).First();
        var b = c.Casieri(2).Last();
        var rezA = await a.Rezervari.RezervaLocAsync(libere[0], 1);
        var rezB = await a.Rezervari.RezervaLocAsync(libere[1], 1);
        Scris.Linie("S5: aștept expirarea rezervărilor de 1 minut (65 s)…");
        await Task.Delay(TimeSpan.FromSeconds(65));

        // Jobul care elibereaza rezervarile nu ruleaza pe copia de test: verificam curatarea din proceduri.
        string preluare, confirmareTarzie, expirata;
        bool ok1, ok2;
        try
        {
            await b.Rezervari.RezervaLocAsync(libere[0], 5);
            preluare = "alt casier a putut rezerva locul expirat";
            try
            {
                await a.Bilete.ConfirmaVanzareAsync(rezA.RezervareID, Vanzare("Test Expirare S5"));
                confirmareTarzie = "EROARE: confirmarea întârziată a reușit";
                ok1 = false;
            }
            catch (RegulaException e) when (e.CodSql is 50032 or 50037)
            {
                confirmareTarzie = $"confirmarea întârziată a fost refuzată ({e.CodSql})";
                ok1 = true;
            }
        }
        catch (Exception e)
        {
            preluare = $"EROARE: locul expirat nu a putut fi rezervat ({e.Message})";
            confirmareTarzie = "";
            ok1 = false;
        }

        try
        {
            await a.Bilete.ConfirmaVanzareAsync(rezB.RezervareID, Vanzare("Test Expirare S5"));
            expirata = "EROARE: rezervarea expirată a fost confirmată";
            ok2 = false;
        }
        catch (RegulaException e) when (e.CodSql == 50037)
        {
            var liber = await Context.ScalarAsync<int>(c.CsAdmin, $"SELECT COUNT(*) FROM autogara.Locuri WHERE LocID = {libere[1]} AND Status = N'Liber'");
            expirata = $"refuz „rezervarea a expirat” (50037), locul a redevenit liber: {(liber == 1 ? "da" : "nu")}";
            ok2 = liber == 1;
        }

        c.Rezultat("S5", "Rezervarea expiră înainte de confirmare", $"rezervări de 1 min, cursa {cursa}",
            "locul expirat poate fi luat de alt casier; confirmarea târzie e refuzată",
            $"{preluare}; {confirmareTarzie}; {expirata}", ok1 && ok2, sw.Elapsed.TotalSeconds);
    }

    // ---------- S6: limita de 5 incercari, atacata in paralel ----------

    public static async Task BlocareConcurenta(Context c)
    {
        var sw = Stopwatch.StartNew();
        var bd = c.Bd;

        // Referinta: 6 incercari una dupa alta.
        var numeSecv = $"stres.bs{DateTime.Now:HHmmss}";
        var parolaSecv = Context.ParolaGenerata();
        await c.CreeazaContAsync(numeSecv, "Casier", parolaSecv);
        var mesajeSecv = new List<string>();
        for (var i = 0; i < 6; i++)
        {
            try { await new AuthService(bd, new Sesiune()).AutentificaAsync(numeSecv, "gresita" + i); }
            catch (RegulaException e) { mesajeSecv.Add(e.Message.Contains("blocat") ? "blocat" : "incorectă"); }
        }

        // Atacul: 20 de incercari gresite pornite in acelasi moment.
        var nume = $"stres.bl{DateTime.Now:HHmmss}";
        var parola = Context.ParolaGenerata();
        await c.CreeazaContAsync(nume, "Casier", parola);
        int incorecte = 0, blocate = 0;
        await Simultan(Enumerable.Range(0, 20), async i =>
        {
            try { await new AuthService(bd, new Sesiune()).AutentificaAsync(nume, "gresita" + i); }
            catch (RegulaException e)
            {
                if (e.Message.Contains("blocat")) Interlocked.Increment(ref blocate);
                else Interlocked.Increment(ref incorecte);
            }
        });
        var inJurnal = await Context.ScalarAsync<int>(c.CsAdmin,
            $"SELECT COUNT(*) FROM autogara.LogAudit WHERE Actiune = N'Autentificare eșuată' AND EntitateID = N'{nume}'");

        string corecta;
        bool blocatCorect;
        try
        {
            await new AuthService(bd, new Sesiune()).AutentificaAsync(nume, parola);
            corecta = "parola corectă a fost acceptată (contul NU este blocat)";
            blocatCorect = false;
        }
        catch (RegulaException e) when (e.Message.Contains("blocat"))
        {
            corecta = "parola corectă este refuzată: contul e blocat";
            blocatCorect = true;
        }

        c.Rezultat("S6", "Limita de 5 încercări la autentificare, atacată în paralel", "20 de încercări greșite simultane",
            "cel mult 5 încercări verificate, restul blocate",
            $"secvențial: {string.Join(", ", mesajeSecv)}; paralel: {incorecte} încercări verificate, {blocate} blocate, "
            + $"{inJurnal} eșecuri în LogAudit; {corecta}",
            incorecte <= AuthService.IncercariPermise && blocatCorect && mesajeSecv.LastOrDefault() == "blocat", sw.Elapsed.TotalSeconds);
    }

    // ---------- S7: incarcare la citire (dashboard-ul tuturor statiilor) ----------

    public static async Task IncarcareCitire(Context c, int nrStatii, int apeluri)
    {
        var sw = Stopwatch.StartNew();
        var timpi = new Timpi();
        var erori = 0;
        await Simultan(c.Casieri(nrStatii), async s =>
        {
            for (var i = 0; i < apeluri; i++)
            {
                try { await timpi.MasoaraAsync(() => s.Dashboard.DateLiveAsync()); }
                catch { Interlocked.Increment(ref erori); }
            }
        });
        c.Rezultat("S7", "Încărcare: dashboard-ul cerut de toate stațiile", $"{nrStatii} stații × {apeluri} apeluri",
            "fără erori, p95 sub 2 s",
            $"{timpi.Numar} apeluri, {erori} erori; {timpi.Rezumat()}; {timpi.Numar / sw.Elapsed.TotalSeconds:0.0} apeluri/s",
            erori == 0 && timpi.Rezumat() is var r && P95(r) < 2000, sw.Elapsed.TotalSeconds);
    }

    private static double P95(string rezumat)
    {
        var i = rezumat.IndexOf("p95 ", StringComparison.Ordinal);
        if (i < 0) return double.MaxValue;
        var s = rezumat[(i + 4)..].Split(' ')[0];
        return double.TryParse(s, out var v) ? v : double.MaxValue;
    }

    // ---------- S8: date neobisnuite si incercari de injectie SQL ----------

    public static async Task DateNeobisnuite(Context c)
    {
        var sw = Stopwatch.StartNew();
        var (cursa, libere) = await c.AlegeCursaAsync(6);
        var s = c.Casieri(1).First();

        var valide = new[]
        {
            "Robert'); DROP TABLE autogara.Bilete;--",
            "<script>alert('x')</script>",
            "Ștefan Țurcanu-Ăîâ 🚌",
            new string('A', 200),
        };
        int pastrateExact = 0;
        var probleme = new List<string>();
        for (var i = 0; i < valide.Length; i++)
        {
            try
            {
                var b = await s.Bilete.VanzareRapidaAsync(libere[i], Vanzare(valide[i]));
                await using var con = new SqlConnection(c.CsAdmin);
                await con.OpenAsync();
                await using var cmd = new SqlCommand("SELECT NumePasager FROM autogara.Bilete WHERE BiletID = @id", con);
                cmd.Parameters.AddWithValue("@id", b.BiletID);
                if ((string?)await cmd.ExecuteScalarAsync() == valide[i]) pastrateExact++;
                else probleme.Add($"numele nr. {i + 1} a fost modificat");
            }
            catch (Exception e) { probleme.Add($"numele nr. {i + 1}: {e.Message}"); }
        }
        var tabelExista = await Context.ScalarAsync<int>(c.CsAdmin, "SELECT COUNT(*) FROM autogara.Bilete") > 0;
        c.Rezultat("S8", "Injecție SQL și caractere speciale în numele pasagerului", "4 nume: SQL, HTML, diacritice+emoji, 200 caractere",
            "salvate exact ca text, tabelul Bilete intact",
            $"{pastrateExact}/4 salvate exact; tabelul Bilete există: {(tabelExista ? "da" : "NU")}"
            + (probleme.Count == 0 ? "" : "; " + string.Join("; ", probleme)),
            pastrateExact == 4 && tabelExista, sw.Elapsed.TotalSeconds);

        // Valori invalide: trebuie oprite de validare, fara sa ramana locul rezervat.
        sw.Restart();
        var invalide = new (string Descriere, VanzareDto Date)[]
        {
            ("201 caractere", Vanzare(new string('B', 201))),
            ("doar spații", Vanzare("   ")),
            ("telefon „abc”", new VanzareDto { NumePasager = "Ion Test", TelefonPasager = "abc" }),
            ("bon fiscal de 51 caractere", new VanzareDto { NumePasager = "Ion Test", NumarBonFiscal = new string('9', 51) }),
        };
        var oprite = 0;
        foreach (var (descriere, date) in invalide)
        {
            try { await s.Bilete.VanzareRapidaAsync(libere[5], date); probleme.Add($"{descriere}: a trecut de validare"); }
            catch (ValidareException) { oprite++; }
            catch (Exception e) { probleme.Add($"{descriere}: {e.GetType().Name}"); }
        }
        var locLiber = await Context.ScalarAsync<int>(c.CsAdmin, $"SELECT COUNT(*) FROM autogara.Locuri WHERE LocID = {libere[5]} AND Status = N'Liber'");
        c.Rezultat("S8", "Valori la limită și invalide la vânzare", "201 caractere, spații, telefon, bon fiscal",
            "toate oprite de validare, locul rămâne liber",
            $"{oprite}/4 oprite cu mesaj de validare; locul rămas liber: {(locLiber == 1 ? "da" : "NU")}",
            oprite == 4 && locLiber == 1, sw.Elapsed.TotalSeconds);

        // Cautarea dupa cod cu text de injectie.
        sw.Restart();
        var coduri = new[] { "' OR '1'='1", "X' UNION SELECT NumeUtilizator FROM autogara.Utilizatori --", "%" };
        var goale = 0;
        foreach (var cod in coduri)
            if (await s.Bilete.CautaDupaCodAsync(cod) is null) goale++;
        c.Rezultat("S8", "Căutarea biletului după cod cu text de injecție", "3 coduri: OR 1=1, UNION, %",
            "niciun bilet găsit", $"{goale}/3 căutări fără rezultat", goale == 3, sw.Elapsed.TotalSeconds);
    }

    // ---------- S9: drepturile contului SQL al aplicatiei ----------

    public static async Task DrepturiSql(Context c)
    {
        var sw = Stopwatch.StartNew();
        var interzise = new[]
        {
            "DELETE FROM autogara.LogAudit WHERE 1 = 0",
            "UPDATE autogara.LogAudit SET Actiune = Actiune WHERE 1 = 0",
            "DROP TABLE autogara.Plati",
            "CREATE TABLE autogara.StresTest (i INT)",
            "ALTER TABLE autogara.Bilete ADD StresTest INT NULL",
            "EXEC sp_addsrvrolemember N'autogara_app', N'sysadmin'",
            "TRUNCATE TABLE autogara.Bilete",
        };
        var refuzate = 0;
        var detalii = new List<string>();
        foreach (var sql in interzise)
        {
            var nr = await IncearcaAsync(c.CsApp, sql);
            if (nr is not null) { refuzate++; detalii.Add($"{sql.Split(' ')[0]} {nr}"); }
            else detalii.Add($"{sql.Split(' ')[0]} PERMIS");
        }
        var citire = await IncearcaAsync(c.CsApp, "SELECT TOP (1) BiletID FROM autogara.Bilete") is null;
        c.Rezultat("S9", "Contul SQL al aplicației încearcă operații interzise", "DELETE/UPDATE jurnal, DROP, CREATE, ALTER, sysadmin, TRUNCATE",
            "toate refuzate de server, citirea permisă",
            $"{refuzate}/{interzise.Length} refuzate (erori: {string.Join(", ", detalii)}); citirea Bilete: {(citire ? "permisă" : "refuzată")}",
            refuzate == interzise.Length && citire, sw.Elapsed.TotalSeconds);
    }

    /// <summary>Ruleaza comanda intr-o tranzactie anulata; intoarce numarul erorii SQL sau null daca a mers.</summary>
    private static async Task<int?> IncearcaAsync(string cs, string sql)
    {
        await using var con = new SqlConnection(cs);
        await con.OpenAsync();
        await using var tr = (SqlTransaction)await con.BeginTransactionAsync();
        try
        {
            await using var cmd = new SqlCommand(sql, con, tr);
            await cmd.ExecuteNonQueryAsync();
            return null;
        }
        catch (SqlException e)
        {
            return e.Number;
        }
        finally
        {
            try { await tr.RollbackAsync(); } catch { /* tranzactia poate fi deja anulata de server */ }
        }
    }

    // ---------- S10: drepturile pe roluri, verificate in servicii ----------

    public static async Task DrepturiRoluri(Context c)
    {
        var sw = Stopwatch.StartNew();
        var (cursa, libere) = await c.AlegeCursaAsync(2);
        var casier = c.Casieri(1).First();
        var pasager = c.Pasager;
        var bilet = await casier.Bilete.VanzareRapidaAsync(libere[0], Vanzare("Test Roluri S10"));

        var verificari = new (string Descriere, Func<Task> Actiune)[]
        {
            ("pasagerul vinde la ghișeu", () => pasager.Bilete.VanzareRapidaAsync(libere[1], Vanzare("Pasager"))),
            ("pasagerul anulează biletul altcuiva", () => pasager.Bilete.AnuleazaBiletAsync(bilet.BiletID)),
            ("pasagerul deschide dashboard-ul", () => pasager.Dashboard.DateLiveAsync()),
            ("casierul cere rambursare integrală", () => casier.Bilete.AnuleazaBiletAsync(bilet.BiletID, rambursareIntegrala: true)),
        };
        var oprite = 0;
        var detalii = new List<string>();
        foreach (var (descriere, actiune) in verificari)
        {
            try { await actiune(); detalii.Add($"{descriere}: PERMIS"); }
            catch (AccesInterzisException) { oprite++; }
            catch (Exception e) { detalii.Add($"{descriere}: {e.GetType().Name}"); }
        }
        var gasit = await pasager.Bilete.CautaDupaCodAsync(bilet.CodBilet);
        if (gasit is null) oprite++;
        else detalii.Add("pasagerul vede biletul altcuiva");

        c.Rezultat("S10", "Operații interzise rolului, apelate direct în backend", "pasager și casier, 5 operații",
            "toate refuzate (acces interzis)",
            $"{oprite}/5 refuzate" + (detalii.Count == 0 ? "" : "; " + string.Join("; ", detalii)),
            oprite == 5, sw.Elapsed.TotalSeconds);
    }
}
