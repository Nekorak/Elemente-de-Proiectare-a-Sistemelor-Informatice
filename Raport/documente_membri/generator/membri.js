// Paginile 4–5: lucrul fiecărui membru al echipei (după capitolele 2–5 ale raportului de grup).
const { P, H, bullets, code, fig, table } = require("../../raport_grup/generator/lib");

const dmitri = () => [
  H(1, "3. CONTRIBUȚIA INDIVIDUALĂ: BAZA DE DATE"),
  P("În proiect, **Gojinevschii Dmitri** a proiectat și a realizat baza de date, nucleul sistemului: toate stațiile citesc și scriu aceleași date, iar regulile importante (locuri, prețuri, rambursări, drepturi) sunt verificate chiar de SQL Server, indiferent de client. Toate obiectele au fost create prin scripturi T-SQL numerotate (01–12), păstrate în folderul `SQL Code` din depozit."),
  ...table(["Obiecte", "Număr", "Exemple"], [
    ["Tabele", "17", "Curse, Locuri, RezervariProvizorii, Bilete, Plati, LogAudit"],
    ["Indici nonclustered", "15", "IX_Curse_DataCursa_TraseuID; UX_Bilete_LocID_Activ (unic, filtrat)"],
    ["Funcții / viziuni", "6 / 5", "fn_AcumLocal, prețul cu reducere; vw_LocuriDisponibile, vw_OcupareCurse"],
    ["Proceduri stocate", "10", "sp_RezervaLoc, sp_ConfirmaVanzareBilet, sp_AnuleazaBilet"],
    ["Login-uri SQL / joburi", "6 / 6", "autogara_app, autogara_rapoarte; eliberarea rezervărilor, backup"],
  ], [26, 12, 62], { caption: "Obiectele bazei de date autogara", size: 21 }),
  H(2, "3.1. Schema și convențiile"),
  P("Baza `autogara` folosește colația `Romanian_100_CI_AS` (diacriticele se sortează corect) și opțiunea READ_COMMITTED_SNAPSHOT, prin care citirile nu blochează scrierile casierilor. Convențiile au fost stabilite înainte de tabele: chei `<Entitate>ID`, GUID pentru utilizatori, rezervări și bilete, coloane de audit în UTC, ștergere logică prin coloana Activ, ROWVERSION pe tabelele modificate concurent și nume explicite pentru toate constrângerile (PK_, FK_, UQ_, CK_), ca erorile să poată fi traduse în aplicație."),
  ...fig("ER_diagrama", "Diagrama relațiilor dintre cele 17 tabele ale schemei autogara", { maxH: 330 }),
  H(2, "3.2. Procedurile stocate și concurența"),
  P("Operațiile critice sunt proceduri stocate cu `SET XACT_ABORT ON`, tranzacție TRY/CATCH și blocări explicite pe rândurile citite. `sp_RezervaLoc` rezervă un loc liber pentru câteva minute, iar `sp_ConfirmaVanzareBilet` transformă rezervarea în bilet: calculează prețul cu reducerea, generează codul biletului, ocupă locul, înregistrează plata și scrie în jurnal. Indexul unic filtrat `UX_Bilete_LocID_Activ` este a doua barieră: pe un loc poate exista un singur bilet activ. Erorile de business au coduri 50010–50066 și mesaje în română."),
  ...fig("BD51_sp_RezervaLoc_ConfirmaVanzare", "sp_RezervaLoc și sp_ConfirmaVanzareBilet: locul rezervat, biletul emis cu reducere, plata și jurnalul", { maxW: 600 }),
  H(2, "3.3. Securitatea, joburile și backup-ul"),
  P("Securitatea urmează principiul drepturilor minime: aplicația folosește contul `autogara_app` (citire, scriere și EXECUTE, dar fără drept de a crea sau șterge tabele și de a modifica jurnalul), rapoartele contul `autogara_rapoarte` (doar citire), iar fiecare membru al echipei are login propriu. Parolele utilizatorilor se păstrează doar ca hash PBKDF2."),
  P("Șase joburi SQL Server Agent automatizează întreținerea: eliberarea rezervărilor expirate (în fiecare minut), backup-ul complet și diferențial, curățarea jurnalului, alertele de ITP și de spațiu pe disc. Scriptul `10_BackupRestore.sql` verifică restaurarea într-o bază separată, cu DBCC CHECKDB. Pentru testare au fost generate aproximativ 10 000 de bilete realiste (`12_DateVolum.sql`)."),
  H(2, "3.4. Dificultăți și soluții"),
  ...bullets([
    "**doi casieri pe același loc**: verificarea „locul e liber” și scrierea biletului se fac în aceeași tranzacție, cu blocări UPDLOCK și HOLDLOCK, iar indexul unic filtrat oprește orice bilet activ dublu;",
    "**ora diferită pe stații**: toate datele se păstrează în UTC, iar ora Moldovei se calculează pe server, prin funcția `fn_AcumLocal`;",
    "**rezervări abandonate**: o rezervare expirată este tratată ca liberă de `sp_RezervaLoc`, iar jobul de eliberare o șterge în cel mult un minut.",
  ]),
  P("**Rezultat.** La testare, 20 de stații au încercat simultan același loc fără niciun bilet dublu. A rămas de corectat backup-ul complet automat, programat la 02:00, când serverul este oprit (defectul D3)."),
];

