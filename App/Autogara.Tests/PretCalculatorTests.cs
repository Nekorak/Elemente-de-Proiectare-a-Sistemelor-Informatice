using Autogara.Business.Reguli;

namespace Autogara.Tests;

public class PretCalculatorTests
{
    [Theory]
    [InlineData(120.00, 0, 120.00)]
    [InlineData(120.00, 50, 60.00)]
    [InlineData(155.00, 30, 108.50)]
    [InlineData(38.00, 20, 30.40)]
    [InlineData(0.05, 50, 0.03)]   // ROUND din SQL: .5 se rotunjeste departe de zero
    [InlineData(10.01, 50, 5.01)]  // 5.005 -> 5.01
    [InlineData(99.99, 100, 0)]
    public void PretCuReducere_CaFnCalculeazaPret(decimal pret, decimal procent, decimal asteptat)
    {
        Assert.Equal(asteptat, PretCalculator.PretCuReducere(pret, procent));
    }

    [Fact]
    public void PretCuReducere_RespingeValoriInvalide()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PretCalculator.PretCuReducere(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => PretCalculator.PretCuReducere(10, 101));
    }

    private static readonly DateTime Plecare = new(2026, 9, 20, 14, 30, 0);

    [Theory]
    [InlineData(0, 3 * 24 * 60, 100)]
    [InlineData(0, 24 * 60, 100)]      // exact 24h
    [InlineData(0, 24 * 60 - 1, 50)]
    [InlineData(0, 2 * 60, 50)]        // exact 2h
    [InlineData(0, 2 * 60 - 1, 0)]
    [InlineData(0, 5, 0)]
    public void ProcentRambursare_PraguriCaFnProcentRambursare(int _, int minuteInainte, decimal asteptat)
    {
        Assert.Equal(asteptat, PretCalculator.ProcentRambursare(Plecare, Plecare.AddMinutes(-minuteInainte)));
    }

    [Fact]
    public void ProcentRambursare_NumaraGranitaDeMinutCaDateDiff()
    {
        // DATEDIFF(MINUTE, 12:29:59, 14:30:00) = 121 (granite de minut), nu 120 minute intregi.
        var moment = new DateTime(2026, 9, 20, 12, 29, 59);
        Assert.Equal(50m, PretCalculator.ProcentRambursare(Plecare, moment));

        // DATEDIFF(MINUTE, 12:30:59, 14:30:00) = 120 -> tot 50%.
        Assert.Equal(50m, PretCalculator.ProcentRambursare(Plecare, new DateTime(2026, 9, 20, 12, 30, 59)));

        // DATEDIFF(MINUTE, 12:31:00, 14:30:00) = 119 -> 0%.
        Assert.Equal(0m, PretCalculator.ProcentRambursare(Plecare, new DateTime(2026, 9, 20, 12, 31, 0)));
    }

    [Theory]
    [InlineData(120.00, 50, 60.00)]
    [InlineData(108.50, 50, 54.25)]
    [InlineData(30.45, 50, 15.23)]
    public void SumaRambursata(decimal pret, decimal procent, decimal asteptat)
    {
        Assert.Equal(asteptat, PretCalculator.SumaRambursata(pret, procent));
    }
}
