namespace Autogara.WinForms.Ui
{
    /// <summary>
    /// Culorile si fonturile aplicatiei, intr-un singur loc. Le folosesc controalele desenate
    /// de mana (harta locurilor, harta statiilor, butoanele cu iconite).
    /// </summary>
    public static class Tema
    {
        public static readonly Color Primar = Color.FromArgb(37, 99, 235);
        public static readonly Color PrimarInchis = Color.FromArgb(29, 78, 216);
        public static readonly Color PrimarDeschis = Color.FromArgb(219, 234, 254);

        public static readonly Color Fundal = Color.FromArgb(245, 247, 250);
        public static readonly Color Suprafata = Color.White;
        public static readonly Color Bordura = Color.FromArgb(217, 222, 229);
        public static readonly Color Text = Color.FromArgb(31, 41, 55);
        public static readonly Color TextSecundar = Color.FromArgb(107, 114, 128);

        public static readonly Color Succes = Color.FromArgb(22, 163, 74);
        public static readonly Color Avertisment = Color.FromArgb(217, 119, 6);
        public static readonly Color Eroare = Color.FromArgb(220, 38, 38);

        public static readonly Color Meniu = Color.FromArgb(15, 23, 42);
        public static readonly Color MeniuActiv = Color.FromArgb(30, 41, 59);
        public static readonly Color MeniuText = Color.FromArgb(203, 213, 225);

        // Harta locurilor
        public static readonly Color LocLiber = Color.FromArgb(220, 252, 231);
        public static readonly Color LocLiberBordura = Color.FromArgb(22, 163, 74);
        public static readonly Color LocRezervat = Color.FromArgb(254, 243, 199);
        public static readonly Color LocRezervatBordura = Color.FromArgb(217, 119, 6);
        public static readonly Color LocOcupat = Color.FromArgb(229, 231, 235);
        public static readonly Color LocOcupatBordura = Color.FromArgb(156, 163, 175);
        public static readonly Color LocAlMeu = Color.FromArgb(37, 99, 235);

        // Harta statiilor
        public static readonly Color NodStatie = Color.FromArgb(37, 99, 235);
        public static readonly Color NodIntersectie = Color.FromArgb(100, 116, 139);
        public static readonly Color Conexiune = Color.FromArgb(148, 163, 184);
        public static readonly Color Selectie = Color.FromArgb(245, 158, 11);
        public static readonly Color GrilaHarta = Color.FromArgb(234, 238, 243);

        public static Font FontNormal(float marime = 9f) => new("Segoe UI", marime);
        public static Font FontIngrosat(float marime = 9f) => new("Segoe UI Semibold", marime);

        public static string Hex(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";
    }
}
