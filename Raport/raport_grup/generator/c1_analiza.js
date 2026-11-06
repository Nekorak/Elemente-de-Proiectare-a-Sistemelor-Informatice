// Capitolul 1: analiza sistemului existent si planificarea proiectului (punctele scenariului de proiectare).
const { P, H, bullets, numbered, fig, table, startChapter } = require("./lib");

const nr = (x) => String(Math.round(x)).replace(/\B(?=(\d{3})+(?!\d))/g, " ");
const lei = (x) => nr(x) + " MDL";

// Eficienta economica: toate valorile sunt estimari (ipotezele sunt in tabelul de ipoteze).
const TARIF = 120; // MDL/ora, tariful mediu al unui dezvoltator incepator
const ore = [["analiza și documentația", 40], ["baza de date", 60], ["backend și DevOps", 90], ["interfața", 80], ["testarea", 50]];
const oreTotal = ore.reduce((a, [, h]) => a + h, 0);
const costuri = [
  ["Elaborarea sistemului", `${oreTotal} de ore × ${TARIF} MDL/oră`, oreTotal * TARIF],
  ["Calculatorul-server dedicat", "procesor de 6 nuclee, 16 GB RAM, SSD 512 GB", 14000],
  ["Sursa de alimentare neîntreruptibilă (UPS)", "protejează serverul la căderi de curent", 2500],
  ["Conectarea IP-ului fix", "taxa unică a furnizorului", 1500],
  ["Licențele software", "SQL Server Express, .NET, OpenSSH: gratuite; Windows: existent", 0],
  ["Instalarea și configurarea", "16 ore + 8 ore pentru mutarea joburilor în Task Scheduler", 24 * TARIF],
  ["Instruirea personalului", "10 ore (secțiunea 6.7)", 10 * TARIF],
  ["Pilotarea", "20 de ore de asistență în două săptămâni (secțiunea 6.8)", 20 * TARIF],
];
const investitie = costuri.reduce((a, r) => a + r[2], 0);
const exploatare = [
  ["Energia electrică a serverului", "120 W × 24 h × 365 zile ≈ 1 050 kWh × 3,2 MDL", 3360],
  ["Abonamentul pentru IP fix", "150 MDL pe lună", 1800],
  ["Întreținerea sistemului", "30 de ore pe an × 120 MDL", 3600],
  ["Discuri pentru copiile de rezervă", "înlocuire anuală", 500],
];
const costAnual = exploatare.reduce((a, r) => a + r[2], 0);
const BILETE_ZI = 300, SEC_VECHI = 170, SEC_NOU = 65, SALARIU_ORA = 60, VALORIFICAT = 0.3;
const oreCasieri = ((SEC_VECHI - SEC_NOU) * BILETE_ZI * 365) / 3600;
const economii = [
  ["Timpul casierilor", `(${SEC_VECHI} − ${SEC_NOU}) s × ${BILETE_ZI} bilete × 365 zile ≈ ${nr(oreCasieri)} de ore; se valorifică doar ${VALORIFICAT * 100} %, × ${SALARIU_ORA} MDL/oră`, oreCasieri * VALORIFICAT * SALARIU_ORA],
  ["Rapoartele pentru contabilitate și conducere", "5 ore pe săptămână × 52 × 75 MDL/oră", 5 * 52 * 75],
  ["Erorile evitate (locuri vândute de două ori, greșeli de casă)", "3 cazuri pe lună × 200 MDL", 3 * 12 * 200],
];
const economieAnuala = economii.reduce((a, r) => a + r[2], 0);
const net = economieAnuala - costAnual;
const RATA = 0.1;
const van = [1, 2, 3].reduce((a, t) => a + net / Math.pow(1 + RATA, t), 0) - investitie;
const recuperareLuni = (investitie / net) * 12;
const roi3 = ((3 * net - investitie) / investitie) * 100;

