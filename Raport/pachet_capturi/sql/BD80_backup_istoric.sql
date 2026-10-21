-- Istoricul backup-urilor bazei autogara (msdb.dbo.backupset) si modelul de recuperare
USE msdb;
SELECT TOP (10) bs.database_name AS Baza, CASE bs.type WHEN 'D' THEN 'Complet' WHEN 'I' THEN 'Diferential' WHEN 'L' THEN 'Log' END AS Tip,
       bs.backup_start_date AS Start, bs.backup_finish_date AS Sfarsit, CAST(bs.backup_size / 1048576.0 AS DECIMAL(10,1)) AS MarimeMB,
       bs.is_copy_only AS CopyOnly, bmf.physical_device_name AS Fisier
FROM dbo.backupset AS bs JOIN dbo.backupmediafamily AS bmf ON bmf.media_set_id = bs.media_set_id
WHERE bs.database_name IN (N'autogara') ORDER BY bs.backup_finish_date DESC;
SELECT name AS Baza, recovery_model_desc AS ModelRecuperare FROM sys.databases WHERE name = N'autogara';
