using System.Security.Cryptography;
using System.Text;
using Autogara.Business.Securitate;

namespace Autogara.Tests;

public class ParolaHasherTests
{
    [Fact]
    public void HashNou_SeVerificaCuParolaCorecta()
    {
        var hash = ParolaHasher.Hash("Parola2026");

        Assert.Equal(53, hash.Length);
        Assert.Equal((true, false), ParolaHasher.Verifica(hash, "oricine", "Parola2026"));
        Assert.False(ParolaHasher.Verifica(hash, "oricine", "parola2026").Corecta);
    }

    [Fact]
    public void HashNou_AreSareDiferitaLaFiecareApel()
    {
        Assert.NotEqual(ParolaHasher.Hash("Parola2026"), ParolaHasher.Hash("Parola2026"));
    }

    [Fact]
    public void HashDinSeed_EsteAcceptatSiCereRehash()
    {
        // Ca in 11_Seed.sql: HASHBYTES('SHA2_512', CONCAT(NumeUtilizator, N':', Parola)) peste NVARCHAR (UTF-16LE).
        var seed = SHA512.HashData(Encoding.Unicode.GetBytes("tatiana.rusu:Test2026"));

        Assert.Equal(64, seed.Length);
        Assert.Equal((true, true), ParolaHasher.Verifica(seed, "tatiana.rusu", "Test2026"));
        Assert.False(ParolaHasher.Verifica(seed, "tatiana.rusu", "Test2027").Corecta);
        Assert.False(ParolaHasher.Verifica(seed, "ion.munteanu", "Test2026").Corecta);
        Assert.True(ParolaHasher.DepindeDeNumeleUtilizator(seed));
    }

    [Fact]
    public void HashNecunoscut_EsteRespins()
    {
        Assert.Equal((false, false), ParolaHasher.Verifica(new byte[20], "x", "y"));
    }

    [Fact]
    public void HashCuMaiPutineIteratii_CereRehash()
    {
        var hash = ParolaHasher.Hash("Parola2026", iteratii: 1000);
        Assert.Equal((true, true), ParolaHasher.Verifica(hash, "x", "Parola2026"));
    }

    [Theory]
    [InlineData("scurt1", false)]
    [InlineData("faracifre", false)]
    [InlineData("12345678", false)]
    [InlineData("tatiana.rusu2026", false)] // contine numele de utilizator
    [InlineData("Autogara2026", true)]
    public void ValideazaParolaNoua(string parola, bool valida)
    {
        Assert.Equal(valida, ParolaHasher.ValideazaParolaNoua(parola, "tatiana.rusu").Count == 0);
    }

    [Fact]
    public void ParolaTemporara_RespectaRegulile()
    {
        for (var i = 0; i < 200; i++)
            Assert.Empty(ParolaHasher.ValideazaParolaNoua(ParolaHasher.GenereazaParolaTemporara()));
    }
}
