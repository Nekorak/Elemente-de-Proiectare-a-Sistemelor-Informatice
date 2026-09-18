namespace Autogara.Domain.Entitati;

public class RezervareProvizorie
{
    public Guid RezervareID { get; set; }
    public int LocID { get; set; }
    public Guid? UtilizatorID { get; set; }

    /// <summary>UTC.</summary>
    public DateTime DataCreare { get; set; }

    /// <summary>UTC.</summary>
    public DateTime DataExpirare { get; set; }

    public Loc? Loc { get; set; }
}
