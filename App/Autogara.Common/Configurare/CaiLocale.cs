namespace Autogara.Common.Configurare;

/// <summary>
/// Singurele foldere locale folosite de aplicatie: configurarea statiei, log-urile si cache-ul
/// pentru modul offline. Datele propriu-zise vin din SQL si de pe file server.
/// </summary>
public static class CaiLocale
{
    public static string FolderAplicatie { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Autogara");

    public static string FolderLoguri => Path.Combine(FolderAplicatie, "Logs");

    public static string FolderCache => Path.Combine(FolderAplicatie, "Cache");
}
