namespace Autogara.Domain.Entitati;

public class TraseuOprire
{
    public int TraseuOprireID { get; set; }
    public int TraseuID { get; set; }
    public int StatieID { get; set; }
    public short Ordine { get; set; }

    public Traseu? Traseu { get; set; }
    public Statie? Statie { get; set; }
}
