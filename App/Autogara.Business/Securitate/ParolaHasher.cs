using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Autogara.Business.Securitate;

/// <summary>
/// Hash-ul parolelor pentru coloana Utilizatori.ParolaHash (VARBINARY(256)).
///
/// Format curent (53 octeti): [0x01][iteratii, 4 octeti big-endian][sare, 16][PBKDF2-SHA256, 32].
///
/// Format vechi (64 octeti): SHA2_512 peste NVARCHAR "NumeUtilizator:Parola" — asa sunt create
/// conturile din 11_Seed.sql. E acceptat la autentificare si inlocuit automat cu formatul curent.
/// </summary>
public static class ParolaHasher
{
    private const byte Versiune = 0x01;
    private const int LungimeSare = 16;
    private const int LungimeHash = 32;
    private const int LungimeTotala = 1 + 4 + LungimeSare + LungimeHash;
    private const int LungimeSeed = 64;

    public const int IteratiiImplicite = 210_000;

    public static byte[] Hash(string parola, int iteratii = IteratiiImplicite)
    {
        var sare = RandomNumberGenerator.GetBytes(LungimeSare);
        var hash = Rfc2898DeriveBytes.Pbkdf2(parola, sare, iteratii, HashAlgorithmName.SHA256, LungimeHash);

        var rezultat = new byte[LungimeTotala];
        rezultat[0] = Versiune;
        BinaryPrimitives.WriteInt32BigEndian(rezultat.AsSpan(1, 4), iteratii);
        sare.CopyTo(rezultat, 5);
        hash.CopyTo(rezultat, 5 + LungimeSare);
        return rezultat;
    }

    /// <param name="numeUtilizator">Exact cum e salvat in baza de date (intra in hash-ul vechi).</param>
    /// <returns>Corecta = parola se potriveste; TrebuieRehash = hash in format vechi, de inlocuit.</returns>
    public static (bool Corecta, bool TrebuieRehash) Verifica(byte[] hashSalvat, string numeUtilizator, string parola)
    {
        if (hashSalvat.Length == LungimeTotala && hashSalvat[0] == Versiune)
        {
            var iteratii = BinaryPrimitives.ReadInt32BigEndian(hashSalvat.AsSpan(1, 4));
            var sare = hashSalvat.AsSpan(5, LungimeSare);
            var asteptat = hashSalvat.AsSpan(5 + LungimeSare, LungimeHash);
            var calculat = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(parola), sare, iteratii, HashAlgorithmName.SHA256, LungimeHash);

            var corecta = CryptographicOperations.FixedTimeEquals(calculat, asteptat);
            return (corecta, corecta && iteratii < IteratiiImplicite);
        }

        if (hashSalvat.Length == LungimeSeed)
        {
            var corecta = CryptographicOperations.FixedTimeEquals(HashSeed(numeUtilizator, parola), hashSalvat);
            return (corecta, corecta);
        }

        return (false, false);
    }

    /// <summary>Hash-ul vechi include numele de utilizator: redenumirea contului l-ar invalida.</summary>
    public static bool DepindeDeNumeleUtilizator(byte[] hashSalvat) => hashSalvat.Length == LungimeSeed;

    /// <summary>Echivalentul HASHBYTES('SHA2_512', CONCAT(NumeUtilizator, N':', Parola)) — NVARCHAR = UTF-16LE.</summary>
    internal static byte[] HashSeed(string numeUtilizator, string parola) =>
        SHA512.HashData(Encoding.Unicode.GetBytes($"{numeUtilizator}:{parola}"));

    /// <summary>Regulile pentru o parola noua; lista goala = parola acceptata.</summary>
    public static List<string> ValideazaParolaNoua(string? parola, string? numeUtilizator = null)
    {
        var erori = new List<string>();
        if (string.IsNullOrEmpty(parola))
        {
            erori.Add("Parola este obligatorie.");
            return erori;
        }

        if (parola.Length < 8) erori.Add("Parola trebuie să aibă cel puțin 8 caractere.");
        if (parola.Length > 128) erori.Add("Parola poate avea cel mult 128 de caractere.");
        if (!parola.Any(char.IsLetter)) erori.Add("Parola trebuie să conțină cel puțin o literă.");
        if (!parola.Any(char.IsDigit)) erori.Add("Parola trebuie să conțină cel puțin o cifră.");
        if (numeUtilizator is not null && parola.Contains(numeUtilizator, StringComparison.OrdinalIgnoreCase))
            erori.Add("Parola nu poate conține numele de utilizator.");
        return erori;
    }

    /// <summary>Parola temporara la resetarea de catre administrator (fara caractere ambigue: 0/O, 1/l/I).</summary>
    public static string GenereazaParolaTemporara(int lungime = 10)
    {
        const string litere = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        const string cifre = "23456789";
        const string toate = litere + cifre;

        var caractere = new char[lungime];
        caractere[0] = litere[RandomNumberGenerator.GetInt32(litere.Length)];
        caractere[1] = cifre[RandomNumberGenerator.GetInt32(cifre.Length)];
        for (var i = 2; i < lungime; i++)
            caractere[i] = toate[RandomNumberGenerator.GetInt32(toate.Length)];

        RandomNumberGenerator.Shuffle(caractere.AsSpan());
        return new string(caractere);
    }
}