const sergiu = () => [
  H(1, "3. CONTRIBUȚIA INDIVIDUALĂ: BACKEND ȘI DEVOPS"),
  P("În proiect, **Hanganu Sergiu** a realizat partea de server a aplicației, adică toată logica dintre interfață și baza de date, și infrastructura pe care rulează sistemul: serverul, serviciile, rețeaua, controlul versiunilor, compilarea, testarea și publicarea."),
  H(2, "3.1. Soluția .NET pe straturi"),
  P("Soluția `App/Autogara.slnx` are 7 proiecte pe .NET 10, cu dependențe într-un singur sens. Bibliotecile externe sunt puține: Entity Framework Core pentru SQL Server, SSH.NET pentru SFTP, Serilog pentru jurnal și DPAPI pentru criptarea parolelor din configurare."),
  ...table(["Proiect", "Rolul"], [
    ["Autogara.Domain", "entitățile (1:1 cu tabelele) și enumerările de status"],
    ["Autogara.Common", "configurarea stației (parole DPAPI), excepțiile, jurnalul Serilog, ora Moldovei"],
    ["Autogara.DataAccess", "contextul EF Core, apelul procedurilor stocate, traducerea erorilor SQL"],
    ["Autogara.FileServer", "clientul SFTP: citirea și scrierea JSON-urilor autobuzelor și ale hărții, cu validare"],
    ["Autogara.Business", "serviciile aplicației, reunite în clasa AutogaraBackend"],
    ["Autogara.Tests", "100 de teste xUnit pentru prețuri, parole, fișiere JSON, editoare"],
  ], [28, 72], { caption: "Proiectele backend-ului", size: 21 }),
  H(2, "3.2. Serviciile și securitatea în aplicație"),
  ...bullets([
    "**autentificarea**: parole PBKDF2-SHA256 cu sare, blocarea contului după 5 încercări greșite, fiecare autentificare scrisă în jurnalul de audit; rolul este verificat în fiecare serviciu, nu doar în meniu;",
    "**vânzarea**: BiletService apelează procedurile stocate, iar PretCalculator calculează reducerea și procentul de rambursare (24 h / 2 h înainte de plecare);",
    "**accesul la date**: reîncercare automată de 3 ori la erori de rețea și traducerea erorilor SQL Server în mesaje pentru utilizator;",
    "**modul offline**: CacheLocalService păstrează local cursele, stațiile, harta și autobuzele, iar ConexiuneStareService verifică periodic baza și serverul de fișiere și revine singur online;",
    "**rapoartele și actualizarea**: export CSV arhivat pe serverul de fișiere și verificarea unei versiuni noi la pornire.",
  ]),
  ...fig("BE15_business_auth", "AuthService.AutentificaAsync: limita de încercări, verificarea parolei și jurnalul", { maxW: 400 }),
  H(2, "3.3. DevOps: serverul, rețeaua și livrarea"),
  P("Pe server, instanța SQL Server EPSI și SQL Server Agent pornesc automat, protocolul TCP/IP ascultă pe portul fix **14330**, iar fișierele sunt servite de OpenSSH Server prin SFTP. S-a ales SFTP și nu un share SMB pentru că portul 445 este blocat de multe rețele, iar SFTP lucrează pe un singur port, criptat. Routerul redirecționează portul 14330 și portul extern 44545 spre portul 22 al serverului; firewall-ul Windows permite doar aceste intrări."),
  ...fig("TC32_conexiune_porturi_publice", "Porturile publice 14330 și 44545 răspund la adresa externă a serverului", { maxW: 440 }),
  P("Codul, scripturile și documentația sunt într-un depozit Git pe GitHub. Soluția se compilează, se testează și se publică la fel pe orice calculator al echipei:"),
  ...code(`dotnet build App\\Autogara.slnx -c Release
dotnet test  App\\Autogara.slnx
dotnet publish App\\Autogara.WinForms -c Release -r win-x64 -o publish`),
  H(2, "3.4. Dificultăți și soluții"),
  ...bullets([
    "**portul 445 blocat în multe rețele**: serverul de fișiere a fost făcut pe SFTP (un singur port, criptat) în loc de un share de rețea;",
    "**parolele de conectare pe stații**: fișierul de configurare le păstrează criptate cu DPAPI, deci nu poate fi folosit pe alt calculator;",
    "**serverul oprit sau internetul căzut**: aplicația trece singură în modul offline, doar pentru citire, și revine online fără repornire;",
    "**erorile SQL greu de înțeles**: numele explicite ale constrângerilor și codurile 50010–50066 sunt traduse în mesaje în română.",
  ]),
  P("**Rezultat.** Echipa a lucrat de pe laptopuri diferite, prin internet, cu aceeași bază și aceleași fișiere. Recompilarea celor 7 proiecte reușește fără erori, iar din 100 de teste unitare trec 99 (unul așteaptă vechea hartă, modificată local)."),
];

