namespace Autogara.Domain.Entitati;

/// <summary>Drum orientat intre doua noduri; in baza de date fiecare drum apare in ambele sensuri.</summary>
public class Conexiune
{
    public int ConexiuneID { get; set; }
    public int NodPlecareID { get; set; }
    public int NodSosireID { get; set; }
    public decimal DistantaKm { get; set; }
    public DateTime CreatLa { get; set; }

    public Nod? NodPlecare { get; set; }
    public Nod? NodSosire { get; set; }
}
