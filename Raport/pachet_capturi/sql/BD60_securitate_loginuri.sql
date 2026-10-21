-- Login-urile SQL ale proiectului si rolurile de server
USE master;
SELECT sp.name AS LoginSql, sp.type_desc AS Tip, sp.default_database_name AS BazaImplicita, sp.is_disabled AS Dezactivat,
       IS_SRVROLEMEMBER(N'sysadmin', sp.name) AS Sysadmin, sl.is_policy_checked AS PoliticaParole, sp.create_date AS Creat
FROM sys.server_principals AS sp LEFT JOIN sys.sql_logins AS sl ON sl.principal_id = sp.principal_id
WHERE sp.name IN (N'autogara_app', N'autogara_rapoarte', N'dba_gojinevschii', N'dev_hanganu', N'dev_crivenco', N'test_sopivnic')
ORDER BY sp.name;
SELECT SERVERPROPERTY('IsIntegratedSecurityOnly') AS DoarWindowsAuth, SERVERPROPERTY('InstanceName') AS Instanta,
       (SELECT DISTINCT local_tcp_port FROM sys.dm_exec_connections WHERE session_id = @@SPID) AS PortTcpSesiune;
