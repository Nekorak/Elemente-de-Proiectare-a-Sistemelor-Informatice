using System.Data;
using Autogara.DataAccess.Vederi;
using Autogara.Domain.Enumerari;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Autogara.DataAccess;

/// <summary>
/// Apelurile catre procedurile stocate din 07_StoredProcedures.sql. Operatiile critice
/// (rezervare, vanzare, anulare) trec numai pe aici, ca logica tranzactionala sa fie cea din SQL.
/// </summary>
public static class ProceduriStocate
{
    public static async Task<(Guid RezervareID, DateTime DataExpirareUtc)> RezervaLocAsync(
        this AutogaraDbContext db, int locId, Guid? utilizatorId, int durataMinute, CancellationToken ct = default)
    {
        var rezervare = Iesire("@RezervareID", SqlDbType.UniqueIdentifier);
        var expirare = Iesire("@DataExpirare", SqlDbType.DateTime2);
        expirare.Scale = 3;

        await db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_RezervaLoc @LocID, @UtilizatorID, @DurataMinute, @RezervareID OUTPUT, @DataExpirare OUTPUT",
            [P("@LocID", locId), P("@UtilizatorID", utilizatorId), P("@DurataMinute", durataMinute), rezervare, expirare],
            ct).ConfigureAwait(false);

