// Paginile comune ale documentelor individuale: 1 = proiectul pe scurt, 2–3 = proiectarea teoretică și arhitecturală.
const { Paragraph, TextRun, AlignmentType, HeadingLevel, BorderStyle } = require("docx");
const { P, H, bullets, fig, table } = require("../../raport_grup/generator/lib");

const ECHIPA = [
  ["dmitri", "Gojinevschii Dmitri", "Baza de date", "schema autogara în SQL Server: tabele, indici, funcții, viziuni, proceduri stocate, securitate, joburi, backup"],
  ["sergiu", "Hanganu Sergiu", "Backend și DevOps", "soluția .NET pe straturi, securitatea în aplicație, modul offline; serverul, rețeaua, Git, compilarea și publicarea"],
  ["alexandr", "Crivenco Alexandr", "Frontend", "aplicația Windows Forms: 32 de ferestre, controalele desenate, meniul pe roluri"],
  ["maxim", "Sopivnic Maxim", "Testare", "40 de cazuri funcționale, teste de concurență, încărcare, securitate și recuperare; 12 defecte documentate"],
];

const C = (children, o = {}) => new Paragraph({ alignment: AlignmentType.CENTER, spacing: { line: 240, before: o.before || 0, after: o.after || 0 }, children });
const T = (text, o = {}) => new TextRun({ text, ...o });

// Antetul de pe prima pagină (în locul unei foi de titlu, ca proiectul să înceapă pe pagina 1)
function antet(m) {
  return [
    C([T("IP Colegiul Tehnic Feroviar din Bălţi · grupa BD-423", { size: 22 })]),
    C([T("Disciplina „Elemente de proiectare a sistemelor informatice” · profesor: Vieru Viorica", { size: 22 })], { after: 160 }),
    C([T("Proiectul de grup „Sistem informatic pentru gestionarea unei autogări”", { bold: true, size: 28 })], { after: 60 }),
    new Paragraph({ alignment: AlignmentType.CENTER, spacing: { line: 240, after: 200 },
      border: { bottom: { style: BorderStyle.SINGLE, size: 8, color: "1E5B46", space: 6 } },
      children: [T("Contribuția individuală: ", { size: 26 }), T(m[1], { bold: true, size: 26 }), T(` — ${m[2]}`, { size: 26 })] }),
  ];
}

// Titlu de capitol fără salt de pagină (primul din document)
const H1 = (text) => new Paragraph({ heading: HeadingLevel.HEADING_1, children: [new TextRun(text)] });

function pagina1(cheie) {
  const m = ECHIPA.find((e) => e[0] === cheie);
  return [
    ...antet(m),
    H1("1. PROIECTUL PE SCURT"),
    P("Proiectul de grup este un sistem informatic pentru casieria unei autogări. Aplicația permite vânzarea biletelor cu alegerea locului pe schema autobuzului, rezervarea provizorie a locului, anularea și rambursarea, gestionarea curselor, traseelor, stațiilor, autobuzelor, șoferilor și a mentenanței, rapoarte și un tablou de bord cu cifrele zilei. Utilizatorii au trei roluri: administrator, casier și pasager."),
    P("**Scopul** a fost un sistem complet, de la baza de date până la interfață, care păstrează datele consistente când mai mulți casieri vând simultan: un loc nu poate fi vândut de două ori. Aplicația rulează pe laptopurile echipei, iar un computer are rolul de server, accesibil prin internet, fără VPN."),
    ...table(["Componenta", "Tehnologiile folosite"], [
      ["Baza de date", "Microsoft SQL Server 2025 (instanța EPSI, portul 14330), T-SQL, SQL Server Agent"],
      ["Aplicația", "C#, .NET 10, Windows Forms; Entity Framework Core, SSH.NET, Serilog, DPAPI"],
      ["Serverul de fișiere", "OpenSSH Server (SFTP); fișiere JSON pentru schemele autobuzelor și hartă"],
      ["Testarea", "xUnit (100 de teste unitare), program de stres propriu, SSMS, sqlcmd"],
      ["Instrumente", "Visual Studio, SQL Server Management Studio, Git și GitHub"],
    ], [26, 74], { caption: "Tehnologiile proiectului", size: 21 }),
    ...table(["Membrul echipei", "Compartimentul", "Ce cuprinde"],
      ECHIPA.map((e) => e[0] === cheie ? [`**${e[1]}**`, `**${e[2]}**`, e[3]] : [e[1], e[2], e[3]]),
      [26, 20, 54], { caption: "Echipa și împărțirea lucrului", size: 21 }),
    P("Proiectul a fost realizat după modelul cascadă, între 17 septembrie și 17 noiembrie 2026, iar tot codul și documentația sunt păstrate într-un depozit Git comun."),
  ];
}

