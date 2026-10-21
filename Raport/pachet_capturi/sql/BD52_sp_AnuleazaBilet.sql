-- Demonstratie: sp_AnuleazaBilet (rambursare dupa regula 24 h / 2 h), intr-o tranzactie ANULATA la final (ROLLBACK)
USE autogara;
SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRANSACTION;
DECLARE @BiletID UNIQUEIDENTIFIER, @Utilizator UNIQUEIDENTIFIER, @Suma DECIMAL(10,2), @Status NVARCHAR(20);
SELECT TOP (1) @BiletID = b.BiletID, @Utilizator = b.VanzutDeUtilizatorID
FROM autogara.Bilete AS b JOIN autogara.Curse AS c ON c.CursaID = b.CursaID
WHERE b.Status = N'Activ' AND c.Status = N'Planificata'
  AND autogara.fn_MomentCursa(c.DataCursa, c.OraPlecare) > DATEADD(DAY, 2, autogara.fn_AcumLocal())
ORDER BY c.DataCursa, c.OraPlecare;
SELECT N'1. inainte' AS Pas, b.CodBilet, b.Pret, b.Status, c.DataCursa, c.OraPlecare, l.Status AS StatusLoc
FROM autogara.Bilete AS b JOIN autogara.Curse AS c ON c.CursaID = b.CursaID JOIN autogara.Locuri AS l ON l.LocID = b.LocID
WHERE b.BiletID = @BiletID;
EXEC autogara.sp_AnuleazaBilet @BiletID = @BiletID, @UtilizatorID = @Utilizator,
     @SumaRambursata = @Suma OUTPUT, @StatusNou = @Status OUTPUT;
SELECT N'2. dupa sp_AnuleazaBilet' AS Pas, b.CodBilet, b.Status, @Suma AS SumaRambursata, l.Status AS StatusLoc
FROM autogara.Bilete AS b JOIN autogara.Locuri AS l ON l.LocID = b.LocID WHERE b.BiletID = @BiletID;
SELECT N'3. platile biletului' AS Pas, Suma, MetodaPlata, Status, DataPlata FROM autogara.Plati WHERE BiletID = @BiletID ORDER BY PlataID;
BEGIN TRY
    EXEC autogara.sp_AnuleazaBilet @BiletID = @BiletID, @UtilizatorID = @Utilizator;
END TRY
BEGIN CATCH
    SELECT N'4. a doua anulare' AS Pas, ERROR_NUMBER() AS Eroare, ERROR_MESSAGE() AS Mesaj;
END CATCH
IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
SELECT N'5. dupa ROLLBACK' AS Pas, Status FROM autogara.Bilete WHERE BiletID = @BiletID;
