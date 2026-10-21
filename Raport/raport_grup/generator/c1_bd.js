const { P, H, bullets, code, fig, table, startChapter } = require("./lib");

const tabele = [
  ["Roluri", "BD10_tabel_Roluri", "Nomenclatorul rolurilor din aplicație. Constrângerea CHECK permite doar valorile Admin, Casier și Pasager, iar denumirea este unică. Rolul determină meniul afișat după autentificare și operațiile permise de backend."],
  ["Utilizatori", "BD11_tabel_Utilizatori", "Conturile aplicației (administratori, casieri, pasageri). Cheia primară este un GUID, numele de utilizator este unic și are minimum 3 caractere, iar e-mailul este verificat cu un model LIKE. Parola nu se păstrează: coloana ParolaHash (VARBINARY) conține doar hash-ul. Coloana Activ face ștergerea „logică” (contul se dezactivează, istoricul rămâne), iar ROWVERSION detectează modificările concurente făcute de doi administratori."],
  ["Noduri", "BD12_tabel_Noduri", "Punctele de pe harta proprie a stațiilor: stații și intersecții (CHECK pe Tip), cu coordonatele X și Y folosite de editorul hărții. Din noduri și conexiuni se calculează distanțele traseelor."],
  ["Conexiuni", "BD13_tabel_Conexiuni", "Drumurile dintre două noduri, cu distanța în kilometri. Două chei externe spre Noduri, o constrângere de unicitate pe perechea de noduri, un CHECK că nodurile sunt diferite și unul că distanța este pozitivă."],
  ["Statii", "BD14_tabel_Statii", "Detaliile stațiilor de autobuz: adresa și peronul. Fiecare stație este legată de exact un nod de pe hartă (UNIQUE pe NodID), deci numele și poziția vin din Noduri, fără dublare."],
  ["Autobuze", "BD15_tabel_Autobuze", "Parcul de autobuze: numărul de înmatriculare (unic), modelul, capacitatea, statusul (Activ, Service, Scos din uz) și data expirării ITP. Coloana CaleFisierJSON indică fișierul cu așezarea locurilor de pe file server (Autobuze/autobuz_{id}.json)."],
  ["MentenantaAutobuze", "BD16_tabel_MentenantaAutobuze", "Istoricul lucrărilor de mentenanță: tipul lucrării, data, kilometrajul (nenegativ) și observațiile, legate de autobuz prin cheie externă."],
  ["Soferi", "BD17_tabel_Soferi", "Șoferii, cu numărul permisului de conducere unic și telefonul. Ca la utilizatori, ștergerea este logică, prin coloana Activ."],
  ["Trasee", "BD18_tabel_Trasee", "Traseele (de exemplu „Chișinău – Bălți”), cu denumire unică. Opririle traseului sunt păstrate separat, în TraseuOpriri."],
  ["TraseuOpriri", "BD19_tabel_TraseuOpriri", "Tabelul de legătură dintre trasee și stații, cu ordinea opririlor. Două constrângeri UNIQUE garantează că o poziție și o stație apar o singură dată pe același traseu. Prima și ultima oprire dau stația de plecare și de sosire."],
  ["TipuriReducere", "BD20_tabel_TipuriReducere", "Reducerile acordate la vânzare (Elev 50 %, Pensionar 30 %, Abonament 20 %), cu procentul limitat între 0 și 100 printr-un CHECK."],
  ["Curse", "BD21_tabel_Curse", "Cursele concrete: traseul, autobuzul, șoferul, data, ora de plecare și de sosire, prețul și statusul (Planificată, În desfășurare, Finalizată, Anulată). Constrângerile verifică ora de sosire după cea de plecare și prețul nenegativ."],
  ["Locuri", "BD22_tabel_Locuri", "Locurile fiecărei curse, create automat după capacitatea autobuzului. Statusul (Liber, Rezervat, Ocupat) este punctul central al vânzării, iar ROWVERSION și blocările din proceduri împiedică vânzarea dublă. Numărul locului este unic în cadrul cursei."],
  ["RezervariProvizorii", "BD23_tabel_RezervariProvizorii", "Rezervările de câteva minute făcute când casierul alege un loc. Un loc poate avea o singură rezervare (UNIQUE pe LocID), iar data expirării trebuie să fie după data creării. Rezervările expirate sunt șterse de jobul SQL Server Agent."],
  ["Bilete", "BD24_tabel_Bilete", "Biletele vândute: codul unic al biletului, cursa, locul, pasagerul, reducerea, utilizatorul care a vândut, prețul final și statusul (Activ, Anulat, Rambursat). Cheia externă compusă (LocID, CursaID) garantează că locul aparține cursei, iar un index filtrat unic permite un singur bilet activ pe loc."],
  ["Plati", "BD25_tabel_Plati", "Plățile asociate biletelor: suma, metoda (Numerar sau Card), statusul (Finalizată sau Rambursată) și numărul bonului fiscal. O anulare cu rambursare adaugă o plată nouă cu statusul Rambursată, deci istoricul încasărilor nu se pierde."],
  ["LogAudit", "BD26_tabel_LogAudit", "Jurnalul de audit: cine, ce acțiune, pe ce entitate și când. Aici ajung autentificările (reușite și eșuate), vânzările, anulările, modificările de date și mesajele joburilor. Contul aplicației nu poate modifica sau șterge înregistrările (DENY UPDATE, DELETE)."],
];

