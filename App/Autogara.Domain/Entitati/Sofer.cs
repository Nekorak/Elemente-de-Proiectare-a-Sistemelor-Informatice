namespace Autogara.Domain.Entitati;

public class Sofer
{
    public int SoferID { get; set; }
    public string Nume { get; set; } = string.Empty;
    public string Prenume { get; set; } = string.Empty;
    public string NrPermis { get; set; } = string.Empty;
    public string? Telefon { get; set; }
    public bool Activ { get; set; } = true;
    public DateTime CreatLa { get; set; }
    public DateTime? ModificatLa { get; set; }

    public string NumeComplet => $"{Prenume} {Nume}";
}