function pagini23() {
  return [
    H(1, "2. PROIECTAREA TEORETICĂ ȘI ARHITECTURALĂ"),
    H(2, "2.1. Ciclul de viață și metodologia"),
    P("Ciclul de viață al unui sistem informatic cuprinde fazele prin care trece sistemul de la idee până la înlocuire: analiza, proiectarea de ansamblu și de detaliu, realizarea, testarea, punerea în funcțiune și exploatarea. Dintre modelele de dezvoltare (cascadă, iterativ, prototip, Agile) s-a ales **modelul cascadă**, pentru că cerințele erau fixate de la început în Sarcina tehnică, iar compartimentele depind unul de altul: backend-ul se construiește peste procedurile bazei de date, iar interfața peste serviciile backend-ului."),
    ...fig("AN04_etape_cascada", "Etapele proiectării sistemului după modelul cascadă", { maxW: 520 }),
    H(2, "2.2. Analiza sistemului existent și cerințele"),
    P("În situația existentă, o autogară ține biletele, cursele și ITP-ul autobuzelor în registre pe hârtie sau în Excel, separat la fiecare ghișeu. De aici apar trei probleme: același loc poate fi vândut de două ori, datele se copiază de mai multe ori, iar rapoartele vin cu întârziere. Din analiză au rezultat cerințele sistemului nou:"),
    ...bullets([
      "**funcționale**: căutarea curselor, rezervarea provizorie și vânzarea cu alegerea locului, reducerile, anularea cu rambursare după reguli de timp, gestionarea nomenclatoarelor, rapoartele și jurnalul de audit;",
      "**nefuncționale**: mai multe stații simultan fără bilete duble, securitate pe roluri, lucru prin internet cu trafic criptat, funcționare doar pentru citire când serverul nu răspunde, interfață în limba română.",
    ]),
    H(2, "2.3. Arhitectura sistemului"),
    P("Sistemul are o arhitectură **client–server în două niveluri**, fără API web intermediar: aplicația desktop de pe fiecare stație se conectează direct la SQL Server (criptat) și la serverul de fișiere (SFTP). Soluția a fost aleasă pentru că există un singur tip de client, iar logica tranzacțională stă deja în procedurile stocate. Routerul redirecționează din internet doar cele două porturi necesare."),
    ...fig("Arhitectura", "Arhitectura sistemului: stația de lucru, rețeaua și serverul", { maxW: 600 }),
    P("Aplicația este împărțită în straturi, cu dependențe într-un singur sens: **interfața** (Autogara.WinForms) apelează doar **serviciile** (Autogara.Business); acestea folosesc **accesul la date** (Autogara.DataAccess, EF Core și proceduri stocate) și **fișierele** (Autogara.FileServer, SFTP); la bază stau **entitățile** (Autogara.Domain) și **utilitarele comune** (Autogara.Common). Astfel regulile nu depind de formulare și pot fi testate separat."),
    H(2, "2.4. Modelul datelor și distribuirea prelucrărilor"),
    P("Modelul datelor are 17 tabele relaționale într-o singură bază, `autogara`: nomenclatoarele (roluri, utilizatori, stații, trasee, autobuze, șoferi, reduceri), cursele cu locurile lor și operațiile (rezervări provizorii, bilete, plăți, jurnalul de audit). Integritatea este garantată de server prin chei externe, constrângeri CHECK și UNIQUE."),
    P("Sistemul este **distribuit în privința prelucrărilor și centralizat în privința datelor**:"),
    ...bullets([
      "regulile critice (rezervarea, vânzarea, anularea) rulează în proceduri stocate tranzacționale, cu blocări UPDLOCK și HOLDLOCK, deci un loc se rezervă o singură dată, oricâte stații încearcă simultan;",
      "validările simple, calculul afișat al prețului și interfața rulează pe stație, ca serverul să nu fie încărcat inutil;",
      "pe stație există doar o copie locală pentru citire (cache), folosită în modul offline; nicio scriere nu se face în afara serverului;",
      "datele se păstrează în UTC, iar ora Moldovei se calculează pe server, ca stațiile cu ceasuri diferite să vadă aceeași oră.",
    ]),
    P("Comportamentul sistemului a fost descris și în UML, prin diagrama cazurilor de utilizare (actorii casier, pasager, administrator și SQL Server Agent), diagrama de clase a entităților și diagrama de secvență a vânzării unui bilet."),
  ];
}

module.exports = { ECHIPA, pagina1, pagini23 };
