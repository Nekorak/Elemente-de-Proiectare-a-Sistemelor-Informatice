USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- Date de volum: ~10 000 de bilete pe ultimele 45 de zile si urmatoarele 14.
-- Se ruleaza dupa 11_Seed.sql. Adauga peste datele existente, nu sterge nimic;
-- zilele care au deja curse sunt sarite. Valorile sunt aleatorii la fiecare rulare.

IF EXISTS (SELECT 1 FROM autogara.Soferi WHERE NrPermis = N'PC 0913482')
BEGIN
    RAISERROR (N'Datele de volum au fost deja adăugate (12_DateVolum.sql a mai fost rulat).', 16, 1);
    RETURN;
END

DECLARE @Azi        DATE         = CAST(autogara.fn_AcumLocal() AS DATE);
DECLARE @Acum       DATETIME2(0) = autogara.fn_AcumLocal();
DECLARE @ZileInapoi INT          = 45;
DECLARE @ZileInainte INT         = 14;

BEGIN TRANSACTION;

------------------------------------------------------------------------------
-- Soferi noi
------------------------------------------------------------------------------
INSERT INTO autogara.Soferi (Nume, Prenume, NrPermis, Telefon, Activ, CreatLa)
SELECT s.Nume, s.Prenume, s.NrPermis, s.Telefon, 1, DATEADD(DAY, -s.Vechime, SYSUTCDATETIME())
FROM (VALUES
    (N'Ciobanu',  N'Vitalie',   N'PC 0913482', N'+373 69 518 204', 70),
    (N'Lupașcu',  N'Oleg',      N'PC 0662175', N'+373 79 230 867', 64),
    (N'Popescu',  N'Igor',      N'PC 0538816', N'+373 68 447 190', 120),
    (N'Ianioglo', N'Fiodor',    N'PC 0721953', N'+373 78 902 335', 210),
    (N'Caraman',  N'Gheorghi',  N'PC 0346208', N'+373 69 117 548', 95),
    (N'Russu',    N'Valeriu',   N'PC 0884761', N'+373 60 785 012', 330),
    (N'Bodrug',   N'Ruslan',    N'PC 0275339', N'+373 67 344 981', 48),
    (N'Melnic',   N'Anatolie',  N'PC 0599417', N'+373 69 630 257', 150)
) AS s (Nume, Prenume, NrPermis, Telefon, Vechime)
WHERE NOT EXISTS (SELECT 1 FROM autogara.Soferi AS x WHERE x.NrPermis = s.NrPermis);

------------------------------------------------------------------------------
-- Nume pentru pasageri
------------------------------------------------------------------------------
CREATE TABLE #PrenumeM (Id INT IDENTITY(0, 1) PRIMARY KEY, Prenume NVARCHAR(50) COLLATE DATABASE_DEFAULT);
INSERT INTO #PrenumeM (Prenume) VALUES
    (N'Ion'), (N'Vasile'), (N'Gheorghe'), (N'Nicolae'), (N'Andrei'), (N'Mihai'), (N'Alexandru'), (N'Dumitru'),
    (N'Sergiu'), (N'Victor'), (N'Pavel'), (N'Iurie'), (N'Vitalie'), (N'Constantin'), (N'Radu'), (N'Octavian'),
    (N'Petru'), (N'Ștefan'), (N'Denis'), (N'Eugen'), (N'Valeriu'), (N'Igor'), (N'Oleg'), (N'Ruslan'),
    (N'Anatolie'), (N'Veaceslav'), (N'Serghei'), (N'Dmitrii'), (N'Alexei'), (N'Vladimir'), (N'Maxim'), (N'Artiom'),
    (N'Daniel'), (N'Cristian'), (N'Adrian'), (N'Marin'), (N'Grigore'), (N'Tudor'), (N'Vlad'), (N'Nichita'),
    (N'Dorin'), (N'Valentin'), (N'Leonid'), (N'Emil'), (N'Ilie'), (N'Mircea'), (N'Corneliu'), (N'Gavril'),
    (N'Simion'), (N'Iacob'), (N'Roman'), (N'Vadim'), (N'Ghenadie'), (N'Boris'), (N'Arcadie'), (N'Eduard'),
    (N'Marcel'), (N'Liviu'), (N'Costel'), (N'Timofei');

CREATE TABLE #PrenumeF (Id INT IDENTITY(0, 1) PRIMARY KEY, Prenume NVARCHAR(50) COLLATE DATABASE_DEFAULT);
INSERT INTO #PrenumeF (Prenume) VALUES
    (N'Maria'), (N'Elena'), (N'Ana'), (N'Tatiana'), (N'Natalia'), (N'Olga'), (N'Svetlana'), (N'Valentina'),
    (N'Cristina'), (N'Irina'), (N'Lilia'), (N'Galina'), (N'Doina'), (N'Mariana'), (N'Ludmila'), (N'Veronica'),
    (N'Aurelia'), (N'Diana'), (N'Adriana'), (N'Nadejda'), (N'Victoria'), (N'Rodica'), (N'Angela'), (N'Silvia'),
    (N'Corina'), (N'Daniela'), (N'Alina'), (N'Ecaterina'), (N'Larisa'), (N'Zinaida'), (N'Raisa'), (N'Lidia'),
    (N'Vera'), (N'Nina'), (N'Eugenia'), (N'Viorica'), (N'Liliana'), (N'Inga'), (N'Snejana'), (N'Oxana'),
    (N'Iuliana'), (N'Mihaela'), (N'Gabriela'), (N'Carolina'), (N'Ionela'), (N'Dorina'), (N'Stela'), (N'Parascovia'),
    (N'Ala'), (N'Polina'), (N'Anastasia'), (N'Alexandra'), (N'Sofia'), (N'Nicoleta'), (N'Marina'), (N'Lucia'),
    (N'Eleonora'), (N'Tamara'), (N'Emilia'), (N'Zoia');

