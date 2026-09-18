using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Autogara.Common.Configurare;

/// <summary>
/// Citeste si salveaza <see cref="AppSettings"/> in
/// %LocalAppData%\Autogara\appsettings.local.json. Parolele se scriu criptate cu DPAPI
/// (legate de contul Windows al statiei), deci fisierul nu poate fi copiat pe alt calculator.
/// </summary>
public sealed class AppSettingsStore
{
    private const string PrefixCriptat = "dpapi:";

    private static readonly JsonSerializerOptions Optiuni = new() { WriteIndented = true };
    private static readonly byte[] Entropie = "Autogara.AppSettings"u8.ToArray();

    public AppSettingsStore(string? caleFisier = null) =>
        CaleFisier = caleFisier ?? Path.Combine(CaiLocale.FolderAplicatie, "appsettings.local.json");

    public string CaleFisier { get; }

    /// <summary>False la prima pornire: UI-ul deschide wizard-ul de configurare.</summary>
    public bool Exista => File.Exists(CaleFisier);

    public AppSettings Citeste()
    {
        if (!Exista)
            throw new ConfigurareLipsaException();

        var setari = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(CaleFisier), Optiuni)
                     ?? throw new ConfigurareLipsaException();

        setari.Sql.Parola = Decripteaza(setari.Sql.Parola);
        setari.FileServer.Parola = Decripteaza(setari.FileServer.Parola);
        return setari;
    }

    public void Salveaza(AppSettings setari)
    {
        var json = JsonSerializer.SerializeToNode(setari, Optiuni)!.AsObject();
        json["Sql"]!["Parola"] = Cripteaza(setari.Sql.Parola);
        json["FileServer"]!["Parola"] = Cripteaza(setari.FileServer.Parola);

        Directory.CreateDirectory(Path.GetDirectoryName(CaleFisier)!);
        File.WriteAllText(CaleFisier, json.ToJsonString(Optiuni), Encoding.UTF8);
    }

    private static string Cripteaza(string text)
    {
        if (string.IsNullOrEmpty(text) || !OperatingSystem.IsWindows())
            return text;

        var criptat = ProtectedData.Protect(Encoding.UTF8.GetBytes(text), Entropie, DataProtectionScope.CurrentUser);
        return PrefixCriptat + Convert.ToBase64String(criptat);
    }

    private static string Decripteaza(string text)
    {
        if (!text.StartsWith(PrefixCriptat, StringComparison.Ordinal) || !OperatingSystem.IsWindows())
            return text;

        try
        {
            var date = Convert.FromBase64String(text[PrefixCriptat.Length..]);
            return Encoding.UTF8.GetString(ProtectedData.Unprotect(date, Entropie, DataProtectionScope.CurrentUser));
        }
        catch (CryptographicException)
        {
            // Fisier copiat de pe alt calculator/cont: parola trebuie reintrodusa in wizard.
            return string.Empty;
        }
    }
}

public sealed class ConfigurareLipsaException()
    : AutogaraException("Stația de lucru nu este configurată. Rulați configurarea inițială.");
