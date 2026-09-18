using System.Text;
using Autogara.Business.Servicii;
using Autogara.Common;
using Autogara.Common.Configurare;
using Autogara.DataAccess;
using Autogara.DataAccess.Vederi;
using Autogara.Domain.Entitati;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Tests;

public class ValoriDbTests
{
    [Fact]
    public void TextelePentruSql_SuntCeleDinCheckConstraints()
    {
        Assert.Equal("In desfasurare", ValoriDb.ToDb(StatusCursa.InDesfasurare));
        Assert.Equal("Scos din uz", ValoriDb.ToDb(StatusAutobuz.ScosDinUz));
        Assert.Equal(StatusCursa.InDesfasurare, ValoriDb.FromDb<StatusCursa>("In desfasurare"));
        Assert.Throws<ArgumentException>(() => ValoriDb.FromDb<StatusLoc>("Blocat"));
    }

    [Fact]
    public void ToateEnumurile_FacDusIntors()
    {
        static void Verifica<T>() where T : struct, Enum
        {
            foreach (var v in Enum.GetValues<T>())
                Assert.Equal(v, ValoriDb.FromDb<T>(ValoriDb.ToDb(v)));
        }

        Verifica<RolTip>();
        Verifica<StatusLoc>();
        Verifica<StatusBilet>();
        Verifica<StatusCursa>();
        Verifica<StatusPlata>();
        Verifica<MetodaPlata>();
        Verifica<StatusAutobuz>();
        Verifica<TipNod>();
    }
}

public class SesiuneTests
{
    [Fact]
    public void FaraAutentificare_OperatiileSuntRefuzate()
    {
        Assert.Throws<AccesInterzisException>(() => new Business.Sesiune().CereAutentificare());
    }

    [Fact]
    public void Casier_NuPoateFaceOperatiiDeAdmin()
    {
        var s = Ajutor.SesiuneCu(RolTip.Casier);
        Assert.Throws<AccesInterzisException>(() => s.CereAdmin());
        Assert.Equal(RolTip.Casier, s.CerePersonal().Rol);
    }

    [Fact]
    public void Pasager_NuEstePersonal()
    {
        Assert.Throws<AccesInterzisException>(() => Ajutor.SesiuneCu(RolTip.Pasager).CerePersonal());
    }
}

public class ConfigurareTests
{
    [Fact]
    public void Setarile_SeSalveazaCuParoleCriptate()
    {
        var cale = Path.Combine(Ajutor.FolderTemporar(), "appsettings.local.json");
        var store = new AppSettingsStore(cale);
        var setari = AppSettings.Implicite();

        store.Salveaza(setari);
        var continut = File.ReadAllText(cale);
        var citite = store.Citeste();

        Assert.DoesNotContain(setari.Sql.Parola, continut);
        Assert.DoesNotContain(setari.FileServer.Parola, continut);
        Assert.Equal(setari.Sql.Parola, citite.Sql.Parola);
        Assert.Equal(setari.FileServer.Parola, citite.FileServer.Parola);
        Assert.Equal(setari.FileServer.Utilizator, citite.FileServer.Utilizator);
        Assert.Empty(citite.Valideaza());
    }

    [Fact]
    public void FaraFisier_CereConfigurare()
    {
        var store = new AppSettingsStore(Path.Combine(Ajutor.FolderTemporar(), "lipsa.json"));
        Assert.False(store.Exista);
        Assert.Throws<ConfigurareLipsaException>(() => store.Citeste());
    }

    [Fact]
    public void ConnectionString_ArePortulSiCriptarea()
    {
        var cs = AppSettings.Implicite().ConnectionString();
        Assert.Contains("Server=188.237.68.25,14330", cs);
        Assert.Contains("Database=autogara", cs);
        Assert.Contains("User Id=autogara_app", cs);
        Assert.Contains("Encrypt=True", cs);
    }
}

