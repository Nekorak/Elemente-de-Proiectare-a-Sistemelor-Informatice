using Autogara.Domain.Enumerari;

namespace Autogara.Business.Dto;

// Obiectele schimbate intre UI si servicii. Datele de intrare sunt clase cu set (se leaga usor
// de controale); rezultatele sunt record-uri.

// ---------- utilizatori ----------

public sealed class UtilizatorEditare
{
    public string NumeUtilizator { get; set; } = string.Empty;
    public string Nume { get; set; } = string.Empty;
    public string Prenume { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefon { get; set; }
    public RolTip Rol { get; set; } = RolTip.Casier;
}

public sealed record UtilizatorRand(
    Guid UtilizatorID, string NumeUtilizator, string Nume, string Prenume, string? Email, string? Telefon,
    RolTip Rol, bool Activ, DateTime CreatLa, byte[] RowVersion)
{
    public string NumeComplet => $"{Prenume} {Nume}";
}

public sealed record CerereResetareParola(Guid? UtilizatorID, string NumeUtilizator, DateTime DataCerere);

// ---------- harta ----------

public sealed class HartaDto
{
    public List<NodDto> Noduri { get; set; } = [];
    public List<ConexiuneDto> Conexiuni { get; set; } = [];
}

public sealed class NodDto
{
    /// <summary>NodID existent, sau un numar negativ temporar pentru un nod nou din editor.</summary>
    public int NodID { get; set; }

    public TipNod Tip { get; set; }
    public string Nume { get; set; } = string.Empty;
    public double X { get; set; }
    public double Y { get; set; }

    /// <summary>Completat la incarcare pentru nodurile de tip Statie.</summary>
    public int? StatieID { get; set; }
}

public sealed class ConexiuneDto
{
    public int NodPlecareID { get; set; }
    public int NodSosireID { get; set; }
    public decimal DistantaKm { get; set; }
}

public sealed record RezultatSalvareHarta(
    IReadOnlyDictionary<int, int> NoduriNoi,
    int NoduriDezactivate,
    bool FisierActualizat,
    string? AvertismentFisier);

public sealed record RutaDto(IReadOnlyList<int> NodIDs, IReadOnlyList<string> NumeNoduri, decimal DistantaKm);

// ---------- statii, trasee ----------

public sealed record StatieRand(int StatieID, int NodID, string Nume, string? Adresa, string? Peron, bool Activ);

public sealed record TraseuRand(int TraseuID, string Denumire, bool Activ, int NrOpriri, string? Plecare, string? Sosire);

public sealed record OprireRand(short Ordine, int StatieID, string Statie, decimal? DistantaDePrecedentaKm);

public sealed record TraseuDetalii(int TraseuID, string Denumire, bool Activ, IReadOnlyList<OprireRand> Opriri, decimal? DistantaTotalaKm);

// ---------- autobuze, soferi, mentenanta ----------

public sealed class AutobuzEditare
{
    public string NrInmatriculare { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateOnly? DataExpirareITP { get; set; }
    public StatusAutobuz Status { get; set; } = StatusAutobuz.Activ;
}

public sealed record AutobuzRand(
    int AutobuzID, string NrInmatriculare, string Model, short CapacitateLocuri, StatusAutobuz Status,
    string? CaleFisierJSON, DateOnly? DataExpirareITP, bool Activ);

public sealed class SoferEditare
{
    public string Nume { get; set; } = string.Empty;
    public string Prenume { get; set; } = string.Empty;
    public string NrPermis { get; set; } = string.Empty;
    public string? Telefon { get; set; }
}

public sealed record SoferRand(int SoferID, string Nume, string Prenume, string NrPermis, string? Telefon, bool Activ)
{
    public string NumeComplet => $"{Prenume} {Nume}";
}

public sealed class MentenantaEditare
{
    public int AutobuzID { get; set; }
    public string TipLucrare { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
    public int? Kilometraj { get; set; }
    public string? Observatii { get; set; }
}

public sealed record MentenantaRand(
    int MentenantaID, int AutobuzID, string NrInmatriculare, string TipLucrare, DateOnly Data, int? Kilometraj, string? Observatii);

public sealed record ItpRand(int AutobuzID, string NrInmatriculare, string Model, StatusAutobuz Status, DateOnly? DataExpirareITP, int? ZileRamase)
{
    public bool Expirat => ZileRamase is < 0;
}

public sealed class TipReducereEditare
{
    public string Denumire { get; set; } = string.Empty;
    public decimal ProcentReducere { get; set; }
}

public sealed record TipReducereRand(int TipReducereID, string Denumire, decimal ProcentReducere, bool Activ);

// ---------- curse si locuri ----------

public sealed class CursaEditare
{
    public int TraseuID { get; set; }
    public int AutobuzID { get; set; }
    public int SoferID { get; set; }
    public DateOnly DataCursa { get; set; }
    public TimeOnly OraPlecare { get; set; }
    public TimeOnly OraSosireEstimata { get; set; }
    public decimal Pret { get; set; }
}

public sealed record CursaRand(
    int CursaID, DateOnly DataCursa, TimeOnly OraPlecare, TimeOnly OraSosireEstimata, decimal Pret, StatusCursa Status,
    int TraseuID, string Traseu, int AutobuzID, string NrInmatriculare, int SoferID, string Sofer,
    int LocuriTotal, int LocuriLibere, int LocuriOcupate);

/// <summary>Rezultatul cautarii: portiunea ceruta din traseu (statia de urcare → statia de coborare).</summary>
public sealed record CursaGasita(
    int CursaID, string Traseu, DateOnly DataCursa, TimeOnly OraPlecare, TimeOnly OraSosireEstimata, decimal Pret,
    StatusCursa Status, string? StatiePlecare, string? PeronPlecare, string? StatieSosire,
    string NrInmatriculare, string ModelAutobuz, int LocuriLibere, bool DisponibilaPentruVanzare);

public sealed record HartaLocuri(
    int CursaID, int AutobuzID, int Randuri, int Coloane, int CuloarDupaColoana,
    IReadOnlyList<LocHarta> Locuri, bool StructuraDinFisier);

public sealed record LocHarta(int LocID, short NumarLoc, int Rand, int Coloana, StatusLoc Status, bool RezervatDeMine);

// ---------- rezervari si bilete ----------

public sealed record RezervareDto(Guid RezervareID, int LocID, DateTime ExpiraLaUtc, int DurataMinute)
{
    /// <summary>Secundele ramase, dupa ceasul statiei (pentru cronometrul din UI).</summary>
    public int SecundeRamase => Math.Max(0, (int)(ExpiraLaUtc - DateTime.UtcNow).TotalSeconds);
}

public sealed class VanzareDto
{
    public string NumePasager { get; set; } = string.Empty;
    public string? TelefonPasager { get; set; }
    public int? TipReducereID { get; set; }
    public MetodaPlata MetodaPlata { get; set; } = MetodaPlata.Numerar;

