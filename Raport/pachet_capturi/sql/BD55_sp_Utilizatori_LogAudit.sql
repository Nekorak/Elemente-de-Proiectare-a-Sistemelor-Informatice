-- Demonstratie: sp_AdaugaUtilizator, sp_DezactiveazaUtilizator, sp_InregistreazaLogAudit, intr-o tranzactie ANULATA (ROLLBACK)
USE autogara;
SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRANSACTION;
DECLARE @Admin UNIQUEIDENTIFIER = (SELECT TOP (1) u.UtilizatorID FROM autogara.Utilizatori AS u
                                   JOIN autogara.Roluri AS r ON r.RolID = u.RolID WHERE r.Denumire = N'Admin' AND u.Activ = 1 ORDER BY u.NumeUtilizator);
DECLARE @Nou UNIQUEIDENTIFIER;
DECLARE @Hash VARBINARY(256) = HASHBYTES('SHA2_512', N'demo.casier' + N'Demo2026');
EXEC autogara.sp_AdaugaUtilizator @NumeUtilizator = N'demo.casier', @ParolaHash = @Hash, @Nume = N'Demonstratie',
     @Prenume = N'Casier', @Email = N'demo.casier@autogara.md', @Rol = N'Casier', @CreatDeUtilizatorID = @Admin,
     @UtilizatorID = @Nou OUTPUT;
SELECT N'1. sp_AdaugaUtilizator' AS Pas, u.NumeUtilizator, u.Nume, u.Prenume, r.Denumire AS Rol, u.Activ
FROM autogara.Utilizatori AS u JOIN autogara.Roluri AS r ON r.RolID = u.RolID WHERE u.UtilizatorID = @Nou;
EXEC autogara.sp_DezactiveazaUtilizator @UtilizatorID = @Nou, @DezactivatDeUtilizatorID = @Admin;
SELECT N'2. sp_DezactiveazaUtilizator' AS Pas, NumeUtilizator, Activ FROM autogara.Utilizatori WHERE UtilizatorID = @Nou;
EXEC autogara.sp_InregistreazaLogAudit @UtilizatorID = @Admin, @Actiune = N'Demonstratie pentru raport', @Entitate = N'Raport';
SELECT TOP (3) N'3. LogAudit' AS Pas, Actiune, Entitate, DataOra FROM autogara.LogAudit ORDER BY LogID DESC;
BEGIN TRY
    EXEC autogara.sp_AdaugaUtilizator @NumeUtilizator = N'demo.casier', @ParolaHash = @Hash, @Nume = N'X', @Prenume = N'Y', @Rol = N'Casier';
END TRY
BEGIN CATCH
    SELECT N'4. acelasi nume a doua oara' AS Pas, ERROR_NUMBER() AS Eroare, ERROR_MESSAGE() AS Mesaj;
END CATCH
IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
SELECT N'5. dupa ROLLBACK' AS Pas, COUNT(*) AS ContDemoExista FROM autogara.Utilizatori WHERE NumeUtilizator = N'demo.casier';
