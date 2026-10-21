const { P, H, bullets, code, fig, table, startChapter } = require("./lib");

module.exports = () => {
  startChapter(3);
  return [
    H(1, "3. FRONTEND"),
    P("Compartimentul a fost realizat de **Crivenco Alexandr**. Interfața este proiectul Autogara.WinForms, o aplicație Windows Forms pe .NET 10 [[WinForms]]. Ea nu conține logică de business: fiecare formular citește datele introduse, apelează un serviciu din AutogaraBackend și afișează rezultatul. În acest capitol sunt descrise designul, fiecare fereastră, cu tabelele și butoanele ei, și un exemplu complet de folosire."),

    H(2, "3.1. Designul și organizarea interfeței"),
    P("Interfața are 32 de ferestre (în folderul `Formulare`), 9 controale desenate de echipă (în `Controale`) și clasele comune din `Ui`. Aspectul fiecărei ferestre stă în fișierul `*.Designer.cs`, editabil vizual în Visual Studio, iar în fișierul `*.cs` stau doar evenimentele, apelurile către backend și afișarea rezultatului."),
    ...fig("FE01a_designer", "Fereastra de vânzare FrmVanzareBilet în Windows Forms Designer, cu butonul de confirmare selectat"),
    ...fig("FE02_designer_main", "Fereastra principală FrmMain în Designer: meniul lateral, antetul și zona de conținut"),
    P("Culorile și fonturile sunt definite într-un singur loc, clasa `Tema`: verdele închis ca culoare principală și galbenul ca semnal, după modelul panourilor de informare din autogări, fontul Bahnschrift pentru titluri și Segoe UI pentru text. Toate ferestrele moștenesc `FormAutogara`, care aplică tema."),
    ...code(`public static readonly Color Primar        = Color.FromArgb(30, 91, 70);
public static readonly Color PrimarInchis   = Color.FromArgb(21, 69, 53);
public static readonly Color PrimarDeschis  = Color.FromArgb(221, 235, 227);
public static readonly Color Semnal         = Color.FromArgb(232, 163, 23);`),
    ...fig("FE03_tema_cod", "Clasa Tema: paleta de culori și fonturile aplicației"),
    P("Iconițele sunt 72 de fișiere SVG din setul Lucide (licență ISC) [[Lucide]], incluse ca resurse și desenate la mărimea și culoarea cerute, deci rămân clare la orice scalare a ecranului. Controalele proprii sunt: schema locurilor (AutobuzSeatMapControl), editorul de autobuz, pânza hărții (HartaCanvasControl), cardurile dashboard-ului (CardKpi), indicatorul de conexiune, butoanele cu iconițe și tabelul stilizat GridAutogara."),
    ...fig("FE04_iconite_svg", "Iconițele SVG ale aplicației (Resurse/Iconite)"),
    P("Reguli comune pentru toate ferestrele:"),
    ...bullets([
      "listele au deasupra filtrele și butoanele (Adaugă, Modifică, Dezactivează), iar adăugarea și modificarea se fac în ferestre modale cu „Salvează” și „Renunță”;",
      "câmpurile greșite sunt marcate direct în formular, cu mesajul lângă câmp (ErrorProvider), iar mesajele sunt în română;",
      "operațiile ireversibile cer confirmare într-o fereastră de întrebare;",
      "indicatorul de conexiune din colțul de jos arată permanent dacă stația lucrează online, doar cu baza de date sau din cache.",
    ]),

    H(2, "3.2. Configurarea stației de lucru"),
    P("La prima pornire se deschide FrmSetupWizard, precompletat cu adresele serverului echipei. Câmpurile sunt grupate pe baza de date (server, port, bază, utilizator, parolă), file server (server, port, utilizator, parolă, folderul de bază) și aplicație (durata rezervării provizorii). Butoanele: **Valorile echipei** (repune valorile implicite), **Testează conexiunea** (verifică separat SQL și SFTP și afișează rezultatul sub formular), **Salvează** (scrie fișierul cu parolele criptate) și **Renunță**."),
    ...fig("EX_configurare", "Configurarea stației de lucru, precompletată cu serverul echipei"),
    ...fig("TC34_test_conexiune_server_indisponibil", "Testul conexiunii cu un port greșit: mesajul clar pentru serverul de baze de date indisponibil"),

    H(2, "3.3. Autentificarea și recuperarea parolei"),
    P("Ecranul de autentificare are două câmpuri (nume de utilizator, parolă), butonul **Autentificare**, legătura **Am uitat parola**, indicatorul de conexiune, legătura **Setări conexiune** și versiunea aplicației. La deschidere se verifică, în fundal, dacă există o versiune nouă pe file server."),
    ...fig("EX_login", "Ecranul de autentificare"),
    ...fig("TC02_login_campuri_goale", "Validarea câmpurilor goale la autentificare"),
    P("Fereastra „Am uitat parola” trimite o cerere de resetare, care ajunge în lista administratorilor; răspunsul nu spune dacă un cont există, ca să nu poată fi ghicite numele de utilizator."),
    ...fig("FE10_recuperare_parola", "Cererea de resetare a parolei, deschisă din ecranul de autentificare"),

    H(2, "3.4. Fereastra principală și meniul pe roluri"),
    P("După autentificare se deschide FrmMain. În stânga este meniul, cu grupele ADMINISTRARE și RAPOARTE, în antet titlul secțiunii, numele și rolul utilizatorului, butonul de schimbare a parolei și **Ieșire**. Secțiunile se deschid în zona de conținut, fără ferestre suplimentare. Meniul conține doar secțiunile permise rolului:"),
    ...table(["Rol", "Secțiuni disponibile"], [
      ["Admin", "Dashboard, Vânzare bilete, Bilete, Curse, Trasee, Stații, Harta, Autobuze, Mentenanță, Șoferi, Reduceri, Utilizatori, Rapoarte, Jurnal audit"],
      ["Casier", "Dashboard, Vânzare bilete, Bilete"],
      ["Pasager", "Cumpără bilet (doar cu cardul), Biletele mele"],
    ], [18, 82], { caption: "Meniul aplicației pe roluri" }),
    ...fig("EX_dashboard_admin", "Fereastra principală a administratorului, cu meniul complet"),
    ...fig("TC06_casier_meniu", "Meniul casierului: doar Dashboard, Vânzare bilete și Bilete"),
    ...fig("FE27_confirmare_iesire", "Confirmarea ieșirii din aplicație (fereastra de întrebare comună)"),

    H(2, "3.5. Dashboard-ul"),
    P("Dashboard-ul arată cifrele zilei în carduri: curse azi, în desfășurare, plecări rămase, bilete vândute și anulate, încasări nete, rezervări active și autobuze cu ITP în următoarele 30 de zile. Dedesubt, tabelul „Următoarele curse” are coloanele plecare, traseu, de la, peron, autobuz, șofer, locuri libere și ocupate, status. Datele se reîmprospătează automat la 30 de secunde sau cu butonul **Reîmprospătează**."),
    ...fig("FE26_dashboard_azi", "Dashboard-ul zilei curente: cardurile cu cifre și următoarele curse"),

    H(2, "3.6. Vânzarea biletelor"),
    P("Secțiunea Vânzare bilete (FrmCautareCurse) are filtrele **Din stația**, **Spre stația** și **Data**, butonul **Caută** și tabelul „Curse găsite” cu plecarea, sosirea, traseul, stațiile de urcare și coborâre, peronul, prețul, locurile libere, autobuzul și statusul. Dublu-click pe o cursă (sau **Alege locul**) deschide fereastra de vânzare."),
    ...fig("FE11_casier_cautare", "Căutarea curselor de către casier"),
    P("Fereastra de vânzare (FrmVanzareBilet) desenează schema autobuzului din fișierul JSON de pe file server. Locurile sunt colorate după stare: liber, rezervat de alt casier, ocupat sau ales. În dreapta sunt datele pasagerului (nume, telefon), reducerea, metoda de plată, numărul bonului fiscal, totalul și butonul **Confirmă vânzarea (F2)**; Esc închide fereastra și eliberează locul rezervat."),
    ...fig("EX_vanzare", "Fereastra de vânzare: schema locurilor, rezervarea provizorie și reducerea"),

    H(2, "3.7. Biletele: căutare și anulare"),
    P("Secțiunea Bilete (FrmBilete) caută un bilet după cod sau din lista biletelor recente și afișează detaliile: pasagerul, cursa, locul, prețul, statusul și plățile. În partea „Anulare” apare suma care s-ar rambursa acum, după regula 24 h / 2 h, opțiunea de rambursare integrală (excepție aprobată de administrator) și butonul **Anulează biletul**, care cere confirmare."),
    ...fig("FE12_bilete_detalii", "Detaliile unui bilet și estimarea rambursării"),
    ...fig("TC19_confirmare_anulare", "Confirmarea anulării, cu codul biletului și numele pasagerului"),

    H(2, "3.8. Cursele"),
    P("Secțiunea Curse are filtrele **De la**, **Până la**, **Traseu** și butonul **Afișează**, iar deasupra tabelului butoanele **Adaugă**, **Modifică**, **A plecat**, **A ajuns**, **Pasagerii** și **Anulează…**. Anularea unei curse rambursează integral toate biletele ei, într-o singură tranzacție."),
    ...fig("EX_curse", "Lista curselor pe interval, cu butoanele de stare"),
    P("Fereastra de adăugare a cursei cere traseul, autobuzul, șoferul, data, ora de plecare și de sosire și prețul; la salvare backend-ul verifică dacă ITP-ul autobuzului este valabil la data cursei și dacă autobuzul sau șoferul nu au deja o cursă care se suprapune, apoi creează locurile."),
    ...fig("FE13_curse_adauga", "Adăugarea unei curse: traseul, autobuzul, șoferul, data și ora"),
    ...fig("FE14_curse_pasagerii", "Lista pasagerilor unei curse"),

    H(2, "3.9. Traseele și stațiile"),
    P("Traseele au o denumire și o listă ordonată de opriri; în fereastra de modificare, stațiile se adaugă din listă și se mută cu săgețile, iar distanța totală se calculează pe hartă. Stațiile au adresa și peronul; numele și poziția vin din nodul de pe hartă."),
    ...fig("EX_trasee", "Modificarea unui traseu: opririle în ordine"),
    ...fig("FE15_statii", "Lista stațiilor și fereastra de modificare a unei stații"),

    H(2, "3.10. Editorul hărții"),
    P("Editorul hărții (FrmEditorHarta, cu controlul HartaCanvasControl) are o bară de instrumente: **Selectare**, **Stație**, **Intersecție**, **Conexiune**, **Șterge**, **Potrivește**, **Reîncarcă** și **Salvează harta**. Stațiile și intersecțiile se adaugă prin click pe hartă, drumurile se trasează prin tragere între două noduri, iar distanța și proprietățile nodului se completează în panoul din dreapta. La salvare, nodurile și drumurile se scriu în baza de date, iar harta în harta.json pe file server."),
    ...fig("EX_harta", "Editorul hărții stațiilor"),

    H(2, "3.11. Autobuzele și editorul de locuri"),
    P("Lista autobuzelor arată numărul de înmatriculare, modelul, capacitatea, statusul și data ITP. Butoanele sunt **Adaugă**, **Modifică** (datele autobuzului) și **Locuri**, care deschide editorul așezării: o grilă în care click pe o poziție goală adaugă un loc, click pe un loc îl șterge, iar numerotarea se face automat pe rânduri. Dacă numărul de locuri diferă de capacitatea din baza de date, aplicația cere confirmare înainte de salvare."),
    ...fig("FE16_autobuze_modifica", "Modificarea datelor unui autobuz"),
    ...fig("EX_editor_autobuz", "Editorul așezării locurilor unui autobuz"),
    ...fig("TC29_editor_autobuz_confirmare_capacitate", "Confirmarea cerută când numărul de locuri diferă de capacitate"),

    H(2, "3.12. Mentenanța și ITP-ul"),
    P("Secțiunea Mentenanță are două tab-uri: **Lucrări** (istoricul lucrărilor, cu butonul **Lucrare nouă**) și **ITP** (autobuzele active cu ITP-ul expirat sau care expiră în 30 de zile, cu butonul **ITP nou**). Înregistrarea unui ITP actualizează data de expirare a autobuzului."),
    ...fig("EX_mentenanta", "Istoricul lucrărilor de mentenanță"),
    ...fig("FE17_mentenanta_lucrare", "Adăugarea unei lucrări de mentenanță"),
    ...fig("FE18_mentenanta_itp", "Înregistrarea unui ITP nou și lista alertelor de ITP"),

    H(2, "3.13. Șoferii și reducerile"),
    P("Șoferii (nume, prenume, permis, telefon) și tipurile de reducere (denumire, procent) au aceeași structură: listă cu butoane și fereastră modală de editare. Un șofer sau o reducere nu se șterge, ci se dezactivează, ca biletele vechi să rămână corecte."),
    ...fig("FE19_soferi", "Lista șoferilor și modificarea unui șofer"),
    ...fig("FE20_reduceri", "Tipurile de reducere și modificarea procentului"),

    H(2, "3.14. Utilizatorii și parolele"),
    P("Administratorul gestionează conturile pe roluri cu butoanele **Adaugă**, **Modifică**, **Parolă temporară** și **Dezactivează**; în dreapta, lista „Cereri de resetare” are butonul **Generează parolă temporară**. Orice utilizator își poate schimba parola din fereastra principală; parola nouă trebuie să aibă cel puțin 8 caractere, cel puțin o literă și o cifră și să nu conțină numele de utilizator."),
    ...fig("EX_utilizatori", "Administrarea conturilor"),
    ...fig("FE21_utilizatori_modifica", "Modificarea unui cont de utilizator"),
    ...fig("FE25_schimba_parola", "Schimbarea parolei utilizatorului autentificat"),

    H(2, "3.15. Rapoartele și jurnalul de audit"),
    P("Secțiunea Rapoarte are lista **Raportul** cu patru tipuri (vânzări pe o zi, pe traseu și casier; vânzări pe interval, pe zile; ocuparea curselor; comparație lunară pe trasee), câmpurile **De la**, **Până la**, **Traseu** și butonul **Afișează**, plus butoanele cu iconițe pentru exportul CSV (se deschide direct în Excel) și pentru arhivarea pe file server. Sub tabel apare totalul."),
    ...fig("EX_raport_zilnic", "Raportul de vânzări pe o zi"),
    ...fig("FE23_raport_interval", "Raportul de vânzări pe interval, pe zile"),
    ...fig("FE22_raport_ocupare", "Raportul de ocupare a curselor"),
    ...fig("FE24_raport_comparativ", "Comparația lunară pe trasee"),
    P("Jurnalul de audit se filtrează după perioadă și text și arată data, utilizatorul, acțiunea și entitatea."),
    ...fig("TC31_jurnal_audit_autentificari", "Jurnalul de audit filtrat după autentificări"),

    H(2, "3.16. Interfața pasagerului"),
    P("Pasagerul vede doar **Cumpără bilet** și **Biletele mele**. La cumpărare, numele este precompletat din cont, plata este doar cu cardul, iar bonul fiscal nu se cere. În „Biletele mele” apar doar biletele lui, cu detaliile fiecăruia."),
    ...fig("EX_pasager_cumpara", "Cumpărarea unui bilet de către pasager"),
    ...fig("FE28_pasager_biletele_mele", "Biletele mele: lista și detaliile biletului ales"),

    H(2, "3.17. Exemplu de folosire: vânzarea unui bilet"),
    P("Exemplul urmărește o vânzare reală făcută la testare de casierul Tatiana Rusu, pentru cursa Chișinău (Autogara Nord) – Bălți."),
    P("**Pasul 1.** Casierul alege stațiile și data și apasă **Caută**. Apar doar cursele care trec prin cele două stații, cu prețul și locurile libere."),
    ...fig("TC08_cautare_curse", "Pasul 1: căutarea curselor Chișinău – Bălți"),
    P("**Pasul 2.** Casierul deschide cursa de 07:30 și alege locul 6. Locul devine galben pentru ceilalți casieri, iar cronometrul arată cât mai durează rezervarea (10 minute)."),
    ...fig("TC09_rezervare_provizorie", "Pasul 2: rezervarea provizorie a locului 6"),
    P("**Pasul 3.** Se completează numele și telefonul pasagerului și se alege reducerea Pensionar; totalul se recalculează imediat: 120,00 MDL − 30 % = 84,00 MDL."),
    ...fig("TC10_calcul_reducere_pensionar", "Pasul 3: datele pasagerului și reducerea"),
    P("**Pasul 4.** F2 confirmă vânzarea. Biletul primește codul AG261014-849CF994, locul devine ocupat, iar în baza de date apar biletul activ și plata finalizată."),
    ...fig("TC13_vanzare_confirmata", "Pasul 4: biletul emis"),
    P("**Pasul 5.** Pe a doua stație, dashboard-ul arată imediat vânzarea: un bilet în plus și încasările crescute cu 84,00 MDL."),
    ...fig("TC14_dashboard_actualizat_alt_casier", "Pasul 5: dashboard-ul actualizat pe stația altui casier"),

    H(2, "3.18. Concluzii la capitolul 3"),
    P("Interfața acoperă toate rolurile și toate operațiile din sarcina tehnică, cu un design unitar, definit într-un singur loc. Separarea strictă de logică a permis schimbarea designului fără a modifica regulile din backend. Controalele desenate (schema locurilor, editorul hărții și al autobuzului) sunt partea cea mai complexă și oferă utilizatorului o imagine clară a autobuzului și a rutelor. Ca îmbunătățiri posibile rămân o variantă web pentru pasageri și tipărirea biletului."),
  ];
};