const alexandr = () => [
  H(1, "3. CONTRIBUȚIA INDIVIDUALĂ: FRONTEND"),
  P("În proiect, **Crivenco Alexandr** a realizat interfața aplicației, proiectul Autogara.WinForms (Windows Forms pe .NET 10). Interfața nu conține logică de business: fiecare formular citește datele introduse, apelează un serviciu din AutogaraBackend și afișează rezultatul."),
  H(2, "3.1. Designul și organizarea"),
  P("Interfața are **32 de ferestre**, 9 controale desenate și clasele comune din folderul `Ui`. Aspectul fiecărei ferestre stă în fișierul `*.Designer.cs`, editabil vizual în Visual Studio. Culorile și fonturile sunt definite într-un singur loc, clasa `Tema`: verdele închis ca culoare principală și galbenul ca semnal, după panourile de informare din autogări, fontul Bahnschrift pentru titluri și Segoe UI pentru text. Iconițele sunt 72 de fișiere SVG (setul Lucide), clare la orice scalare."),
  ...bullets([
    "listele au deasupra filtrele și butoanele Adaugă, Modifică, Dezactivează; formularele modale au „Salvează” și „Renunță”;",
    "câmpurile greșite sunt marcate lângă câmp, cu mesaje în română; operațiile ireversibile cer confirmare;",
    "indicatorul de conexiune arată permanent dacă stația lucrează online sau din cache.",
  ]),
  H(2, "3.2. Fereastra principală și meniul pe roluri"),
  P("După autentificare se deschide FrmMain: meniul în stânga, antetul cu utilizatorul și rolul, iar secțiunile se deschid în zona de conținut. Meniul conține doar secțiunile permise rolului: administratorul are toate cele 14 secțiuni, casierul doar Dashboard, Vânzare bilete și Bilete, iar pasagerul doar Cumpără bilet și Biletele mele. Dashboard-ul arată cifrele zilei în carduri și se reîmprospătează singur la 30 de secunde."),
  ...fig("EX_dashboard_admin", "Fereastra principală a administratorului, cu meniul complet și dashboard-ul", { maxW: 470 }),
  H(2, "3.3. Vânzarea și controalele desenate"),
  P("Fereastra de vânzare desenează schema autobuzului din fișierul JSON de pe serverul de fișiere. Locurile sunt colorate după stare (liber, rezervat de alt casier, ocupat, ales); la clic pe un loc se face rezervarea provizorie, iar în dreapta se completează pasagerul, reducerea și plata. **F2** confirmă vânzarea, iar **Esc** închide fereastra și eliberează locul."),
  ...fig("EX_vanzare", "Fereastra de vânzare: schema locurilor, rezervarea provizorie și reducerea", { maxW: 470 }),
  P("Cele mai complexe părți sunt controalele desenate de echipă: **schema locurilor** (AutobuzSeatMapControl), **editorul de autobuz**, în care administratorul așază locurile, culoarul și ușile, și **editorul hărții** (HartaCanvasControl), în care se adaugă stații și drumuri cu distanțele lor. La prima pornire, asistentul de configurare cere adresele serverului și testează separat baza de date și serverul de fișiere."),
  H(2, "3.4. Dificultăți și soluții"),
  ...bullets([
    "**autobuze cu așezări diferite ale locurilor**: schema nu este desenată fix, ci citită din fișierul JSON al fiecărui autobuz, creat în editorul de autobuz;",
    "**ferestre care „îngheață” la apelurile prin internet**: toate apelurile către server sunt asincrone (async/await), iar butoanele se dezactivează cât durează operația;",
    "**ecrane cu scalări diferite**: iconițele sunt vectoriale (SVG), desenate la mărimea cerută, deci rămân clare;",
    "**greșelile de introducere**: câmpurile sunt validate înainte de apelul serverului, cu mesajul lângă câmp.",
  ]),
  P("**Rezultat.** Interfața acoperă toate rolurile și operațiile din Sarcina tehnică, cu un design unitar definit într-un singur loc; separarea de logică a permis schimbarea designului fără a modifica backend-ul."),
];

