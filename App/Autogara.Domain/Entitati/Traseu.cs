namespace Autogara.Domain.Entitati;

public class Traseu
{
    public int TraseuID { get; set; }
    public string Denumire { get; set; } = string.Empty;
    public bool Activ { get; set; } = true;
    public DateTime CreatLa { get; set; }
    public DateTime? ModificatLa { get; set; }

    public List<TraseuOprire> Opriri { get; set; } = [];
}
