using System.Text.Json;
using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.FileServer;
using Serilog;

namespace Autogara.Business.Servicii;

/// <summary>
/// Auto-update: pe file server, in radacina File-Server, sta version.json:
/// <code>{ "versiune": "1.2.0", "fisier": "Actualizari/Autogara-1.2.0.zip", "note": "..." }</code>
/// </summary>
public sealed class ActualizareService(SftpFileClient fileServer)
{
    public const string CaleVersiune = "version.json";

    private static readonly JsonSerializerOptions Optiuni = new() { PropertyNameCaseInsensitive = true };

    /// <returns>Informatiile versiunii noi, sau null daca aplicatia e la zi / nu exista version.json.</returns>
    public async Task<InfoActualizare?> VerificaAsync(Version versiuneCurenta, CancellationToken ct = default)
    {
        try
        {
            if (!await fileServer.ExistaAsync(CaleVersiune, ct))
                return null;

            var info = JsonSerializer.Deserialize<InfoActualizare>(await fileServer.CitesteTextAsync(CaleVersiune, ct), Optiuni);
            if (info is null || !Version.TryParse(info.Versiune, out var disponibila) || string.IsNullOrWhiteSpace(info.Fisier))
            {
                Log.Warning("version.json de pe file server este invalid");
                return null;
            }

            return disponibila > versiuneCurenta ? info : null;
        }
        catch (Exception ex) when (ex is ConexiuneException or JsonException)
        {
            // Fara actualizare daca file server-ul nu raspunde; aplicatia porneste normal.
            Log.Warning(ex, "Verificarea actualizărilor a eșuat");
            return null;
        }
    }

    /// <summary>Descarca pachetul in %TEMP%\Autogara\; UI-ul il porneste/dezarhiveaza.</summary>
    public async Task<string> DescarcaAsync(InfoActualizare info, CancellationToken ct = default)
    {
        var folder = Path.Combine(Path.GetTempPath(), "Autogara");
        var cale = Path.Combine(folder, Path.GetFileName(info.Fisier.Replace('\\', '/')));
        await fileServer.DescarcaAsync(info.Fisier, cale, ct);
        return cale;
    }
}
