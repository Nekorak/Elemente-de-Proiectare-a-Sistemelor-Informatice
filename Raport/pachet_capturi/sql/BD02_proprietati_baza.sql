-- Proprietatile bazei de date autogara si schema dedicata
USE autogara;
SELECT name AS BazaDeDate, collation_name AS Collation, recovery_model_desc AS ModelRecuperare,
       is_read_committed_snapshot_on AS ReadCommittedSnapshot, page_verify_option_desc AS VerificarePagini,
       is_auto_update_stats_on AS AutoUpdateStatistici, compatibility_level AS NivelCompatibilitate
FROM sys.databases WHERE name = N'autogara';
SELECT s.name AS Schema_, USER_NAME(s.principal_id) AS Proprietar FROM sys.schemas AS s WHERE s.name = N'autogara';
SELECT f.name AS FisierLogic, f.type_desc AS Tip, f.physical_name AS CaleFizica, CAST(f.size * 8 / 1024.0 AS DECIMAL(10,1)) AS MarimeMB
FROM sys.database_files AS f;
