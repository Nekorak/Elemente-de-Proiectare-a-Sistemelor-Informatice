namespace Autogara.Common;

/// <summary>
/// Eroare cu mesaj gata de afisat utilizatorului. Toate celelalte exceptii din aplicatie
/// se traduc in una din acestea inainte sa ajunga in UI.
/// </summary>
public class AutogaraException : Exception
{
    public AutogaraException(string mesaj, Exception? cauza = null) : base(mesaj, cauza) { }
}

/// <summary>Regula de business incalcata (locul e ocupat, rezervarea a expirat etc.).</summary>
public class RegulaException : AutogaraException
{
    public RegulaException(string mesaj, int? codSql = null, Exception? cauza = null) : base(mesaj, cauza) =>
        CodSql = codSql;

    /// <summary>Numarul erorii THROW 500xx din procedura stocata, daca de acolo vine.</summary>
    public int? CodSql { get; }
}

/// <summary>Date introduse invalid; contine toate erorile, nu doar prima.</summary>
public class ValidareException : AutogaraException
{
    public ValidareException(IEnumerable<string> erori) : this(erori.ToList()) { }

    public ValidareException(string eroare) : this([eroare]) { }

    private ValidareException(List<string> erori) : base(string.Join(Environment.NewLine, erori)) => Erori = erori;

    public IReadOnlyList<string> Erori { get; }

    public static void AruncaDaca(IReadOnlyCollection<string> erori)
    {
        if (erori.Count > 0)
            throw new ValidareException(erori);
    }
}

/// <summary>Serverul SQL sau file server-ul nu raspunde.</summary>
public class ConexiuneException : AutogaraException
{
    public ConexiuneException(string mesaj, Exception? cauza = null) : base(mesaj, cauza) { }
}

/// <summary>Altcineva a modificat aceeasi inregistrare intre timp (RowVersion diferit).</summary>
public class ConcurentaException : AutogaraException
{
    public ConcurentaException(Exception? cauza = null)
        : base("Înregistrarea a fost modificată de alt utilizator între timp. Reîncărcați datele și încercați din nou.", cauza) { }
}

/// <summary>Utilizatorul curent nu are rolul necesar pentru operatie.</summary>
public class AccesInterzisException : AutogaraException
{
    public AccesInterzisException(string mesaj = "Nu aveți drepturi pentru această operație.") : base(mesaj) { }
}

public class NegasitException : AutogaraException
{
    public NegasitException(string mesaj) : base(mesaj) { }
}
