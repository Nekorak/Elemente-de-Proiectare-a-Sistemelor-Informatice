/* =====================================================================
   01_CreateDatabase.sql
   Creează baza de date "autogara" (dacă nu există) cu collation-ul
   Romanian_100_CI_AS și setările de bază.
   Rulare: ca DBA (sysadmin), pe instanța serverului de acasă.
   ===================================================================== */
USE master;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF DB_ID(N'autogara') IS NULL
BEGIN
    CREATE DATABASE autogara COLLATE Romanian_100_CI_AS;
END
ELSE IF CONVERT(NVARCHAR(128), DATABASEPROPERTYEX(N'autogara', 'Collation')) <> N'Romanian_100_CI_AS'
BEGIN
    RAISERROR (N'Baza de date autogara există deja, dar are alt collation decât Romanian_100_CI_AS.', 16, 1);
END
GO

/* Pentru a recrea baza de la zero în mediul de dezvoltare (DISTRUGE DATELE!):
ALTER DATABASE autogara SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE autogara;
*/

-- Backup-urile planificate sunt complete + diferențiale (fără backup de log),
-- deci modelul SIMPLE împiedică creșterea necontrolată a fișierului de log.
ALTER DATABASE autogara SET RECOVERY SIMPLE;
GO

-- Cititorii (rapoarte, dashboard) nu blochează scrierile de la ghișeu.
-- Operațiile critice folosesc explicit UPDLOCK/HOLDLOCK în proceduri.
ALTER DATABASE autogara SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE;
GO

ALTER DATABASE autogara SET PAGE_VERIFY CHECKSUM;
ALTER DATABASE autogara SET AUTO_UPDATE_STATISTICS ON;
GO
