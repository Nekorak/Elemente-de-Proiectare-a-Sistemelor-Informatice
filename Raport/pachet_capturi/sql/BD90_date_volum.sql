-- Volumul datelor dupa 11_Seed.sql si 12_DateVolum.sql
USE autogara;
SELECT (SELECT COUNT(*) FROM autogara.Utilizatori) AS Utilizatori, (SELECT COUNT(*) FROM autogara.Curse) AS Curse,
       (SELECT COUNT(*) FROM autogara.Locuri) AS Locuri, (SELECT COUNT(*) FROM autogara.Bilete) AS Bilete,
       (SELECT COUNT(*) FROM autogara.Plati) AS Plati, (SELECT COUNT(*) FROM autogara.LogAudit) AS LogAudit;
SELECT r.Denumire AS Rol, COUNT(*) AS Conturi, SUM(CASE WHEN u.Activ = 1 THEN 1 ELSE 0 END) AS Active
FROM autogara.Utilizatori AS u JOIN autogara.Roluri AS r ON r.RolID = u.RolID GROUP BY r.Denumire;
SELECT MIN(DataCursa) AS PrimaCursa, MAX(DataCursa) AS UltimaCursa, COUNT(DISTINCT TraseuID) AS Trasee FROM autogara.Curse;
SELECT Status, COUNT(*) AS Bilete, SUM(Pret) AS Valoare FROM autogara.Bilete GROUP BY Status;
