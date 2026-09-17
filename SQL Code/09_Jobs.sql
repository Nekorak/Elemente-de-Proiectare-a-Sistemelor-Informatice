USE msdb;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

DECLARE @BackupPath NVARCHAR(260) = N'D:\Backup\autogara';

IF NOT EXISTS (SELECT 1 FROM msdb.dbo.syscategories WHERE name = N'Autogara' AND category_class = 1)
    EXEC msdb.dbo.sp_add_category @class = N'JOB', @type = N'LOCAL', @name = N'Autogara';

DECLARE @Job SYSNAME;
DECLARE jobs CURSOR LOCAL FAST_FORWARD FOR
    SELECT name FROM msdb.dbo.sysjobs
    WHERE name IN (N'Job_ElibereazaRezervariExpirate', N'Job_BackupComplet', N'Job_BackupDiferential',
                   N'Job_CurataLogAudit', N'Job_VerificaExpirareITP', N'Job_VerificaSpatiuDisc');
OPEN jobs;
FETCH NEXT FROM jobs INTO @Job;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC msdb.dbo.sp_delete_job @job_name = @Job, @delete_unused_schedule = 1;
    FETCH NEXT FROM jobs INTO @Job;
END
CLOSE jobs;
DEALLOCATE jobs;

DECLARE @Comanda NVARCHAR(MAX);

EXEC msdb.dbo.sp_add_job
     @job_name = N'Job_ElibereazaRezervariExpirate',
     @description = N'Eliberează locurile cu rezervare provizorie expirată.',
     @category_name = N'Autogara',
     @owner_login_name = N'sa';

EXEC msdb.dbo.sp_add_jobstep
     @job_name = N'Job_ElibereazaRezervariExpirate',
     @step_name = N'Eliberare rezervari',
     @subsystem = N'TSQL',
     @database_name = N'autogara',
     @command = N'EXEC autogara.sp_ElibereazaRezervariExpirate;',
     @retry_attempts = 2,
     @retry_interval = 1;

EXEC msdb.dbo.sp_add_jobschedule
     @job_name = N'Job_ElibereazaRezervariExpirate',
     @name = N'Autogara - la fiecare minut',
     @freq_type = 4, @freq_interval = 1,
     @freq_subday_type = 4, @freq_subday_interval = 1,
     @active_start_time = 000000, @active_end_time = 235959;

EXEC msdb.dbo.sp_add_jobserver @job_name = N'Job_ElibereazaRezervariExpirate', @server_name = N'(local)';

EXEC msdb.dbo.sp_add_job
     @job_name = N'Job_BackupComplet',
     @description = N'Backup complet zilnic al bazei autogara, cu verificare și ștergerea fișierelor mai vechi de 30 de zile.',
     @category_name = N'Autogara',
     @owner_login_name = N'sa';

