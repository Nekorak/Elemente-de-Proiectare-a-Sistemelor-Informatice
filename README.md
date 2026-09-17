<div align="center">

# Autogara — Aplicație de Gestionare a Autogării

Proiect de echipă · Elemente de Proiectare a Sistemelor Informatice

![.NET](https://img.shields.io/badge/.NET_10-WinForms-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/Microsoft_SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Model](https://img.shields.io/badge/model-waterfall-blue)
![Status](https://img.shields.io/badge/etapa_curent%C4%83-Baz%C4%83_de_Date-orange)

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
- [ ] Stratul de acces la date și conexiunea la distanță
- [ ] Autentificarea, rolurile și recuperarea parolei
- [ ] Serviciile pentru hartă
- [ ] Parsarea și validarea JSON-urilor autobuzelor
- [ ] Rezervarea, vânzarea, anularea și rambursarea biletelor
- [ ] Calculul prețurilor și al reducerilor
- [ ] Integrarea cu casa de marcat fiscală (dacă e necesară)
- [ ] Rapoartele și datele pentru dashboard
- [ ] Cache-ul local și modul offline
- [ ] Logarea erorilor, tranzacțiile, limitarea brute-force

### Frontend — Crivenco Alexandr
- [ ] Wizard-ul de configurare și login-ul
- [ ] Editorul de hartă
- [ ] Editorul de autobuz (generează JSON)
- [ ] Formularul de vânzare cu harta locurilor
- [ ] Formularele Admin
- [ ] Dashboard-ul și rapoartele
- [ ] Validările și mesajele de eroare
- [ ] Indicatorul online/offline și auto-update-ul

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
SQL Code/               scripturile bazei de date (00_RunAll.sql le rulează în ordine)
Sarcina Tehnica.docx    sarcina tehnică, aprobată de colegiu
plan.md                 planul tehnic detaliat al proiectului
Credentials.txt         credențialele proiectului (repo privat)
```

## Rularea bazei de date

Deschide `SQL Code/00_RunAll.sql` în SSMS și activează **Query → SQLCMD Mode**. Completează variabilele de la începutul fișierului, apoi rulează cu F5.

Datele de test acoperă intervalul de la 7 zile în urmă până la 7 zile înainte față de ziua rulării. Conturile din seed au parola temporară `Autogara#2026`.

## Reguli de lucru

- Lucrăm direct pe `main`, fără branch-uri.
- Înainte de lucru: `git pull`. După lucru: `git commit` și `git push`.
- Când o sarcină e gata, bifeaz-o în secțiunea ToDo.
- Toată logica stă în Backend; formularele doar apelează funcțiile acestuia.
