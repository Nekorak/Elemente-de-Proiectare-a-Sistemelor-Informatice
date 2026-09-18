using Autogara.Domain.Enumerari;

namespace Autogara.Domain.Entitati;

public class Plata
{
    public long PlataID { get; set; }
    public Guid BiletID { get; set; }
    public decimal Suma { get; set; }
    public MetodaPlata MetodaPlata { get; set; }

    /// <summary>UTC.</summary>
    public DateTime DataPlata { get; set; }

    public StatusPlata Status { get; set; } = StatusPlata.Finalizata;
    public string? NumarBonFiscal { get; set; }
}
