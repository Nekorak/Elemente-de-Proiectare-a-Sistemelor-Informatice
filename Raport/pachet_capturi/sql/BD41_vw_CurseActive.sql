-- Viziunea vw_CurseActive: cursele de azi si viitoare, cu statia de plecare/sosire, autobuzul, soferul si locurile
USE autogara;
SELECT TOP (14) DataCursa, OraPlecare, StatiePlecare, PeronPlecare, StatieSosire, NrInmatriculare, Sofer,
       Pret, LocuriTotal, LocuriLibere, LocuriRezervate, LocuriOcupate, Status
FROM autogara.vw_CurseActive ORDER BY DataCursa, OraPlecare;
