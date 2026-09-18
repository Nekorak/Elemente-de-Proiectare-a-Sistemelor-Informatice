namespace Autogara.Domain.Entitati;

public class Utilizator
{
    public Guid UtilizatorID { get; set; }
    public string NumeUtilizator { get; set; } = string.Empty;
    public byte[] ParolaHash { get; set; } = [];
    public string Nume { get; set; } = string.Empty;
    public string Prenume { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefon { get; set; }
    public int RolID { get; set; }
    public bool Activ { get; set; } = true;
    public DateTime CreatLa { get; set; }
    public DateTime? ModificatLa { get; set; }
    public byte[] RowVersion { get; set; } = [];

    public Rol? Rol { get; set; }

    public string NumeComplet => $"{Prenume} {Nume}";
}
