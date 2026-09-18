namespace Autogara.Business.Reguli;

/// <summary>
/// Aceleasi formule ca fn_CalculeazaPret si fn_ProcentRambursare din 05_Functions.sql.
/// Sunt aici doar ca UI-ul sa afiseze pretul/rambursarea inainte de confirmare; suma finala
/// o calculeaza tot procedura stocata.
/// </summary>
public static class PretCalculator
{
    public static decimal PretCuReducere(decimal pretBaza, decimal procentReducere)
    {
        if (pretBaza < 0) throw new ArgumentOutOfRangeException(nameof(pretBaza));
        if (procentReducere is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(procentReducere));

        // ROUND din SQL rotunjeste .5 departe de zero.
        return Math.Round(pretBaza * (100 - procentReducere) / 100, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>100% cu cel putin 24h inainte de plecare, 50% cu cel putin 2h, altfel 0%.</summary>
    public static decimal ProcentRambursare(DateTime plecareLocal, DateTime momentLocal)
    {
        // DATEDIFF(MINUTE, ...) numara granitele de minut trecute, nu minutele intregi.
        var minute = (long)(Trunchiaza(plecareLocal) - Trunchiaza(momentLocal)).TotalMinutes;

        return minute switch
        {
            >= 24 * 60 => 100m,
            >= 2 * 60 => 50m,
            _ => 0m,
        };
    }

    public static decimal SumaRambursata(decimal pret, decimal procent) =>
        Math.Round(pret * procent / 100, 2, MidpointRounding.AwayFromZero);

    private static DateTime Trunchiaza(DateTime d) => new(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
}
