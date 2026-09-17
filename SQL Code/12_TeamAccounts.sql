/* =====================================================================
   12_TeamAccounts.sql
   Conturile personale ale echipei:
     A. login-uri SQL Server (acces direct la baza de date din SSMS)
     B. conturi în aplicație (tabelul Utilizatori)

   Se rulează DUPĂ 11_Seed.sql. Seed-ul golește tabelul Utilizatori,
   deci după fiecare rulare a seed-ului trebuie rulat și acest script.
   Parolele se dau ca variabile SQLCMD (vezi 00_RunAll.sql / Credentials.txt).

   Login-uri SQL și drepturi:
     dba_gojinevschii  Bază de Date  sysadmin (contul DBA, folosit manual)
     dev_hanganu       Backend       citire + scriere + EXECUTE + VIEW DEFINITION
     dev_crivenco      Frontend      citire + EXECUTE + VIEW DEFINITION
     test_sopivnic     Tester        citire + scriere + EXECUTE + pornire joburi Agent

   Conturi în aplicație: toți patru au rolul Admin. Rolurile Casier și
   Pasager se testează cu conturile din seed (ex. tatiana.rusu, ana.popa).
   ===================================================================== */

:on error exit

USE master;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT 1 FROM (VALUES
        (N'$(ParolaSqlDmitri)'), (N'$(ParolaSqlSergiu)'), (N'$(ParolaSqlAlexandr)'), (N'$(ParolaSqlMaxim)'),
        (N'$(ParolaAppDmitri)'), (N'$(ParolaAppSergiu)'), (N'$(ParolaAppAlexandr)'), (N'$(ParolaAppMaxim)')) AS p (Parola)
    WHERE Parola = N'' OR Parola LIKE N'<%')
    RAISERROR (N'Completați toate variabilele SQLCMD cu parolele echipei înainte de rulare.', 16, 1);
GO

/* ===================== A. Login-uri SQL Server ===================== */

IF SUSER_ID(N'dba_gojinevschii') IS NULL
    CREATE LOGIN dba_gojinevschii WITH PASSWORD = N'$(ParolaSqlDmitri)',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN dba_gojinevschii WITH PASSWORD = N'$(ParolaSqlDmitri)';

IF SUSER_ID(N'dev_hanganu') IS NULL
    CREATE LOGIN dev_hanganu WITH PASSWORD = N'$(ParolaSqlSergiu)',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN dev_hanganu WITH PASSWORD = N'$(ParolaSqlSergiu)';

IF SUSER_ID(N'dev_crivenco') IS NULL
    CREATE LOGIN dev_crivenco WITH PASSWORD = N'$(ParolaSqlAlexandr)',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN dev_crivenco WITH PASSWORD = N'$(ParolaSqlAlexandr)';

IF SUSER_ID(N'test_sopivnic') IS NULL
    CREATE LOGIN test_sopivnic WITH PASSWORD = N'$(ParolaSqlMaxim)',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN test_sopivnic WITH PASSWORD = N'$(ParolaSqlMaxim)';
GO

-- DBA: administrator complet al instanței
ALTER SERVER ROLE sysadmin ADD MEMBER dba_gojinevschii;
GO

USE autogara;
GO

IF USER_ID(N'dev_hanganu') IS NULL
    CREATE USER dev_hanganu FOR LOGIN dev_hanganu WITH DEFAULT_SCHEMA = autogara;
IF USER_ID(N'dev_crivenco') IS NULL
    CREATE USER dev_crivenco FOR LOGIN dev_crivenco WITH DEFAULT_SCHEMA = autogara;
IF USER_ID(N'test_sopivnic') IS NULL
    CREATE USER test_sopivnic FOR LOGIN test_sopivnic WITH DEFAULT_SCHEMA = autogara;
GO

-- Backend: lucrează cu datele și procedurile, vede structura (EF Core), nu modifică schema
ALTER ROLE db_datareader ADD MEMBER dev_hanganu;
ALTER ROLE db_datawriter ADD MEMBER dev_hanganu;
GRANT EXECUTE, VIEW DEFINITION ON SCHEMA::autogara TO dev_hanganu;
GRANT SHOWPLAN TO dev_hanganu;

-- Frontend: vede datele și structura, poate apela procedurile
ALTER ROLE db_datareader ADD MEMBER dev_crivenco;
GRANT EXECUTE, VIEW DEFINITION ON SCHEMA::autogara TO dev_crivenco;

