namespace Autogara.FileServer.Modele;

/// <summary>Continutul fisierului Autobuze/autobuz_{AutobuzID}.json — asezarea locurilor in autobuz.</summary>
public sealed class AutobuzStructura
{
    public int AutobuzId { get; set; }
    public int Randuri { get; set; }
    public int Coloane { get; set; }

    /// <summary>Culoarul e intre coloana aceasta si urmatoarea; 0 = fara culoar.</summary>
    public int CuloarDupaColoana { get; set; }

    public List<LocStructura> Locuri { get; set; } = [];
}

public sealed class LocStructura
{
    public int NumarLoc { get; set; }
    public int Rand { get; set; }
    public int Coloana { get; set; }
}
