/* =====================================================================
   02_Schema.sql
   Schema dedicată "autogara" — toate obiectele aplicației stau aici.
   ===================================================================== */
USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF SCHEMA_ID(N'autogara') IS NULL
    EXEC (N'CREATE SCHEMA autogara AUTHORIZATION dbo;');
GO
