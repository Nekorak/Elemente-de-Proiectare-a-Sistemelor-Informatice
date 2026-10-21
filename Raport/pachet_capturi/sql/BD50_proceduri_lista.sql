-- Procedurile stocate din schema autogara, cu parametrii lor
USE autogara;
SELECT p.name AS Procedura,
       ISNULL(STUFF((SELECT ', ' + pr.name + CASE WHEN pr.is_output = 1 THEN ' OUTPUT' ELSE '' END
              FROM sys.parameters AS pr WHERE pr.object_id = p.object_id ORDER BY pr.parameter_id FOR XML PATH('')), 1, 2, ''), '') AS Parametri,
       p.create_date AS Creata, p.modify_date AS Modificata
FROM sys.procedures AS p WHERE p.schema_id = SCHEMA_ID(N'autogara') ORDER BY p.name;
