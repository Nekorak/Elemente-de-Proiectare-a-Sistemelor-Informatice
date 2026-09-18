using System.Text.Encodings.Web;
using System.Text.Json;
using Autogara.Common;

namespace Autogara.FileServer;

/// <summary>Setarile comune de serializare pentru fisierele de pe file server.</summary>
internal static class JsonFisiere
{
    public static readonly JsonSerializerOptions Optiuni = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        // Diacriticele raman lizibile in fisier ("Chișinău", nu "Chișinău").
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static T Deserializeaza<T>(string json, string cale)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, Optiuni)
                   ?? throw new ValidareException($"Fișierul {cale} este gol.");
        }
        catch (JsonException ex)
        {
            throw new ValidareException($"Fișierul {cale} nu este un JSON valid: {ex.Message}");
        }
    }

    public static string Serializeaza<T>(T valoare) => JsonSerializer.Serialize(valoare, Optiuni);
}
