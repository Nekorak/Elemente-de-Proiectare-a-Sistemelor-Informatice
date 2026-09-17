USE master;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF SUSER_ID(N'autogara_app') IS NULL
    CREATE LOGIN autogara_app WITH PASSWORD = N'LD*xsv=45y!ztw3v',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN autogara_app WITH PASSWORD = N'LD*xsv=45y!ztw3v';

IF SUSER_ID(N'autogara_rapoarte') IS NULL
    CREATE LOGIN autogara_rapoarte WITH PASSWORD = N'gmHb+p!tG9*5VKkK',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN autogara_rapoarte WITH PASSWORD = N'gmHb+p!tG9*5VKkK';

IF SUSER_ID(N'dba_gojinevschii') IS NULL
    CREATE LOGIN dba_gojinevschii WITH PASSWORD = N'4?CWzxVTqgeag368',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN dba_gojinevschii WITH PASSWORD = N'4?CWzxVTqgeag368';

IF SUSER_ID(N'dev_hanganu') IS NULL
    CREATE LOGIN dev_hanganu WITH PASSWORD = N'wc*Gc-kDb#6*DQcj',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN dev_hanganu WITH PASSWORD = N'wc*Gc-kDb#6*DQcj';

IF SUSER_ID(N'dev_crivenco') IS NULL
    CREATE LOGIN dev_crivenco WITH PASSWORD = N'mW8DqAu*9U4Sw-xg',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN dev_crivenco WITH PASSWORD = N'mW8DqAu*9U4Sw-xg';

IF SUSER_ID(N'test_sopivnic') IS NULL
    CREATE LOGIN test_sopivnic WITH PASSWORD = N'9=a6RKyfvNP7_RF8',
        DEFAULT_DATABASE = autogara, CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN test_sopivnic WITH PASSWORD = N'9=a6RKyfvNP7_RF8';
GO

ALTER SERVER ROLE sysadmin ADD MEMBER dba_gojinevschii;
GO

USE autogara;
GO

IF USER_ID(N'autogara_app') IS NULL
    CREATE USER autogara_app FOR LOGIN autogara_app WITH DEFAULT_SCHEMA = autogara;
IF USER_ID(N'autogara_rapoarte') IS NULL
    CREATE USER autogara_rapoarte FOR LOGIN autogara_rapoarte WITH DEFAULT_SCHEMA = autogara;
IF USER_ID(N'dev_hanganu') IS NULL
    CREATE USER dev_hanganu FOR LOGIN dev_hanganu WITH DEFAULT_SCHEMA = autogara;
IF USER_ID(N'dev_crivenco') IS NULL
    CREATE USER dev_crivenco FOR LOGIN dev_crivenco WITH DEFAULT_SCHEMA = autogara;
IF USER_ID(N'test_sopivnic') IS NULL
    CREATE USER test_sopivnic FOR LOGIN test_sopivnic WITH DEFAULT_SCHEMA = autogara;
GO

ALTER ROLE db_datareader ADD MEMBER autogara_app;
ALTER ROLE db_datawriter ADD MEMBER autogara_app;
GRANT EXECUTE ON SCHEMA::autogara TO autogara_app;
DENY UPDATE, DELETE ON OBJECT::autogara.LogAudit TO autogara_app;
DENY ALTER ON SCHEMA::autogara TO autogara_app;
DENY CREATE TABLE, CREATE VIEW, CREATE PROCEDURE, CREATE FUNCTION TO autogara_app;

ALTER ROLE db_datareader ADD MEMBER autogara_rapoarte;
DENY INSERT, UPDATE, DELETE ON SCHEMA::autogara TO autogara_rapoarte;
DENY SELECT ON OBJECT::autogara.Utilizatori (ParolaHash) TO autogara_rapoarte;

ALTER ROLE db_datareader ADD MEMBER dev_hanganu;
ALTER ROLE db_datawriter ADD MEMBER dev_hanganu;
GRANT EXECUTE, VIEW DEFINITION ON SCHEMA::autogara TO dev_hanganu;
GRANT SHOWPLAN TO dev_hanganu;

ALTER ROLE db_datareader ADD MEMBER dev_crivenco;
GRANT EXECUTE, VIEW DEFINITION ON SCHEMA::autogara TO dev_crivenco;

ALTER ROLE db_datareader ADD MEMBER test_sopivnic;
ALTER ROLE db_datawriter ADD MEMBER test_sopivnic;
GRANT EXECUTE, VIEW DEFINITION ON SCHEMA::autogara TO test_sopivnic;

DENY UPDATE, DELETE ON OBJECT::autogara.LogAudit TO dev_hanganu, test_sopivnic;
GO

USE msdb;
GO

IF USER_ID(N'test_sopivnic') IS NULL
    CREATE USER test_sopivnic FOR LOGIN test_sopivnic;
ALTER ROLE SQLAgentOperatorRole ADD MEMBER test_sopivnic;
GO

SELECT sp.name AS LoginSql,
       sp.is_disabled AS Dezactivat,
       IS_SRVROLEMEMBER(N'sysadmin', sp.name) AS Sysadmin
FROM sys.server_principals AS sp
WHERE sp.name IN (N'autogara_app', N'autogara_rapoarte', N'dba_gojinevschii',
                  N'dev_hanganu', N'dev_crivenco', N'test_sopivnic')
ORDER BY sp.name;
GO
