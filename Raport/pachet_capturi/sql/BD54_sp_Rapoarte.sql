-- Procedurile de raportare: sp_RaportVanzariZilnic (ieri) si sp_RaportOcupareCurse (ultimele 3 zile)
USE autogara;
DECLARE @Ieri DATE = DATEADD(DAY, -1, CAST(autogara.fn_AcumLocal() AS DATE));
DECLARE @Start DATE = DATEADD(DAY, -3, CAST(autogara.fn_AcumLocal() AS DATE));
EXEC autogara.sp_RaportVanzariZilnic @Data = @Ieri;
EXEC autogara.sp_RaportOcupareCurse @DataStart = @Start, @DataStop = @Ieri;