    /// <summary>Numarul bonului de la casa de marcat, daca exista.</summary>
    public string? NumarBonFiscal { get; set; }
}

public sealed record PretDto(decimal PretBaza, string? Reducere, decimal ProcentReducere, decimal PretFinal);

public sealed record BiletEmis(Guid BiletID, string CodBilet, decimal Pret, int CursaID, short NumarLoc);

public sealed record BiletDetalii(
    Guid BiletID, string CodBilet, StatusBilet Status, string NumePasager, string? TelefonPasager,
    decimal Pret, string? Reducere, DateTime DataEmitereLocal, string VandutDe,
    int CursaID, string Traseu, DateOnly DataCursa, TimeOnly OraPlecare, StatusCursa StatusCursa,
    short NumarLoc, string NrInmatriculare, MetodaPlata? MetodaPlata);

public sealed record EstimareAnulare(decimal Pret, decimal ProcentRambursare, decimal SumaRambursata, bool PoateFiAnulat, string Explicatie);

public sealed record RezultatAnulare(string CodBilet, decimal SumaRambursata, StatusBilet StatusNou);

public sealed record RezultatAnulareCursa(int BileteRambursate, decimal SumaTotalaRambursata, int RezervariEliberate);

// ---------- dashboard, audit, conexiune ----------

public sealed record DashboardDto(
    DateTime GeneratLa,
    int CurseAzi,
    int CurseInDesfasurare,
    int CurseRamaseAzi,
    int BileteVanduteAzi,
    int BileteAnulateAzi,
    decimal IncasariNeteAzi,
    int RezervariActive,
    int AutobuzeItpInUrmatoarele30Zile,
    IReadOnlyList<Autogara.DataAccess.Vederi.CursaActivaRand> UrmatoareleCurse);

public sealed record LogAuditRand(long LogID, DateTime DataOraLocal, string? Utilizator, string Actiune, string? Entitate, string? EntitateID);

public sealed class FiltruAudit
{
    public DateOnly? DeLa { get; set; }
    public DateOnly? PanaLa { get; set; }
    public Guid? UtilizatorID { get; set; }
    public string? Text { get; set; }
    public int MaxRanduri { get; set; } = 500;
}

public sealed record StareConexiune(bool BazaDeDate, bool FileServer, DateTime VerificatLa)
{
    public bool Online => BazaDeDate && FileServer;

    /// <summary>Fara baza de date aplicatia lucreaza doar cu datele din cache (fara vanzari).</summary>
    public bool ModLocal => !BazaDeDate;
}

public sealed record RezultatTestConfigurare(bool BazaDeDate, string MesajBazaDeDate, bool FileServer, string MesajFileServer)
{
    public bool Reusit => BazaDeDate && FileServer;
}

public sealed record InfoActualizare(string Versiune, string Fisier, string? Note);
