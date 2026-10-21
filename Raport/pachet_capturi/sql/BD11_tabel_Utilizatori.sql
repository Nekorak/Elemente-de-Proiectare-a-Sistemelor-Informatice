-- Tabelul autogara.Utilizatori: structura, constrangerile si un esantion de date
USE autogara;
SELECT c.column_id AS Nr, c.name AS Coloana,
       TYPE_NAME(c.user_type_id) + CASE WHEN TYPE_NAME(c.user_type_id) IN ('nvarchar','varchar','varbinary')
            THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(CASE WHEN TYPE_NAME(c.user_type_id) = 'nvarchar' THEN c.max_length/2 ELSE c.max_length END AS VARCHAR(5)) END + ')'
            WHEN TYPE_NAME(c.user_type_id) = 'decimal' THEN '(' + CAST(c.precision AS VARCHAR(3)) + ',' + CAST(c.scale AS VARCHAR(3)) + ')' ELSE '' END AS Tip,
       CASE WHEN c.is_nullable = 1 THEN 'NULL' ELSE 'NOT NULL' END AS Nul,
       CASE WHEN c.is_identity = 1 THEN 'IDENTITY' ELSE '' END AS Identitate
FROM sys.columns AS c WHERE c.object_id = OBJECT_ID(N'autogara.Utilizatori') ORDER BY c.column_id;
SELECT o.name AS Constrangere, o.type_desc AS Tip,
       COALESCE(cc.definition, dc.definition, OBJECT_SCHEMA_NAME(fk.referenced_object_id) + '.' + OBJECT_NAME(fk.referenced_object_id), '') AS Definitie_sau_Referinta
FROM sys.objects AS o
LEFT JOIN sys.check_constraints AS cc ON cc.object_id = o.object_id
LEFT JOIN sys.default_constraints AS dc ON dc.object_id = o.object_id
LEFT JOIN sys.foreign_keys AS fk ON fk.object_id = o.object_id
WHERE o.parent_object_id = OBJECT_ID(N'autogara.Utilizatori') AND o.type IN ('PK','F','UQ','C','D')
ORDER BY CASE o.type WHEN 'PK' THEN 1 WHEN 'F' THEN 2 WHEN 'UQ' THEN 3 WHEN 'C' THEN 4 ELSE 5 END, o.name;
SELECT COUNT(*) AS TotalRanduri FROM autogara.Utilizatori;
SELECT TOP (8) NumeUtilizator, Nume, Prenume, Email, Telefon, RolID, Activ, CONVERT(VARCHAR(18), ParolaHash, 1) + '...' AS ParolaHash_Inceput, CreatLa FROM autogara.Utilizatori ORDER BY CreatLa DESC;