module.exports = () => {
  startChapter(1);
  return [
    H(1, "1. ANALIZA SISTEMULUI EXISTENT ȘI PLANIFICAREA PROIECTULUI"),
    P("Capitolul descrie partea de analiză și de organizare a proiectului, înainte de proiectarea propriu-zisă: obiectivele sistemului, firma beneficiară și sistemul ei informațional existent, etapele și metodologia de lucru, caietul de sarcini, intrările și ieșirile sistemului, riscurile și eficiența economică. Capitolul a fost elaborat de toată echipa."),
    P("Beneficiarul ales este o autogară regională tipică din Republica Moldova, nu o firmă anume. De aceea, cifrele despre personal, volumul vânzărilor, timpii de lucru și costuri sunt **estimări ale echipei**, făcute pe baza modului obișnuit de lucru al unei autogări și a datelor din proiect; ele sunt marcate ca estimări în tabele. Valorile măsurate (de exemplu performanțele la testare) provin din capitolele 2–5."),

    // ---------------- 1.1 Obiective ----------------
    H(2, "1.1. Obiectivele sistemului informatic"),
    P("Scopul general al sistemului este automatizarea vânzării biletelor și a evidenței curselor într-o autogară cu mai multe ghișee, cu date consistente atunci când mai mulți casieri lucrează simultan. Din acest scop rezultă obiectivele de mai jos. Fiecare are un indicator care poate fi verificat, iar coloana a treia arată unde a fost verificat."),
    ...table(["Nr.", "Obiectiv", "Indicator", "Verificat în"], [
      ["O1", "Vânzarea rapidă a biletului cu alegerea locului", "o vânzare durează cel mult 1–2 minute la ghișeu; confirmarea pe server sub 1 s în condiții normale", "4.17, 5.4 (S2)"],
      ["O2", "Niciun loc vândut de două ori", "0 bilete duble cu până la 20 de stații care vând simultan același loc", "5.4 (S1–S3)"],
      ["O3", "Rezervarea provizorie a locului", "locul ales e blocat pentru ceilalți casieri și eliberat automat după expirare", "2.8, 5.4 (S5)"],
      ["O4", "Anularea cu rambursare după reguli clare", "100 % cu cel puțin 24 h înainte, 50 % cu cel puțin 2 h, altfel 0 %", "2.4, 5.3.3"],
      ["O5", "Rapoarte imediate pentru conducere și contabilitate", "vânzări pe zi și pe interval, ocupare, comparație lunară, export CSV", "4.15, 5.3.5"],
      ["O6", "Evidența flotei și a ITP-ului", "alertă automată pentru ITP-ul care expiră în 14 zile; autobuzul cu ITP expirat trece în Service", "2.8"],
      ["O7", "Securitatea datelor", "conturi cu drepturi minime, parole PBKDF2, blocare după 5 încercări, jurnal de audit nemodificabil", "2.7, 3.6.1, 5.6"],
      ["O8", "Păstrarea datelor", "backup zilnic verificat și restaurare testată", "2.9, 5.2, 5.7"],
      ["O9", "Lucrul de pe mai multe stații prin internet", "acces criptat, pe porturi nestandard, fără VPN", "3.8.4, 5.3.6"],
    ], [7, 30, 43, 20], { caption: "Obiectivele sistemului și indicatorii lor" }),
    P("**Clasificarea sistemului informatic.** Sistemul informatic este partea automatizată a sistemului informațional al firmei: el culege, păstrează, prelucrează și transmite cu ajutorul calculatorului o parte din informațiile de care firma are nevoie [[Lungu]]. După criteriile obișnuite de clasificare, sistemul proiectat se încadrează astfel:"),
    ...bullets([
      "**după domeniul de activitate:** sistem informatic de gestiune pentru transportul rutier de pasageri (evidența curselor, a biletelor, a încasărilor și a flotei);",
      "**după nivelul de conducere deservit:** sistem operațional (tranzacțional) la nivelul ghișeelor, cu rapoarte de sinteză pentru nivelul tactic (conducerea autogării); nu este un sistem de asistare a deciziilor;",
      "**după modul de prelucrare a datelor:** prelucrare în timp real, tranzacțională (OLTP): fiecare rezervare, vânzare și anulare se înregistrează imediat în baza de date, iar joburile automate fac prelucrări periodice (eliberarea rezervărilor, backup-ul, alertele);",
      "**după arhitectură:** sistem client–server, multiutilizator, cu o bază de date centralizată și componente distribuite pe stații (secțiunea 3.9);",
      "**după gradul de automatizare:** sistem parțial automatizat, pentru că emiterea bonului fiscal, contabilitatea și anunțarea curselor rămân în afara lui (secțiunea 1.3.7);",
      "**după modul de realizare:** sistem realizat la comandă (produs-program propriu), nu un pachet de programe cumpărat și adaptat.",
    ]),

    // ---------------- 1.2 Firma ----------------
    H(2, "1.2. Firma beneficiară: serviciile și resursele economice"),
    P("Autogara luată ca model deservește cursele interurbane ale mai multor transportatori. Ea nu deține autobuze: transportatorii își aduc autobuzele și șoferii, iar autogara le oferă peroanele, vânzarea biletelor și informarea pasagerilor. Serviciile și resursele de mai jos sunt estimate pentru o autogară cu trei ghișee."),
    ...table(["Serviciul", "Cui se adresează", "Sursa de venit pentru autogară"], [
      ["Vânzarea biletelor la ghișeu (cu alegerea locului)", "pasagerilor", "comisionul din prețul biletului"],
      ["Rezervarea și vânzarea online a biletelor", "pasagerilor cu cont", "comisionul din prețul biletului"],
      ["Informarea despre orar, prețuri și locuri libere", "pasagerilor", "inclus în serviciile de autogară"],
      ["Accesul autobuzelor la peroane, anunțarea curselor", "transportatorilor", "taxa de peron pe cursă"],
      ["Evidența curselor, a autobuzelor și a ITP-ului", "transportatorilor și conducerii", "inclus în contractul cu transportatorul"],
      ["Sala de așteptare, păstrarea bagajelor", "pasagerilor", "taxa pentru bagaje, chiria spațiilor comerciale"],
    ], [38, 24, 38], { caption: "Serviciile autogării (estimare)" }),
    ...table(["Resursa", "Situația estimată"], [
      ["Personalul", "director (1), dispeceri (2, pe ture), casieri (6: trei ghișee în două ture), contabil (1), tehnician pentru calculatoare (1, cu jumătate de normă)"],
      ["Spațiile", "clădirea cu sala de așteptare, 3 ghișee, 8 peroane"],
      ["Echipamentele", "3 calculatoare la ghișee și unul la dispecerat (Windows), imprimante, case de marcat la fiecare ghișeu, panoul de afișare a orarului"],
      ["Rețeaua", "conexiune la internet prin router, rețea locală între ghișee"],
      ["Volumul activității", "aproximativ 300 de bilete pe zi și 40–50 de curse pe zi"],
      ["Bugetul pentru informatizare", "limitat; se preferă soluții fără licențe costisitoare (secțiunea 1.8)"],
    ], [24, 76], { caption: "Resursele autogării (estimare)" }),

    // ---------------- 1.3 Studiul sistemului existent ----------------
    H(2, "1.3. Studiul și analiza sistemului informațional existent"),
    P("Studiul sistemului existent este primul document al etapei de analiză. El descrie cum circulă astăzi informația în autogară, cum se colectează datele, ce probleme apar și cum sunt protejate informațiile, iar la final formulează cerințele pentru sistemul nou. Secțiunile 1.3.1–1.3.6 alcătuiesc acest document."),

    H(3, "1.3.1. Fluxul informațional al firmei"),
    P("În situația existentă, informația circulă în cea mai mare parte pe hârtie. Transportatorul aduce orarul, datele autobuzelor și ale șoferilor și actele de ITP la dispecer. Dispecerul ține orarul (pe hârtie sau într-un fișier Excel) și registrul autobuzelor. Casierul copiază din orar cursa și prețul, completează biletul tipizat și trece vânzarea în registrul casei. La sfârșitul zilei, din registru se face raportul de casă pentru contabilitate, iar lunar totalurile sunt recopiate în Excel pentru conducere. Dacă două ghișee vând aceeași cursă, casierii verifică locurile libere telefonic, prin dispecer."),
    ...fig("AN01_flux_existent", "Fluxul informațional existent al autogării (H1–H3: registrele pe hârtie sau în Excel)", { maxW: 640 }),
    P("Diagrama de context a sistemului nou (Figura 1.2) arată aceleași entități externe, dar toate fluxurile trec printr-un singur sistem informatic. Notația folosită este Gane–Sarson: dreptunghiurile cu chenar gros sunt entitățile externe, dreptunghiurile rotunjite sunt procesele, iar dreptunghiurile deschise sunt depozitele de date [[DataFlow]]."),
    ...fig("AN02_dfd_context", "Diagrama de context (DFD, nivelul 0) a sistemului informatic al autogării", { maxW: 640 }),

    H(3, "1.3.2. Metodele de colectare a informațiilor"),
    P("Pentru studiul autogării s-au folosit metodele obișnuite ale analizei de sistem. Tabelul arată ce se află cu fiecare metodă și la ce etapă este folosită."),
    ...table(["Metoda", "Ce se află", "Etapa"], [
      ["Interviul cu casierul și cu dispecerul", "pașii vânzării, documentele folosite, problemele zilnice", "analiza"],
      ["Observarea directă la ghișeu (fișa de observare)", "durata fiecărui pas, numărul de documente, erorile", "analiza"],
      ["Analiza documentelor", "conținutul biletului, al orarului, al raportului de casă și al registrului ITP", "analiza, proiectarea de detaliu"],
      ["Chestionarul pentru pasageri", "cum află orarul, dacă ar cumpăra biletul online", "analiza"],
      ["Studiul aplicațiilor similare", "funcțiile obișnuite ale sistemelor de vânzare a biletelor", "proiectarea de ansamblu"],
      ["Discuțiile în echipă și Sarcina tehnică", "împărțirea pe compartimente, deciziile tehnice", "proiectarea de ansamblu și de detaliu"],
      ["Istoricul Git, rezultatele testelor, jurnalele aplicației", "starea reală a lucrului, defectele, performanțele", "realizarea, testarea, punerea în funcțiune"],
    ], [32, 46, 22], { caption: "Metodele de colectare a informațiilor" }),

    H(3, "1.3.3. Observarea colectării rapide de informații"),
    P("Fișa de observare urmărește o vânzare obișnuită la ghișeu, pas cu pas. Duratele pentru situația existentă sunt estimate; cele pentru sistemul nou sunt estimate după pașii exemplului de vânzare din secțiunea 4.17."),
    ...table(["Pasul vânzării", "Situația existentă", "Durata (estimare)", "Sistemul nou", "Durata (estimare)"], [
      ["Găsirea cursei", "căutare în orarul tipărit", "30 s", "stațiile și data, butonul Caută", "10 s"],
      ["Verificarea locurilor libere", "registrul casei; telefon la dispecer pentru alt ghișeu", "40 s", "schema autobuzului, cu locurile colorate după stare", "5 s"],
      ["Completarea biletului", "de mână, pe biletul tipizat", "50 s", "numele și telefonul pasagerului, reducerea", "20 s"],
      ["Încasarea", "numerar, calculul restului", "30 s", "numerar sau card; totalul calculat automat", "30 s"],
      ["Înregistrarea vânzării", "rând nou în registrul casei", "20 s", "automat la confirmare (F2)", "0 s"],
      ["**Total**", "", "**≈ 170 s**", "", "**≈ 65 s**"],
    ], [20, 24, 12, 30, 14], { caption: "Fișa de observare a unei vânzări la ghișeu" }),

    H(3, "1.3.4. Problemele de colectare a datelor"),
    ...bullets([
      "aceeași informație (cursa, prețul, locul) este scrisă de mai multe ori: în orar, pe bilet, în registru și apoi în Excel, deci apar greșeli de transcriere;",
      "casierii nu văd în timp real locurile vândute la alte ghișee, așa că un loc poate fi vândut de două ori;",
      "raportul de casă și totalurile lunare se fac manual, cu întârziere de o zi sau de o lună;",
      "evidența ITP-ului este separată de orar, deci o cursă poate fi planificată cu un autobuz care are ITP-ul expirat;",
      "anularea și rambursarea se calculează de mână, fără o regulă aplicată la fel de toți casierii;",
      "nu se știe cine a făcut o anumită operație, pentru că registrele nu păstrează autorul.",
    ]),

    H(3, "1.3.5. Accesul și securizarea informațiilor în situația existentă"),
    P("Tabelul pune alături problemele de acces și securitate observate de obicei într-o autogară fără sistem centralizat și soluția aleasă în sistemul nou."),
    ...table(["Situația existentă", "Riscul", "Soluția în sistemul nou"], [
      ["un singur cont Windows, cu parola știută de toți casierii", "nu se poate afla cine a lucrat", "cont personal pentru fiecare utilizator, cu rol (2.7, 3.6.1)"],
      ["registrele stau în sertarul ghișeului", "pierderea sau modificarea lor", "datele numai în baza de date, cu jurnal de audit nemodificabil (2.7)"],
      ["fișierul Excel e pe un singur calculator, fără copie", "pierderea datelor la defectarea discului", "backup zilnic și restaurare testată (2.9, 5.7)"],
      ["parolele sunt scrise pe hârtie lângă calculator", "acces neautorizat", "parole PBKDF2, politica de parole, blocare după 5 încercări (3.6.1)"],
      ["datele pasagerilor (nume, telefon) rămân pe hârtie", "încălcarea protecției datelor personale", "acces doar pe roluri, conexiune criptată, contul aplicației fără drepturi de administrare (2.7, 1.5.2)"],
    ], [32, 26, 42], { caption: "Accesul și securizarea informațiilor: situația existentă și soluția" }),

    H(3, "1.3.6. Necesitățile informaționale ale beneficiarului"),
    ...table(["Utilizatorul", "Informația necesară", "Când"], [
      ["Casierul", "cursele dintre două stații, locurile libere în timp real, prețul cu reducerea, biletele vândute de el", "la fiecare vânzare"],
      ["Dispecerul / administratorul", "orarul, traseele, autobuzele disponibile, șoferii, ITP-ul care expiră, mentenanța", "zilnic"],
      ["Conducerea", "vânzările pe zi, pe traseu și pe casier, gradul de ocupare, comparația cu luna precedentă, jurnalul operațiilor", "zilnic și lunar"],
      ["Contabilitatea", "încasările pe metode de plată, rambursările, numerele bonurilor fiscale", "zilnic"],
      ["Pasagerul", "cursele, prețul, locurile libere, biletele proprii", "la cerere"],
      ["Transportatorul", "gradul de ocupare al curselor sale", "lunar"],
    ], [22, 58, 20], { caption: "Necesitățile informaționale pe utilizatori" }),
    P("**Concluzia studiului.** Sistemul existent nu poate garanta un loc vândut o singură dată, produce rapoarte cu întârziere și nu protejează datele. Sistemul nou trebuie să aibă o singură bază de date pentru toate ghișeele, operații de vânzare tranzacționale, roluri și jurnal de audit, rapoarte automate și copii de rezervă zilnice. Aceste cerințe stau la baza caietului de sarcini (secțiunea 1.5)."),

    H(3, "1.3.7. Aria de acoperire a sistemului informațional de către sistemul informatic"),
    P("Sistemul informațional cuprinde toate informațiile și activitățile autogării, inclusiv cele făcute de mână sau în alte programe. Sistemul informatic proiectat acoperă doar o parte din el: activitățile legate de curse, de vânzarea biletelor și de evidența flotei. Schema de mai jos arată ce este automatizat complet, ce este automatizat doar parțial și ce rămâne în afara sistemului."),
    ...fig("AN05_aria_acoperire", "Aria de acoperire a sistemului informațional al autogării de către sistemul informatic", { maxW: 640 }),
    P("Din cele 19 activități identificate, 12 sunt acoperite complet, 2 parțial (încasarea fiscală și ITP-ul) și 5 rămân în afara sistemului. Acoperirea parțială are motive clare: bonul fiscal trebuie emis de casa de marcat înregistrată la fisc, iar integrarea cu ea a fost amânată (1.5.2); controlul tehnic al autobuzelor este făcut de transportator, iar autogara are nevoie doar de data expirării și de alertă. Activitățile din afara sistemului nu țin de vânzarea biletelor și pot fi adăugate în versiunile următoare, de exemplu exportul raportului zilnic direct în programul de contabilitate."),

    // ---------------- 1.4 Etape si metodologie ----------------
    H(2, "1.4. Etapele proiectării și metodologia"),
    H(3, "1.4.1. Ciclul de viață al sistemului informatic"),
    P("Ciclul de viață al unui sistem informatic cuprinde toate fazele prin care trece sistemul, de la ideea de a-l realiza până la înlocuirea lui [[Lungu]]. Fazele se repetă: după o perioadă de exploatare apar cerințe noi, care pornesc o nouă analiză și o versiune nouă a sistemului. Figura arată fazele și unde se află fiecare în proiectul nostru."),
    ...fig("AN06_ciclu_viata", "Ciclul de viață al sistemului informatic și fazele parcurse în proiect", { maxW: 600 }),
    P("Proiectul a parcurs complet primele patru faze. Faza a cincea este făcută parțial: testarea este încheiată (capitolul 5), iar experimentarea la beneficiar, instruirea și recepția sunt pregătite ca documente și planuri (capitolul 6), pentru că autogara este un beneficiar model. Exploatarea și mentenanța au proceduri scrise în manualul administratorului (6.4), iar dezvoltările propuse pentru o versiune nouă sunt enumerate în concluzii. Etapele concrete ale realizării, de la analiză la punerea în funcțiune, sunt descrise în secțiunea următoare."),
    H(3, "1.4.2. Etapele și activitățile"),
    P("Proiectul a parcurs șase etape. Perioadele sunt cele din istoricul Git (diagrama Gantt din introducere); pentru etapele de analiză și proiectare, data este cea a primului document salvat în depozit."),
    ...fig("AN04_etape_cascada", "Etapele proiectării sistemului după modelul cascadă", { maxW: 600 }),
    ...table(["Etapa", "Activitățile", "Rezultatul", "Responsabil", "Metoda de colectare"], [
      ["1. Analiza", "studiul autogării, nevoile utilizatorilor, alegerea tehnologiilor", "studiul sistemului existent, Sarcina tehnică", "echipa; Hanganu Sergiu", "interviul, observarea, analiza documentelor, chestionarul"],
      ["2. Proiectarea de ansamblu", "arhitectura client–server, împărțirea pe compartimente, planul tehnic", "schema de ansamblu (Figura 3.1), planul tehnic", "Hanganu Sergiu", "studiul aplicațiilor similare, discuții în echipă"],
      ["3. Proiectarea de detaliu", "schema bazei de date, straturile .NET, ferestrele, formatul JSON", "diagrama relațiilor (Figura 2.4), DFD (Figura 1.8)", "fiecare responsabil de compartiment", "analiza documentelor (biletul, orarul)"],
      ["4. Realizarea", "scripturile SQL, backend-ul, interfața, serverul", "baza autogara, aplicația, serverul de fișiere", "toți patru", "istoricul Git, testele automate"],
      ["5. Testarea", "testarea funcțională, de concurență, de securitate și de recuperare", "raportul de testare, defectele D1–D12", "Sopivnic Maxim", "cazurile de test, programul de stres, jurnalele"],
      ["6. Punerea în funcțiune", "datele de volum, configurarea stațiilor, manualele, instruirea, pilotarea", "documentația (capitolul 6)", "echipa", "rezultatele testelor, observarea în pilotare"],
    ], [16, 26, 22, 14, 22], { caption: "Etapele proiectării, activitățile și metodele de colectare a informațiilor" }),

    H(3, "1.4.3. Alegerea metodologiei"),
    P("Au fost comparate patru modele de dezvoltare [[Sommerville]]. Criteriile au fost cele care contau pentru proiect: cerințe cunoscute de la început, echipă mică, timp scurt și dependența puternică dintre compartimente (backend-ul are nevoie de procedurile bazei de date, interfața de serviciile backend-ului)."),
    ...table(["Criteriul", "Cascada", "Agil (Scrum)", "Spirala", "RAD"], [
      ["Cerințele sunt fixate de la început", "potrivit", "nu e necesar", "potrivit", "parțial"],
      ["Echipă de 4 studenți, fără client permanent", "potrivit", "greu (cere clientul la fiecare iterație)", "prea complex", "cere utilizatori disponibili"],
      ["Compartimente care depind unul de altul", "potrivit (ordine clară)", "posibil, cu mai multă coordonare", "posibil", "slab"],
      ["Documentația cerută la disciplină", "completă, pe etape", "redusă", "completă", "redusă"],
      ["Analiza riscurilor", "la început", "continuă", "foarte bună", "slabă"],
      ["Schimbarea cerințelor pe parcurs", "costisitoare", "ușoară", "posibilă", "ușoară"],
    ], [28, 18, 20, 16, 18], { caption: "Compararea metodologiilor de dezvoltare" }),
    P("S-a ales **modelul cascadă**: cerințele erau fixate în Sarcina tehnică, iar fiecare compartiment avea nevoie ca cel dinainte să fie terminat. Baza de date a fost gata prima, backend-ul s-a construit peste procedurile ei, iar interfața a folosit doar serviciile backend-ului. Dezavantajul cascadei, costul mare al schimbărilor târzii, s-a văzut la testare: defectele D3 și D7 cer întoarcerea la etapa de realizare. Pentru a-l reduce, s-au folosit metode care fac schimbările mai ieftine: modelarea entitate–relație înainte de cod, arhitectura pe straturi (o schimbare în baza de date nu atinge interfața) și controlul versiunilor în Git."),

    H(3, "1.4.4. Fazele procesului managerial al proiectării"),
    P("Conducerea proiectului a urmat fazele obișnuite ale procesului managerial: planificarea, organizarea, coordonarea, controlul și evaluarea cu corectarea abaterilor [[Andronie]]. Tabelul arată, pentru fiecare fază, ce s-a făcut în proiect și unde se găsește dovada."),
    ...table(["Faza managerială", "Conținutul fazei", "Ce s-a făcut în proiect", "Dovada"], [
      ["Planificarea", "obiectivele, etapele, termenele, resursele și costurile", "obiectivele măsurabile, Sarcina tehnică, cele șase etape, calendarul lucrărilor, estimarea costurilor", "1.1, 1.4.2, diagrama Gantt, 1.8, Sarcina Tehnica.docx"],
      ["Organizarea", "împărțirea lucrărilor, a rolurilor și a resurselor", "patru compartimente cu câte un responsabil; depozitul Git comun; serverul echipei; câte un cont SQL personal pentru fiecare membru", "Tabelul 1 din introducere, 2.7, 3.8"],
      ["Coordonarea", "legătura dintre membri și dintre compartimente, deciziile comune", "coordonarea tehnică (Hanganu Sergiu); interfețe fixate între subsisteme: procedurile stocate și clasa AutogaraBackend; convențiile comune ale bazei de date", "2.1, 3.1, 1.4.5"],
      ["Controlul", "compararea stării reale cu planul", "rapoartele de stare pe etape; testele automate la fiecare compilare; testarea pe cazuri de test; registrul defectelor D1–D12", "6.2, 3.7, capitolul 5, 5.8"],
      ["Evaluarea și corectarea", "analiza abaterilor și măsurile de corectare", "analiza riscurilor și a abaterilor de la cascadă; remedierea defectelor D3 și D7 planificată înainte de pilotare", "1.7, 6.8, 6.10"],
    ], [16, 24, 40, 20], { caption: "Fazele procesului managerial al proiectării sistemului informatic", size: 20 }),

    H(3, "1.4.5. Proiectarea eșalonată pe subsisteme"),
    P("Sistemul a fost împărțit în subsisteme care pot fi proiectate, realizate și verificate separat, apoi asamblate. Împărțirea s-a făcut după dependențe: un subsistem începe după ce subsistemul pe care îl folosește are interfața fixată. Astfel, compartimentele au putut lucra în paralel, iar o schimbare în interiorul unui subsistem nu a cerut schimbări în celelalte. Figura arată cele cinci eșaloane și ordinea dintre ele."),
    ...fig("AN08_subsisteme", "Proiectarea eșalonată a sistemului pe subsisteme și componente", { maxW: 640 }),
    ...table(["Eșalonul", "Subsistemul", "Interfața prin care e folosit", "Criteriul de terminare"], [
      ["I", "Baza de date (schema autogara)", "procedurile stocate, viziunile, rolurile SQL", "scripturile 01–08 rulează fără erori; constrângerile verificate"],
      ["I", "Domain și Common", "clasele entităților, setările stației, jurnalul", "proiectele compilează; testele unitare trec"],
      ["I", "Serverul (DevOps)", "porturile 14330 și 44545, conturile", "conectarea reușită din internet de pe toate laptopurile"],
      ["II", "DataAccess", "AutogaraDbContext și metodele ProceduriStocate", "fiecare procedură apelată cu succes din cod"],
      ["II", "FileServer", "citirea și scrierea fișierelor JSON prin SFTP, cu validare", "fișierele autobuzelor și ale hărții citite și salvate"],
      ["II", "Joburile și backup-ul", "joburile SQL Server Agent, scriptul de restaurare", "joburile rulează după program; restaurarea testată"],
      ["III", "Business", "clasa AutogaraBackend cu serviciile ei", "regulile de preț, rambursare, roluri și parole acoperite de teste"],
      ["IV", "Interfața WinForms", "ferestrele aplicației", "toate funcțiile rolurilor accesibile din meniu"],
      ["V", "Testarea și punerea în funcțiune", "raportul de testare, manualele, documentația", "cazurile de test executate; documentația predată"],
    ], [10, 26, 34, 30], { caption: "Subsistemele, interfețele lor și criteriile de terminare", size: 20 }),
    P("Eșalonarea a avut două avantaje vizibile. Primul: baza de date, cu procedurile ei tranzacționale, a fost gata înainte de backend, deci regulile de concurență au fost scrise o singură dată, în SQL Server. Al doilea: interfața a folosit doar clasa AutogaraBackend, așa că ferestrele au putut fi construite fără să cunoască tabelele. Dezavantajul este că un defect găsit târziu într-un eșalon de jos (de exemplu D7, în autentificare) cere verificarea tuturor eșaloanelor de deasupra."),

    // ---------------- 1.5 Caietul de sarcini ----------------
    H(2, "1.5. Caietul de sarcini"),
    P("Caietul de sarcini pornește de la documentul „Sarcina Tehnica.docx” din depozitul proiectului și îl completează cu cerințele tehnice și legislative și cu drepturile și obligațiile părților."),
    H(3, "1.5.1. Cerințele tehnice"),
    ...table(["Domeniul", "Cerința"], [
      ["Arhitectura", "aplicație desktop pe stațiile de lucru (client) și un calculator-server cu MS SQL Server și server de fișiere SFTP, accesibil prin internet"],
      ["Serverul", "Windows 10/11 sau Windows Server, SQL Server 2019 sau mai nou; în proiect s-a folosit ediția Developer, iar la autogară se folosește ediția Express (fără licență), cu joburile mutate în Task Scheduler, sau ediția Standard; 16 GB RAM, SSD, UPS"],
      ["Stațiile de lucru", "Windows 10/11 pe 64 de biți, .NET 10 Desktop Runtime, conexiune la internet"],
      ["Rețeaua", "IP fix sau DDNS; porturile 14330 (SQL Server) și 44545 (SFTP) redirecționate spre server; firewall-ul deschis doar pentru aceste porturi"],
      ["Securitatea", "conexiuni criptate; conturi SQL cu drepturi minime; parole păstrate doar ca hash PBKDF2; blocarea contului după 5 încercări greșite în 15 minute; jurnal de audit"],
      ["Concurența și performanța", "un loc nu poate fi vândut de două ori, cu cel puțin 20 de stații simultane; vânzarea confirmată în cel mult câteva secunde"],
      ["Disponibilitatea", "backup complet zilnic, backup diferențial în timpul programului, restaurare verificată; în lipsa serverului, aplicația afișează ultimele date salvate local"],
      ["Interfața", "în limba română, cu schema autobuzului și harta stațiilor desenate; meniul arată doar funcțiile rolului"],
    ], [22, 78], { caption: "Cerințele tehnice" }),
    H(3, "1.5.2. Cerințele legislative"),
    ...bullets([
      "**Protecția datelor cu caracter personal** (Legea nr. 133/2011 [[Lege133]]): sistemul păstrează numele și telefonul pasagerilor și datele utilizatorilor. Datele se colectează doar pentru vânzarea biletului, accesul este limitat pe roluri, conexiunile sunt criptate, iar jurnalul de audit arată cine a accesat sau a modificat datele. Jurnalul se păstrează 12 luni (jobul de curățare din secțiunea 2.8).",
      "**Transportul rutier de pasageri** (Codul transporturilor rutiere [[CodTransport]] și regulamentul transporturilor auto de călători și bagaje): biletul este documentul de călătorie al pasagerului și trebuie să conțină cursa, data, locul și prețul; aceste câmpuri sunt obligatorii în tabelele Bilete și Curse.",
      "**Evidența fiscală a încasărilor**: plățile în numerar se fac cu bon fiscal emis de casa de marcat. Sistemul păstrează numărul bonului fiscal la fiecare plată, dar nu este integrat cu casa de marcat; integrarea rămâne o cerință pentru versiunea următoare.",
      "**Drepturile de autor asupra programelor**: toate componentele folosite (SQL Server Express, .NET, OpenSSH, bibliotecile NuGet) au licențe care permit folosirea gratuită în firmă.",
    ]),
    H(3, "1.5.3. Sarcinile pentru proiectarea detaliată"),
    P("Sarcina tehnică a stabilit sarcinile fiecărui compartiment. Tabelul le reia pe scurt și arată starea lor la data raportului."),
    ...table(["Compartimentul", "Sarcina", "Starea"], [
      ["Baza de date", "convențiile, schema și tabelele, rolurile și securitatea, viziunile, procedurile stocate, indicii, scriptul final și datele de test", "realizat (capitolul 2)"],
      ["Baza de date", "joburile SQL Server Agent", "realizat; backup-ul complet automat nu rulează (D3)"],
      ["Baza de date", "planul de backup și restaurarea testată", "realizat (2.9, 5.2)"],
      ["Baza de date", "dicționarul de date", "realizat în acest raport (6.9)"],
      ["Backend", "accesul la date, autentificarea pe roluri și recuperarea parolei, serviciile hărții, fișierele JSON, clientul SFTP", "realizat (capitolul 3)"],
      ["Backend", "rezervarea, vânzarea, anularea, prețurile și reducerile, rapoartele, modul offline, jurnalul", "realizat (capitolul 3)"],
      ["Backend", "limitarea încercărilor de autentificare", "realizat; poate fi ocolită prin încercări simultane (D7)"],
      ["Backend", "integrarea cu casa de marcat fiscală", "nerealizat; se păstrează doar numărul bonului"],
      ["Frontend", "configurarea stației, autentificarea, editoarele hărții și autobuzului, vânzarea, formularele de administrare, dashboard-ul, rapoartele, validările", "realizat (capitolul 4)"],
      ["Frontend", "indicatorul de conexiune și actualizarea automată", "indicatorul realizat; actualizarea pregătită în cod, nicio versiune publicată încă"],
      ["Testarea", "planul pe roluri, editoarele, vânzarea, concurența, conexiunea, securitatea, rapoartele, joburile, backup-ul", "realizat (capitolul 5)"],
      ["Testarea", "testarea de regresie înainte de livrare", "în curs: se repetă după remedierea D3 și D7"],
    ], [18, 56, 26], { caption: "Sarcinile din Sarcina tehnică și starea lor" }),
    H(3, "1.5.4. Drepturile și obligațiile beneficiarului și ale executantului"),
    ...table(["", "Beneficiarul (autogara)", "Executantul (echipa de proiect)"], [
      ["Drepturi", "să primească sistemul conform caietului de sarcini și la termen; să ceară remedierea defectelor în perioada de garanție; să refuze recepția dacă criteriile de acceptare nu sunt îndeplinite; să primească toată documentația și codul sursă", "să primească datele și accesul necesar (orar, trasee, autobuze, spațiu pentru server); să ceară clarificări la cerințe; să propună modificări ale cerințelor, cu acordul beneficiarului; să fie plătit pentru etapele recepționate"],
      ["Obligații", "să furnizeze datele inițiale și o persoană de contact; să asigure serverul, rețeaua și stațiile conform cerințelor tehnice; să participe la instruire și pilotare; să recepționeze fiecare etapă în termenul stabilit", "să livreze sistemul, manualele și scripturile de instalare; să respecte confidențialitatea datelor pasagerilor; să instruiască personalul; să remedieze defectele găsite în pilotare și în garanție; să predea parolele de administrare doar persoanei desemnate"],
    ], [12, 44, 44], { caption: "Drepturile și obligațiile părților" }),
    P("Relația dintre beneficiar și executant nu se reduce la contract: pe toată durata proiectului între cele două părți circulă cerințe, date, versiuni ale sistemului și documente. Diagrama arată cine participă din fiecare parte și ce trece de la una la alta; recepția fiecărei etape se face pe baza actelor din secțiunea 6.11."),
    ...fig("AN07_beneficiar_executant", "Relația beneficiarului și a executantului sistemului informatic", { maxW: 640 }),

    // ---------------- 1.6 Intrari si iesiri ----------------
    H(2, "1.6. Intrările și ieșirile sistemului"),
    P("Diagrama fluxurilor de date de nivelul 1 descompune sistemul în șase procese. Ea arată de unde vin intrările, în ce depozite de date ajung și ce ieșiri primesc utilizatorii. Depozitele D1–D6 corespund tabelelor din capitolul 2, iar D7 fișierelor de pe serverul SFTP."),
    ...fig("AN03_dfd_nivel1", "Diagrama fluxurilor de date (DFD, nivelul 1) a sistemului nou (figura este rotită)", { maxW: 640, maxH: 880 }),
    P("Intrările sunt definite pornind de la ieșiri: fiecare ieșire din Tabelul 1.16 trebuie să poată fi obținută din datele introduse. Volumele sunt estimări pentru autogara-model, cu 300 de bilete și 40–50 de curse pe zi (Tabelul 1.3)."),
    ...table(["Intrarea", "Sursa", "Unde se introduce", "Frecvența", "Volumul (estimare)", "Validarea"], [
      ["Datele pasagerului (nume, telefon)", "pasagerul", "fereastra de vânzare", "la fiecare bilet", "≈ 300 pe zi", "nume obligatoriu, telefon în format moldovenesc"],
      ["Cursa, locul ales, reducerea, metoda de plată, nr. bonului", "casierul", "căutarea curselor, schema autobuzului", "la fiecare bilet", "≈ 300 pe zi", "locul liber sau rezervat de același casier; procentul reducerii 0–100"],
      ["Cererea de anulare (codul biletului)", "pasagerul, casierul", "secțiunea Bilete", "la cerere", "≈ 10–15 pe zi", "biletul activ, cursa neplecată"],
      ["Stațiile, nodurile și distanțele hărții", "administratorul", "editorul hărții", "rar", "câteva pe an", "distanța pozitivă, noduri diferite"],
      ["Traseele și opririle", "transportatorul, prin administrator", "secțiunea Trasee", "la schimbarea orarului", "câteva pe lună", "denumire unică, ordinea opririlor unică"],
      ["Cursele (traseu, autobuz, șofer, data, orele, prețul)", "transportatorul, prin administrator", "secțiunea Curse", "zilnic sau săptămânal", "≈ 300 pe săptămână", "sosirea după plecare, preț nenegativ"],
      ["Autobuzele, schema locurilor, ITP-ul, mentenanța", "transportatorul", "secțiunile Autobuze și Mentenanță, editorul autobuzului", "la schimbări", "câteva pe lună", "număr de înmatriculare unic, capacitate pozitivă, fișierul JSON validat"],
      ["Șoferii", "transportatorul", "secțiunea Șoferi", "la schimbări", "câteva pe lună", "număr de permis unic"],
      ["Conturile utilizatorilor și parolele", "conducerea, prin administrator", "secțiunea Utilizatori", "la angajare", "câteva pe an", "nume unic, politica de parole"],
    ], [20, 14, 18, 12, 12, 24], { caption: "Intrările sistemului informatic", size: 18 }),
    P("Ieșirile sunt definite după tipologia lor: documente pentru pasager (biletul), situații operative (pentru lucrul zilnic la ghișeu), situații de sinteză (pentru conducere și contabilitate), semnalări (alerte) și ieșiri de arhivare. Pentru fiecare se precizează scopul, destinatarul, forma, volumul, frecvența și modul de obținere."),
    ...table(["Ieșirea", "Tipul", "Scopul", "Destinatarul", "Forma", "Volumul (estimare)", "Frecvența", "Modul de obținere"], [
      ["Biletul (cod unic, cursa, locul, prețul)", "document", "dovada călătoriei", "pasagerul", "pe ecran, în „Biletele mele”", "≈ 300 pe zi", "la fiecare vânzare", "sp_ConfirmaVanzareBilet"],
      ["Suma rambursată", "document", "plata înapoi la anulare", "pasagerul, contabilitatea", "pe ecran; plată cu statusul Rambursată", "≈ 10–15 pe zi", "la anulare", "sp_AnuleazaBilet, după regula 100/50/0 %"],
      ["Lista curselor cu locurile libere", "situație operativă", "alegerea cursei și a locului", "casierul, pasagerul", "tabel în aplicație", "40–50 de rânduri pe zi", "la cerere", "vw_CurseActive, vw_LocuriDisponibile"],
      ["Dashboard-ul zilei", "situație operativă", "urmărirea vânzărilor în timpul zilei", "casierii, conducerea", "carduri și tabel, reîmprospătat la 30 s", "1 ecran", "permanent", "DashboardService, din viziuni"],
      ["Raportul de vânzări pe zi, pe interval, pe traseu și casier", "situație de sinteză", "raportul de casă, controlul încasărilor", "conducerea, contabilitatea", "tabel, export CSV, arhivă pe file server", "1–2 pagini pe zi", "zilnic, lunar", "sp_RaportVanzariZilnic, vw_VanzariZilnice"],
      ["Raportul de ocupare și comparația lunară", "situație de sinteză", "planificarea curselor", "conducerea, transportatorii", "tabel, export CSV", "un rând pe cursă sau pe traseu", "lunar", "sp_RaportOcupareCurse, vw_RapoarteComparative"],
      ["Alertele de ITP și de spațiu pe disc", "semnalare", "prevenirea curselor fără ITP și a opririi serverului", "administratorul", "jurnalul de audit, log-ul SQL Server", "câteva pe săptămână", "zilnic, la 6 ore", "joburile SQL Server Agent"],
      ["Jurnalul de audit", "situație de control", "aflarea autorului fiecărei operații", "conducerea", "tabel filtrabil", "≈ 400 de rânduri pe zi", "la cerere", "tabelul LogAudit, AuditService"],
      ["Copiile de rezervă ale bazei și ale fișierelor JSON", "arhivare", "restaurarea după o defecțiune", "administratorul", "fișiere .bak, folderul Backup de pe file server", "1 complet + 4 diferențiale pe zi", "zilnic, la fiecare modificare", "joburile de backup, FileServer"],
    ], [16, 9, 14, 11, 14, 11, 10, 15], { caption: "Ieșirile sistemului informatic", size: 16 }),

    // ---------------- 1.7 Riscuri ----------------
    H(2, "1.7. Riscurile și abaterile de la termene"),
    P("Riscurile au fost identificate la început, la alegerea arhitecturii, și urmărite pe parcurs. Probabilitatea și impactul sunt apreciate de echipă pe scara mică, medie, mare; ultima coloană arată ce s-a întâmplat efectiv."),
    ...table(["Riscul", "Prob.", "Impact", "Măsura", "Ce s-a întâmplat"], [
      ["Serverul de acasă este oprit sau fără internet", "mare", "mare", "mod offline doar pentru citire; joburi cu reîncercare", "backup-ul complet de la 02:00 nu a rulat niciodată, deci și cel diferențial eșuează (D3)"],
      ["Schimbarea IP-ului public sau a routerului", "medie", "mare", "IP fix sau DDNS; setările stației editabile", "nu s-a produs"],
      ["Vânzarea dublă a unui loc la concurență", "mare", "mare", "proceduri tranzacționale, UPDLOCK, index filtrat unic", "0 bilete duble cu 20 de stații (5.4)"],
      ["Ghicirea parolelor prin încercări repetate", "medie", "mare", "blocare după 5 încercări", "limita poate fi ocolită prin încercări simultane (D7)"],
      ["Pierderea datelor sau a codului", "mică", "mare", "Git și GitHub; backup; restaurare testată", "nu s-a produs; restaurarea a reușit (5.2)"],
      ["Indisponibilitatea unui membru al echipei", "medie", "medie", "un responsabil pe compartiment, cod comun în Git", "pauză de lucru între 19.09 și 13.10"],
      ["Întârzierea testării", "medie", "medie", "plan de testare pe roluri", "testarea făcută în două etape (13.10 și 21–22.10)"],
      ["Integrarea cu casa de marcat cerută legal", "medie", "medie", "păstrarea numărului bonului", "integrarea amânată (1.5.2)"],
    ], [24, 8, 9, 27, 32], { caption: "Riscurile proiectului", size: 20 }),
    P("**Abaterile.** Față de modelul cascadă planificat, „fiecare etapă se termină complet, fără revenire”, au apărut trei abateri. Prima: după realizarea din 17–19 septembrie a urmat o pauză de aproape patru săptămâni, iar testarea s-a făcut în două reprize, pe 13 și pe 21–22 octombrie. A doua: testarea a găsit defecte (D3, D7) care cer întoarcerea la realizare, deci o iterație în plus. A treia: trei sarcini din Sarcina tehnică au rămas neterminate sau amânate: integrarea cu casa de marcat, publicarea unei actualizări automate și dicționarul de date. Dicționarul a fost întocmit acum, în secțiunea 6.9."),

    // ---------------- 1.8 Eficienta economica ----------------
    H(2, "1.8. Indicatorii eficienței economice"),
    P("Eficiența economică compară investiția în sistem cu economiile pe care le aduce autogării. **Toate valorile sunt estimări**, cu ipotezele scrise în tabele; prețurile sunt în lei moldovenești (MDL)."),
    ...table(["Componenta", "Calculul", "Suma"], [...costuri.map((r) => [r[0], r[1], lei(r[2])]), ["**Investiția totală**", "", `**${lei(investitie)}**`]],
      [34, 46, 20], { caption: "Costurile unice ale sistemului (estimare)" }),
    P(`Orele de elaborare sunt împărțite astfel: ${ore.map(([n, h]) => `${n} ${h} de ore`).join(", ")}. Se folosește ediția gratuită SQL Server Express; pentru că ea nu are SQL Server Agent, joburile se mută în Task Scheduler, ceea ce este inclus la instalare.`),
    ...table(["Cheltuiala anuală", "Calculul", "Suma pe an"], [...exploatare.map((r) => [r[0], r[1], lei(r[2])]), ["**Total cheltuieli de exploatare**", "", `**${lei(costAnual)}**`]],
      [34, 46, 20], { caption: "Cheltuielile anuale de exploatare (estimare)" }),
    ...table(["Economia anuală", "Calculul", "Suma pe an"], [...economii.map((r) => [r[0], r[1], lei(r[2])]), ["**Total economii**", "", `**${lei(economieAnuala)}**`]],
      [34, 46, 20], { caption: "Economiile anuale aduse de sistem (estimare)" }),
    P(`Timpul câștigat de casieri nu înseamnă automat salarii economisite, de aceea s-a luat în calcul doar ${VALORIFICAT * 100} % din el (mai puține ore suplimentare, un ghișeu mai puțin deschis la ore de vârf). Duratele de ${SEC_VECHI} s și ${SEC_NOU} s vin din fișa de observare (Tabelul 1.5).`),
    ...table(["Indicatorul", "Formula", "Valoarea"], [
      ["Economia netă anuală", "economii − cheltuieli de exploatare", lei(net)],
      ["Termenul de recuperare a investiției", "investiția / economia netă anuală", `${(recuperareLuni / 12).toFixed(2).replace(".", ",")} ani (≈ ${Math.round(recuperareLuni)} luni)`],
      ["Rentabilitatea investiției (ROI) pe 3 ani", "(3 × economia netă − investiția) / investiția", `${Math.round(roi3)} %`],
      [`Valoarea actualizată netă (VAN) pe 3 ani, rata ${RATA * 100} %`, "Σ economia netă / (1 + r)^t − investiția", lei(van)],
    ], [36, 40, 24], { caption: "Indicatorii eficienței economice" }),
    P(`Cu aceste ipoteze, investiția se recuperează în mai puțin de un an, iar valoarea actualizată netă pe trei ani este pozitivă, deci proiectul este eficient economic. Chiar dacă economia de timp a casierilor nu s-ar lua deloc în calcul, rapoartele automate și erorile evitate (${lei(economii[1][2] + economii[2][2])} pe an) acoperă cheltuielile de exploatare de ${lei(costAnual)}, iar investiția s-ar recupera în aproximativ ${Math.round(investitie / (economii[1][2] + economii[2][2] - costAnual))} ani.`),

    // ---------------- 1.9 Concluzii ----------------
    H(2, "1.9. Concluzii la capitolul 1"),
    P("Analiza a arătat că problemele principale ale unei autogări fără sistem informatic sunt vânzarea dublă a locurilor, datele copiate de mai multe ori și rapoartele întârziate. Din ele au rezultat nouă obiective măsurabile și caietul de sarcini cu cerințele tehnice și legislative. Proiectul a urmat modelul cascadă în șase etape, potrivit pentru o echipă mică și cerințe fixate; abaterile (pauza dintre realizare și testare și defectele care cer o iterație în plus) sunt cunoscute și au măsuri. Sistemul a fost încadrat după criteriile de clasificare, iar aria lui de acoperire arată ce rămâne în afara automatizării. Ciclul de viață, fazele manageriale și eșalonarea pe subsisteme explică ordinea lucrărilor. Intrările și ieșirile sistemului sunt descrise prin diagramele fluxurilor de date și prin tabelele cu tipul, scopul, volumul și modul de obținere, iar eficiența economică estimată arată că investiția se recuperează în mai puțin de un an."),
  ];
};
