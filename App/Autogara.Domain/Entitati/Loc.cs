using Autogara.Domain.Enumerari;

namespace Autogara.Domain.Entitati;

public class Loc
{
    public int LocID { get; set; }
    public int CursaID { get; set; }
    public short NumarLoc { get; set; }
    public StatusLoc Status { get; set; } = StatusLoc.Liber;
    public byte[] RowVersion { get; set; } = [];

    public Cursa? Cursa { get; set; }
}
