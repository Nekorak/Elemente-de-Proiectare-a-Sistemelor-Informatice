-- Istoricul rularilor joburilor (rezumat) si ultimele mesaje
USE msdb;
SELECT j.name AS Job,
       SUM(CASE WHEN jh.run_status = 1 THEN 1 ELSE 0 END) AS Reusite,
       SUM(CASE WHEN jh.run_status = 0 THEN 1 ELSE 0 END) AS Esuate,
       MAX(msdb.dbo.agent_datetime(jh.run_date, jh.run_time)) AS UltimaRulare
FROM dbo.sysjobs AS j LEFT JOIN dbo.sysjobhistory AS jh ON jh.job_id = j.job_id AND jh.step_id = 0
WHERE j.name LIKE N'Job[_]%' GROUP BY j.name ORDER BY j.name;
SELECT TOP (8) j.name AS Job, msdb.dbo.agent_datetime(jh.run_date, jh.run_time) AS Rulare,
       CASE jh.run_status WHEN 1 THEN 'Reusit' ELSE 'Esuat' END AS Rezultat, LEFT(jh.message, 160) AS Mesaj
FROM dbo.sysjobhistory AS jh JOIN dbo.sysjobs AS j ON j.job_id = jh.job_id
WHERE j.name LIKE N'Job[_]%' AND jh.step_id = 0
ORDER BY jh.instance_id DESC;
