# Capturi de ecran pentru raportul proiectului de grup (Autogara / EPSI)

Scopul: capturi REALE de pe acest PC (serverul proiectului), pentru raportul pe capitole
Baza de date / Backend + DevOps / Frontend. Fiecare captură are un cod (BD.., BE.., DO.., FE..)
și se salvează ca PNG cu exact acel cod ca prefix, de ex. `BD11_tabel_Utilizatori.png`.

## Reguli
- NU se atinge folderul `C:\Users\nekor\Desktop\Welbor` și nici alte proiecte.
- NU se modifică date reale: în aplicație nu se salvează nimic, nu se vinde, nu se anulează, nu se șterge.
  Ferestrele modale se deschid doar pentru captură și se închid cu „Renunță” / Esc.
  Scripturile SQL din `sql/` fie doar citesc, fie lucrează într-o tranzacție care se termină cu ROLLBACK.
  Dacă un script dă eroare, corectează-l minim (tot doar citire / ROLLBACK) și notează în `jurnal_capturi.md`.
- Nu se rulează backup, restore, joburi, scripturile 01–12 din `SQL Code`, `git push` pe main sau `git commit` în folderul proiectului.
- Folderul de lucru: `C:\Users\nekor\Documents\Raport_Autogara\` (creează-l; copiază aici acest pachet).
  Capturile finale: `C:\Users\nekor\Documents\Raport_Autogara\capturi\`.
- Capturile se fac pe fereastra întreagă a aplicației (maximizată, 1920×1080), cu PrintWindow / CopyFromScreen
  pe dreptunghiul ferestrei, fără bara de activități. După fiecare captură, deschide PNG-ul și verifică
  că se vede ce trebuie (textul lizibil, rezultatele vizibile, nimic acoperit). Refă dacă nu.
- Parolele: conturile SQL și conturile din aplicație sunt în `Credentials.txt` și în `Testare/Raport_testare.docx`
  (Admin `m.sopivnic`, Casier `tatiana.rusu`, Pasager `ana.popa`). Nu captura ecrane cu parole în clar.
- Scrie în `capturi\jurnal_capturi.md` câte o linie pe captură: cod, ce arată, observații
  (de ex. „job-ul de backup complet nu a rulat niciodată”). Aceste observații ajung în raport.

## La final: trimiterea capturilor
Același clone temporar în care ai găsit acest pachet (NU folderul proiectului de pe Desktop):
```
cd C:\Users\nekor\Documents\Raport_Autogara
git clone https://github.com/Nekorak/Elemente-de-Proiectare-a-Sistemelor-Informatice.git repo_tmp
cd repo_tmp
git checkout -b claude/project-thread-dtgc0s
mkdir Raport\capturi ; copy ..\capturi\* Raport\capturi\
git add Raport/capturi ; git commit -m "Raport de grup: capturi de ecran (BD, Backend, DevOps, Frontend)"
git push -u origin claude/project-thread-dtgc0s
```
(Git Credential Manager: folosește contul Nekorak.) Dacă ramura există deja pe remote, fă `git fetch` și pune commit-ul peste ea.

---

## A. Baza de date — SSMS (preferat) sau sqlcmd

Preferat: SQL Server Management Studio conectat la `localhost\EPSI` (Windows auth sau `dba_gojinevschii`),
fereastra maximizată, Object Explorer vizibil în stânga, scriptul deschis în editor, executat (F5),
rezultatele în grilă (Results to Grid). Dacă SSMS nu se poate automatiza, fallback: o fereastră
PowerShell maximizată cu `sqlcmd -S localhost\EPSI -E -d autogara -f 65001 -W -s " | " -i <fisier>`
(ca în capturile de testare existente), cu comanda vizibilă în captură.

| Cod | Ce se capturează |
|---|---|
| BD01_object_explorer_tabele | SSMS: Object Explorer → localhost\EPSI → Databases → autogara → Tables extins (cele 17 tabele autogara.*) |
| BD02_proprietati_baza | `sql/BD02_proprietati_baza.sql` |
| BD03_obiecte_pe_tipuri | `sql/BD03_obiecte_pe_tipuri.sql` |
| BD04_diagrama | Opțional: dacă există deja o diagramă în Database Diagrams, deschide-o. NU crea una nouă (ar modifica baza). |
| BD10 … BD26 (17 capturi) | câte un script `sql/BD1x_tabel_<Tabel>.sql` / `sql/BD2x_tabel_<Tabel>.sql`; numele capturii = numele scriptului |
| BD05_object_explorer_tabel_coloane | Object Explorer: autogara.Bilete extins → Columns, Keys, Constraints, Indexes |
| BD30_indici | `sql/BD30_indici.sql` |
| BD31_index_folosit | `sql/BD31_index_folosit.sql` cu „Include Actual Execution Plan” (Ctrl+M); captura pe tab-ul Execution plan |
| BD32_object_explorer_programmability | Object Explorer: Views, Programmability → Stored Procedures, Functions → Scalar-valued Functions extinse |
| BD35_functii | `sql/BD35_functii.sql` |
| BD40 … BD44 | cele 5 viziuni, scripturile `sql/BD4x_vw_*.sql` |
| BD45_definitie_view | Object Explorer → Views → vw_CurseActive → Script View as → ALTER To → New Query Editor Window (codul viziunii în editor) |
| BD50_proceduri_lista | `sql/BD50_proceduri_lista.sql` |
| BD51 … BD55 | scripturile `sql/BD5x_*.sql` (demonstrații cu ROLLBACK; în captură să se vadă toate seturile de rezultate — dacă nu încap, fă 2 capturi: `_a`, `_b`) |
| BD56_definitie_procedura | Object Explorer → sp_RezervaLoc → Modify (codul procedurii în editor, partea cu tranzacția și UPDLOCK) |
| BD60 … BD62 | securitatea: `sql/BD6x_*.sql` |
| BD63_object_explorer_logins | Object Explorer: Security → Logins extins (cele 6 login-uri ale proiectului vizibile) |
| BD64_proprietati_login | Login Properties pentru `autogara_app` → pagina User Mapping (autogara bifat, db_datareader + db_datawriter). Închide cu Cancel |
| BD70 … BD72 | joburile: `sql/BD7x_*.sql` |
| BD73_object_explorer_joburi | Object Explorer: SQL Server Agent → Jobs extins (cele 6 joburi Job_*) |
| BD74_job_activity_monitor | SQL Server Agent → Job Activity Monitor (stare, ultima rulare, următoarea rulare). Închide după |
| BD75_proprietati_job | Job Properties pentru Job_ElibereazaRezervariExpirate → pagina Schedules. Cancel |
| BD80_backup_istoric | `sql/BD80_backup_istoric.sql` |
| BD81_script_backup | `SQL Code/10_BackupRestore.sql` deschis în editor (NU executat) |
| BD90_date_volum | `sql/BD90_date_volum.sql` |
| BD91_scripturi_proiect | SSMS sau Explorer: folderul `SQL Code` cu scripturile 01–12 (soluția `SQL Code.slnx` în SSMS Solution Explorer, dacă se deschide) |

## B. Backend — cod în Visual Studio

Deschide `App\Autogara.slnx` în Visual Studio (sau, dacă VS nu e instalat, VS Code), fereastra maximizată,
Solution Explorer în dreapta. Pentru fiecare captură: deschide fișierul, Ctrl+G → linia indicată,
așază linia în partea de sus a editorului. Nu modifica și nu salva fișiere.

| Cod | Fișier (din `App\`) | Linia | Ce să se vadă |
|---|---|---|---|
| BE01_solutie_proiecte | Solution Explorer | – | cele 7 proiecte, fiecare extins un nivel |
| BE02_referinte_proiecte | `Autogara.Business/Autogara.Business.csproj` | 1 | ProjectReference + PackageReference |
| BE03_domain_entitate | `Autogara.Domain/Entitati/Bilet.cs` | 1 | entitatea Bilet (1:1 cu tabelul) |
| BE04_domain_valori | `Autogara.Domain/Enumerari/ValoriDb.cs` | 1 | statusurile, ca în CHECK-urile din BD |
| BE05_common_connection | `Autogara.Common/Configurare/AppSettings.cs` | 35 | ConnectionString() și Valideaza() (NU liniile 18–33, conțin parole) |
| BE06_common_dpapi | `Autogara.Common/Configurare/AppSettingsStore.cs` | 26 | Citeste/Salveaza/Cripteaza/Decripteaza cu ProtectedData |
| BE07_common_jurnal_erori | `Autogara.Common/Jurnal.cs` și `ExceptionHandler.cs` (2 tab-uri; captură pe ExceptionHandler, sau două capturi `_a`/`_b`) | 1 | Serilog cu păstrare 30 zile; mesaje prietenoase |
| BE08_dataaccess_context | `Autogara.DataAccess/AutogaraDbContext.cs` | 169 | maparea Loc și Bilet (RowVersion, conversia statusurilor) |
| BE09_dataaccess_retry_tranzactii | `Autogara.DataAccess/BazaDeDate.cs` | 11 | EnableRetryOnFailure, InTranzactieAsync |
| BE10_dataaccess_proceduri | `Autogara.DataAccess/ProceduriStocate.cs` | 13 | RezervaLocAsync și ConfirmaVanzareBiletAsync cu parametri OUTPUT |
| BE11_dataaccess_erori_sql | `Autogara.DataAccess/EroriSql.cs` | 13 | traducerea erorilor SQL (50xxx, rețea, ROWVERSION) |
| BE12_fileserver_sftp | `Autogara.FileServer/SftpFileClient.cs` | 88 | ScrieTextCuBackupAsync și ExecutaAsync (reconectare, reîncercare) |
| BE13_fileserver_validare | `Autogara.FileServer/Validare.cs` | 9 | validarea autobuz_{id}.json |
| BE14_business_backend | `Autogara.Business/AutogaraBackend.cs` | 20 | constructorul care leagă toate serviciile |
| BE15_business_auth | `Autogara.Business/Servicii/AuthService.cs` | 26 | AutentificaAsync, blocarea după 5 încercări |
| BE16_business_parole | `Autogara.Business/Securitate/ParolaHasher.cs` | 15 | PBKDF2-SHA256, 210 000 iterații |
| BE17_business_vanzare | `Autogara.Business/Servicii/BiletService.cs` | 38 | ConfirmaVanzareAsync, VanzareRapidaAsync, EstimeazaAnulareAsync |
| BE18_business_pret | `Autogara.Business/Reguli/PretCalculator.cs` | 1 | reducerea și regula de rambursare 24 h / 2 h |
| BE19_business_cache | `Autogara.Business/Servicii/CacheLocalService.cs` | 15 | ObtineAsync (server → cache local) |
| BE20_business_conexiune | `Autogara.Business/Servicii/ConexiuneStareService.cs` | 14 | verificarea periodică SQL + SFTP |
| BE21_business_rapoarte | `Autogara.Business/Servicii/RaportService.cs` | 59 | ExportCsv și ArhiveazaPeFileServerAsync |
| BE22_business_actualizare | `Autogara.Business/Servicii/ActualizareService.cs` | 1 | auto-update prin version.json |
| BE23_teste_explorer | Test Explorer în VS după „Run All Tests” (dacă VS e disponibil; altfel sari peste, există DO12) | – | lista testelor și rezultatul |

## C. DevOps — PowerShell / unelte Windows (fereastră PowerShell maximizată, comanda vizibilă)

| Cod | Comandă / acțiune |
|---|---|
| DO01_servicii | `Get-Service 'MSSQL$EPSI','SQLAgent$EPSI','SQLBrowser','sshd' \| Format-Table Name,DisplayName,Status,StartType -AutoSize` |
| DO02_sql_config_manager_tcp | SQL Server Configuration Manager → SQL Server Network Configuration → Protocols for EPSI → TCP/IP → Properties → IP Addresses, derulat la IPAll (TCP Port 14330). Cancel. Fallback: `Get-ItemProperty` din registry pentru `...\MSSQL*.EPSI\MSSQLServer\SuperSocketNetLib\Tcp\IPAll` |
| DO03_porturi_ascultare | `netstat -ano \| findstr /R /C:":14330 " /C:":22 " \| findstr LISTENING` |
| DO04_firewall | `Get-NetFirewallRule -Direction Inbound -Enabled True \| Where-Object { ($_ \| Get-NetFirewallPortFilter).LocalPort -in '14330','22' } \| ForEach-Object { [pscustomobject]@{Regula=$_.DisplayName; Actiune=$_.Action; Port=($_ \| Get-NetFirewallPortFilter).LocalPort; Protocol=($_ \| Get-NetFirewallPortFilter).Protocol} } \| Format-Table -AutoSize` |
| DO05_sshd_config | `Get-Content "$env:ProgramData\ssh\sshd_config" \| Where-Object { $_ -and $_ -notmatch '^\s*#' }` |
| DO06_porturi_publice | `Test-NetConnection 188.237.68.25 -Port 14330` și `-Port 44545`, rezultat compact (ComputerName, RemotePort, TcpTestSucceeded) |
| DO07_sqlcmd_versiune | `sqlcmd -S localhost\EPSI -E -Q "SELECT @@SERVERNAME AS Server, SERVERPROPERTY('Edition') AS Editie, SERVERPROPERTY('ProductVersion') AS Versiune"` |
| DO08_file_server_structura | `tree "C:\Users\nekor\Desktop\Elemente de Proiectare a Sistemelor Informatice\File-Server" /F` (dacă Backup e prea lung, arată doar primele niveluri + Autobuze și Harta) |
| DO09_git_istoric | în folderul proiectului (doar citire): `git log --oneline --graph --decorate -25` |
| DO10_git_remote_status | `git remote -v; git branch -a; git status -sb` (doar citire) |
| DO11_dotnet_build | în clone-ul temporar sau în folderul proiectului: `dotnet --version; dotnet build App\Autogara.slnx -c Release` (captura pe final: „Build succeeded”, 0 erori) |
| DO12_dotnet_test | `dotnet test App\Autogara.slnx` (captura pe rezumat: total, trecute, picate) |
| DO13_configurare_statie | `Get-Content "$env:LOCALAPPDATA\Autogara\appsettings.local.json"` — parolele trebuie să apară ca `dpapi:...` (criptate). Dacă apar în clar, NU captura; notează în jurnal |
| DO14_loguri_cache | `Get-ChildItem "$env:LOCALAPPDATA\Autogara" -Recurse \| Select-Object -First 25 FullName,Length,LastWriteTime \| Format-Table -AutoSize` și `Get-Content (Get-ChildItem "$env:LOCALAPPDATA\Autogara\Logs" \| Sort LastWriteTime \| Select -Last 1).FullName -Tail 15` |
| DO15_actualizare_version | dacă există `File-Server\version.json`: `Get-Content` pe el și `Get-ChildItem File-Server\Actualizari`. Dacă nu există, notează în jurnal și sari |
| DO16_publicare_aplicatie | `dotnet publish App\Autogara.WinForms -c Release -r win-x64 --self-contained false -o C:\Users\nekor\Documents\Raport_Autogara\publish` (doar în folderul Documents) și `Get-ChildItem` pe folderul publish (primele fișiere + Autogara.WinForms.exe) |

## D. Frontend — aplicația WinForms

Pornește aplicația (`dotnet run --project App\Autogara.WinForms` sau exe-ul din build), fereastra principală maximizată.
Capturile existente (configurare, login, dashboard, vânzare, hartă, editor autobuz, curse, trasee, mentenanță,
utilizatori, raport zilnic, audit, pasager) se refolosesc; aici sunt doar cele care lipsesc.
Plus 3 capturi din Visual Studio Designer.

| Cod | Ce se capturează |
|---|---|
| FE01_designer_vanzare | VS: `Formulare/FrmVanzareBilet.cs` în modul Design (Shift+F7) cu Properties vizibil |
| FE02_designer_main | VS: `Formulare/FrmMain.cs` în Design (meniul lateral) |
| FE03_tema_cod | VS: `Ui/Tema.cs` (culorile și fonturile) |
| FE04_iconite_svg | Explorer sau Solution Explorer: `Resurse/Iconite` (lista fișierelor SVG) |
| FE10_recuperare_parola | Login → „Am uitat parola”: fereastra de cerere, completată cu `ana.popa`, NU trimite. Închide |
| FE11_casier_cautare | Login Casier `tatiana.rusu`: Vânzare bilete → caută Chișinău → Bălți pe data de mâine → rezultate (nu deschide vânzarea) |
| FE12_bilete_detalii | Admin `m.sopivnic`: Bilete → caută un cod de bilet activ al unei curse viitoare (din BD) → detaliile și estimarea rambursării. NU apăsa Anulează |
| FE13_curse_adauga | Admin: Curse → Adaugă → fereastra modală completată parțial (traseu, autobuz, șofer, dată, oră). Renunță |
| FE14_curse_pasagerii | Admin: Curse → selectează o cursă cu bilete → Pasagerii (lista) |
| FE15_statii | Admin: Stații → selectează o stație → Modifică (fereastra modală). Renunță |
| FE16_autobuze_modifica | Admin: Autobuze → selectează SMB 115 → Modifică (datele autobuzului, nu editorul de locuri). Renunță |
| FE17_mentenanta_lucrare | Admin: Mentenanță → Lucrare nouă (modal). Renunță |
| FE18_mentenanta_itp | Admin: Mentenanță → ITP nou (modal) + lista ITP cu alertele. Renunță |
| FE19_soferi | Admin: Șoferi → selectează un șofer → Modifică (modal). Renunță |
| FE20_reduceri | Admin: Reduceri → selectează Pensionar → Modifică (modal). Renunță |
| FE21_utilizatori_modifica | Admin: Utilizatori → selectează un casier → Modifică (modal). Renunță |
| FE22_raport_ocupare | Admin: Rapoarte → Ocuparea curselor, ultimele 7 zile → Afișează |
| FE23_raport_interval | Admin: Rapoarte → Vânzări pe interval, ultimele 30 de zile → Afișează |
| FE24_raport_comparativ | Admin: Rapoarte → Comparativ (lunar) → Afișează |
| FE25_schimba_parola | Admin: meniul utilizatorului / butonul de schimbare a parolei → fereastra, goală. Renunță |
| FE26_dashboard_azi | Admin: Dashboard, după reîmprospătare |
| FE27_confirmare_iesire | Admin: Ieșire → fereastra de confirmare (FrmIntrebare). Răspunde „Nu” |
| FE28_pasager_biletele_mele | Pasager `ana.popa`: Biletele mele, cu un bilet selectat (detalii) |

Dacă un buton are alt nume decât cel de aici, folosește-l pe cel real și notează în jurnal.
