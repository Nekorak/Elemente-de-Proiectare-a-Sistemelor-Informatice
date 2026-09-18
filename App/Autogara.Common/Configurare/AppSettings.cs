namespace Autogara.Common.Configurare;

/// <summary>
/// Configurarea unei statii de lucru: unde e serverul SQL, unde e file server-ul,
/// cu ce conturi se conecteaza aplicatia. Se salveaza local de <see cref="AppSettingsStore"/>,
/// cu parolele criptate DPAPI (nu in clar pe disc).
/// </summary>
public sealed class AppSettings
{
    public SetariSql Sql { get; set; } = new();
    public SetariFileServer FileServer { get; set; } = new();
    public SetariAplicatie Aplicatie { get; set; } = new();

    /// <summary>Valorile serverului echipei, folosite ca precompletare in wizard-ul de configurare.</summary>
    public static AppSettings Implicite() => new()
    {
        Sql = new SetariSql
        {
            Server = "188.237.68.25",
            Port = 14330,
            BazaDeDate = "autogara",
            Utilizator = "autogara_app",
            Parola = "AppEPSI2026",
        },
        FileServer = new SetariFileServer
        {
            Host = "188.237.68.25",
            Port = 44545,
            Utilizator = "Sergiu Hanganu",
            Parola = "Tokyopage799",
            CaleBaza = "Desktop/Elemente de Proiectare a Sistemelor Informatice/File-Server",
        },
    };

    public string ConnectionString()
    {
        var server = Sql.Port is > 0 ? $"{Sql.Server},{Sql.Port}" : Sql.Server;
        return $"Server={server};Database={Sql.BazaDeDate};User Id={Sql.Utilizator};Password={Sql.Parola};" +
               $"Encrypt={Sql.Criptare};TrustServerCertificate={Sql.IncredereCertificat};" +
               $"Connect Timeout={Sql.SecundeTimeout};Application Name=Autogara";
    }

    public IReadOnlyList<string> Valideaza()
    {
        var erori = new List<string>();
        if (string.IsNullOrWhiteSpace(Sql.Server)) erori.Add("Adresa serverului SQL lipsește.");
        if (string.IsNullOrWhiteSpace(Sql.BazaDeDate)) erori.Add("Numele bazei de date lipsește.");
        if (string.IsNullOrWhiteSpace(Sql.Utilizator)) erori.Add("Utilizatorul SQL lipsește.");
        if (string.IsNullOrEmpty(Sql.Parola)) erori.Add("Parola SQL lipsește.");
        if (Sql.Port is < 0 or > 65535) erori.Add("Portul SQL este invalid.");
        if (string.IsNullOrWhiteSpace(FileServer.Host)) erori.Add("Adresa file server-ului lipsește.");
        if (FileServer.Port is < 1 or > 65535) erori.Add("Portul file server-ului este invalid.");
        if (string.IsNullOrWhiteSpace(FileServer.Utilizator)) erori.Add("Utilizatorul file server-ului lipsește.");
        if (string.IsNullOrEmpty(FileServer.Parola)) erori.Add("Parola file server-ului lipsește.");
        if (Aplicatie.DurataRezervareMinute is < 1 or > 60) erori.Add("Durata rezervării trebuie să fie între 1 și 60 de minute.");
        return erori;
    }
}

public sealed class SetariSql
{
    public string Server { get; set; } = string.Empty;

    /// <summary>0 = portul implicit / instanta numita.</summary>
    public int Port { get; set; }

    public string BazaDeDate { get; set; } = "autogara";
    public string Utilizator { get; set; } = string.Empty;
    public string Parola { get; set; } = string.Empty;
    public bool Criptare { get; set; } = true;

    /// <summary>Serverul foloseste un certificat auto-semnat.</summary>
    public bool IncredereCertificat { get; set; } = true;

    public int SecundeTimeout { get; set; } = 15;
}

public sealed class SetariFileServer
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 22;
    public string Utilizator { get; set; } = string.Empty;
    public string Parola { get; set; } = string.Empty;

    /// <summary>Folderul File-Server, relativ la folderul de profil al contului SFTP.</summary>
    public string CaleBaza { get; set; } = string.Empty;

    public int SecundeTimeout { get; set; } = 15;
}

public sealed class SetariAplicatie
{
    /// <summary>Numele statiei de lucru, apare in log-uri.</summary>
    public string NumeStatie { get; set; } = Environment.MachineName;

    public int DurataRezervareMinute { get; set; } = 10;
    public int SecundeVerificareConexiune { get; set; } = 30;
    public string NivelLog { get; set; } = "Information";
}
