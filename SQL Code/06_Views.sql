/* =====================================================================
   06_Views.sql
   Views pentru interogările frecvente, dashboard și rapoarte.
   ===================================================================== */
USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------------------------------------------------------------------
   vw_LocuriDisponibile — locurile libere pe cursele încă neplecate.
   Folosit de FrmVanzareBilet / FrmCasier.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW autogara.vw_LocuriDisponibile
AS
SELECT
    l.LocID,
    l.CursaID,
    l.NumarLoc,
    c.DataCursa,
    c.OraPlecare,
    c.OraSosireEstimata,
    c.Pret,
    t.TraseuID,
    t.Denumire          AS Traseu,
    a.AutobuzID,
    a.NrInmatriculare,
    a.CaleFisierJSON
FROM autogara.Locuri   AS l
JOIN autogara.Curse    AS c ON c.CursaID   = l.CursaID
JOIN autogara.Trasee   AS t ON t.TraseuID  = c.TraseuID
JOIN autogara.Autobuze AS a ON a.AutobuzID = c.AutobuzID
WHERE l.Status = N'Liber'
  AND c.Status = N'Planificata'
  AND autogara.fn_MomentCursa(c.DataCursa, c.OraPlecare) > autogara.fn_AcumLocal();
GO

/* ---------------------------------------------------------------------
   vw_CurseActive — cursele planificate / în desfășurare de azi încolo,
   cu stația de plecare/sosire, autobuz, șofer și situația locurilor.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW autogara.vw_CurseActive
AS
SELECT
    c.CursaID,
    c.DataCursa,
    c.OraPlecare,
    c.OraSosireEstimata,
    c.Status,
    c.Pret,
    t.TraseuID,
    t.Denumire                          AS Traseu,
    sp.StatieID                         AS StatiePlecareID,
    sp.Nume                             AS StatiePlecare,
    sp.Peron                            AS PeronPlecare,
    ss.StatieID                         AS StatieSosireID,
    ss.Nume                             AS StatieSosire,
    a.AutobuzID,
    a.NrInmatriculare,
    a.Model                             AS ModelAutobuz,
    s.SoferID,
    CONCAT(s.Prenume, N' ', s.Nume)     AS Sofer,
    s.Telefon                           AS TelefonSofer,
    ISNULL(lc.LocuriTotal, 0)           AS LocuriTotal,
    ISNULL(lc.LocuriLibere, 0)          AS LocuriLibere,
    ISNULL(lc.LocuriRezervate, 0)       AS LocuriRezervate,
    ISNULL(lc.LocuriOcupate, 0)         AS LocuriOcupate
FROM autogara.Curse    AS c
JOIN autogara.Trasee   AS t ON t.TraseuID  = c.TraseuID
JOIN autogara.Autobuze AS a ON a.AutobuzID = c.AutobuzID
JOIN autogara.Soferi   AS s ON s.SoferID   = c.SoferID
OUTER APPLY (
    SELECT TOP (1) st.StatieID, n.Nume, st.Peron
    FROM autogara.TraseuOpriri AS o
    JOIN autogara.Statii AS st ON st.StatieID = o.StatieID
    JOIN autogara.Noduri AS n  ON n.NodID     = st.NodID
    WHERE o.TraseuID = c.TraseuID
    ORDER BY o.Ordine ASC
) AS sp
OUTER APPLY (
    SELECT TOP (1) st.StatieID, n.Nume
    FROM autogara.TraseuOpriri AS o
    JOIN autogara.Statii AS st ON st.StatieID = o.StatieID
    JOIN autogara.Noduri AS n  ON n.NodID     = st.NodID
    WHERE o.TraseuID = c.TraseuID
    ORDER BY o.Ordine DESC
) AS ss
OUTER APPLY (
    SELECT
        COUNT(*)                                            AS LocuriTotal,
        SUM(CASE WHEN l.Status = N'Liber'    THEN 1 ELSE 0 END) AS LocuriLibere,
        SUM(CASE WHEN l.Status = N'Rezervat' THEN 1 ELSE 0 END) AS LocuriRezervate,
        SUM(CASE WHEN l.Status = N'Ocupat'   THEN 1 ELSE 0 END) AS LocuriOcupate
    FROM autogara.Locuri AS l
    WHERE l.CursaID = c.CursaID
) AS lc
WHERE c.Status IN (N'Planificata', N'In desfasurare')
  AND c.DataCursa >= CAST(autogara.fn_AcumLocal() AS DATE);
GO

/* ---------------------------------------------------------------------
   vw_VanzariZilnice — încasări și rambursări pe zi (ora locală) și
   metodă de plată.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW autogara.vw_VanzariZilnice
AS
SELECT
    CAST(autogara.fn_UtcLaLocal(p.DataPlata) AS DATE)                           AS Data,
    p.MetodaPlata,
    SUM(CASE WHEN p.Status = N'Finalizata' THEN 1 ELSE 0 END)                   AS BileteVandute,
    SUM(CASE WHEN p.Status = N'Rambursata' THEN 1 ELSE 0 END)                   AS Rambursari,
    SUM(CASE WHEN p.Status = N'Finalizata' THEN p.Suma ELSE 0 END)              AS SumaIncasata,
    SUM(CASE WHEN p.Status = N'Rambursata' THEN p.Suma ELSE 0 END)              AS SumaRambursata,
    SUM(CASE WHEN p.Status = N'Finalizata' THEN p.Suma ELSE -p.Suma END)        AS IncasariNete
FROM autogara.Plati AS p
GROUP BY CAST(autogara.fn_UtcLaLocal(p.DataPlata) AS DATE), p.MetodaPlata;
GO

/* ---------------------------------------------------------------------
   vw_OcupareCurse — gradul de ocupare și încasările pe fiecare cursă.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW autogara.vw_OcupareCurse
AS
SELECT
    c.CursaID,
    c.DataCursa,
    c.OraPlecare,
    c.Status                                AS StatusCursa,
    t.TraseuID,
    t.Denumire                              AS Traseu,
    a.NrInmatriculare,
    a.CapacitateLocuri,
    ISNULL(lc.LocuriOcupate, 0)             AS LocuriOcupate,
    ISNULL(lc.LocuriRezervate, 0)           AS LocuriRezervate,
    a.CapacitateLocuri
        - ISNULL(lc.LocuriOcupate, 0)
        - ISNULL(lc.LocuriRezervate, 0)     AS LocuriLibere,
    CAST(100.0 * ISNULL(lc.LocuriOcupate, 0) / NULLIF(a.CapacitateLocuri, 0) AS DECIMAL(5, 2)) AS GradOcupareProcent,
    ISNULL(bc.BileteActive, 0)              AS BileteActive,
    ISNULL(bc.BileteAnulate, 0)             AS BileteAnulate,
    ISNULL(bc.Incasari, 0)                  AS Incasari
FROM autogara.Curse    AS c
JOIN autogara.Trasee   AS t ON t.TraseuID  = c.TraseuID
JOIN autogara.Autobuze AS a ON a.AutobuzID = c.AutobuzID
OUTER APPLY (
    SELECT
        SUM(CASE WHEN l.Status = N'Ocupat'   THEN 1 ELSE 0 END) AS LocuriOcupate,
        SUM(CASE WHEN l.Status = N'Rezervat' THEN 1 ELSE 0 END) AS LocuriRezervate
    FROM autogara.Locuri AS l
    WHERE l.CursaID = c.CursaID
) AS lc
OUTER APPLY (
    SELECT
        SUM(CASE WHEN b.Status = N'Activ'  THEN 1 ELSE 0 END)      AS BileteActive,
        SUM(CASE WHEN b.Status <> N'Activ' THEN 1 ELSE 0 END)      AS BileteAnulate,
        SUM(CASE WHEN b.Status = N'Activ'  THEN b.Pret ELSE 0 END) AS Incasari
    FROM autogara.Bilete AS b
    WHERE b.CursaID = c.CursaID
) AS bc;
GO

/* ---------------------------------------------------------------------
   vw_RapoarteComparative — indicatori lunari pe traseu, comparați cu
   luna precedentă (curse, bilete, încasări, ocupare medie).
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW autogara.vw_RapoarteComparative
AS
WITH Lunar AS
(
    SELECT
        o.TraseuID,
        o.Traseu,
        YEAR(o.DataCursa)                               AS An,
        MONTH(o.DataCursa)                              AS Luna,
        COUNT(*)                                        AS NrCurse,
        SUM(o.BileteActive)                             AS BileteVandute,
        SUM(o.Incasari)                                 AS Incasari,
        CAST(AVG(o.GradOcupareProcent) AS DECIMAL(5, 2)) AS GradOcupareMediu
    FROM autogara.vw_OcupareCurse AS o
    WHERE o.StatusCursa <> N'Anulata'
    GROUP BY o.TraseuID, o.Traseu, YEAR(o.DataCursa), MONTH(o.DataCursa)
),
CuPrecedent AS
(
    SELECT
        l.*,
        LAG(l.BileteVandute)    OVER (PARTITION BY l.TraseuID ORDER BY l.An, l.Luna) AS BileteLunaPrecedenta,
        LAG(l.Incasari)         OVER (PARTITION BY l.TraseuID ORDER BY l.An, l.Luna) AS IncasariLunaPrecedenta,
        LAG(l.GradOcupareMediu) OVER (PARTITION BY l.TraseuID ORDER BY l.An, l.Luna) AS OcupareLunaPrecedenta
    FROM Lunar AS l
)
SELECT
    TraseuID,
    Traseu,
    An,
    Luna,
    NrCurse,
    BileteVandute,
    BileteLunaPrecedenta,
    Incasari,
    IncasariLunaPrecedenta,
    CAST(100.0 * (Incasari - IncasariLunaPrecedenta) / NULLIF(IncasariLunaPrecedenta, 0) AS DECIMAL(7, 2)) AS VariatieIncasariProcent,
    GradOcupareMediu,
    OcupareLunaPrecedenta,
    CAST(GradOcupareMediu - OcupareLunaPrecedenta AS DECIMAL(6, 2)) AS VariatieOcuparePuncte
FROM CuPrecedent;
GO
