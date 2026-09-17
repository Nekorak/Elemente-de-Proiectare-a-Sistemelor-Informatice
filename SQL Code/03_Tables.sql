USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

DROP TABLE IF EXISTS
    autogara.LogAudit,
    autogara.Plati,
    autogara.Bilete,
    autogara.RezervariProvizorii,
    autogara.Locuri,
    autogara.Curse,
    autogara.TipuriReducere,
    autogara.TraseuOpriri,
    autogara.Trasee,
    autogara.Soferi,
    autogara.MentenantaAutobuze,
    autogara.Autobuze,
    autogara.Statii,
    autogara.Conexiuni,
    autogara.Noduri,
    autogara.Utilizatori,
    autogara.Roluri;
GO

CREATE TABLE autogara.Roluri
(
    RolID       INT IDENTITY(1, 1)  NOT NULL,
    Denumire    NVARCHAR(50)        NOT NULL,

    CONSTRAINT PK_Roluri PRIMARY KEY (RolID),
    CONSTRAINT UQ_Roluri_Denumire UNIQUE (Denumire),
    CONSTRAINT CK_Roluri_Denumire CHECK (Denumire IN (N'Admin', N'Casier', N'Pasager'))
);
GO

CREATE TABLE autogara.Utilizatori
(
    UtilizatorID    UNIQUEIDENTIFIER    NOT NULL CONSTRAINT DF_Utilizatori_UtilizatorID DEFAULT NEWID(),
    NumeUtilizator  NVARCHAR(50)        NOT NULL,
    ParolaHash      VARBINARY(256)      NOT NULL,
    Nume            NVARCHAR(100)       NOT NULL,
    Prenume         NVARCHAR(100)       NOT NULL,
    Email           NVARCHAR(150)       NULL,
    Telefon         NVARCHAR(20)        NULL,
    RolID           INT                 NOT NULL,
    Activ           BIT                 NOT NULL CONSTRAINT DF_Utilizatori_Activ DEFAULT 1,
    CreatLa         DATETIME2(3)        NOT NULL CONSTRAINT DF_Utilizatori_CreatLa DEFAULT SYSUTCDATETIME(),
    ModificatLa     DATETIME2(3)        NULL,
    RowVersion      ROWVERSION          NOT NULL,

    CONSTRAINT PK_Utilizatori PRIMARY KEY (UtilizatorID),
    CONSTRAINT UQ_Utilizatori_NumeUtilizator UNIQUE (NumeUtilizator),
    CONSTRAINT FK_Utilizatori_Roluri FOREIGN KEY (RolID) REFERENCES autogara.Roluri (RolID),
    CONSTRAINT CK_Utilizatori_NumeUtilizator CHECK (LEN(NumeUtilizator) >= 3),
    CONSTRAINT CK_Utilizatori_Email CHECK (Email IS NULL OR Email LIKE N'%_@_%._%')
);
GO

CREATE TABLE autogara.Noduri
(
    NodID       INT IDENTITY(1, 1)  NOT NULL,
    Tip         NVARCHAR(20)        NOT NULL,
    Nume        NVARCHAR(100)       NOT NULL,
    CoordX      FLOAT               NOT NULL,
    CoordY      FLOAT               NOT NULL,
    Activ       BIT                 NOT NULL CONSTRAINT DF_Noduri_Activ DEFAULT 1,
    CreatLa     DATETIME2(3)        NOT NULL CONSTRAINT DF_Noduri_CreatLa DEFAULT SYSUTCDATETIME(),
    ModificatLa DATETIME2(3)        NULL,

    CONSTRAINT PK_Noduri PRIMARY KEY (NodID),
    CONSTRAINT CK_Noduri_Tip CHECK (Tip IN (N'Statie', N'Intersectie'))
);
GO

CREATE TABLE autogara.Conexiuni
(
    ConexiuneID INT IDENTITY(1, 1)  NOT NULL,
    NodPlecareID INT                NOT NULL,
    NodSosireID  INT                NOT NULL,
    DistantaKm   DECIMAL(6, 2)      NOT NULL,
    CreatLa      DATETIME2(3)       NOT NULL CONSTRAINT DF_Conexiuni_CreatLa DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Conexiuni PRIMARY KEY (ConexiuneID),
    CONSTRAINT FK_Conexiuni_NodPlecare FOREIGN KEY (NodPlecareID) REFERENCES autogara.Noduri (NodID),
    CONSTRAINT FK_Conexiuni_NodSosire  FOREIGN KEY (NodSosireID)  REFERENCES autogara.Noduri (NodID),
    CONSTRAINT UQ_Conexiuni_Noduri UNIQUE (NodPlecareID, NodSosireID),
    CONSTRAINT CK_Conexiuni_NoduriDiferite CHECK (NodPlecareID <> NodSosireID),
    CONSTRAINT CK_Conexiuni_DistantaKm CHECK (DistantaKm > 0)
);
GO

