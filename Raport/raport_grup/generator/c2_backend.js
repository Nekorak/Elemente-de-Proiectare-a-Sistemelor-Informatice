const { P, H, bullets, code, fig, table, startChapter } = require("./lib");

module.exports = () => {
  startChapter(2);
  return [
    H(1, "2. BACKEND ȘI DEVOPS"),
    P("Compartimentul a fost realizat de **Hanganu Sergiu**. El cuprinde două părți: codul de server al aplicației, adică toată logica dintre interfață și baza de date, și infrastructura pe care rulează sistemul: serverul, serviciile, rețeaua, controlul versiunilor, compilarea, testarea și publicarea (DevOps)."),

    H(2, "2.1. Arhitectura aplicației"),
    P("Sistemul are o arhitectură client–server fără API web intermediar: aplicația desktop de pe fiecare stație se conectează direct la SQL Server (protocolul TDS, criptat) și la serverul de fișiere (SFTP). Pe server rulează instanța SQL Server EPSI și serviciul OpenSSH, iar routerul redirecționează din internet doar cele două porturi necesare. Alegerea s-a făcut pentru că există un singur tip de client, iar logica tranzacțională stă deja în procedurile stocate."),
    ...fig("Arhitectura", "Arhitectura sistemului: stația de lucru, rețeaua și serverul", { maxH: 420 }),
    P("Soluția .NET 10 (`App/Autogara.slnx`) este împărțită în 7 proiecte, cu dependențe într-un singur sens: interfața folosește doar stratul Business, Business folosește DataAccess și FileServer, iar acestea folosesc Domain și Common. Astfel, regulile aplicației nu depind de formulare și pot fi testate separat."),
    ...table(["Proiect", "Tip", "Rol"], [
      ["Autogara.Domain", "bibliotecă", "entitățile (1:1 cu tabelele) și enumerările de status; fără dependențe"],
      ["Autogara.Common", "bibliotecă", "configurarea stației (parole DPAPI), excepțiile, jurnalul Serilog, ora Moldovei"],
      ["Autogara.DataAccess", "bibliotecă", "AutogaraDbContext (EF Core), apelurile procedurilor stocate, traducerea erorilor SQL"],
      ["Autogara.FileServer", "bibliotecă", "clientul SFTP și citirea/scrierea JSON-urilor autobuzelor și ale hărții, cu validare"],
      ["Autogara.Business", "bibliotecă", "serviciile aplicației, reunite în clasa AutogaraBackend"],
      ["Autogara.WinForms", "aplicație Windows", "interfața (capitolul 3)"],
      ["Autogara.Tests", "teste xUnit", "teste unitare pentru reguli, parole, JSON, editoare și traducerea interogărilor"],
    ], [24, 18, 58], { caption: "Proiectele soluției Autogara" }),
    ...fig("BE01_solutie_proiecte", "Soluția Autogara în Visual Studio: cele 7 proiecte în Solution Explorer"),
    P("Bibliotecile externe sunt puține și declarate în fișierele .csproj: Microsoft.EntityFrameworkCore.SqlServer [[EfCore]], SSH.NET pentru SFTP [[SshNet]], Serilog pentru jurnal [[Serilog]], System.Security.Cryptography.ProtectedData pentru DPAPI și Svg pentru iconițe."),
    ...fig("BE02_referinte_proiecte", "Fișierele Autogara.Business.csproj și Autogara.DataAccess.csproj: referințele între proiecte și pachetele NuGet (EF Core în DataAccess)"),

    H(2, "2.2. Autogara.Domain: entitățile"),
    P("Fiecare tabel are o clasă cu aceleași coloane (de exemplu `Bilet`, `Cursa`, `Loc`), fără logică. Statusurile din baza de date sunt păstrate ca text (de exemplu „Planificata”, „In desfasurare”), iar în cod sunt enumerări; clasa `ValoriDb` face conversia în ambele sensuri, exact după valorile permise de constrângerile CHECK."),
    ...fig("BE03_domain_entitate", "Entitatea Bilet din Autogara.Domain, corespunzătoare tabelului autogara.Bilete"),
    ...fig("BE04_domain_valori", "Clasa ValoriDb: conversia enumerărilor în valorile din constrângerile CHECK"),

    H(2, "2.3. Autogara.Common: configurarea, jurnalul și erorile"),
    P("Fiecare stație de lucru are un fișier de configurare propriu, `%LocalAppData%\\Autogara\\appsettings.local.json`, cu adresa serverului SQL, portul, contul aplicației, datele file server-ului și setările aplicației (durata rezervării, intervalul de verificare a conexiunii). Șirul de conexiune se construiește din aceste setări, cu criptare și timeout."),
    ...fig("BE05_common_connection", "AppSettings: construirea șirului de conexiune și validarea setărilor"),
    P("Parolele din fișier nu sunt în clar: `AppSettingsStore` le criptează cu DPAPI (`ProtectedData`, domeniul utilizatorului Windows curent) și le salvează cu prefixul `dpapi:`, deci fișierul copiat pe alt calculator nu poate fi folosit [[Dpapi]]."),
    ...code(`private static string Cripteaza(string text)
{
    if (string.IsNullOrEmpty(text) || !OperatingSystem.IsWindows())
        return text;

    var criptat = ProtectedData.Protect(Encoding.UTF8.GetBytes(text), Entropie, DataProtectionScope.CurrentUser);
    return PrefixCriptat + Convert.ToBase64String(criptat);
}`),
    ...fig("BE06_common_dpapi", "AppSettingsStore: citirea, salvarea și criptarea DPAPI a parolelor"),
    P("Erorile se scriu în jurnalul local al stației (`Logs\\autogara-AAAALLZZ.log`, un fișier pe zi, păstrat 30 de zile), cu numele stației în fiecare rând. `ExceptionHandler.MesajPrietenos` transformă orice excepție într-un mesaj în română pentru utilizator, fără detalii tehnice."),
    ...fig("BE07_common_jurnal_erori", "Configurarea jurnalului Serilog și traducerea excepțiilor în mesaje prietenoase"),

    H(2, "2.4. Autogara.DataAccess: accesul la baza de date"),
    P("Citirile se fac prin Entity Framework Core: `AutogaraDbContext` mapează cele 17 tabele și 5 viziuni din schema autogara, cu numele constrângerilor și conversiile de status, iar coloanele RowVersion sunt marcate ca tokenuri de concurență [[EfCore]]."),
    ...fig("BE08_dataaccess_context", "AutogaraDbContext: maparea tabelelor Locuri și Bilete"),
    P("Clasa `BazaDeDate` deschide un context nou pentru fiecare operație, activează reîncercarea automată la erorile tranzitorii de rețea (3 reîncercări, până la 5 secunde), importantă pentru conexiunea prin internet [[EfResilience]], și rulează operațiile compuse într-o singură tranzacție (de exemplu, anularea unei curse rambursează toate biletele ei sau nu modifică nimic)."),
    ...fig("BE09_dataaccess_retry_tranzactii", "BazaDeDate: reîncercarea la erori de rețea și operațiile în tranzacție"),
    P("Operațiile critice nu sunt rescrise în C#: `ProceduriStocate` apelează procedurile din capitolul 1.6 cu parametri SQL tipizați și citește parametrii OUTPUT, ca logica tranzacțională să rămână una singură, cea din baza de date."),
    ...code(`var rezervare = Iesire("@RezervareID", SqlDbType.UniqueIdentifier);
var expirare = Iesire("@DataExpirare", SqlDbType.DateTime2);

await db.Database.ExecuteSqlRawAsync(
    "EXEC autogara.sp_RezervaLoc @LocID, @UtilizatorID, @DurataMinute, @RezervareID OUTPUT, @DataExpirare OUTPUT",
    [P("@LocID", locId), P("@UtilizatorID", utilizatorId), P("@DurataMinute", durataMinute), rezervare, expirare],
    ct).ConfigureAwait(false);`),
    ...fig("BE10_dataaccess_proceduri", "ProceduriStocate: apelul sp_RezervaLoc și sp_ConfirmaVanzareBilet cu parametri OUTPUT"),
    P("`EroriSql.Traduce` transformă erorile SQL în excepții ale aplicației: mesajele 50xxx din proceduri se afișează ca atare, încălcarea unei constrângeri UNIQUE primește un mesaj după numele constrângerii, pierderea conexiunii devine „Serverul bazei de date nu poate fi contactat”, iar un conflict de ROWVERSION anunță că datele au fost modificate între timp de alt utilizator."),
    ...fig("BE11_dataaccess_erori_sql", "EroriSql: traducerea erorilor SQL Server în mesaje pentru utilizator"),

    H(2, "2.5. Autogara.FileServer: fișierele de pe server"),
    P("Structura locurilor fiecărui autobuz și harta stațiilor sunt fișiere JSON pe serverul de fișiere, accesate prin SFTP cu biblioteca SSH.NET. `SftpFileClient` păstrează o singură conexiune, se reconectează și reîncearcă la erorile de rețea, citește fișierele direct în memorie și, la suprascriere, copiază mai întâi versiunea veche în `Backup/{folder}/{dată}/`. În cod nu există nicio cale locală de fișiere: totul trece prin acest client."),
    ...fig("BE12_fileserver_sftp", "SftpFileClient: scrierea cu backup automat și reîncercarea la erori de rețea"),
    P("Înainte de scriere și după citire, `Validare` verifică fișierele: numărul de locuri egal cu capacitatea autobuzului, locuri numerotate continuu de la 1, fiecare loc pe o poziție proprie în grilă și, pentru hartă, conexiuni doar între noduri existente."),
    ...fig("BE13_fileserver_validare", "Validarea structurii unui autobuz înainte de salvarea pe file server"),

    H(2, "2.6. Autogara.Business: serviciile aplicației"),
    P("Toate serviciile sunt construite o singură dată, la pornire, în clasa `AutogaraBackend`, pe care formularele o folosesc direct. Fiecare serviciu verifică rolul utilizatorului autentificat înainte de operație (`Sesiune.CereAdmin()`, `AreRol(...)`), deci drepturile nu depind doar de butoanele afișate în interfață."),
    ...fig("BE14_business_backend", "AutogaraBackend: construirea serviciilor și legătura dintre straturi"),
    ...table(["Serviciu", "Ce face"], [
      ["Auth, Utilizatori", "autentificarea, deconectarea, schimbarea și resetarea parolei; conturile, cu cel puțin un administrator activ"],
      ["Harta, Statii, Trasee", "nodurile și conexiunile, harta.json, cel mai scurt drum; stațiile; traseele cu opriri ordonate și distanța totală"],
      ["Autobuze, Mentenanta, Soferi, Reduceri", "autobuzele cu structura locurilor pe file server, lucrările, ITP-ul și alertele; șoferii; reducerile"],
      ["Curse", "căutarea după stații și dată, crearea cu verificarea autobuzului, șoferului și ITP-ului, statusurile, anularea cu rambursare"],
      ["Rezervari, Bilete", "rezervarea provizorie, vânzarea, vânzarea rapidă, estimarea și efectuarea anulării, căutarea după cod"],
      ["Rapoarte, Dashboard, Audit", "vânzări pe zi și pe interval, ocupare, comparativ lunar, export CSV; cifrele zilei; jurnalul"],
      ["Conexiune, Cache, Actualizari", "verificarea periodică a serverelor, modul offline, versiunea nouă a aplicației"],
    ], [30, 70], { caption: "Serviciile din AutogaraBackend" }),
    H(3, "2.6.1. Autentificarea și parolele"),
    P("La autentificare, încercările eșuate se numără în jurnalul de audit din baza de date, nu local: după 5 încercări greșite în 15 minute contul este blocat temporar pe toate stațiile. Contul dezactivat este refuzat, iar fiecare autentificare reușită sau eșuată este înregistrată."),
    ...fig("BE15_business_auth", "AuthService.AutentificaAsync: limita de încercări, verificarea parolei și jurnalul"),
    P("Parolele sunt păstrate ca hash PBKDF2-SHA256 cu sare aleatorie de 16 octeți și 210 000 de iterații, în formatul `[versiune][iterații][sare][hash]` [[Pbkdf2]] [[Owasp]]. Conturile create de scriptul de date de test au un hash SHA2_512 mai vechi; aplicația îl acceptă și îl înlocuiește automat cu formatul PBKDF2 la prima autentificare reușită. Recuperarea parolei se face prin administrator: cererea din ecranul de login ajunge în lista administratorilor, care generează o parolă temporară."),
    ...fig("BE16_business_parole", "ParolaHasher: hash-ul PBKDF2-SHA256 și verificarea parolei"),
    H(3, "2.6.2. Vânzarea, prețurile și rambursarea"),
    P("`BiletService` validează datele pasagerului (nume obligatoriu, telefon în format moldovenesc), apelează procedura de vânzare și întoarce biletul emis. Pasagerul poate vedea și anula doar biletele proprii, iar cumpărarea online se face doar cu cardul. Estimarea anulării folosește aceeași regulă ca funcția SQL, din `PretCalculator`, ca suma afișată înainte de confirmare să fie exact cea rambursată."),
    ...fig("BE17_business_vanzare", "BiletService: confirmarea vânzării, vânzarea rapidă și estimarea anulării"),
    ...fig("BE18_business_pret", "PretCalculator: prețul cu reducere și procentul de rambursare (24 h / 2 h)"),
    H(3, "2.6.3. Modul offline și starea conexiunii"),
    P("Ultimele date citite cu succes (curse active, stații, reduceri, hartă, structurile autobuzelor) se salvează în `%LocalAppData%\\Autogara\\Cache`. Dacă serverul nu răspunde, aplicația le afișează pe acestea, cu ora la care au fost salvate. `ConexiuneStareService` verifică la 30 de secunde SQL Server și file server-ul; la revenirea conexiunii, cache-ul se reîmprospătează automat. Vânzarea, rezervarea și anularea cer întotdeauna conexiunea la baza de date, ca un loc să nu poată fi vândut de două ori."),
    ...fig("BE19_business_cache", "CacheLocalService: datele de pe server, cu rezervă locală pentru modul offline"),
    ...fig("BE20_business_conexiune", "ConexiuneStareService: verificarea periodică a bazei de date și a file server-ului"),
    H(3, "2.6.4. Rapoartele și actualizările"),
    P("Rapoartele se exportă în CSV (UTF-8 cu BOM și separator „;”, deci se deschid corect în Excel) și pot fi arhivate pe file server în `Rapoarte/{an}/{lună}/`. La pornire, `ActualizareService` citește `version.json` de pe file server; dacă versiunea de acolo este mai nouă decât cea instalată, pachetul se descarcă și utilizatorul este anunțat."),
    ...fig("BE21_business_rapoarte", "RaportService: exportul CSV și arhivarea pe file server"),
    ...fig("BE22_business_actualizare", "ActualizareService: verificarea și descărcarea unei versiuni noi"),

    H(2, "2.7. Testele automate"),
    P("Proiectul Autogara.Tests conține teste xUnit [[Xunit]] pentru părțile care nu au nevoie de server: calculul prețului și al rambursării, hash-ul parolelor și conversia formatului vechi, validarea fișierelor JSON ale autobuzelor și ale hărții, editoarele vizuale, rutele pe hartă și traducerea interogărilor LINQ în SQL. Prin teoriile cu mai multe seturi de date rezultă 100 de cazuri."),
    P("Testele rulează în Test Explorer din Visual Studio în aproximativ 6,5 secunde: 99 trec și 1 eșuează. Testul eșuat, `FisiereJsonTests.HartaDinRepository_EsteValida`, așteaptă 15 noduri în harta din depozit, dar pe calculatorul de lucru `harta.json` a fost modificat local (17 noduri) și modificarea nu a fost încă salvată în Git. Nu este o eroare de cod: după commit-ul hărții noi, numărul așteptat din test trebuie actualizat la 17."),
    ...fig("BE23_teste_explorer", "Testele unitare în Test Explorer din Visual Studio: 99 trec, 1 eșuează (harta modificată local)"),

    H(2, "2.8. DevOps: serverul, rețeaua și livrarea"),
    P("Partea DevOps cuprinde tot ce face ca aplicația să ruleze de pe orice laptop al echipei: serviciile de pe server, porturile, accesul din internet, controlul versiunilor, compilarea, testarea, jurnalele și actualizarea."),
    H(3, "2.8.1. Serviciile serverului"),
    P("Pe server, instanța EPSI are două servicii cu pornire automată: motorul SQL Server (`MSSQL$EPSI`) și SQL Server Agent pentru joburi (`SQLAgent$EPSI`); alături rulează SQL Server Browser. Fișierele sunt servite de OpenSSH Server (`sshd`), tot cu pornire automată. Astfel, după o repornire a calculatorului, baza de date, joburile și fișierele sunt disponibile fără intervenție. Pe același calculator există și alte instanțe SQL Server, folosite la alte discipline; ele sunt oprite și nu au legătură cu proiectul."),
    ...fig("DO01_servicii", "SQL Server Configuration Manager: serviciile instanței EPSI (motorul și Agentul) rulează, cu pornire automată"),
    P("Interogarea de mai jos arată datele instanței: ediția Enterprise Developer pe 64 de biți, versiunea 17.0.1145.4 (SQL Server 2025, RTM), colația serverului și autentificarea mixtă (Windows și SQL Server), necesară pentru conturile SQL ale aplicației. Baza `autogara` are colația proprie `Romanian_100_CI_AS`, ca să sorteze corect diacriticele."),
    ...fig("DO07_instanta_versiune", "Instanța SQL Server EPSI: ediția, versiunea, colația, autentificarea și resursele serverului (interogare în SSMS)"),
    H(3, "2.8.2. SQL Server pe portul 14330"),
    P("Din SQL Server Configuration Manager, protocolul TCP/IP al instanței EPSI a fost activat, iar la IPAll portul dinamic a fost șters și s-a fixat portul **14330** [[MsSqlPort]]. Setările se păstrează în registrul Windows al instanței, de unde le poate citi și SQL Server prin vederea `sys.dm_server_registry`; captura arată că TCP este activ, instanța ascultă pe toate adresele, portul dinamic este gol și portul fix este 14330."),
    ...fig("DO02_retea_tcp_14330", "Configurarea rețelei instanței EPSI citită din registru: TCP/IP activ, IPAll cu portul fix 14330"),
    H(3, "2.8.3. Serverul de fișiere SFTP"),
    P("Serverul de fișiere folosește OpenSSH Server, inclus în Windows [[OpenSsh]], cu autentificare prin contul Windows al serverului. S-a ales SFTP și nu un share de rețea SMB pentru că o cale UNC folosește obligatoriu portul 445, care nu poate fi mutat și este blocat de multe rețele (colegiu, hotspot mobil); SFTP lucrează pe un singur port, criptat, prin NAT, fără VPN. Folderul servit este `File-Server` din proiect: `Autobuze` conține așezarea locurilor pentru fiecare autobuz (`autobuz_N.json`), `Harta` conține harta stațiilor (`harta.json`), iar `Backup` păstrează copiile anterioare ale acestor fișiere. Folderul `Rapoarte` se creează automat la prima arhivare a unui raport. Configurarea `sshd_config` nu este reprodusă în raport, deoarece conține date de acces."),
    ...fig("DO08_file_server_structura", "Structura folderului File-Server: rădăcina, fișierele autobuzelor și harta"),
    H(3, "2.8.4. Accesul din internet"),
    P("Pe router sunt redirecționate două porturi externe spre calculatorul-server; portul SFTP extern este 44545, nu 22, ca să nu fie găsit de scanările automate. Pe server, regulile firewall-ului Windows permit intrările pe porturile 14330 și 22."),
    ...table(["Serviciu", "Port extern", "Destinație în rețeaua locală"], [
      ["MS SQL Server (instanța EPSI)", "14330", "192.168.1.15:14330"],
      ["File server (SFTP)", "44545", "192.168.1.15:22"],
    ], [40, 20, 40], { caption: "Redirecționarea porturilor pe router" }),
    P("Verificarea se face de pe un calculator al echipei cu `Test-NetConnection` din PowerShell: ambele porturi publice răspund la adresa externă a serverului."),
    ...fig("TC32_conexiune_porturi_publice", "Porturile publice 14330 și 44545 răspund la adresa externă a serverului"),
    H(3, "2.8.5. Controlul versiunilor: Git și GitHub"),
    P("Tot proiectul (scripturile SQL, soluția .NET, fișierele File-Server, documentația și testarea) este într-un depozit Git privat pe GitHub [[Git]]. Fiecare membru a lucrat pe partea sa, cu commit-uri mici și descrise în română; istoricul arată ordinea etapelor din modelul cascadă: baza de date, backend, frontend, testare. În Visual Studio, ramura locală `main` este sincronizată cu `origin/main` (niciun commit de trimis sau de primit)."),
    ...fig("DO09_git_istoric", "Istoricul commit-urilor proiectului în fereastra Git Repository din Visual Studio"),
    H(3, "2.8.6. Compilarea, testarea și publicarea"),
    P("Soluția se compilează și se testează la fel pe orice calculator al echipei, din Visual Studio sau din linia de comandă cu .NET SDK 10 [[DotnetCli]]. Varianta de livrare se obține cu `dotnet publish` în configurația Release, pentru Windows x64."),
    ...code(`dotnet build App\\Autogara.slnx -c Release
dotnet test  App\\Autogara.slnx
dotnet publish App\\Autogara.WinForms -c Release -r win-x64 --self-contained false -o publish`),
    P("Recompilarea completă (Rebuild) a celor 7 proiecte reușește fără erori în 7,4 secunde. Compilatorul afișează avertismentele NU1902/NU1903: pachetul `System.Security.Cryptography.Pkcs` 9.0.4, adus indirect de dependențele proiectelor Business și Tests, are vulnerabilități cunoscute; remedierea este actualizarea lui la o versiune corectată, propusă ca îmbunătățire. Rezultatul testelor automate este prezentat în subcapitolul 2.7."),
    ...fig("DO11_dotnet_build", "Recompilarea soluției în Visual Studio: 7 proiecte compilate, 0 erori, avertismentele NU1902/NU1903"),
    H(3, "2.8.7. Configurarea stației de lucru și jurnalele"),
    P("La prima pornire, aplicația deschide configurarea stației, precompletată cu adresele serverului echipei; butonul „Testează conexiunea” verifică separat baza de date și file server-ul (capitolul 3.2). Pe stație, în `%LOCALAPPDATA%\\Autogara`, rămân doar fișierul de configurare `appsettings.local.json` (parolele sunt criptate DPAPI, deci fișierul nu poate fi folosit pe alt calculator), folderul `Logs` cu un jurnal Serilog pe zi și folderul `Cache`."),
    ...fig("DO14_loguri_cache", "Folderul local al aplicației (configurare, jurnale, cache) și jurnalele zilnice"),
    P("Jurnalul unei zile de lucru arată pornirea aplicației cu adresa serverului SQL și portul SFTP, autentificările, o vânzare și rambursarea ei, blocarea unui cont după 5 încercări greșite și o eroare de rețea tratată. Parolele nu ajung niciodată în jurnal."),
    ...fig("DO14c_jurnal_continut", "Conținutul unui jurnal zilnic (Serilog): pornirea, autentificările, vânzarea, rambursarea și blocarea contului"),
    ...fig("TC33_test_conexiune_reusit", "Testul de conexiune din aplicație: conectare reușită la baza de date și la file server"),
    H(3, "2.8.8. Actualizarea automată"),
    P("Mecanismul de actualizare este pregătit în cod (subcapitolul 2.6.4): pentru o versiune nouă, pachetul publicat se pune în `File-Server/Actualizari/`, iar în `version.json` se scriu versiunea și numele fișierului. La deschiderea ecranului de autentificare, fiecare stație compară versiunea și descarcă pachetul, fără ca echipa să copieze manual aplicația pe fiecare laptop. Până acum nu s-a publicat nicio actualizare, de aceea pe server nu există încă `version.json` și folderul `Actualizari` (vezi structura File-Server din subcapitolul 2.8.3); dacă fișierul lipsește, aplicația pornește normal."),
    ...code(`{ "versiune": "1.2.0", "fisier": "Actualizari/Autogara-1.2.0.zip", "note": "..." }`),

    H(2, "2.9. Concluzii la capitolul 2"),
    P("Backend-ul separă clar straturile: interfața nu conține logică, accesul la date trece prin EF Core și prin procedurile stocate, iar fișierele prin clientul SFTP. Securitatea este aplicată pe server (conturi SQL, porturi, firewall) și în aplicație (roluri verificate în servicii, parole PBKDF2, DPAPI, limita de încercări, audit). Partea DevOps a făcut posibil lucrul echipei de pe laptopuri diferite, prin internet, fără VPN, cu aceeași bază și aceleași fișiere, iar compilarea, testarea și publicarea se fac cu aceleași comenzi pe orice calculator."),
  ];
};
