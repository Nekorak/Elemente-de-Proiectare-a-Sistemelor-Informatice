// Fragmente comune tuturor manualelor: convențiile de notare, legenda imaginilor, mesajele de eroare.
const { P, bullets, table } = require("./lib");

// Paragraf din introducere: despre capturi și machete
const despreImagini = () => [
  P("**Imaginile din manual.** Majoritatea figurilor sunt capturi de ecran din aplicația reală, făcute la testarea sistemului (13–22.10.2026), pe baza de date a echipei. Ferestrele care nu au fost fotografiate la testare sunt prezentate ca **machete**: ele sunt desenate automat din fișierele de proiectare ale ferestrelor (`*.Designer.cs`), cu aceleași câmpuri, butoane, texte și culori ca în aplicație, și sunt marcate în legendă cu „(machetă)”. Datele din imagini sunt datele de test ale proiectului (stațiile, traseele, autobuzele și conturile din scriptul `11_Seed.sql`); numele pasagerilor sunt fictive."),
];

const conventii = (extra = []) => [
  P("**Convențiile de notare** folosite în manual:"),
  ...table(["Notația", "Semnificația", "Exemplu"], [
    ["**Text îngroșat**", "numele unui buton, al unui câmp sau al unei secțiuni din meniu, exact ca pe ecran", "se apasă **Caută**"],
    ["„Text între ghilimele”", "un mesaj afișat de aplicație", "„Locul nu mai este liber”"],
    ["`Text monospațiat`", "un cod, un fișier, o comandă sau o valoare introdusă de la tastatură", "`AG261014-849CF994`"],
    ["F2, Esc, Enter", "o tastă de pe tastatură", "F2 confirmă vânzarea"],
    ["Meniu → Secțiune", "drumul până la o fereastră", "Bilete → Anulează biletul"],
    ...extra,
  ], [26, 48, 26], { caption: "Convențiile de notare din manual" }),
];

// Starea conexiunii (IndicatorConexiune)
const indicator = () => table(["Ce arată indicatorul", "Ce înseamnă", "Ce se poate face"], [
  ["„Se verifică...”", "aplicația tocmai a pornit și verifică serverul", "se așteaptă câteva secunde"],
  ["■ verde „Online”", "stația este conectată la baza de date și la serverul de fișiere", "toate operațiile"],
  ["■ galben „Fără file server”", "baza de date răspunde, serverul de fișiere nu", "vânzarea merge; schemele autobuzelor și harta se iau din copia locală"],
  ["■ roșu „Mod local (offline)”", "serverul nu răspunde (internet căzut, server oprit)", "se pot consulta cursele salvate local; vânzarea, rezervarea și anularea așteaptă revenirea conexiunii"],
], [30, 38, 32], { caption: "Stările indicatorului de conexiune din colțul ferestrei" });

const regulaRambursare = () => table(["Momentul anulării", "Se rambursează", "Statusul biletului după anulare"], [
  ["cu cel puțin 24 de ore înainte de plecare", "100 % din preț", "Rambursat"],
  ["între 24 de ore și 2 ore înainte de plecare", "50 % din preț", "Rambursat"],
  ["cu mai puțin de 2 ore înainte de plecare", "nimic (0 %)", "Anulat"],
  ["după plecarea cursei", "biletul nu mai poate fi anulat", "rămâne Activ"],
  ["cursa este anulată de autogară", "100 % din preț, automat", "Rambursat"],
], [40, 28, 32], { caption: "Regula de rambursare la anularea biletului" });

module.exports = { despreImagini, conventii, indicator, regulaRambursare };
