using Autogara.Common.Configurare;

namespace Autogara.FileServer;

/// <summary>
/// Datele de conectare la file server. Se citesc din configurarea locala a statiei
/// de lucru, nu se scriu in cod.
/// </summary>
public sealed class SftpSetari
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Utilizator { get; init; }
    public required string Parola { get; init; }

    /// <summary>
    /// Folderul de lucru pe server, relativ la folderul de profil al contului
    /// (sesiunea SFTP porneste in C:/Users/nekor).
    /// </summary>
    public required string CaleBaza { get; init; }

    public int SecundeTimeout { get; init; } = 15;
    public int NumarReincercari { get; init; } = 3;

    public static SftpSetari Din(SetariFileServer s) => new()
    {
        Host = s.Host,
        Port = s.Port,
        Utilizator = s.Utilizator,
        Parola = s.Parola,
        CaleBaza = s.CaleBaza.Replace('\\', '/').TrimEnd('/'),
        SecundeTimeout = s.SecundeTimeout,
    };
}
