using Autogara.Domain.Enumerari;

namespace Autogara.Domain.Entitati;

public class Bilet
{
    public Guid BiletID { get; set; }
    public string CodBilet { get; set; } = string.Empty;
    public int CursaID { get; set; }
    public int LocID { get; set; }
    public string NumePasager { get; set; } = string.Empty;
    public string? TelefonPasager { get; set; }
    public int? TipReducereID { get; set; }
    public Guid VanzutDeUtilizatorID { get; set; }

    /// <summary>UTC.</summary>
    public DateTime DataEmitere { get; set; }

    public decimal Pret { get; set; }
    public StatusBilet Status { get; set; } = StatusBilet.Activ;
    public DateTime? ModificatLa { get; set; }
    public byte[] RowVersion { get; set; } = [];

    public Cursa? Cursa { get; set; }
    public Loc? Loc { get; set; }
    public TipReducere? TipReducere { get; set; }
    public Utilizator? VanzutDe { get; set; }
    public List<Plata> Plati { get; set; } = [];
}
