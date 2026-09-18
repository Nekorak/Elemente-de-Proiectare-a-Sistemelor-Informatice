using System.Text.RegularExpressions;

namespace Autogara.Business.Reguli;

/// <summary>Validari de format comune; fiecare adauga mesajul in lista primita.</summary>
public static partial class Validari
{
    public static void Obligatoriu(List<string> erori, string? valoare, string camp, int maxLungime)
    {
        if (string.IsNullOrWhiteSpace(valoare))
            erori.Add($"{camp} este obligatoriu.");
        else if (valoare.Trim().Length > maxLungime)
            erori.Add($"{camp} poate avea cel mult {maxLungime} caractere.");
    }

    public static void Optional(List<string> erori, string? valoare, string camp, int maxLungime)
    {
        if (valoare is not null && valoare.Trim().Length > maxLungime)
            erori.Add($"{camp} poate avea cel mult {maxLungime} caractere.");
    }

    public static void Telefon(List<string> erori, string? telefon)
    {
        if (string.IsNullOrWhiteSpace(telefon))
            return;
        if (telefon.Trim().Length > 20 || !TelefonRegex().IsMatch(telefon.Trim()))
            erori.Add("Numărul de telefon nu are un format valid (ex. +373 69 123 456).");
    }

    public static void Email(List<string> erori, string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;
        if (email.Trim().Length > 150 || !EmailRegex().IsMatch(email.Trim()))
            erori.Add("Adresa de email nu are un format valid.");
    }

    public static string? Curata(string? text) => string.IsNullOrWhiteSpace(text) ? null : text.Trim();

    [GeneratedRegex(@"^\+?[0-9][0-9 ()\-]{5,18}$")]
    private static partial Regex TelefonRegex();

    // Aceeasi forma ca CK_Utilizatori_Email ('%_@_%._%'), plus fara spatii.
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
