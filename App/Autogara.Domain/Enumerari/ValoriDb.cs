namespace Autogara.Domain.Enumerari;

/// <summary>
/// Conversia enum ↔ textul salvat in coloanele de status din SQL
/// (ex. StatusCursa.InDesfasurare ↔ "In desfasurare").
/// </summary>
public static class ValoriDb
{
    private static readonly Dictionary<Type, Dictionary<Enum, string>> Texte = new()
    {
        [typeof(RolTip)] = Map<RolTip>((RolTip.Admin, "Admin"), (RolTip.Casier, "Casier"), (RolTip.Pasager, "Pasager")),
        [typeof(StatusLoc)] = Map<StatusLoc>((StatusLoc.Liber, "Liber"), (StatusLoc.Rezervat, "Rezervat"), (StatusLoc.Ocupat, "Ocupat")),
        [typeof(StatusBilet)] = Map<StatusBilet>((StatusBilet.Activ, "Activ"), (StatusBilet.Anulat, "Anulat"), (StatusBilet.Rambursat, "Rambursat")),
        [typeof(StatusCursa)] = Map<StatusCursa>(
            (StatusCursa.Planificata, "Planificata"), (StatusCursa.InDesfasurare, "In desfasurare"),
            (StatusCursa.Finalizata, "Finalizata"), (StatusCursa.Anulata, "Anulata")),
        [typeof(StatusPlata)] = Map<StatusPlata>((StatusPlata.Finalizata, "Finalizata"), (StatusPlata.Rambursata, "Rambursata")),
        [typeof(MetodaPlata)] = Map<MetodaPlata>((MetodaPlata.Numerar, "Numerar"), (MetodaPlata.Card, "Card")),
        [typeof(StatusAutobuz)] = Map<StatusAutobuz>(
            (StatusAutobuz.Activ, "Activ"), (StatusAutobuz.Service, "Service"), (StatusAutobuz.ScosDinUz, "Scos din uz")),
        [typeof(TipNod)] = Map<TipNod>((TipNod.Statie, "Statie"), (TipNod.Intersectie, "Intersectie")),
    };

    public static string ToDb<T>(T valoare) where T : struct, Enum => Texte[typeof(T)][valoare];

    public static T FromDb<T>(string text) where T : struct, Enum
    {
        foreach (var (cheie, valoare) in Texte[typeof(T)])
            if (string.Equals(valoare, text, StringComparison.OrdinalIgnoreCase))
                return (T)cheie;

        throw new ArgumentException($"Valoare necunoscuta pentru {typeof(T).Name}: '{text}'.", nameof(text));
    }

    public static IReadOnlyCollection<string> Toate<T>() where T : struct, Enum => Texte[typeof(T)].Values;

    private static Dictionary<Enum, string> Map<T>(params (T Valoare, string Text)[] perechi) where T : struct, Enum =>
        perechi.ToDictionary(p => (Enum)p.Valoare, p => p.Text);
}
