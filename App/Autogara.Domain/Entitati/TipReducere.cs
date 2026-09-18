namespace Autogara.Domain.Entitati;

public class TipReducere
{
    public int TipReducereID { get; set; }
    public string Denumire { get; set; } = string.Empty;
    public decimal ProcentReducere { get; set; }
    public bool Activ { get; set; } = true;
}
