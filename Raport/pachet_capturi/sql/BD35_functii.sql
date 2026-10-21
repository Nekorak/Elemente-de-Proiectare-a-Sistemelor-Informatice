-- Cele 6 functii scalare din 05_Functions.sql, apelate cu exemple
USE autogara;
SELECT autogara.fn_AcumLocal()                                   AS fn_AcumLocal,
       SYSUTCDATETIME()                                           AS OraUTC,
       autogara.fn_UtcLaLocal(SYSUTCDATETIME())                   AS fn_UtcLaLocal,
       autogara.fn_LocalLaUtc(autogara.fn_AcumLocal())            AS fn_LocalLaUtc,
       autogara.fn_MomentCursa('2026-10-25', '07:30')             AS fn_MomentCursa;
SELECT r.Denumire AS Reducere, r.ProcentReducere AS Procent, 120.00 AS PretBaza,
       autogara.fn_CalculeazaPret(120.00, r.TipReducereID) AS fn_CalculeazaPret
FROM autogara.TipuriReducere AS r
UNION ALL SELECT N'(fara reducere)', 0, 120.00, autogara.fn_CalculeazaPret(120.00, NULL);
DECLARE @Plecare DATETIME2(0) = '2026-10-25 07:30';
SELECT v.Caz, v.Moment AS MomentAnulare, @Plecare AS Plecare,
       autogara.fn_ProcentRambursare(@Plecare, v.Moment) AS fn_ProcentRambursare
FROM (VALUES (N'cu 2 zile inainte', CAST('2026-10-23 07:30' AS DATETIME2(0))),
             (N'cu 15 ore inainte', CAST('2026-10-24 16:30' AS DATETIME2(0))),
             (N'cu 1 ora inainte',  CAST('2026-10-25 06:30' AS DATETIME2(0)))) AS v (Caz, Moment);
