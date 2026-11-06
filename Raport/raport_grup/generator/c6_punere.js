// Capitolul 6: punerea in functiune si documentatia (punctele scenariului de proiectare).
const fs = require("fs");
const path = require("path");
const { P, H, bullets, numbered, code, fig, table, startChapter } = require("./lib");

const dictionar = JSON.parse(fs.readFileSync(path.join(__dirname, "dictionar.json"), "utf8"));

// Descrierile coloanelor pentru dictionarul de date
const GENERIC = {
  CreatLa: "momentul creării rândului (UTC)",
  ModificatLa: "momentul ultimei modificări (UTC)",
  Activ: "1 = activ, 0 = dezactivat (ștergere logică)",
  RowVersion: "versiunea rândului, pentru controlul concurenței",
};
const DESCRIERI = {
  "Roluri.Denumire": "numele rolului: Admin, Casier sau Pasager",
  "Utilizatori.NumeUtilizator": "numele de autentificare, minimum 3 caractere",
  "Utilizatori.ParolaHash": "hash-ul PBKDF2-SHA256 al parolei, cu sarea și numărul de iterații",
  "Utilizatori.Nume": "numele de familie",
  "Utilizatori.Prenume": "prenumele",
  "Utilizatori.Email": "adresa de e-mail (opțională)",
  "Utilizatori.Telefon": "telefonul (opțional)",
  "Utilizatori.RolID": "rolul utilizatorului",
  "Noduri.Tip": "Statie sau Intersectie",
  "Noduri.Nume": "numele afișat pe hartă",
  "Noduri.CoordX": "coordonata orizontală în editorul hărții",
  "Noduri.CoordY": "coordonata verticală în editorul hărții",
  "Conexiuni.NodPlecareID": "nodul de la un capăt al drumului",
  "Conexiuni.NodSosireID": "nodul de la celălalt capăt (diferit de primul)",
  "Conexiuni.DistantaKm": "lungimea drumului în km, pozitivă",
  "Statii.NodID": "nodul hărții care reprezintă stația (un nod are cel mult o stație)",
  "Statii.Adresa": "adresa stației",
  "Statii.Peron": "peronul de plecare",
  "Autobuze.NrInmatriculare": "numărul de înmatriculare, unic",
  "Autobuze.Model": "marca și modelul",
  "Autobuze.CapacitateLocuri": "numărul de locuri, pozitiv",
  "Autobuze.Status": "Activ, Service sau Scos din uz",
  "Autobuze.CaleFisierJSON": "calea fișierului cu schema locurilor pe file server",
  "Autobuze.DataExpirareITP": "data expirării inspecției tehnice",
  "MentenantaAutobuze.AutobuzID": "autobuzul la care s-a lucrat",
  "MentenantaAutobuze.TipLucrare": "tipul lucrării de mentenanță",
  "MentenantaAutobuze.Data": "data lucrării",
  "MentenantaAutobuze.Kilometraj": "kilometrajul la data lucrării, nenegativ",
  "MentenantaAutobuze.Observatii": "observații libere",
  "Soferi.Nume": "numele de familie",
  "Soferi.Prenume": "prenumele",
  "Soferi.NrPermis": "numărul permisului de conducere, unic",
  "Soferi.Telefon": "telefonul",
  "Trasee.Denumire": "denumirea traseului, unică (de exemplu Chișinău – Bălți)",
  "TraseuOpriri.TraseuID": "traseul",
  "TraseuOpriri.StatieID": "stația de oprire (o dată pe traseu)",
  "TraseuOpriri.Ordine": "poziția opririi pe traseu, unică pe traseu",
  "TipuriReducere.Denumire": "numele reducerii, unic",
  "TipuriReducere.ProcentReducere": "procentul reducerii, între 0 și 100",
  "Curse.TraseuID": "traseul cursei",
  "Curse.AutobuzID": "autobuzul alocat",
  "Curse.SoferID": "șoferul alocat",
  "Curse.DataCursa": "data plecării",
  "Curse.OraPlecare": "ora plecării (ora Moldovei)",
  "Curse.OraSosireEstimata": "ora estimată a sosirii, după plecare",
  "Curse.Pret": "prețul întreg al biletului, nenegativ",
  "Curse.Status": "Planificata, In desfasurare, Finalizata sau Anulata",
  "Locuri.CursaID": "cursa căreia îi aparține locul",
  "Locuri.NumarLoc": "numărul locului, unic în cursă",
  "Locuri.Status": "Liber, Rezervat sau Ocupat",
  "RezervariProvizorii.LocID": "locul rezervat (cel mult o rezervare pe loc)",
  "RezervariProvizorii.UtilizatorID": "casierul care a făcut rezervarea",
  "RezervariProvizorii.DataCreare": "momentul rezervării (UTC)",
  "RezervariProvizorii.DataExpirare": "momentul expirării, după DataCreare",
  "Bilete.CodBilet": "codul biletului afișat pasagerului, unic (index unic)",
  "Bilete.CursaID": "cursa; împreună cu LocID trebuie să existe în Locuri",
  "Bilete.LocID": "locul; un singur bilet activ pe loc (index filtrat unic)",
  "Bilete.NumePasager": "numele pasagerului",
  "Bilete.TelefonPasager": "telefonul pasagerului",
  "Bilete.TipReducereID": "reducerea aplicată (opțională)",
  "Bilete.VanzutDeUtilizatorID": "utilizatorul care a vândut biletul",
  "Bilete.DataEmitere": "momentul vânzării (UTC)",
  "Bilete.Pret": "prețul final, după reducere",
  "Bilete.Status": "Activ, Anulat sau Rambursat",
  "Plati.BiletID": "biletul plătit sau rambursat",
  "Plati.Suma": "suma, nenegativă",
  "Plati.MetodaPlata": "Numerar sau Card",
  "Plati.DataPlata": "momentul plății (UTC)",
  "Plati.Status": "Finalizata (încasare) sau Rambursata",
  "Plati.NumarBonFiscal": "numărul bonului fiscal de la casa de marcat",
  "LogAudit.UtilizatorID": "utilizatorul care a făcut acțiunea (gol pentru joburi)",
  "LogAudit.Actiune": "descrierea acțiunii",
  "LogAudit.Entitate": "tabelul sau obiectul afectat",
  "LogAudit.EntitateID": "identificatorul rândului afectat",
  "LogAudit.DataOra": "momentul acțiunii (UTC)",
};
function descriere(tabel, c) {
  const d = DESCRIERI[`${tabel}.${c.col}`] || GENERIC[c.col];
  if (d) return d;
  if (c.chei.startsWith("PK")) return "cheia primară";
  if (c.chei.includes("FK")) return "referința la " + c.chei.split("→")[1].trim();
  return "";
}
function tipCol(c) {
  let t = c.tip.replace(/\s*IDENTITY\(1, 1\)/, " IDENTITY");
  return t + (c.null ? ", NULL" : "");
}

