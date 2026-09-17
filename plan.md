# Plan tehnic detaliat — Aplicație Autogară

Document de referință intern (nu se livrează echipei ca atare — sarcina tehnică livrată echipei conține doar pașii de acțiune). Acest fișier păstrează toate deciziile tehnice detaliate, ca să pot continua lucrul pe proiect fără să le regândesc.

Stack: Microsoft SQL Server + .NET 10 WinForms (C#). Arhitectură: laptop = client, computer de acasă = server (MS SQL Server + file server), acces la distanță prin port-forwarding (fără VPN). Model de lucru: waterfall — fiecare etapă se finalizează complet, fără revenire ulterioară.

Echipă: Bază de Date — Gojinevschii Dmitri; Backend — Hanganu Sergiu; Frontend — Crivenco Alexandr; Tester — Sopivnic Maxim.

## Bază de Date (MS SQL Server)

### Schemă și convenții
- Schemă dedicată: `autogara`
- Collation: `Romanian_100_CI_AS`
- Convenție denumire: PascalCase, tabele la plural
- Chei primare: `INT IDENTITY` pentru entități interne; `UNIQUEIDENTIFIER` (GUID) pentru entități relevante pentru cache local/offline (Utilizatori, Bilete, RezervariProvizorii)
- Coloane audit: `CreatLa` (DATETIME2, default SYSUTCDATETIME()), `ModificatLa` (DATETIME2, NULL)
- Soft-delete prin coloana `Activ` (BIT) pe entitățile de referință
- Concurență: `ROWVERSION` pe Locuri, Bilete, Utilizatori
- Status-uri: CHECK constraints cu valori fixe, nu text liber

### Tabele

**Roluri**: RolID (INT PK IDENTITY), Denumire (NVARCHAR(50) NOT NULL UNIQUE — 'Admin','Casier','Pasager')

**Utilizatori**: UtilizatorID (UNIQUEIDENTIFIER PK DEFAULT NEWID()), NumeUtilizator (NVARCHAR(50) NOT NULL UNIQUE), ParolaHash (VARBINARY(256) NOT NULL), Nume, Prenume (NVARCHAR(100) NOT NULL), Email (NVARCHAR(150) NULL), Telefon (NVARCHAR(20) NULL), RolID (INT FK→Roluri), Activ (BIT DEFAULT 1), CreatLa, ModificatLa, RowVersion

**Noduri**: NodID (INT PK IDENTITY), Tip (NVARCHAR(20) CHECK IN ('Statie','Intersectie')), Nume, CoordX (FLOAT), CoordY (FLOAT), Activ

**Conexiuni**: ConexiuneID (INT PK IDENTITY), NodPlecareID, NodSosireID (FK→Noduri), DistantaKm (DECIMAL(6,2) CHECK > 0), UNIQUE(NodPlecareID, NodSosireID), CHECK NodPlecareID <> NodSosireID

**Statii**: StatieID (INT PK IDENTITY), NodID (FK→Noduri, UNIQUE), Adresa, Peron

**Autobuze**: AutobuzID (INT PK IDENTITY), NrInmatriculare (UNIQUE), Model, CapacitateLocuri (SMALLINT CHECK > 0), Status (CHECK IN ('Activ','Service','Scos din uz')), CaleFisierJSON (NVARCHAR(260) — cale pe file server), DataExpirareITP (DATE NULL), Activ

**MentenantaAutobuze**: MentenantaID (INT PK IDENTITY), AutobuzID (FK), TipLucrare, Data (DATE), Kilometraj (INT NULL), Observatii

**Soferi**: SoferID (INT PK IDENTITY), Nume, Prenume, NrPermis (UNIQUE), Telefon, Activ

**Trasee**: TraseuID (INT PK IDENTITY), Denumire (NVARCHAR(150)), Activ

**TraseuOpriri**: TraseuOprireID (INT PK IDENTITY), TraseuID (FK), StatieID (FK), Ordine (SMALLINT), UNIQUE(TraseuID, Ordine)

**TipuriReducere**: TipReducereID (INT PK IDENTITY), Denumire ('Elev','Pensionar','Abonament'), ProcentReducere (DECIMAL(5,2) CHECK BETWEEN 0 AND 100), Activ

**Curse**: CursaID (INT PK IDENTITY), TraseuID (FK), AutobuzID (FK), SoferID (FK), DataCursa (DATE), OraPlecare (TIME(0)), OraSosireEstimata (TIME(0) CHECK > OraPlecare), Pret (DECIMAL(10,2) CHECK >= 0), Status (CHECK IN ('Planificata','In desfasurare','Finalizata','Anulata')), CreatLa

**Locuri**: LocID (INT PK IDENTITY), CursaID (FK), NumarLoc (SMALLINT), Status (CHECK IN ('Liber','Rezervat','Ocupat')), RowVersion, UNIQUE(CursaID, NumarLoc)

**RezervariProvizorii**: RezervareID (UNIQUEIDENTIFIER PK DEFAULT NEWID()), LocID (FK, UNIQUE), UtilizatorID (FK NULL), DataCreare, DataExpirare (DATETIME2 NOT NULL)

**Bilete**: BiletID (UNIQUEIDENTIFIER PK DEFAULT NEWID()), CodBilet (UNIQUE), CursaID (FK), LocID (FK, UNIQUE), NumePasager, TelefonPasager, TipReducereID (FK NULL), VanzutDeUtilizatorID (FK), DataEmitere, Pret, Status (CHECK IN ('Activ','Anulat','Rambursat')), RowVersion

**Plati**: PlataID (BIGINT PK IDENTITY), BiletID (FK), Suma (DECIMAL(10,2) CHECK >= 0), MetodaPlata (CHECK IN ('Numerar','Card')), DataPlata, Status (CHECK IN ('Finalizata','Rambursata')), NumarBonFiscal (NULL)

**LogAudit**: LogID (BIGINT PK IDENTITY), UtilizatorID (FK NULL), Actiune (NVARCHAR(200)), Entitate, EntitateID, DataOra

### Roluri și securitate SQL Server
- `autogara_app`: db_datareader + db_datawriter + EXECUTE pe schema autogara (nu db_owner)
- `autogara_rapoarte` (opțional): doar db_datareader
- Cont DBA separat, folosit doar manual
- Autentificare SQL Server (nu Windows Auth), port non-implicit, conexiune criptată (SSL/TLS)

### Views
vw_LocuriDisponibile, vw_CurseActive, vw_VanzariZilnice, vw_OcupareCurse, vw_RapoarteComparative

### Proceduri stocate
sp_RezervaLoc, sp_ConfirmaVanzareBilet, sp_AnuleazaBilet, sp_ElibereazaRezervariExpirate, sp_CreeazaLocuriPentruCursa, sp_RaportVanzariZilnic, sp_RaportOcupareCurse, sp_AdaugaUtilizator, sp_DezactiveazaUtilizator, sp_InregistreazaLogAudit

### Joburi (SQL Server Agent)
Job_ElibereazaRezervariExpirate (1-2 min), Job_BackupComplet (zilnic), Job_BackupDiferential (la câteva ore), Job_CurataLogAudit (lunar), Job_VerificaExpirareITP (zilnic), Job_VerificaSpatiuDisc (periodic)

### Indici
IX_Curse_DataCursa_TraseuID, IX_Locuri_CursaID, IX_RezervariProvizorii_DataExpirare, IX_Bilete_CodBilet

### Backup
Backup complet zilnic + diferențial la câteva ore, păstrate 30 zile; backup separat pentru fișierele de pe file server; test de restaurare obligatoriu înainte de lansare.

## File Server

Server separat de partajare fișiere, pe același computer de acasă ca MS SQL Server, dar cu port dedicat propriu (redirecționat separat pe router față de portul SQL).

### Structură foldere
Folderul rădăcină pe server se numește `File-Server` (același nume ca folderul creat local în proiect, pentru consistență):
```
File-Server
  /Autobuze
    autobuz_{AutobuzID}.json      — câte un fișier per autobuz, structura locurilor
  /Harta
    harta.json                     — configurația unică a hărții (noduri + conexiuni)
  /Backup
    /Autobuze/AAAA-LL-ZZ/...       — copie a fișierelor JSON înainte de suprascriere
    /Harta/AAAA-LL-ZZ/...
```

### Schemă JSON — autobuz (`autobuz_{AutobuzID}.json`)
```json
{
  "autobuzId": 12,
  "randuri": 10,
  "coloane": 4,
  "culoarDupaColoana": 2,
  "locuri": [
    { "numarLoc": 1, "rand": 1, "coloana": 1 },
    { "numarLoc": 2, "rand": 1, "coloana": 2 }
  ]
}
```
Validare la citire: `randuri`/`coloane` > 0, `locuri.length` = capacitatea din tabelul Autobuze, `numarLoc` unic în listă.

### Schemă JSON — hartă (`harta.json`)
```json
{
  "noduri": [
    { "id": 1, "tip": "Statie", "nume": "Chisinau", "x": 120.0, "y": 340.0 }
  ],
  "conexiuni": [
    { "nodPlecareId": 1, "nodSosireId": 2, "distantaKm": 45.5 }
  ]
}
```
Acest fișier e o oglindă/export al tabelelor Noduri și Conexiuni din SQL — sursa de adevăr rămâne baza de date; fișierul e generat la salvare din editorul de hartă și recitit la pornirea aplicației (sau la cerere), util și ca backup rapid vizual în afara bazei de date.

### Alte fișiere dependente
- Fișier de configurare aplicație per stație de lucru (IP/DDNS server SQL, cale file server, port) — nu conține parola în clar
- Export-uri de rapoarte (Excel/PDF), arhivate opțional pe file server într-un folder `/Rapoarte/{an}/{luna}`
- Log-uri locale ale aplicației, dacă se centralizează (opțional) — `/Logs/{statie}/{data}.log`

### Permisiuni și acces
- Share SMB dedicat, cu cont de rețea separat de contul SQL Server (nu „Everyone”, nu cont de administrator)
- Acces citire/scriere limitat la contul aplicației; fără acces anonim
- Backup periodic al întregului folder `File-Server` către o locație externă (disc secundar/cloud), la fel ca backup-ul bazei de date

## Backend (C# .NET 10) — Hanganu Sergiu

Nu e un API web separat — e un set de proiecte C# (class libraries) în aceeași soluție cu WinForms, referențiate direct de UI. Motivul: aplicație desktop cu un singur tip de client, fără nevoie de server HTTP intermediar.

### Structură soluție
```
Autogara.sln
  Autogara.Domain        — entități (POCO) și enum-uri, fără dependențe externe
  Autogara.DataAccess    — DbContext (EF Core), mapări, acces la proceduri stocate
  Autogara.FileServer    — citire/scriere JSON (autobuze, hartă) pe share-ul de rețea
  Autogara.Business      — servicii de business, orchestrează DataAccess + FileServer
  Autogara.Common        — configurare, retry/Polly, logare, excepții
  Autogara.WinForms      — proiectul UI (Frontend)
  Autogara.Tests         — teste unitare pentru Business (folosit și de Tester)
```

### Autogara.Domain — entități și enum-uri
- Entități: `Utilizator`, `Rol`, `Nod`, `Conexiune`, `Statie`, `Autobuz`, `MentenantaAutobuz`, `Sofer`, `Traseu`, `TraseuOprire`, `TipReducere`, `Cursa`, `Loc`, `RezervareProvizorie`, `Bilet`, `Plata`, `LogAudit` — proprietăți 1:1 cu coloanele din SQL (vezi secțiunea Bază de Date)
- Enum-uri: `RolTip` (Admin, Casier, Pasager), `StatusLoc` (Liber, Rezervat, Ocupat), `StatusBilet` (Activ, Anulat, Rambursat), `StatusCursa` (Planificata, InDesfasurare, Finalizata, Anulata), `StatusPlata` (Finalizata, Rambursata), `MetodaPlata` (Numerar, Card) — sincronizate 1:1 cu valorile din CHECK constraints SQL

### Autogara.DataAccess
- `AutogaraDbContext : DbContext` — un `DbSet<T>` per entitate, Fluent API pentru chei, relații, `RowVersion` ca `IsRowVersion()` concurrency token
- `ConnectionStringProvider` — citește connection string-ul din configurația locală a stației (nu hardcodat), suportă `Encrypt=True`
- Apeluri către procedurile stocate prin `FromSqlRaw`/`ExecuteSqlRawAsync` (nu LINQ-to-SQL pentru operațiile critice — rezervare, vânzare, anulare — ca să garanteze aceeași logică transacțională ca la ghișeu, indiferent de client)
- Politică de retry (Polly / `EnableRetryOnFailure` din EF Core) pentru erori tranzitorii de rețea

### Autogara.FileServer
- `JsonAutobuzRepository`: `ReadAsync(caleFisier)`, `WriteAsync(caleFisier, structura)` — validează schema JSON înainte de scriere, retry pe erori de rețea
- `JsonHartaRepository`: `ReadAsync()`, `WriteAsync(hartaDto)` — citește/scrie `harta.json`
- Cale de rețea (UNC, ex. `\\ServerAcasa\File-Server\...`) configurabilă, separată de connection string-ul SQL

### Autogara.Business — servicii (cu metodele lor principale)
- `AuthService` — `AutentificaAsync(user, parola)`, `SchimbaParolaAsync(...)`, `GenereazaTokenResetareAsync(...)`; parole hash-uite (PBKDF2/BCrypt), niciodată în clar
- `CursaService` — `CautaCurseAsync(statiePlecare, statieSosire, data)`, `CreeazaCursaAsync(dto)` (apelează `sp_CreeazaLocuriPentruCursa`), `AnuleazaCursaAsync(cursaId)`
- `RezervareService` — `RezervaLocAsync(locId, utilizatorId, durataMinute)` (apelează `sp_RezervaLoc`), `ElibereazaRezervareAsync(rezervareId)`
- `BiletService` — `ConfirmaVanzareAsync(rezervareId, dto)` (apelează `sp_ConfirmaVanzareBilet`), `AnuleazaBiletAsync(biletId)` (apelează `sp_AnuleazaBilet`, aplică politica de rambursare pe bază de timp rămas), `GenereazaCodBilet()`, `CalculeazaPret(pretBaza, tipReducereId)`
- `HartaService` — `IncarcaHartaAsync()`, `SalveazaHartaAsync(hartaDto)` (scrie în SQL, apoi rescrie `harta.json`)
- `AutobuzService` — `CreeazaAutobuzAsync(dto, structuraJson)`, `ActualizeazaStructuraAsync(autobuzId, structuraJson)`, `IncarcaStructuraAsync(autobuzId)`
- `MentenantaService` — `AdaugaInregistrareAsync(dto)`, `AutobuzeCuItpApropiatAsync(zileInainte)`
- `RaportService` — `VanzariZilniceAsync(data)`, `OcupareCurseAsync(start, stop)`, `RapoarteComparativeAsync(...)`
- `DashboardService` — `DateLiveAsync()` (curse active, locuri vândute, încasări)
- `AuditService` — `InregistreazaAsync(utilizatorId, actiune, entitate, entitateId)` (apelează `sp_InregistreazaLogAudit`)
- `ConexiuneStareService` — verifică periodic accesul la SQL și file server, expune eveniment `ConexiuneSchimbata` pentru UI
- `CacheLocalService` — salvare/citire locală (SQLite sau fișiere) pentru mod offline temporar, sincronizare la revenirea conexiunii

### Autogara.Common
- `AppSettings` — connection string, cale file server, porturi — citite dintr-un fișier de configurare local per stație (nu parola în clar; opțional criptat cu DPAPI)
- `RetryPolicy` (Polly) pentru operații SQL/rețea
- `Logger` (Serilog) — fișier local per stație, cu nivel configurabil
- `ExceptionHandler` central — traduce excepțiile tehnice (SQL, rețea) în mesaje prietenoase pentru UI

### Reguli transversale
- Fiecare scriere pe `Locuri`/`Bilete`/`Utilizatori` verifică `RowVersion`; la conflict (`DbUpdateConcurrencyException`) operația e reîncercată sau utilizatorul e informat că altcineva a modificat între timp
- Verificare de rol înainte de orice operație administrativă (ex. `RequireRol(RolTip.Admin)`), nu doar ascunderea butonului în UI
- Toate operațiile critice (vânzare, anulare, rezervare) rulează într-o singură tranzacție SQL — nu se face „save parțial”

## Frontend (WinForms) — Crivenco Alexandr

### Formulare
- `FrmSetupWizard` — configurare inițială (IP/DDNS server, cale file server, test conexiune)
- `FrmLogin` — autentificare, redirecționare pe rol
- `FrmRecuperareParola`
- `FrmMain` — shell cu meniu adaptat pe rol (admin vede tot; casier vede vânzare/anulare; pasager, dacă aplică, vede doar căutare/bilete proprii)
- `FrmCautareCurse` — filtre stație plecare/sosire, dată
- `FrmVanzareBilet` — conține `AutobuzSeatMapControl` pentru alocarea vizuală a locului
- `FrmCasier` — vânzare/anulare rapidă, selector reduceri, cronometru rezervare provizorie
- `FrmEditorHarta` — conține `HartaCanvasControl`
- `FrmEditorAutobuz` — conține `AutobuzEditorControl`, salvează structura ca JSON
- `FrmAdminRute`, `FrmAdminAutobuze`, `FrmAdminMentenanta`, `FrmAdminSoferi`, `FrmAdminStatii`, `FrmAdminTrasee`, `FrmAdminCurse`, `FrmAdminUtilizatori` — CRUD pe fiecare entitate
- `FrmDashboard` — date live (curse active, locuri vândute, încasări)
- `FrmRapoarte` — selecție interval/rută, tabel, export Excel/PDF, rapoarte comparative

### Controale custom
- `HartaCanvasControl` (`UserControl`) — desenare noduri prin click, trasare conexiuni prin drag între noduri, `OnPaint` custom (GDI+/`Graphics`), evenimente `NodAdaugat`, `ConexiuneCreata`
- `AutobuzSeatMapControl` (`UserControl`) — grilă de celule/butoane pe baza JSON-ului autobuzului, colorare după `StatusLoc`, eveniment `LocSelectat`
- `AutobuzEditorControl` — variantă editabilă a hărții de locuri (adăugare/ștergere rând-coloană, poziționare culoar)
- `ConnectionStatusIndicator` — control mic, indicator vizual conectat/deconectat/mod local, legat de `ConexiuneStareService`

### Separare strictă UI / Logică

Regulă obligatorie pentru tot proiectul WinForms, ca Frontend-ul să rămână doar „stil + apeluri”, iar Backend-ul să dețină toată logica:

- **Backend = fișiere cu funcții.** Toată logica (validări de business, calcule, acces la date, acces la fișiere) stă exclusiv în `Autogara.Business` (și, prin el, în `Autogara.DataAccess`/`Autogara.FileServer`). Fiecare formular apelează funcții gata definite acolo (ex. `biletService.ConfirmaVanzareAsync(...)`) — niciodată SQL, JSON sau reguli de business scrise direct în formular.
- **Frontend = doar apeluri.** Codul din spatele formularului (`FrmX.cs`) conține exclusiv: evenimente (`Click`, `Load` etc.), apelul către funcția corespunzătoare din Backend, și afișarea rezultatului/erorii în controale. Nicio regulă de business, niciun calcul, nicio interogare — doar „ia datele de pe ecran → cheamă funcția → pune rezultatul pe ecran”.
- **Stilurile și elementele UI stau în Designer, nu în logică.** Tot ce ține de aspect — poziția controalelor, mărime, culori, fonturi, anchors/dock, text implicit — se configurează din WinForms Designer și rămâne în fișierul generat `FrmX.Designer.cs`. Fișierul de logică `FrmX.cs` nu setează proprietăți vizuale (culori, poziții, fonturi) decât atunci când ele depind de o stare din date (ex. culoarea unui loc: liber/ocupat) — și chiar și atunci, prin proprietăți/metode clar denumite, nu prin cod de layout amestecat cu logica.
- Excepție: `HartaCanvasControl` și `AutobuzSeatMapControl` desenează dinamic (`OnPaint`), deci acolo desenarea ține de control, nu de Designer — dar și acolo, culorile/formele vin din constante/temă centralizată, nu hardcodate inline în formulare.
- Apeluri de rețea/SQL făcute `async`/`await`, cu marshalling către UI thread unde e nevoie (`this.Invoke`), și un indicator de progres pe operațiile mai lungi
- Validare de format pe formulare cu `ErrorProvider` (câmpuri obligatorii, format telefon/email) — validarea de business (ex. „locul e deja ocupat”) rămâne tot în Backend, formularul doar afișează mesajul returnat
- Scurtături de tastatură pentru casier (ex. F2 vânzare rapidă, Esc anulare pas curent), pentru viteză la ghișeu
- Auto-update: verificare versiune la pornire (fișier `version.json` pe file server), descărcare/instalare dacă există una nouă

## Tester
Plan pe roluri, test editor hartă, test editor autobuz, test vizualizare locuri, test conexiune la distanță, test mod offline, test acces file server, test concurență, test rezervare provizorie/expirare, test reduceri, test politică anulare, test casă de marcat, test integritate date, test securitate, test joburi SQL, test dashboard, test rapoarte, test backup/restore, test auto-update, test rezoluții ecran, urmărire bug-uri și regresie.
