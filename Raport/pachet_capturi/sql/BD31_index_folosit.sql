-- Indexul IX_Curse_DataCursa_TraseuID folosit la cautarea curselor pe o zi (planul de executie: Include Actual Execution Plan, Ctrl+M)
USE autogara;
SET STATISTICS IO ON;
DECLARE @Azi DATE = CAST(autogara.fn_AcumLocal() AS DATE);
SELECT CursaID, TraseuID, OraPlecare, OraSosireEstimata, Pret, Status
FROM autogara.Curse WHERE DataCursa = @Azi ORDER BY OraPlecare;
