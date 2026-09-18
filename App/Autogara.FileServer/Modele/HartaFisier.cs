namespace Autogara.FileServer.Modele;

/// <summary>
/// Continutul fisierului Harta/harta.json — oglinda tabelelor Noduri si Conexiuni.
/// Sursa de adevar ramane baza de date.
/// </summary>
public sealed class HartaFisier
{
    public List<NodHarta> Noduri { get; set; } = [];
    public List<ConexiuneHarta> Conexiuni { get; set; } = [];
}

public sealed class NodHarta
{
    /// <summary>NodID din baza de date.</summary>
    public int Id { get; set; }

    /// <summary>"Statie" sau "Intersectie".</summary>
    public string Tip { get; set; } = string.Empty;

    public string Nume { get; set; } = string.Empty;
    public double X { get; set; }
    public double Y { get; set; }
}

public sealed class ConexiuneHarta
{
    public int NodPlecareId { get; set; }
    public int NodSosireId { get; set; }
    public decimal DistantaKm { get; set; }
}
