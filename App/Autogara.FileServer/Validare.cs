using Autogara.FileServer.Modele;

namespace Autogara.FileServer;

/// <summary>
/// Verificarile facute inainte de a scrie un JSON pe file server si dupa ce e citit.
/// Returneaza toate erorile gasite (lista goala = valid).
/// </summary>
public static class Validare
{
    public static List<string> Autobuz(AutobuzStructura? s, int? capacitateAsteptata = null)
    {
        var erori = new List<string>();
        if (s is null)
        {
            erori.Add("Structura autobuzului lipsește.");
            return erori;
        }

        if (s.Randuri <= 0) erori.Add("Numărul de rânduri trebuie să fie mai mare ca 0.");
        if (s.Coloane <= 0) erori.Add("Numărul de coloane trebuie să fie mai mare ca 0.");
        if (s.CuloarDupaColoana < 0 || (s.Coloane > 0 && s.CuloarDupaColoana >= s.Coloane))
            erori.Add("Culoarul trebuie să fie între două coloane existente (sau 0, fără culoar).");

        s.Locuri ??= [];
        if (s.Locuri.Count == 0)
            erori.Add("Autobuzul nu are niciun loc.");

        if (capacitateAsteptata is { } capacitate && s.Locuri.Count != capacitate)
            erori.Add($"Structura are {s.Locuri.Count} locuri, dar capacitatea autobuzului este {capacitate}.");

        foreach (var nr in s.Locuri.GroupBy(l => l.NumarLoc).Where(g => g.Count() > 1).Select(g => g.Key))
            erori.Add($"Locul {nr} apare de mai multe ori.");

        foreach (var l in s.Locuri.Where(l => l.NumarLoc <= 0))
            erori.Add($"Număr de loc invalid: {l.NumarLoc}.");

        foreach (var l in s.Locuri.Where(l => l.Rand < 1 || l.Rand > s.Randuri || l.Coloana < 1 || l.Coloana > s.Coloane))
            erori.Add($"Locul {l.NumarLoc} este în afara grilei ({l.Rand}, {l.Coloana}).");

        foreach (var g in s.Locuri.GroupBy(l => (l.Rand, l.Coloana)).Where(g => g.Count() > 1))
            erori.Add($"Pe poziția rând {g.Key.Rand}, coloana {g.Key.Coloana} sunt mai multe locuri ({string.Join(", ", g.Select(l => l.NumarLoc))}).");

        // Locurile sunt create in SQL numerotate 1..capacitate (sp_CreeazaLocuriPentruCursa).
        if (s.Locuri.Count > 0 && s.Locuri.All(l => l.NumarLoc > 0))
        {
            var lipsa = Enumerable.Range(1, s.Locuri.Count).Except(s.Locuri.Select(l => l.NumarLoc)).ToList();
            if (lipsa.Count > 0)
                erori.Add($"Locurile trebuie numerotate continuu de la 1; lipsesc: {string.Join(", ", lipsa)}.");
        }

        return erori;
    }

    public static List<string> Harta(HartaFisier? h)
    {
        var erori = new List<string>();
        if (h is null)
        {
            erori.Add("Harta lipsește.");
            return erori;
        }

        h.Noduri ??= [];
        h.Conexiuni ??= [];

        foreach (var id in h.Noduri.GroupBy(n => n.Id).Where(g => g.Count() > 1).Select(g => g.Key))
            erori.Add($"Nodul {id} apare de mai multe ori.");

        foreach (var n in h.Noduri)
        {
            if (n.Tip is not ("Statie" or "Intersectie"))
                erori.Add($"Nodul {n.Id}: tip necunoscut „{n.Tip}”.");
            if (string.IsNullOrWhiteSpace(n.Nume))
                erori.Add($"Nodul {n.Id} nu are nume.");
        }

        var ids = h.Noduri.Select(n => n.Id).ToHashSet();
        foreach (var c in h.Conexiuni)
        {
            if (!ids.Contains(c.NodPlecareId) || !ids.Contains(c.NodSosireId))
                erori.Add($"Conexiunea {c.NodPlecareId} → {c.NodSosireId} leagă un nod inexistent.");
            if (c.NodPlecareId == c.NodSosireId)
                erori.Add($"Conexiunea {c.NodPlecareId} → {c.NodSosireId} leagă nodul de el însuși.");
            if (c.DistantaKm <= 0 || c.DistantaKm >= 10000)
                erori.Add($"Conexiunea {c.NodPlecareId} → {c.NodSosireId}: distanța trebuie să fie între 0 și 10000 km.");
        }

        foreach (var g in h.Conexiuni.GroupBy(c => (c.NodPlecareId, c.NodSosireId)).Where(g => g.Count() > 1))
            erori.Add($"Conexiunea {g.Key.NodPlecareId} → {g.Key.NodSosireId} apare de mai multe ori.");

        return erori;
    }
}