        return ((Guid)rezervare.Value, DateTime.SpecifyKind((DateTime)expirare.Value, DateTimeKind.Utc));
    }

    public static async Task<(Guid BiletID, string CodBilet, decimal PretFinal)> ConfirmaVanzareBiletAsync(
        this AutogaraDbContext db, Guid rezervareId, string numePasager, string? telefonPasager, int? tipReducereId,
        Guid vanzutDe, MetodaPlata metoda, string? numarBonFiscal, CancellationToken ct = default)
    {
        var bilet = Iesire("@BiletID", SqlDbType.UniqueIdentifier);
        var cod = Iesire("@CodBilet", SqlDbType.NVarChar, 20);
        var pret = Iesire("@PretFinal", SqlDbType.Decimal);
        pret.Precision = 10;
        pret.Scale = 2;

        await db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_ConfirmaVanzareBilet @RezervareID, @NumePasager, @TelefonPasager, @TipReducereID, " +
            "@VanzutDeUtilizatorID, @MetodaPlata, @NumarBonFiscal, @BiletID OUTPUT, @CodBilet OUTPUT, @PretFinal OUTPUT",
            [
                P("@RezervareID", rezervareId), P("@NumePasager", numePasager), P("@TelefonPasager", telefonPasager),
                P("@TipReducereID", tipReducereId), P("@VanzutDeUtilizatorID", vanzutDe),
                P("@MetodaPlata", ValoriDb.ToDb(metoda)), P("@NumarBonFiscal", numarBonFiscal), bilet, cod, pret,
            ],
            ct).ConfigureAwait(false);

        return ((Guid)bilet.Value, (string)cod.Value, (decimal)pret.Value);
    }

    public static async Task<(decimal SumaRambursata, StatusBilet StatusNou)> AnuleazaBiletAsync(
        this AutogaraDbContext db, Guid biletId, Guid utilizatorId, bool rambursareIntegrala, CancellationToken ct = default)
    {
        var suma = Iesire("@SumaRambursata", SqlDbType.Decimal);
        suma.Precision = 10;
        suma.Scale = 2;
        var status = Iesire("@StatusNou", SqlDbType.NVarChar, 20);

        await db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_AnuleazaBilet @BiletID, @UtilizatorID, @RambursareIntegrala, @SumaRambursata OUTPUT, @StatusNou OUTPUT",
            [P("@BiletID", biletId), P("@UtilizatorID", utilizatorId), P("@RambursareIntegrala", rambursareIntegrala), suma, status],
            ct).ConfigureAwait(false);

        return ((decimal)suma.Value, ValoriDb.FromDb<StatusBilet>((string)status.Value));
    }

    public static async Task<int> ElibereazaRezervariExpirateAsync(this AutogaraDbContext db, CancellationToken ct = default)
    {
        var nr = Iesire("@NrEliberate", SqlDbType.Int);
        await db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_ElibereazaRezervariExpirate @NrEliberate OUTPUT", [nr], ct).ConfigureAwait(false);
        return nr.Value is int n ? n : 0;
    }

    public static async Task<int> CreeazaLocuriPentruCursaAsync(this AutogaraDbContext db, int cursaId, CancellationToken ct = default)
    {
        var nr = Iesire("@LocuriCreate", SqlDbType.Int);
        await db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_CreeazaLocuriPentruCursa @CursaID, @LocuriCreate OUTPUT", [P("@CursaID", cursaId), nr], ct)
            .ConfigureAwait(false);
        return nr.Value is int n ? n : 0;
    }

    public static Task<List<RaportVanzariRand>> RaportVanzariZilnicAsync(
        this AutogaraDbContext db, DateOnly? data, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<RaportVanzariRand>(
                "EXEC autogara.sp_RaportVanzariZilnic @Data",
                P("@Data", data, SqlDbType.Date))
            .ToListAsync(ct);

    public static Task<List<RaportOcupareRand>> RaportOcupareCurseAsync(
        this AutogaraDbContext db, DateOnly start, DateOnly stop, int? traseuId, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<RaportOcupareRand>(
                "EXEC autogara.sp_RaportOcupareCurse @DataStart, @DataStop, @TraseuID",
                P("@DataStart", start, SqlDbType.Date), P("@DataStop", stop, SqlDbType.Date), P("@TraseuID", traseuId))
            .ToListAsync(ct);

    public static async Task<Guid> AdaugaUtilizatorAsync(
        this AutogaraDbContext db, string numeUtilizator, byte[] parolaHash, string nume, string prenume,
        string? email, string? telefon, RolTip rol, Guid? creatDe, CancellationToken ct = default)
    {
        var id = Iesire("@UtilizatorID", SqlDbType.UniqueIdentifier);

        await db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_AdaugaUtilizator @NumeUtilizator, @ParolaHash, @Nume, @Prenume, @Email, @Telefon, " +
            "@Rol, @CreatDeUtilizatorID, @UtilizatorID OUTPUT",
            [
                P("@NumeUtilizator", numeUtilizator), P("@ParolaHash", parolaHash, SqlDbType.VarBinary),
                P("@Nume", nume), P("@Prenume", prenume), P("@Email", email), P("@Telefon", telefon),
                P("@Rol", ValoriDb.ToDb(rol)), P("@CreatDeUtilizatorID", creatDe), id,
            ],
            ct).ConfigureAwait(false);

        return (Guid)id.Value;
    }

    public static Task DezactiveazaUtilizatorAsync(
        this AutogaraDbContext db, Guid utilizatorId, Guid dezactivatDe, CancellationToken ct = default) =>
        db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_DezactiveazaUtilizator @UtilizatorID, @DezactivatDeUtilizatorID",
            [P("@UtilizatorID", utilizatorId), P("@DezactivatDeUtilizatorID", dezactivatDe)], ct);

    public static Task InregistreazaLogAuditAsync(
        this AutogaraDbContext db, Guid? utilizatorId, string actiune, string? entitate, string? entitateId,
        CancellationToken ct = default) =>
        db.Database.ExecuteSqlRawAsync(
            "EXEC autogara.sp_InregistreazaLogAudit @UtilizatorID, @Actiune, @Entitate, @EntitateID",
            [
                P("@UtilizatorID", utilizatorId), P("@Actiune", Taie(actiune, 200)),
                P("@Entitate", entitate), P("@EntitateID", entitateId),
            ],
            ct);

    // ---------- parametri ----------

    private static SqlParameter P(string nume, object? valoare, SqlDbType? tip = null)
    {
        var p = new SqlParameter(nume, valoare switch
        {
            null => DBNull.Value,
            DateOnly d => d.ToDateTime(TimeOnly.MinValue),
            _ => valoare,
        });

        if (tip is { } t)
            p.SqlDbType = t;
        else if (valoare is null)
            p.SqlDbType = SqlDbType.NVarChar; // tipul nu conteaza pentru NULL, dar trebuie sa existe

        return p;
    }

    private static SqlParameter Iesire(string nume, SqlDbType tip, int marime = 0) =>
        new(nume, tip, marime) { Direction = ParameterDirection.Output };

    private static string Taie(string text, int max) => text.Length <= max ? text : text[..max];
}
