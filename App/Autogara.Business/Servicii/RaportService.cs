using System.Globalization;
using System.Reflection;
using System.Text;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.DataAccess.Vederi;
using Autogara.FileServer;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

/// <summary>
/// Rapoartele (sp_RaportVanzariZilnic, sp_RaportOcupareCurse, vw_VanzariZilnice,
/// vw_RapoarteComparative) si exportul lor ca CSV (se deschide direct in Excel).
/// </summary>
public sealed class RaportService(BazaDeDate bd, SftpFileClient fileServer, Sesiune sesiune)
{
    /// <summary>Vanzarile unei zile, pe traseu, casier si metoda de plata.</summary>
    public Task<List<RaportVanzariRand>> VanzariZilniceAsync(DateOnly? data = null)
    {
        sesiune.CerePersonal();
        return bd.CitesteAsync(db => db.RaportVanzariZilnicAsync(data ?? OraLocala.Azi));
    }

    /// <summary>Totalurile pe zile si metoda de plata dintr-un interval.</summary>
    public Task<List<VanzareZilnicaRand>> VanzariPeIntervalAsync(DateOnly deLa, DateOnly panaLa)
    {
        sesiune.CereAdmin();
        VerificaInterval(deLa, panaLa);

        return bd.CitesteAsync(db => db.VanzariZilnice
            .Where(v => v.Data >= deLa && v.Data <= panaLa)
            .OrderBy(v => v.Data).ThenBy(v => v.MetodaPlata)
            .ToListAsync());
    }

    public Task<List<RaportOcupareRand>> OcupareCurseAsync(DateOnly deLa, DateOnly panaLa, int? traseuId = null)
    {
        sesiune.CereAdmin();
        VerificaInterval(deLa, panaLa);
        return bd.CitesteAsync(db => db.RaportOcupareCurseAsync(deLa, panaLa, traseuId));
    }

    /// <summary>Comparatia luna curenta / luna precedenta, pe trasee.</summary>
    public Task<List<RaportComparativRand>> RapoarteComparativeAsync(int? an = null, int? traseuId = null)
    {
        sesiune.CereAdmin();

        return bd.CitesteAsync(db => db.RapoarteComparative
            .Where(r => (an == null || r.An == an) && (traseuId == null || r.TraseuID == traseuId))
            .OrderBy(r => r.Traseu).ThenBy(r => r.An).ThenBy(r => r.Luna)
            .ToListAsync());
    }

    /// <summary>
    /// CSV cu separator ';' si BOM UTF-8 — Excel in setarile regionale RO/MD il deschide direct,
    /// cu diacriticele corecte si coloanele separate.
    /// </summary>
    public static byte[] ExportCsv<T>(IEnumerable<T> randuri)
    {
        var proprietati = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToArray();

        var sb = new StringBuilder();
        sb.AppendLine(string.Join(';', proprietati.Select(p => Escape(p.Name))));
        foreach (var r in randuri)
            sb.AppendLine(string.Join(';', proprietati.Select(p => Escape(Format(p.GetValue(r))))));

        return [.. Encoding.UTF8.GetPreamble(), .. Encoding.UTF8.GetBytes(sb.ToString())];
    }

    /// <summary>Arhiveaza exportul pe file server, in Rapoarte/{an}/{luna}/.</summary>
    public async Task<string> ArhiveazaPeFileServerAsync(string numeFisier, byte[] continut)
    {
        sesiune.CereAdmin();

        var nume = string.Concat(numeFisier.Select(c => Path.GetInvalidFileNameChars().Contains(c) || c == '/' ? '_' : c));
        if (string.IsNullOrWhiteSpace(nume))
            throw new ValidareException("Numele fișierului este invalid.");

        var azi = OraLocala.Azi;
        var cale = $"Rapoarte/{azi:yyyy}/{azi:MM}/{nume}";

        var temp = Path.GetTempFileName();
        try
        {
            await File.WriteAllBytesAsync(temp, continut);
            await fileServer.IncarcaAsync(temp, cale);
        }
        finally
        {
            File.Delete(temp);
        }

        return cale;
    }

    private static string Format(object? v) => v switch
    {
        null => string.Empty,
        DateOnly d => d.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
        TimeOnly t => t.ToString("HH:mm", CultureInfo.InvariantCulture),
        DateTime dt => dt.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
        decimal m => m.ToString("0.00", CultureInfo.GetCultureInfo("ro-RO")),
        double x => x.ToString("0.##", CultureInfo.GetCultureInfo("ro-RO")),
        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
        _ => v.ToString() ?? string.Empty,
    };

    private static string Escape(string s) =>
        s.IndexOfAny([';', '"', '\n', '\r']) >= 0 ? $"\"{s.Replace("\"", "\"\"")}\"" : s;

    private static void VerificaInterval(DateOnly deLa, DateOnly panaLa)
    {
        if (panaLa < deLa)
            throw new ValidareException("Data de sfârșit trebuie să fie după data de început.");
        if (panaLa.DayNumber - deLa.DayNumber > 366)
            throw new ValidareException("Intervalul poate fi de cel mult un an.");
    }
}
