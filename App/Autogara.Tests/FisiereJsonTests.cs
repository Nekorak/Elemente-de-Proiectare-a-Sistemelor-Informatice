using System.Text.Json;
using Autogara.Business.Servicii;
using Autogara.FileServer;
using Autogara.FileServer.Modele;

namespace Autogara.Tests;

/// <summary>Fisierele reale din File-Server/ trebuie sa treaca validarea folosita de aplicatie.</summary>
public class FisiereJsonTests
{
    private static readonly JsonSerializerOptions Optiuni = new() { PropertyNameCaseInsensitive = true };

    // Capacitatile din 11_Seed.sql (Autobuze.CapacitateLocuri).
    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 20)]
    [InlineData(3, 22)]
    [InlineData(4, 49)]
    [InlineData(5, 49)]
    [InlineData(6, 51)]
    [InlineData(7, 31)]
    [InlineData(8, 18)]
    public void AutobuzeleDinRepository_SuntValide(int autobuzId, int capacitate)
    {
        var s = JsonSerializer.Deserialize<AutobuzStructura>(Ajutor.CitesteFisier($"Autobuze/autobuz_{autobuzId}.json"), Optiuni)!;

        Assert.Equal(autobuzId, s.AutobuzId);
        Assert.Empty(Validare.Autobuz(s, capacitate));
    }

    [Fact]
    public void HartaDinRepository_EsteValida()
    {
        var h = JsonSerializer.Deserialize<HartaFisier>(Ajutor.CitesteFisier("Harta/harta.json"), Optiuni)!;

        Assert.Equal(15, h.Noduri.Count);
        Assert.Equal(30, h.Conexiuni.Count);
        Assert.Empty(Validare.Harta(h));
    }

    [Fact]
    public void Autobuz_CapacitateGresita_EsteRespinsa()
    {
        var s = AutobuzService.StructuraImplicita(20);
        Assert.Contains(Validare.Autobuz(s, 22), e => e.Contains("capacitatea"));
    }

    [Fact]
    public void Autobuz_LocDublat_SiPozitieDublata_SuntRespinse()
    {
        var s = AutobuzService.StructuraImplicita(8);
        s.Locuri[1].NumarLoc = 1;
        s.Locuri[3].Rand = s.Locuri[2].Rand;
        s.Locuri[3].Coloana = s.Locuri[2].Coloana;

        var erori = Validare.Autobuz(s);
        Assert.Contains(erori, e => e.Contains("apare de mai multe ori"));
        Assert.Contains(erori, e => e.Contains("sunt mai multe locuri"));
        Assert.Contains(erori, e => e.Contains("lipsesc: 2"));
    }

    [Fact]
    public void Autobuz_LocInAfaraGrilei_EsteRespins()
    {
        var s = AutobuzService.StructuraImplicita(8);
        s.Locuri[7].Rand = 99;
        Assert.Contains(Validare.Autobuz(s), e => e.Contains("în afara grilei"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(18)]
    [InlineData(49)]
    [InlineData(51)]
    [InlineData(120)]
    public void StructuraImplicita_EsteValida(int capacitate)
    {
        Assert.Empty(Validare.Autobuz(AutobuzService.StructuraImplicita(capacitate), capacitate));
    }

    [Fact]
    public void Harta_ConexiuneCatreNodInexistent_EsteRespinsa()
    {
        var h = new HartaFisier
        {
            Noduri = [new NodHarta { Id = 1, Tip = "Statie", Nume = "A" }],
            Conexiuni = [new ConexiuneHarta { NodPlecareId = 1, NodSosireId = 2, DistantaKm = 5 }],
        };
        Assert.Contains(Validare.Harta(h), e => e.Contains("nod inexistent"));
    }

    [Fact]
    public void Harta_TipNecunoscut_SiDistantaZero_SuntRespinse()
    {
        var h = new HartaFisier
        {
            Noduri = [new NodHarta { Id = 1, Tip = "Gara", Nume = "A" }, new NodHarta { Id = 2, Tip = "Statie", Nume = "B" }],
            Conexiuni = [new ConexiuneHarta { NodPlecareId = 1, NodSosireId = 2, DistantaKm = 0 }],
        };
        var erori = Validare.Harta(h);
        Assert.Contains(erori, e => e.Contains("tip necunoscut"));
        Assert.Contains(erori, e => e.Contains("distanța"));
    }
}
