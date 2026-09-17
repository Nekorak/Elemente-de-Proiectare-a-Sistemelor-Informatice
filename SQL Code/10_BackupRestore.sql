USE master;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

DECLARE @BackupPath NVARCHAR(260) = N'D:\Backup\autogara';

DECLARE @Fisier NVARCHAR(400) = @BackupPath + N'\autogara_FULL_' + FORMAT(SYSDATETIME(), 'yyyyMMdd_HHmm') + N'_manual.bak';
DECLARE @Optiuni NVARCHAR(100) = CASE WHEN SERVERPROPERTY('EngineEdition') = 4 THEN N'' ELSE N', COMPRESSION' END;
DECLARE @Sql NVARCHAR(MAX) =
    N'BACKUP DATABASE autogara TO DISK = @f WITH INIT, CHECKSUM, COPY_ONLY' + @Optiuni
  + N', NAME = N''autogara - backup manual (test restaurare)'';';

EXEC sp_executesql @Sql, N'@f NVARCHAR(400)', @f = @Fisier;

RESTORE VERIFYONLY FROM DISK = @Fisier WITH CHECKSUM;

IF DB_ID(N'autogara_TestRestaurare') IS NOT NULL
BEGIN
    ALTER DATABASE autogara_TestRestaurare SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE autogara_TestRestaurare;
END

DECLARE @ListaFisiere TABLE
(
    LogicalName NVARCHAR(128), PhysicalName NVARCHAR(260), [Type] CHAR(1), FileGroupName NVARCHAR(128),
    Size NUMERIC(20, 0), MaxSize NUMERIC(20, 0), FileID BIGINT, CreateLSN NUMERIC(25, 0), DropLSN NUMERIC(25, 0),
    UniqueID UNIQUEIDENTIFIER, ReadOnlyLSN NUMERIC(25, 0), ReadWriteLSN NUMERIC(25, 0), BackupSizeInBytes BIGINT,
    SourceBlockSize INT, FileGroupID INT, LogGroupGUID UNIQUEIDENTIFIER, DifferentialBaseLSN NUMERIC(25, 0),
    DifferentialBaseGUID UNIQUEIDENTIFIER, IsReadOnly BIT, IsPresent BIT, TDEThumbprint VARBINARY(32),
    SnapshotURL NVARCHAR(360)
);

INSERT INTO @ListaFisiere
EXEC (N'RESTORE FILELISTONLY FROM DISK = N''' + @Fisier + N'''');

DECLARE @Move NVARCHAR(MAX) = N'';
SELECT @Move += CONCAT(N', MOVE N''', LogicalName, N''' TO N''',
                       LEFT(PhysicalName, LEN(PhysicalName) - CHARINDEX(N'\', REVERSE(PhysicalName)) + 1),
                       N'autogara_TestRestaurare_', FileID,
                       CASE [Type] WHEN 'L' THEN N'.ldf' ELSE N'.mdf' END, N'''')
FROM @ListaFisiere;

SET @Sql = N'RESTORE DATABASE autogara_TestRestaurare FROM DISK = @f WITH RECOVERY, CHECKSUM, STATS = 25' + @Move + N';';
EXEC sp_executesql @Sql, N'@f NVARCHAR(400)', @f = @Fisier;
GO

DBCC CHECKDB (N'autogara_TestRestaurare') WITH NO_INFOMSGS;
GO

WITH Original AS
(
    SELECT t.name AS Tabel, SUM(p.rows) AS Randuri
    FROM autogara.sys.tables AS t
    JOIN autogara.sys.partitions AS p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
    GROUP BY t.name
),
Restaurat AS
(
    SELECT t.name AS Tabel, SUM(p.rows) AS Randuri
    FROM autogara_TestRestaurare.sys.tables AS t
    JOIN autogara_TestRestaurare.sys.partitions AS p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
    GROUP BY t.name
)
SELECT
    o.Tabel,
    o.Randuri AS RanduriOriginal,
    r.Randuri AS RanduriRestaurat,
    CASE WHEN o.Randuri = r.Randuri THEN N'OK' ELSE N'DIFERENȚĂ' END AS Rezultat
FROM Original AS o
LEFT JOIN Restaurat AS r ON r.Tabel = o.Tabel
ORDER BY o.Tabel;
GO

ALTER DATABASE autogara_TestRestaurare SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE autogara_TestRestaurare;
GO
