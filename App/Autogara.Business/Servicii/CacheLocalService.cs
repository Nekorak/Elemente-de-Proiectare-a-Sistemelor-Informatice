using System.Collections.Concurrent;
using System.Text.Json;
using Autogara.Common;
using Autogara.Common.Configurare;
using Serilog;

namespace Autogara.Business.Servicii;

/// <summary>
/// Cache local pentru modul offline: ultimele date de referinta citite cu succes (curse, statii,
/// reduceri, harta, structuri de autobuz) se pastreaza in %LocalAppData%\Autogara\Cache.
/// Cand serverul nu raspunde, se afiseaza acestea. Operatiile care modifica date (vanzare,
/// rezervare, anulare) nu merg offline — locurile trebuie verificate pe server.
/// </summary>
public sealed class CacheLocalService
{
    private static readonly JsonSerializerOptions Optiuni = new() { WriteIndented = false };

    private readonly string _folder;
    private readonly ConcurrentDictionary<string, Func<Task>> _reimprospatari = new();

    public CacheLocalService(string? folder = null) => _folder = folder ?? CaiLocale.FolderCache;

    /// <summary>S-au intors date din cache in loc de date de pe server (cheie, momentul salvarii, UTC).</summary>
    public event Action<string, DateTime>? DateDinCacheFolosite;

    /// <summary>
    /// Incearca serverul; la succes salveaza rezultatul local, la eroare de conexiune intoarce
    /// ultima copie salvata. Fara copie locala, eroarea de conexiune merge mai departe.
    /// </summary>
    public async Task<T> ObtineAsync<T>(string cheie, Func<Task<T>> dePeServer)
    {
        _reimprospatari[cheie] = async () => await SalveazaAsync(cheie, await dePeServer().ConfigureAwait(false)).ConfigureAwait(false);

        try
        {
            var valoare = await dePeServer().ConfigureAwait(false);
            await SalveazaAsync(cheie, valoare).ConfigureAwait(false);
            return valoare;
        }
        catch (ConexiuneException)
        {
            var (salvat, la) = await CitesteAsync<T>(cheie).ConfigureAwait(false);
            if (salvat is null)
                throw;

            Log.Warning("Server indisponibil; se folosesc datele din cache pentru {Cheie} (salvate la {La:u})", cheie, la);
            DateDinCacheFolosite?.Invoke(cheie, la!.Value);
            return salvat;
        }
    }

    public async Task SalveazaAsync<T>(string cheie, T valoare)
    {
        try
        {
            Directory.CreateDirectory(_folder);
            var temp = Cale(cheie) + ".tmp";
            await using (var fs = File.Create(temp))
                await JsonSerializer.SerializeAsync(fs, valoare, Optiuni).ConfigureAwait(false);
            File.Move(temp, Cale(cheie), overwrite: true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException)
        {
            // Cache-ul e optional: o eroare aici nu opreste operatia principala.
            Log.Warning(ex, "Nu s-a putut salva cache-ul {Cheie}", cheie);
        }
    }

    public async Task<(T? Valoare, DateTime? SalvatLaUtc)> CitesteAsync<T>(string cheie)
    {
        var cale = Cale(cheie);
        if (!File.Exists(cale))
            return (default, null);

        try
        {
            await using var fs = File.OpenRead(cale);
            var valoare = await JsonSerializer.DeserializeAsync<T>(fs, Optiuni).ConfigureAwait(false);
            return (valoare, File.GetLastWriteTimeUtc(cale));
        }
        catch (Exception ex) when (ex is IOException or JsonException or NotSupportedException)
        {
            Log.Warning(ex, "Cache corupt sau inaccesibil: {Cheie}", cheie);
            return (default, null);
        }
    }

    /// <summary>
    /// Sincronizarea la revenirea conexiunii: reciteste de pe server tot ce a fost cerut in
    /// sesiunea curenta. Apelat de backend cand ConexiuneStareService anunta ca serverul e iar disponibil.
    /// </summary>
    public async Task ReimprospateazaTotAsync()
    {
        foreach (var (cheie, reimprospatare) in _reimprospatari)
        {
            try
            {
                await reimprospatare().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Reîmprospătarea cache-ului {Cheie} a eșuat", cheie);
            }
        }
    }

    public void Goleste()
    {
        if (Directory.Exists(_folder))
            foreach (var f in Directory.EnumerateFiles(_folder, "*.json"))
                File.Delete(f);
    }

    private string Cale(string cheie)
    {
        var sigur = string.Concat(cheie.Select(c => char.IsLetterOrDigit(c) || c is '-' or '_' ? c : '_'));
        return Path.Combine(_folder, sigur + ".json");
    }
}
