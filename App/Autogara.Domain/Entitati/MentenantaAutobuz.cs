namespace Autogara.Domain.Entitati;

public class MentenantaAutobuz
{
    public int MentenantaID { get; set; }
    public int AutobuzID { get; set; }
    public string TipLucrare { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
    public int? Kilometraj { get; set; }
    public string? Observatii { get; set; }
    public DateTime CreatLa { get; set; }

    public Autobuz? Autobuz { get; set; }
}