public class CacheLocalTests
{
    [Fact]
    public async Task ServerIndisponibil_IntoarceUltimaCopie()
    {
        var cache = new CacheLocalService(Ajutor.FolderTemporar());
        string? folosit = null;
        cache.DateDinCacheFolosite += (cheie, _) => folosit = cheie;

        var online = await cache.ObtineAsync("reduceri", () => Task.FromResult(new List<string> { "Elev", "Pensionar" }));
        var offline = await cache.ObtineAsync<List<string>>("reduceri", () => throw new ConexiuneException("jos"));

        Assert.Equal(online, offline);
        Assert.Equal("reduceri", folosit);
    }

    [Fact]
    public async Task FaraCopie_EroareaMergeMaiDeparte()
    {
        var cache = new CacheLocalService(Ajutor.FolderTemporar());
        await Assert.ThrowsAsync<ConexiuneException>(() =>
            cache.ObtineAsync<List<string>>("nimic", () => throw new ConexiuneException("jos")));
    }

    [Fact]
    public async Task AlteErori_NuFolosescCacheul()
    {
        var cache = new CacheLocalService(Ajutor.FolderTemporar());
        await cache.ObtineAsync("x", () => Task.FromResult(1));
        await Assert.ThrowsAsync<RegulaException>(() => cache.ObtineAsync<int>("x", () => throw new RegulaException("regula")));
    }
}

public class RaportCsvTests
{
    [Fact]
    public void Csv_AreBomAntetSiSeparatorPunctSiVirgula()
    {
        var randuri = new[]
        {
            new VanzareZilnicaRand { Data = new DateOnly(2026, 9, 18), MetodaPlata = "Numerar", BileteVandute = 3, SumaIncasata = 360.5m, IncasariNete = 360.5m },
            new VanzareZilnicaRand { Data = new DateOnly(2026, 9, 18), MetodaPlata = "Card; \"test\"", BileteVandute = 1 },
        };

        var octeti = RaportService.ExportCsv(randuri);
        Assert.Equal(Encoding.UTF8.GetPreamble(), octeti[..3]);

        var linii = Encoding.UTF8.GetString(octeti[3..]).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal("Data;MetodaPlata;BileteVandute;Rambursari;SumaIncasata;SumaRambursata;IncasariNete", linii[0]);
        Assert.Equal("18.09.2026;Numerar;3;0;360,50;0,00;360,50", linii[1]);
        Assert.StartsWith("18.09.2026;\"Card; \"\"test\"\"\";1;", linii[2]);
    }
}

public class ModelEfTests
{
    [Fact]
    public void Modelul_SeConstruiesteSiFolosesteSchemaAutogara()
    {
        using var db = new BazaDeDate("Server=127.0.0.1,1;Database=autogara;User Id=x;Password=y;").CreeazaContext();

        var tabele = db.Model.GetEntityTypes()
            .Where(t => t.GetTableName() is not null)
            .ToDictionary(t => t.ClrType, t => (t.GetSchema(), t.GetTableName()));

        Assert.Equal(("autogara", "Utilizatori"), tabele[typeof(Utilizator)]);
        Assert.Equal(("autogara", "MentenantaAutobuze"), tabele[typeof(MentenantaAutobuz)]);
        Assert.Equal(("autogara", "RezervariProvizorii"), tabele[typeof(RezervareProvizorie)]);
        Assert.Equal("vw_CurseActive", db.Model.FindEntityType(typeof(CursaActivaRand))!.GetViewName());

        var status = db.Model.FindEntityType(typeof(Cursa))!.FindProperty(nameof(Cursa.Status))!;
        Assert.Equal("In desfasurare", status.GetValueConverter()!.ConvertToProvider(StatusCursa.InDesfasurare));

        Assert.True(db.Model.FindEntityType(typeof(Loc))!.FindProperty(nameof(Loc.RowVersion))!.IsConcurrencyToken);
    }

    [Fact]
    public void Interogare_SeTraduceInSqlCuValorileDinCheck()
    {
        using var db = new BazaDeDate("Server=127.0.0.1,1;Database=autogara;User Id=x;Password=y;").CreeazaContext();

        var sql = db.Curse.Where(c => c.Status == StatusCursa.InDesfasurare).ToQueryString();

        Assert.Contains("[autogara].[Curse]", sql);
        Assert.Contains("N'In desfasurare'", sql);
    }
}
