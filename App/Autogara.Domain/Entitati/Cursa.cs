using Autogara.Domain.Enumerari;

namespace Autogara.Domain.Entitati;

public class Cursa
{
    public int CursaID { get; set; }
    public int TraseuID { get; set; }
    public int AutobuzID { get; set; }
    public int SoferID { get; set; }
    public DateOnly DataCursa { get; set; }
    public TimeOnly OraPlecare { get; set; }
    public TimeOnly OraSosireEstimata { get; set; }
    public decimal Pret { get; set; }
    public StatusCursa Status { get; set; } = StatusCursa.Planificata;
    public DateTime CreatLa { get; set; }
    public DateTime? ModificatLa { get; set; }

    public Traseu? Traseu { get; set; }
    public Autobuz? Autobuz { get; set; }
    public Sofer? Sofer { get; set; }
    public List<Loc> Locuri { get; set; } = [];

    /// <summary>Momentul plecarii, in ora locala (Moldova).</summary>
    public DateTime MomentPlecare => DataCursa.ToDateTime(OraPlecare);
}
