/* =====================================================================
   07_StoredProcedures.sql
   Proceduri stocate pentru operațiile critice (rezervare, vânzare,
   anulare), rapoarte, utilizatori și audit.

   Coduri de eroare (THROW):
     5001x  curse / locuri          5002x  rezervări
     5003x  vânzare bilet           5004x  anulare bilet
     5005x  rapoarte                5006x  utilizatori
   Toate operațiile critice rulează într-o singură tranzacție.
   ===================================================================== */
USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------------------------------------------------------------------
   sp_InregistreazaLogAudit
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_InregistreazaLogAudit
    @UtilizatorID UNIQUEIDENTIFIER = NULL,
    @Actiune      NVARCHAR(200),
    @Entitate     NVARCHAR(100)    = NULL,
    @EntitateID   NVARCHAR(50)     = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO autogara.LogAudit (UtilizatorID, Actiune, Entitate, EntitateID)
    VALUES (@UtilizatorID, @Actiune, @Entitate, @EntitateID);
END;
GO

/* ---------------------------------------------------------------------
   sp_CreeazaLocuriPentruCursa — generează locurile 1..CapacitateLocuri
   pentru o cursă (idempotent: nu dublează locurile existente).
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_CreeazaLocuriPentruCursa
    @CursaID      INT,
    @LocuriCreate INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Capacitate SMALLINT;

    SELECT @Capacitate = a.CapacitateLocuri
    FROM autogara.Curse    AS c
    JOIN autogara.Autobuze AS a ON a.AutobuzID = c.AutobuzID
    WHERE c.CursaID = @CursaID;

    IF @Capacitate IS NULL
        THROW 50010, N'Cursa specificată nu există.', 1;

    WITH Cifre AS (SELECT n FROM (VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) AS v (n)),
         Numere AS (SELECT u.n + 10 * z.n + 100 * s.n + 1 AS NumarLoc
                    FROM Cifre AS u CROSS JOIN Cifre AS z CROSS JOIN Cifre AS s)
    INSERT INTO autogara.Locuri (CursaID, NumarLoc, Status)
    SELECT @CursaID, nr.NumarLoc, N'Liber'
    FROM Numere AS nr
    WHERE nr.NumarLoc <= @Capacitate
      AND NOT EXISTS (SELECT 1 FROM autogara.Locuri AS l
                      WHERE l.CursaID = @CursaID AND l.NumarLoc = nr.NumarLoc);

    SET @LocuriCreate = @@ROWCOUNT;
END;
GO

/* ---------------------------------------------------------------------
   sp_ElibereazaRezervariExpirate — rulat de Job_ElibereazaRezervariExpirate.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_ElibereazaRezervariExpirate
    @NrEliberate INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Eliberate TABLE (LocID INT PRIMARY KEY);

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE r
        OUTPUT deleted.LocID INTO @Eliberate (LocID)
        FROM autogara.RezervariProvizorii AS r WITH (READPAST)
        WHERE r.DataExpirare <= SYSUTCDATETIME();

        UPDATE l
        SET l.Status = N'Liber'
        FROM autogara.Locuri AS l
        JOIN @Eliberate AS e ON e.LocID = l.LocID
        WHERE l.Status = N'Rezervat';

        SET @NrEliberate = @@ROWCOUNT;

        -- plasă de siguranță: locuri rămase 'Rezervat' fără rezervare
        UPDATE l
        SET l.Status = N'Liber'
        FROM autogara.Locuri AS l
        WHERE l.Status = N'Rezervat'
          AND NOT EXISTS (SELECT 1 FROM autogara.RezervariProvizorii AS r WHERE r.LocID = l.LocID);

        SET @NrEliberate += @@ROWCOUNT;

        IF @NrEliberate > 0
        BEGIN
            DECLARE @Mesaj NVARCHAR(200) = CONCAT(N'Eliberare automată rezervări expirate: ', @NrEliberate, N' locuri');
            EXEC autogara.sp_InregistreazaLogAudit
                 @UtilizatorID = NULL, @Actiune = @Mesaj, @Entitate = N'RezervariProvizorii';
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

/* ---------------------------------------------------------------------
   sp_RezervaLoc — blochează un loc liber pentru @DurataMinute minute.
   Doi casieri pe același loc: al doilea primește eroarea 50023.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_RezervaLoc
    @LocID        INT,
    @UtilizatorID UNIQUEIDENTIFIER = NULL,
    @DurataMinute INT              = 10,
    @RezervareID  UNIQUEIDENTIFIER = NULL OUTPUT,
    @DataExpirare DATETIME2(3)     = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @DurataMinute NOT BETWEEN 1 AND 60
        THROW 50024, N'Durata rezervării provizorii trebuie să fie între 1 și 60 de minute.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @StatusLoc NVARCHAR(20), @StatusCursa NVARCHAR(20), @Plecare DATETIME2(0);

        SELECT @StatusLoc   = l.Status,
               @StatusCursa = c.Status,
               @Plecare     = autogara.fn_MomentCursa(c.DataCursa, c.OraPlecare)
        FROM autogara.Locuri AS l WITH (UPDLOCK, HOLDLOCK)
        JOIN autogara.Curse  AS c ON c.CursaID = l.CursaID
        WHERE l.LocID = @LocID;

        IF @StatusLoc IS NULL
            THROW 50020, N'Locul specificat nu există.', 1;

        IF @StatusCursa <> N'Planificata'
            THROW 50021, N'Nu se pot face rezervări: cursa nu este planificată (a plecat, s-a finalizat sau a fost anulată).', 1;

        IF @Plecare <= autogara.fn_AcumLocal()
            THROW 50022, N'Nu se pot face rezervări: ora de plecare a cursei a trecut.', 1;

        -- o rezervare expirată, încă neeliberată de job, nu blochează locul
        IF @StatusLoc = N'Rezervat'
           AND EXISTS (SELECT 1 FROM autogara.RezervariProvizorii WITH (UPDLOCK, HOLDLOCK)
                       WHERE LocID = @LocID AND DataExpirare <= SYSUTCDATETIME())
        BEGIN
            DELETE FROM autogara.RezervariProvizorii WHERE LocID = @LocID;
            SET @StatusLoc = N'Liber';
        END

        IF @StatusLoc <> N'Liber'
            THROW 50023, N'Locul nu mai este liber (a fost rezervat sau vândut între timp).', 1;

        UPDATE autogara.Locuri SET Status = N'Rezervat' WHERE LocID = @LocID;

        SET @RezervareID  = NEWID();
        SET @DataExpirare = DATEADD(MINUTE, @DurataMinute, SYSUTCDATETIME());

        INSERT INTO autogara.RezervariProvizorii (RezervareID, LocID, UtilizatorID, DataCreare, DataExpirare)
        VALUES (@RezervareID, @LocID, @UtilizatorID, SYSUTCDATETIME(), @DataExpirare);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

/* ---------------------------------------------------------------------
   sp_ConfirmaVanzareBilet — transformă o rezervare provizorie în bilet
   și înregistrează plata. Prețul se calculează în baza de date.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_ConfirmaVanzareBilet
    @RezervareID          UNIQUEIDENTIFIER,
    @NumePasager          NVARCHAR(200),
    @TelefonPasager       NVARCHAR(20)     = NULL,
    @TipReducereID        INT              = NULL,
    @VanzutDeUtilizatorID UNIQUEIDENTIFIER,
    @MetodaPlata          NVARCHAR(10),
    @NumarBonFiscal       NVARCHAR(50)     = NULL,
    @BiletID              UNIQUEIDENTIFIER = NULL OUTPUT,
    @CodBilet             NVARCHAR(20)     = NULL OUTPUT,
    @PretFinal            DECIMAL(10, 2)   = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NULLIF(LTRIM(RTRIM(@NumePasager)), N'') IS NULL
        THROW 50030, N'Numele pasagerului este obligatoriu.', 1;

    IF @MetodaPlata NOT IN (N'Numerar', N'Card')
        THROW 50031, N'Metoda de plată trebuie să fie Numerar sau Card.', 1;

    DECLARE @Expirata BIT = 0;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @LocID INT, @Expirare DATETIME2(3);

        SELECT @LocID = r.LocID, @Expirare = r.DataExpirare
        FROM autogara.RezervariProvizorii AS r WITH (UPDLOCK, HOLDLOCK)
        WHERE r.RezervareID = @RezervareID;

        IF @LocID IS NULL
            THROW 50032, N'Rezervarea provizorie nu există (a expirat și a fost eliberată sau a fost deja confirmată).', 1;

        IF @Expirare <= SYSUTCDATETIME()
        BEGIN
            -- eliberăm locul și raportăm eroarea după COMMIT
            DELETE FROM autogara.RezervariProvizorii WHERE RezervareID = @RezervareID;
            UPDATE autogara.Locuri SET Status = N'Liber' WHERE LocID = @LocID AND Status = N'Rezervat';
            SET @Expirata = 1;
        END
        ELSE
        BEGIN
            DECLARE @CursaID INT, @PretBaza DECIMAL(10, 2), @DataCursa DATE, @StatusCursa NVARCHAR(20);

            SELECT @CursaID     = c.CursaID,
                   @PretBaza    = c.Pret,
                   @DataCursa   = c.DataCursa,
                   @StatusCursa = c.Status
            FROM autogara.Locuri AS l WITH (UPDLOCK)
            JOIN autogara.Curse  AS c ON c.CursaID = l.CursaID
            WHERE l.LocID = @LocID;

            IF @StatusCursa <> N'Planificata'
                THROW 50033, N'Cursa nu mai este disponibilă pentru vânzare.', 1;

            IF NOT EXISTS (SELECT 1 FROM autogara.Utilizatori WHERE UtilizatorID = @VanzutDeUtilizatorID AND Activ = 1)
                THROW 50034, N'Utilizatorul care efectuează vânzarea nu există sau este dezactivat.', 1;

            IF @TipReducereID IS NOT NULL
               AND NOT EXISTS (SELECT 1 FROM autogara.TipuriReducere WHERE TipReducereID = @TipReducereID AND Activ = 1)
                THROW 50035, N'Tipul de reducere selectat nu există sau nu mai este activ.', 1;

            SET @PretFinal = autogara.fn_CalculeazaPret(@PretBaza, @TipReducereID);

            -- Cod bilet: AG + data cursei (aammzz) + 8 caractere hex aleatoare, ex. AG260917-3FA9C21B
            SET @CodBilet = CONCAT(N'AG', CONVERT(CHAR(6), @DataCursa, 12), N'-', CONVERT(VARCHAR(8), CRYPT_GEN_RANDOM(4), 2));
            WHILE EXISTS (SELECT 1 FROM autogara.Bilete WHERE CodBilet = @CodBilet)
                SET @CodBilet = CONCAT(N'AG', CONVERT(CHAR(6), @DataCursa, 12), N'-', CONVERT(VARCHAR(8), CRYPT_GEN_RANDOM(4), 2));

            SET @BiletID = NEWID();

            UPDATE autogara.Locuri SET Status = N'Ocupat' WHERE LocID = @LocID AND Status = N'Rezervat';
            IF @@ROWCOUNT = 0
                THROW 50036, N'Locul nu mai este în starea Rezervat.', 1;

            DELETE FROM autogara.RezervariProvizorii WHERE RezervareID = @RezervareID;

            INSERT INTO autogara.Bilete
                (BiletID, CodBilet, CursaID, LocID, NumePasager, TelefonPasager,
                 TipReducereID, VanzutDeUtilizatorID, DataEmitere, Pret, Status)
            VALUES
                (@BiletID, @CodBilet, @CursaID, @LocID, LTRIM(RTRIM(@NumePasager)), @TelefonPasager,
                 @TipReducereID, @VanzutDeUtilizatorID, SYSUTCDATETIME(), @PretFinal, N'Activ');

            INSERT INTO autogara.Plati (BiletID, Suma, MetodaPlata, DataPlata, Status, NumarBonFiscal)
            VALUES (@BiletID, @PretFinal, @MetodaPlata, SYSUTCDATETIME(), N'Finalizata', @NumarBonFiscal);

            DECLARE @Actiune NVARCHAR(200) = CONCAT(N'Vânzare bilet ', @CodBilet, N' (', @PretFinal, N' MDL, ', @MetodaPlata, N')');
            DECLARE @BiletText NVARCHAR(50) = CONVERT(NVARCHAR(50), @BiletID);
            EXEC autogara.sp_InregistreazaLogAudit @VanzutDeUtilizatorID, @Actiune, N'Bilete', @BiletText;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    IF @Expirata = 1
        THROW 50037, N'Rezervarea provizorie a expirat; locul a fost eliberat. Reluați selecția locului.', 1;
END;
GO

/* ---------------------------------------------------------------------
   sp_AnuleazaBilet — anulează un bilet activ, eliberează locul și
   înregistrează rambursarea conform politicii (fn_ProcentRambursare).
   Dacă cursa a fost anulată de autogară, rambursarea este integrală.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_AnuleazaBilet
    @BiletID               UNIQUEIDENTIFIER,
    @UtilizatorID          UNIQUEIDENTIFIER,
    @RambursareIntegrala   BIT            = 0,
    @SumaRambursata        DECIMAL(10, 2) = NULL OUTPUT,
    @StatusNou             NVARCHAR(20)   = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @StatusBilet NVARCHAR(20), @Pret DECIMAL(10, 2), @LocID INT, @CodBilet NVARCHAR(20),
                @StatusCursa NVARCHAR(20), @Plecare DATETIME2(0), @Metoda NVARCHAR(10);

        SELECT @StatusBilet = b.Status,
               @Pret        = b.Pret,
               @LocID       = b.LocID,
               @CodBilet    = b.CodBilet,
               @StatusCursa = c.Status,
               @Plecare     = autogara.fn_MomentCursa(c.DataCursa, c.OraPlecare)
        FROM autogara.Bilete AS b WITH (UPDLOCK, HOLDLOCK)
        JOIN autogara.Curse  AS c ON c.CursaID = b.CursaID
        WHERE b.BiletID = @BiletID;

        IF @StatusBilet IS NULL
            THROW 50040, N'Biletul specificat nu există.', 1;

        IF @StatusBilet <> N'Activ'
            THROW 50041, N'Biletul a fost deja anulat sau rambursat.', 1;

        DECLARE @Procent DECIMAL(5, 2);

        IF @RambursareIntegrala = 1 OR @StatusCursa = N'Anulata'
            SET @Procent = 100;
        ELSE
        BEGIN
            IF @StatusCursa IN (N'In desfasurare', N'Finalizata') OR @Plecare <= autogara.fn_AcumLocal()
                THROW 50042, N'Biletul nu mai poate fi anulat: cursa a plecat.', 1;

            SET @Procent = autogara.fn_ProcentRambursare(@Plecare, autogara.fn_AcumLocal());
        END

        SET @SumaRambursata = ROUND(@Pret * @Procent / 100, 2);
        SET @StatusNou      = CASE WHEN @SumaRambursata > 0 THEN N'Rambursat' ELSE N'Anulat' END;

        UPDATE autogara.Bilete
        SET Status = @StatusNou, ModificatLa = SYSUTCDATETIME()
        WHERE BiletID = @BiletID;

        IF @StatusCursa = N'Planificata'
            UPDATE autogara.Locuri SET Status = N'Liber' WHERE LocID = @LocID AND Status = N'Ocupat';

        IF @SumaRambursata > 0
        BEGIN
            SELECT TOP (1) @Metoda = MetodaPlata
            FROM autogara.Plati
            WHERE BiletID = @BiletID AND Status = N'Finalizata'
            ORDER BY DataPlata;

            INSERT INTO autogara.Plati (BiletID, Suma, MetodaPlata, DataPlata, Status)
            VALUES (@BiletID, @SumaRambursata, ISNULL(@Metoda, N'Numerar'), SYSUTCDATETIME(), N'Rambursata');
        END

        DECLARE @Actiune NVARCHAR(200) =
            CONCAT(N'Anulare bilet ', @CodBilet, N' — rambursat ', @SumaRambursata, N' MDL (', CAST(@Procent AS INT), N'%)');
        DECLARE @BiletText NVARCHAR(50) = CONVERT(NVARCHAR(50), @BiletID);
        EXEC autogara.sp_InregistreazaLogAudit @UtilizatorID, @Actiune, N'Bilete', @BiletText;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

/* ---------------------------------------------------------------------
   sp_RaportVanzariZilnic — vânzări pe o zi (ora locală), pe traseu,
   casier și metodă de plată. @Data NULL = azi.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_RaportVanzariZilnic
    @Data DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @Data = ISNULL(@Data, CAST(autogara.fn_AcumLocal() AS DATE));

    DECLARE @DeLaUtc DATETIME2(3) = autogara.fn_LocalLaUtc(CAST(@Data AS DATETIME2(3)));
    DECLARE @PanaLaUtc DATETIME2(3) = autogara.fn_LocalLaUtc(CAST(DATEADD(DAY, 1, @Data) AS DATETIME2(3)));

    SELECT
        @Data                                                               AS Data,
        t.Denumire                                                          AS Traseu,
        CONCAT(u.Prenume, N' ', u.Nume)                                     AS Casier,
        p.MetodaPlata,
        SUM(CASE WHEN p.Status = N'Finalizata' THEN 1 ELSE 0 END)           AS BileteVandute,
        SUM(CASE WHEN p.Status = N'Rambursata' THEN 1 ELSE 0 END)           AS Rambursari,
        SUM(CASE WHEN p.Status = N'Finalizata' THEN p.Suma ELSE 0 END)      AS SumaIncasata,
        SUM(CASE WHEN p.Status = N'Rambursata' THEN p.Suma ELSE 0 END)      AS SumaRambursata,
        SUM(CASE WHEN p.Status = N'Finalizata' THEN p.Suma ELSE -p.Suma END) AS IncasariNete
    FROM autogara.Plati       AS p
    JOIN autogara.Bilete      AS b ON b.BiletID      = p.BiletID
    JOIN autogara.Curse       AS c ON c.CursaID      = b.CursaID
    JOIN autogara.Trasee      AS t ON t.TraseuID     = c.TraseuID
    JOIN autogara.Utilizatori AS u ON u.UtilizatorID = b.VanzutDeUtilizatorID
    WHERE p.DataPlata >= @DeLaUtc
      AND p.DataPlata <  @PanaLaUtc
    GROUP BY t.Denumire, u.Prenume, u.Nume, p.MetodaPlata
    ORDER BY t.Denumire, Casier, p.MetodaPlata;
END;
GO

/* ---------------------------------------------------------------------
   sp_RaportOcupareCurse — gradul de ocupare pe un interval de date,
   opțional filtrat pe traseu.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_RaportOcupareCurse
    @DataStart DATE,
    @DataStop  DATE,
    @TraseuID  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @DataStart IS NULL OR @DataStop IS NULL OR @DataStop < @DataStart
        THROW 50050, N'Intervalul de date pentru raport este invalid.', 1;

    SELECT
        o.CursaID,
        o.DataCursa,
        o.OraPlecare,
        o.Traseu,
        o.NrInmatriculare,
        o.StatusCursa,
        o.CapacitateLocuri,
        o.LocuriOcupate,
        o.LocuriRezervate,
        o.LocuriLibere,
        o.GradOcupareProcent,
        o.BileteActive,
        o.BileteAnulate,
        o.Incasari
    FROM autogara.vw_OcupareCurse AS o
    WHERE o.DataCursa BETWEEN @DataStart AND @DataStop
      AND (@TraseuID IS NULL OR o.TraseuID = @TraseuID)
    ORDER BY o.DataCursa, o.OraPlecare, o.Traseu;
END;
GO

/* ---------------------------------------------------------------------
   sp_AdaugaUtilizator — parola vine deja hash-uită din backend
   (PBKDF2/BCrypt); baza de date nu vede niciodată parola în clar.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_AdaugaUtilizator
    @NumeUtilizator      NVARCHAR(50),
    @ParolaHash          VARBINARY(256),
    @Nume                NVARCHAR(100),
    @Prenume             NVARCHAR(100),
    @Email               NVARCHAR(150)    = NULL,
    @Telefon             NVARCHAR(20)     = NULL,
    @Rol                 NVARCHAR(50),
    @CreatDeUtilizatorID UNIQUEIDENTIFIER = NULL,
    @UtilizatorID        UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @RolID INT = (SELECT RolID FROM autogara.Roluri WHERE Denumire = @Rol);

    IF @RolID IS NULL
        THROW 50060, N'Rolul specificat nu există (Admin, Casier sau Pasager).', 1;

    IF @ParolaHash IS NULL OR DATALENGTH(@ParolaHash) < 16
        THROW 50061, N'Hash-ul parolei lipsește sau este invalid.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM autogara.Utilizatori WITH (UPDLOCK, HOLDLOCK) WHERE NumeUtilizator = @NumeUtilizator)
            THROW 50062, N'Numele de utilizator este deja folosit.', 1;

        SET @UtilizatorID = NEWID();

        INSERT INTO autogara.Utilizatori
            (UtilizatorID, NumeUtilizator, ParolaHash, Nume, Prenume, Email, Telefon, RolID, Activ)
        VALUES
            (@UtilizatorID, @NumeUtilizator, @ParolaHash, @Nume, @Prenume, @Email, @Telefon, @RolID, 1);

        DECLARE @Actiune NVARCHAR(200) = CONCAT(N'Creare utilizator ', @NumeUtilizator, N' (rol ', @Rol, N')');
        DECLARE @IdText NVARCHAR(50) = CONVERT(NVARCHAR(50), @UtilizatorID);
        EXEC autogara.sp_InregistreazaLogAudit @CreatDeUtilizatorID, @Actiune, N'Utilizatori', @IdText;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

/* ---------------------------------------------------------------------
   sp_DezactiveazaUtilizator — soft-delete; nu permite dezactivarea
   ultimului administrator activ sau a propriului cont.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE autogara.sp_DezactiveazaUtilizator
    @UtilizatorID             UNIQUEIDENTIFIER,
    @DezactivatDeUtilizatorID UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @UtilizatorID = @DezactivatDeUtilizatorID
        THROW 50063, N'Nu vă puteți dezactiva propriul cont.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Activ BIT, @Rol NVARCHAR(50), @NumeUtilizator NVARCHAR(50);

        SELECT @Activ = u.Activ, @Rol = r.Denumire, @NumeUtilizator = u.NumeUtilizator
        FROM autogara.Utilizatori AS u WITH (UPDLOCK, HOLDLOCK)
        JOIN autogara.Roluri      AS r ON r.RolID = u.RolID
        WHERE u.UtilizatorID = @UtilizatorID;

        IF @Activ IS NULL
            THROW 50064, N'Utilizatorul specificat nu există.', 1;

        IF @Activ = 0
            THROW 50065, N'Utilizatorul este deja dezactivat.', 1;

        IF @Rol = N'Admin'
           AND (SELECT COUNT(*)
                FROM autogara.Utilizatori AS u WITH (UPDLOCK, HOLDLOCK)
                JOIN autogara.Roluri AS r ON r.RolID = u.RolID
                WHERE r.Denumire = N'Admin' AND u.Activ = 1) <= 1
            THROW 50066, N'Nu se poate dezactiva ultimul administrator activ.', 1;

        UPDATE autogara.Utilizatori
        SET Activ = 0, ModificatLa = SYSUTCDATETIME()
        WHERE UtilizatorID = @UtilizatorID;

        -- rezervările provizorii ale utilizatorului nu mai au sens
        UPDATE l
        SET l.Status = N'Liber'
        FROM autogara.Locuri AS l
        JOIN autogara.RezervariProvizorii AS r ON r.LocID = l.LocID
        WHERE r.UtilizatorID = @UtilizatorID AND l.Status = N'Rezervat';

        DELETE FROM autogara.RezervariProvizorii WHERE UtilizatorID = @UtilizatorID;

        DECLARE @Actiune NVARCHAR(200) = CONCAT(N'Dezactivare utilizator ', @NumeUtilizator);
        DECLARE @IdText NVARCHAR(50) = CONVERT(NVARCHAR(50), @UtilizatorID);
        EXEC autogara.sp_InregistreazaLogAudit @DezactivatDeUtilizatorID, @Actiune, N'Utilizatori', @IdText;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
