namespace Autogara.Common;

/// <summary>
/// Baza de date salveaza momentele in UTC (SYSUTCDATETIME), iar orele curselor in ora Moldovei.
/// Aceeasi conversie ca fn_UtcLaLocal / fn_AcumLocal din 05_Functions.sql.
/// </summary>
public static class OraLocala
{
    private static readonly TimeZoneInfo Zona = GasesteZona();

    public static DateTime Acum => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Zona);

    public static DateOnly Azi => DateOnly.FromDateTime(Acum);

    public static DateTime DinUtc(DateTime utc) =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Zona);

    public static DateTime LaUtc(DateTime local) =>
        TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(local, DateTimeKind.Unspecified), Zona);

    private static TimeZoneInfo GasesteZona()
    {
        foreach (var id in new[] { "E. Europe Standard Time", "Europe/Chisinau" })
            if (TimeZoneInfo.TryFindSystemTimeZoneById(id, out var zona))
                return zona;

        return TimeZoneInfo.Local;
    }
}
