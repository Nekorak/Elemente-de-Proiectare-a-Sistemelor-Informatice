using Autogara.Common;
using Autogara.FileServer.Modele;

namespace Autogara.FileServer;

/// <summary>Citirea si scrierea structurii locurilor unui autobuz (Autobuze/autobuz_{id}.json).</summary>
public sealed class JsonAutobuzRepository(SftpFileClient client)
{
    public static string CaleImplicita(int autobuzId) => $"Autobuze/autobuz_{autobuzId}.json";

    /// <param name="cale">Valoarea din Autobuze.CaleFisierJSON (acceptă și '\').</param>
    /// <param name="capacitate">Daca e data, se verifica si numarul de locuri.</param>
    public async Task<AutobuzStructura> ReadAsync(string cale, int? capacitate = null, CancellationToken ct = default)
    {
        var json = await client.CitesteTextAsync(cale, ct).ConfigureAwait(false);
        var structura = JsonFisiere.Deserializeaza<AutobuzStructura>(json, cale);
        ValidareException.AruncaDaca(Validare.Autobuz(structura, capacitate));
        return structura;
    }

    /// <summary>Valideaza, apoi scrie; versiunea veche se copiaza in Backup/Autobuze/AAAA-LL-ZZ/.</summary>
    public async Task WriteAsync(string cale, AutobuzStructura structura, int? capacitate = null, CancellationToken ct = default)
    {
        ValidareException.AruncaDaca(Validare.Autobuz(structura, capacitate));

        structura.Locuri = structura.Locuri.OrderBy(l => l.NumarLoc).ToList();
        await client.ScrieTextCuBackupAsync(cale, JsonFisiere.Serializeaza(structura), ct).ConfigureAwait(false);
    }

    public Task<bool> ExistaAsync(string cale, CancellationToken ct = default) => client.ExistaAsync(cale, ct);
}
