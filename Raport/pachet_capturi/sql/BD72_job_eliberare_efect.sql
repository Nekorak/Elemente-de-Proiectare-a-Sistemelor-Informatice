-- Efectul jobului Job_ElibereazaRezervariExpirate in jurnalul de audit (ultimele inregistrari automate)
USE autogara;
SELECT TOP (10) DataOra, Actiune, Entitate FROM autogara.LogAudit
WHERE Actiune LIKE N'Eliberare automat%' ORDER BY LogID DESC;
SELECT COUNT(*) AS RezervariProvizoriiActive, MIN(DataExpirare) AS PrimaExpirare FROM autogara.RezervariProvizorii;
