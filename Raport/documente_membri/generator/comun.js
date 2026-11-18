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
  ];
}

// Paginile de proiectare urmează cele 32 de puncte ale scenariului de proiectare (numerele în paranteză la fiecare subtitlu)
function pagini23() {
  const S = 18;
  return [
    H(1, "2. PROIECTAREA TEORETICĂ ȘI ARHITECTURALĂ"),
    P("Secțiunea urmează pe scurt cele 32 de puncte ale scenariului de proiectare (numerele lor sunt în paranteză la subtitluri); cifrele despre firmă și costuri sunt estimări ale echipei."),

    H(2, "2.1. Obiectivele, etapele și metodologia (p. 1–3)"),
    P("**Obiectivele** sistemului, fiecare cu un indicator verificabil: vânzarea biletului cu alegerea locului în cel mult 2 minute; niciun loc vândut de două ori, cu până la 20 de stații simultane; rezervarea provizorie a locului, eliberată automat la expirare; anularea cu rambursare de 100 %, 50 % sau 0 %, după timpul rămas până la plecare; rapoarte imediate pentru conducere; evidența flotei cu alertă de ITP; securitatea datelor; backup zilnic verificat; lucrul de pe mai multe stații prin internet, criptat."),
    ...table(["Etapa (perioada)", "Activitățile", "Metodele de colectare a informațiilor"], [
      ["1. Analiza (17.09)", "studiul autogării, cerințele, Sarcina tehnică", "interviul, observarea, analiza documentelor, chestionarul"],
      ["2. Proiectarea de ansamblu (17.09)", "arhitectura, compartimentele", "studiul aplicațiilor similare, discuțiile în echipă"],
      ["3. Proiectarea de detaliu (17–18.09)", "schema bazei, straturile .NET, ferestrele", "analiza documentelor (bilet, orar, raport de casă)"],
      ["4. Realizarea (17–19.09)", "baza de date, backend-ul, interfața", "istoricul Git, testele automate"],
      ["5. Testarea (13.10; 21–22.10)", "funcțională; stres, securitate, recuperare", "rezultatele testelor, jurnalele aplicației"],
      ["6. Punerea în funcțiune (până la 17.11)", "serverul, manualele, documentația", "rapoartele de stare, defectele găsite"],
    ], [33, 31, 36], { caption: "Etapele proiectării și metodele de colectare a informațiilor", size: S }),
    P("**Metodologia.** Au fost comparate modelele cascadă, iterativ, prototip și Agile. S-a ales **cascada**, pentru că cerințele erau fixate în Sarcina tehnică, echipa era mică, iar compartimentele depind unul de altul: backend-ul se construiește peste procedurile bazei, iar interfața peste serviciile backend-ului."),

    H(2, "2.2. Firma și studiul sistemului existent (p. 7, 8, 10–14)"),
    P("**Firma** este o autogară regională cu 3 ghișee, care deservește cursele mai multor transportatori. Serviciile ei: vânzarea biletelor la ghișeu și online, informarea pasagerilor, accesul autobuzelor la peroane (taxa de peron), păstrarea bagajelor. Resursele: aproximativ 11 angajați (director, 2 dispeceri, 6 casieri, contabil, tehnician), 4 calculatoare, case de marcat, rețea locală și internet; volumul este de circa 300 de bilete și 40–50 de curse pe zi."),
    P("**Fluxul informațional existent** este în mare parte pe hârtie: transportatorul aduce orarul și actele autobuzelor la dispecer, casierul copiază cursa și prețul pe biletul tipizat și în registrul casei, din registru se face raportul de casă, iar lunar totalurile se recopiază în Excel. **Observarea ghidată** a unei vânzări la ghișeu a dat circa 170 s în situația existentă, față de circa 65 s estimate în sistemul nou. **Problemele de colectare**: aceeași informație se scrie de 3–4 ori, casierii nu văd locurile vândute la alte ghișee, rapoartele se fac manual și cu întârziere, ITP-ul se ține separat de orar, iar autorul unei operații nu se cunoaște."),
    P("**Accesul și securizarea existente**: un singur cont Windows cu parola știută de toți, registrele în sertarul ghișeului, fișierul Excel fără copie, parolele scrise pe hârtie, datele pasagerilor pe hârtie. **Necesitățile informaționale**: casierul are nevoie de cursele și locurile libere în timp real; dispecerul, de orar, flotă și ITP; conducerea și contabilitatea, de vânzările pe zi, traseu și casier, de ocupare și de încasările pe metode de plată; pasagerul, de curse, prețuri și biletele proprii. Aceste constatări alcătuiesc documentul „Studiul și analiza sistemului informațional existent”, din care au rezultat cerințele sistemului nou."),

    H(2, "2.3. Caietul de sarcini (p. 9, 19, 20)"),
    ...bullets([
      "**cerințele tehnice**: server Windows cu SQL Server și OpenSSH, IP fix și două porturi redirecționate; stații Windows 10/11 cu .NET 10; conexiuni criptate; cel puțin 20 de stații simultane fără bilete duble; backup zilnic și restaurare verificată; interfață în limba română;",
      "**cerințele legislative**: protecția datelor cu caracter personal (Legea nr. 133/2011), biletul ca document de călătorie (cursa, data, locul, prețul), evidența fiscală (se păstrează numărul bonului fiscal; integrarea cu casa de marcat este amânată), licențe care permit folosirea gratuită;",
      "**sarcinile pentru proiectarea detaliată**, pe compartimente (Tabelul 2): schema, procedurile, securitatea, joburile și backup-ul bazei; accesul la date, autentificarea, vânzarea și modul offline; ferestrele, editoarele și dashboard-ul; planul de testare, concurența, securitatea și regresia;",
      "**drepturile și obligațiile**: beneficiarul primește sistemul conform caietului de sarcini și cere remedieri în garanție, dar furnizează datele inițiale, serverul și participă la instruire și recepție; executantul primește datele și accesul necesar, dar livrează sistemul și manualele, instruiește personalul și păstrează confidențialitatea datelor.",
    ]),

    H(2, "2.4. Intrările și ieșirile sistemului (p. 21, 22)"),
    ...bullets([
      "**intrările**: datele pasagerului (nume, telefon); cursa, locul, reducerea, metoda de plată și numărul bonului; cererile de anulare; stațiile și distanțele hărții; traseele, cursele, autobuzele cu schema locurilor, ITP-ul și mentenanța, șoferii; conturile utilizatorilor;",
      "**ieșirile**: biletul cu cod unic; suma rambursată; lista curselor cu locurile libere; dashboard-ul zilei; rapoartele de vânzări, de ocupare și comparația lunară (cu export CSV); alertele de ITP și de spațiu pe disc; jurnalul de audit; copiile de rezervă.",
    ]),

    H(2, "2.5. Proiectul de ansamblu și de detaliu (p. 15, 16, 18, 23)"),
    P("Sistemul are o arhitectură **client–server în două niveluri**, fără API web intermediar: aplicația desktop de pe fiecare stație se conectează direct la SQL Server (criptat) și la serverul de fișiere (SFTP), iar routerul redirecționează din internet doar cele două porturi necesare."),
    ...fig("Arhitectura", "Schema proiectului de ansamblu: stația de lucru, rețeaua și serverul", { maxW: 380 }),
    P("**Proiectul de detaliu.** Aplicația este împărțită în straturi cu dependențe într-un singur sens: interfața (WinForms) apelează doar serviciile (Business), acestea folosesc accesul la date (EF Core și proceduri stocate) și fișierele (SFTP), iar la bază stau entitățile și utilitarele comune. **Structura bazei de date**: 17 tabele (nomenclatoare, curse cu locuri, rezervări, bilete, plăți, jurnal de audit), 15 indici, 6 funcții, 5 viziuni, 10 proceduri stocate și 6 joburi. Datele sunt centralizate pe server, iar prelucrările distribuite: regulile critice rulează în proceduri tranzacționale cu blocări UPDLOCK și HOLDLOCK, validările pe stație, iar stația păstrează doar o copie pentru citire în modul offline."),
    P("**Securizarea informațiilor în sistem**: conturi SQL cu drepturi minime, parole doar ca hash PBKDF2, blocare după 5 încercări, roluri, conexiuni criptate pe porturi nestandard, DPAPI pe stație, jurnal de audit nemodificabil, backup verificat."),

    H(2, "2.6. Riscurile, monitorizarea și eficiența economică (p. 4–6, 17)"),
    P("**Riscurile și abaterile.** Principalele riscuri au fost serverul oprit sau fără internet, vânzarea dublă la concurență, ghicirea parolelor și indisponibilitatea unui membru al echipei. Față de plan au apărut abateri: o pauză de aproape 4 săptămâni între realizare (19.09) și testare (13.10), testarea în două reprize, o iterație în plus pentru defectele D3 (backup-ul automat) și D7 (limita de autentificare) și amânarea integrării cu casa de marcat."),
    P("**Raportul de stare și performanțele intermediare.** După fiecare etapă s-a verificat ce s-a terminat: baza de date (17.09), backend-ul și serverul (18.09; 99 din 100 de teste unitare trec), interfața (19.09; 32 de ferestre), testarea (13.10 și 21–22.10), care a confirmat sistemul în ansamblu: 0 bilete duble cu 20 de stații, 11,7 bilete vândute pe secundă, dashboard-ul în 9 ms (mediana). Obiectivele sunt atinse, cu excepția securității și a backup-ului, atinse parțial (D7, D3)."),
    P("**Eficiența economică (estimare):** investiția de 62 880 MDL, cheltuieli anuale de 9 260 MDL și economii anuale de circa 84 190 MDL (timpul casierilor, rapoartele automate, erorile evitate) dau o economie netă de circa 74 930 MDL pe an, un termen de recuperare de aproximativ 10 luni și o valoare actualizată netă pe 3 ani (rata 10 %) de circa 123 450 MDL."),

    H(2, "2.7. Punerea în funcțiune și documentația (p. 24–32)"),
    ...table(["Punctul", "Ce s-a realizat"], [
      ["24. Manualul de prezentare", "ce este sistemul, pentru cine, modulele, cum funcționează și cerințele (raport, 6.3)"],
      ["25. Manualul administratorului", "instalarea serverului și a stațiilor, întreținerea, restaurarea, diagnosticarea; manualele Administratori și Programator/DevOps"],
      ["26. Manualul utilizatorului", "pașii pentru casier, administrator și pasager; manualele Operatori-casieri, Manageri și Utilizatori-pasageri"],
      ["27. Condițiile de experimentare", "copia izolată autogara_test, conturi de test, 5 instanțe ale aplicației; modelul actului despre rezultatele experimentării"],
      ["28. Procedurile manuale și automate", "omul: vânzarea, planificarea curselor, restaurarea, raportul de casă; sistemul: prețul, eliberarea rezervărilor (la minut), backup-ul, alertele de ITP, curățarea jurnalului"],
      ["29. Instruirea personalului", "pe o copie a bazei: casierii 2 ore pe grupă, dispecerii și administratorul 4 ore, contabilul și conducerea 1 oră, tehnicianul 4 ore"],
      ["30. Condițiile pentru pilotare", "2 săptămâni la un ghișeu, în paralel cu modul vechi; criterii: 0 locuri duble, vânzarea în cel mult 2 minute, backup zilnic reușit; plan de revenire și proces-verbal"],
      ["31. Verificarea performanțelor", "40 de cazuri funcționale, teste de concurență și încărcare cu 20 de stații, securitate și recuperare; 12 defecte documentate"],
      ["32. Definitivarea documentației", "raportul de grup, dicționarul de date, cele 5 manuale, scripturile SQL, codul sursă, raportul de testare și modelul actului de recepție, toate în depozitul Git"],
    ], [28, 72], { caption: "Punerea în funcțiune și documentația sistemului", size: S }),
  ];
}

module.exports = { ECHIPA, pagina1, pagini23 };
