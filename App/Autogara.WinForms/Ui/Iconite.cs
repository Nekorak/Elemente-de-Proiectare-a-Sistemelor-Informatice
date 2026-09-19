using System.Collections.Concurrent;
using System.Reflection;
using Svg;

namespace Autogara.WinForms.Ui
{
    /// <summary>
    /// Iconitele Lucide (SVG, incluse in aplicatie) desenate ca Bitmap la marimea si culoarea cerute.
    /// Numele sunt cele din Resurse/Iconite, fara extensie (ex. "bus", "ticket-x").
    /// </summary>
    public static class Iconite
    {
        private static readonly ConcurrentDictionary<(string, int, int), Bitmap> Cache = new();
        private static Icon _iconAplicatie;

        public static Bitmap Deseneaza(string nume, int marime, Color culoare)
        {
            return Cache.GetOrAdd((nume, marime, culoare.ToArgb()), _ =>
            {
                var svg = CitesteSvg(nume);
                if (svg is null)
                    return new Bitmap(marime, marime);

                var document = SvgDocument.FromSvg<SvgDocument>(svg.Replace("currentColor", Tema.Hex(culoare)));
                document.Width = marime;
                document.Height = marime;
                return document.Draw(marime, marime);
            });
        }

        public static bool Exista(string nume) => !string.IsNullOrEmpty(nume) && CitesteSvg(nume) is not null;

        /// <summary>Iconita ferestrelor: autobuzul pe o tablita verde, cu banda galbena jos.</summary>
        public static Icon IconAplicatie
        {
            get
            {
                if (_iconAplicatie is not null)
                    return _iconAplicatie;

                using var bmp = new Bitmap(64, 64);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using var fundal = new SolidBrush(Tema.Primar);
                    using var semnal = new SolidBrush(Tema.Semnal);
                    using var cale = Desen.Dreptunghi(new RectangleF(0, 0, 63, 63), 4);
                    g.FillPath(fundal, cale);
                    g.FillRectangle(semnal, 0, 52, 64, 12);
                    g.DrawImage(Deseneaza("bus-front", 40, Color.White), 12, 8);
                }

                _iconAplicatie = Icon.FromHandle(bmp.GetHicon());
                return _iconAplicatie;
            }
        }

        private static string CitesteSvg(string nume)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Iconite.{nume}.svg");
            if (stream is null)
                return null;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
