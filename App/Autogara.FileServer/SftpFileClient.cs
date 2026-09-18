using System.Net.Sockets;
using System.Text;
using Autogara.Common;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace Autogara.FileServer;

/// <summary>
/// Acces la fisierele de pe file server, prin SFTP, de pe orice statie de lucru.
/// Toate caile primite sunt relative la <see cref="SftpSetari.CaleBaza"/> si folosesc
/// separatorul '/', ex. "Autobuze/autobuz_3.json".
/// </summary>
public sealed class SftpFileClient : IDisposable
{
    private readonly SftpSetari _setari;
    private readonly SemaphoreSlim _acces = new(1, 1);
    private SftpClient? _client;

    public SftpFileClient(SftpSetari setari) => _setari = setari;

    // ---------- operatii ----------

    /// <summary>Numele fisierelor din folderul indicat (fara '.' si '..').</summary>
    public Task<IReadOnlyList<string>> ListeazaAsync(string folder = "", CancellationToken ct = default) =>
        ExecutaAsync(client => (IReadOnlyList<string>)client
            .ListDirectory(CaleCompleta(folder))
            .Where(f => f.IsRegularFile)
            .Select(f => f.Name)
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList(), ct);

    public Task<bool> ExistaAsync(string cale, CancellationToken ct = default) =>
        ExecutaAsync(client => client.Exists(CaleCompleta(cale)), ct);

    /// <summary>Citeste un fisier text (JSON) direct in memorie.</summary>
    public Task<string> CitesteTextAsync(string cale, CancellationToken ct = default) =>
        ExecutaAsync(client =>
        {
            using var ms = new MemoryStream();
            client.DownloadFile(CaleCompleta(cale), ms);
            return Encoding.UTF8.GetString(ms.ToArray());
        }, ct);

