USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

DELETE FROM autogara.LogAudit;
DELETE FROM autogara.Plati;
DELETE FROM autogara.Bilete;
DELETE FROM autogara.RezervariProvizorii;
DELETE FROM autogara.Locuri;
DELETE FROM autogara.Curse;
DELETE FROM autogara.TipuriReducere;
DELETE FROM autogara.TraseuOpriri;
DELETE FROM autogara.Trasee;
DELETE FROM autogara.Soferi;
DELETE FROM autogara.MentenantaAutobuze;
DELETE FROM autogara.Autobuze;
DELETE FROM autogara.Statii;
DELETE FROM autogara.Conexiuni;
DELETE FROM autogara.Noduri;
DELETE FROM autogara.Utilizatori;
DELETE FROM autogara.Roluri;

DECLARE @Reseed NVARCHAR(MAX) = N'';
SELECT @Reseed += CONCAT(N'DBCC CHECKIDENT (''autogara.', OBJECT_NAME(ic.object_id), N''', RESEED, 0) WITH NO_INFOMSGS;', CHAR(10))
FROM sys.identity_columns AS ic
WHERE OBJECT_SCHEMA_NAME(ic.object_id) = N'autogara'
  AND ic.last_value IS NOT NULL;
EXEC (@Reseed);
GO

BEGIN TRANSACTION;

SET IDENTITY_INSERT autogara.Roluri ON;
INSERT INTO autogara.Roluri (RolID, Denumire) VALUES
    (1, N'Admin'),
    (2, N'Casier'),
    (3, N'Pasager');
SET IDENTITY_INSERT autogara.Roluri OFF;

DECLARE @Utilizatori TABLE
(
    NumeUtilizator NVARCHAR(50), Nume NVARCHAR(100), Prenume NVARCHAR(100),
    Email NVARCHAR(150), Telefon NVARCHAR(20), RolID INT, Activ BIT, VechimeZile INT, Parola NVARCHAR(100)
);

INSERT INTO @Utilizatori VALUES
    (N'd.gojinevschii',     N'Gojinevschii', N'Dmitri',    N'd.gojinevschii@autogara.local',     NULL,               1, 1, 60,  N'Vs7uLTx*EtdxQQU!'),
    (N's.hanganu',          N'Hanganu',      N'Sergiu',    N's.hanganu@autogara.local',          NULL,               1, 1, 60,  N'aci*#vkBk5jaBaip'),
    (N'a.crivenco',         N'Crivenco',     N'Alexandr',  N'a.crivenco@autogara.local',         NULL,               1, 1, 60,  N'pE!-J#H?oiBT57Ut'),
    (N'm.sopivnic',         N'Sopivnic',     N'Maxim',     N'm.sopivnic@autogara.local',         NULL,               1, 1, 60,  N'BG5oZP!b6AN!+yz4'),
    (N'gheorghe.lungu',    N'Lungu',        N'Gheorghe',  N'gheorghe.lungu@autogara.local',     N'+373 69 214 377', 1, 1, 420, N'Autogara#2026'),
    (N'rodica.cazacu',      N'Cazacu',       N'Rodica',    N'rodica.cazacu@autogara.local',      N'+373 68 530 912', 1, 1, 240, N'Autogara#2026'),
    (N'tatiana.rusu',       N'Rusu',         N'Tatiana',   N'tatiana.rusu@autogara.local',       N'+373 69 781 046', 2, 1, 380, N'Autogara#2026'),
    (N'victoria.ceban',     N'Ceban',        N'Victoria',  N'victoria.ceban@autogara.local',     N'+373 78 115 629', 2, 1, 310, N'Autogara#2026'),
    (N'ion.munteanu',       N'Munteanu',     N'Ion',       N'ion.munteanu@autogara.local',       N'+373 60 947 233', 2, 1, 150, N'Autogara#2026'),
    (N'natalia.bivol',      N'Bivol',        N'Natalia',   N'natalia.bivol@autogara.local',      N'+373 69 356 804', 2, 1, 95, N'Autogara#2026'),
    (N'olga.cojocaru',      N'Cojocaru',     N'Olga',      N'olga.cojocaru@autogara.local',      N'+373 79 663 170', 2, 0, 520, N'Autogara#2026'),
    (N'ana.popa',           N'Popa',         N'Ana',       N'ana.popa@example.com',              N'+373 69 845 302', 3, 1, 45, N'Autogara#2026'),
    (N'mihai.botnaru',      N'Botnaru',      N'Mihai',     N'mihai.botnaru@example.com',         N'+373 78 290 614', 3, 1, 12, N'Autogara#2026');

INSERT INTO autogara.Utilizatori
    (UtilizatorID, NumeUtilizator, ParolaHash, Nume, Prenume, Email, Telefon, RolID, Activ, CreatLa, ModificatLa)
SELECT
    NEWID(),
    u.NumeUtilizator,
    HASHBYTES('SHA2_512', CONCAT(u.NumeUtilizator, N':', u.Parola)),
    u.Nume, u.Prenume, u.Email, u.Telefon, u.RolID, u.Activ,
    DATEADD(DAY, -u.VechimeZile, SYSUTCDATETIME()),
    CASE WHEN u.Activ = 0 THEN DATEADD(DAY, -20, SYSUTCDATETIME()) END
FROM @Utilizatori AS u;

SET IDENTITY_INSERT autogara.Noduri ON;
INSERT INTO autogara.Noduri (NodID, Tip, Nume, CoordX, CoordY) VALUES
    (1,  N'Statie',      N'Chișinău – Autogara Centrală',   892, 592),
    (2,  N'Statie',      N'Chișinău – Autogara Nord',       880, 568),
    (3,  N'Statie',      N'Orhei',                          888, 448),
    (4,  N'Statie',      N'Bălți',                          532, 296),
    (5,  N'Statie',      N'Soroca',                         680, 136),
    (6,  N'Statie',      N'Edineț',                         284, 132),
    (7,  N'Statie',      N'Strășeni',                       804, 544),
    (8,  N'Statie',      N'Călărași',                       684, 496),
    (9,  N'Statie',      N'Ungheni',                        480, 516),
    (10, N'Statie',      N'Hîncești',                       796, 668),
    (11, N'Statie',      N'Cimișlia',                       872, 792),
    (12, N'Statie',      N'Comrat',                         824, 880),
    (13, N'Statie',      N'Cahul',                          636, 1040),
    (14, N'Intersectie', N'Intersecția Peresecina (M2)',    868, 500),
    (15, N'Intersectie', N'Intersecția Ciocîlteni (M2/R14)', 768, 388);
SET IDENTITY_INSERT autogara.Noduri OFF;

DECLARE @Drumuri TABLE (NodA INT, NodB INT, DistantaKm DECIMAL(6, 2));
INSERT INTO @Drumuri VALUES
    (1,  2,   4.50),
    (2,  14, 26.00),
    (14, 3,  18.00),
    (3,  15, 22.00),
    (15, 4,  70.00),
    (15, 5,  85.00),
    (4,  6,  70.00),
    (1,  7,  24.00),
    (7,  8,  30.00),
    (8,  9,  52.00),
    (1,  10, 36.00),
    (1,  11, 70.00),
    (10, 11, 38.00),
    (11, 12, 30.00),
    (12, 13, 75.00);

INSERT INTO autogara.Conexiuni (NodPlecareID, NodSosireID, DistantaKm)
SELECT NodA, NodB, DistantaKm FROM @Drumuri
UNION ALL
SELECT NodB, NodA, DistantaKm FROM @Drumuri;

SET IDENTITY_INSERT autogara.Statii ON;
INSERT INTO autogara.Statii (StatieID, NodID, Adresa, Peron) VALUES
    (1,  1,  N'str. Mitropolit Varlaam 58, Chișinău',     N'P3'),
    (2,  2,  N'str. Calea Moșilor 2/1, Chișinău',         N'P5'),
    (3,  3,  N'str. Vasile Mahu 150, Orhei',              N'P1'),
    (4,  4,  N'str. Ștefan cel Mare 2, Bălți',            N'P4'),
    (5,  5,  N'str. Independenței 88, Soroca',            N'P2'),
    (6,  6,  N'str. Independenței 4, Edineț',             N'P1'),
    (7,  7,  N'str. Ștefan cel Mare 128, Strășeni',       N'P1'),
    (8,  8,  N'str. Alexei Mateevici 2, Călărași',        N'P1'),
    (9,  9,  N'str. Națională 1, Ungheni',                N'P2'),
    (10, 10, N'str. Mihalcea Hîncu 140, Hîncești',        N'P1'),
    (11, 11, N'str. Ștefan cel Mare 20, Cimișlia',        N'P1'),
    (12, 12, N'str. Lenin 150, Comrat',                   N'P2'),
    (13, 13, N'str. Ștefan cel Mare 2A, Cahul',           N'P3');
SET IDENTITY_INSERT autogara.Statii OFF;

DECLARE @Azi DATE = CAST(autogara.fn_AcumLocal() AS DATE);

SET IDENTITY_INSERT autogara.Autobuze ON;
INSERT INTO autogara.Autobuze
    (AutobuzID, NrInmatriculare, Model, CapacitateLocuri, Status, CaleFisierJSON, DataExpirareITP, Activ, CreatLa)
VALUES
    (1, N'KVA 318', N'Mercedes-Benz Sprinter 516 CDI',   20, N'Activ',       N'Autobuze\autobuz_1.json', DATEADD(DAY, 178, @Azi), 1, DATEADD(DAY, -400, SYSUTCDATETIME())),
    (2, N'KVB 427', N'Mercedes-Benz Sprinter 516 CDI',   20, N'Activ',       N'Autobuze\autobuz_2.json', DATEADD(DAY,   9, @Azi), 1, DATEADD(DAY, -400, SYSUTCDATETIME())),
    (3, N'DRL 902', N'Iveco Daily 50C18 Tourys',         22, N'Activ',       N'Autobuze\autobuz_3.json', DATEADD(DAY, 241, @Azi), 1, DATEADD(DAY, -350, SYSUTCDATETIME())),
    (4, N'SMB 115', N'Setra S 415 HD',                   49, N'Activ',       N'Autobuze\autobuz_4.json', DATEADD(DAY,  96, @Azi), 1, DATEADD(DAY, -390, SYSUTCDATETIME())),
    (5, N'SMC 604', N'Neoplan Tourliner N2216 SHD',      49, N'Activ',       N'Autobuze\autobuz_5.json', DATEADD(DAY, 305, @Azi), 1, DATEADD(DAY, -300, SYSUTCDATETIME())),
    (6, N'TRX 771', N'MAN Lion''s Coach R07',            51, N'Activ',       N'Autobuze\autobuz_6.json', DATEADD(DAY,  63, @Azi), 1, DATEADD(DAY, -280, SYSUTCDATETIME())),
    (7, N'ORH 208', N'Isuzu Novo Lux',                   31, N'Service',     N'Autobuze\autobuz_7.json', DATEADD(DAY,  -5, @Azi), 1, DATEADD(DAY, -410, SYSUTCDATETIME())),
    (8, N'FDL 336', N'Ford Transit 460 Bus',             18, N'Scos din uz', N'Autobuze\autobuz_8.json', DATEADD(DAY, -130, @Azi), 0, DATEADD(DAY, -900, SYSUTCDATETIME()));
SET IDENTITY_INSERT autogara.Autobuze OFF;

INSERT INTO autogara.MentenantaAutobuze (AutobuzID, TipLucrare, Data, Kilometraj, Observatii) VALUES
    (1, N'Schimb ulei motor și filtre',          DATEADD(DAY, -62,  @Azi), 312450, N'Ulei 5W-30, filtru ulei, aer și motorină'),
    (1, N'Inspecție tehnică periodică (ITP)',     DATEADD(DAY, -187, @Azi), 298110, N'Admis fără observații'),
    (2, N'Înlocuire plăcuțe frână față',          DATEADD(DAY, -35,  @Azi), 287930, N'Discurile în toleranță'),
    (2, N'Inspecție tehnică periodică (ITP)',     DATEADD(DAY, -356, @Azi), 251400, N'Admis; ITP expiră în curând — programare necesară'),
    (3, N'Schimb anvelope (iarnă → vară)',        DATEADD(DAY, -150, @Azi), 198760, N'Anvelope Continental VanContact 225/65 R16'),
    (3, N'Revizie sistem climatizare',            DATEADD(DAY, -98,  @Azi), 204300, N'Încărcare freon, filtru habitaclu nou'),
    (4, N'Schimb ulei motor și filtre',           DATEADD(DAY, -21,  @Azi), 874210, NULL),
    (4, N'Reparație ușă pasageri spate',          DATEADD(DAY, -74,  @Azi), 866980, N'Înlocuit cilindru pneumatic'),
    (4, N'Inspecție tehnică periodică (ITP)',     DATEADD(DAY, -269, @Azi), 835500, N'Admis'),
    (5, N'Înlocuire kit ambreiaj',                DATEADD(DAY, -44,  @Azi), 692040, N'Kit Sachs, volantă verificată'),
    (5, N'Inspecție tehnică periodică (ITP)',     DATEADD(DAY, -60,  @Azi), 689870, N'Admis'),
    (6, N'Schimb ulei cutie de viteze',           DATEADD(DAY, -12,  @Azi), 541330, NULL),
    (7, N'Diagnoză motor — pierdere putere',      DATEADD(DAY, -6,   @Azi), 403870, N'Turbină defectă; piesa comandată, autobuz în service'),
    (7, N'Inspecție tehnică periodică (ITP)',     DATEADD(DAY, -370, @Azi), 366020, N'Admis'),
    (8, N'Evaluare stare tehnică',                DATEADD(DAY, -140, @Azi), 689500, N'Caroserie corodată, reparație neeconomică — propus pentru casare');

SET IDENTITY_INSERT autogara.Soferi ON;
INSERT INTO autogara.Soferi (SoferID, Nume, Prenume, NrPermis, Telefon, Activ) VALUES
    (1, N'Țurcanu',  N'Vasile',     N'PC 0391254', N'+373 69 402 118', 1),
    (2, N'Grosu',    N'Nicolae',    N'PC 0527841', N'+373 79 118 450', 1),
    (3, N'Moraru',   N'Serghei',    N'PC 0610337', N'+373 68 773 902', 1),
    (4, N'Bivol',    N'Petru',      N'PC 0284519', N'+373 69 250 667', 1),
    (5, N'Sîrbu',    N'Andrei',     N'PC 0745102', N'+373 78 604 381', 1),
    (6, N'Rotaru',   N'Veaceslav',  N'PC 0458963', N'+373 60 331 725', 1),
    (7, N'Cebotari', N'Iurie',      N'PC 0819274', N'+373 69 997 014', 1),
    (8, N'Guțu',     N'Dumitru',    N'PC 0197630', N'+373 79 540 286', 0);
SET IDENTITY_INSERT autogara.Soferi OFF;

SET IDENTITY_INSERT autogara.Trasee ON;
INSERT INTO autogara.Trasee (TraseuID, Denumire, Activ) VALUES
    (1,  N'Chișinău (Nord) – Orhei – Bălți',                 1),
    (2,  N'Bălți – Orhei – Chișinău (Nord)',                 1),
    (3,  N'Chișinău – Strășeni – Călărași – Ungheni',        1),
    (4,  N'Ungheni – Călărași – Strășeni – Chișinău',        1),
    (5,  N'Chișinău – Cimișlia – Comrat – Cahul',            1),
    (6,  N'Cahul – Comrat – Cimișlia – Chișinău',            1),
    (7,  N'Chișinău (Nord) – Orhei – Soroca',                1),
    (8,  N'Soroca – Orhei – Chișinău (Nord)',                1),
    (9,  N'Bălți – Edineț',                                  1),
    (10, N'Edineț – Bălți',                                  1),
    (11, N'Chișinău – Hîncești',                             1),
    (12, N'Hîncești – Chișinău',                             1),
    (13, N'Chișinău – Hîncești – Cimișlia',                  0);
SET IDENTITY_INSERT autogara.Trasee OFF;

INSERT INTO autogara.TraseuOpriri (TraseuID, StatieID, Ordine) VALUES
    (1, 2, 1),  (1, 3, 2),  (1, 4, 3),
    (2, 4, 1),  (2, 3, 2),  (2, 2, 3),
    (3, 1, 1),  (3, 7, 2),  (3, 8, 3),  (3, 9, 4),
    (4, 9, 1),  (4, 8, 2),  (4, 7, 3),  (4, 1, 4),
    (5, 1, 1),  (5, 11, 2), (5, 12, 3), (5, 13, 4),
    (6, 13, 1), (6, 12, 2), (6, 11, 3), (6, 1, 4),
    (7, 2, 1),  (7, 3, 2),  (7, 5, 3),
    (8, 5, 1),  (8, 3, 2),  (8, 2, 3),
    (9, 4, 1),  (9, 6, 2),
    (10, 6, 1), (10, 4, 2),
    (11, 1, 1), (11, 10, 2),
    (12, 10, 1), (12, 1, 2),
    (13, 1, 1), (13, 10, 2), (13, 11, 3);

SET IDENTITY_INSERT autogara.TipuriReducere ON;
INSERT INTO autogara.TipuriReducere (TipReducereID, Denumire, ProcentReducere, Activ) VALUES
    (1, N'Elev',       50.00, 1),
    (2, N'Pensionar',  30.00, 1),
    (3, N'Abonament',  20.00, 1);
SET IDENTITY_INSERT autogara.TipuriReducere OFF;

DECLARE @Orar TABLE
(
    TraseuID INT, AutobuzID INT, SoferID INT,
    OraPlecare TIME(0), OraSosire TIME(0), Pret DECIMAL(10, 2)
);
INSERT INTO @Orar VALUES
    (12, 3, 7, '06:50', '07:45',  38.00),
    (5,  5, 4, '06:45', '10:20', 155.00),
    (1,  4, 1, '07:30', '10:05', 120.00),
    (3,  3, 3, '08:15', '10:10',  85.00),
    (7,  6, 5, '09:00', '11:55', 140.00),
    (9,  2, 6, '11:00', '12:25',  60.00),
    (1,  1, 2, '12:10', '14:40', 120.00),
    (10, 2, 6, '13:30', '14:55',  60.00),
    (4,  3, 3, '13:30', '15:25',  85.00),
    (6,  5, 4, '14:00', '17:35', 155.00),
    (2,  4, 1, '15:00', '17:35', 120.00),
    (8,  6, 5, '15:30', '18:25', 140.00),
    (2,  1, 2, '17:50', '20:20', 120.00),
    (11, 3, 7, '18:10', '19:05',  38.00);

DECLARE @Acum DATETIME2(0) = autogara.fn_AcumLocal();

WITH Zile AS
(
    SELECT DATEADD(DAY, v.n - 7, @Azi) AS DataCursa
    FROM (VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14)) AS v (n)
)
INSERT INTO autogara.Curse
    (TraseuID, AutobuzID, SoferID, DataCursa, OraPlecare, OraSosireEstimata, Pret, Status, CreatLa)
SELECT
    o.TraseuID, o.AutobuzID, o.SoferID, z.DataCursa, o.OraPlecare, o.OraSosire, o.Pret,
    CASE
        WHEN z.DataCursa = DATEADD(DAY, -3, @Azi) AND o.TraseuID IN (9, 10)      THEN N'Anulata'
        WHEN autogara.fn_MomentCursa(z.DataCursa, o.OraSosire)  <= @Acum          THEN N'Finalizata'
        WHEN autogara.fn_MomentCursa(z.DataCursa, o.OraPlecare) <= @Acum          THEN N'In desfasurare'
        ELSE N'Planificata'
    END,
    DATEADD(DAY, -21, CAST(z.DataCursa AS DATETIME2(3)))
FROM Zile AS z
CROSS JOIN @Orar AS o
ORDER BY z.DataCursa, o.OraPlecare;

DECLARE @CursaID INT;
DECLARE curse CURSOR LOCAL FAST_FORWARD FOR SELECT CursaID FROM autogara.Curse ORDER BY CursaID;
OPEN curse;
FETCH NEXT FROM curse INTO @CursaID;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC autogara.sp_CreeazaLocuriPentruCursa @CursaID = @CursaID;
    FETCH NEXT FROM curse INTO @CursaID;
END
CLOSE curse;
DEALLOCATE curse;

DECLARE @Prenume TABLE (Id INT IDENTITY(0, 1), Prenume NVARCHAR(50));
INSERT INTO @Prenume (Prenume) VALUES
    (N'Ion'), (N'Maria'), (N'Vasile'), (N'Elena'), (N'Gheorghe'), (N'Ana'), (N'Nicolae'), (N'Tatiana'),
    (N'Andrei'), (N'Natalia'), (N'Mihai'), (N'Olga'), (N'Alexandru'), (N'Svetlana'), (N'Dumitru'), (N'Valentina'),
    (N'Sergiu'), (N'Cristina'), (N'Victor'), (N'Irina'), (N'Pavel'), (N'Lilia'), (N'Iurie'), (N'Galina'),
    (N'Vitalie'), (N'Doina'), (N'Constantin'), (N'Mariana'), (N'Radu'), (N'Ludmila'), (N'Octavian'), (N'Veronica'),
    (N'Petru'), (N'Aurelia'), (N'Ștefan'), (N'Diana'), (N'Denis'), (N'Adriana'), (N'Eugen'), (N'Nadejda');

DECLARE @NumeFamilie TABLE (Id INT IDENTITY(0, 1), Nume NVARCHAR(50));
INSERT INTO @NumeFamilie (Nume) VALUES
    (N'Rusu'), (N'Ceban'), (N'Munteanu'), (N'Lungu'), (N'Popa'), (N'Botnaru'), (N'Cojocaru'), (N'Bivol'),
    (N'Ciobanu'), (N'Moraru'), (N'Țurcanu'), (N'Rotaru'), (N'Guțu'), (N'Cebotari'), (N'Sîrbu'), (N'Grosu'),
    (N'Baciu'), (N'Melnic'), (N'Croitoru'), (N'Pascari'), (N'Vrabie'), (N'Ursu'), (N'Plămădeală'), (N'Cazacu'),
    (N'Roșca'), (N'Istrati'), (N'Căpățînă'), (N'Mocanu'), (N'Negru'), (N'Oprea'), (N'Andronic'), (N'Buzu'),
    (N'Sandu'), (N'Tofan'), (N'Leahu'), (N'Gârlea'), (N'Josan'), (N'Frunză'), (N'Cucoș'), (N'Postolachi');

DECLARE @Casieri TABLE (Id INT IDENTITY(0, 1), UtilizatorID UNIQUEIDENTIFIER);
INSERT INTO @Casieri (UtilizatorID)
SELECT UtilizatorID FROM autogara.Utilizatori
WHERE NumeUtilizator IN (N'tatiana.rusu', N'victoria.ceban', N'ion.munteanu', N'natalia.bivol')
ORDER BY NumeUtilizator;

DECLARE @NrCasieri INT = (SELECT COUNT(*) FROM @Casieri);
DECLARE @PrefixeMobil NCHAR(12) = N'606268697879';

IF OBJECT_ID('tempdb..#Vanzari') IS NOT NULL DROP TABLE #Vanzari;

WITH Candidati AS
(
    SELECT
        l.LocID, l.CursaID, l.NumarLoc,
        c.DataCursa, c.Pret AS PretBaza, c.Status AS StatusCursa,
        autogara.fn_MomentCursa(c.DataCursa, c.OraPlecare) AS Plecare,
        CAST(SUBSTRING(hb.H, 1, 3)  AS INT) AS H1,
        CAST(SUBSTRING(hb.H, 4, 3)  AS INT) AS H2,
        CAST(SUBSTRING(hb.H, 7, 3)  AS INT) AS H3,
        CAST(SUBSTRING(hb.H, 10, 3) AS INT) AS H4,
        CAST(SUBSTRING(hc.H, 1, 3)  AS INT) AS HCursa
    FROM autogara.Locuri AS l
    JOIN autogara.Curse  AS c ON c.CursaID = l.CursaID
    CROSS APPLY (SELECT HASHBYTES('MD5', CONCAT(N'loc-', l.LocID)) AS H) AS hb
    CROSS APPLY (SELECT HASHBYTES('MD5', CONCAT(N'cursa-', c.CursaID)) AS H) AS hc
)
SELECT
    k.*,
    CASE
        WHEN k.StatusCursa = N'Anulata'                            THEN 35
        WHEN k.StatusCursa IN (N'Finalizata', N'In desfasurare')   THEN 55 + k.HCursa % 40
        ELSE 8 + CASE WHEN 62 - 9 * DATEDIFF(DAY, @Azi, k.DataCursa) > 0
                      THEN 62 - 9 * DATEDIFF(DAY, @Azi, k.DataCursa) ELSE 0 END
               + k.HCursa % 12
    END AS ProcentOcupare
INTO #Vanzari
FROM Candidati AS k;

DELETE FROM #Vanzari WHERE H1 % 100 >= ProcentOcupare;

ALTER TABLE #Vanzari ADD
    BiletID UNIQUEIDENTIFIER NULL, CodBilet NVARCHAR(20) NULL, NumePasager NVARCHAR(200) NULL,
    TelefonPasager NVARCHAR(20) NULL, TipReducereID INT NULL, VanzutDe UNIQUEIDENTIFIER NULL,
    DataEmitereLocal DATETIME2(0) NULL, Pret DECIMAL(10, 2) NULL, StatusBilet NVARCHAR(20) NULL,
    ProcentRambursare DECIMAL(5, 2) NULL, MetodaPlata NVARCHAR(10) NULL;

UPDATE v
SET BiletID        = NEWID(),
    CodBilet       = CONCAT(N'AG', CONVERT(CHAR(6), v.DataCursa, 12), N'-',
                            CONVERT(VARCHAR(8), CONVERT(BINARY(4), CAST(v.LocID AS BIGINT) * CAST(2654435761 AS BIGINT) % CAST(4294967296 AS BIGINT)), 2)),
    NumePasager    = CONCAT(p.Prenume, N' ', n.Nume),
    TelefonPasager = CASE WHEN v.H4 % 100 < 85
                          THEN CONCAT(N'+373 ', SUBSTRING(@PrefixeMobil, 2 * (v.H2 % 6) + 1, 2), N' ',
                                      RIGHT(CONCAT(N'00', v.H3 % 1000), 3), N' ', RIGHT(CONCAT(N'00', v.H4 % 1000), 3))
                     END,
    TipReducereID  = CASE WHEN v.H3 % 100 < 9  THEN 1
                          WHEN v.H3 % 100 < 21 THEN 2
                          WHEN v.H3 % 100 < 25 THEN 3 END,
    VanzutDe       = cs.UtilizatorID,
    MetodaPlata    = CASE WHEN v.H2 % 100 < 38 THEN N'Card' ELSE N'Numerar' END,
    DataEmitereLocal = CASE
        WHEN DATEADD(MINUTE, -(20 + v.H1 % 7200), v.Plecare) <= @Acum
            THEN DATEADD(MINUTE, -(20 + v.H1 % 7200), v.Plecare)
        ELSE DATEADD(MINUTE, -(5 + v.H1 % 2880), @Acum)
    END
FROM #Vanzari AS v
JOIN @Prenume     AS p  ON p.Id  = v.H2 % 40
JOIN @NumeFamilie AS n  ON n.Id  = v.H3 % 40
JOIN @Casieri     AS cs ON cs.Id = v.H4 % @NrCasieri;

UPDATE #Vanzari
SET Pret = autogara.fn_CalculeazaPret(PretBaza, TipReducereID),
    StatusBilet = CASE
        WHEN StatusCursa = N'Anulata'   THEN N'Rambursat'
        WHEN H4 % 100 IN (0, 1, 2)      THEN N'Rambursat'
        WHEN H4 % 100 = 3
             AND StatusCursa <> N'Planificata' THEN N'Anulat'
        ELSE N'Activ'
    END;

UPDATE #Vanzari
SET ProcentRambursare = CASE
        WHEN StatusBilet <> N'Rambursat' THEN 0
        WHEN StatusCursa = N'Anulata'    THEN 100
        WHEN H4 % 100 = 2                THEN 50
        ELSE 100
    END;

INSERT INTO autogara.Bilete
    (BiletID, CodBilet, CursaID, LocID, NumePasager, TelefonPasager, TipReducereID,
     VanzutDeUtilizatorID, DataEmitere, Pret, Status, ModificatLa)
SELECT
    BiletID, CodBilet, CursaID, LocID, NumePasager, TelefonPasager, TipReducereID,
    VanzutDe, autogara.fn_LocalLaUtc(DataEmitereLocal), Pret, StatusBilet,
    CASE WHEN StatusBilet <> N'Activ'
         THEN autogara.fn_LocalLaUtc(DATEADD(MINUTE, DATEDIFF(MINUTE, DataEmitereLocal,
                  CASE WHEN Plecare < @Acum THEN Plecare ELSE @Acum END) / 2, DataEmitereLocal)) END
FROM #Vanzari;

INSERT INTO autogara.Plati (BiletID, Suma, MetodaPlata, DataPlata, Status, NumarBonFiscal)
SELECT
    BiletID, Pret, MetodaPlata, autogara.fn_LocalLaUtc(DataEmitereLocal), N'Finalizata',
    CONCAT(N'BF', CONVERT(CHAR(6), DataEmitereLocal, 12), N'-',
           RIGHT(CONCAT(N'0000', ROW_NUMBER() OVER (PARTITION BY CAST(DataEmitereLocal AS DATE) ORDER BY DataEmitereLocal, LocID)), 4))
FROM #Vanzari;

INSERT INTO autogara.Plati (BiletID, Suma, MetodaPlata, DataPlata, Status, NumarBonFiscal)
SELECT
    v.BiletID, ROUND(v.Pret * v.ProcentRambursare / 100, 2), v.MetodaPlata, b.ModificatLa, N'Rambursata',
    CONCAT(N'RB', CONVERT(CHAR(6), autogara.fn_UtcLaLocal(b.ModificatLa), 12), N'-',
           RIGHT(CONCAT(N'0000', ROW_NUMBER() OVER (PARTITION BY CAST(autogara.fn_UtcLaLocal(b.ModificatLa) AS DATE) ORDER BY b.ModificatLa, v.LocID)), 4))
FROM #Vanzari AS v
JOIN autogara.Bilete AS b ON b.BiletID = v.BiletID
WHERE v.StatusBilet = N'Rambursat';

UPDATE l
SET l.Status = N'Ocupat'
FROM autogara.Locuri AS l
JOIN autogara.Bilete AS b ON b.LocID = l.LocID
WHERE b.Status = N'Activ';

DROP TABLE #Vanzari;

DECLARE @CursaMaine INT =
    (SELECT TOP (1) CursaID FROM autogara.Curse
     WHERE TraseuID = 1 AND DataCursa = DATEADD(DAY, 1, @Azi) AND Status = N'Planificata'
     ORDER BY OraPlecare);

DECLARE @CasierRezervare UNIQUEIDENTIFIER =
    (SELECT UtilizatorID FROM autogara.Utilizatori WHERE NumeUtilizator = N'tatiana.rusu');

DECLARE @LocuriRezervate TABLE (Rand INT IDENTITY(1, 1), LocID INT);
INSERT INTO @LocuriRezervate (LocID)
SELECT TOP (4) LocID FROM autogara.Locuri
WHERE CursaID = @CursaMaine AND Status = N'Liber'
ORDER BY NumarLoc DESC;

INSERT INTO autogara.RezervariProvizorii (LocID, UtilizatorID, DataCreare, DataExpirare)
SELECT
    LocID, @CasierRezervare,
    CASE WHEN Rand = 4 THEN DATEADD(MINUTE, -13, SYSUTCDATETIME()) ELSE DATEADD(MINUTE, -2, SYSUTCDATETIME()) END,
    CASE WHEN Rand = 4 THEN DATEADD(MINUTE, -3,  SYSUTCDATETIME()) ELSE DATEADD(MINUTE,  8, SYSUTCDATETIME()) END
FROM @LocuriRezervate;

UPDATE l SET l.Status = N'Rezervat'
FROM autogara.Locuri AS l
JOIN @LocuriRezervate AS r ON r.LocID = l.LocID;

DECLARE @Admin  UNIQUEIDENTIFIER = (SELECT UtilizatorID FROM autogara.Utilizatori WHERE NumeUtilizator = N'gheorghe.lungu');
DECLARE @Rodica UNIQUEIDENTIFIER = (SELECT UtilizatorID FROM autogara.Utilizatori WHERE NumeUtilizator = N'rodica.cazacu');
DECLARE @Olga   UNIQUEIDENTIFIER = (SELECT UtilizatorID FROM autogara.Utilizatori WHERE NumeUtilizator = N'olga.cojocaru');
DECLARE @Victoria UNIQUEIDENTIFIER = (SELECT UtilizatorID FROM autogara.Utilizatori WHERE NumeUtilizator = N'victoria.ceban');

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID, DataOra) VALUES
    (@Rodica, N'Import inițial hartă: 15 noduri, 30 conexiuni',                    N'Noduri',      NULL,  DATEADD(DAY, -30, SYSUTCDATETIME())),
    (@Admin, N'Creare autobuz TRX 771 (MAN Lion''s Coach R07, 51 locuri)',       N'Autobuze',    N'6',  DATEADD(DAY, -28, SYSUTCDATETIME())),
    (@Admin, N'Actualizare structură locuri autobuz SMB 115',                     N'Autobuze',    N'4',  DATEADD(DAY, -26, SYSUTCDATETIME())),
    (@Admin, N'Suspendare traseu Chișinău – Hîncești – Cimișlia',                 N'Trasee',      N'13', DATEADD(DAY, -22, SYSUTCDATETIME())),
    (@Admin, N'Dezactivare utilizator olga.cojocaru',                             N'Utilizatori', CONVERT(NVARCHAR(50), @Olga), DATEADD(DAY, -20, SYSUTCDATETIME())),
    (@Admin, N'Creare orar curse pentru următoarele 14 zile',                     N'Curse',       NULL,  DATEADD(DAY, -8, SYSUTCDATETIME())),
    (@Admin, N'Autobuz ORH 208 trecut în Service — turbină defectă',              N'Autobuze',    N'7',  DATEADD(DAY, -6, SYSUTCDATETIME())),
    (@Admin, N'Anulare curse Bălți ↔ Edineț — defecțiune tehnică autobuz KVB 427', N'Curse',      NULL,  DATEADD(DAY, -3, DATEADD(HOUR, -6, SYSUTCDATETIME()))),
    (NULL,   N'ALERTĂ ITP: KVB 427 (Mercedes-Benz Sprinter 516 CDI) expiră în 9 zile', N'Autobuze', N'2', DATEADD(HOUR, -4, SYSUTCDATETIME())),
    (@Victoria, N'Autentificare reușită',                                         N'Utilizatori', CONVERT(NVARCHAR(50), @Victoria), DATEADD(HOUR, -3, SYSUTCDATETIME())),
    (NULL,   N'Autentificare eșuată pentru utilizatorul olga.cojocaru (cont dezactivat)', N'Utilizatori', CONVERT(NVARCHAR(50), @Olga), DATEADD(HOUR, -2, SYSUTCDATETIME()));

COMMIT TRANSACTION;
GO

SELECT N'Roluri' AS Tabel, COUNT(*) AS Randuri FROM autogara.Roluri
UNION ALL SELECT N'Utilizatori',         COUNT(*) FROM autogara.Utilizatori
UNION ALL SELECT N'Noduri',              COUNT(*) FROM autogara.Noduri
UNION ALL SELECT N'Conexiuni',           COUNT(*) FROM autogara.Conexiuni
UNION ALL SELECT N'Statii',              COUNT(*) FROM autogara.Statii
UNION ALL SELECT N'Autobuze',            COUNT(*) FROM autogara.Autobuze
UNION ALL SELECT N'MentenantaAutobuze',  COUNT(*) FROM autogara.MentenantaAutobuze
UNION ALL SELECT N'Soferi',              COUNT(*) FROM autogara.Soferi
UNION ALL SELECT N'Trasee',              COUNT(*) FROM autogara.Trasee
UNION ALL SELECT N'TraseuOpriri',        COUNT(*) FROM autogara.TraseuOpriri
UNION ALL SELECT N'TipuriReducere',      COUNT(*) FROM autogara.TipuriReducere
UNION ALL SELECT N'Curse',               COUNT(*) FROM autogara.Curse
UNION ALL SELECT N'Locuri',              COUNT(*) FROM autogara.Locuri
UNION ALL SELECT N'RezervariProvizorii', COUNT(*) FROM autogara.RezervariProvizorii
UNION ALL SELECT N'Bilete',              COUNT(*) FROM autogara.Bilete
UNION ALL SELECT N'Plati',               COUNT(*) FROM autogara.Plati
UNION ALL SELECT N'LogAudit',            COUNT(*) FROM autogara.LogAudit;
GO
