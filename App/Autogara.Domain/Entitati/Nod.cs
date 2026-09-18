using Autogara.Domain.Enumerari;

namespace Autogara.Domain.Entitati;

public class Nod
{
    public int NodID { get; set; }
    public TipNod Tip { get; set; }
    public string Nume { get; set; } = string.Empty;
    public double CoordX { get; set; }
    public double CoordY { get; set; }
    public bool Activ { get; set; } = true;
    public DateTime CreatLa { get; set; }
    public DateTime? ModificatLa { get; set; }

    public Statie? Statie { get; set; }
}
