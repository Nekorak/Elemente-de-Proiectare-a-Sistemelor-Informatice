-- Viziunea vw_VanzariZilnice: incasarile pe zi si metoda de plata (ultimele 7 zile)
USE autogara;
SELECT Data, MetodaPlata, BileteVandute, Rambursari, SumaIncasata, SumaRambursata, IncasariNete
FROM autogara.vw_VanzariZilnice
WHERE Data >= DATEADD(DAY, -7, CAST(autogara.fn_AcumLocal() AS DATE))
ORDER BY Data DESC, MetodaPlata;
