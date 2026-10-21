-- Joburile SQL Server Agent ale proiectului: pasi, program si ultima rulare
USE msdb;
SELECT j.name AS Job, j.enabled AS Activ, js.step_id AS Pas, js.step_name AS NumePas, js.database_name AS Baza,
       s.name AS Program, h.UltimaRulare, h.Rezultat
FROM dbo.sysjobs AS j
JOIN dbo.sysjobsteps AS js ON js.job_id = j.job_id
LEFT JOIN dbo.sysjobschedules AS jsc ON jsc.job_id = j.job_id
LEFT JOIN dbo.sysschedules AS s ON s.schedule_id = jsc.schedule_id
OUTER APPLY (SELECT TOP (1) msdb.dbo.agent_datetime(jh.run_date, jh.run_time) AS UltimaRulare,
                    CASE jh.run_status WHEN 1 THEN 'Reusit' WHEN 0 THEN 'Esuat' ELSE 'Altul' END AS Rezultat
             FROM dbo.sysjobhistory AS jh WHERE jh.job_id = j.job_id AND jh.step_id = 0
             ORDER BY jh.instance_id DESC) AS h
WHERE j.name LIKE N'Job[_]%'
ORDER BY j.name, js.step_id;
