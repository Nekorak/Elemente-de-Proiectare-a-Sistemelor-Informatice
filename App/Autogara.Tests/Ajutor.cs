using Autogara.Business;
using Autogara.Domain.Enumerari;

namespace Autogara.Tests;

internal static class Ajutor
{
    /// <summary>Folderul File-Server din repository (datele reale, doar citite de teste).</summary>
    public static string FolderFileServer
    {
        get
        {
            for (var d = new DirectoryInfo(AppContext.BaseDirectory); d is not null; d = d.Parent)
            {
                var cale = Path.Combine(d.FullName, "File-Server");
                if (Directory.Exists(cale))
                    return cale;
            }
            throw new DirectoryNotFoundException("Folderul File-Server nu a fost găsit deasupra folderului testelor.");
        }
    }

    public static string CitesteFisier(string caleRelativa) =>
        File.ReadAllText(Path.Combine(FolderFileServer, caleRelativa));

    public static Sesiune SesiuneCu(RolTip rol)
    {
        var s = new Sesiune();
        s.Porneste(new UtilizatorAutentificat(Guid.NewGuid(), "test." + rol.ToString().ToLowerInvariant(), "Test", "Utilizator", null, rol));
        return s;
    }

    public static string FolderTemporar()
    {
        var cale = Path.Combine(Path.GetTempPath(), "autogara-teste", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(cale);
        return cale;
    }
}
