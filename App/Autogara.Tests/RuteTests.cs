using System.Text.Json;
using Autogara.Business.Reguli;
using Autogara.FileServer.Modele;

namespace Autogara.Tests;

public class RuteTests
{
    private static List<Rute.Muchie> MuchiiHartaReale()
    {
        var h = JsonSerializer.Deserialize<HartaFisier>(Ajutor.CitesteFisier("Harta/harta.json"),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return h.Conexiuni.Select(c => new Rute.Muchie(c.NodPlecareId, c.NodSosireId, c.DistantaKm)).ToList();
    }

    [Fact]
    public void ChisinauBalti_PrinOrhei()
    {
        // 1 Chisinau Centrala → 2 Nord → 14 Peresecina → 3 Orhei → 15 Ciocilteni → 4 Balti
        var drum = Rute.CelMaiScurt(MuchiiHartaReale(), 1, 4);

        Assert.NotNull(drum);
        Assert.Equal([1, 2, 14, 3, 15, 4], drum.Noduri);
        Assert.Equal(140.50m, drum.DistantaKm);
    }

    [Fact]
    public void ChisinauCahul_AlegeDrumulMaiScurt()
    {
        // Direct 1→11 (70 km) e mai scurt decat 1→10→11 (36 + 38 = 74 km).
        var drum = Rute.CelMaiScurt(MuchiiHartaReale(), 1, 13);

        Assert.NotNull(drum);
        Assert.Equal([1, 11, 12, 13], drum.Noduri);
        Assert.Equal(175m, drum.DistantaKm);
    }

    [Fact]
    public void AcelasiNod_DistantaZero()
    {
        var drum = Rute.CelMaiScurt(MuchiiHartaReale(), 5, 5);
        Assert.Equal(0, drum!.DistantaKm);
    }

    [Fact]
    public void NodNeconectat_Null()
    {
        Assert.Null(Rute.CelMaiScurt(MuchiiHartaReale(), 1, 999));
        Assert.Null(Rute.CelMaiScurt([new Rute.Muchie(1, 2, 5)], 2, 1)); // conexiunile sunt orientate
    }
}
