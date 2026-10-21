-- Viziunea vw_RapoarteComparative: comparatia lunara pe trasee (LAG fata de luna precedenta)
USE autogara;
SELECT TOP (14) Traseu, An, Luna, NrCurse, BileteVandute, BileteLunaPrecedenta, Incasari, IncasariLunaPrecedenta,
       VariatieIncasariProcent, GradOcupareMediu, VariatieOcuparePuncte
FROM autogara.vw_RapoarteComparative
WHERE IncasariLunaPrecedenta IS NOT NULL
ORDER BY Incasari DESC;