-- NumeF diferă doar la numele rusești/ucrainene (Ivanov / Ivanova).
CREATE TABLE #NumeFamilie (Id INT IDENTITY(0, 1) PRIMARY KEY, NumeM NVARCHAR(50) COLLATE DATABASE_DEFAULT, NumeF NVARCHAR(50) COLLATE DATABASE_DEFAULT);
INSERT INTO #NumeFamilie (NumeM, NumeF)
SELECT n, n FROM (VALUES
    (N'Rusu'), (N'Ceban'), (N'Munteanu'), (N'Lungu'), (N'Popa'), (N'Botnaru'), (N'Cojocaru'), (N'Bivol'),
    (N'Ciobanu'), (N'Moraru'), (N'Țurcanu'), (N'Rotaru'), (N'Guțu'), (N'Cebotari'), (N'Sîrbu'), (N'Grosu'),
    (N'Baciu'), (N'Melnic'), (N'Croitoru'), (N'Pascari'), (N'Vrabie'), (N'Ursu'), (N'Plămădeală'), (N'Cazacu'),
    (N'Roșca'), (N'Istrati'), (N'Căpățînă'), (N'Mocanu'), (N'Negru'), (N'Oprea'), (N'Andronic'), (N'Buzu'),
    (N'Sandu'), (N'Tofan'), (N'Leahu'), (N'Gârlea'), (N'Josan'), (N'Frunză'), (N'Cucoș'), (N'Postolachi'),
    (N'Bejenaru'), (N'Bălan'), (N'Bostan'), (N'Bulat'), (N'Burlacu'), (N'Caraman'), (N'Chiriac'), (N'Cozma'),
    (N'Creangă'), (N'Cristea'), (N'Dabija'), (N'Donică'), (N'Dragan'), (N'Dodon'), (N'Enache'), (N'Gaina'),
    (N'Gheorghiță'), (N'Goncear'), (N'Graur'), (N'Hîncu'), (N'Iordache'), (N'Lupu'), (N'Lupașcu'), (N'Mîndru'),
    (N'Manole'), (N'Marin'), (N'Matei'), (N'Moldovan'), (N'Morari'), (N'Nastas'), (N'Nistor'), (N'Ojog'),
    (N'Olaru'), (N'Pîrlog'), (N'Pleșca'), (N'Poiată'), (N'Prisăcaru'), (N'Răileanu'), (N'Railean'), (N'Russu'),
    (N'Sârbu'), (N'Scutaru'), (N'Spînu'), (N'Stratan'), (N'Surdu'), (N'Tabără'), (N'Taran'), (N'Toma'),
    (N'Turcan'), (N'Ungureanu'), (N'Vasilache'), (N'Veleșcu'), (N'Vieru'), (N'Zaharia'), (N'Zgardan'), (N'Strătilă'),
    (N'Bodrug'), (N'Codreanu'), (N'Gondiu'), (N'Popescu'), (N'Covali'), (N'Ababii'), (N'Agachi'), (N'Albu'),
    (N'Ianioglo'), (N'Topal'), (N'Uzun'), (N'Chiosea'), (N'Dimitroglo'), (N'Caramalac'), (N'Cara'), (N'Gaidarji')
) AS v (n)
UNION ALL
SELECT m, f FROM (VALUES
    (N'Ivanov', N'Ivanova'), (N'Popov', N'Popova'), (N'Smirnov', N'Smirnova'), (N'Kozlov', N'Kozlova'),
    (N'Morozov', N'Morozova'), (N'Volkov', N'Volkova'), (N'Pavlov', N'Pavlova'), (N'Romanov', N'Romanova'),
    (N'Grigoriev', N'Grigorieva'), (N'Sidorov', N'Sidorova'), (N'Kuznețov', N'Kuznețova'), (N'Belov', N'Belova'),
    (N'Bondarenco', N'Bondarenco'), (N'Kovalenco', N'Kovalenco'), (N'Tkacenco', N'Tkacenco'), (N'Șevcenco', N'Șevcenco'),
    (N'Lisnic', N'Lisnic'), (N'Babenco', N'Babenco'), (N'Rudenco', N'Rudenco'), (N'Sîrghi', N'Sîrghi')
) AS v (m, f);

