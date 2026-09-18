namespace Autogara.Domain.Entitati;

public class LogAudit
{
    public long LogID { get; set; }
    public Guid? UtilizatorID { get; set; }
    public string Actiune { get; set; } = string.Empty;
    public string? Entitate { get; set; }
    public string? EntitateID { get; set; }

    /// <summary>UTC.</summary>
    public DateTime DataOra { get; set; }
}