CREATE TABLE autogara.Statii
(
    StatieID    INT IDENTITY(1, 1)  NOT NULL,
    NodID       INT                 NOT NULL,
    Adresa      NVARCHAR(200)       NULL,
    Peron       NVARCHAR(10)        NULL,
    CreatLa     DATETIME2(3)        NOT NULL CONSTRAINT DF_Statii_CreatLa DEFAULT SYSUTCDATETIME(),
    ModificatLa DATETIME2(3)        NULL,

    CONSTRAINT PK_Statii PRIMARY KEY (StatieID),
    CONSTRAINT FK_Statii_Noduri FOREIGN KEY (NodID) REFERENCES autogara.Noduri (NodID),
    CONSTRAINT UQ_Statii_NodID UNIQUE (NodID)
);
GO

CREATE TABLE autogara.Autobuze
(
    AutobuzID        INT IDENTITY(1, 1) NOT NULL,
    NrInmatriculare  NVARCHAR(15)       NOT NULL,
    Model            NVARCHAR(100)      NOT NULL,
    CapacitateLocuri SMALLINT           NOT NULL,
    Status           NVARCHAR(20)       NOT NULL CONSTRAINT DF_Autobuze_Status DEFAULT N'Activ',
    CaleFisierJSON   NVARCHAR(260)      NULL,
    DataExpirareITP  DATE               NULL,
    Activ            BIT                NOT NULL CONSTRAINT DF_Autobuze_Activ DEFAULT 1,
    CreatLa          DATETIME2(3)       NOT NULL CONSTRAINT DF_Autobuze_CreatLa DEFAULT SYSUTCDATETIME(),
    ModificatLa      DATETIME2(3)       NULL,

    CONSTRAINT PK_Autobuze PRIMARY KEY (AutobuzID),
    CONSTRAINT UQ_Autobuze_NrInmatriculare UNIQUE (NrInmatriculare),
    CONSTRAINT CK_Autobuze_CapacitateLocuri CHECK (CapacitateLocuri > 0),
    CONSTRAINT CK_Autobuze_Status CHECK (Status IN (N'Activ', N'Service', N'Scos din uz'))
);
GO

CREATE TABLE autogara.MentenantaAutobuze
(
    MentenantaID INT IDENTITY(1, 1) NOT NULL,
    AutobuzID    INT                NOT NULL,
    TipLucrare   NVARCHAR(100)      NOT NULL,
    Data         DATE               NOT NULL,
    Kilometraj   INT                NULL,
    Observatii   NVARCHAR(500)      NULL,
    CreatLa      DATETIME2(3)       NOT NULL CONSTRAINT DF_MentenantaAutobuze_CreatLa DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MentenantaAutobuze PRIMARY KEY (MentenantaID),
    CONSTRAINT FK_MentenantaAutobuze_Autobuze FOREIGN KEY (AutobuzID) REFERENCES autogara.Autobuze (AutobuzID),
    CONSTRAINT CK_MentenantaAutobuze_Kilometraj CHECK (Kilometraj IS NULL OR Kilometraj >= 0)
);
GO

CREATE TABLE autogara.Soferi
(
    SoferID     INT IDENTITY(1, 1)  NOT NULL,
    Nume        NVARCHAR(100)       NOT NULL,
    Prenume     NVARCHAR(100)       NOT NULL,
    NrPermis    NVARCHAR(20)        NOT NULL,
    Telefon     NVARCHAR(20)        NULL,
    Activ       BIT                 NOT NULL CONSTRAINT DF_Soferi_Activ DEFAULT 1,
    CreatLa     DATETIME2(3)        NOT NULL CONSTRAINT DF_Soferi_CreatLa DEFAULT SYSUTCDATETIME(),
    ModificatLa DATETIME2(3)        NULL,

    CONSTRAINT PK_Soferi PRIMARY KEY (SoferID),
    CONSTRAINT UQ_Soferi_NrPermis UNIQUE (NrPermis)
);
GO

CREATE TABLE autogara.Trasee
(
    TraseuID    INT IDENTITY(1, 1)  NOT NULL,
    Denumire    NVARCHAR(150)       NOT NULL,
    Activ       BIT                 NOT NULL CONSTRAINT DF_Trasee_Activ DEFAULT 1,
    CreatLa     DATETIME2(3)        NOT NULL CONSTRAINT DF_Trasee_CreatLa DEFAULT SYSUTCDATETIME(),
    ModificatLa DATETIME2(3)        NULL,

    CONSTRAINT PK_Trasee PRIMARY KEY (TraseuID),
    CONSTRAINT UQ_Trasee_Denumire UNIQUE (Denumire)
);
GO