DECLARE @NrPM INT = (SELECT COUNT(*) FROM #PrenumeM);
DECLARE @NrPF INT = (SELECT COUNT(*) FROM #PrenumeF);
DECLARE @NrNF INT = (SELECT COUNT(*) FROM #NumeFamilie);
DECLARE @Prefixe NCHAR(18) = N'606267686976777879';

------------------------------------------------------------------------------
-- Utilizatori: casieri noi și conturi de pasageri
------------------------------------------------------------------------------
DECLARE @RolCasier  INT = (SELECT RolID FROM autogara.Roluri WHERE Denumire = N'Casier');
DECLARE @RolPasager INT = (SELECT RolID FROM autogara.Roluri WHERE Denumire = N'Pasager');
DECLARE @RolAdmin   INT = (SELECT RolID FROM autogara.Roluri WHERE Denumire = N'Admin');

INSERT INTO autogara.Utilizatori (NumeUtilizator, ParolaHash, Nume, Prenume, Email, Telefon, RolID, Activ, CreatLa)
SELECT c.NumeUtilizator, HASHBYTES('SHA2_512', CONCAT(c.NumeUtilizator, N':', N'Test2026')),
       c.Nume, c.Prenume, CONCAT(c.NumeUtilizator, N'@autogara.local'), c.Telefon, @RolCasier, 1,
       DATEADD(DAY, -c.Vechime, SYSUTCDATETIME())
FROM (VALUES
    (N'elena.grosu',     N'Grosu',  N'Elena',    N'+373 69 284 551', 75),
    (N'cristina.melnic', N'Melnic', N'Cristina', N'+373 79 601 438', 58),
    (N'vadim.sandu',     N'Sandu',  N'Vadim',    N'+373 68 912 073', 52)
) AS c (NumeUtilizator, Nume, Prenume, Telefon, Vechime)
WHERE NOT EXISTS (SELECT 1 FROM autogara.Utilizatori AS u WHERE u.NumeUtilizator = c.NumeUtilizator);

CREATE TABLE #Conturi
(
    Rand INT IDENTITY(1, 1), Femeie BIT, Prenume NVARCHAR(50) COLLATE DATABASE_DEFAULT, Nume NVARCHAR(50) COLLATE DATABASE_DEFAULT,
    R1 INT, R2 INT, R3 INT, NumeUtilizator NVARCHAR(50) COLLATE DATABASE_DEFAULT NULL, Email NVARCHAR(150) COLLATE DATABASE_DEFAULT NULL, Telefon NVARCHAR(20) COLLATE DATABASE_DEFAULT NULL
);

INSERT INTO #Conturi (Femeie, Prenume, Nume, R1, R2, R3)
SELECT TOP (80)
    CASE WHEN r.A % 100 < 55 THEN 1 ELSE 0 END,
    CASE WHEN r.A % 100 < 55 THEN pf.Prenume ELSE pm.Prenume END,
    CASE WHEN r.A % 100 < 55 THEN nf.NumeF ELSE nf.NumeM END,
    r.B, r.C, r.D
FROM (SELECT TOP (80) ABS(CHECKSUM(NEWID())) AS A, ABS(CHECKSUM(NEWID())) AS B,
             ABS(CHECKSUM(NEWID())) AS C, ABS(CHECKSUM(NEWID())) AS D
      FROM sys.all_objects) AS r
JOIN #PrenumeM     AS pm ON pm.Id = r.B % @NrPM
JOIN #PrenumeF     AS pf ON pf.Id = r.B % @NrPF
JOIN #NumeFamilie  AS nf ON nf.Id = r.C % @NrNF;

UPDATE #Conturi
SET NumeUtilizator = LEFT(LOWER(TRANSLATE(
        CASE R1 % 6
            WHEN 0 THEN CONCAT(Prenume, N'.', Nume)
            WHEN 1 THEN CONCAT(Prenume, N'.', Nume, 70 + R2 % 36 - CASE WHEN 70 + R2 % 36 >= 100 THEN 100 ELSE 0 END)
            WHEN 2 THEN CONCAT(Prenume, Nume)
            WHEN 3 THEN CONCAT(LEFT(Prenume, 1), N'.', Nume)
            WHEN 4 THEN CONCAT(Nume, N'_', Prenume)
            ELSE        CONCAT(Prenume, N'_', Nume, R2 % 10)
        END,
        N'ăâîșțşţĂÂÎȘȚŞŢ', N'aaiststAAISTST')), 50);

UPDATE #Conturi
SET Email = CASE WHEN R3 % 100 < 6 THEN NULL
                 ELSE CONCAT(REPLACE(NumeUtilizator, N'_', N'.'), N'@',
                      CASE WHEN R3 % 100 < 58 THEN N'gmail.com'
                           WHEN R3 % 100 < 73 THEN N'mail.ru'
                           WHEN R3 % 100 < 85 THEN N'yahoo.com'
                           WHEN R3 % 100 < 94 THEN N'outlook.com'
                           ELSE N'inbox.ru' END) END,
    Telefon = CONCAT(N'+373 ', SUBSTRING(@Prefixe, 2 * (R2 % 9) + 1, 2), N' ',
                     RIGHT(CONCAT(N'00', R3 % 1000), 3), N' ', RIGHT(CONCAT(N'00', R1 % 1000), 3));

WITH Unice AS
(
    SELECT *, ROW_NUMBER() OVER (PARTITION BY NumeUtilizator ORDER BY Rand) AS Dup
    FROM #Conturi
)
INSERT INTO autogara.Utilizatori (NumeUtilizator, ParolaHash, Nume, Prenume, Email, Telefon, RolID, Activ, CreatLa)
SELECT TOP (60)
    u.NumeUtilizator, HASHBYTES('SHA2_512', CONCAT(u.NumeUtilizator, N':', N'Test2026')),
    u.Nume, u.Prenume, u.Email, u.Telefon, @RolPasager, 1,
    DATEADD(MINUTE, -(u.R1 % 1440), DATEADD(DAY, -(3 + u.R2 % 300), SYSUTCDATETIME()))
FROM Unice AS u
WHERE u.Dup = 1
  AND LEN(u.NumeUtilizator) >= 3
  AND NOT EXISTS (SELECT 1 FROM autogara.Utilizatori AS x WHERE x.NumeUtilizator = u.NumeUtilizator)
ORDER BY u.Rand;

------------------------------------------------------------------------------
-- Mentenanță în perioada nouă
------------------------------------------------------------------------------
INSERT INTO autogara.MentenantaAutobuze (AutobuzID, TipLucrare, Data, Kilometraj, Observatii)
SELECT a.AutobuzID, m.TipLucrare, DATEADD(DAY, -m.ZileInUrma, @Azi),
       CASE WHEN m.DeltaKm IS NULL THEN NULL
            ELSE ISNULL((SELECT MAX(x.Kilometraj) FROM autogara.MentenantaAutobuze AS x WHERE x.AutobuzID = a.AutobuzID), 250000) + m.DeltaKm END,
       m.Observatii
FROM (VALUES
    (N'KVA 318', 38, N'Înlocuire becuri faruri și stopuri',          3100,  NULL),
    (N'KVA 318',  9, N'Schimb ulei motor și filtre',                  9800,  N'Ulei 5W-30; filtru motorină schimbat'),
    (N'KVB 427', 33, N'Reparație sistem de încălzire salon',          4200,  N'Pompă recirculare înlocuită'),
    (N'KVB 427',  4, N'Inspecție tehnică periodică (ITP)',            11050, N'Admis; următoarea peste 6 luni'),
    (N'DRL 902', 41, N'Înlocuire curea distribuție',                  2500,  N'Kit Gates + pompă apă'),
    (N'DRL 902', 15, N'Schimb anvelope (vară → iarnă)',                8900,  N'Michelin Agilis Alpin 225/65 R16'),
    (N'SMB 115', 29, N'Revizie frâne spate',                          6400,  N'Saboți noi, tamburi rectificați'),
    (N'SMB 115',  6, N'Curățare filtru particule (DPF)',              12800, NULL),
    (N'SMC 604', 36, N'Schimb ulei motor și filtre',                  5200,  NULL),
    (N'SMC 604', 11, N'Reparație climatizare',                        11700, N'Compresor AC înlocuit, garanție 12 luni'),
    (N'TRX 771', 24, N'Înlocuire parbriz',                            7300,  N'Fisură după pietriș pe R14'),
    (N'TRX 771',  2, N'Schimb anvelope (vară → iarnă)',               13900, NULL),
    (N'ORH 208', 27, N'Înlocuire turbocompresor',                     150,   N'Piesă nouă Garrett; test rutier 40 km OK'),
    (N'ORH 208', 21, N'Inspecție tehnică periodică (ITP)',            220,   N'Respins: emisii peste limită; reprogramat'),
    (N'ORH 208',  8, N'Diagnoză sistem injecție',                     260,   N'Injector cilindrul 3 defect, comandat')
) AS m (NrInmatriculare, ZileInUrma, TipLucrare, DeltaKm, Observatii)
JOIN autogara.Autobuze AS a ON a.NrInmatriculare = m.NrInmatriculare
ORDER BY m.ZileInUrma DESC;

-- KVB 427 a trecut ITP-ul acum 4 zile.
UPDATE autogara.Autobuze
SET DataExpirareITP = DATEADD(MONTH, 6, DATEADD(DAY, -4, @Azi)), ModificatLa = DATEADD(DAY, -4, SYSUTCDATETIME())
WHERE NrInmatriculare = N'KVB 427' AND (DataExpirareITP IS NULL OR DataExpirareITP < DATEADD(DAY, 30, @Azi));

------------------------------------------------------------------------------
-- Curse: orarul zilnic pe zilele din perioadă care nu au încă nicio cursă
------------------------------------------------------------------------------
DECLARE @Orar TABLE
(
    TraseuID INT, NrInmatriculare NVARCHAR(15), NrPermis NVARCHAR(20),
    OraPlecare TIME(0), OraSosire TIME(0), Pret DECIMAL(10, 2), Cerere INT
);
INSERT INTO @Orar VALUES
    (12, N'DRL 902', N'PC 0819274', '06:50', '07:45',  38.00, 44),
    (5,  N'SMC 604', N'PC 0284519', '06:45', '10:20', 155.00, 52),
    (11, N'KVA 318', N'PC 0913482', '07:15', '08:10',  38.00, 40),
    (1,  N'SMB 115', N'PC 0391254', '07:30', '10:05', 120.00, 55),
    (3,  N'DRL 902', N'PC 0610337', '08:15', '10:10',  85.00, 48),
    (12, N'KVA 318', N'PC 0913482', '09:00', '09:55',  38.00, 42),
    (7,  N'TRX 771', N'PC 0745102', '09:00', '11:55', 140.00, 46),
    (9,  N'KVB 427', N'PC 0458963', '11:00', '12:25',  60.00, 34),
    (1,  N'KVA 318', N'PC 0527841', '12:10', '14:40', 120.00, 50),
    (10, N'KVB 427', N'PC 0458963', '13:30', '14:55',  60.00, 32),
    (4,  N'DRL 902', N'PC 0610337', '13:30', '15:25',  85.00, 47),
    (6,  N'SMC 604', N'PC 0284519', '14:00', '17:35', 155.00, 50),
    (2,  N'SMB 115', N'PC 0391254', '15:00', '17:35', 120.00, 52),
    (8,  N'TRX 771', N'PC 0745102', '15:30', '18:25', 140.00, 44),
    (9,  N'KVB 427', N'PC 0662175', '16:20', '17:45',  60.00, 30),
    (2,  N'KVA 318', N'PC 0527841', '17:50', '20:20', 120.00, 49),
    (11, N'DRL 902', N'PC 0819274', '18:10', '19:05',  38.00, 46),
    (10, N'KVB 427', N'PC 0662175', '18:30', '19:55',  60.00, 28);

DECLARE @CurseNoi TABLE (CursaID INT PRIMARY KEY);

WITH Numere AS
(
    SELECT TOP (@ZileInapoi + @ZileInainte + 1) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS n
    FROM sys.all_objects
),
Zile AS
(
    SELECT DATEADD(DAY, n - @ZileInapoi, @Azi) AS DataCursa
    FROM Numere
    WHERE NOT EXISTS (SELECT 1 FROM autogara.Curse AS c WHERE c.DataCursa = DATEADD(DAY, n - @ZileInapoi, @Azi))
),
Planificare AS
(
    SELECT
        o.TraseuID, a.AutobuzID, s.SoferID, z.DataCursa, o.OraPlecare, o.OraSosire, o.Pret,
        ABS(CHECKSUM(NEWID())) % 1000 AS Zar
    FROM Zile AS z
    CROSS JOIN @Orar AS o
    JOIN autogara.Trasee   AS t ON t.TraseuID = o.TraseuID AND t.Activ = 1
    JOIN autogara.Autobuze AS a ON a.NrInmatriculare = o.NrInmatriculare AND a.Activ = 1 AND a.Status = N'Activ'
    JOIN autogara.Soferi   AS s ON s.NrPermis = o.NrPermis AND s.Activ = 1
)
INSERT INTO autogara.Curse
    (TraseuID, AutobuzID, SoferID, DataCursa, OraPlecare, OraSosireEstimata, Pret, Status, CreatLa)
OUTPUT inserted.CursaID INTO @CurseNoi (CursaID)
SELECT
    p.TraseuID, p.AutobuzID, p.SoferID, p.DataCursa, p.OraPlecare, p.OraSosire, p.Pret,
    CASE
        WHEN p.Zar < 12 AND p.DataCursa <= DATEADD(DAY, 2, @Azi)                  THEN N'Anulata'
        WHEN autogara.fn_MomentCursa(p.DataCursa, p.OraSosire)  <= @Acum           THEN N'Finalizata'
        WHEN autogara.fn_MomentCursa(p.DataCursa, p.OraPlecare) <= @Acum           THEN N'In desfasurare'
        ELSE N'Planificata'
    END,
    autogara.fn_LocalLaUtc(DATEADD(DAY, -21, CAST(p.DataCursa AS DATETIME2(3))))
FROM Planificare AS p
ORDER BY p.DataCursa, p.OraPlecare;

WITH Cifre AS (SELECT n FROM (VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) AS v (n)),
     Numere AS (SELECT u.n + 10 * z.n + 1 AS NumarLoc FROM Cifre AS u CROSS JOIN Cifre AS z)
INSERT INTO autogara.Locuri (CursaID, NumarLoc, Status)
SELECT c.CursaID, nr.NumarLoc, N'Liber'
FROM @CurseNoi AS cn
JOIN autogara.Curse    AS c ON c.CursaID = cn.CursaID
JOIN autogara.Autobuze AS a ON a.AutobuzID = c.AutobuzID
JOIN Numere            AS nr ON nr.NumarLoc <= a.CapacitateLocuri
ORDER BY c.CursaID, nr.NumarLoc;

------------------------------------------------------------------------------
-- Gradul de ocupare al fiecărei curse
------------------------------------------------------------------------------
CREATE TABLE #Curse
(
    CursaID INT PRIMARY KEY, DataCursa DATE, Plecare DATETIME2(0), PretBaza DECIMAL(10, 2),
    StatusCursa NVARCHAR(20) COLLATE DATABASE_DEFAULT, Ocupare INT, MomentAnulare DATETIME2(0) NULL
);

INSERT INTO #Curse (CursaID, DataCursa, Plecare, PretBaza, StatusCursa, Ocupare)
SELECT
    c.CursaID, c.DataCursa, autogara.fn_MomentCursa(c.DataCursa, c.OraPlecare), c.Pret, c.Status,
    CASE WHEN c.Status = N'Anulata' THEN 25 + ABS(CHECKSUM(NEWID())) % 20
    ELSE
        CAST(o.Cerere
             -- luni .. duminică (0 = luni)
             * CASE DATEDIFF(DAY, '19000101', c.DataCursa) % 7
                   WHEN 0 THEN 1.10 WHEN 1 THEN 0.85 WHEN 2 THEN 0.85 WHEN 3 THEN 0.95
                   WHEN 4 THEN 1.40 WHEN 5 THEN 1.00 ELSE 1.45 END
             * CASE WHEN c.OraPlecare < '09:00' THEN 1.15 WHEN c.OraPlecare >= '17:00' THEN 1.10 ELSE 1.00 END
             * CASE WHEN c.DataCursa > @Azi
                    THEN CASE WHEN 1 - 0.14 * DATEDIFF(DAY, @Azi, c.DataCursa) > 0.06
                              THEN 1 - 0.14 * DATEDIFF(DAY, @Azi, c.DataCursa) ELSE 0.06 END
                    WHEN c.Status = N'Planificata' THEN 0.80
                    ELSE 1.00 END
             * (0.70 + (ABS(CHECKSUM(NEWID())) % 60) / 100.0)
        AS INT)
    END
FROM @CurseNoi AS cn
JOIN autogara.Curse AS c ON c.CursaID = cn.CursaID
JOIN autogara.Autobuze AS a ON a.AutobuzID = c.AutobuzID
JOIN @Orar AS o ON o.TraseuID = c.TraseuID AND o.OraPlecare = c.OraPlecare AND o.NrInmatriculare = a.NrInmatriculare;

UPDATE #Curse SET Ocupare = 97 WHERE Ocupare > 97;

------------------------------------------------------------------------------
-- Clienți: oameni care cumpără bilete (unii revin des — navetiști)
------------------------------------------------------------------------------
CREATE TABLE #Clienti
(
    ClientID INT IDENTITY(0, 1) PRIMARY KEY, NumeAfisat NVARCHAR(200) COLLATE DATABASE_DEFAULT, Telefon NVARCHAR(20) COLLATE DATABASE_DEFAULT NULL,
    R1 INT, R2 INT, R3 INT, R4 INT, Femeie BIT, Prenume NVARCHAR(50) COLLATE DATABASE_DEFAULT, Prenume2 NVARCHAR(50) COLLATE DATABASE_DEFAULT, Nume NVARCHAR(50) COLLATE DATABASE_DEFAULT
);

INSERT INTO #Clienti (R1, R2, R3, R4)
SELECT TOP (2600) ABS(CHECKSUM(NEWID())), ABS(CHECKSUM(NEWID())), ABS(CHECKSUM(NEWID())), ABS(CHECKSUM(NEWID()))
FROM sys.all_objects AS a CROSS JOIN sys.all_objects AS b;

UPDATE k
SET Femeie   = CASE WHEN k.R1 % 100 < 54 THEN 1 ELSE 0 END,
    Prenume  = CASE WHEN k.R1 % 100 < 54 THEN pf.Prenume  ELSE pm.Prenume  END,
    Prenume2 = CASE WHEN k.R1 % 100 < 54 THEN pf2.Prenume ELSE pm2.Prenume END,
    Nume     = CASE WHEN k.R1 % 100 < 54 THEN nf.NumeF    ELSE nf.NumeM    END
FROM #Clienti AS k
JOIN #PrenumeM    AS pm  ON pm.Id  = k.R2 % @NrPM
JOIN #PrenumeF    AS pf  ON pf.Id  = k.R2 % @NrPF
JOIN #PrenumeM    AS pm2 ON pm2.Id = (k.R2 / 100) % @NrPM
JOIN #PrenumeF    AS pf2 ON pf2.Id = (k.R2 / 100) % @NrPF
JOIN #NumeFamilie AS nf  ON nf.Id  = k.R3 % @NrNF;

-- Cum scrie omul numele: de obicei corect, uneori fără diacritice, cu numele mare,
-- în ordine inversă, cu litere mici sau cu prenume dublu.
UPDATE #Clienti
SET NumeAfisat = CASE
        WHEN R4 % 100 < 70 THEN CONCAT(Prenume, N' ', Nume)
        WHEN R4 % 100 < 81 THEN TRANSLATE(CONCAT(Prenume, N' ', Nume), N'ăâîșțşţĂÂÎȘȚŞŢ', N'aaiststAAISTST')
        WHEN R4 % 100 < 87 THEN CONCAT(UPPER(Nume), N' ', Prenume)
        WHEN R4 % 100 < 93 THEN CONCAT(Nume, N' ', Prenume)
        WHEN R4 % 100 < 96 THEN LOWER(TRANSLATE(CONCAT(Prenume, N' ', Nume), N'ăâîșțşţĂÂÎȘȚŞŢ', N'aaiststAAISTST'))
        WHEN Prenume <> Prenume2 THEN CONCAT(Prenume, N'-', Prenume2, N' ', Nume)
        ELSE CONCAT(Prenume, N' ', Nume)
    END,
    Telefon = CASE
        WHEN R3 / 100 % 100 >= 80 THEN NULL
        WHEN R4 / 100 % 100 < 45 THEN CONCAT(N'+373 ', SUBSTRING(@Prefixe, 2 * (R1 / 100 % 9) + 1, 2), N' ',
                                          RIGHT(CONCAT(N'00', R2 / 7 % 1000), 3), N' ', RIGHT(CONCAT(N'00', R3 / 7 % 1000), 3))
        WHEN R4 / 100 % 100 < 75 THEN CONCAT(N'0', SUBSTRING(@Prefixe, 2 * (R1 / 100 % 9) + 1, 2),
                                          RIGHT(CONCAT(N'00', R2 / 7 % 1000), 3), RIGHT(CONCAT(N'00', R3 / 7 % 1000), 3))
        WHEN R4 / 100 % 100 < 90 THEN CONCAT(N'0', SUBSTRING(@Prefixe, 2 * (R1 / 100 % 9) + 1, 2), N' ',
                                          RIGHT(CONCAT(N'00', R2 / 7 % 1000), 3), N' ', RIGHT(CONCAT(N'00', R3 / 7 % 1000), 3))
        ELSE CONCAT(N'+373', SUBSTRING(@Prefixe, 2 * (R1 / 100 % 9) + 1, 2),
                    RIGHT(CONCAT(N'00', R2 / 7 % 1000), 3), RIGHT(CONCAT(N'00', R3 / 7 % 1000), 3))
    END;

DECLARE @NrClienti INT = (SELECT COUNT(*) FROM #Clienti);

CREATE TABLE #Casieri (Id INT IDENTITY(0, 1) PRIMARY KEY, UtilizatorID UNIQUEIDENTIFIER, NumeUtilizator NVARCHAR(50) COLLATE DATABASE_DEFAULT);
INSERT INTO #Casieri (UtilizatorID, NumeUtilizator)
SELECT u.UtilizatorID, u.NumeUtilizator
FROM autogara.Utilizatori AS u
WHERE u.RolID = @RolCasier AND u.Activ = 1
ORDER BY u.NumeUtilizator;

CREATE TABLE #Pasageri
(
    Id INT IDENTITY(0, 1) PRIMARY KEY, UtilizatorID UNIQUEIDENTIFIER, NumeUtilizator NVARCHAR(50) COLLATE DATABASE_DEFAULT,
    NumeAfisat NVARCHAR(200) COLLATE DATABASE_DEFAULT, Telefon NVARCHAR(20) COLLATE DATABASE_DEFAULT, CreatLa DATETIME2(3)
);
INSERT INTO #Pasageri (UtilizatorID, NumeUtilizator, NumeAfisat, Telefon, CreatLa)
SELECT u.UtilizatorID, u.NumeUtilizator, CONCAT(u.Prenume, N' ', u.Nume), u.Telefon, u.CreatLa
FROM autogara.Utilizatori AS u
WHERE u.RolID = @RolPasager AND u.Activ = 1
ORDER BY u.CreatLa;

DECLARE @NrCasieri  INT = (SELECT COUNT(*) FROM #Casieri);
DECLARE @NrPasageri INT = (SELECT COUNT(*) FROM #Pasageri);

------------------------------------------------------------------------------
-- Vânzările
------------------------------------------------------------------------------
CREATE TABLE #V
(
    LocID INT PRIMARY KEY, CursaID INT, NumarLoc SMALLINT, DataCursa DATE, Plecare DATETIME2(0),
    PretBaza DECIMAL(10, 2), StatusCursa NVARCHAR(20) COLLATE DATABASE_DEFAULT,
    R1 INT, R2 INT, R3 INT, R4 INT, R5 INT, R6 INT,
    BiletID UNIQUEIDENTIFIER NULL, CodBilet NVARCHAR(20) COLLATE DATABASE_DEFAULT NULL, NumePasager NVARCHAR(200) COLLATE DATABASE_DEFAULT NULL,
    TelefonPasager NVARCHAR(20) COLLATE DATABASE_DEFAULT NULL, TipReducereID INT NULL, VanzutDe UNIQUEIDENTIFIER NULL,
    Online BIT NULL, MetodaPlata NVARCHAR(10) COLLATE DATABASE_DEFAULT NULL, DataEmitereLocal DATETIME2(0) NULL,
    Pret DECIMAL(10, 2) NULL, StatusBilet NVARCHAR(20) COLLATE DATABASE_DEFAULT NULL, ModificatLocal DATETIME2(0) NULL,
    ProcentRambursare DECIMAL(5, 2) NULL
);

INSERT INTO #V (LocID, CursaID, NumarLoc, DataCursa, Plecare, PretBaza, StatusCursa, R1, R2, R3, R4, R5, R6)
SELECT l.LocID, l.CursaID, l.NumarLoc, k.DataCursa, k.Plecare, k.PretBaza, k.StatusCursa,
       ABS(CHECKSUM(NEWID())), ABS(CHECKSUM(NEWID())), ABS(CHECKSUM(NEWID())),
       ABS(CHECKSUM(NEWID())), ABS(CHECKSUM(NEWID())), ABS(CHECKSUM(NEWID()))
FROM autogara.Locuri AS l
JOIN #Curse AS k ON k.CursaID = l.CursaID;

DELETE v FROM #V AS v JOIN #Curse AS k ON k.CursaID = v.CursaID
WHERE v.R1 % 100 >= k.Ocupare;

-- Cine a vândut: ~9% online (contul pasagerului, doar card), restul la casă.
UPDATE #V SET Online = CASE WHEN @NrPasageri > 0 AND R5 % 100 < 9 THEN 1 ELSE 0 END;

UPDATE v
SET NumePasager    = k.NumeAfisat,
    TelefonPasager = k.Telefon,
    VanzutDe       = cs.UtilizatorID,
    MetodaPlata    = CASE WHEN v.R6 % 100 < 33 THEN N'Card' ELSE N'Numerar' END
FROM #V AS v
JOIN #Clienti AS k  ON k.ClientID = CASE WHEN v.R2 % 100 < 30 THEN v.R3 % 260 ELSE v.R3 % @NrClienti END
JOIN #Casieri AS cs ON cs.Id = v.R4 % @NrCasieri
WHERE v.Online = 0;

UPDATE v
SET NumePasager    = p.NumeAfisat,
    TelefonPasager = p.Telefon,
    VanzutDe       = p.UtilizatorID,
    MetodaPlata    = N'Card'
FROM #V AS v
JOIN #Pasageri AS p ON p.Id = v.R3 % @NrPasageri
WHERE v.Online = 1;

UPDATE #V
SET TipReducereID = CASE WHEN R4 / 100 % 100 < 9  THEN (SELECT TipReducereID FROM autogara.TipuriReducere WHERE Denumire = N'Elev'      AND Activ = 1)
                         WHEN R4 / 100 % 100 < 21 THEN (SELECT TipReducereID FROM autogara.TipuriReducere WHERE Denumire = N'Pensionar' AND Activ = 1)
                         WHEN R4 / 100 % 100 < 25 THEN (SELECT TipReducereID FROM autogara.TipuriReducere WHERE Denumire = N'Abonament' AND Activ = 1)
                    END,
    -- cele mai multe bilete se iau în ziua plecării; o parte cu 1–3 zile înainte, puține cu o săptămână
    DataEmitereLocal = DATEADD(MINUTE, -CASE
            WHEN R5 / 100 % 100 < 55 THEN 15 + R6 / 100 % 480
            WHEN R5 / 100 % 100 < 85 THEN 1440 + R6 / 100 % 2880
            ELSE 4320 + R6 / 100 % 8640
        END, Plecare);

-- Cursele de mâine și de mai târziu: biletele s-au vândut în zilele de până acum,
-- mai multe recent decât acum o săptămână.
UPDATE #V
SET DataEmitereLocal = DATEADD(MINUTE, -(5 + (R2 / 100 % 120) * (R2 / 100 % 120)), @Acum)
WHERE DataEmitereLocal > @Acum;

-- La casă nu se vinde noaptea: biletele „emise” între 22:00 și 06:00 se mută dimineața.
UPDATE #V
SET DataEmitereLocal = DATEADD(MINUTE, 360 + R1 / 100 % 90, CAST(CAST(DataEmitereLocal AS DATE) AS DATETIME2(0)))
WHERE Online = 0
  AND (DATEPART(HOUR, DataEmitereLocal) < 6 OR DATEPART(HOUR, DataEmitereLocal) >= 22)
  AND DATEADD(MINUTE, 360 + R1 / 100 % 90, CAST(CAST(DataEmitereLocal AS DATE) AS DATETIME2(0))) < DATEADD(MINUTE, -10, Plecare)
  AND DATEADD(MINUTE, 360 + R1 / 100 % 90, CAST(CAST(DataEmitereLocal AS DATE) AS DATETIME2(0))) < @Acum;

-- Contul de pasager nu poate cumpăra înainte să existe.
UPDATE v
SET DataEmitereLocal = DATEADD(MINUTE, 30 + v.R6 % 600, autogara.fn_UtcLaLocal(p.CreatLa))
FROM #V AS v
JOIN #Pasageri AS p ON p.UtilizatorID = v.VanzutDe
WHERE v.Online = 1 AND autogara.fn_LocalLaUtc(v.DataEmitereLocal) < p.CreatLa;

DELETE FROM #V WHERE DataEmitereLocal >= Plecare OR DataEmitereLocal > @Acum;

UPDATE #V SET Pret = autogara.fn_CalculeazaPret(PretBaza, TipReducereID);

-- Anulările de curse: toate biletele se rambursează integral, în momentul anulării.
UPDATE k
SET MomentAnulare = CASE
        WHEN DATEADD(MINUTE, 20 + k.CursaID * 37 % 300, ISNULL(m.UltimaVanzare, DATEADD(HOUR, -6, k.Plecare)))
             < CASE WHEN k.Plecare < @Acum THEN DATEADD(MINUTE, -15, k.Plecare) ELSE DATEADD(MINUTE, -5, @Acum) END
        THEN DATEADD(MINUTE, 20 + k.CursaID * 37 % 300, ISNULL(m.UltimaVanzare, DATEADD(HOUR, -6, k.Plecare)))
        ELSE CASE WHEN k.Plecare < @Acum THEN DATEADD(MINUTE, -15, k.Plecare) ELSE DATEADD(MINUTE, -5, @Acum) END
    END
FROM #Curse AS k
OUTER APPLY (SELECT MAX(v.DataEmitereLocal) AS UltimaVanzare FROM #V AS v WHERE v.CursaID = k.CursaID) AS m
WHERE k.StatusCursa = N'Anulata';

DELETE v FROM #V AS v JOIN #Curse AS k ON k.CursaID = v.CursaID
WHERE k.StatusCursa = N'Anulata' AND v.DataEmitereLocal >= k.MomentAnulare;

UPDATE v
SET StatusBilet = N'Rambursat', ModificatLocal = k.MomentAnulare, ProcentRambursare = 100
FROM #V AS v JOIN #Curse AS k ON k.CursaID = v.CursaID
WHERE k.StatusCursa = N'Anulata';

-- Anulări individuale (~4%): procentul urmează regula reală (100% / 50% / 0%).
UPDATE #V
SET ModificatLocal = DATEADD(MINUTE,
        DATEDIFF(MINUTE, DataEmitereLocal, CASE WHEN Plecare < @Acum THEN Plecare ELSE @Acum END) * (20 + R5 / 10000 % 75) / 100,
        DataEmitereLocal)
WHERE StatusBilet IS NULL AND R6 / 10000 % 100 < 4;

UPDATE #V
SET ProcentRambursare = autogara.fn_ProcentRambursare(Plecare, ModificatLocal)
WHERE StatusBilet IS NULL AND ModificatLocal IS NOT NULL;

UPDATE #V
SET StatusBilet = CASE WHEN ProcentRambursare > 0 THEN N'Rambursat' ELSE N'Anulat' END
WHERE StatusBilet IS NULL AND ModificatLocal IS NOT NULL;

UPDATE #V SET StatusBilet = N'Activ', ProcentRambursare = 0 WHERE StatusBilet IS NULL;

UPDATE #V
SET BiletID  = NEWID(),
    CodBilet = CONCAT(N'AG', CONVERT(CHAR(6), DataCursa, 12), N'-',
                      CONVERT(VARCHAR(8), CONVERT(BINARY(4), CAST(LocID AS BIGINT) * CAST(2654435761 AS BIGINT) % CAST(4294967296 AS BIGINT)), 2));

INSERT INTO autogara.Bilete
    (BiletID, CodBilet, CursaID, LocID, NumePasager, TelefonPasager, TipReducereID,
     VanzutDeUtilizatorID, DataEmitere, Pret, Status, ModificatLa)
SELECT
    BiletID, CodBilet, CursaID, LocID, NumePasager, TelefonPasager, TipReducereID,
    VanzutDe, autogara.fn_LocalLaUtc(DataEmitereLocal), Pret, StatusBilet,
    CASE WHEN StatusBilet <> N'Activ' THEN autogara.fn_LocalLaUtc(ModificatLocal) END
FROM #V
ORDER BY DataEmitereLocal;

------------------------------------------------------------------------------
-- Plăți (bonurile fiscale continuă numerotarea zilei)
------------------------------------------------------------------------------
WITH Existente AS
(
    SELECT SUBSTRING(NumarBonFiscal, 3, 6) AS Zi, MAX(TRY_CAST(SUBSTRING(NumarBonFiscal, 10, 10) AS INT)) AS UltimulNr
    FROM autogara.Plati
    WHERE NumarBonFiscal LIKE N'BF______-%'
    GROUP BY SUBSTRING(NumarBonFiscal, 3, 6)
),
Noi AS
(
    SELECT v.*, CONVERT(CHAR(6), v.DataEmitereLocal, 12) AS Zi,
           ROW_NUMBER() OVER (PARTITION BY CAST(v.DataEmitereLocal AS DATE) ORDER BY v.DataEmitereLocal, v.LocID) AS Nr
    FROM #V AS v
)
INSERT INTO autogara.Plati (BiletID, Suma, MetodaPlata, DataPlata, Status, NumarBonFiscal)
SELECT n.BiletID, n.Pret, n.MetodaPlata, autogara.fn_LocalLaUtc(n.DataEmitereLocal), N'Finalizata',
       CONCAT(N'BF', n.Zi, N'-', RIGHT(CONCAT(N'0000', n.Nr + ISNULL(e.UltimulNr, 0)), 4))
FROM Noi AS n
LEFT JOIN Existente AS e ON e.Zi = n.Zi
ORDER BY n.DataEmitereLocal;

WITH Existente AS
(
    SELECT SUBSTRING(NumarBonFiscal, 3, 6) AS Zi, MAX(TRY_CAST(SUBSTRING(NumarBonFiscal, 10, 10) AS INT)) AS UltimulNr
    FROM autogara.Plati
    WHERE NumarBonFiscal LIKE N'RB______-%'
    GROUP BY SUBSTRING(NumarBonFiscal, 3, 6)
),
Noi AS
(
    SELECT v.*, CONVERT(CHAR(6), v.ModificatLocal, 12) AS Zi,
           ROW_NUMBER() OVER (PARTITION BY CAST(v.ModificatLocal AS DATE) ORDER BY v.ModificatLocal, v.LocID) AS Nr
    FROM #V AS v
    WHERE v.StatusBilet = N'Rambursat'
)
INSERT INTO autogara.Plati (BiletID, Suma, MetodaPlata, DataPlata, Status, NumarBonFiscal)
SELECT n.BiletID, ROUND(n.Pret * n.ProcentRambursare / 100, 2), n.MetodaPlata,
       autogara.fn_LocalLaUtc(n.ModificatLocal), N'Rambursata',
       CONCAT(N'RB', n.Zi, N'-', RIGHT(CONCAT(N'0000', n.Nr + ISNULL(e.UltimulNr, 0)), 4))
FROM Noi AS n
LEFT JOIN Existente AS e ON e.Zi = n.Zi
ORDER BY n.ModificatLocal;

UPDATE l
SET l.Status = N'Ocupat'
FROM autogara.Locuri AS l
JOIN #V AS v ON v.LocID = l.LocID
WHERE v.StatusBilet = N'Activ';

------------------------------------------------------------------------------
-- Jurnalul de audit, cum l-ar fi scris aplicația
------------------------------------------------------------------------------
INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID, DataOra)
SELECT VanzutDe, CONCAT(N'Vânzare bilet ', CodBilet, N' (', Pret, N' MDL, ', MetodaPlata, N')'),
       N'Bilete', CONVERT(NVARCHAR(50), BiletID), autogara.fn_LocalLaUtc(DataEmitereLocal)
FROM #V;

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID, DataOra)
SELECT v.VanzutDe,
       CONCAT(N'Anulare bilet ', v.CodBilet, N' — rambursat ', CAST(ROUND(v.Pret * v.ProcentRambursare / 100, 2) AS DECIMAL(10, 2)),
              N' MDL (', CAST(v.ProcentRambursare AS INT), N'%)'),
       N'Bilete', CONVERT(NVARCHAR(50), v.BiletID), autogara.fn_LocalLaUtc(v.ModificatLocal)
FROM #V AS v
JOIN #Curse AS k ON k.CursaID = v.CursaID
WHERE v.StatusBilet <> N'Activ' AND k.StatusCursa <> N'Anulata';

DECLARE @AdminLog UNIQUEIDENTIFIER =
    ISNULL((SELECT UtilizatorID FROM autogara.Utilizatori WHERE NumeUtilizator = N'gheorghe.lungu' AND Activ = 1),
           (SELECT TOP (1) UtilizatorID FROM autogara.Utilizatori WHERE RolID = @RolAdmin AND Activ = 1 ORDER BY CreatLa));

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID, DataOra)
SELECT @AdminLog,
       CONCAT(N'Anulare cursă #', k.CursaID, N': ',
              CASE k.CursaID % 4 WHEN 0 THEN N'defecțiune tehnică la autobuz'
                                 WHEN 1 THEN N'condiții meteo nefavorabile'
                                 WHEN 2 THEN N'șofer indisponibil (concediu medical)'
                                 ELSE N'număr insuficient de pasageri' END,
              N' — ', COUNT(v.LocID), N' bilete rambursate (', ISNULL(SUM(v.Pret), 0), N' MDL)'),
       N'Curse', CONVERT(NVARCHAR(50), k.CursaID), autogara.fn_LocalLaUtc(k.MomentAnulare)
FROM #Curse AS k
LEFT JOIN #V AS v ON v.CursaID = k.CursaID
WHERE k.StatusCursa = N'Anulata'
GROUP BY k.CursaID, k.MomentAnulare;

-- Autentificare / deconectare pentru fiecare zi în care cineva a vândut.
CREATE TABLE #Ture (UtilizatorID UNIQUEIDENTIFIER, Zi DATE, Prima DATETIME2(0), Ultima DATETIME2(0), R INT);
INSERT INTO #Ture (UtilizatorID, Zi, Prima, Ultima, R)
SELECT VanzutDe, CAST(DataEmitereLocal AS DATE), MIN(DataEmitereLocal), MAX(DataEmitereLocal), ABS(CHECKSUM(NEWID()))
FROM #V
GROUP BY VanzutDe, CAST(DataEmitereLocal AS DATE);

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID, DataOra)
SELECT t.UtilizatorID, N'Autentificare eșuată', N'Autentificare', u.NumeUtilizator,
       autogara.fn_LocalLaUtc(DATEADD(MINUTE, -(4 + t.R % 15) - 1, t.Prima))
FROM #Ture AS t
JOIN autogara.Utilizatori AS u ON u.UtilizatorID = t.UtilizatorID
WHERE t.R / 1000 % 100 < 6;

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID, DataOra)
SELECT t.UtilizatorID, N'Autentificare reușită', N'Autentificare', u.NumeUtilizator,
       autogara.fn_LocalLaUtc(DATEADD(MINUTE, -(4 + t.R % 15), t.Prima))
FROM #Ture AS t
JOIN autogara.Utilizatori AS u ON u.UtilizatorID = t.UtilizatorID;

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID, DataOra)
SELECT t.UtilizatorID, N'Deconectare', N'Autentificare', u.NumeUtilizator,
       autogara.fn_LocalLaUtc(DATEADD(MINUTE, 3 + t.R / 100 % 40, t.Ultima))
FROM #Ture AS t
JOIN autogara.Utilizatori AS u ON u.UtilizatorID = t.UtilizatorID
WHERE DATEADD(MINUTE, 3 + t.R / 100 % 40, t.Ultima) < DATEADD(MINUTE, -30, @Acum);

COMMIT TRANSACTION;

DROP TABLE #PrenumeM, #PrenumeF, #NumeFamilie, #Conturi, #Curse, #Clienti, #Casieri, #Pasageri, #V, #Ture;

SELECT N'Utilizatori' AS Tabel, COUNT(*) AS Randuri FROM autogara.Utilizatori
UNION ALL SELECT N'Soferi',             COUNT(*) FROM autogara.Soferi
UNION ALL SELECT N'MentenantaAutobuze', COUNT(*) FROM autogara.MentenantaAutobuze
UNION ALL SELECT N'Curse',              COUNT(*) FROM autogara.Curse
UNION ALL SELECT N'Locuri',             COUNT(*) FROM autogara.Locuri
UNION ALL SELECT N'Bilete',             COUNT(*) FROM autogara.Bilete
UNION ALL SELECT N'Plati',              COUNT(*) FROM autogara.Plati
UNION ALL SELECT N'LogAudit',           COUNT(*) FROM autogara.LogAudit;
GO
