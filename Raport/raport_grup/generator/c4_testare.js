const fs = require("fs");
const path = require("path");
const { P, H, fig, table, startChapter } = require("./lib");
const T = JSON.parse(fs.readFileSync(path.join(__dirname, "..", "tabele_vechi.json"), "utf8"));
const capturi = fs.readdirSync(path.join(__dirname, "..", "img")).filter((f) => /^TC\d\d_/.test(f)).sort();

module.exports = () => {
  startChapter(4);
  const mediu = T[12], cazuri = T[13], defecte = T[14];
  const titluri = cazuri.slice(1).map((r) => r[1]);
  return [
    H(1, "4. TESTAREA"),
    P("Testarea a fost făcută de **Sopivnic Maxim** pe aplicația pornită de pe o stație de lucru, conectată la server prin adresa publică, cu baza de date populată cu date realiste. Fiecare caz de test are o captură de ecran ca dovadă; raportul complet se află și separat, în Testare/Raport_testare.docx."),
    H(2, "4.1. Mediul de testare"),
    ...table(mediu[0], mediu.slice(1), [22, 78], { caption: "Mediul de testare", size: 20 }),
    H(2, "4.2. Rezumat"),
    P("Au fost executate 40 de cazuri de test: 35 trecute, 1 trecut cu observație și 4 eșuate. Au fost găsite 6 defecte (1 major, 1 mediu, 3 minore, 1 informativ)."),
    P("Funcțiile principale lucrează corect: autentificarea și rolurile, vânzarea cu rezervare provizorie, reducerile, anularea cu rambursare după regula de 24 h / 2 h, concurența între două stații și expirarea rezervărilor. Datele se păstrează consistent în baza de date. Problema cea mai importantă ține de server: backup-ul automat nu funcționează (D3)."),
    P("Cazul cu numărul N are captura de ecran în figura 4.N, de la sfârșitul capitolului."),
    H(2, "4.3. Cazurile de test"),
    ...table(cazuri[0], cazuri.slice(1), [5, 15, 22, 18, 30, 10], { caption: "Cazurile de test", size: 18 }),
    H(2, "4.4. Defecte găsite"),
    ...table(defecte[0], defecte.slice(1).map((r) => r.map((c) => c.replace(/Fig\. 5\.(\d+)(–5\.(\d+))?/g, (m, a, b, c2) => `Fig. 4.${a}` + (c2 ? `–4.${c2}` : "")).replace(/, 5\.(\d+)/g, ", 4.$1"))), [5, 11, 50, 20, 14], { caption: "Defectele găsite la testare", size: 18 }),
    H(2, "4.5. Teste neexecutate"),
    P("Modul offline (pierderea conexiunii cu aplicația deschisă): necesită oprirea serviciului SQL Server sau deconectarea serverului de la rețea, ceea ce ar întrerupe lucrul celorlalți membri ai echipei. Se execută într-un interval convenit."),
    P("Restaurarea bazei de date pe serverul real: depinde de un backup complet valid, care lipsește (D3). Se reia după remedierea defectului D3."),
    H(2, "4.6. Concluzia testării"),
    P("Aplicația este pregătită pentru predare din punctul de vedere al funcționalității. Înainte de livrare trebuie remediate D3 (backup) și D1 (testul automat), iar datele de test (D4, D5) trebuie curățate din baza de date."),
    H(2, "4.7. Capturile de ecran ale testelor"),
    ...capturi.flatMap((f, i) => fig(f.replace(/\.png$/, ""), titluri[i] || f)),
  ];
};
