const { P, H, bullets } = require("./lib");

module.exports = () => [
  H(1, "CONCLUZII"),
  P("Scopul proiectului a fost atins: am realizat un sistem informatic funcțional pentru o autogară, cu bază de date pe un server real, aplicație desktop pentru trei roluri și acces de la distanță pentru toți membrii echipei. Funcțiile principale, vânzarea cu alegerea locului, rezervarea provizorie, anularea cu rambursare, rapoartele și administrarea, au fost verificate prin 40 de cazuri de test funcționale, iar partea cea mai sensibilă, vânzarea aceluiași loc de pe mai multe stații, a fost verificată cu până la 20 de stații simultane, fără niciun bilet dublu."),
  P("**Eficiența elaborării.** Împărțirea pe compartimente, cu un responsabil pentru fiecare, și modelul cascadă au permis lucrul în paralel fără conflicte: baza de date a fost terminată prima, backend-ul s-a construit peste procedurile ei, iar interfața a folosit doar serviciile backend-ului. Depozitul Git comun a păstrat istoricul fiecărei etape, iar testarea a găsit defecte reale (backup-ul, limita de autentificare ocolită prin încercări simultane, datele de test rămase), pe care le putem remedia înainte de predare."),
  P("**Tehnologiile utilizate:** Microsoft SQL Server (T-SQL, proceduri stocate, viziuni, SQL Server Agent), SQL Server Management Studio, C# și .NET 10, Windows Forms, Entity Framework Core, SSH.NET și OpenSSH Server, Serilog, xUnit, Git și GitHub, Visual Studio."),
  P("**Lucruri noi învățate:**"),
  ...bullets([
    "proiectarea unei scheme cu constrângeri complete și protejarea operațiilor critice la concurență (tranzacții, UPDLOCK, ROWVERSION, index filtrat unic);",
    "administrarea unui server SQL real: port propriu, conturi cu drepturi minime, joburi automate, strategia de backup și testul de restaurare;",
    "publicarea serviciilor în internet în siguranță: redirecționarea porturilor, firewall, SFTP în locul SMB;",
    "organizarea unei aplicații pe straturi și păstrarea parolelor (PBKDF2, DPAPI);",
    "lucrul în echipă cu Git și testarea sistematică pe cazuri de test documentate.",
  ]),
  P("**Beneficiile produsului.** Beneficiarii sunt casierii (vânzare rapidă, fără erori de loc), administrația autogării (rapoarte, evidența curselor, a flotei și a ITP-ului, jurnalul operațiilor) și pasagerii (cumpărarea online a biletului și lista biletelor proprii)."),
  P("**Impresii personale.** Proiectul ne-a arătat cât de mult contează o bază de date bine proiectată: majoritatea problemelor de concurență și de integritate au fost rezolvate o singură dată, în SQL Server, și nu în fiecare fereastră. Cea mai mare provocare a fost accesul de la distanță la serverul de acasă, rezolvat prin porturi nestandard și SFTP. Ca dezvoltări viitoare vedem remedierea defectelor găsite la testare, o versiune web pentru pasageri și integrarea cu casa de marcat fiscală."),
];
