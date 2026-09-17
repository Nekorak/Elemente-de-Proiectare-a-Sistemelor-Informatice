/* =====================================================================
   08_Security.sql
   Conturi SQL Server dedicate aplicației și permisiuni minime.

   Rulare în SQLCMD mode (SSMS: Query > SQLCMD Mode) sau cu sqlcmd:
     sqlcmd -S <server>,<port> -U <dba> -i 08_Security.sql ^
            -v ParolaApp="..." ParolaRapoarte="..."
   Parolele NU se scriu în acest fișier — se trec ca variabile SQLCMD
   (vezi Credentials.txt, completat local).

   Pași de configurare care NU se fac din T-SQL (SQL Server Configuration
   Manager), obligatorii înainte de expunerea serverului prin port-forwarding:
     1. Autentificare mixtă (SQL Server and Windows Authentication mode).
     2. TCP/IP activat, port static non-implicit (ex. 14330), port dinamic gol.
     3. Certificat TLS asociat instanței + "Force Encryption = Yes".
     4. Contul "sa" dezactivat; contul DBA separat folosit doar manual.
     5. Firewall: permis doar portul SQL ales.
   ===================================================================== */

:on error exit

USE master;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF N'$(ParolaApp)' = N'' OR N'$(ParolaApp)' LIKE N'<%'
   OR N'$(ParolaRapoarte)' = N'' OR N'$(ParolaRapoarte)' LIKE N'<%'
    RAISERROR (N'Completați variabilele SQLCMD ParolaApp și ParolaRapoarte înainte de rulare.', 16, 1);
GO

/* ------------------------------ Login-uri --------------------------- */

IF SUSER_ID(N'autogara_app') IS NULL
    CREATE LOGIN autogara_app
        WITH PASSWORD = N'$(ParolaApp)',
             DEFAULT_DATABASE = autogara,
             CHECK_POLICY = ON,
             CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN autogara_app WITH PASSWORD = N'$(ParolaApp)';
GO

IF SUSER_ID(N'autogara_rapoarte') IS NULL
    CREATE LOGIN autogara_rapoarte
        WITH PASSWORD = N'$(ParolaRapoarte)',
             DEFAULT_DATABASE = autogara,
             CHECK_POLICY = ON,
             CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN autogara_rapoarte WITH PASSWORD = N'$(ParolaRapoarte)';
GO

/* -------------------------- Utilizatori în BD ----------------------- */

USE autogara;
GO

IF USER_ID(N'autogara_app') IS NULL
    CREATE USER autogara_app FOR LOGIN autogara_app WITH DEFAULT_SCHEMA = autogara;
GO

IF USER_ID(N'autogara_rapoarte') IS NULL
    CREATE USER autogara_rapoarte FOR LOGIN autogara_rapoarte WITH DEFAULT_SCHEMA = autogara;
GO

/* ----------------------------- Permisiuni --------------------------- */

-- Aplicația: citire + scriere + execuție proceduri/funcții (NU db_owner)
ALTER ROLE db_datareader ADD MEMBER autogara_app;
ALTER ROLE db_datawriter ADD MEMBER autogara_app;
GRANT EXECUTE ON SCHEMA::autogara TO autogara_app;
GO

-- Jurnalul de audit nu poate fi modificat sau șters din aplicație
-- (curățarea se face doar de Job_CurataLogAudit, rulat de SQL Server Agent).
DENY UPDATE, DELETE ON OBJECT::autogara.LogAudit TO autogara_app;
GO

-- Aplicația nu poate modifica structura bazei de date
DENY ALTER ON SCHEMA::autogara TO autogara_app;
DENY CREATE TABLE, CREATE VIEW, CREATE PROCEDURE, CREATE FUNCTION TO autogara_app;
GO

-- Rapoarte: doar citire
ALTER ROLE db_datareader ADD MEMBER autogara_rapoarte;
DENY INSERT, UPDATE, DELETE ON SCHEMA::autogara TO autogara_rapoarte;
-- hash-urile parolelor nu sunt necesare pentru rapoarte
DENY SELECT ON OBJECT::autogara.Utilizatori (ParolaHash) TO autogara_rapoarte;
GO

/* ----------------------------- Verificare --------------------------- */

SELECT
    dp.name                     AS Utilizator,
    r.name                      AS RolBazaDeDate
FROM sys.database_principals AS dp
LEFT JOIN sys.database_role_members AS rm ON rm.member_principal_id = dp.principal_id
LEFT JOIN sys.database_principals   AS r  ON r.principal_id = rm.role_principal_id
WHERE dp.name IN (N'autogara_app', N'autogara_rapoarte')
ORDER BY dp.name, r.name;
GO
