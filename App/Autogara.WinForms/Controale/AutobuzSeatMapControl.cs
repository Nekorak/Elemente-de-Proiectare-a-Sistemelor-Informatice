using System.ComponentModel;
using System.Drawing.Drawing2D;
using Autogara.Business.Dto;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>
    /// Harta locurilor unei curse: pozitiile din JSON-ul autobuzului, colorate dupa status.
    /// Se pot alege doar locurile libere si cele rezervate de utilizatorul curent.
    /// </summary>
    public class AutobuzSeatMapControl : Control
    {
        private HartaLocuri _harta;
        private int? _selectat;
        private LocHarta _subMouse;

        public AutobuzSeatMapControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            BackColor = Tema.Fundal;
            Size = new Size(360, 520);
        }

        /// <summary>Un loc liber (sau rezervat de mine) a fost ales cu mouse-ul.</summary>
        public event EventHandler<LocHarta> LocSelectat;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HartaLocuri Harta
        {
            get => _harta;
            set
            {
                _harta = value;
                _subMouse = null;
                Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? LocSelectatID
        {
            get => _selectat;
            set { _selectat = value; Invalidate(); }
        }

        [Category("Autogara"), DefaultValue(true)]
        public bool ArataLegenda { get; set; } = true;

        private int SpatiuLegenda => ArataLegenda ? LogicalToDeviceUnits(40) : 0;

        private GeometrieAutobuz Geometrie() =>
            new(_harta.Randuri, _harta.Coloane, _harta.CuloarDupaColoana, ClientSize, LogicalToDeviceUnits, SpatiuLegenda);

        private bool SePoateAlege(LocHarta l) => l.Status == StatusLoc.Liber || l.RezervatDeMine;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_harta is null)
            {
                TextRenderer.DrawText(g, "Alegeți o cursă pentru a vedea locurile.", Font, ClientRectangle, Tema.TextSecundar,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
                return;
            }

            var geo = Geometrie();
            geo.DeseneazaCaroserie(g, LogicalToDeviceUnits);

            using var fontNumar = Tema.FontIngrosat(Math.Clamp(geo.MarimeLoc / DeviceDpi * 96f / 3.2f, 7f, 11f));
            foreach (var l in _harta.Locuri)
            {
                var r = geo.Loc(l.Rand, l.Coloana);
                var (fundal, bordura, text) = Culori(l);
                if (l == _subMouse && SePoateAlege(l) && l.LocID != _selectat)
                    fundal = ControlPaint.Light(bordura, 1.6f);

                using (var cale = Desen.Dreptunghi(r, geo.MarimeLoc * 0.18f))
                using (var b = new SolidBrush(fundal))
                using (var p = new Pen(bordura, LogicalToDeviceUnits(l.LocID == _selectat ? 2 : 1)))
                {
                    g.FillPath(b, cale);
                    g.DrawPath(p, cale);
                }

                // spatarul scaunului
                var spatar = new RectangleF(r.X + r.Width * 0.18f, r.Y + r.Height * 0.1f, r.Width * 0.64f, r.Height * 0.12f);
                using (var cale = Desen.Dreptunghi(spatar, spatar.Height / 2))
                using (var b = new SolidBrush(Color.FromArgb(60, bordura)))
                    g.FillPath(b, cale);

                Desen.TextCentrat(g, l.NumarLoc.ToString(), fontNumar, text,
                    new RectangleF(r.X, r.Y + r.Height * 0.12f, r.Width, r.Height * 0.88f));
            }

            if (ArataLegenda)
                DeseneazaLegenda(g);
        }

        private (Color Fundal, Color Bordura, Color Text) Culori(LocHarta l)
        {
            if (l.LocID == _selectat)
                return (Tema.LocAlMeu, Tema.PrimarInchis, Color.White);
            if (l.RezervatDeMine)
                return (Tema.PrimarDeschis, Tema.LocAlMeu, Tema.PrimarInchis);

            return l.Status switch
            {
                StatusLoc.Liber => (Tema.LocLiber, Tema.LocLiberBordura, Tema.Text),
                StatusLoc.Rezervat => (Tema.LocRezervat, Tema.LocRezervatBordura, Tema.TextSecundar),
                _ => (Tema.LocOcupat, Tema.LocOcupatBordura, Tema.TextSecundar),
            };
        }

        private void DeseneazaLegenda(Graphics g)
        {
            (string Text, Color Fundal, Color Bordura)[] elemente =
            [
                ("Liber", Tema.LocLiber, Tema.LocLiberBordura),
                ("Rezervat", Tema.LocRezervat, Tema.LocRezervatBordura),
                ("Ocupat", Tema.LocOcupat, Tema.LocOcupatBordura),
                ("Ales", Tema.LocAlMeu, Tema.PrimarInchis),
            ];

            var patrat = LogicalToDeviceUnits(14);
            var y = ClientSize.Height - LogicalToDeviceUnits(28);
            var latimi = elemente.Select(el => patrat + LogicalToDeviceUnits(6) + TextRenderer.MeasureText(el.Text, Font).Width).ToArray();
            var spatiu = LogicalToDeviceUnits(16);
            var x = (ClientSize.Width - (latimi.Sum() + spatiu * (elemente.Length - 1))) / 2;

            for (var i = 0; i < elemente.Length; i++)
            {
                var r = new RectangleF(x, y, patrat, patrat);
                using (var cale = Desen.Dreptunghi(r, 3))
                using (var b = new SolidBrush(elemente[i].Fundal))
                using (var p = new Pen(elemente[i].Bordura))
                {
                    g.FillPath(b, cale);
                    g.DrawPath(p, cale);
                }

                TextRenderer.DrawText(g, elemente[i].Text, Font, new Point(x + patrat + LogicalToDeviceUnits(6), y - 1), Tema.TextSecundar);
                x += latimi[i] + spatiu;
            }
        }

        private LocHarta LocLa(Point p)
        {
            if (_harta is null)
                return null;
            var geo = Geometrie();
            return _harta.Locuri.FirstOrDefault(l => geo.Loc(l.Rand, l.Coloana).Contains(p));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var l = LocLa(e.Location);
            Cursor = l is not null && SePoateAlege(l) ? Cursors.Hand : Cursors.Default;
            if (l != _subMouse)
            {
                _subMouse = l;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _subMouse = null;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Focus();
            if (e.Button == MouseButtons.Left && LocLa(e.Location) is { } l && SePoateAlege(l))
                LocSelectat?.Invoke(this, l);
        }
    }
}
