# Testele de concurență și de încărcare

Programul simulează mai multe stații de lucru (casieri) care lucrează **simultan** cu backend-ul aplicației
(aceleași servicii ca în WinForms: `AuthService`, `RezervareService`, `BiletService`, `DashboardService`).

Rulează **doar** pe copia de test `autogara_test`; dacă baza conectată are alt nume, programul se oprește.
Configurarea SQL se citește din configurarea stației (`%LOCALAPPDATA%\Autogara`, parole DPAPI), deci nu
conține nicio parolă; numele bazei este înlocuit cu `autogara_test`. Conturile de test `stres.*` sunt create
de program, cu parole generate la rulare (nu se salvează nicăieri).

```
dotnet run -c Release -- [--server 127.0.0.1] [--port 14330] [--instanta .\EPSI] [--doar S1,S2]
```

Rezultatele: tabel în consolă și fișierul `rezultate.md`.