SET @Comanda = REPLACE(N'
DECLARE @Fisier NVARCHAR(400) = N''{BackupPath}\autogara_FULL_'' + FORMAT(SYSDATETIME(), ''yyyyMMdd_HHmm'') + N''.bak'';
DECLARE @Optiuni NVARCHAR(100) = CASE WHEN SERVERPROPERTY(''EngineEdition'') = 4 THEN N'''' ELSE N'', COMPRESSION'' END;
DECLARE @Sql NVARCHAR(MAX) = N''BACKUP DATABASE autogara TO DISK = @f WITH INIT, CHECKSUM'' + @Optiuni
                           + N'', NAME = N''''autogara - backup complet'''';'';
EXEC sp_executesql @Sql, N''@f NVARCHAR(400)'', @f = @Fisier;
RESTORE VERIFYONLY FROM DISK = @Fisier WITH CHECKSUM;', N'{BackupPath}', @BackupPath);

EXEC msdb.dbo.sp_add_jobstep
     @job_name = N'Job_BackupComplet',
     @step_name = N'Backup complet',
     @subsystem = N'TSQL',
     @database_name = N'master',
     @on_success_action = 3,
     @command = @Comanda;

SET @Comanda = REPLACE(N'
DECLARE @Limita DATETIME = DATEADD(DAY, -30, GETDATE());
EXECUTE master.dbo.xp_delete_file 0, N''{BackupPath}'', N''bak'', @Limita, 0;
EXECUTE master.dbo.xp_delete_file 0, N''{BackupPath}'', N''dif'', @Limita, 0;', N'{BackupPath}', @BackupPath);

EXEC msdb.dbo.sp_add_jobstep
     @job_name = N'Job_BackupComplet',
     @step_name = N'Stergere backup-uri mai vechi de 30 zile',
     @subsystem = N'TSQL',
     @database_name = N'master',
     @command = @Comanda;

EXEC msdb.dbo.sp_add_jobschedule
     @job_name = N'Job_BackupComplet',
     @name = N'Autogara - zilnic 02:00',
     @freq_type = 4, @freq_interval = 1,
     @active_start_time = 020000;

EXEC msdb.dbo.sp_add_jobserver @job_name = N'Job_BackupComplet', @server_name = N'(local)';

EXEC msdb.dbo.sp_add_job
     @job_name = N'Job_BackupDiferential',
     @description = N'Backup diferențial al bazei autogara în timpul programului autogării.',
     @category_name = N'Autogara',
     @owner_login_name = N'sa';

SET @Comanda = REPLACE(N'
DECLARE @Fisier NVARCHAR(400) = N''{BackupPath}\autogara_DIFF_'' + FORMAT(SYSDATETIME(), ''yyyyMMdd_HHmm'') + N''.dif'';
DECLARE @Optiuni NVARCHAR(100) = CASE WHEN SERVERPROPERTY(''EngineEdition'') = 4 THEN N'''' ELSE N'', COMPRESSION'' END;
DECLARE @Sql NVARCHAR(MAX) = N''BACKUP DATABASE autogara TO DISK = @f WITH DIFFERENTIAL, INIT, CHECKSUM'' + @Optiuni
                           + N'', NAME = N''''autogara - backup diferential'''';'';
EXEC sp_executesql @Sql, N''@f NVARCHAR(400)'', @f = @Fisier;
RESTORE VERIFYONLY FROM DISK = @Fisier WITH CHECKSUM;', N'{BackupPath}', @BackupPath);

EXEC msdb.dbo.sp_add_jobstep
     @job_name = N'Job_BackupDiferential',
     @step_name = N'Backup diferential',
     @subsystem = N'TSQL',
     @database_name = N'master',
     @command = @Comanda;

EXEC msdb.dbo.sp_add_jobschedule
     @job_name = N'Job_BackupDiferential',
     @name = N'Autogara - la 4 ore (06-22)',
     @freq_type = 4, @freq_interval = 1,
     @freq_subday_type = 8, @freq_subday_interval = 4,
     @active_start_time = 060000, @active_end_time = 220000;

EXEC msdb.dbo.sp_add_jobserver @job_name = N'Job_BackupDiferential', @server_name = N'(local)';

EXEC msdb.dbo.sp_add_job
     @job_name = N'Job_CurataLogAudit',
     @description = N'Șterge înregistrările din LogAudit mai vechi de 12 luni.',
     @category_name = N'Autogara',
     @owner_login_name = N'sa';

EXEC msdb.dbo.sp_add_jobstep
     @job_name = N'Job_CurataLogAudit',
     @step_name = N'Stergere log vechi',
     @subsystem = N'TSQL',
     @database_name = N'autogara',
     @command = N'
DECLARE @Limita DATETIME2(3) = DATEADD(MONTH, -12, SYSUTCDATETIME());
DECLARE @Sterse INT = 1, @Total INT = 0;
WHILE @Sterse > 0
BEGIN
    DELETE TOP (5000) FROM autogara.LogAudit WHERE DataOra < @Limita;
    SET @Sterse = @@ROWCOUNT;
    SET @Total += @Sterse;
END
DECLARE @Mesaj NVARCHAR(200) = CONCAT(N''Curățare LogAudit: '', @Total, N'' înregistrări mai vechi de 12 luni șterse'');
EXEC autogara.sp_InregistreazaLogAudit NULL, @Mesaj, N''LogAudit'';';

EXEC msdb.dbo.sp_add_jobschedule
     @job_name = N'Job_CurataLogAudit',
     @name = N'Autogara - lunar ziua 1, 03:30',
     @freq_type = 16, @freq_interval = 1, @freq_recurrence_factor = 1,
     @active_start_time = 033000;

EXEC msdb.dbo.sp_add_jobserver @job_name = N'Job_CurataLogAudit', @server_name = N'(local)';

EXEC msdb.dbo.sp_add_job
     @job_name = N'Job_VerificaExpirareITP',
     @description = N'Alertă pentru autobuzele cu ITP expirat sau care expiră în 14 zile.',
     @category_name = N'Autogara',
     @owner_login_name = N'sa';

EXEC msdb.dbo.sp_add_jobstep
     @job_name = N'Job_VerificaExpirareITP',
     @step_name = N'Verificare ITP',
     @subsystem = N'TSQL',
     @database_name = N'autogara',
     @command = N'
DECLARE @Azi DATE = CAST(autogara.fn_AcumLocal() AS DATE);

UPDATE autogara.Autobuze
SET Status = N''Service'', ModificatLa = SYSUTCDATETIME()
WHERE Activ = 1 AND Status = N''Activ'' AND DataExpirareITP < @Azi;

INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID)
SELECT NULL,
       CASE WHEN a.DataExpirareITP < @Azi
            THEN CONCAT(N''ALERTĂ ITP expirat: '', a.NrInmatriculare, N'' ('', a.Model, N''), expirat la '', CONVERT(CHAR(10), a.DataExpirareITP, 104))
            ELSE CONCAT(N''ALERTĂ ITP: '', a.NrInmatriculare, N'' ('', a.Model, N'') expiră la '', CONVERT(CHAR(10), a.DataExpirareITP, 104),
                        N'' — '', DATEDIFF(DAY, @Azi, a.DataExpirareITP), N'' zile rămase'')
       END,
       N''Autobuze'',
       CAST(a.AutobuzID AS NVARCHAR(50))
