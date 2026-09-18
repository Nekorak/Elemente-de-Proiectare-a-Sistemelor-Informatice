namespace Autogara.DataAccess.Vederi;

// Randurile intoarse de view-urile din 06_Views.sql si de procedurile de raportare.
// Numele proprietatilor = numele coloanelor.

public sealed class CursaActivaRand
{
    public int CursaID { get; set; }
    public DateOnly DataCursa { get; set; }
    public TimeOnly OraPlecare { get; set; }
    public TimeOnly OraSosireEstimata { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Pret { get; set; }
    public int TraseuID { get; set; }
    public string Traseu { get; set; } = string.Empty;
    public int? StatiePlecareID { get; set; }
    public string? StatiePlecare { get; set; }
    public string? PeronPlecare { get; set; }
    public int? StatieSosireID { get; set; }
    public string? StatieSosire { get; set; }
    public int AutobuzID { get; set; }
    public string NrInmatriculare { get; set; } = string.Empty;
    public string ModelAutobuz { get; set; } = string.Empty;
    public int SoferID { get; set; }
    public string Sofer { get; set; } = string.Empty;
    public string? TelefonSofer { get; set; }
    public int LocuriTotal { get; set; }
    public int LocuriLibere { get; set; }
    public int LocuriRezervate { get; set; }
    public int LocuriOcupate { get; set; }
}

public sealed class LocDisponibilRand
{
    public int LocID { get; set; }
    public int CursaID { get; set; }
    public short NumarLoc { get; set; }
    public DateOnly DataCursa { get; set; }
    public TimeOnly OraPlecare { get; set; }
    public TimeOnly OraSosireEstimata { get; set; }
    public decimal Pret { get; set; }
    public int TraseuID { get; set; }
    public string Traseu { get; set; } = string.Empty;
    public int AutobuzID { get; set; }
    public string NrInmatriculare { get; set; } = string.Empty;
    public string? CaleFisierJSON { get; set; }
}

public sealed class VanzareZilnicaRand
{
    public DateOnly Data { get; set; }
    public string MetodaPlata { get; set; } = string.Empty;
    public int BileteVandute { get; set; }
    public int Rambursari { get; set; }
    public decimal SumaIncasata { get; set; }
    public decimal SumaRambursata { get; set; }
    public decimal IncasariNete { get; set; }
}

public sealed class OcupareCursaRand
{
    public int CursaID { get; set; }
    public DateOnly DataCursa { get; set; }
    public TimeOnly OraPlecare { get; set; }
    public string StatusCursa { get; set; } = string.Empty;
    public int TraseuID { get; set; }
    public string Traseu { get; set; } = string.Empty;
    public string NrInmatriculare { get; set; } = string.Empty;
    public short CapacitateLocuri { get; set; }
    public int LocuriOcupate { get; set; }
    public int LocuriRezervate { get; set; }
    public int LocuriLibere { get; set; }
    public decimal? GradOcupareProcent { get; set; }
    public int BileteActive { get; set; }
    public int BileteAnulate { get; set; }
    public decimal Incasari { get; set; }
}

public sealed class RaportComparativRand
{
    public int TraseuID { get; set; }
    public string Traseu { get; set; } = string.Empty;
    public int An { get; set; }
    public int Luna { get; set; }
    public int NrCurse { get; set; }
    public int BileteVandute { get; set; }
    public int? BileteLunaPrecedenta { get; set; }
    public decimal Incasari { get; set; }
    public decimal? IncasariLunaPrecedenta { get; set; }
    public decimal? VariatieIncasariProcent { get; set; }
    public decimal? GradOcupareMediu { get; set; }
    public decimal? OcupareLunaPrecedenta { get; set; }
    public decimal? VariatieOcuparePuncte { get; set; }
}

/// <summary>Rezultatul sp_RaportVanzariZilnic (detaliat pe traseu, casier si metoda de plata).</summary>
public sealed class RaportVanzariRand
{
    public DateOnly Data { get; set; }
    public string Traseu { get; set; } = string.Empty;
    public string Casier { get; set; } = string.Empty;
    public string MetodaPlata { get; set; } = string.Empty;
    public int BileteVandute { get; set; }
    public int Rambursari { get; set; }
    public decimal SumaIncasata { get; set; }
    public decimal SumaRambursata { get; set; }
    public decimal IncasariNete { get; set; }
}

/// <summary>Rezultatul sp_RaportOcupareCurse.</summary>
public sealed class RaportOcupareRand
{
    public int CursaID { get; set; }
    public DateOnly DataCursa { get; set; }
    public TimeOnly OraPlecare { get; set; }
    public string Traseu { get; set; } = string.Empty;
    public string NrInmatriculare { get; set; } = string.Empty;
    public string StatusCursa { get; set; } = string.Empty;
    public short CapacitateLocuri { get; set; }
    public int LocuriOcupate { get; set; }
    public int LocuriRezervate { get; set; }
    public int LocuriLibere { get; set; }
    public decimal? GradOcupareProcent { get; set; }
    public int BileteActive { get; set; }
    public int BileteAnulate { get; set; }
    public decimal Incasari { get; set; }
}
