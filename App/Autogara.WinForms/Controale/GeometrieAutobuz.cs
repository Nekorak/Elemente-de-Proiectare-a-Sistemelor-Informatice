using System.Drawing.Drawing2D;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>
    /// Asezarea grilei de locuri in suprafata controlului (comuna pentru harta locurilor si editor):
    /// autobuzul vertical, cu fata in sus, culoarul intre coloane.
    /// </summary>
    internal sealed class GeometrieAutobuz
    {
        private readonly int _coloane;
        private readonly int _culoar;
        private readonly float _loc;
        private readonly float _spatiu;
        private readonly float _latimeCuloar;
        private readonly PointF _origine;

        public GeometrieAutobuz(int randuri, int coloane, int culoarDupaColoana, Size client, Func<int, int> scalare, int spatiuJos = 0)
        {
            _coloane = Math.Max(1, coloane);
            randuri = Math.Max(1, randuri);
            _culoar = culoarDupaColoana > 0 && culoarDupaColoana < _coloane ? culoarDupaColoana : 0;

            var pad = scalare(20);
            var fata = scalare(56);
            var disponibilW = client.Width - 2 * pad - scalare(24);
            var disponibilH = client.Height - 2 * pad - fata - scalare(16) - spatiuJos;

            // spatiu intre locuri = 20% din loc; culoar = 60% din loc
            var unitatiW = _coloane + (_coloane - 1) * 0.2f + (_culoar > 0 ? 0.6f : 0);
            var unitatiH = randuri + (randuri - 1) * 0.2f;
            _loc = Math.Clamp(Math.Min(disponibilW / unitatiW, disponibilH / unitatiH), scalare(16), scalare(52));
            _spatiu = _loc * 0.2f;
            _latimeCuloar = _culoar > 0 ? _loc * 0.6f : 0;

            var latimeGrila = _coloane * _loc + (_coloane - 1) * _spatiu + _latimeCuloar;
            var inaltimeGrila = randuri * _loc + (randuri - 1) * _spatiu;
            var bordura = scalare(12);

            Contur = new RectangleF((client.Width - latimeGrila) / 2 - bordura, pad, latimeGrila + 2 * bordura, fata + inaltimeGrila + 2 * bordura);
            _origine = new PointF(Contur.X + bordura, Contur.Y + fata + bordura);
            ZonaSofer = new RectangleF(Contur.X + bordura, Contur.Y + bordura, _loc, fata - bordura);
            MarimeLoc = _loc;
        }

        public RectangleF Contur { get; }
        public RectangleF ZonaSofer { get; }
        public float MarimeLoc { get; }

        public RectangleF Loc(int rand, int coloana)
        {
            var x = _origine.X + (coloana - 1) * (_loc + _spatiu);
            if (_culoar > 0 && coloana > _culoar)
                x += _latimeCuloar;
            var y = _origine.Y + (rand - 1) * (_loc + _spatiu);
            return new RectangleF(x, y, _loc, _loc);
        }

        public (int Rand, int Coloana)? Pozitie(Point p, int randuri)
        {
            for (var r = 1; r <= randuri; r++)
                for (var c = 1; c <= _coloane; c++)
                    if (Loc(r, c).Contains(p))
                        return (r, c);
            return null;
        }

        public void DeseneazaCaroserie(Graphics g, Func<int, int> scalare)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var cale = Desen.Dreptunghi(Contur, scalare(22)))
            using (var fundal = new SolidBrush(Tema.Suprafata))
            using (var bordura = new Pen(Tema.Bordura, scalare(2)))
            {
                g.FillPath(fundal, cale);
                g.DrawPath(bordura, cale);
            }

            // parbriz
            var parbriz = new RectangleF(Contur.X + scalare(18), Contur.Y + scalare(6), Contur.Width - scalare(36), scalare(6));
            using (var cale = Desen.Dreptunghi(parbriz, scalare(3)))
            using (var b = new SolidBrush(Tema.GrilaHarta))
                g.FillPath(b, cale);

            // soferul, in stanga fata
            var icon = scalare(22);
            g.DrawImage(Iconite.Deseneaza("armchair", icon, Tema.TextSecundar),
                ZonaSofer.X + (ZonaSofer.Width - icon) / 2, ZonaSofer.Y + (ZonaSofer.Height - icon) / 2 + scalare(4), icon, icon);
            using var font = Tema.FontNormal(8f);
            TextRenderer.DrawText(g, "Față", font,
                new Rectangle((int)Contur.X, (int)(Contur.Y + scalare(14)), (int)Contur.Width, scalare(20)), Tema.TextSecundar,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
        }
    }
}
