using System.Globalization;
using Autogara.Domain.Enumerari;

namespace Autogara.WinForms.Ui
{
    /// <summary>Textul afisat pentru valorile din date (statusuri, date, sume).</summary>
    public static class Afisare
    {
        public static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("ro-RO");

        public static string Text(object valoare) => valoare switch
        {
            null => string.Empty,
            StatusCursa s => s switch
            {
                StatusCursa.Planificata => "Planificată",
                StatusCursa.InDesfasurare => "În desfășurare",
                StatusCursa.Finalizata => "Finalizată",
                _ => "Anulată",
            },
            StatusBilet s => s switch
            {
                StatusBilet.Activ => "Activ",
                StatusBilet.Anulat => "Anulat",
                _ => "Rambursat",
            },
            StatusLoc s => s switch
            {
                StatusLoc.Liber => "Liber",
                StatusLoc.Rezervat => "Rezervat",
                _ => "Ocupat",
            },
            StatusAutobuz s => s switch
            {
                StatusAutobuz.Activ => "Activ",
                StatusAutobuz.Service => "În service",
                _ => "Scos din uz",
            },
            TipNod t => t == TipNod.Statie ? "Stație" : "Intersecție",
            MetodaPlata m => m == MetodaPlata.Numerar ? "Numerar" : "Card",
            RolTip r => r.ToString(),
            bool b => b ? "Da" : "Nu",
            DateOnly d => d.ToString("dd.MM.yyyy", Cultura),
            TimeOnly t => t.ToString("HH:mm", Cultura),
            DateTime dt => dt.ToString("dd.MM.yyyy HH:mm", Cultura),
            decimal m => m.ToString("N2", Cultura),
            _ => Convert.ToString(valoare, Cultura) ?? string.Empty,
        };

        /// <summary>Statusurile din view-uri vin ca text din SQL ("In desfasurare").</summary>
        public static string StatusCursaSql(string status) =>
            Enum.TryParse<StatusCursa>(status?.Replace(" ", ""), true, out var s) ? Text(s) : status;

        public static string Lei(decimal suma) => $"{suma.ToString("N2", Cultura)} MDL";

        public static string Durata(int secunde) => $"{secunde / 60}:{secunde % 60:00}";
    }

    /// <summary>Element pentru ComboBox-uri: text afisat + valoarea din spate.</summary>
    public sealed class ElementLista<T>
    {
        public ElementLista(T valoare, string text)
        {
            Valoare = valoare;
            Text = text;
        }

        public T Valoare { get; }
        public string Text { get; }

        public override string ToString() => Text;
    }
}
