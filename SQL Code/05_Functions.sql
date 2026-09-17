USE autogara;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER FUNCTION autogara.fn_AcumLocal()
RETURNS DATETIME2(0)
AS
BEGIN
    RETURN CAST(SYSDATETIMEOFFSET() AT TIME ZONE N'E. Europe Standard Time' AS DATETIME2(0));
END;
GO

CREATE OR ALTER FUNCTION autogara.fn_UtcLaLocal(@MomentUtc DATETIME2(3))
RETURNS DATETIME2(3)
AS
BEGIN
    RETURN CAST((@MomentUtc AT TIME ZONE N'UTC') AT TIME ZONE N'E. Europe Standard Time' AS DATETIME2(3));
END;
GO

CREATE OR ALTER FUNCTION autogara.fn_LocalLaUtc(@MomentLocal DATETIME2(3))
RETURNS DATETIME2(3)
AS
BEGIN
    RETURN CAST((@MomentLocal AT TIME ZONE N'E. Europe Standard Time') AT TIME ZONE N'UTC' AS DATETIME2(3));
END;
GO

CREATE OR ALTER FUNCTION autogara.fn_MomentCursa(@Data DATE, @Ora TIME(0))
RETURNS DATETIME2(0)
AS
BEGIN
    RETURN DATEADD(SECOND, DATEDIFF(SECOND, CAST('00:00:00' AS TIME(0)), @Ora), CAST(@Data AS DATETIME2(0)));
END;
GO

CREATE OR ALTER FUNCTION autogara.fn_CalculeazaPret(@PretBaza DECIMAL(10, 2), @TipReducereID INT)
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @Procent DECIMAL(5, 2) = 0;

    IF @TipReducereID IS NOT NULL
        SELECT @Procent = ProcentReducere
        FROM autogara.TipuriReducere
        WHERE TipReducereID = @TipReducereID AND Activ = 1;

    RETURN ROUND(@PretBaza * (100 - ISNULL(@Procent, 0)) / 100, 2);
END;
GO

CREATE OR ALTER FUNCTION autogara.fn_ProcentRambursare(@PlecareLocal DATETIME2(0), @MomentLocal DATETIME2(0))
RETURNS DECIMAL(5, 2)
AS
BEGIN
    DECLARE @Minute INT = DATEDIFF(MINUTE, @MomentLocal, @PlecareLocal);

    RETURN CASE
               WHEN @Minute >= 24 * 60 THEN 100
               WHEN @Minute >= 2 * 60  THEN 50
               ELSE 0
           END;
END;
GO
