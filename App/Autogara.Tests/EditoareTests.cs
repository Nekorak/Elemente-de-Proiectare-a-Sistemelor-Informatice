using Autogara.Business.Dto;
using Autogara.Business.Servicii;
using Autogara.FileServer;

namespace Autogara.Tests;

/// <summary>Functiile din backend folosite de editorul de autobuz si de editorul de harta.</summary>
public class EditoareTests
{
    [Fact]
    public void ComutaLoc_ScoateSiAdaugaLocul_SiRenumeroteaza()
    {
        var s = AutobuzService.StructuraImplicita(8);

        AutobuzService.ComutaLoc(s, 1, 2); // scoate locul 2
        Assert.Equal(7, s.Locuri.Count);
        Assert.Empty(Validare.Autobuz(s, 7));
        Assert.Equal((1, 3), (s.Locuri[1].Rand, s.Locuri[1].Coloana)); // fostul 3 devine 2

        AutobuzService.ComutaLoc(s, 1, 2); // il pune la loc
        Assert.Equal(8, s.Locuri.Count);
        Assert.Equal(Enumerable.Range(1, 8), s.Locuri.Select(l => l.NumarLoc));
    }

    [Fact]
    public void ComutaLoc_InAfaraGrilei_NuSchimbaNimic()
    {
        var s = AutobuzService.StructuraImplicita(8);
        AutobuzService.ComutaLoc(s, 99, 1);
        Assert.Equal(8, s.Locuri.Count);
    }

    [Fact]
    public void Redimensioneaza_MicsoreazaSiMaresteGrila()
    {
        var s = AutobuzService.StructuraImplicita(16); // 4 x 4

        AutobuzService.Redimensioneaza(s, 3, 4, 2);
        Assert.Equal(12, s.Locuri.Count);
        Assert.Empty(Validare.Autobuz(s, 12));

        AutobuzService.Redimensioneaza(s, 3, 5, 9);
        Assert.Equal(15, s.Locuri.Count);
        Assert.Equal(0, s.CuloarDupaColoana); // culoar invalid -> fara culoar
        Assert.Empty(Validare.Autobuz(s, 15));
    }

    [Fact]
    public void Redimensioneaza_GrilaGoala_EsteRespinsa()
    {
        Assert.Throws<Common.ValidareException>(() => AutobuzService.Redimensioneaza(AutobuzService.StructuraImplicita(4), 0, 4, 2));
    }

    [Fact]
    public void EstimeazaDistanta_FolosesteScaraHartii()
    {
        var h = new HartaDto
        {
            Noduri =
            [
                new NodDto { NodID = 1, X = 0, Y = 0 },
                new NodDto { NodID = 2, X = 100, Y = 0 },
                new NodDto { NodID = 3, X = 0, Y = 40 },
            ],
            Conexiuni = [new ConexiuneDto { NodPlecareID = 1, NodSosireID = 2, DistantaKm = 50 }], // 0,5 km pe unitate
        };

        Assert.Equal(20m, HartaService.EstimeazaDistantaKm(h, 1, 3));
        Assert.Equal(1m, HartaService.EstimeazaDistantaKm(h, 1, 99)); // nod necunoscut
    }
}
