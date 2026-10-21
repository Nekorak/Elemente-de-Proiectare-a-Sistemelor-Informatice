-- Demonstratie: sp_RezervaLoc -> sp_ConfirmaVanzareBilet, intr-o tranzactie ANULATA la final (ROLLBACK), datele reale nu se modifica
USE autogara;
SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRANSACTION;
DECLARE @LocID INT = (SELECT TOP (1) LocID FROM autogara.vw_LocuriDisponibile
                      WHERE DataCursa > CAST(autogara.fn_AcumLocal() AS DATE) ORDER BY DataCursa, OraPlecare, NumarLoc);
DECLARE @Casier UNIQUEIDENTIFIER = (SELECT TOP (1) u.UtilizatorID FROM autogara.Utilizatori AS u
                                    JOIN autogara.Roluri AS r ON r.RolID = u.RolID WHERE r.Denumire = N'Casier' AND u.Activ = 1 ORDER BY u.NumeUtilizator);
DECLARE @RezervareID UNIQUEIDENTIFIER, @Expira DATETIME2(3), @BiletID UNIQUEIDENTIFIER, @Cod NVARCHAR(20), @Pret DECIMAL(10,2);

EXEC autogara.sp_RezervaLoc @LocID = @LocID, @UtilizatorID = @Casier, @DurataMinute = 10,
     @RezervareID = @RezervareID OUTPUT, @DataExpirare = @Expira OUTPUT;
SELECT N'1. dupa sp_RezervaLoc' AS Pas, l.LocID, l.NumarLoc, l.Status AS StatusLoc, @RezervareID AS RezervareID, @Expira AS ExpiraUTC
FROM autogara.Locuri AS l WHERE l.LocID = @LocID;

EXEC autogara.sp_ConfirmaVanzareBilet @RezervareID = @RezervareID, @NumePasager = N'Pasager Demonstratie',
     @TelefonPasager = N'+37369000000', @TipReducereID = 2, @VanzutDeUtilizatorID = @Casier, @MetodaPlata = N'Numerar',
     @BiletID = @BiletID OUTPUT, @CodBilet = @Cod OUTPUT, @PretFinal = @Pret OUTPUT;
SELECT N'2. dupa sp_ConfirmaVanzareBilet' AS Pas, b.CodBilet, b.NumePasager, b.Pret, b.Status AS StatusBilet, l.Status AS StatusLoc,
       p.Suma AS Plata, p.MetodaPlata, p.Status AS StatusPlata
FROM autogara.Bilete AS b JOIN autogara.Locuri AS l ON l.LocID = b.LocID JOIN autogara.Plati AS p ON p.BiletID = b.BiletID
WHERE b.BiletID = @BiletID;
SELECT TOP (1) N'3. jurnalul de audit' AS Pas, Actiune, Entitate, DataOra FROM autogara.LogAudit ORDER BY LogID DESC;

ROLLBACK TRANSACTION;
SELECT N'4. dupa ROLLBACK' AS Pas, COUNT(*) AS BileteCuCodulDemonstratiei FROM autogara.Bilete WHERE CodBilet = @Cod;
