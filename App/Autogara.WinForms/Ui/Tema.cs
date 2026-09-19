namespace Autogara.WinForms.Ui
{
    /// <summary>
    /// Culorile si fonturile aplicatiei, intr-un singur loc. Stilul e cel al panourilor din
    /// autogari: verde inchis de indicator, galben de semnalizare, fundal de hartie si
    /// Bahnschrift (fontul DIN al indicatoarelor) pentru titluri, meniu si cifre.
    /// </summary>
    public static class Tema
    {
        public static readonly Color Primar = Color.FromArgb(30, 91, 70);
        public static readonly Color PrimarInchis = Color.FromArgb(21, 69, 53);
        public static readonly Color PrimarDeschis = Color.FromArgb(221, 235, 227);

        /// <summary>Galbenul de semnalizare: sectiunea activa, linia de sub antet, randul selectat.</summary>
        public static readonly Color Semnal = Color.FromArgb(232, 163, 23);
        public static readonly Color SemnalDeschis = Color.FromArgb(248, 234, 196);

        public static readonly Color Fundal = Color.FromArgb(238, 236, 230);
        public static readonly Color Suprafata = Color.FromArgb(250, 249, 246);
        public static readonly Color Bordura = Color.FromArgb(212, 208, 198);
        public static readonly Color Text = Color.FromArgb(34, 34, 31);
        public static readonly Color TextSecundar = Color.FromArgb(110, 106, 96);

        public static readonly Color Succes = Color.FromArgb(46, 125, 50);
        public static readonly Color Avertisment = Color.FromArgb(183, 121, 31);
        public static readonly Color Eroare = Color.FromArgb(179, 38, 30);

        public static readonly Color Meniu = Color.FromArgb(28, 36, 32);
        public static readonly Color MeniuActiv = Color.FromArgb(38, 49, 43);
        public static readonly Color MeniuText = Color.FromArgb(201, 207, 200);
        public static readonly Color MeniuGrup = Color.FromArgb(127, 138, 131);

        // Harta locurilor
        public static readonly Color LocLiber = Color.FromArgb(227, 239, 230);
        public static readonly Color LocLiberBordura = Color.FromArgb(46, 125, 50);
        public static readonly Color LocRezervat = Color.FromArgb(250, 235, 200);
        public static readonly Color LocRezervatBordura = Color.FromArgb(183, 121, 31);
        public static readonly Color LocOcupat = Color.FromArgb(226, 223, 216);
        public static readonly Color LocOcupatBordura = Color.FromArgb(163, 158, 147);
        public static readonly Color LocAlMeu = Color.FromArgb(30, 91, 70);

        // Harta statiilor
        public static readonly Color NodStatie = Color.FromArgb(30, 91, 70);
        public static readonly Color NodIntersectie = Color.FromArgb(127, 138, 131);
        public static readonly Color Conexiune = Color.FromArgb(160, 155, 145);
        public static readonly Color Selectie = Color.FromArgb(232, 163, 23);
        public static readonly Color GrilaHarta = Color.FromArgb(230, 227, 219);

        /// <summary>Colturile: aproape drepte, ca la indicatoare.</summary>
        public const int Raza = 2;

        public static Font FontNormal(float marime = 9f) => new("Segoe UI", marime);
        public static Font FontIngrosat(float marime = 9f) => new("Bahnschrift SemiBold", marime);
        public static Font FontIndicator(float marime = 9f) => new("Bahnschrift", marime);

        public static string Hex(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";
    }
}
