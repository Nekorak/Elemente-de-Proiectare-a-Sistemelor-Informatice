-- Viziunea vw_OcupareCurse: gradul de ocupare si incasarile pe fiecare cursa (cursele de ieri)
USE autogara;
SELECT TOP (14) DataCursa, OraPlecare, Traseu, NrInmatriculare, CapacitateLocuri, LocuriOcupate, LocuriLibere,
       GradOcupareProcent, BileteActive, BileteAnulate, Incasari, StatusCursa
FROM autogara.vw_OcupareCurse
WHERE DataCursa = DATEADD(DAY, -1, CAST(autogara.fn_AcumLocal() AS DATE))
ORDER BY GradOcupareProcent DESC;