const viziuni = [
  ["vw_LocuriDisponibile", "BD40_vw_LocuriDisponibile", "Locurile libere ale curselor planificate care nu au plecat încă, împreună cu traseul, prețul și fișierul JSON al autobuzului. Condiția de timp folosește funcțiile fn_MomentCursa și fn_AcumLocal, deci ora Moldovei, nu ora UTC a serverului."],
  ["vw_CurseActive", "BD41_vw_CurseActive", "Cursele de azi și cele viitoare, cu stația de plecare și de sosire (prima și ultima oprire, obținute cu OUTER APPLY), autobuzul, șoferul și numărul de locuri libere, rezervate și ocupate. Este sursa listei de curse din fereastra de vânzare și din dashboard."],
  ["vw_VanzariZilnice", "BD42_vw_VanzariZilnice", "Totalurile pe zi și pe metoda de plată: bilete vândute, rambursări, suma încasată, suma rambursată și încasările nete. Ziua se calculează în ora locală, din data plății păstrată în UTC."],
  ["vw_OcupareCurse", "BD43_vw_OcupareCurse", "Gradul de ocupare al fiecărei curse (locuri ocupate raportate la capacitate), biletele active și anulate și încasările cursei. Pe ea se bazează raportul de ocupare și viziunea comparativă."],
  ["vw_RapoarteComparative", "BD44_vw_RapoarteComparative", "Comparația lunară pe trasee: numărul de curse, biletele, încasările și ocuparea medie, alături de valorile lunii precedente, obținute cu funcția de fereastră LAG, și variația în procente."],
];

const joburi = [
  ["Job_ElibereazaRezervariExpirate", "la fiecare minut", "execută sp_ElibereazaRezervariExpirate: locurile cu rezervarea expirată redevin libere; 2 reîncercări la eroare"],
  ["Job_BackupComplet", "zilnic, 02:00", "BACKUP DATABASE cu CHECKSUM (și COMPRESSION, unde ediția permite), apoi RESTORE VERIFYONLY; al doilea pas șterge fișierele mai vechi de 30 de zile"],
  ["Job_BackupDiferential", "la 4 ore, 06:00–22:00", "backup diferențial în timpul programului autogării, verificat cu RESTORE VERIFYONLY"],
  ["Job_CurataLogAudit", "lunar, ziua 1, 03:30", "șterge din LogAudit înregistrările mai vechi de 12 luni, în loturi de 5 000, și notează câte au fost șterse"],
  ["Job_VerificaExpirareITP", "zilnic, 07:00", "trece în Service autobuzele cu ITP expirat și scrie alerte în jurnal pentru cele care expiră în 14 zile"],
  ["Job_VerificaSpatiuDisc", "la 6 ore", "verifică spațiul liber pe discurile bazei de date (sub 5 GB sau sub 10 %) și scrie o alertă în jurnal și în log-ul SQL Server"],
];

