using Autogara.Common;
using Autogara.FileServer.Modele;

namespace Autogara.FileServer;

/// <summary>Citirea si scrierea hartii (Harta/harta.json).</summary>
public sealed class JsonHartaRepository(SftpFileClient client)
{
    public const string Cale = "Harta/harta.json";

    public async Task<HartaFisier> ReadAsync(CancellationToken ct = default)
    {
        var json = await client.CitesteTextAsync(Cale, ct).ConfigureAwait(false);
        var harta = JsonFisiere.Deserializeaza<HartaFisier>(json, Cale);
        ValidareException.AruncaDaca(Validare.Harta(harta));
        return harta;
    }

    /// <summary>Valideaza, apoi scrie; versiunea veche se copiaza in Backup/Harta/AAAA-LL-ZZ/.</summary>
    public async Task WriteAsync(HartaFisier harta, CancellationToken ct = default)
    {
        ValidareException.AruncaDaca(Validare.Harta(harta));
        await client.ScrieTextCuBackupAsync(Cale, JsonFisiere.Serializeaza(harta), ct).ConfigureAwait(false);
    }
}