-- Tester: citire + scriere pentru scenarii de test, execuție proceduri
ALTER ROLE db_datareader ADD MEMBER test_sopivnic;
ALTER ROLE db_datawriter ADD MEMBER test_sopivnic;
GRANT EXECUTE, VIEW DEFINITION ON SCHEMA::autogara TO test_sopivnic;

-- Jurnalul de audit rămâne nemodificabil pentru toată lumea în afară de DBA
DENY UPDATE, DELETE ON OBJECT::autogara.LogAudit TO dev_hanganu, test_sopivnic;
GO

-- Tester: poate porni manual joburile SQL Server Agent (Test joburi SQL)
USE msdb;
GO
IF USER_ID(N'test_sopivnic') IS NULL
    CREATE USER test_sopivnic FOR LOGIN test_sopivnic;
ALTER ROLE SQLAgentOperatorRole ADD MEMBER test_sopivnic;
GO

/* ====================== B. Conturi în aplicație ===================== */

USE autogara;
GO

-- Hash temporar, același format ca în 11_Seed.sql:
-- SHA2_512(NumeUtilizator + ':' + parola)
DECLARE @Echipa TABLE
(
    NumeUtilizator NVARCHAR(50), Nume NVARCHAR(100), Prenume NVARCHAR(100),
    Email NVARCHAR(150), Telefon NVARCHAR(20), Parola NVARCHAR(100)
);

INSERT INTO @Echipa VALUES
    (N'd.gojinevschii', N'Gojinevschii', N'Dmitri',    N'd.gojinevschii@autogara.local', NULL, N'$(ParolaAppDmitri)'),
    (N's.hanganu',      N'Hanganu',      N'Sergiu',    N's.hanganu@autogara.local',      NULL, N'$(ParolaAppSergiu)'),
    (N'a.crivenco',     N'Crivenco',     N'Alexandr',  N'a.crivenco@autogara.local',     NULL, N'$(ParolaAppAlexandr)'),
    (N'm.sopivnic',     N'Sopivnic',     N'Maxim',     N'm.sopivnic@autogara.local',     NULL, N'$(ParolaAppMaxim)');

DECLARE @RolAdmin INT = (SELECT RolID FROM autogara.Roluri WHERE Denumire = N'Admin');

UPDATE u
SET u.ParolaHash  = HASHBYTES('SHA2_512', CONCAT(e.NumeUtilizator, N':', e.Parola)),
    u.RolID       = @RolAdmin,
    u.Activ       = 1,
    u.ModificatLa = SYSUTCDATETIME()
FROM autogara.Utilizatori AS u
JOIN @Echipa AS e ON e.NumeUtilizator = u.NumeUtilizator;

INSERT INTO autogara.Utilizatori (NumeUtilizator, ParolaHash, Nume, Prenume, Email, Telefon, RolID, Activ)
SELECT e.NumeUtilizator, HASHBYTES('SHA2_512', CONCAT(e.NumeUtilizator, N':', e.Parola)),
       e.Nume, e.Prenume, e.Email, e.Telefon, @RolAdmin, 1
FROM @Echipa AS e
WHERE NOT EXISTS (SELECT 1 FROM autogara.Utilizatori AS u WHERE u.NumeUtilizator = e.NumeUtilizator);

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID)
SELECT NULL, CONCAT(N'Creare/actualizare cont echipă ', e.NumeUtilizator), N'Utilizatori', CONVERT(NVARCHAR(50), u.UtilizatorID)
FROM @Echipa AS e
JOIN autogara.Utilizatori AS u ON u.NumeUtilizator = e.NumeUtilizator;
GO

/* ============================ Verificare =========================== */
SELECT sp.name AS LoginSql, sp.is_disabled AS Dezactivat,
       IS_SRVROLEMEMBER(N'sysadmin', sp.name) AS Sysadmin
FROM sys.server_principals AS sp
WHERE sp.name IN (N'dba_gojinevschii', N'dev_hanganu', N'dev_crivenco', N'test_sopivnic');

SELECT u.NumeUtilizator, r.Denumire AS Rol, u.Activ
FROM autogara.Utilizatori AS u
JOIN autogara.Roluri AS r ON r.RolID = u.RolID
WHERE u.NumeUtilizator IN (N'd.gojinevschii', N's.hanganu', N'a.crivenco', N'm.sopivnic');
GO
