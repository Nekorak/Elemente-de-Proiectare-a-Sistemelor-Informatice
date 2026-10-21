-- Viziunea vw_LocuriDisponibile: locurile libere ale curselor planificate care nu au plecat
USE autogara;
SELECT COUNT(*) AS LocuriDisponibileTotal FROM autogara.vw_LocuriDisponibile;
SELECT TOP (12) CursaID, DataCursa, OraPlecare, Traseu, NumarLoc, Pret, NrInmatriculare, CaleFisierJSON
FROM autogara.vw_LocuriDisponibile ORDER BY DataCursa, OraPlecare, CursaID, NumarLoc;
