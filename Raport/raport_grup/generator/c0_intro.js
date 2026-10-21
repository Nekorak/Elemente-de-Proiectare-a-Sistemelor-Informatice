const { P, H, bullets, table } = require("./lib");

module.exports = () => [
  H(1, "INTRODUCERE"),
  P("**Tema proiectului:** „Sistem informatic pentru gestionarea unei autogări”. Proiectul a fost realizat în grup, de patru persoane, la disciplina Elemente de proiectare a sistemelor informatice, și cuprinde baza de date, partea de server (backend și infrastructura DevOps), interfața aplicației (frontend) și testarea."),
  P("**Descrierea pe scurt.** Aplicația este destinată unei autogări: vânzarea biletelor cu alegerea locului pe schema autobuzului, rezervarea provizorie a locului, anularea și rambursarea biletelor, gestionarea curselor, traseelor, stațiilor, autobuzelor, șoferilor și a mentenanței, rapoarte și un tablou de bord (dashboard) cu cifrele zilei. Utilizatorii au trei roluri: administrator, casier și pasager. Aplicația rulează pe stațiile de lucru (laptopuri), iar un computer al echipei are rolul de server: pe el rulează Microsoft SQL Server, cu baza de date, și un server de fișiere SFTP, cu fișierele JSON ale autobuzelor și ale hărții. Stațiile se conectează la server prin internet."),
  P("**Scopul proiectului** este proiectarea și realizarea unui sistem informatic complet, de la baza de date până la interfață, care să automatizeze activitatea casieriei unei autogări și să păstreze datele consistente atunci când mai mulți casieri lucrează simultan."),
  P("**Actualitatea temei.** Multe autogări din Republica Moldova vând încă bilete pe hârtie sau în programe care nu arată locurile ocupate în timp real, iar evidența curselor, a autobuzelor și a ITP-ului se ține separat. Un sistem centralizat, cu o singură bază de date pentru toate casele, previne vânzarea de două ori a aceluiași loc, oferă rapoarte imediate și păstrează un jurnal al tuturor operațiilor. Pentru specialitatea noastră, proiectul este un exemplu real de administrare a unei baze de date: proiectarea schemei, securitatea pe roluri, joburile automate, backup-ul și accesul de la distanță."),
  P("**Împărțirea pe compartimente și pe membrii echipei:**"),
  ...table(["Compartiment", "Responsabil", "Ce cuprinde"], [
    ["1. Baza de date", "Gojinevschii Dmitri", "schema autogara: 17 tabele, indici, funcții, 5 viziuni, 10 proceduri stocate, roluri și permisiuni, 6 joburi SQL Server Agent, backup și restaurare, date de test"],
    ["2. Backend și DevOps", "Hanganu Sergiu", "soluția .NET pe straturi (Domain, Common, DataAccess, FileServer, Business), securitatea în aplicație, modul offline; serverul: SQL Server pe port propriu, OpenSSH/SFTP, firewall, redirecționarea porturilor, Git, compilare, testare și publicare"],
    ["3. Frontend", "Crivenco Alexandr", "aplicația Windows Forms: designul, cele 32 de ferestre, controalele desenate (schema locurilor, editorul hărții și al autobuzului), meniul pe roluri"],
    ["4. Testarea", "Sopivnic Maxim", "40 de cazuri de test pe roluri, concurență, securitate, integritatea datelor și joburi; 6 defecte documentate"],
  ], [24, 22, 54], { caption: "Compartimentele proiectului și responsabilii" }),
  P("Fiecare compartiment se încheie cu concluziile proprii, iar fiecare punct este argumentat cu capturi de ecran din mediul real de lucru: SQL Server Management Studio și consola, Visual Studio, serverul și aplicația pornită. Capturile sunt incluse direct în text, lângă explicație."),
  P("**Avantajele soluției:**"),
  ...bullets([
    "o singură bază de date pentru toate stațiile; operațiile critice (rezervare, vânzare, anulare) rulează în proceduri stocate tranzacționale, deci un loc nu poate fi vândut de două ori;",
    "securitate pe mai multe niveluri: conturi SQL cu drepturi minime, parole PBKDF2 în aplicație, blocarea după încercări greșite, jurnal de audit;",
    "operații automate pe server: eliberarea rezervărilor expirate, backup, curățarea jurnalului, alertele de ITP și de spațiu pe disc;",
    "acces de la distanță fără VPN, pe porturi nestandard, cu trafic criptat;",
    "interfață în limba română, cu schema autobuzului și harta stațiilor desenate în aplicație.",
  ]),
  P("**Dezavantaje și limite:**"),
  ...bullets([
    "serverul este un computer de acasă: dacă este oprit, aplicația lucrează doar în mod de citire, din datele salvate local;",
    "vânzarea nu este integrată cu o casă de marcat fiscală;",
    "aplicația este doar pentru Windows (Windows Forms), fără variantă web sau mobilă;",
    "backup-ul automat depinde de serverul pornit la ora programată (vezi defectul D3 de la testare).",
  ]),
  P("**Concluzie.** Proiectul acoperă întreg ciclul unui sistem informatic cu bază de date: analiză, proiectare, realizare, punere în funcțiune pe un server real și testare. Modelul de lucru ales a fost cascada (waterfall): fiecare etapă s-a finalizat înainte de trecerea la următoarea, iar munca fiecărui membru a fost păstrată în același depozit Git."),
];
