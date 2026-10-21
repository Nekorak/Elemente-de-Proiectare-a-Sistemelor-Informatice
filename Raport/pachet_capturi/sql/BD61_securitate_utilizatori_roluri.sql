-- Utilizatorii bazei autogara si rolurile de baza de date in care sunt membri
USE autogara;
SELECT dp.name AS UtilizatorBD, dp.default_schema_name AS SchemaImplicita,
       ISNULL(STUFF((SELECT ', ' + r.name FROM sys.database_role_members AS rm
                     JOIN sys.database_principals AS r ON r.principal_id = rm.role_principal_id
                     WHERE rm.member_principal_id = dp.principal_id FOR XML PATH('')), 1, 2, ''), '') AS RoluriBD
FROM sys.database_principals AS dp
WHERE dp.type = 'S' AND dp.name IN (N'autogara_app', N'autogara_rapoarte', N'dev_hanganu', N'dev_crivenco', N'test_sopivnic')
ORDER BY dp.name;
