-- Permisiunile explicite (GRANT / DENY) acordate in 08_Security.sql
USE autogara;
SELECT pr.name AS Utilizator, pe.state_desc AS Stare, pe.permission_name AS Permisiune, pe.class_desc AS Nivel,
       CASE pe.class WHEN 3 THEN SCHEMA_NAME(pe.major_id)
                     WHEN 1 THEN OBJECT_SCHEMA_NAME(pe.major_id) + '.' + OBJECT_NAME(pe.major_id)
                                 + ISNULL(' (' + COL_NAME(pe.major_id, NULLIF(pe.minor_id, 0)) + ')', '')
                     ELSE '' END AS Obiect
FROM sys.database_permissions AS pe JOIN sys.database_principals AS pr ON pr.principal_id = pe.grantee_principal_id
WHERE pr.name IN (N'autogara_app', N'autogara_rapoarte', N'dev_hanganu', N'dev_crivenco', N'test_sopivnic')
  AND pe.permission_name <> 'CONNECT'
ORDER BY pr.name, pe.state_desc, pe.permission_name;
