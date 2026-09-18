using System.Net.Sockets;
using Autogara.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Autogara.DataAccess;

/// <summary>
/// Traduce exceptiile SQL/EF in exceptiile aplicatiei (Autogara.Common), cu mesaje pentru utilizator.
/// Erorile THROW 500xx din procedurile stocate au deja mesajul in romana si se transmit ca atare.
/// </summary>
public static class EroriSql
{
    private static readonly HashSet<int> EroriRetea =
        [-2, -1, 2, 53, 64, 121, 233, 258, 1231, 10053, 10054, 10060, 10061, 11001, 11004, 40613];

    public static Exception Traduce(Exception ex)
    {
        switch (ex)
        {
            case AutogaraException or OperationCanceledException:
                return ex;

            case DbUpdateConcurrencyException:
                return new ConcurentaException(ex);

            case RetryLimitExceededException { InnerException: { } interior }:
                return Traduce(interior);

            case DbUpdateException { InnerException: SqlException sql }:
                return Traduce(sql);

            case SqlException sql:
                return DinSql(sql);

            case InvalidOperationException { InnerException: SqlException sql }:
                return DinSql(sql);

            case SocketException or TimeoutException:
                return new ConexiuneException("Serverul bazei de date nu poate fi contactat. Verificați conexiunea la internet.", ex);

            default:
                return ex;
        }
    }

    private static Exception DinSql(SqlException sql)
    {
        var nr = sql.Number;

        if (nr is >= 50000 and < 51000)
            return new RegulaException(sql.Message, nr, sql);

        if (EroriRetea.Contains(nr))
            return new ConexiuneException("Serverul bazei de date nu poate fi contactat. Verificați conexiunea la internet.", sql);

        return nr switch
        {
            18456 => new ConexiuneException("Autentificarea la serverul bazei de date a eșuat. Verificați configurarea stației.", sql),
            4060 => new ConexiuneException("Baza de date „autogara” nu este disponibilă pe server.", sql),
            2627 or 2601 => new RegulaException(MesajUnicitate(sql.Message), nr, sql),
            547 => new RegulaException(
                "Operația nu este permisă: înregistrarea este folosită în altă parte sau o valoare nu respectă regulile bazei de date.",
                nr, sql),
            1205 => new RegulaException("Operația a intrat în conflict cu altă operație simultană. Încercați din nou.", nr, sql),
            229 or 230 => new AccesInterzisException("Contul aplicației nu are drept pentru această operație în baza de date."),
            _ => sql,
        };
    }

    private static string MesajUnicitate(string mesajSql)
    {
        foreach (var (constrangere, mesaj) in MesajeUnicitate)
            if (mesajSql.Contains(constrangere, StringComparison.OrdinalIgnoreCase))
                return mesaj;

        return "Există deja o înregistrare cu aceeași valoare.";
    }

    private static readonly (string, string)[] MesajeUnicitate =
    [
        ("UQ_Utilizatori_NumeUtilizator", "Numele de utilizator este deja folosit."),
        ("UQ_Autobuze_NrInmatriculare", "Există deja un autobuz cu acest număr de înmatriculare."),
        ("UQ_Soferi_NrPermis", "Există deja un șofer cu acest număr de permis."),
        ("UQ_Trasee_Denumire", "Există deja un traseu cu această denumire."),
        ("UQ_TipuriReducere_Denumire", "Există deja un tip de reducere cu această denumire."),
        ("UQ_Statii_NodID", "Nodul ales are deja o stație asociată."),
        ("UQ_Conexiuni_Noduri", "Există deja o conexiune între aceste noduri."),
        ("UQ_TraseuOpriri", "O stație apare de două ori în traseu."),
        ("UX_Bilete_LocID_Activ", "Pe acest loc există deja un bilet activ."),
        ("UQ_RezervariProvizorii_LocID", "Locul este deja rezervat."),
    ];
}