    /// <summary>Scrie (sau suprascrie) un fisier text pe server.</summary>
    public Task ScrieTextAsync(string cale, string continut, CancellationToken ct = default) =>
        ExecutaAsync(client =>
        {
            AsiguraFolder(client, FolderulDin(cale));
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(continut));
            client.UploadFile(ms, CaleCompleta(cale), canOverride: true);
            return true;
        }, ct);

    /// <summary>Descarca un fisier de pe server intr-un fisier local.</summary>
    public Task DescarcaAsync(string caleRemote, string caleLocala, CancellationToken ct = default) =>
        ExecutaAsync(client =>
        {
            var folderLocal = Path.GetDirectoryName(caleLocala);
            if (!string.IsNullOrEmpty(folderLocal))
                Directory.CreateDirectory(folderLocal);

            using var fs = File.Create(caleLocala);
            client.DownloadFile(CaleCompleta(caleRemote), fs);
            return true;
        }, ct);

    /// <summary>Incarca un fisier local pe server.</summary>
    public Task IncarcaAsync(string caleLocala, string caleRemote, CancellationToken ct = default) =>
        ExecutaAsync(client =>
        {
            AsiguraFolder(client, FolderulDin(caleRemote));
            using var fs = File.OpenRead(caleLocala);
            client.UploadFile(fs, CaleCompleta(caleRemote), canOverride: true);
            return true;
        }, ct);

    public Task StergeAsync(string cale, CancellationToken ct = default) =>
        ExecutaAsync(client =>
        {
            var completa = CaleCompleta(cale);
            if (client.Exists(completa))
                client.DeleteFile(completa);
            return true;
        }, ct);

    /// <summary>
    /// Suprascrie un fisier, dupa ce copiaza versiunea existenta in
    /// Backup/{folder}/{AAAA-LL-ZZ}/{nume}. Varianta de folosit la modificari.
    /// </summary>
    public Task ScrieTextCuBackupAsync(string cale, string continut, CancellationToken ct = default) =>
        ExecutaAsync(client =>
        {
            var completa = CaleCompleta(cale);

            if (client.Exists(completa))
            {
                var caleBackup = string.Join('/',
                    "Backup", FolderulDin(cale), DateTime.Today.ToString("yyyy-MM-dd"), NumeleDin(cale));

                AsiguraFolder(client, FolderulDin(caleBackup));

                using var vechi = new MemoryStream();
                client.DownloadFile(completa, vechi);
                vechi.Position = 0;
                client.UploadFile(vechi, CaleCompleta(caleBackup), canOverride: true);
            }

            AsiguraFolder(client, FolderulDin(cale));
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(continut));
            client.UploadFile(ms, completa, canOverride: true);
            return true;
        }, ct);

    // ---------- conexiune ----------

    /// <summary>Se conecteaza si verifica existenta folderului de baza (pentru wizard-ul de configurare).</summary>
    public Task<bool> TesteazaConexiuneAsync(CancellationToken ct = default) =>
        ExecutaAsync(client => client.Exists(_setari.CaleBaza), ct);

    private async Task<T> ExecutaAsync<T>(Func<SftpClient, T> operatie, CancellationToken ct)
    {
        await _acces.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            for (var incercare = 1; ; incercare++)
            {
                try
                {
                    return await Task.Run(() => operatie(Conectat()), ct).ConfigureAwait(false);
                }
                catch (Exception ex) when (EsteEroareDeRetea(ex) && incercare < _setari.NumarReincercari)
                {
                    Inchide();
                    await Task.Delay(TimeSpan.FromSeconds(incercare), ct).ConfigureAwait(false);
                }
            }
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new NegasitException($"Fișierul nu a fost găsit pe file server ({ex.Message}).");
        }
        catch (SshAuthenticationException ex)
        {
            Inchide();
            throw new ConexiuneException("Autentificarea la file server a eșuat. Verificați utilizatorul și parola din configurare.", ex);
        }
        catch (Exception ex) when (EsteEroareDeRetea(ex))
        {
            Inchide();
            throw new ConexiuneException("File server-ul nu poate fi contactat. Verificați conexiunea la internet.", ex);
        }
        finally
        {
            _acces.Release();
        }
    }

    private SftpClient Conectat()
    {
        if (_client is { IsConnected: true })
            return _client;

        Inchide();

        var client = new SftpClient(_setari.Host, _setari.Port, _setari.Utilizator, _setari.Parola)
        {
            OperationTimeout = TimeSpan.FromSeconds(_setari.SecundeTimeout),
            KeepAliveInterval = TimeSpan.FromSeconds(30)
        };
        client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(_setari.SecundeTimeout);
        client.Connect();

        _client = client;
        return client;
    }

    private void Inchide()
    {
        try { _client?.Dispose(); } catch { /* conexiune deja pierduta */ }
        _client = null;
    }

    private static bool EsteEroareDeRetea(Exception ex) =>
        ex is SshConnectionException or SshOperationTimeoutException or SocketException or IOException;

    // ---------- cai ----------

    private string CaleCompleta(string cale) =>
        string.IsNullOrEmpty(cale) ? _setari.CaleBaza : $"{_setari.CaleBaza}/{Normalizata(cale)}";

    private static string Normalizata(string cale) => cale.Replace('\\', '/').Trim('/');

    private static string FolderulDin(string cale)
    {
        var n = Normalizata(cale);
        var i = n.LastIndexOf('/');
        return i < 0 ? string.Empty : n[..i];
    }

    private static string NumeleDin(string cale)
    {
        var n = Normalizata(cale);
        var i = n.LastIndexOf('/');
        return i < 0 ? n : n[(i + 1)..];
    }

    /// <summary>Creeaza folderele lipsa, nivel cu nivel (SFTP nu creeaza recursiv).</summary>
    private void AsiguraFolder(SftpClient client, string folder)
    {
        if (string.IsNullOrEmpty(folder))
            return;

        var cale = _setari.CaleBaza;
        foreach (var parte in folder.Split('/', StringSplitOptions.RemoveEmptyEntries))
        {
            cale = $"{cale}/{parte}";
            if (!client.Exists(cale))
                client.CreateDirectory(cale);
        }
    }

    public void Dispose()
    {
        Inchide();
        _acces.Dispose();
    }
}