const maxim = () => [
  H(1, "3. CONTRIBUȚIA INDIVIDUALĂ: TESTAREA"),
  P("În proiect, **Sopivnic Maxim** a condus testarea, în două etape: pe 13 octombrie 2026 au fost verificate funcțiile aplicației caz cu caz, iar pe 21–22 octombrie s-a încercat „stricarea” sistemului cu mai multe stații simultane, încărcare, atacuri simple și căderi. Testele care scriu date au rulat pe `autogara_test`, o copie a bazei restaurată dintr-un backup COPY_ONLY, ca datele reale să rămână neatinse."),
  ...table(["Nivel", "Cum s-a testat", "Rezultat"], [
    ["Funcțional", "40 de cazuri, manual, în aplicație și în SSMS, cu captură pentru fiecare", "35 trecute, 1 cu observație, 4 eșuate"],
    ["Concurență și încărcare", "program de stres propriu, până la 20 de stații virtuale pornite simultan", "13 din 14 verificări trecute"],
    ["Mai multe instanțe", "5 instanțe ale aplicației deschise simultan, conturi diferite", "7 scenarii, toate trecute"],
    ["Securitate", "injecție SQL, valori invalide, drepturile contului SQL și ale rolurilor", "toate atacurile oprite"],
    ["Recuperare", "căderea stației, baza OFFLINE, backup și restaurare", "4 scenarii trecute"],
  ], [20, 50, 30], { caption: "Nivelurile de testare și rezultatele", size: 21 }),
  H(2, "3.1. Concurența și încărcarea"),
  P("Programul de consolă `Testare/Stres` folosește direct serviciile aplicației, deci trece prin aceleași reguli și proceduri ca o stație reală, și refuză să ruleze pe altă bază decât copia de test. Stațiile virtuale așteaptă la o barieră comună și pornesc în același moment. Cu 5, 10 și 20 de stații pe același loc a trecut o singură rezervare, restul fiind refuzate; la vânzarea în masă, 51 de bilete au fost vândute în 4,4 s fără niciun loc vândut de două ori; dashboard-ul a răspuns la 200 de apeluri cu mediana de 9 ms."),
  ...fig("TS01_stres_rezumat", "Rularea programului de stres: rezultatul fiecărui scenariu și totalul de 14 verificări", { maxW: 520 }),
  H(2, "3.2. Mai multe instanțe și recuperarea"),
  P("Cu 5 instanțe ale aplicației așezate una lângă alta s-a văzut efectul unei stații asupra celorlalte: când doi casieri aleg același loc, al doilea primește refuzul, iar schema lui se reîncarcă; doi administratori care salvează același cont sunt opriți prin RowVersion. Baza trecută OFFLINE cu stațiile deschise a fost detectată de toate în cel mult 30 de secunde (mod local), iar după readucerea ei ONLINE stațiile au revenit singure, fără repornire."),
  ...fig("GT02_acelasi_loc_doua_statii", "Două stații pe același loc: casierul 1 are locul rezervat, casierul 3 primește refuzul", { maxW: 470 }),
  H(2, "3.3. Defectele găsite"),
  P("Au fost documentate **12 defecte** (D1–D12), cu severitatea, locul de reproducere și dovada. Cele mai importante:"),
  ...bullets([
    "**D3 (majoră)**: backup-ul diferențial eșuează, pentru că backup-ul complet de la 02:00 nu rulează (serverul este oprit noaptea);",
    "**D7 (majoră)**: limita de 5 încercări la autentificare poate fi ocolită cu 20 de încercări trimise simultan;",
    "**D8, D10 (minore)**: blocaje reciproce la vânzarea în masă (datele rămân corecte); un casier dezactivat mai poate rezerva locuri.",
  ]),
  H(2, "3.4. Dificultăți și soluții"),
  ...bullets([
    "**testele nu trebuiau să strice datele reale**: toate testele care scriu au rulat pe copia `autogara_test`, iar programul de stres refuză orice altă bază;",
    "**simultaneitate reală**: stațiile virtuale pornesc de la o barieră comună, ca cererile să ajungă la server în același moment;",
    "**dovezile**: fiecare caz are captură de ecran, iar momentele căderii și ale revenirii au fost luate din jurnalele stațiilor.",
  ]),
  P("**Rezultat.** Partea cea mai sensibilă, vânzarea aceluiași loc de pe mai multe stații, a rezistat la 20 de stații simultane fără bilete duble sau plăți greșite. Înainte de predare trebuie remediate D3 și D7."),
];

module.exports = { dmitri, sergiu, alexandr, maxim };