CREATE TABLE autogara.TraseuOpriri
(
    TraseuOprireID INT IDENTITY(1, 1) NOT NULL,
    TraseuID       INT                NOT NULL,
    StatieID       INT                NOT NULL,
    Ordine         SMALLINT           NOT NULL,

    CONSTRAINT PK_TraseuOpriri PRIMARY KEY (TraseuOprireID),
    CONSTRAINT FK_TraseuOpriri_Trasee FOREIGN KEY (TraseuID) REFERENCES autogara.Trasee (TraseuID),
    CONSTRAINT FK_TraseuOpriri_Statii FOREIGN KEY (StatieID) REFERENCES autogara.Statii (StatieID),
    CONSTRAINT UQ_TraseuOpriri_Ordine UNIQUE (TraseuID, Ordine),
    CONSTRAINT UQ_TraseuOpriri_Statie UNIQUE (TraseuID, StatieID),
    CONSTRAINT CK_TraseuOpriri_Ordine CHECK (Ordine > 0)
);
GO

CREATE TABLE autogara.TipuriReducere
(
    TipReducereID   INT IDENTITY(1, 1)  NOT NULL,
    Denumire        NVARCHAR(50)        NOT NULL,
    ProcentReducere DECIMAL(5, 2)       NOT NULL,
    Activ           BIT                 NOT NULL CONSTRAINT DF_TipuriReducere_Activ DEFAULT 1,

    CONSTRAINT PK_TipuriReducere PRIMARY KEY (TipReducereID),
    CONSTRAINT UQ_TipuriReducere_Denumire UNIQUE (Denumire),
    CONSTRAINT CK_TipuriReducere_Procent CHECK (ProcentReducere BETWEEN 0 AND 100)
);
GO

CREATE TABLE autogara.Curse
(
    CursaID           INT IDENTITY(1, 1) NOT NULL,
    TraseuID          INT                NOT NULL,
    AutobuzID         INT                NOT NULL,
    SoferID           INT                NOT NULL,
    DataCursa         DATE               NOT NULL,
    OraPlecare        TIME(0)            NOT NULL,
    OraSosireEstimata TIME(0)            NOT NULL,
    Pret              DECIMAL(10, 2)     NOT NULL,
    Status            NVARCHAR(20)       NOT NULL CONSTRAINT DF_Curse_Status DEFAULT N'Planificata',
    CreatLa           DATETIME2(3)       NOT NULL CONSTRAINT DF_Curse_CreatLa DEFAULT SYSUTCDATETIME(),
    ModificatLa       DATETIME2(3)       NULL,

    CONSTRAINT PK_Curse PRIMARY KEY (CursaID),
    CONSTRAINT FK_Curse_Trasee   FOREIGN KEY (TraseuID)  REFERENCES autogara.Trasee (TraseuID),
    CONSTRAINT FK_Curse_Autobuze FOREIGN KEY (AutobuzID) REFERENCES autogara.Autobuze (AutobuzID),
    CONSTRAINT FK_Curse_Soferi   FOREIGN KEY (SoferID)   REFERENCES autogara.Soferi (SoferID),
    CONSTRAINT CK_Curse_Ore CHECK (OraSosireEstimata > OraPlecare),
    CONSTRAINT CK_Curse_Pret CHECK (Pret >= 0),
    CONSTRAINT CK_Curse_Status CHECK (Status IN (N'Planificata', N'In desfasurare', N'Finalizata', N'Anulata'))
);
GO

CREATE TABLE autogara.Locuri
(
    LocID      INT IDENTITY(1, 1) NOT NULL,
    CursaID    INT                NOT NULL,
    NumarLoc   SMALLINT           NOT NULL,
    Status     NVARCHAR(20)       NOT NULL CONSTRAINT DF_Locuri_Status DEFAULT N'Liber',
    RowVersion ROWVERSION         NOT NULL,

    CONSTRAINT PK_Locuri PRIMARY KEY (LocID),
    CONSTRAINT FK_Locuri_Curse FOREIGN KEY (CursaID) REFERENCES autogara.Curse (CursaID),
    CONSTRAINT UQ_Locuri_CursaNumar UNIQUE (CursaID, NumarLoc),
    CONSTRAINT UQ_Locuri_LocCursa UNIQUE (LocID, CursaID),
    CONSTRAINT CK_Locuri_NumarLoc CHECK (NumarLoc > 0),
    CONSTRAINT CK_Locuri_Status CHECK (Status IN (N'Liber', N'Rezervat', N'Ocupat'))
);
GO