FROM autogara.Autobuze AS a
WHERE a.Activ = 1
  AND a.DataExpirareITP <= DATEADD(DAY, 14, @Azi);

DECLARE @Nr INT = @@ROWCOUNT;
IF @Nr > 0
    RAISERROR (N''Autogara: %d autobuze cu ITP expirat sau care expira in 14 zile.'', 10, 1, @Nr) WITH LOG;';

EXEC msdb.dbo.sp_add_jobschedule
     @job_name = N'Job_VerificaExpirareITP',
     @name = N'Autogara - zilnic 07:00',
     @freq_type = 4, @freq_interval = 1,
     @active_start_time = 070000;

EXEC msdb.dbo.sp_add_jobserver @job_name = N'Job_VerificaExpirareITP', @server_name = N'(local)';

EXEC msdb.dbo.sp_add_job
     @job_name = N'Job_VerificaSpatiuDisc',
     @description = N'Verifică spațiul liber pe discurile cu fișierele bazei de date autogara.',
     @category_name = N'Autogara',
     @owner_login_name = N'sa';

EXEC msdb.dbo.sp_add_jobstep
     @job_name = N'Job_VerificaSpatiuDisc',
     @step_name = N'Verificare spatiu',
     @subsystem = N'TSQL',
     @database_name = N'master',
     @command = N'
DECLARE @Probleme NVARCHAR(1000);

SELECT @Probleme = STRING_AGG(CONCAT(v.volume_mount_point, N'' '',
                                     CAST(v.available_bytes / 1073741824.0 AS DECIMAL(10, 1)), N'' GB liberi ('',
                                     CAST(100.0 * v.available_bytes / v.total_bytes AS DECIMAL(5, 1)), N''%)''), N''; '')
FROM (
    SELECT DISTINCT vs.volume_mount_point, vs.available_bytes, vs.total_bytes
    FROM sys.master_files AS mf
    CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.file_id) AS vs
    WHERE mf.database_id = DB_ID(N''autogara'')
) AS v
WHERE v.available_bytes < 5368709120
   OR 100.0 * v.available_bytes / v.total_bytes < 10;

IF @Probleme IS NOT NULL
BEGIN
    INSERT INTO autogara.autogara.LogAudit (UtilizatorID, Actiune, Entitate)
    VALUES (NULL, LEFT(CONCAT(N''ALERTĂ spațiu disc: '', @Probleme), 200), N''Server'');
    RAISERROR (N''Autogara: spatiu disc insuficient: %s'', 16, 1, @Probleme) WITH LOG;
END';

EXEC msdb.dbo.sp_add_jobschedule
     @job_name = N'Job_VerificaSpatiuDisc',
     @name = N'Autogara - la 6 ore',
     @freq_type = 4, @freq_interval = 1,
     @freq_subday_type = 8, @freq_subday_interval = 6,
     @active_start_time = 000000, @active_end_time = 235959;

EXEC msdb.dbo.sp_add_jobserver @job_name = N'Job_VerificaSpatiuDisc', @server_name = N'(local)';
GO

SELECT j.name AS Job, j.enabled AS Activ, s.name AS Program
FROM msdb.dbo.sysjobs AS j
JOIN msdb.dbo.sysjobschedules AS js ON js.job_id = j.job_id
JOIN msdb.dbo.sysschedules    AS s  ON s.schedule_id = js.schedule_id
WHERE j.name LIKE N'Job[_]%'
ORDER BY j.name;
GO
