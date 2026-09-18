namespace Autogara.Domain.Entitati;

public class Statie
{
    public int StatieID { get; set; }
    public int NodID { get; set; }
    public string? Adresa { get; set; }
    public string? Peron { get; set; }
    public DateTime CreatLa { get; set; }
    public DateTime? ModificatLa { get; set; }

    public Nod? Nod { get; set; }
}