CREATE TABLE autogara.RezervariProvizorii
(
    RezervareID  UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_RezervariProvizorii_RezervareID DEFAULT NEWID(),
    LocID        INT              NOT NULL,
    UtilizatorID UNIQUEIDENTIFIER NULL,
    DataCreare   DATETIME2(3)     NOT NULL CONSTRAINT DF_RezervariProvizorii_DataCreare DEFAULT SYSUTCDATETIME(),
    DataExpirare DATETIME2(3)     NOT NULL,

    CONSTRAINT PK_RezervariProvizorii PRIMARY KEY (RezervareID),
    CONSTRAINT FK_RezervariProvizorii_Locuri FOREIGN KEY (LocID) REFERENCES autogara.Locuri (LocID),
    CONSTRAINT FK_RezervariProvizorii_Utilizatori FOREIGN KEY (UtilizatorID) REFERENCES autogara.Utilizatori (UtilizatorID),
    CONSTRAINT UQ_RezervariProvizorii_LocID UNIQUE (LocID),
    CONSTRAINT CK_RezervariProvizorii_Expirare CHECK (DataExpirare > DataCreare)
);
GO

CREATE TABLE autogara.Bilete
(
    BiletID              UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_Bilete_BiletID DEFAULT NEWID(),
    CodBilet             NVARCHAR(20)     NOT NULL,
    CursaID              INT              NOT NULL,
    LocID                INT              NOT NULL,
    NumePasager          NVARCHAR(200)    NOT NULL,
    TelefonPasager       NVARCHAR(20)     NULL,
    TipReducereID        INT              NULL,
    VanzutDeUtilizatorID UNIQUEIDENTIFIER NOT NULL,
    DataEmitere          DATETIME2(3)     NOT NULL CONSTRAINT DF_Bilete_DataEmitere DEFAULT SYSUTCDATETIME(),
    Pret                 DECIMAL(10, 2)   NOT NULL,
    Status               NVARCHAR(20)     NOT NULL CONSTRAINT DF_Bilete_Status DEFAULT N'Activ',
    ModificatLa          DATETIME2(3)     NULL,
    RowVersion           ROWVERSION       NOT NULL,

    CONSTRAINT PK_Bilete PRIMARY KEY (BiletID),
    CONSTRAINT FK_Bilete_Curse FOREIGN KEY (CursaID) REFERENCES autogara.Curse (CursaID),
    CONSTRAINT FK_Bilete_Locuri FOREIGN KEY (LocID, CursaID) REFERENCES autogara.Locuri (LocID, CursaID),
    CONSTRAINT FK_Bilete_TipuriReducere FOREIGN KEY (TipReducereID) REFERENCES autogara.TipuriReducere (TipReducereID),
    CONSTRAINT FK_Bilete_Utilizatori FOREIGN KEY (VanzutDeUtilizatorID) REFERENCES autogara.Utilizatori (UtilizatorID),
    CONSTRAINT CK_Bilete_Pret CHECK (Pret >= 0),
    CONSTRAINT CK_Bilete_Status CHECK (Status IN (N'Activ', N'Anulat', N'Rambursat'))
);
GO

CREATE TABLE autogara.Plati
(
    PlataID        BIGINT IDENTITY(1, 1) NOT NULL,
    BiletID        UNIQUEIDENTIFIER      NOT NULL,
    Suma           DECIMAL(10, 2)        NOT NULL,
    MetodaPlata    NVARCHAR(10)          NOT NULL,
    DataPlata      DATETIME2(3)          NOT NULL CONSTRAINT DF_Plati_DataPlata DEFAULT SYSUTCDATETIME(),
    Status         NVARCHAR(20)          NOT NULL CONSTRAINT DF_Plati_Status DEFAULT N'Finalizata',
    NumarBonFiscal NVARCHAR(50)          NULL,

    CONSTRAINT PK_Plati PRIMARY KEY (PlataID),
    CONSTRAINT FK_Plati_Bilete FOREIGN KEY (BiletID) REFERENCES autogara.Bilete (BiletID),
    CONSTRAINT CK_Plati_Suma CHECK (Suma >= 0),
    CONSTRAINT CK_Plati_MetodaPlata CHECK (MetodaPlata IN (N'Numerar', N'Card')),
    CONSTRAINT CK_Plati_Status CHECK (Status IN (N'Finalizata', N'Rambursata'))
);
GO

CREATE TABLE autogara.LogAudit
(
    LogID        BIGINT IDENTITY(1, 1) NOT NULL,
    UtilizatorID UNIQUEIDENTIFIER      NULL,
    Actiune      NVARCHAR(200)         NOT NULL,
    Entitate     NVARCHAR(100)         NULL,
    EntitateID   NVARCHAR(50)          NULL,
    DataOra      DATETIME2(3)          NOT NULL CONSTRAINT DF_LogAudit_DataOra DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_LogAudit PRIMARY KEY (LogID),
    CONSTRAINT FK_LogAudit_Utilizatori FOREIGN KEY (UtilizatorID) REFERENCES autogara.Utilizatori (UtilizatorID)
);
GO
