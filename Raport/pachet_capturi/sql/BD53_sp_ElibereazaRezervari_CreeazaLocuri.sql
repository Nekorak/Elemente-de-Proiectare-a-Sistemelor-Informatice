-- Demonstratie: sp_CreeazaLocuriPentruCursa si sp_ElibereazaRezervariExpirate, intr-o tranzactie ANULATA la final (ROLLBACK)
USE autogara;
SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRANSACTION;
DECLARE @Model TABLE (TraseuID INT, AutobuzID INT, SoferID INT, Pret DECIMAL(10,2));
INSERT INTO @Model SELECT TOP (1) TraseuID, AutobuzID, SoferID, Pret FROM autogara.Curse ORDER BY CursaID DESC;
INSERT INTO autogara.Curse (TraseuID, AutobuzID, SoferID, DataCursa, OraPlecare, OraSosireEstimata, Pret)
SELECT TraseuID, AutobuzID, SoferID, DATEADD(DAY, 30, CAST(autogara.fn_AcumLocal() AS DATE)), '23:00', '23:50', Pret FROM @Model;
DECLARE @CursaID INT = SCOPE_IDENTITY(), @Create INT, @Eliberate INT;
EXEC autogara.sp_CreeazaLocuriPentruCursa @CursaID = @CursaID, @LocuriCreate = @Create OUTPUT;
SELECT N'1. sp_CreeazaLocuriPentruCursa' AS Pas, @CursaID AS CursaNoua, @Create AS LocuriCreate,
       (SELECT CapacitateLocuri FROM autogara.Autobuze WHERE AutobuzID = (SELECT AutobuzID FROM @Model)) AS CapacitateAutobuz;
-- o rezervare deja expirata pe locul 1
DECLARE @Loc INT = (SELECT LocID FROM autogara.Locuri WHERE CursaID = @CursaID AND NumarLoc = 1);
UPDATE autogara.Locuri SET Status = N'Rezervat' WHERE LocID = @Loc;
INSERT INTO autogara.RezervariProvizorii (LocID, DataCreare, DataExpirare)
VALUES (@Loc, DATEADD(MINUTE, -15, SYSUTCDATETIME()), DATEADD(MINUTE, -5, SYSUTCDATETIME()));
SELECT N'2. rezervare expirata' AS Pas, NumarLoc, Status FROM autogara.Locuri WHERE LocID = @Loc;
EXEC autogara.sp_ElibereazaRezervariExpirate @NrEliberate = @Eliberate OUTPUT;
SELECT N'3. sp_ElibereazaRezervariExpirate' AS Pas, @Eliberate AS LocuriEliberate,
       (SELECT Status FROM autogara.Locuri WHERE LocID = @Loc) AS StatusLoc1;
ROLLBACK TRANSACTION;
SELECT N'4. dupa ROLLBACK' AS Pas, COUNT(*) AS CursaDemonstratieExista FROM autogara.Curse WHERE CursaID = @CursaID;
