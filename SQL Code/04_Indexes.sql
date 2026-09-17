/* =====================================================================
   04_Indexes.sql
   Indici pentru performanță (cei din plan + indici pe cheile străine
   folosite frecvent în rapoarte și căutări).
   ===================================================================== */
USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ------------------------- Indicii din plan ------------------------ */

-- Căutarea curselor după dată și traseu (FrmCautareCurse, dashboard)
DROP INDEX IF EXISTS IX_Curse_DataCursa_TraseuID ON autogara.Curse;
CREATE INDEX IX_Curse_DataCursa_TraseuID
    ON autogara.Curse (DataCursa, TraseuID)
    INCLUDE (OraPlecare, OraSosireEstimata, Pret, Status, AutobuzID, SoferID);
GO

-- Harta locurilor unei curse, filtrare după status
DROP INDEX IF EXISTS IX_Locuri_CursaID ON autogara.Locuri;
CREATE INDEX IX_Locuri_CursaID
    ON autogara.Locuri (CursaID, Status)
    INCLUDE (NumarLoc);
GO

-- Job-ul de eliberare a rezervărilor expirate (rulează la 1 minut)
DROP INDEX IF EXISTS IX_RezervariProvizorii_DataExpirare ON autogara.RezervariProvizorii;
CREATE INDEX IX_RezervariProvizorii_DataExpirare
    ON autogara.RezervariProvizorii (DataExpirare)
    INCLUDE (LocID);
GO

-- Căutare/validare bilet după cod (și garanția de unicitate a codului)
DROP INDEX IF EXISTS IX_Bilete_CodBilet ON autogara.Bilete;
CREATE UNIQUE INDEX IX_Bilete_CodBilet
    ON autogara.Bilete (CodBilet);
GO

/* ---------------------- Integritate suplimentară ------------------- */

-- Un loc poate avea cel mult un bilet ACTIV (anulatele/rambursatele rămân în istoric)
DROP INDEX IF EXISTS UX_Bilete_LocID_Activ ON autogara.Bilete;
CREATE UNIQUE INDEX UX_Bilete_LocID_Activ
    ON autogara.Bilete (LocID)
    WHERE Status = N'Activ';
GO

/* ----------------------- Indici pe chei străine -------------------- */

DROP INDEX IF EXISTS IX_Bilete_CursaID ON autogara.Bilete;
CREATE INDEX IX_Bilete_CursaID
    ON autogara.Bilete (CursaID, Status)
    INCLUDE (Pret, LocID);
GO

DROP INDEX IF EXISTS IX_Bilete_DataEmitere ON autogara.Bilete;
CREATE INDEX IX_Bilete_DataEmitere
    ON autogara.Bilete (DataEmitere)
    INCLUDE (CursaID, Pret, Status, VanzutDeUtilizatorID, TipReducereID);
GO

DROP INDEX IF EXISTS IX_Plati_BiletID ON autogara.Plati;
CREATE INDEX IX_Plati_BiletID
    ON autogara.Plati (BiletID);
GO

DROP INDEX IF EXISTS IX_Plati_DataPlata ON autogara.Plati;
CREATE INDEX IX_Plati_DataPlata
    ON autogara.Plati (DataPlata)
    INCLUDE (BiletID, Suma, MetodaPlata, Status);
GO

DROP INDEX IF EXISTS IX_TraseuOpriri_StatieID ON autogara.TraseuOpriri;
CREATE INDEX IX_TraseuOpriri_StatieID
    ON autogara.TraseuOpriri (StatieID)
    INCLUDE (TraseuID, Ordine);
GO

DROP INDEX IF EXISTS IX_Conexiuni_NodSosireID ON autogara.Conexiuni;
CREATE INDEX IX_Conexiuni_NodSosireID
    ON autogara.Conexiuni (NodSosireID);
GO

DROP INDEX IF EXISTS IX_MentenantaAutobuze_AutobuzID ON autogara.MentenantaAutobuze;
CREATE INDEX IX_MentenantaAutobuze_AutobuzID
    ON autogara.MentenantaAutobuze (AutobuzID, Data DESC);
GO

DROP INDEX IF EXISTS IX_Utilizatori_RolID ON autogara.Utilizatori;
CREATE INDEX IX_Utilizatori_RolID
    ON autogara.Utilizatori (RolID)
    INCLUDE (Activ);
GO

-- Job_CurataLogAudit (ștergere după dată) și filtrare istoric
DROP INDEX IF EXISTS IX_LogAudit_DataOra ON autogara.LogAudit;
CREATE INDEX IX_LogAudit_DataOra
    ON autogara.LogAudit (DataOra)
    INCLUDE (UtilizatorID, Entitate);
GO

-- Job_VerificaExpirareITP
DROP INDEX IF EXISTS IX_Autobuze_DataExpirareITP ON autogara.Autobuze;
CREATE INDEX IX_Autobuze_DataExpirareITP
    ON autogara.Autobuze (DataExpirareITP)
    WHERE Activ = 1;
GO
