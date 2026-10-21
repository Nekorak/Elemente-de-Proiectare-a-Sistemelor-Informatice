const fs = require("fs");
const path = require("path");
const { P, H, fig, table, startChapter, state } = require("./lib");
const T = JSON.parse(fs.readFileSync(path.join(__dirname, "..", "tabele_vechi.json"), "utf8"));
const IMG = process.env.IMG_DIR || path.join(__dirname, "..", "img");
const capturi = fs.readdirSync(IMG).filter((f) => /^TC\d\d_/.test(f)).sort().map((f) => f.replace(/\.png$/, ""));

// Grupele cazurilor de test functionale (numerele cazurilor din tabelul vechi)
const GRUPE = [
  ["4.3.1. Autentificarea și rolurile", 1, 7],
  ["4.3.2. Căutarea curselor și vânzarea biletelor", 8, 17],
  ["4.3.3. Anularea și rambursarea", 18, 23],
  ["4.3.4. Contul de pasager", 24, 26],
  ["4.3.5. Editoarele, rapoartele și jurnalul de audit", 27, 31],
  ["4.3.6. Serverul, securitatea și datele", 32, 40],
];

function capitol(nr) {
  startChapter(4);
  const F = (cod, legenda) => fig(cod, legenda);
  const n = (cod) => nr[cod] || "4.?";
  const mediu = T[12], cazuri = T[13];

  const mediuNou = mediu.slice(1).map((r) => {
    if (r[0] === "Data testării") return [r[0], "13 octombrie 2026 (testele funcționale); 21–22 octombrie 2026 (concurență, încărcare, mai multe instanțe, securitate, recuperare)"];
    if (r[0] === "Conturi folosite") return [r[0], r[1] + "; doar în copia de test: stres.casier01–20, stres.admin, stres.pasager (programul de stres) și gui.admin1–2, gui.casier1–4, gui.pasager (testele din interfață)"];
    if (r[0] === "Stația de test") return [r[0], "Windows 11 Pro, 12 procesoare logice, aplicația WinForms .NET 10; până la 5 instanțe ale aplicației deschise simultan și programul de stres cu până la 20 de stații virtuale"];
    return r;
  });
  mediuNou.push(["Baza pentru testele care scriu", "autogara_test: copie a bazei autogara, restaurată dintr-un backup COPY_ONLY pe aceeași instanță EPSI; baza reală autogara a fost doar citită"]);

  const grupa = ([titlu, a, b]) => {
    const randuri = cazuri.slice(1).filter((r) => +r[0] >= a && +r[0] <= b)
      .map((r) => [r[0], r[1], r[2], r[3], r[4], `${r[5]}\nFig. ${n(capturi[+r[0] - 1])}`]);
    return [
      H(3, titlu),
      ...table(cazuri[0], randuri, [5, 15, 22, 18, 30, 10], { caption: `Cazurile de test ${a}–${b}`, size: 18 }),
      ...randuri.flatMap((r) => F(capturi[+r[0] - 1], `Cazul ${r[0]}: ${r[1]}`)),
    ];
  };

  const stres = [
    ["S1", "Același loc rezervat simultan de 5, 10 și 20 de stații (câte 10 runde)", "o singură rezervare pe loc, restul refuzate", "10/10 runde corecte la fiecare nivel; 40, 90 și 190 de refuzuri „locul nu mai este liber”; 0 bilete duble; p95 541, 94 și 100 ms", "Trecut"],
    ["S2", "Vânzare în masă: 20 de stații vând până se epuizează cele 51 de locuri ale unei curse", "fiecare loc vândut o singură dată, plățile corecte", "51 de bilete în 4,4 s (11,7 bilete/s); 0 duble, 0 plăți greșite, 0 rezervări rămase; 19 conflicte rezolvate și 5 blocaje reciproce (deadlock); mediana 7 ms, p95 4 306 ms", "Trecut cu observație (D8)"],
    ["S3", "Aceeași rezervare confirmată simultan (dublu clic, două stații), 10 runde", "un singur bilet și o singură plată", "10/10 runde corecte; în 7 runde biletul a fost emis de alt casier decât cel care a rezervat locul", "Trecut cu observație (D9)"],
    ["S4", "Același bilet anulat simultan de 10 stații, 10 runde", "o singură anulare și o singură rambursare", "10/10 runde corecte; celelalte anulări refuzate (50041)", "Trecut"],
    ["S5", "Rezervare de 1 minut lăsată să expire, apoi confirmată", "confirmarea întârziată refuzată, locul liber", "alt casier a luat locul expirat; confirmarea refuzată (50032 / 50037); locul a redevenit liber", "Trecut"],
    ["S6", "Limita de 5 încercări la autentificare, cu 20 de parole greșite trimise simultan", "cel mult 5 încercări verificate, restul blocate", "secvențial: 5 × „incorectă”, apoi „blocat”; în paralel: toate cele 20 de încercări verificate, 0 blocate, 20 de rânduri în LogAudit", "Eșuat (D7)"],
    ["S7", "Încărcare: dashboard-ul cerut de 20 de stații × 10 ori", "fără erori, timp mic", "200 de apeluri, 0 erori; mediana 9 ms, p95 147 ms; 827 de apeluri/s", "Trecut"],
    ["S8", "Injecție SQL și valori invalide la vânzare și la căutarea biletului", "textul salvat exact, valorile invalide oprite", "4/4 nume (SQL, HTML, diacritice cu emoji, 200 de caractere) salvate exact; 4/4 valori invalide oprite; 3/3 căutări cu injecție fără rezultat", "Trecut"],
    ["S9", "Contul SQL al aplicației încearcă operații interzise (în tranzacții anulate)", "toate refuzate", "7/7 refuzate: DELETE și UPDATE pe jurnal (229), DROP (3701), CREATE (262), ALTER (1088), sysadmin (15151), TRUNCATE (1088); citirea permisă", "Trecut"],
    ["S10", "Operații interzise rolului, apelate direct în backend (pasager și casier)", "toate refuzate", "5/5 refuzate cu AccesInterzis", "Trecut"],
  ];

  const gui = [
    ["G1", "Două stații aleg același loc; a doua avea schema deschisă dinainte (neactualizată)", "a doua stație primește refuzul, nu se emite bilet", "„Locul nu mai este liber (a fost rezervat sau vândut între timp).”; schema stației se reîncarcă și arată locul galben", "Trecut", `${n("GT02_acelasi_loc_doua_statii")}, ${n("GT03_loc_rezervat_vazut_de_alta_statie")}`],
    ["G2", "Casierul 1 confirmă vânzarea; celelalte stații au dashboard-ul deschis", "biletul emis; cifrele se actualizează pe celelalte stații", "bilet AG261022-EC12C9E8, 120,00 MDL; pe stațiile admin și casier: „Bilete vândute azi” = 1, „Încasări nete” = 120,00 MDL, fără reîmprospătare manuală", "Trecut", `${n("GT04_vanzare_confirmata_casier1")}, ${n("GT05_dashboard_actualizat_alte_statii")}`],
    ["G3", "Validări: confirmare fără loc ales, fără nume, cu telefon invalid", "mesaje clare, nimic salvat", "„Alegeți mai întâi un loc liber…”, „Numele pasagerului este obligatoriu.”, mesajul de format al telefonului", "Trecut", `${n("GT06_validare_fara_loc")}, ${n("GT07_validare_nume_obligatoriu")}, ${n("GT16_validare_telefon")}`],
    ["G4", "Casierul închide fereastra de vânzare cu un loc rezervat", "locul se eliberează imediat", "alt casier a putut lua locul 7 imediat după închidere", "Trecut", n("GT08_loc_eliberat_la_inchidere")],
    ["G5", "Stația cade (procesul oprit forțat cu taskkill /F) cu un loc rezervat", "rezervarea rămâne până la expirare, apoi nu mai blochează locul", "locul 8 rămâne „Rezervat” (în bază: rezervarea lui gui.casier3, încă 548 s)", "Trecut", n("GT09_rezervare_ramasa_dupa_cadere")],
    ["G6", "Administratorul dezactivează un casier care are un loc rezervat", "rezervarea se eliberează, casierul nu mai poate vinde", "rezervarea eliberată; vânzarea refuzată (50034); casierul dezactivat a putut însă rezerva un loc nou", "Trecut cu observație (D10)", `${n("GT10_admin_dezactiveaza_casier")}, ${n("GT11_vanzare_refuzata_casier_dezactivat")}, ${n("GT12_confirmare_refuzata_utilizator_dezactivat")}`],
    ["G7", "Doi administratori modifică același cont în același timp", "a doua salvare refuzată, fără pierderea primei modificări", "„Înregistrarea a fost modificată de alt utilizator între timp…”; în bază a rămas telefonul salvat de primul administrator", "Trecut", n("GT17_conflict_doi_administratori")],
  ];

  const defecte = [
    ["D1", "Medie", "Testul automat HartaDinRepository_EsteValida pică: fișierul File-Server/Harta/harta.json are 17 noduri, iar testul așteaptă 15. Fișierul a fost modificat din editorul de hartă și nu e salvat în Git, la fel ca autobuz_10.json și autobuz_11.json.", "dotnet test App/Autogara.slnx", `Fig. ${n(capturi[39])}`],
    ["D2", "Minoră", "După anularea unui bilet, butonul „Anulează biletul” rămâne activ și cere din nou confirmarea. Serverul respinge a doua anulare, deci nu există dublă rambursare (confirmat și de S4, cu 10 stații simultan).", "Bilete → bilet rambursat → Anulează biletul", `Fig. ${n(capturi[20])}–${n(capturi[21])}`],
    ["D3", "Majoră", "Backup-ul diferențial eșuează la fiecare rulare: nu există un backup complet de bază, pentru că jobul de la 02:00 nu rulează (serverul e oprit noaptea). În plus, folderul D:\\Backup\\autogara nu exista pe server (eroarea de sistem 3 la primul backup de test); a fost creat la testare.", "Istoricul joburilor; pregătirea copiei de test", `Fig. ${n(capturi[38])}, ${n("TS04_copie_backup_restore")}`],
    ["D4", "Minoră", "Locul 1 al cursei 211 are starea Ocupat fără niciun bilet activ; a rămas de la testele din timpul dezvoltării.", "Interogarea de consistență", `Fig. ${n(capturi[37])}`],
    ["D5", "Minoră", "În baza de date au rămas 10 înregistrări de test (nodul „TEST nod 172534”, șoferii TST*, utilizatorul test.backend172534, bilete „TEST …”), vizibile în listele din aplicație.", "Interogarea de consistență; editorul de hartă", `Fig. ${n(capturi[37])}, ${n(capturi[26])}`],
    ["D6", "Informativ", "Jobul de verificare a spațiului pe disc eșuează intenționat: discul C: are ~8% liber (sub pragul de 10%). Alerta funcționează, dar spațiul trebuie eliberat.", "Istoricul joburilor", `Fig. ${n(capturi[38])}`],
    ["D7", "Majoră", "Limita de 5 încercări la autentificare poate fi ocolită prin încercări simultane: numărul eșecurilor este citit și verificat fără blocare, așa că 20 de parole greșite trimise în același moment sunt toate verificate. Contul se blochează abia după aceea. Remediere: numărarea și înregistrarea încercării în aceeași tranzacție serializată (sp_getapplock pe numele contului sau un contor actualizat atomic).", "Programul de stres, scenariul S6", `Fig. ${n("TS01_stres_rezumat")}, ${n("TS03_stres_rezultate_md")}`],
    ["D8", "Minoră", "La vânzarea în masă (20 de stații pe aceeași cursă) apar blocaje reciproce (deadlock 1205): 5 din 51 de vânzări. Casierul primește „încercați din nou”, iar o vânzare poate dura până la 4,3 s. Datele rămân corecte. Remediere: aceeași ordine de blocare a rândurilor în sp_RezervaLoc și sp_ConfirmaVanzareBilet sau reluarea automată a tranzacției.", "Programul de stres, scenariul S2", `Fig. ${n("TS01_stres_rezumat")}`],
    ["D9", "Informativ", "O rezervare provizorie poate fi confirmată de alt casier decât cel care a făcut-o (7 din 10 runde în S3). Nu produce bilete duble, dar biletul apare vândut de alt casier.", "Programul de stres, scenariul S3", `Fig. ${n("TS03_stres_rezultate_md")}`],
    ["D10", "Minoră", "Un casier dezactivat în timpul lucrului rămâne autentificat și poate încă rezerva locuri (sp_RezervaLoc nu verifică dacă utilizatorul e activ); locurile stau blocate până la 10 minute. Vânzarea este refuzată corect de server.", "Utilizatori → Dezactivează, apoi vânzare pe stația casierului", `Fig. ${n("GT12_confirmare_refuzata_utilizator_dezactivat")}`],
    ["D11", "Minoră", "Mesajul de bază indisponibilă are numele bazei scris fix: „Baza de date „autogara” nu este disponibilă”, deși stația era configurată pe autogara_test.", "Baza trecută OFFLINE cu aplicația deschisă", `Fig. ${n("GT14_eroare_baza_indisponibila")}`],
    ["D12", "Minoră", "Erorile afișate utilizatorului (conflicte, bază indisponibilă, vânzări refuzate) sunt scrise în fișierul de log cu nivelul Information, nu Warning sau Error, deci nu pot fi filtrate după gravitate.", "%LOCALAPPDATA%\\Autogara\\Logs\\autogara-*.log", "secțiunea 4.7"],
  ];

  return [
    H(1, "4. TESTAREA"),
    P("Testarea a fost condusă de **Sopivnic Maxim** în două etape. Prima etapă (13 octombrie 2026) a verificat funcțiile aplicației caz cu caz, pe baza reală, cu câte o captură de ecran pentru fiecare caz. A doua etapă (21–22 octombrie 2026) a încercat să „strice” sistemul: mai multe stații care lucrează în același timp pe aceleași date, încărcare, atacuri simple și căderi ale stației sau ale serverului. Testele care scriu date au rulat pe o copie a bazei, ca datele reale să rămână neatinse."),

    H(2, "4.1. Planul de testare"),
    ...table(["Nivel", "Ce se verifică", "Cum", "Volum"], [
      ["Funcțional", "fiecare funcție a aplicației, pe roluri", "manual, în aplicație și în SSMS", "40 de cazuri"],
      ["Concurență și încărcare", "rezervare, vânzare, anulare și autentificare cu multe stații simultan; timpi de răspuns", "programul Testare/Stres, cu până la 20 de stații virtuale pornite în același moment", "10 scenarii, 14 verificări"],
      ["Mai multe instanțe", "comportamentul interfeței când alte stații schimbă aceleași date", "5 instanțe ale aplicației deschise simultan, cu conturi diferite", "7 scenarii"],
      ["Securitate", "injecție SQL, valori invalide, drepturile contului SQL și ale rolurilor", "programul de stres și cazurile 35–37", "4 scenarii"],
      ["Recuperare", "căderea stației, baza indisponibilă, backup și restaurare", "taskkill, ALTER DATABASE … OFFLINE/ONLINE, BACKUP/RESTORE", "4 scenarii"],
    ], [16, 34, 34, 16], { caption: "Nivelurile de testare" }),
    P("Un test este trecut când rezultatul obținut este cel așteptat, iar datele din baza de date rămân consistente după test (fără bilete duble, plăți greșite sau locuri blocate). Fiecare problemă găsită primește un cod de defect (secțiunea 4.8)."),

    H(2, "4.2. Mediul de testare și copia bazei de date"),
    ...table(mediu[0], mediuNou, [22, 78], { caption: "Mediul de testare", size: 20 }),
    P("Pentru testele care scriu date s-a făcut un backup COPY_ONLY al bazei autogara (nu afectează lanțul de backup-uri), verificat cu RESTORE VERIFYONLY și restaurat pe aceeași instanță sub numele autogara_test. Copia a trecut DBCC CHECKDB fără erori, iar toate cele 17 tabele au același număr de rânduri ca originalul (de exemplu 13 804 bilete, 14 303 plăți, 31 677 de locuri). Contul aplicației, autogara_app, are în copie aceleași drepturi minime ca în baza reală. Primul backup a eșuat, pentru că folderul D:\\Backup\\autogara nu exista pe server (defectul D3); după crearea folderului totul a mers în mai puțin de o secundă."),
    ...F("TS04_copie_backup_restore", "Backup-ul COPY_ONLY, verificarea și restaurarea în baza autogara_test"),
    ...F("TS05_copie_comparatie", "Compararea numărului de rânduri: autogara și autogara_test, toate tabelele egale"),
    P("Stația de test a fost trecută pe baza autogara_test din asistentul de configurare al aplicației, iar la final configurarea a fost readusă pe baza reală."),
    ...F("GT01_configurare_autogara_test", "Configurarea stației pe copia de test autogara_test, cu ambele conexiuni verificate"),

    H(2, "4.3. Testarea funcțională"),
    P("Au fost executate 40 de cazuri de test: 35 trecute, 1 trecut cu observație și 4 eșuate. Cazurile sunt grupate pe module; după tabelul fiecărui grup urmează capturile de ecran ale cazurilor lui, în aceeași ordine."),
    ...GRUPE.flatMap(grupa),

    H(2, "4.4. Testele de concurență și încărcare"),
    P("Programul de consolă din Testare/Stres folosește direct serviciile aplicației (Autogara.Business), deci trece prin aceleași reguli și proceduri stocate ca o stație reală. Înainte de pornire verifică faptul că este conectat la autogara_test și refuză să ruleze pe altă bază. Pregătește 22 de conturi de test cu parole generate, le autentifică pe toate în paralel (2,2 s), apoi rulează scenariile. Într-un scenariu toate stațiile virtuale așteaptă la o barieră comună și pornesc în același moment, ca cererile să ajungă la server efectiv simultan."),
    ...table(["ID", "Scenariu", "Rezultat așteptat", "Rezultat obținut", "Status"], stres, [6, 26, 18, 38, 12], { caption: "Rezultatele testelor de concurență și încărcare (21.10.2026, 23:29)", size: 18 }),
    ...F("TS01_stres_rezumat", "Rularea programului de stres: rezultatul fiecărui scenariu și totalul de 14 verificări"),
    ...F("TS03_stres_rezultate_md", "Fișierul rezultate.md generat de program, cu parametrii și timpii fiecărui scenariu"),
    P("Rezervarea și vânzarea sunt sigure la concurență: chiar cu 20 de stații pe același loc, procedura sp_RezervaLoc (blocări UPDLOCK și HOLDLOCK) lasă să treacă o singură rezervare, iar celelalte primesc imediat refuzul. La vânzarea în masă nu s-a vândut niciun loc de două ori, dar au apărut 5 blocaje reciproce, pe care SQL Server le rezolvă oprind una dintre tranzacții; aplicația le arată casierului ca „încercați din nou” (D8). Singurul test eșuat este S6: limita de autentificare nu ține la încercări simultane (D7)."),

    H(2, "4.5. Testarea cu mai multe instanțe ale aplicației"),
    P("Pe stația de test au fost deschise simultan 5 instanțe ale aplicației, autentificate ca gui.admin1 și gui.casier1–4 (ulterior una a fost reautentificată ca gui.admin2). Ferestrele au fost așezate una lângă alta, ca efectul acțiunii unei stații să se vadă pe celelalte în aceeași captură."),
    ...table(["ID", "Scenariu", "Rezultat așteptat", "Rezultat obținut", "Status", "Fig."], gui, [5, 24, 18, 31, 12, 10], { caption: "Scenariile cu mai multe instanțe ale aplicației", size: 18 }),
    H(3, "4.5.1. Același loc pe două stații"),
    P("Casierul 1 a ales locul 6 al cursei de 07:30 Chișinău (Nord) – Orhei – Bălți. Casierul 3 avea deschisă aceeași schemă dinainte, în care locul 6 apărea încă liber, și l-a ales și el. Serverul a refuzat a doua rezervare, iar schema casierului 3 s-a reîncărcat și arată locul ca rezervat."),
    ...F("GT02_acelasi_loc_doua_statii", "Două stații pe același loc: casierul 1 are locul 6 rezervat, casierul 3 primește refuzul"),
    ...F("GT03_loc_rezervat_vazut_de_alta_statie", "După refuz, schema casierului 3 arată locul 6 rezervat (galben) și 45 de locuri libere"),
    H(3, "4.5.2. Vânzarea și actualizarea celorlalte stații"),
    P("Casierul 1 a confirmat vânzarea. Dashboard-urile celorlalte stații s-au actualizat singure la următoarea reîmprospătare automată: un bilet vândut și 120,00 MDL încasați. Schema locurilor deschisă pe altă stație nu se actualizează singură, ci la următoarea acțiune pe ea; locul vândut nu poate fi însă ales, pentru că serverul verifică starea la fiecare rezervare."),
    ...F("GT04_vanzare_confirmata_casier1", "Vânzarea confirmată de casierul 1: biletul AG261022-EC12C9E8, locul 6, 120,00 MDL"),
    ...F("GT05_dashboard_actualizat_alte_statii", "Dashboard-urile celorlalte stații arată biletul vândut și încasarea, fără reîmprospătare manuală"),
    H(3, "4.5.3. Validările formularelor"),
    ...F("GT06_validare_fara_loc", "Confirmare fără loc ales: „Alegeți mai întâi un loc liber pe schema autobuzului.”"),
    ...F("GT07_validare_nume_obligatoriu", "Confirmare fără numele pasagerului: „Numele pasagerului este obligatoriu.” (locul 7 rămâne rezervat)"),
    ...F("GT16_validare_telefon", "Telefon invalid în formularul unui cont: mesajul de format, cu exemplul +373 69 123 456"),
    H(3, "4.5.4. Închiderea normală și căderea stației"),
    P("Când casierul închide fereastra de vânzare, rezervarea lui se anulează imediat: alt casier a putut lua locul 7 în aceeași secundă. Când stația cade (procesul aplicației a fost oprit forțat cu taskkill /F, ca la o pană de curent), nu are cine să anuleze rezervarea, așa că locul rămâne rezervat până la expirare. Interogarea făcută la 11 secunde după cădere a arătat rezervarea lui gui.casier3 pe locul 8, cu 548 de secunde rămase. După expirare locul nu mai este blocat: procedura sp_RezervaLoc tratează rezervarea expirată ca liberă, iar jobul de eliberare o șterge la următoarea rulare (verificat și de scenariul S5)."),
    ...F("GT08_loc_eliberat_la_inchidere", "După închiderea ferestrei casierului 3, alt casier rezervă imediat același loc 7"),
    ...F("GT09_rezervare_ramasa_dupa_cadere", "După căderea stației casierului 3, locul 8 rămâne rezervat pe celelalte stații"),
    ...table(["Loc", "Stare", "Rezervare creată (UTC)", "Expiră (UTC)", "Utilizator", "Secunde rămase"], [
      ["6", "Ocupat", "—", "—", "—", "—"],
      ["7", "Liber", "—", "—", "—", "—"],
      ["8", "Rezervat", "21:12:36", "21:22:36", "gui.casier3", "548"],
    ], [8, 14, 22, 18, 20, 18], { caption: "Locurile cursei 921 la 11 s după căderea stației (autogara_test)", size: 20 }),
    H(3, "4.5.5. Casier dezactivat în timpul lucrului"),
    P("Casierul 2 avea locul 9 rezervat și numele pasagerului completat când administratorul i-a dezactivat contul. Aplicația anunță că rezervările în curs se eliberează, iar confirmarea vânzării a fost refuzată. Casierul a rămas însă autentificat și a putut rezerva un loc nou (locul 10); abia confirmarea a fost oprită de server cu mesajul „Utilizatorul care efectuează vânzarea nu există sau este dezactivat.” (D10)."),
    ...F("GT10_admin_dezactiveaza_casier", "Administratorul dezactivează contul gui.casier2, care are un loc rezervat pe altă stație"),
    ...F("GT11_vanzare_refuzata_casier_dezactivat", "Confirmarea locului 9 după dezactivare: rezervarea fusese deja eliberată"),
    ...F("GT12_confirmare_refuzata_utilizator_dezactivat", "Casierul dezactivat rezervă locul 10, dar vânzarea este refuzată de server"),
    H(3, "4.5.6. Doi administratori pe același cont"),
    P("Ambii administratori au deschis formularul contului gui.casier1. Primul a salvat un telefon nou; când al doilea a salvat, aplicația a detectat prin coloana RowVersion că înregistrarea s-a schimbat între timp și a refuzat salvarea. În baza de date a rămas modificarea primului administrator, deci nu s-a pierdut nimic."),
    ...F("GT17_conflict_doi_administratori", "Doi administratori modifică același cont: a doua salvare este refuzată, lista primului arată telefonul salvat"),
    P("Observație de interfață: casetele de confirmare ale sistemului (de exemplu la dezactivarea contului) au butoanele în limba sistemului de operare („Да/Нет” pe un Windows în rusă), restul aplicației fiind în română."),

    H(2, "4.6. Testele de securitate"),
    P("Pe lângă cazurile 35–37 (drepturile conturilor SQL și constrângerile de integritate), programul de stres a încercat atacuri simple. Numele de pasager cu text de injecție SQL (`'); DROP TABLE autogara.Bilete;--`), cod HTML, diacritice cu emoji și 200 de caractere au fost salvate exact, iar tabelul Bilete a rămas neatins, pentru că toate operațiile folosesc parametri. Căutarea biletului după cod cu `' OR 1=1 --`, `UNION SELECT` sau `%` nu a întors nimic. Contul autogara_app nu a putut șterge sau modifica jurnalul de audit, crea, modifica sau șterge tabele ori deveni sysadmin. Casierul și pasagerul nu au putut apela funcții de administrator nici direct în backend, ocolind meniul. Singura problemă de securitate este limita de autentificare la încercări simultane (D7)."),

    H(2, "4.7. Testele de recuperare"),
    P("**Baza de date indisponibilă cu aplicațiile deschise.** Baza autogara_test a fost trecută OFFLINE (ALTER DATABASE … SET OFFLINE WITH ROLLBACK IMMEDIATE) cât timp patru stații erau deschise; baza reală a rămas ONLINE. Toate stațiile au detectat singure căderea în cel mult 30 de secunde (indicatorul a trecut pe „Mod local (offline)”). O căutare pornită în acest timp a fost reîncercată automat, apoi a afișat un mesaj clar, fără ca aplicația să se închidă. După readucerea bazei ONLINE, stațiile au revenit la „Online” și au lucrat normal, fără repornire și fără reautentificare."),
    ...F("GT13_mod_offline_indicator", "Baza trecută OFFLINE: stațiile arată „Mod local (offline)”, iar dashboard-ul afișează motivul"),
    ...F("GT14_eroare_baza_indisponibila", "O căutare făcută cât baza este OFFLINE: mesaj de eroare, aplicația rămâne deschisă"),
    ...F("GT15_revenire_online_fara_repornire", "După readucerea bazei ONLINE, stațiile revin la „Online” și căutarea funcționează fără repornire"),
    ...table(["Stația", "Cădere detectată", "Revenire detectată"], [
      ["gui.casier1", "00:19:09", "00:21:39"],
      ["gui.casier2", "00:19:13", "00:21:43"],
      ["gui.casier4", "00:19:21", "00:21:21"],
      ["gui.admin1", "00:19:28", "00:20:58"],
    ], [34, 33, 33], { caption: "Momentele detectate de stații (baza OFFLINE la 00:19:02, ONLINE la 00:20:44)", size: 20 }),
    P("Rezervarea locului 8, rămasă de la stația căzută, a supraviețuit trecerii bazei OFFLINE și înapoi ONLINE. Momentele de mai sus și mesajele afișate au fost luate din fișierele de log ale stațiilor; acolo s-a observat că erorile sunt scrise cu nivelul Information (D12)."),
    P("**Căderea stației** (secțiunea 4.5.4): rezervarea rămâne până la expirare, fără locuri blocate definitiv. **Backup și restaurare** (secțiunea 4.2): restaurarea dintr-un backup a fost făcută efectiv, cu CHECKDB și comparația tabelelor, deci testul care în prima etapă nu fusese executat este acum trecut, pe copie."),
    P("Nu s-a testat oprirea file server-ului SFTP, pentru că el deservește și ceilalți membri ai echipei; indicatorul are pentru acest caz starea separată „Fără file server”."),

    H(2, "4.8. Defecte găsite"),
    P("Defectele D1–D6 provin din prima etapă, iar D7–D12 din testele de concurență, mai multe instanțe și recuperare."),
    ...table(["ID", "Severitate", "Descriere", "Unde se reproduce", "Dovadă"], defecte, [5, 11, 50, 20, 14], { caption: "Defectele găsite la testare", size: 18 }),

    H(2, "4.9. Concluzia testării"),
    P("Au fost executate 40 de cazuri funcționale, 14 verificări automate de concurență, încărcare și securitate, 7 scenarii cu mai multe instanțe ale aplicației și 4 scenarii de recuperare. Partea cea mai sensibilă a sistemului, vânzarea aceluiași loc de pe mai multe stații, a rezistat la 20 de stații simultane fără niciun bilet dublu sau plată greșită, iar conflictele dintre administratori sunt prinse prin RowVersion. Aplicația supraviețuiește căderii bazei de date și își revine singură."),
    P("Înainte de predare trebuie remediate D3 (backup-ul complet automat) și D7 (limita de autentificare la încercări simultane), apoi D1 (testul automat) și curățate datele de test (D4, D5). Celelalte defecte sunt minore și nu afectează corectitudinea datelor."),
  ];
}

module.exports = () => {
  // Prima trecere numeroteaza figurile, ca textul sa le poata cita inainte de a aparea.
  const lungimi = ["used", "missing", "headings", "cit"].map((k) => [k, state[k].length]);
  capitol({});
  const nr = {};
  state.used.slice(lungimi[0][1]).forEach((cod, i) => { nr[cod] = `4.${i + 1}`; });
  for (const [k, l] of lungimi) state[k].length = l;
  return capitol(nr);
};
