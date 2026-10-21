-- Obiectele din schema autogara, grupate pe tip
USE autogara;
SELECT o.type_desc AS TipObiect, COUNT(*) AS Numar
FROM sys.objects AS o
WHERE o.schema_id = SCHEMA_ID(N'autogara') AND o.type IN ('U','V','P','FN','IF','TF')
GROUP BY o.type_desc
UNION ALL
SELECT N'INDEX (fara cheile primare si unice)', COUNT(*)
FROM sys.indexes AS i JOIN sys.objects AS o ON o.object_id = i.object_id
WHERE o.schema_id = SCHEMA_ID(N'autogara') AND i.is_primary_key = 0 AND i.is_unique_constraint = 0 AND i.type > 0
UNION ALL
SELECT N'CONSTRANGERI (PK, FK, UQ, CHECK, DEFAULT)', COUNT(*)
FROM sys.objects AS o WHERE o.schema_id = SCHEMA_ID(N'autogara') AND o.type IN ('PK','F','UQ','C','D');
SELECT t.name AS Tabel, SUM(p.rows) AS Randuri
FROM sys.tables AS t JOIN sys.partitions AS p ON p.object_id = t.object_id AND p.index_id IN (0,1)
WHERE t.schema_id = SCHEMA_ID(N'autogara')
GROUP BY t.name ORDER BY t.name;
