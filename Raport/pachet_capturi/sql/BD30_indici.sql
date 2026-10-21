-- Indicii nonclustered creati de 04_Indexes.sql (cheie, coloane incluse, filtru)
USE autogara;
SELECT OBJECT_NAME(i.object_id) AS Tabel, i.name AS Index_, CASE WHEN i.is_unique = 1 THEN 'UNIQUE' ELSE '' END AS Unic,
       STUFF((SELECT ', ' + c.name + CASE WHEN ic.is_descending_key = 1 THEN ' DESC' ELSE '' END
              FROM sys.index_columns AS ic JOIN sys.columns AS c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
              WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
              ORDER BY ic.key_ordinal FOR XML PATH('')), 1, 2, '') AS ColoaneCheie,
       ISNULL(STUFF((SELECT ', ' + c.name
              FROM sys.index_columns AS ic JOIN sys.columns AS c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
              WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1
              FOR XML PATH('')), 1, 2, ''), '') AS ColoaneIncluse,
       ISNULL(i.filter_definition, '') AS Filtru
FROM sys.indexes AS i JOIN sys.objects AS o ON o.object_id = i.object_id
WHERE o.schema_id = SCHEMA_ID(N'autogara') AND i.type = 2 AND i.is_primary_key = 0 AND i.is_unique_constraint = 0
ORDER BY Tabel, Index_;
