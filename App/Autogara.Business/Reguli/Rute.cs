namespace Autogara.Business.Reguli;

/// <summary>Cel mai scurt drum pe harta (Dijkstra pe conexiunile orientate dintre noduri).</summary>
public static class Rute
{
    public sealed record Muchie(int De, int La, decimal DistantaKm);

    public sealed record Drum(IReadOnlyList<int> Noduri, decimal DistantaKm);

    /// <returns>null daca nodul de sosire nu poate fi atins.</returns>
    public static Drum? CelMaiScurt(IEnumerable<Muchie> muchii, int start, int stop)
    {
        if (start == stop)
            return new Drum([start], 0);

        var vecini = muchii
            .Where(m => m.DistantaKm > 0)
            .GroupBy(m => m.De)
            .ToDictionary(g => g.Key, g => g.ToList());

        var distanta = new Dictionary<int, decimal> { [start] = 0 };
        var precedent = new Dictionary<int, int>();
        var coada = new PriorityQueue<int, decimal>();
        coada.Enqueue(start, 0);

        while (coada.TryDequeue(out var nod, out var d))
        {
            if (d > distanta[nod])
                continue; // intrare veche din coada
            if (nod == stop)
                break;

            foreach (var m in vecini.GetValueOrDefault(nod) ?? [])
            {
                var nou = d + m.DistantaKm;
                if (!distanta.TryGetValue(m.La, out var existent) || nou < existent)
                {
                    distanta[m.La] = nou;
                    precedent[m.La] = nod;
                    coada.Enqueue(m.La, nou);
                }
            }
        }

        if (!distanta.TryGetValue(stop, out var total))
            return null;

        var drum = new List<int> { stop };
        for (var n = stop; n != start; n = precedent[n])
            drum.Add(precedent[n]);
        drum.Reverse();

        return new Drum(drum, total);
    }
}