module.exports = () => {
  startChapter(1);
  return [
    H(1, "1. BAZA DE DATE"),
    P("Compartimentul a fost realizat de **Gojinevschii Dmitri**. Baza de date este nucleul sistemului: toate stațiile de lucru citesc și scriu aceleași date, iar regulile importante (locuri, prețuri, rambursări, drepturi) sunt verificate chiar de SQL Server, indiferent de client. Serverul este Microsoft SQL Server, instanța numită **EPSI**, pe computerul echipei [[MsSql]]. Toate obiectele au fost create prin scripturi T-SQL numerotate, păstrate în folderul `SQL Code` din depozitul Git."),

    H(2, "1.1. Crearea bazei de date și convențiile"),
    P("Scriptul `01_CreateDatabase.sql` creează baza **autogara** cu collation-ul `Romanian_100_CI_AS` (sortare și comparare corecte pentru diacriticele românești, fără a ține cont de majuscule), modelul de recuperare SIMPLE, verificarea paginilor prin CHECKSUM și opțiunea READ_COMMITTED_SNAPSHOT, prin care citirile nu blochează scrierile casierilor. Scriptul `02_Schema.sql` creează schema dedicată **autogara**, în care stau toate obiectele."),
    ...code(`CREATE DATABASE autogara COLLATE Romanian_100_CI_AS;
ALTER DATABASE autogara SET RECOVERY SIMPLE;
ALTER DATABASE autogara SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE;
ALTER DATABASE autogara SET PAGE_VERIFY CHECKSUM;`),
    ...fig("BD02_proprietati_baza", "Proprietățile bazei de date autogara, schema dedicată și fișierele bazei"),
    P("Convențiile stabilite înainte de crearea tabelelor:"),
    ...bullets([
      "denumiri PascalCase, tabelele la plural (Curse, Bilete), cheile primare `<Entitate>ID`;",
      "chei INT IDENTITY pentru nomenclatoare și GUID (UNIQUEIDENTIFIER) pentru utilizatori, rezervări și bilete, care pot fi generate și pe stația de lucru;",
      "coloane de audit CreatLa / ModificatLa pe tabele, cu ora în UTC; conversia în ora Moldovei se face prin funcții;",
      "ștergere logică prin coloana Activ, ca istoricul vânzărilor să rămână complet;",
      "ROWVERSION pe tabelele cu modificări concurente (Utilizatori, Locuri, Bilete);",
      "toate constrângerile au nume explicite (PK_, FK_, UQ_, CK_, DF_), ca mesajele de eroare să poată fi traduse în aplicație.",
    ]),
    P("Scripturile se rulează în ordinea numerotării: 01–04 baza, schema, tabelele și indicii; 05–07 funcțiile, viziunile și procedurile; 08 securitatea; 09 joburile; 10 backup-ul cu test de restaurare; 11–12 datele de test și de volum."),
    ...fig("BD91_scripturi_proiect", "Scripturile bazei de date din folderul SQL Code, în ordinea rulării"),
    ...fig("BD03_obiecte_pe_tipuri", "Numărul obiectelor din schema autogara, pe tipuri, și numărul de rânduri din fiecare tabel"),

    H(2, "1.2. Tabelele și relațiile dintre ele"),
    P("Schema are 17 tabele, legate prin chei externe. Ele se împart în patru grupe: utilizatorii și jurnalul (Roluri, Utilizatori, LogAudit), harta și traseele (Noduri, Conexiuni, Statii, Trasee, TraseuOpriri), flota (Autobuze, MentenantaAutobuze, Soferi) și vânzarea (Curse, Locuri, RezervariProvizorii, TipuriReducere, Bilete, Plati). Diagrama de mai jos arată cheile primare, cheile externe și direcția fiecărei relații."),
    ...fig("ER_diagrama", "Diagrama relațiilor dintre cele 17 tabele ale schemei autogara", { maxH: 760 }),
    ...fig("BD01_object_explorer_tabele", "Tabelele schemei autogara în Object Explorer (SQL Server Management Studio)"),
    ...fig("BD05_object_explorer_tabel_coloane", "Coloanele, cheile, constrângerile și indicii tabelului Bilete în Object Explorer"),
    P("Pentru fiecare tabel urmează rolul lui, constrângerile importante și o captură cu structura (coloane, tipuri, NULL), constrângerile cu definițiile lor și un eșantion de date reale din baza de lucru."),
    ...tabele.flatMap(([t, cod, text], i) => [
      H(3, `1.2.${i + 1}. Tabelul ${t}`),
      P(text),
      ...fig(cod, `Tabelul autogara.${t}: structura, constrângerile și datele`),
    ]),

    H(2, "1.3. Indicii"),
    P("Pe lângă indicii creați automat de cheile primare și de constrângerile UNIQUE, scriptul `04_Indexes.sql` adaugă 15 indici nonclustered, aleși după interogările frecvente ale aplicației: căutarea curselor pe dată și traseu, harta locurilor unei curse, rezervările care expiră, căutarea biletului după cod, rapoartele pe zi și jurnalul pe perioadă. Mulți folosesc coloane incluse (INCLUDE), ca interogarea să fie servită complet din index."),
    P("Doi indici sunt filtrați: `UX_Bilete_LocID_Activ` este unic doar pentru biletele cu statusul Activ, deci pe un loc poate exista un singur bilet activ, dar oricâte anulate; `IX_Autobuze_DataExpirareITP` cuprinde doar autobuzele active, pentru alertele de ITP."),
    ...code(`CREATE UNIQUE INDEX UX_Bilete_LocID_Activ
    ON autogara.Bilete (LocID)
    WHERE Status = N'Activ';

CREATE INDEX IX_Curse_DataCursa_TraseuID
    ON autogara.Curse (DataCursa, TraseuID)
    INCLUDE (OraPlecare, OraSosireEstimata, Pret, Status, AutobuzID, SoferID);`),
    ...fig("BD30_indici", "Cei 15 indici nonclustered: coloanele cheie, coloanele incluse și filtrul"),
    ...fig("BD31_index_folosit", "Planul de execuție al căutării curselor pe o zi: SQL Server folosește indexul IX_Curse_DataCursa_TraseuID (Index Seek)"),

    H(2, "1.4. Funcțiile"),
    P("Scriptul `05_Functions.sql` conține 6 funcții scalare, folosite de viziuni și de proceduri, ca aceeași regulă să fie scrisă într-un singur loc:"),
    ...table(["Funcția", "Ce întoarce"], [
      ["fn_AcumLocal()", "data și ora curentă în fusul orar al Moldovei (E. Europe Standard Time)"],
      ["fn_UtcLaLocal(@MomentUtc)", "conversia unei ore UTC din tabele în ora locală"],
      ["fn_LocalLaUtc(@MomentLocal)", "conversia inversă, pentru filtrarea pe zile locale"],
      ["fn_MomentCursa(@Data, @Ora)", "momentul plecării cursei, din data și ora păstrate separat"],
      ["fn_CalculeazaPret(@PretBaza, @TipReducereID)", "prețul după reducere, rotunjit la doi zecimali"],
      ["fn_ProcentRambursare(@Plecare, @Moment)", "100 % cu cel puțin 24 h înainte, 50 % între 24 h și 2 h, 0 % sub 2 h"],
    ], [42, 58], { caption: "Funcțiile schemei autogara" }),
    ...code(`CREATE OR ALTER FUNCTION autogara.fn_ProcentRambursare(@PlecareLocal DATETIME2(0), @MomentLocal DATETIME2(0))
RETURNS DECIMAL(5, 2)
AS
BEGIN
    DECLARE @Minute INT = DATEDIFF(MINUTE, @MomentLocal, @PlecareLocal);
    RETURN CASE WHEN @Minute >= 24 * 60 THEN 100
                WHEN @Minute >= 2 * 60  THEN 50
                ELSE 0 END;
END;`),
    ...fig("BD35_functii", "Apelul celor 6 funcții: ora locală, prețul cu fiecare reducere și procentul de rambursare în trei situații"),

    H(2, "1.5. Viziunile (views)"),
    P("Cele 5 viziuni din `06_Views.sql` adună datele din mai multe tabele pentru listele și rapoartele aplicației. Aplicația le citește ca pe niște tabele obișnuite (Entity Framework le mapează ca entități fără cheie), iar contul de rapoarte are acces doar la citire."),
    ...fig("BD32_object_explorer_programmability", "Viziunile, procedurile stocate și funcțiile schemei autogara în Object Explorer"),
    ...viziuni.flatMap(([v, cod, text], i) => [
      H(3, `1.5.${i + 1}. ${v}`),
      P(text),
      ...fig(cod, `Rezultatul viziunii autogara.${v}`),
    ]),
    P("Definiția unei viziuni poate fi deschisă direct din Object Explorer (Script View as → ALTER To). Viziunea vw_CurseActive, de exemplu, folosește trei OUTER APPLY: prima oprire, ultima oprire și numărarea locurilor pe status."),
    ...fig("BD45_definitie_view", "Codul viziunii vw_CurseActive deschis din Object Explorer"),

    H(2, "1.6. Procedurile stocate"),
    P("Operațiile critice sunt scrise ca proceduri stocate în `07_StoredProcedures.sql`. Fiecare procedură de modificare folosește `SET XACT_ABORT ON`, o tranzacție cu TRY/CATCH și blocări explicite (UPDLOCK, HOLDLOCK) pe rândurile citite, deci două stații care încearcă simultan același loc nu pot trece amândouă de verificare. Erorile de business sunt aruncate cu THROW, cu coduri 50010–50066 și mesaje în română, pe care aplicația le afișează ca atare."),
    ...table(["Procedura", "Rolul"], [
      ["sp_RezervaLoc", "rezervă provizoriu un loc liber (1–60 de minute) și întoarce ID-ul rezervării și ora expirării"],
      ["sp_ConfirmaVanzareBilet", "transformă rezervarea în bilet: calculează prețul, generează codul AGaallzz-XXXXXXXX, ocupă locul, înregistrează plata și jurnalul"],
      ["sp_AnuleazaBilet", "anulează biletul, calculează rambursarea, eliberează locul și adaugă plata de rambursare"],
      ["sp_ElibereazaRezervariExpirate", "eliberează locurile cu rezervări expirate (folosită de job)"],
      ["sp_CreeazaLocuriPentruCursa", "creează locurile unei curse după capacitatea autobuzului"],
      ["sp_RaportVanzariZilnic", "vânzările unei zile pe traseu, casier și metodă de plată"],
      ["sp_RaportOcupareCurse", "ocuparea curselor pe un interval, opțional pe un traseu"],
      ["sp_AdaugaUtilizator", "creează un cont, cu verificarea rolului și a unicității numelui"],
      ["sp_DezactiveazaUtilizator", "dezactivează un cont, fără a permite dezactivarea ultimului administrator"],
      ["sp_InregistreazaLogAudit", "scrie o înregistrare în jurnalul de audit"],
    ], [34, 66], { caption: "Procedurile stocate ale schemei autogara" }),
    ...fig("BD50_proceduri_lista", "Cele 10 proceduri stocate și parametrii lor (OUTPUT marchează valorile întoarse)"),
    ...fig("BD56_definitie_procedura", "Codul procedurii sp_RezervaLoc: tranzacția, blocarea rândului și verificările"),
    P("Pentru capturile de mai jos procedurile au fost executate pe baza reală, dar în interiorul unei tranzacții închise cu ROLLBACK: se vede efectul fiecărui pas, iar la final datele rămân neschimbate."),
    H(3, "1.6.1. Rezervarea și vânzarea biletului"),
    P("`sp_RezervaLoc` verifică existența locului, starea cursei și ora plecării, eliberează o rezervare deja expirată pe același loc, apoi trece locul în starea Rezervat. `sp_ConfirmaVanzareBilet` verifică rezervarea, cursa, utilizatorul și reducerea, calculează prețul prin `fn_CalculeazaPret`, generează un cod unic, ocupă locul și scrie biletul, plata și jurnalul în aceeași tranzacție."),
    ...code(`SELECT @StatusLoc = l.Status, @StatusCursa = c.Status,
       @Plecare = autogara.fn_MomentCursa(c.DataCursa, c.OraPlecare)
FROM autogara.Locuri AS l WITH (UPDLOCK, HOLDLOCK)
JOIN autogara.Curse  AS c ON c.CursaID = l.CursaID
WHERE l.LocID = @LocID;
...
IF @StatusLoc <> N'Liber'
    THROW 50023, N'Locul nu mai este liber (a fost rezervat sau vândut între timp).', 1;`),
    ...fig("BD51_sp_RezervaLoc_ConfirmaVanzare", "sp_RezervaLoc și sp_ConfirmaVanzareBilet: locul rezervat, biletul emis cu reducerea de pensionar, plata, jurnalul, apoi ROLLBACK"),
    H(3, "1.6.2. Anularea și rambursarea"),
    P("`sp_AnuleazaBilet` refuză biletele deja anulate și cursele plecate, calculează procentul cu `fn_ProcentRambursare`, schimbă statusul biletului în Rambursat (sau Anulat, dacă suma e zero), eliberează locul și adaugă plata de rambursare. A doua anulare a aceluiași bilet este respinsă cu eroarea 50041."),
    ...fig("BD52_sp_AnuleazaBilet", "sp_AnuleazaBilet: biletul înainte și după anulare, plata de rambursare și respingerea celei de-a doua anulări"),
    H(3, "1.6.3. Crearea locurilor și eliberarea rezervărilor"),
    P("`sp_CreeazaLocuriPentruCursa` generează numerele de la 1 la capacitatea autobuzului fără bucle, dintr-un tabel de cifre, și nu dublează locurile existente. `sp_ElibereazaRezervariExpirate` șterge rezervările expirate cu indicația READPAST (nu așteaptă după rândurile blocate de casieri) și readuce locurile în starea Liber."),
    ...fig("BD53_sp_ElibereazaRezervari_CreeazaLocuri", "sp_CreeazaLocuriPentruCursa pentru o cursă nouă și sp_ElibereazaRezervariExpirate pe o rezervare expirată, apoi ROLLBACK"),
    H(3, "1.6.4. Procedurile de raportare"),
    P("Procedurile de raportare doar citesc. `sp_RaportVanzariZilnic` calculează intervalul zilei locale în UTC și grupează plățile pe traseu, casier și metodă; `sp_RaportOcupareCurse` filtrează viziunea vw_OcupareCurse pe un interval și validează datele primite."),
    ...fig("BD54_sp_Rapoarte", "Rezultatul procedurilor sp_RaportVanzariZilnic și sp_RaportOcupareCurse"),
    H(3, "1.6.5. Conturile și jurnalul de audit"),
    P("`sp_AdaugaUtilizator` primește hash-ul parolei deja calculat de aplicație și refuză un nume de utilizator existent; `sp_DezactiveazaUtilizator` eliberează și rezervările contului dezactivat și nu permite dezactivarea propriului cont sau a ultimului administrator activ. Toate trei scriu în jurnal prin `sp_InregistreazaLogAudit`."),
    ...fig("BD55_sp_Utilizatori_LogAudit", "Crearea și dezactivarea unui cont de test, înregistrările din jurnal și respingerea unui nume duplicat, apoi ROLLBACK"),

    H(2, "1.7. Rolurile, utilizatorii și securitatea"),
    P("Instanța EPSI acceptă autentificare mixtă și ascultă pe portul TCP **14330**, nu pe 1433, cel căutat primul din exterior. Conexiunile aplicației sunt criptate (Encrypt=True). Scriptul `08_Security.sql` creează câte un login pentru fiecare utilizare, după principiul drepturilor minime [[MsSqlSecurity]]:"),
    ...table(["Login", "Cine îl folosește", "Drepturi în baza autogara"], [
      ["autogara_app", "aplicația, pe toate stațiile", "db_datareader, db_datawriter, EXECUTE pe schemă; DENY ALTER pe schemă, DENY CREATE TABLE/VIEW/PROCEDURE/FUNCTION, DENY UPDATE/DELETE pe LogAudit"],
      ["autogara_rapoarte", "rapoarte, doar citire", "db_datareader; DENY INSERT/UPDATE/DELETE pe schemă; fără acces la coloana ParolaHash"],
      ["dba_gojinevschii", "administratorul bazei", "sysadmin, folosit doar manual"],
      ["dev_hanganu", "Backend", "citire, scriere, EXECUTE, VIEW DEFINITION, SHOWPLAN; fără modificarea jurnalului"],
      ["dev_crivenco", "Frontend", "citire, EXECUTE, VIEW DEFINITION"],
      ["test_sopivnic", "Tester", "citire, scriere, EXECUTE, VIEW DEFINITION; în msdb: SQLAgentOperatorRole (pornirea și istoricul joburilor)"],
    ], [20, 22, 58], { caption: "Login-urile SQL ale proiectului și drepturile lor" }),
    ...fig("BD63_object_explorer_logins", "Login-urile proiectului în Object Explorer → Security → Logins"),
    ...fig("BD60_securitate_loginuri", "Login-urile SQL: tipul, baza implicită, starea, apartenența la sysadmin și politica de parole"),
    ...fig("BD61_securitate_utilizatori_roluri", "Utilizatorii bazei autogara și rolurile de bază de date în care sunt membri"),
    ...fig("BD62_securitate_permisiuni", "Permisiunile explicite GRANT și DENY din 08_Security.sql"),
    ...fig("BD64_proprietati_login", "Login Properties pentru autogara_app, pagina User Mapping"),
    P("Drepturile au fost verificate practic, conectând sqlcmd cu fiecare cont: autogara_app nu poate crea sau șterge tabele, iar autogara_rapoarte nu poate modifica date și nu poate executa proceduri."),
    ...fig("TC35_sql_drepturi_autogara_app", "Contul autogara_app: fără db_owner, CREATE TABLE și DROP TABLE respinse, SELECT permis"),
    ...fig("TC36_sql_drepturi_autogara_rapoarte", "Contul autogara_rapoarte: SELECT permis, UPDATE și EXECUTE respinse (eroarea 229)"),
    P("La nivelul datelor, parolele utilizatorilor aplicației nu se păstrează niciodată în clar: în coloana ParolaHash ajunge doar hash-ul PBKDF2 calculat de aplicație (capitolul 2.6.1). Jurnalul de audit nu poate fi modificat de aplicație, iar constrângerile CHECK, FOREIGN KEY și UNIQUE opresc datele invalide chiar dacă ar veni din alt client."),
    ...fig("TC37_sql_constrangeri_integritate", "Constrângerile de integritate opresc un status invalid, o cursă inexistentă, un cod duplicat, un preț negativ și orele inversate"),

    H(2, "1.8. Joburile SQL Server Agent"),
    P("Scriptul `09_Jobs.sql` creează 6 joburi în categoria „Autogara”, cu proprietarul sa [[MsSqlAgent]]. Scriptul este reluabil: șterge mai întâi joburile cu aceleași nume, apoi le creează din nou cu pașii și programul lor."),
    ...table(["Job", "Program", "Ce face"], joburi, [30, 18, 52], { caption: "Joburile SQL Server Agent ale proiectului" }),
    ...fig("BD73_object_explorer_joburi", "Joburile proiectului în Object Explorer → SQL Server Agent → Jobs"),
    ...fig("BD70_joburi_lista", "Joburile, pașii lor, programul și rezultatul ultimei rulări"),
    ...fig("BD75_proprietati_job", "Programul jobului Job_ElibereazaRezervariExpirate (Job Properties → Schedules)"),
    ...fig("BD74_job_activity_monitor", "Job Activity Monitor: starea joburilor, ultima și următoarea rulare"),
    ...fig("BD71_joburi_istoric", "Istoricul rulărilor: rulări reușite și eșuate pe fiecare job și ultimele mesaje"),
    P("Istoricul arată starea reală a joburilor pe serverul echipei. Jobul de eliberare a rezervărilor rulează în fiecare minut fără erori. Jobul de verificare a spațiului pe disc apare și cu rulări eșuate, dar acestea sunt intenționate: când spațiul liber scade sub 10 % (pe server, 9,7 %), jobul se oprește cu RAISERROR, ca alerta să fie vizibilă în istoric, în log-ul SQL Server și în LogAudit. Jobul de backup complet, programat la 02:00, nu a rulat niciodată, deoarece calculatorul-server este oprit noaptea; din această cauză toate cele 17 rulări ale backup-ului diferențial au eșuat (erorile 3035 și 3013: nu există un backup complet de bază). Joburile lunare și cel de verificare a ITP nu au ajuns încă la prima rulare."),
    ...fig("BD71b_cauza_esec_backup", "Mesajul de eroare al jobului Job_BackupDiferential: lipsește backup-ul complet de bază"),
    P("Efectul jobului de eliberare se vede în jurnalul de audit: la fiecare rulare în care găsește rezervări expirate, el scrie câte locuri a eliberat."),
    ...fig("BD72_job_eliberare_efect", "Înregistrările scrise de Job_ElibereazaRezervariExpirate în LogAudit și rezervările provizorii active"),

    H(2, "1.9. Backup-ul și restaurarea"),
    P("Strategia de backup are două niveluri: backup complet zilnic și backup diferențial la 4 ore în timpul programului, ambele cu CHECKSUM și verificate imediat cu RESTORE VERIFYONLY; fișierele mai vechi de 30 de zile se șterg automat [[MsSqlBackup]]. Scriptul `10_BackupRestore.sql` face testul de restaurare: un backup COPY_ONLY, restaurarea lui într-o bază separată (autogara_TestRestaurare), DBCC CHECKDB și compararea numărului de rânduri din fiecare tabel cu originalul, apoi ștergerea bazei de test."),
    ...fig("BD81_script_backup", "Scriptul 10_BackupRestore.sql: backup, restaurare într-o bază separată, CHECKDB și comparare"),
    ...fig("BD80_backup_istoric", "Istoricul backup-urilor bazei autogara din msdb și modelul de recuperare"),
    P("În `msdb` sunt înregistrate doar două backup-uri complete: cel COPY_ONLY făcut de scriptul de test al restaurării (17.09, 4,7 MB, în `D:\\Backup\\autogara`) și unul făcut pe 08.10 de un program extern prin VSS. Baza are modelul de recuperare SIMPLE, deci nu se fac backup-uri ale jurnalului de tranzacții. Concluzia practică: backup-ul complet trebuie mutat într-o oră în care serverul este pornit (de exemplu 22:30) sau rulat manual după fiecare zi de lucru; până atunci, protecția datelor se bazează pe backup-urile manuale."),
    P("La testarea din 21.10.2026 s-a mai găsit o cauză: folderul `D:\\Backup\\autogara`, în care scriu joburile și scriptul de restaurare, nu mai exista pe server, iar primul backup de test a eșuat cu eroarea de sistem 3 („calea nu a fost găsită”). Folderul a fost creat din nou, apoi backup-ul COPY_ONLY, verificarea și restaurarea într-o copie separată (autogara_test) au reușit fără erori (secțiunea 4.2). Joburile de backup trebuie urmărite după această corectare."),

    H(2, "1.10. Datele de test și de volum"),
    P("Scriptul `11_Seed.sql` populează baza cu date de pornire: rolurile, conturile echipei și de test, harta cu stațiile reale din nordul și centrul Moldovei, traseele, autobuzele, șoferii, reducerile și cursele pentru două săptămâni. Scriptul `12_DateVolum.sql` adaugă aproximativ 10 000 de bilete realiste pe ultimele 45 de zile și următoarele 14, cu nume generate, casieri pe ture și rambursări, ca rapoartele și indicii să fie verificate pe un volum apropiat de cel real."),
    ...fig("BD90_date_volum", "Volumul datelor: numărul de rânduri, conturile pe roluri, perioada curselor și biletele pe status"),

    H(2, "1.11. Concluzii la capitolul 1"),
    P("Baza de date autogara respectă convențiile stabilite la început și păstrează integritatea datelor prin constrângeri, nu doar prin aplicație. Operațiile critice sunt tranzacționale și protejate la concurență, ceea ce a fost confirmat și la testare (până la 20 de stații simultane pe același loc, fără niciun bilet dublu). Securitatea se bazează pe conturi separate cu drepturi minime, iar sarcinile repetitive sunt automatizate prin SQL Server Agent. Ca puncte de îmbunătățit au rămas backup-ul complet automat, care nu a rulat niciodată pentru că serverul este oprit noaptea (și, din această cauză, backup-ul diferențial eșuează), și dicționarul de date ca document separat."),
  ];
};
