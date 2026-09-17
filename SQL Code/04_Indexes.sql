USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

DROP INDEX IF EXISTS IX_Curse_DataCursa_TraseuID ON autogara.Curse;
CREATE INDEX IX_Curse_DataCursa_TraseuID
    ON autogara.Curse (DataCursa, TraseuID)
    INCLUDE (OraPlecare, OraSosireEstimata, Pret, Status, AutobuzID, SoferID);
GO

DROP INDEX IF EXISTS IX_Locuri_CursaID ON autogara.Locuri;
CREATE INDEX IX_Locuri_CursaID
    ON autogara.Locuri (CursaID, Status)
    INCLUDE (NumarLoc);
GO

DROP INDEX IF EXISTS IX_RezervariProvizorii_DataExpirare ON autogara.RezervariProvizorii;
CREATE INDEX IX_RezervariProvizorii_DataExpirare
    ON autogara.RezervariProvizorii (DataExpirare)
    INCLUDE (LocID);
GO

DROP INDEX IF EXISTS IX_Bilete_CodBilet ON autogara.Bilete;
CREATE UNIQUE INDEX IX_Bilete_CodBilet
    ON autogara.Bilete (CodBilet);
GO

DROP INDEX IF EXISTS UX_Bilete_LocID_Activ ON autogara.Bilete;
CREATE UNIQUE INDEX UX_Bilete_LocID_Activ
    ON autogara.Bilete (LocID)
    WHERE Status = N'Activ';
GO

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

DROP INDEX IF EXISTS IX_LogAudit_DataOra ON autogara.LogAudit;
CREATE INDEX IX_LogAudit_DataOra
    ON autogara.LogAudit (DataOra)
    INCLUDE (UtilizatorID, Entitate);
GO

DROP INDEX IF EXISTS IX_Autobuze_DataExpirareITP ON autogara.Autobuze;
CREATE INDEX IX_Autobuze_DataExpirareITP
    ON autogara.Autobuze (DataExpirareITP)
    WHERE Activ = 1;
GO
