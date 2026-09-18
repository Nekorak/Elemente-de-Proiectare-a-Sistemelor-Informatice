<div align="center">

# Autogara — Aplicație de Gestionare a Autogării

Proiect de echipă · Elemente de Proiectare a Sistemelor Informatice

![.NET](https://img.shields.io/badge/.NET_10-WinForms-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/Microsoft_SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Model](https://img.shields.io/badge/model-waterfall-blue)
![Status](https://img.shields.io/badge/etapa_curent%C4%83-Testare-orange)

</div>

## Despre proiect

Aplicație desktop pentru gestionarea unei autogări:

- vânzarea biletelor, cu alegerea locului pe schema autobuzului
- rezervarea provizorie a locului, care expiră automat
- anularea și rambursarea biletelor; reduceri pentru elevi, pensionari și abonament
- harta stațiilor, editată vizual
- gestionarea curselor, traseelor, autobuzelor, șoferilor și mentenanței
- dashboard live și rapoarte
- roluri: Admin, Casier, Pasager

Aplicația rulează pe laptop (client). Computerul de acasă este serverul: pe el rulează MS SQL Server și file server-ul cu fișierele JSON, accesibile prin port-forwarding.

## Echipa

| Rol | Membru |
|---|---|
| Bază de Date | Gojinevschii Dmitri |
| Backend | Hanganu Sergiu |
| Frontend | Crivenco Alexandr |
| Tester | Sopivnic Maxim |

## ToDo

### Bază de Date — Gojinevschii Dmitri
- [x] Convențiile (denumire, collation, chei, audit, soft-delete, concurență)
- [x] Schema și tabelele, cu relații și constrângeri
- [x] Rolurile și securitatea SQL Server
- [x] Views
- [x] Proceduri stocate
- [x] Joburi SQL Server Agent
- [x] Indici
- [x] Scriptul de backup/restore (testat local)
- [ ] Testul de restaurare pe serverul real
- [x] Scriptul de creare a bazei de date și seed-ul
- [ ] Dicționarul de date

### Backend — Hanganu Sergiu
- [x] Stratul de acces la date și conexiunea la distanță
- [x] Autentificarea, rolurile și recuperarea parolei
- [x] Serviciile pentru hartă
- [x] Parsarea și validarea JSON-urilor autobuzelor
- [x] Rezervarea, vânzarea, anularea și rambursarea biletelor
- [x] Calculul prețurilor și al reducerilor
- [ ] Integrarea cu casa de marcat fiscală (dacă e necesară)
- [x] Rapoartele și datele pentru dashboard
- [x] Cache-ul local și modul offline
- [x] Logarea erorilor, tranzacțiile, limitarea brute-force

### Frontend — Crivenco Alexandr
- [x] Wizard-ul de configurare și login-ul
- [x] Editorul de hartă
- [x] Editorul de autobuz (generează JSON)
- [x] Formularul de vânzare cu harta locurilor
- [x] Formularele Admin
- [x] Dashboard-ul și rapoartele
- [x] Validările și mesajele de eroare
- [x] Indicatorul online/offline și auto-update-ul

### Tester — Sopivnic Maxim
- [ ] Planul de testare pe roluri
- [ ] Testarea editorului de hartă și a celui de autobuz
- [ ] Testarea vânzării, a rezervării provizorii și a anulării
- [ ] Testarea concurenței și a expirării rezervărilor
- [ ] Testarea conexiunii la distanță și a modului offline
- [ ] Testarea securității și a integrității datelor
- [ ] Testarea rapoartelor, a dashboard-ului și a joburilor SQL
- [ ] Testarea backup-ului și a restaurării
- [ ] Testarea de regresie înainte de livrare

## Structura repository-ului

```
App/                    soluția .NET (Backend + Frontend)
File-Server/            structura folderelor de pe file server
SQL Code/               scripturile bazei de date, numerotate în ordinea rulării
Sarcina Tehnica.docx    sarcina tehnică, aprobată de colegiu
plan.md                 planul tehnic detaliat al proiectului
Credentials.txt         credențialele proiectului (repo privat)
```

## Rularea bazei de date

Scripturile din `SQL Code` se rulează manual în SSMS, în ordinea numerelor (`01` → `11`). `10_BackupRestore.sql` se rulează doar la testul de restaurare. În `09_Jobs.sql` și `10_BackupRestore.sql`, verifică `@BackupPath` de la începutul scriptului.

Datele de test acoperă intervalul de la 7 zile în urmă până la 7 zile înainte față de ziua rulării.

Login-urile SQL și conturile din aplicație sunt în `Credentials.txt`.

## Backend (soluția `App/Autogara.slnx`)

| Proiect | Ce conține |
|---|---|
| `Autogara.Domain` | entitățile (1:1 cu tabelele) și enum-urile de status |
| `Autogara.Common` | configurarea stației (parole criptate DPAPI), excepțiile, log-ul, ora Moldovei |
| `Autogara.DataAccess` | `AutogaraDbContext` (EF Core), apelurile procedurilor stocate, traducerea erorilor SQL |
| `Autogara.FileServer` | clientul SFTP și citirea/scrierea JSON-urilor de autobuz și a hărții, cu validare |
| `Autogara.Business` | serviciile folosite de formulare, reunite în `AutogaraBackend` |
| `Autogara.WinForms` | interfața (Frontend): formularele, controalele desenate și iconițele SVG |
| `Autogara.Tests` | teste unitare (`dotnet test App/Autogara.slnx`) |

Frontend-ul folosește doar `AutogaraBackend`:

```csharp
var store = new AppSettingsStore();              // %LocalAppData%\Autogara\appsettings.local.json
if (!store.Exista)
{
    // FrmSetupWizard: precompletează cu AppSettings.Implicite(), testează cu
    // AutogaraBackend.TesteazaConfigurareaAsync(setari), apoi store.Salveaza(setari).
}

using var backend = AutogaraBackend.Creeaza(store.Citeste());
await backend.Auth.AutentificaAsync(utilizator, parola);          // backend.Sesiune.Utilizator.Rol → meniul
var curse = await backend.Curse.CautaCurseAsync(statiePlecare, statieSosire, data);
var locuri = await backend.Curse.HartaLocuriAsync(cursaId);       // pentru AutobuzSeatMapControl
var rezervare = await backend.Rezervari.RezervaLocAsync(locId);   // cronometru: rezervare.SecundeRamase
var bilet = await backend.Bilete.ConfirmaVanzareAsync(rezervare.RezervareID, vanzare);
```

Orice eroare se afișează cu `ExceptionHandler.MesajPrietenos(ex)` — mesajele sunt deja în română. `ValidareException.Erori` are lista completă, pentru `ErrorProvider`. Evenimentul `backend.Conexiune.ConexiuneSchimbata` vine de pe alt thread: în formular se folosește `BeginInvoke`.

Conturile din seed au parola în formatul vechi (SHA2_512); la prima autentificare reușită, backend-ul o transformă automat în PBKDF2. Recuperarea parolei: utilizatorul trimite o cerere din ecranul de login, iar un administrator o vede în `Auth.CereriResetareAsync()` și generează o parolă temporară cu `Auth.ReseteazaParolaAsync(...)`.

## Frontend (`App/Autogara.WinForms`)

Pornirea aplicației:

```bash
dotnet run --project App/Autogara.WinForms
```

La prima pornire se deschide configurarea stației (precompletată cu serverul echipei), apoi login-ul. Fereastra principală are meniul adaptat pe rol:

| Rol | Secțiuni |
|---|---|
| Admin | Dashboard, Vânzare, Bilete, Curse, Trasee, Stații, Harta, Autobuze, Mentenanță, Șoferi, Reduceri, Utilizatori, Rapoarte, Jurnal audit |
| Casier | Dashboard, Vânzare, Bilete |
| Pasager | Cumpără bilet (doar cu cardul), Biletele mele |

Scurtături: **F2** deschide vânzarea (în fereastra de vânzare, F2 confirmă biletul); **Esc** închide fereastra de vânzare și eliberează locul rezervat.

- Formularele: aspectul este în `*.Designer.cs`, iar în `*.cs` sunt doar evenimentele, apelurile la `AutogaraBackend` și afișarea rezultatului.
- Controale desenate: `AutobuzSeatMapControl` (harta locurilor), `AutobuzEditorControl` (editorul de locuri), `HartaCanvasControl` (editorul hărții), `IndicatorConexiune`, `CardKpi`, `ButonIcon`.
- Iconițele sunt SVG din setul [Lucide](https://lucide.dev) (licență ISC, în `Resurse/Iconite/LICENSE-lucide.txt`), incluse în aplicație și desenate cu biblioteca `Svg`. Culorile și fonturile comune sunt în `Ui/Tema.cs`.

## Reguli de lucru

- Lucrăm direct pe `main`, fără branch-uri.
- Înainte de lucru: `git pull`. După lucru: `git commit` și `git push`.
- Când o sarcină e gata, bifeaz-o în secțiunea ToDo.
- Toată logica stă în Backend; formularele doar apelează funcțiile acestuia.