function dictionarDate() {
  const out = [];
  for (const t of dictionar) {
    const rows = t.coloane.map((c) => [`**${c.col}**`, tipCol(c), [c.chei, c.default ? `implicit ${c.default}` : ""].filter(Boolean).join("; "), descriere(t.tabel, c)]);
    const uq = t.uq_compuse.map((u) => `(${u.join(", ")})`).join(", ");
    out.push(...table(["Coloana", "Tipul", "Chei, implicit", "Descrierea"], rows, [22, 22, 22, 34],
      { caption: `Dicționarul de date: tabelul ${t.tabel}${uq ? `; unic pe ${uq}` : ""}`, size: 18 }));
  }
  return out;
}

module.exports = () => {
  startChapter(6);
  return [
    H(1, "6. PUNEREA ÎN FUNCȚIUNE ȘI DOCUMENTAȚIA"),
    P("Ultimul capitol adună rezultatele etapelor anterioare și pregătește predarea sistemului către beneficiar: performanțele subsistemelor și ale sistemului întreg, rapoartele de stare pe etape, cele trei manuale (de prezentare, al administratorului și al utilizatorului), procedurile manuale și automate, instruirea personalului, pilotarea și dicționarul de date. Capitolul a fost elaborat de toată echipa."),

    // ---------------- 6.1 Performante ----------------
    H(2, "6.1. Performanțele subsistemelor și ale sistemului în ansamblu"),
    P("Tabelul reunește rezultatele măsurate în capitolele 2–5. Fiecare subsistem a fost verificat separat la sfârșitul etapei lui, iar sistemul întreg la testarea din capitolul 5."),
    ...table(["Subsistemul", "Indicatorul", "Rezultatul", "Sursa"], [
      ["Baza de date", "obiectele create", "17 tabele, 15 indici, 6 funcții, 5 viziuni, 10 proceduri stocate, 6 joburi", "2.1"],
      ["Baza de date", "integritatea datelor", "DBCC CHECKDB fără erori; constrângerile opresc datele invalide", "5.2, 5.3.6"],
      ["Baza de date", "restaurarea dintr-un backup", "reușită, toate cele 17 tabele identice cu originalul", "5.2"],
      ["Backend", "testele automate xUnit", "100 de cazuri în ~6,5 s: 99 trec, 1 eșuează din cauza hărții modificate local (D1)", "3.7"],
      ["Backend", "compilarea soluției", "7 proiecte, 0 erori, 7,4 s", "3.8.6"],
      ["Backend", "autentificarea simultană", "22 de conturi autentificate în paralel în 2,2 s", "5.4"],
      ["Frontend", "ferestrele și controalele", "32 de ferestre, 9 controale desenate, meniul pe trei roluri", "4.1, 4.4"],
      ["Frontend", "actualizarea celorlalte stații", "o vânzare apare pe celelalte stații fără reîmprospătare manuală", "5.5.2"],
      ["Sistemul întreg", "vânzarea concurentă", "20 de stații pe același loc: 0 bilete duble; 51 de bilete în 4,4 s (11,7 bilete/s)", "5.4 (S1, S2)"],
      ["Sistemul întreg", "timpul de răspuns al dashboard-ului", "200 de cereri de la 20 de stații: mediana 9 ms, p95 147 ms", "5.4 (S7)"],
      ["Sistemul întreg", "căderea bazei de date", "stațiile detectează căderea în cel mult 26 s și revin singure în cel mult 1 minut", "5.7"],
      ["Sistemul întreg", "testarea funcțională", "40 de cazuri: 35 trecute, 1 cu observație, 4 eșuate", "5.3"],
    ], [18, 24, 44, 14], { caption: "Performanțele măsurate ale subsistemelor și ale sistemului", size: 20 }),
    P("Comparate cu obiectivele din secțiunea 1.1, rezultatele arată că sunt atinse O1–O6 și O9. Obiectivul O7 (securitatea) este atins parțial, pentru că limita de autentificare poate fi ocolită prin încercări simultane (D7), iar O8 (păstrarea datelor) este atins parțial, pentru că backup-ul complet automat nu rulează cât serverul este oprit noaptea (D3)."),

    // ---------------- 6.2 Monitorizare ----------------
    H(2, "6.2. Monitorizarea și controlul: raportul de stare pe etape"),
    P("După fiecare etapă, echipa a verificat ce s-a terminat, ce abateri au apărut și ce decizie se ia pentru etapa următoare. Raportul de mai jos rezumă aceste controale; datele vin din istoricul Git și din rezultatele testelor."),
    ...table(["Etapa (data controlului)", "Starea", "Abaterile", "Decizia"], [
      ["Analiza și proiectarea (17.09)", "Sarcina tehnică și planul tehnic salvate în Git; compartimentele împărțite", "—", "se începe realizarea bazei de date"],
      ["Baza de date (17.09 seara)", "schema, procedurile, securitatea, joburile și datele de pornire create", "backup-ul complet programat la 02:00, când serverul poate fi oprit", "se trece la backend; ora backup-ului rămâne de revizuit"],
      ["Backend și serverul (18.09)", "soluția .NET pe straturi, testele automate, serverul SFTP și porturile funcționale", "—", "se trece la interfață"],
      ["Interfața (19.09)", "ferestrele, designul nou, ferestrele modale", "integrarea cu casa de marcat și publicarea actualizărilor amânate", "se trece la testare"],
      ["Testarea funcțională (13.10)", "40 de cazuri, date de volum (~10 000 de bilete), defectele D1–D6", "pauză de aproape 4 săptămâni înainte de testare", "testarea se extinde la concurență și recuperare"],
      ["Testarea extinsă (21–22.10)", "teste de stres, 5 instanțe, securitate, recuperare; defectele D7–D12", "D3 și D7 cer revenirea la realizare", "remedierea D3, D7, D1, D4, D5 înainte de predare"],
      ["Documentația (21.10–03.11)", "raportul de grup, diagrama Gantt, analiza și documentația de predare", "dicționarul de date întârziat față de Sarcina tehnică", "se pregătesc instruirea și pilotarea"],
    ], [20, 32, 24, 24], { caption: "Raportul de stare după fiecare etapă", size: 20 }),

    // ---------------- 6.3 Manual prezentare ----------------
    H(2, "6.3. Manualul de prezentare a sistemului"),
    P("**Ce este.** „Autogara” este un sistem informatic pentru vânzarea biletelor și evidența curselor dintr-o autogară. Toate ghișeele lucrează pe o singură bază de date, așa că fiecare casier vede imediat locurile vândute de ceilalți, iar un loc nu poate fi vândut de două ori."),
    P("**Pentru cine este.** Sistemul are trei roluri:"),
    ...bullets([
      "**Casierul** caută cursele, alege locul pe schema autobuzului, vinde și anulează bilete și vede dashboard-ul zilei;",
      "**Administratorul** gestionează traseele, stațiile, harta, cursele, autobuzele, mentenanța, șoferii, reducerile și utilizatorii și vede rapoartele și jurnalul de audit;",
      "**Pasagerul** își cumpără biletul cu cardul și își vede biletele.",
    ]),
    P("**Modulele.** Vânzarea biletelor, Biletele (căutare și anulare), Curse, Trasee și Stații, Harta, Autobuze (cu editorul de locuri), Mentenanță, Șoferi, Reduceri, Utilizatori, Rapoarte, Jurnalul de audit și Dashboard-ul. Fereastra principală, cu meniul pe roluri, este prezentată în secțiunea 4.4."),
    P("**Cum funcționează.** Aplicația rulează pe calculatoarele de la ghișee și se conectează prin internet la serverul autogării, unde se află baza de date SQL Server și serverul de fișiere cu schemele autobuzelor și harta (Figura 3.1). Operațiile de vânzare, rezervare și anulare se execută pe server, în tranzacții. Dacă serverul nu răspunde, aplicația afișează ultimele date salvate, dar nu vinde."),
    P("**Cerințele.** Serverul: Windows, SQL Server, OpenSSH, IP fix și două porturi redirecționate. Stațiile: Windows 10/11 pe 64 de biți cu .NET 10 Desktop Runtime și internet. Cerințele complete sunt în secțiunea 1.5.1."),

    // ---------------- 6.4 Manual admin ----------------
    H(2, "6.4. Manualul administratorului"),
    P("Manualul se adresează persoanei care instalează și întreține sistemul. Detaliile tehnice și capturile sunt în capitolele 2 și 3; aici sunt doar pașii, în ordinea în care se fac."),
    H(3, "6.4.1. Instalarea serverului"),
    ...numbered([
      "Se instalează SQL Server cu autentificare mixtă și SQL Server Management Studio. În SQL Server Configuration Manager se activează TCP/IP, se șterge portul dinamic și se fixează portul 14330 (secțiunea 3.8.2).",
      "Se creează folderul pentru backup (în proiect `D:\\Backup\\autogara`) și se verifică că serviciul SQL Server poate scrie în el; lipsa lui a fost una din cauzele defectului D3.",
      "Se rulează scripturile din folderul `SQL Code` în ordine: `01_CreateDatabase.sql` … `10_BackupRestore.sql`. Scriptul `11_Seed.sql` adaugă date de demonstrație, iar `12_DateVolum.sql` date de volum pentru testare; la autogară ele se înlocuiesc cu datele reale.",
      "În scriptul de securitate (`08_Security.sql`) se setează parole noi pentru conturile `autogara_app` și `autogara_rapoarte`; parolele se păstrează doar la administrator.",
      "Se instalează OpenSSH Server din Windows, cu pornire automată, și se servește folderul `File-Server` (secțiunea 3.8.3).",
      "Pe router se redirecționează porturile 14330 și 44545 spre server, iar în firewall se permit doar porturile 14330 și 22 (secțiunea 3.8.4). Verificarea se face cu `Test-NetConnection` de pe alt calculator.",
      "Se verifică în SQL Server Agent că cele 6 joburi sunt activate și că serverul este pornit la ora backup-ului complet; altfel ora jobului se mută în programul de lucru.",
    ]),
    H(3, "6.4.2. Instalarea aplicației pe stații"),
    ...numbered([
      "Pe calculatorul de dezvoltare se publică aplicația:",
    ]),
    ...code("dotnet publish App\\Autogara.WinForms -c Release -r win-x64 --self-contained false -o publish"),
    ...numbered([
      "Pe fiecare stație se instalează .NET 10 Desktop Runtime și se copiază folderul `publish`.",
      "La prima pornire se completează configurarea stației: adresa și portul serverului SQL, baza `autogara`, contul aplicației, datele SFTP. Butonul „Testează conexiunea” trebuie să arate ambele conexiuni reușite (secțiunea 4.2). Parolele se salvează criptat DPAPI, numai pentru utilizatorul Windows curent.",
      "Se creează conturile casierilor din secțiunea Utilizatori, fiecare cu rolul lui și o parolă temporară, pe care casierul o schimbă la prima autentificare.",
    ], { continua: true }),
    H(3, "6.4.3. Întreținerea zilnică și lunară"),
    ...table(["Ce se verifică", "Unde", "Cât de des"], [
      ["joburile au rulat fără erori (backup, rezervări, ITP, spațiu pe disc)", "SQL Server Agent → Job Activity Monitor", "zilnic"],
      ["alertele de ITP și de spațiu pe disc", "Jurnal audit în aplicație; log-ul SQL Server", "zilnic"],
      ["cererile de resetare a parolei", "Utilizatori → Cereri de resetare", "zilnic"],
      ["un backup restaurat de probă pe o bază separată", "`10_BackupRestore.sql` (baza `autogara_TestRestaurare`)", "lunar"],
      ["spațiul liber pe discul serverului (minimum 10 %)", "jobul de verificare a spațiului", "săptămânal"],
      ["actualizările Windows și SQL Server", "Windows Update", "lunar"],
    ], [46, 36, 18], { caption: "Verificările periodice ale administratorului" }),
    H(3, "6.4.4. Restaurarea după o defecțiune"),
    ...numbered([
      "Se opresc stațiile de la ghișee (sau se lasă în modul offline).",
      "În SSMS se restaurează ultimul backup complet cu `NORECOVERY`, apoi ultimul backup diferențial cu `RECOVERY`.",
      "Se rulează `DBCC CHECKDB` și se compară numărul de rânduri cu ultimul raport.",
      "Se pornesc stațiile; ele revin singure online, fără reautentificare (secțiunea 5.7).",
    ]),
    H(3, "6.4.5. Diagnosticarea problemelor"),
    ...table(["Problema", "Cauza probabilă", "Ce se face"], [
      ["Stația arată „Mod local (offline)”", "serverul oprit, internetul căzut, IP-ul schimbat", "se verifică serverul și porturile cu Test-NetConnection"],
      ["„Fără file server”", "serviciul sshd oprit sau portul 44545 închis", "se pornește sshd; se verifică redirecționarea"],
      ["Backup-ul diferențial eșuează", "nu există backup complet de bază sau lipsește folderul", "se face manual un backup complet; se verifică folderul (D3)"],
      ["Un utilizator este blocat", "5 parole greșite în 15 minute", "se așteaptă 15 minute sau se generează o parolă temporară"],
      ["Eroare necunoscută la o stație", "—", "se citește jurnalul `%LOCALAPPDATA%\\Autogara\\Logs\\autogara-<data>.log`"],
    ], [28, 34, 38], { caption: "Problemele frecvente și rezolvarea lor" }),

    // ---------------- 6.5 Manual utilizator ----------------
    H(2, "6.5. Manualul utilizatorului"),
    H(3, "6.5.1. Casierul"),
    P("**Autentificarea.** Se introduc numele de utilizator și parola și se apasă Autentificare. La parola uitată se folosește legătura „Am uitat parola”: cererea ajunge la administrator, care dă o parolă temporară."),
    P("**Vânzarea unui bilet:**"),
    ...numbered([
      "În meniu se alege **Vânzare bilete**, apoi stația de plecare, stația de sosire și data; se apasă **Caută**.",
      "Se face dublu-clic pe cursa dorită. Se deschide schema autobuzului: locurile libere, rezervate de alți casieri și ocupate au culori diferite.",
      "Se face clic pe un loc liber. Locul este rezervat pentru 10 minute; cronometrul arată timpul rămas.",
      "Se completează numele și telefonul pasagerului, se alege reducerea (dacă este cazul), metoda de plată și numărul bonului fiscal. Totalul se calculează automat.",
      "Se apasă **F2** (Confirmă vânzarea). Biletul primește un cod unic, iar locul devine ocupat pe toate stațiile. **Esc** închide fereastra fără vânzare și eliberează locul.",
    ]),
    P("**Anularea unui bilet.** În **Bilete** se caută biletul după cod, se verifică suma care se rambursează (100 % cu cel puțin 24 de ore înainte de plecare, 50 % cu cel puțin 2 ore, altfel nimic) și se apasă **Anulează biletul**, apoi se confirmă."),
    P("**Dashboard-ul** arată cifrele zilei și următoarele curse; se reîmprospătează singur la 30 de secunde."),
    H(3, "6.5.2. Administratorul"),
    ...bullets([
      "**Curse**: se adaugă o cursă alegând traseul, autobuzul, șoferul, data, orele și prețul; locurile se creează automat după capacitatea autobuzului.",
      "**Trasee, Stații, Harta**: stațiile se pun pe hartă în editorul hărții, apoi se leagă prin drumuri cu distanța în km; traseul este lista ordonată a stațiilor.",
      "**Autobuze și Mentenanță**: la autobuz se completează numărul, modelul, capacitatea și data ITP, iar schema locurilor se desenează în editorul autobuzului; lucrările se trec în Mentenanță.",
      "**Utilizatori**: Adaugă, Modifică, Parolă temporară, Dezactivează; cererile de resetare apar în dreapta.",
      "**Rapoarte**: se alege tipul raportului și perioada, se apasă Afișează; raportul se poate exporta în CSV sau arhiva pe file server.",
      "**Jurnal audit**: se filtrează după perioadă și text.",
    ]),
    H(3, "6.5.3. Pasagerul"),
    P("Pasagerul se autentifică cu contul lui, alege **Cumpără bilet**, caută cursa și alege locul la fel ca un casier; numele este precompletat, iar plata se face doar cu cardul. În **Biletele mele** vede toate biletele lui și le poate anula după aceleași reguli de rambursare."),
    H(3, "6.5.4. Mesajele frecvente"),
    ...table(["Mesajul", "Ce înseamnă", "Ce se face"], [
      ["„Locul nu mai este liber”", "alt casier a rezervat sau a vândut locul între timp", "se alege alt loc"],
      ["„Numele este obligatoriu”, telefon invalid", "datele pasagerului lipsesc sau au format greșit", "se completează corect și se apasă din nou F2"],
      ["Rezervarea a expirat", "au trecut 10 minute de la alegerea locului", "se alege din nou locul"],
      ["Cont blocat temporar", "5 parole greșite în 15 minute", "se așteaptă sau se cere parolă temporară"],
      ["„Mod local (offline)”", "serverul nu răspunde", "se poate consulta orarul; vânzarea se reia când indicatorul revine la Online"],
      ["„Încercați din nou”", "conflict la vânzarea simultană pe aceeași cursă (D8)", "se repetă confirmarea"],
    ], [28, 38, 34], { caption: "Mesajele frecvente pentru utilizatori" }),

    // ---------------- 6.6 Proceduri ----------------
    H(2, "6.6. Procedurile manuale și automatizate"),
    P("Tabelul împarte procedurile sistemului între ce face omul și ce face sistemul singur și arată cine are instrucțiunea pentru fiecare."),
    ...table(["Procedura", "Tipul", "Executantul", "Frecvența", "Instrucțiunea"], [
      ["Vânzarea și anularea biletelor", "manuală, asistată", "casierul", "la cerere", "6.5.1"],
      ["Calculul prețului, al reducerii și al rambursării", "automată", "sistemul", "la fiecare bilet", "—"],
      ["Eliberarea rezervărilor expirate", "automată", "Job_ElibereazaRezervariExpirate", "la fiecare minut", "2.8"],
      ["Planificarea curselor", "manuală", "administratorul", "zilnic sau săptămânal", "6.5.2"],
      ["Crearea locurilor unei curse", "automată", "procedura stocată", "la crearea cursei", "—"],
      ["Backup-ul complet și diferențial", "automată", "Job_BackupComplet, Job_BackupDiferential", "zilnic; la 4 ore", "2.8, 2.9"],
      ["Verificarea backup-ului prin restaurare", "manuală", "administratorul", "lunar", "6.4.3"],
      ["Restaurarea după o defecțiune", "manuală", "administratorul", "la nevoie", "6.4.4"],
      ["Alerta de ITP și trecerea autobuzului în Service", "automată", "Job_VerificaExpirareITP", "zilnic, 07:00", "2.8"],
      ["Programarea autobuzului la ITP", "manuală", "administratorul, transportatorul", "la alertă", "—"],
      ["Curățarea jurnalului de audit mai vechi de 12 luni", "automată", "Job_CurataLogAudit", "lunar", "2.8"],
      ["Verificarea spațiului pe disc", "automată", "Job_VerificaSpatiuDisc", "la 6 ore", "2.8"],
      ["Rapoartele zilnice și lunare", "manuală (la cerere)", "administratorul, contabilul", "zilnic, lunar", "6.5.2"],
      ["Închiderea zilei: raportul de casă și bonurile fiscale", "manuală", "casierul, contabilul", "zilnic", "—"],
      ["Crearea conturilor și resetarea parolelor", "manuală", "administratorul", "la nevoie", "6.5.2"],
    ], [34, 14, 24, 16, 12], { caption: "Procedurile manuale și automatizate", size: 20 }),

    // ---------------- 6.7 Instruire ----------------
    H(2, "6.7. Instruirea personalului"),
    P("Instruirea se face înainte de pilotare, la autogară, pe stațiile de la ghișee conectate la o copie a bazei de date (ca baza `autogara_test` de la testare), ca exercițiile să nu modifice datele reale. Materialele sunt manualele din secțiunile 6.4 și 6.5."),
    ...table(["Grupul", "Durata", "Conținutul", "Evaluarea"], [
      ["Casierii (6 persoane, în două grupe)", "2 ore pe grupă", "autentificarea și schimbarea parolei, căutarea curselor, vânzarea cu alegerea locului, reducerile, anularea, dashboard-ul, mesajele frecvente, ce se face în modul offline", "fiecare casier vinde și anulează 3 bilete fără ajutor"],
      ["Dispecerii și administratorul (3 persoane)", "4 ore", "tot ce fac casierii, plus cursele, traseele, harta, autobuzele, mentenanța, șoferii, utilizatorii, rapoartele, jurnalul de audit", "planifică o cursă completă și scoate raportul zilei"],
      ["Contabilul și conducerea (2 persoane)", "1 oră", "rapoartele de vânzări și de ocupare, exportul CSV, jurnalul de audit", "obține raportul de casă pe o zi"],
      ["Tehnicianul / administratorul serverului (1 persoană)", "4 ore", "instalarea, joburile, backup-ul și restaurarea, diagnosticarea (6.4)", "face o restaurare de probă"],
    ], [24, 12, 44, 20], { caption: "Planul de instruire a personalului" }),

    // ---------------- 6.8 Pilotare ----------------
    H(2, "6.8. Condițiile organizatorice pentru pilotare"),
    P("Pilotarea verifică sistemul în lucrul real înainte de folosirea lui la toate ghișeele. Condițiile organizatorice sunt:"),
    ...bullets([
      "**Durata și locul:** două săptămâni, la un singur ghișeu, în paralel cu modul vechi de lucru la celelalte ghișee.",
      "**Responsabilii:** din partea autogării, dispecerul-șef (persoana de contact); din partea echipei, un membru de serviciu în programul de lucru.",
      "**Condițiile tehnice:** serverul pornit permanent și pe UPS, backup-ul complet mutat în programul de lucru, defectele D3 și D7 remediate, datele reale ale curselor introduse înainte de start.",
      "**Evidența:** fiecare problemă se notează într-un registru de pilotare, cu data, stația și pașii; jurnalele stațiilor se păstrează.",
      "**Criteriile de reușită:** niciun loc vândut de două ori; vânzarea unui bilet în cel mult 2 minute; backup-ul reușit în fiecare zi; totalul raportului zilnic egal cu raportul de casă.",
      "**Planul de revenire:** dacă un criteriu nu este îndeplinit, ghișeul pilot revine la modul vechi de lucru, iar biletele vândute în sistem rămân valabile și se trec în registrul casei.",
      "**Decizia de extindere:** la sfârșitul pilotării, conducerea și echipa semnează un proces-verbal; dacă toate criteriile sunt îndeplinite, sistemul se extinde la toate ghișeele.",
    ]),

    // ---------------- 6.9 Dictionar ----------------
    H(2, "6.9. Dicționarul de date"),
    P("Dicționarul de date descrie fiecare coloană din cele 17 tabele ale schemei `autogara`: tipul de date, cheile (PK = cheie primară, FK = cheie externă, UQ = valoare unică), valoarea implicită și semnificația. Tipurile, cheile și valorile implicite au fost extrase automat din scriptul `03_Tables.sql`, deci corespund exact bazei de date. Constrângerile CHECK sunt descrise în coloana Descrierea, iar relațiile dintre tabele sunt prezentate în Figura 2.4."),
    ...dictionarDate(),

    // ---------------- 6.10 Documentatia ----------------
    H(2, "6.10. Documentația predată"),
    ...table(["Documentul sau componenta", "Unde se află"], [
      ["Sarcina tehnică", "Sarcina Tehnica.docx"],
      ["Scripturile bazei de date (01–12)", "SQL Code/"],
      ["Codul sursă al aplicației (7 proiecte) și testele automate", "App/"],
      ["Fișierele serverului (schemele autobuzelor, harta)", "File-Server/"],
      ["Raportul de testare și programul de stres", "Testare/"],
      ["Raportul de grup, cu manualele (6.3–6.5) și dicționarul de date (6.9)", "Raport/raport_grup/raport_proiect_grup.docx"],
      ["Generatorul raportului și al diagramelor", "Raport/raport_grup/generator/"],
      ["Manualele pe roluri: utilizatori (pasageri), operatori-casieri, manageri, administratori, programator/DevOps", "Manuale/ (generatorul în Manuale/generator/)"],
      ["Modelele actului despre rezultatele experimentării și ale actului de recepție", "acest raport, secțiunea 6.11"],
    ], [60, 40], { caption: "Documentația și componentele predate (depozitul Git)" }),
    P("Starea defectelor la predare este cea din secțiunea 5.8: înainte de pilotare trebuie remediate D3 (backup-ul complet automat) și D7 (limita de autentificare), apoi D1 și datele de test rămase (D4, D5). După remediere se repetă testarea de regresie."),

    // ---------------- 6.11 Actele ----------------
    H(2, "6.11. Actele de experimentare și de recepție"),
    P("Integrarea sistemului la beneficiar se încheie cu două documente. Actul despre rezultatele experimentării se întocmește la sfârșitul pilotării (6.8) și arată dacă sistemul a îndeplinit criteriile de reușită. Actul de recepție se semnează după remedierea observațiilor și înseamnă trecerea sistemului în exploatarea curentă a autogării. Mai jos sunt modelele celor două acte, completate cu datele proiectului; rubricile cu linie se completează la pilotare."),
    H(3, "6.11.1. Actul despre rezultatele experimentării"),
    ...table(["Rubrica", "Conținutul"], [
      ["Denumirea sistemului", "Sistem informatic pentru gestionarea unei autogări („Autogara”), versiunea 1.0"],
      ["Beneficiarul", "autogara (persoana de contact: dispecerul-șef)"],
      ["Executantul", "echipa de proiect, grupa BD-423: Gojinevschii Dmitri, Hanganu Sergiu, Crivenco Alexandr, Sopivnic Maxim"],
      ["Perioada și locul experimentării", "două săptămâni, de la ____ până la ____, la ghișeul nr. ____, în paralel cu modul vechi de lucru"],
      ["Comisia", "din partea beneficiarului: dispecerul-șef, un casier, contabilul; din partea executantului: membrul de serviciu"],
      ["Condițiile asigurate", "serverul pornit permanent și pe UPS; backup-ul complet mutat în programul de lucru; defectele D3 și D7 remediate; datele reale ale curselor introduse"],
      ["Problemele constatate", "conform registrului de pilotare: ____ probleme, dintre care remediate ____"],
      ["Concluzia comisiei", "sistemul □ se recomandă pentru recepție   □ se recomandă după remedierea observațiilor   □ nu se recomandă"],
      ["Semnăturile", "beneficiarul ____________        executantul ____________        data ____________"],
    ], [26, 74], { caption: "Modelul actului despre rezultatele experimentării" }),
    ...table(["Criteriul de reușită (6.8)", "Valoarea cerută", "Rezultatul obținut", "Îndeplinit"], [
      ["Locuri vândute de două ori", "0", "____", "da / nu"],
      ["Durata vânzării unui bilet la ghișeu", "cel mult 2 minute", "____", "da / nu"],
      ["Backup-ul zilnic al bazei", "reușit în fiecare zi", "____ din ____ zile", "da / nu"],
      ["Totalul raportului zilnic față de raportul de casă", "egal în fiecare zi", "____", "da / nu"],
      ["Disponibilitatea serverului în programul de lucru", "fără întreruperi mai lungi de 15 minute", "____", "da / nu"],
    ], [38, 24, 24, 14], { caption: "Rezultatele experimentării pe criterii (anexă la act)" }),
    H(3, "6.11.2. Actul de recepție a sistemului informatic"),
    ...table(["Rubrica", "Conținutul"], [
      ["Obiectul recepției", "sistemul informatic „Autogara” 1.0: baza de date autogara, aplicația pentru stații, serverul de fișiere, joburile automate"],
      ["Temeiul", "caietul de sarcini (1.5) și actul despre rezultatele experimentării nr. ____ din ____"],
      ["Documentația predată", "conform Tabelului 6.25: scripturile SQL, codul sursă, manualele pe roluri, dicționarul de date, raportul de testare, acest raport"],
      ["Instruirea personalului", "efectuată conform planului din secțiunea 6.7; au fost instruiți ____ casieri și ____ administratori"],
      ["Observațiile rămase", "lista observațiilor nerezolvate, cu termenul de remediere: ____"],
      ["Garanția și mentenanța", "executantul remediază defectele găsite în perioada de garanție de ____ luni; întreținerea curentă se face după manualul administratorului (6.4)"],
      ["Decizia comisiei", "sistemul □ se recepționează   □ se recepționează cu condițiile de mai sus   □ nu se recepționează"],
      ["Semnăturile", "conducătorul autogării ____________        executantul ____________        data ____________"],
    ], [26, 74], { caption: "Modelul actului de recepție a sistemului informatic" }),
    P("La data raportului, sistemul este testat, iar actele sunt pregătite. Ele pot fi semnate doar după pilotarea la o autogară reală; până atunci, rezultatele testării din capitolul 5 țin locul rezultatelor experimentării."),

    // ---------------- 6.12 Concluzii ----------------
    H(2, "6.12. Concluzii la capitolul 6"),
    P("Performanțele măsurate arată că sistemul își atinge obiectivele principale: vânzarea concurentă fără bilete duble, răspunsuri în milisecunde și revenirea singură după căderea serverului. Rapoartele de stare au arătat abaterile fiecărei etape și deciziile luate. Pentru predare au fost pregătite manualele de prezentare, al administratorului și al utilizatorului, împărțirea procedurilor între om și sistem, planul de instruire, condițiile de pilotare, dicționarul de date și modelele actelor de experimentare și de recepție, astfel încât autogara să poată folosi și întreține sistemul fără echipa de proiect."),
  ];
};
