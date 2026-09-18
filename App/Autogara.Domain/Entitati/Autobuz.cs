using Autogara.Domain.Enumerari;

namespace Autogara.Domain.Entitati;

public class Autobuz
{
    public int AutobuzID { get; set; }
    public string NrInmatriculare { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public short CapacitateLocuri { get; set; }
    public StatusAutobuz Status { get; set; } = StatusAutobuz.Activ;

    /// <summary>Calea relativa pe file server, ex. "Autobuze\autobuz_3.json".</summary>
    public string? CaleFisierJSON { get; set; }

    public DateOnly? DataExpirareITP { get; set; }
    public bool Activ { get; set; } = true;
    public DateTime CreatLa { get; set; }
    public DateTime? ModificatLa { get; set; }
}
