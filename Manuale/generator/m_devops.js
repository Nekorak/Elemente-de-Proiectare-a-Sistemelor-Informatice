// Manualul programatorului / DevOps (structura codului, mediul de dezvoltare, testele, livrarea)
const { P, H, bullets, numbered, code, fig, table, startChapter } = require("./lib");
const { despreImagini, conventii } = require("./comun");

module.exports = {
  titlu: "MANUALUL PROGRAMATORULUI",
  subtitlu: "(dezvoltarea, testarea și livrarea sistemului — DevOps)",
  surse: ["Sommerville"],
  continut: () => [
    H(1, "INTRODUCERE"),
    P("Manualul se adresează **programatorilor** care continuă dezvoltarea sistemului „Autogara” și persoanei care se ocupă de **DevOps**: compilarea, testarea, controlul versiunilor și livrarea versiunilor noi pe stații. El descrie structura soluției și a bazei de date, regulile fiecărui strat, pregătirea mediului de dezvoltare, testele automate și de încărcare, livrarea și actualizarea automată, extinderea sistemului și defectele cunoscute."),
    P("Sistemul are trei părți: baza de date SQL Server (schema `autogara`, cu proceduri stocate pentru operațiile critice), serverul de fișiere SFTP (fișiere JSON pentru hartă și schemele autobuzelor) și aplicația desktop .NET 10 Windows Forms, organizată pe straturi. Instalarea serverului și întreținerea lui sunt descrise în Manualul administratorului."),
    P("Codul sursă, scripturile și documentația sunt în depozitul Git al proiectului, pe GitHub. Parolele nu se scriu în cod, în scripturile urcate în depozit sau în documentație."),
    ...despreImagini(),
    ...conventii(),

    // ======================= 1 =======================
    (startChapter(1), H(1, "1. ARHITECTURA SOLUȚIEI")),
    H(2, "1.1. Straturile"),
    P("Soluția `App/Autogara.slnx` are șapte proiecte. Dependențele merg într-un singur sens, de la interfață spre date, iar interfața nu vede niciodată direct baza de date sau serverul de fișiere [[Sommerville]]."),
    ...fig("DG10_straturi", "Straturile soluției și dependențele dintre proiecte", { maxH: 520 }),
    ...table(["Proiectul", "Responsabilitatea", "Pachetele principale"], [
      ["`Autogara.WinForms`", "formularele, controalele proprii (schema autobuzului, harta, cardurile, grila), tema și iconițele; doar afișare și apeluri spre backend", "Svg 3.4.7"],
      ["`Autogara.Business`", "`AutogaraBackend` și serviciile: regulile, validările, sesiunea, securitatea parolelor, cache-ul offline, rapoartele, actualizarea", "—"],
      ["`Autogara.DataAccess`", "contextul EF Core, apelul procedurilor stocate, reîncercarea la erori de rețea, traducerea erorilor SQL", "Microsoft.EntityFrameworkCore.SqlServer 10.0.0"],
      ["`Autogara.FileServer`", "clientul SFTP, citirea și validarea fișierelor JSON, copia automată înainte de scriere", "SSH.NET 2026.0.0"],
      ["`Autogara.Domain`", "entitățile (17, câte una pe tabel) și enumerările, cu conversia în valorile din baza de date", "—"],
      ["`Autogara.Common`", "configurarea stației (DPAPI), jurnalul Serilog, excepțiile aplicației, ora locală", "Serilog 4.3.0, ProtectedData 10.0.0"],
      ["`Autogara.Tests`", "testele unitare xUnit, fără server", "xunit 2.9.3, coverlet"],
    ], [22, 52, 26], { caption: "Proiectele soluției" }),
    ...fig("BE01_solutie_proiecte", "Soluția în Solution Explorer", { maxW: 200 }),
    ...fig("BE02a_csproj_business", "Referințele proiectului Business"),
    H(2, "1.2. Regulile straturilor"),
    ...bullets([
      "**WinForms** referă doar `Autogara.Business`. Un formular nu creează `DbContext`, nu scrie SQL și nu deschide conexiuni SFTP; el apelează `Aplicatie.Backend.<Serviciu>`.",
      "**Business** decide: verifică regulile, drepturile rolului din sesiune și datele de intrare, apoi apelează DataAccess sau FileServer. Mesajele pentru utilizator se scriu aici sau în procedurile stocate, în română.",
      "**Operațiile critice** (rezervarea locului, vânzarea, anularea, conturile) se fac doar prin **proceduri stocate**, într-o tranzacție pe server; aplicația nu modifică direct tabelele `Locuri`, `Bilete` și `Plati`.",
      "**Ora** se lucrează în UTC în baza de date; ora locală a Republicii Moldova se obține cu `fn_AcumLocal` / `fn_UtcLaLocal` pe server și cu `OraLocala` în aplicație.",
      "**Fișierele JSON** de pe serverul de fișiere se validează înainte de scriere și după citire; o schemă de autobuz trebuie să aibă exact `CapacitateLocuri` locuri.",
    ]),

    // ======================= 2 =======================
    (startChapter(2), H(1, "2. MEDIUL DE DEZVOLTARE")),
    H(2, "2.1. Programele necesare"),
    ...table(["Programul", "Folosit pentru"], [
      [".NET SDK 10 și Visual Studio 2026 (sarcina „.NET desktop development”)", "compilarea, designerul Windows Forms, depanarea, Test Explorer"],
      ["SQL Server (ediția Developer) cu o instanță locală și SSMS", "baza de date de dezvoltare, scripturile, planurile de execuție"],
      ["OpenSSH Server (opțional, local)", "un server de fișiere de test; altfel se folosește cel al echipei"],
      ["Git (și contul GitHub al echipei)", "controlul versiunilor, pull request-urile"],
      ["Python 3, Node.js, LibreOffice (opțional)", "generatoarele de documentație din `Raport/` și `Manuale/`"],
    ], [50, 50], { caption: "Programele mediului de dezvoltare" }),
    H(2, "2.2. Pregătirea"),
    ...numbered([
      "Se clonează depozitul [[Git]]:",
    ]),
    ...code(`git clone https://github.com/Nekorak/Elemente-de-Proiectare-a-Sistemelor-Informatice.git
cd Elemente-de-Proiectare-a-Sistemelor-Informatice`),
    ...numbered([
      "Pe instanța locală se rulează scripturile `SQL Code/01` … `11` (fără `12_DateVolum.sql`, dacă nu se testează volumul). Parolele locale se pun într-un fișier `SQL Code/Parole.local.sql`, ignorat de Git.",
      "Se compilează soluția și se rulează testele (capitolul 7).",
      "Se pornește `Autogara.WinForms` (proiectul de start implicit). La prima pornire, configurarea stației se completează cu serverul local (de exemplu `localhost\\EPSI` sau `127.0.0.1` și portul instanței) și cu serverul de fișiere; setările ajung în `%LOCALAPPDATA%\\Autogara\\appsettings.local.json`, ignorat de Git.",
    ], { continua: true }),
    H(2, "2.3. Structura depozitului"),
    ...table(["Folderul", "Ce conține"], [
      ["`App/`", "soluția .NET (cele 7 proiecte)"],
      ["`SQL Code/`", "scripturile bazei de date `01`–`12`, în ordinea rulării"],
      ["`File-Server/`", "conținutul inițial al serverului de fișiere: `Autobuze/`, `Harta/`, structura `Backup/`"],
      ["`Testare/`", "raportul de testare, capturile, programul de stres `Testare/Stres`"],
      ["`Raport/`", "raportul de grup și generatorul lui"],
      ["`Manuale/`", "cele cinci manuale, imaginile și generatorul lor (`Manuale/generator`)"],
    ], [24, 76], { caption: "Folderele depozitului" }),

    // ======================= 3 =======================
    (startChapter(3), H(1, "3. BAZA DE DATE PENTRU PROGRAMATORI")),
    H(2, "3.1. Modelul de date"),
    P("Schema `autogara` are 17 tabele. Nucleul vânzării este lanțul **Curse → Locuri → RezervariProvizorii / Bilete → Plati**; rețeaua este **Noduri → Conexiuni → Statii → Trasee → TraseuOpriri**; flota este **Autobuze → MentenantaAutobuze**, cu **Soferi**; conturile sunt **Roluri → Utilizatori**, iar **LogAudit** păstrează istoricul."),
    ...fig("ER_diagrama", "Diagrama relațiilor dintre cele 17 tabele", { maxH: 700 }),
    H(2, "3.2. Procedurile stocate"),
    ...table(["Procedura", "Ce face", "Apelată din"], [
      ["`sp_RezervaLoc`", "blochează rândul locului (`UPDLOCK, HOLDLOCK`), verifică cursa și ora, creează rezervarea provizorie (1–60 min)", "`RezervareService`"],
      ["`sp_ConfirmaVanzareBilet`", "verifică rezervarea și utilizatorul, calculează prețul cu reducere, generează codul biletului, scrie `Bilete`, `Plati`, `LogAudit`", "`BiletService`"],
      ["`sp_AnuleazaBilet`", "aplică regula de rambursare (100 / 50 / 0 %), eliberează locul, scrie plata de rambursare", "`BiletService`"],
      ["`sp_ElibereazaRezervariExpirate`", "eliberează rezervările expirate", "jobul de la fiecare minut"],
      ["`sp_CreeazaLocuriPentruCursa`", "creează locurile 1…N ale unei curse noi", "`CursaService`"],
      ["`sp_RaportVanzariZilnic`, `sp_RaportOcupareCurse`", "rapoartele pe o zi și de ocupare", "`RaportService`"],
      ["`sp_AdaugaUtilizator`, `sp_DezactiveazaUtilizator`", "conturile, cu regula ultimului administrator activ", "`UtilizatorService`"],
      ["`sp_InregistreazaLogAudit`", "o înregistrare în jurnalul de audit", "toate serviciile"],
    ], [30, 50, 20], { caption: "Procedurile stocate" }),
    ...fig("BD51_sp_RezervaLoc_ConfirmaVanzare", "Procedurile de rezervare și de vânzare"),
    H(2, "3.3. Codurile de eroare"),
    P("Procedurile semnalează încălcarea unei reguli cu `THROW 500xx`, cu mesajul gata pentru utilizator, în română. DataAccess transformă orice eroare din intervalul 50000–50999 în `RegulaException`, iar mesajul ajunge neschimbat pe ecran. Un cod nou se alege în grupa operației."),
    ...table(["Grupa", "Operația", "Exemple"], [
      ["50010–50024", "cursa și rezervarea locului", "50023 „Locul nu mai este liber (a fost rezervat sau vândut între timp).”"],
      ["50030–50037", "confirmarea vânzării", "50034 utilizator dezactivat; 50037 rezervare expirată"],
      ["50040–50042", "anularea biletului", "50041 bilet deja anulat; 50042 cursa a plecat"],
      ["50050", "rapoartele", "interval de date invalid"],
      ["50060–50066", "conturile", "50062 nume de utilizator folosit; 50066 ultimul administrator activ"],
    ], [18, 30, 52], { caption: "Grupele codurilor de eroare din procedurile stocate" }),
    H(2, "3.4. Modificarea structurii"),
    ...bullets([
      "Scripturile sunt **idempotente**: `CREATE OR ALTER` pentru vederi, funcții și proceduri, `IF NOT EXISTS` / `IF COL_LENGTH(...) IS NULL` pentru tabele și coloane, ca să se poată rula din nou pe o bază existentă.",
      "O coloană nouă se adaugă în `03_Tables.sql` (pentru instalările noi) și într-un script de migrare numerotat pentru bazele existente; apoi în entitatea din `Autogara.Domain` și în maparea din `AutogaraDbContext`.",
      "Drepturile noi se dau doar rolurilor care au nevoie de ele, în `08_Security.sql` [[MsSqlSecurity]].",
      "Valorile de stare (`Planificata`, `Activ`, `Rezervat` …) sunt fixate prin constrângeri CHECK și au corespondent în `ValoriDb`; o valoare nouă se adaugă în ambele locuri.",
    ]),

    // ======================= 4 =======================
    (startChapter(4), H(1, "4. BACKEND-UL")),
    H(2, "4.1. AutogaraBackend și serviciile"),
    P("`AutogaraBackend` construiește o singură dată, la pornire, toate serviciile și le dă aceleași obiecte comune: `BazaDeDate`, `SftpFileClient`, setările și `Sesiune` (utilizatorul autentificat și rolul lui). Interfața folosește doar proprietățile backend-ului."),
    ...fig("BE14_business_backend", "AutogaraBackend: construirea serviciilor"),
    ...table(["Serviciul", "Ce acoperă"], [
      ["`Auth`, `Utilizatori`", "autentificarea cu limita de încercări, schimbarea parolei, cererile de resetare; conturile"],
      ["`Curse`, `Rezervari`, `Bilete`", "planificarea curselor, rezervarea locului, vânzarea, anularea, estimarea rambursării"],
      ["`Harta`, `Statii`, `Trasee`", "harta din JSON, stațiile, traseele cu distanțele calculate pe hartă"],
      ["`Autobuze`, `Mentenanta`, `Soferi`, `Reduceri`", "flota, schemele de locuri, lucrările și ITP-ul, șoferii, tipurile de reducere"],
      ["`Rapoarte`, `Dashboard`, `Audit`", "rapoartele și exportul CSV, cifrele zilei, jurnalul de audit"],
      ["`Conexiune`, `Cache`, `Actualizari`", "starea conexiunii (la 30 s), copia locală pentru modul offline, versiunile noi"],
    ], [34, 66], { caption: "Serviciile backend-ului" }),
    H(2, "4.2. Accesul la date"),
    P("`BazaDeDate` creează un `AutogaraDbContext` pentru fiecare operație și folosește strategia de reîncercare EF Core (3 reîncercări, cel mult 5 secunde între ele) pentru erorile trecătoare de rețea [[EfCore]] [[EfResilience]]. Citirile trec prin `CitesteAsync`, scrierile prin `ScrieAsync`, iar operațiile cu mai mulți pași se fac în `InTranzactieAsync`, în interiorul strategiei de reîncercare. Procedurile stocate se apelează din `ProceduriStocate`, cu parametri OUTPUT tipizați."),
    ...fig("BE09a_bazadedate", "BazaDeDate: reîncercarea și tranzacțiile"),
    ...fig("BE10_dataaccess_proceduri", "Apelul procedurilor de rezervare și vânzare"),
    H(2, "4.3. Erorile"),
    P("Toate erorile ajung la interfață ca excepții ale aplicației, derivate din `AutogaraException`, cu mesajul pentru utilizator. `Mesaje.Eroare` le afișează, iar orice altă excepție se scrie în jurnal și se afișează cu un mesaj general."),
    ...table(["Excepția", "Când apare", "Exemple de surse"], [
      ["`ValidareException`", "datele introduse sunt greșite; conține lista erorilor", "validările din servicii"],
      ["`RegulaException`", "o regulă de business nu permite operația", "`THROW 500xx`, unicitatea (2627/2601), cheile externe (547), deadlock (1205)"],
      ["`ConexiuneException`", "serverul nu poate fi contactat sau autentificarea contului aplicației eșuează", "erorile de rețea, 18456, 4060, SFTP"],
      ["`ConcurentaException`", "înregistrarea a fost modificată între timp de alt utilizator", "`DbUpdateConcurrencyException`"],
      ["`AccesInterzisException`", "rolul nu are drept la operație", "verificarea din sesiune; 229/230 de la server"],
      ["`NegasitException`", "înregistrarea nu mai există", "căutările după cod"],
    ], [26, 40, 34], { caption: "Excepțiile aplicației" }),
    ...fig("BE11a", "EroriSql: traducerea erorilor SQL Server"),
    H(2, "4.4. Serverul de fișiere"),
    P("`SftpFileClient` [[SshNet]] citește și scrie fișiere relativ la folderul de bază din configurare, cu reîncercare la erorile de rețea. Metoda de scriere folosită la modificări copiază întâi versiunea existentă în `Backup/<folder>/AAAA-LL-ZZ/`. `JsonHartaRepository` și `JsonAutobuzRepository` validează structura (noduri unice, conexiuni spre noduri existente, numerotarea locurilor, capacitatea) înainte de scriere."),
    ...fig("BE12a", "SftpFileClient: scrierea cu backup automat"),
    H(2, "4.5. Securitatea"),
    ...bullets([
      "**Parolele** se păstrează ca hash PBKDF2-SHA256, cu sare de 16 octeți și 210 000 de iterații, în formatul `[versiune][iterații][sare][hash]` [[Pbkdf2]]. Formatul vechi (SHA2-512 din scriptul de date inițiale) este recunoscut și înlocuit automat la prima autentificare reușită.",
      "**Limita de încercări**: 5 eșecuri în 15 minute blochează contul 15 minute. Numărarea nu este încă atomică (defectul D7, capitolul 10).",
      "**Configurarea stației** criptează parolele SQL și SFTP cu DPAPI, pentru utilizatorul Windows curent [[Dpapi]].",
      "**Drepturile** se verifică în servicii (rolul din sesiune) și pe server (contul `autogara_app` nu poate modifica structura sau jurnalul de audit).",
    ]),
    ...fig("BE16a", "ParolaHasher: hash-ul PBKDF2 și verificarea"),
    H(2, "4.6. Jurnalul"),
    P("`Jurnal.Configureaza` pornește Serilog [[Serilog]] cu un fișier pe zi în `%LOCALAPPDATA%\\Autogara\\Logs`, păstrat 30 de zile. Se scriu pornirea, conexiunile, operațiile importante și erorile, cu proprietăți structurate (`{CursaID}`, `{Bilete}`), niciodată parole sau hash-uri."),
    ...fig("BE07a_jurnal", "Configurarea jurnalului Serilog"),

    // ======================= 5 =======================
    (startChapter(5), H(1, "5. INTERFAȚA (WINDOWS FORMS)")),
    P("Interfața este scrisă în Windows Forms [[WinForms]]. Fiecare fereastră are fișierul de designer (`*.Designer.cs`), editat doar din Windows Forms Designer, și fișierul de cod, cu evenimentele. `FrmMain` are meniul lateral și încarcă secțiunile ca formulare-copil în zona de conținut; ferestrele de editare se deschid modal."),
    ...fig("FE02_designer_main", "Fereastra principală în Designer"),
    ...table(["Clasa", "Rolul"], [
      ["`Ui/Tema`", "culorile și fonturile aplicației; nicio culoare nu se scrie direct într-un formular"],
      ["`Ui/Iconite`, `Resurse/Iconite`", "iconițele SVG (setul Lucide [[Lucide]]), desenate la dimensiunea și culoarea cerute"],
      ["`Ui/Mesaje`", "`Info`, `Atentie`, `Confirma`, `Eroare` și `RuleazaAsync`, care dezactivează butonul, arată cursorul de așteptare și tratează excepțiile"],
      ["`Ui/Afisare`", "formatarea datelor, orelor și sumelor (`1 932,00 MDL`)"],
      ["`Controale/ButonIcon`", "butoanele cu iconiță și stilurile Primar, Secundar, Pericol, Transparent, Meniu"],
      ["`Controale/GridAutogara`", "tabelul cu stilul aplicației și `Selectat<T>()`"],
      ["`Controale/AutobuzSeatMapControl`, `AutobuzEditorControl`", "schema locurilor la vânzare și în editor"],
      ["`Controale/HartaCanvasControl`", "harta stațiilor, cu zoom, deplasare și editare"],
      ["`Controale/IndicatorConexiune`, `CardKpi`", "starea conexiunii; cardurile dashboard-ului"],
    ], [36, 64], { caption: "Clasele comune ale interfeței" }),
    ...fig("FE03a", "Clasa Tema: paleta de culori"),
    P("Un handler de eveniment care apelează serverul se scrie ca `async void` și își pune tot corpul în `Mesaje.RuleazaAsync(buton, async () => …)`. Astfel butonul nu poate fi apăsat de două ori, iar erorile de rețea sau de reguli ajung la utilizator ca mesaj, nu ca eroare netratată. Excepțiile care scapă totuși sunt prinse în `Program.cs` (`Application.ThreadException`) și scrise în jurnal."),
    H(2, "5.1. Fluxul unei vânzări prin straturi"),
    P("Diagrama de secvență arată cum trece o vânzare prin straturi: rezervarea locului la clic și confirmarea la F2. Ambele operații sunt câte o procedură stocată, într-o tranzacție."),
    ...fig("DG11_secventa_vanzare", "Secvența unei vânzări prin straturile aplicației"),

    // ======================= 6 =======================
    (startChapter(6), H(1, "6. COMPILAREA ȘI TESTAREA")),
    H(2, "6.1. Comenzile"),
    P("Soluția se compilează și se testează la fel din Visual Studio sau din linia de comandă [[DotnetCli]]:"),
    ...code(`dotnet build App\\Autogara.slnx -c Release
dotnet test  App\\Autogara.slnx
dotnet test  App\\Autogara.slnx --filter "FullyQualifiedName~PretCalculator"`),
    ...fig("DO11_dotnet_build", "Recompilarea soluției: 7 proiecte, 0 erori"),
    P("Avertismentele NU1902/NU1903 vin de la pachetul `System.Security.Cryptography.Pkcs` 9.0.4, adus indirect; se elimină adăugând o referință directă la o versiune corectată."),
    H(2, "6.2. Testele unitare"),
    P("Testele xUnit [[Xunit]] nu au nevoie de server și rulează în câteva secunde. Prin teoriile cu mai multe seturi de date rezultă 100 de cazuri."),
    ...table(["Fișierul", "Ce testează"], [
      ["`PretCalculatorTests`", "prețul cu reducere, rotunjirea, procentul de rambursare la 24 h și 2 h"],
      ["`ParolaHasherTests`", "hash-ul PBKDF2, verificarea, formatul vechi, regulile parolei noi"],
      ["`FisiereJsonTests`", "citirea și validarea hărții și a schemelor de autobuz din `File-Server`"],
      ["`EditoareTests`", "grila implicită, adăugarea și scoaterea locurilor, renumerotarea"],
      ["`RuteTests`", "drumul cel mai scurt pe hartă (Chișinău – Bălți prin Orhei, Chișinău – Cahul), nodurile neconectate"],
      ["`TraducereLinqTests`", "că interogările LINQ importante se traduc în SQL, nu se evaluează în memorie"],
      ["`DiverseTests`", "valorile enumerărilor față de constrângerile CHECK, drepturile rolurilor, configurarea stației (DPAPI), cache-ul offline, exportul CSV, modelul EF"],
    ], [28, 72], { caption: "Fișierele de teste" }),
    ...fig("BE23a", "Testele în Test Explorer"),
    P("Un test pică intenționat până la rezolvarea defectului D1: `HartaDinRepository_EsteValida` așteaptă 15 noduri în `File-Server/Harta/harta.json`. Când harta reală se schimbă, testul se actualizează odată cu fișierul, în același commit."),
    H(2, "6.3. Testele de încărcare și concurență"),
    P("Programul `Testare/Stres` simulează mai multe stații care lucrează simultan cu aceleași servicii ca aplicația. El rulează **doar** pe copia `autogara_test` (se oprește pe orice altă bază), citește configurarea din stația locală și creează conturi de test cu parole generate la rulare."),
    ...code(`cd Testare\\Stres
dotnet run -c Release -- --instanta .\\EPSI --doar S1,S2`),
    ...fig("TS01_stres_rezumat", "Rezumatul testelor de stres"),
    P("Copia de test se face cu un backup `COPY_ONLY` al bazei reale, restaurat sub numele `autogara_test` (Manualul administratorului, capitolul 7). Testele de încărcare nu se rulează niciodată pe baza reală."),

    // ======================= 7 =======================
    (startChapter(7), H(1, "7. CONTROLUL VERSIUNILOR")),
    ...bullets([
      "Ramura `main` conține doar cod compilat și testat. Orice modificare se face pe o ramură nouă, cu nume descriptiv (de exemplu `raport-ocupare-pe-ore`), și intră în `main` prin **pull request** pe GitHub, după revizuire.",
      "Commit-urile sunt mici și descrise în română, la imperativ sau ca rezumat al schimbării (de exemplu „Rapoarte: exportul CSV cu separator punct și virgulă”). Un commit nu amestecă modificări fără legătură.",
      "Nu se urcă în depozit: `bin/`, `obj/`, `appsettings.local.json`, `Parole.local.sql`, backup-urile (`*.bak`, `*.dif`), copiile din `File-Server/Backup`. Regulile sunt în `.gitignore`.",
      "**Parolele nu se urcă în Git.** Fișierul `Credentials.txt` din rădăcina depozitului și parolele scrise în `08_Security.sql` sunt cele de dezvoltare; înainte de punerea în funcțiune fișierul se scoate din depozit, iar parolele se schimbă pe server.",
      "Fișierele de date de pe serverul de fișiere (`harta.json`, `autobuz_N.json`) se urcă doar când schimbarea lor este voită și se actualizează și testele care depind de ele.",
    ]),
    ...fig("DO09_git_istoric", "Istoricul commit-urilor proiectului"),

    // ======================= 8 =======================
    (startChapter(8), H(1, "8. LIVRAREA ȘI ACTUALIZAREA")),
    P("O versiune nouă ajunge pe stații prin pașii din diagramă. Aplicația verifică la fiecare deschidere a ecranului de autentificare fișierul `version.json` de pe serverul de fișiere și propune descărcarea dacă versiunea anunțată este mai mare decât cea proprie."),
    ...fig("DG12_livrare", "Livrarea unei versiuni noi", { maxH: 620 }),
    ...numbered([
      "Se mărește versiunea în `App/Autogara.WinForms/Autogara.WinForms.csproj` (`<Version>1.2.0</Version>`), după regula: a treia cifră pentru corecturi, a doua pentru funcții noi, prima pentru schimbări incompatibile ale bazei de date.",
      "După merge în `main`, se publică aplicația:",
    ]),
    ...code(`dotnet publish App\\Autogara.WinForms -c Release -r win-x64 --self-contained false -o publish
Compress-Archive publish\\* Autogara-1.2.0.zip`),
    ...numbered([
      "Arhiva se copiază pe serverul de fișiere, în `Actualizari/`, iar în rădăcina lui se scrie `version.json` cu `versiune`, `fisier` și `note`.",
      "Dacă versiunea cere modificări ale bazei, scriptul de migrare se dă administratorului, care îl rulează pe server **înainte** de publicarea fișierului `version.json`, după un backup complet.",
    ], { continua: true }),
    ...fig("BE22_business_actualizare", "ActualizareService: verificarea și descărcarea versiunii noi"),

    // ======================= 9 =======================
    (startChapter(9), H(1, "9. EXTINDEREA SISTEMULUI")),
    P("O funcție nouă trece prin toate straturile, de jos în sus. Ca exemplu, un raport nou „Vânzări pe ore”:"),
    ...numbered([
      "**Baza de date:** o vedere sau o procedură nouă în `06_Views.sql` / `07_StoredProcedures.sql`, cu `CREATE OR ALTER`, și dreptul de citire pentru `autogara_rapoarte`, dacă e cazul.",
      "**DataAccess:** maparea vederii (folderul `Vederi`) sau metoda nouă din `ProceduriStocate`.",
      "**Business:** un DTO în `Dto.cs` și o metodă în `RaportService`, cu validarea intervalului și verificarea rolului; exportul CSV refolosește metoda existentă.",
      "**WinForms:** un element nou în lista tipurilor din `FrmRapoarte`, o grilă în designer și ramura corespunzătoare din `btnAfiseaza_Click`, cu totalul în `lblTotal`.",
      "**Teste:** un test unitar pentru calcul și, dacă interogarea este LINQ, un test în `TraducereLinqTests`.",
      "**Documentație:** secțiunea din Manualul managerului și, la nevoie, o machetă nouă în `Manuale/generator/machete/machete.py`.",
    ], { continua: false }),
    P("Pentru un câmp nou pe o entitate existentă, ordinea este aceeași: scriptul de migrare, entitatea și maparea, DTO-ul și validarea, formularul de editare, testele."),
    P("Documentația se regenerează cu generatoarele din depozit: `bash Manuale/generator/make.sh` pentru manuale (cu `MACHETE=1` se redesenează și machetele, din fișierele `*.Designer.cs`) și generatorul din `Raport/raport_grup/generator` pentru raportul de grup."),

    // ======================= 10 =======================
    (startChapter(10), H(1, "10. DEFECTELE CUNOSCUTE")),
    P("Defectele găsite la testare și încă deschise, cu remedierea propusă, în ordinea priorității:"),
    ...table(["Cod", "Gravitate", "Defectul", "Remedierea propusă"], [
      ["D7", "Majoră", "limita de 5 încercări la autentificare poate fi ocolită prin încercări simultane", "numărarea și înregistrarea încercării în aceeași tranzacție, cu `sp_getapplock` pe numele contului"],
      ["D3", "Majoră", "backup-ul diferențial eșuează fără backup complet de bază (serverul oprit la 02:00)", "ora backup-ului complet în programul de lucru; verificarea folderului în `09_Jobs.sql`"],
      ["D8", "Minoră", "deadlock-uri (1205) la vânzarea în masă pe aceeași cursă", "aceeași ordine de blocare în `sp_RezervaLoc` și `sp_ConfirmaVanzareBilet` sau reluarea automată"],
      ["D10", "Minoră", "un casier dezactivat mai poate rezerva locuri", "verificarea `Utilizatori.Activ` în `sp_RezervaLoc`"],
      ["D1", "Medie", "testul hărții așteaptă 15 noduri, harta reală are 17", "actualizarea testului odată cu `harta.json`"],
      ["D2", "Minoră", "butonul „Anulează biletul” rămâne activ după anulare", "reîmprospătarea stării butonului după operație"],
      ["D9", "Informativ", "o rezervare poate fi confirmată de alt casier decât cel care a făcut-o", "compararea `UtilizatorID` din rezervare în `sp_ConfirmaVanzareBilet`"],
      ["D11", "Minoră", "numele bazei „autogara” scris fix în mesajul de bază indisponibilă", "numele din configurarea stației"],
      ["D12", "Minoră", "erorile afișate se scriu în jurnal cu nivelul Information", "nivelul Warning sau Error la scrierea în jurnal a erorilor afișate"],
      ["D4, D5", "Minoră", "date de test rămase în bază (un loc ocupat fără bilet, înregistrări „TEST”)", "script de curățare înainte de punerea în funcțiune"],
      ["D6", "Informativ", "alerta de spațiu pe disc pe serverul de dezvoltare", "eliberarea spațiului (nu e defect de cod)"],
    ], [8, 12, 40, 40], { caption: "Defectele cunoscute și remedierea propusă", size: 20 }),

    // ======================= 11 =======================
    (startChapter(11), H(1, "11. CONVENȚIILE DE COD")),
    ...bullets([
      "Numele claselor, metodelor și variabilelor sunt în română, fără diacritice (`CursaService`, `RezervaLocAsync`, `dataCursa`); textele afișate sunt în română, cu diacritice.",
      "Metodele care ajung la server sunt asincrone, cu sufixul `Async`, și primesc `CancellationToken` unde operația poate dura.",
      "Controalele din designer au prefixul tipului: `btn`, `lbl`, `txt`, `cmb`, `dtp`, `num`, `chk`, `grid`, `pnl`, `tab`.",
      "Mesajele pentru utilizator sunt propoziții complete, spun ce s-a întâmplat și ce se poate face, fără detalii tehnice; detaliile merg în jurnal.",
      "Fiecare regulă se verifică o dată în backend (sau în procedura stocată); interfața poate repeta verificarea doar pentru a marca un câmp, nu în locul backend-ului.",
      "Codul nou vine cu test, dacă se poate testa fără server, și cu un commit care descrie de ce s-a făcut schimbarea.",
      "Comentariile XML (`/// <summary>`) explică de ce face o clasă ce face, nu repetă codul.",
    ]),
  ],
};
