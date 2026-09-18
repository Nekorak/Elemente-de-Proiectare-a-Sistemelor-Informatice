using System.ComponentModel;
using System.Drawing.Drawing2D;
using Autogara.Business.Dto;
using Autogara.Business.Servicii;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    public enum ModHarta
    {
        Selectare,
        AdaugaStatie,
        AdaugaIntersectie,
        Conexiune,
        Sterge,
    }

    /// <summary>
    /// Editorul vizual al hartii: noduri adaugate prin click, conexiuni trasate prin drag intre
    /// noduri, mutarea nodurilor, zoom cu rotita si deplasare cu butonul drept/mijlociu.
    /// Conexiunile se afiseaza o singura data pe pereche; la salvare backend-ul le pune in ambele sensuri.
    /// </summary>
    public class HartaCanvasControl : Control
    {
        private HartaDto _harta = new();
        private ModHarta _mod = ModHarta.Selectare;
        private float _scara = 1f;
        private PointF _deplasare;

        private NodDto _nodSelectat;
        private (int A, int B)? _conexiuneSelectata;
        private NodDto _nodTras;
        private PointF _decalajTragere;
        private NodDto _nodStartConexiune;
        private Point _mouse;
        private Point? _ultimPan;
        private bool _aMutat;
        private bool _vizualizareManuala;

        public HartaCanvasControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            BackColor = Tema.Suprafata;
            Size = new Size(600, 450);
        }

        public event EventHandler<NodDto> NodAdaugat;
        public event EventHandler<ConexiuneDto> ConexiuneCreata;
        public event EventHandler SelectieSchimbata;
        public event EventHandler HartaModificata;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HartaDto Harta
        {
            get => _harta;
            set
            {
                _harta = value ?? new HartaDto();
                _nodSelectat = null;
                _conexiuneSelectata = null;
                Modificata = false;
                Potriveste();
                SelectieSchimbata?.Invoke(this, EventArgs.Empty);
            }
        }

        [Category("Autogara"), DefaultValue(ModHarta.Selectare)]
        public ModHarta Mod
        {
            get => _mod;
            set
            {
                _mod = value;
                _nodStartConexiune = null;
                Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Modificata { get; private set; }

        [Browsable(false)]
        public NodDto NodSelectat => _nodSelectat;

        /// <summary>Conexiunea selectata (primul sens gasit), sau null.</summary>
        [Browsable(false)]
        public ConexiuneDto ConexiuneSelectata => _conexiuneSelectata is { } p ? Gaseste(p.A, p.B) : null;

        // ---------- operatii apelate din formular ----------

        public void SeteazaNod(string nume, TipNod tip)
        {
            if (_nodSelectat is null)
                return;
            _nodSelectat.Nume = nume;
            _nodSelectat.Tip = tip;
            Marcheaza();
        }

        public void SeteazaDistanta(decimal km)
        {
            if (_conexiuneSelectata is not { } p)
                return;
            foreach (var c in _harta.Conexiuni.Where(c => Pereche(c) == p))
                c.DistantaKm = km;
            Marcheaza();
        }

        public void StergeSelectia()
        {
            if (_nodSelectat is { } n)
            {
                _harta.Noduri.Remove(n);
                _harta.Conexiuni.RemoveAll(c => c.NodPlecareID == n.NodID || c.NodSosireID == n.NodID);
                _nodSelectat = null;
            }
            else if (_conexiuneSelectata is { } p)
            {
                _harta.Conexiuni.RemoveAll(c => Pereche(c) == p);
                _conexiuneSelectata = null;
            }
            else
            {
                return;
            }

            Marcheaza();
            SelectieSchimbata?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Zoom si deplasare astfel incat toate nodurile sa fie vizibile.</summary>
        public void Potriveste()
        {
            _vizualizareManuala = false;
            if (_harta.Noduri.Count == 0 || ClientSize.Width < 10 || ClientSize.Height < 10)
            {
                _scara = 1f;
                _deplasare = new PointF(LogicalToDeviceUnits(40), LogicalToDeviceUnits(40));
                Invalidate();
                return;
            }

            var minX = _harta.Noduri.Min(n => n.X);
            var maxX = _harta.Noduri.Max(n => n.X);
            var minY = _harta.Noduri.Min(n => n.Y);
            var maxY = _harta.Noduri.Max(n => n.Y);
            var margine = LogicalToDeviceUnits(70);

            var sx = (ClientSize.Width - 2 * margine) / (float)Math.Max(1, maxX - minX);
            var sy = (ClientSize.Height - 2 * margine) / (float)Math.Max(1, maxY - minY);
            _scara = Math.Clamp(Math.Min(sx, sy), 0.1f, 6f);
            _deplasare = new PointF(
                (ClientSize.Width - (float)(maxX - minX) * _scara) / 2 - (float)minX * _scara,
                (ClientSize.Height - (float)(maxY - minY) * _scara) / 2 - (float)minY * _scara);
            Invalidate();
        }

        // ---------- coordonate ----------

        private PointF LaEcran(double x, double y) => new((float)x * _scara + _deplasare.X, (float)y * _scara + _deplasare.Y);

        private PointF LaHarta(Point p) => new((p.X - _deplasare.X) / _scara, (p.Y - _deplasare.Y) / _scara);

        private static (int, int) Pereche(ConexiuneDto c) =>
            (Math.Min(c.NodPlecareID, c.NodSosireID), Math.Max(c.NodPlecareID, c.NodSosireID));

        private ConexiuneDto Gaseste(int a, int b) => _harta.Conexiuni.FirstOrDefault(c => Pereche(c) == (a, b));

        private IEnumerable<ConexiuneDto> Perechi() => _harta.Conexiuni.GroupBy(Pereche).Select(g => g.First());

        private float RazaNod(NodDto n) => LogicalToDeviceUnits(n.Tip == TipNod.Statie ? 9 : 6);

        private NodDto NodLa(Point p)
        {
            var toleranta = LogicalToDeviceUnits(6);
            return _harta.Noduri
                .Select(n => (Nod: n, D: Distanta(LaEcran(n.X, n.Y), p)))
                .Where(x => x.D <= RazaNod(x.Nod) + toleranta)
                .OrderBy(x => x.D)
                .Select(x => x.Nod)
                .FirstOrDefault();
        }

        private (int A, int B)? ConexiuneLa(Point p)
        {
            var noduri = _harta.Noduri.ToDictionary(n => n.NodID);
            var toleranta = LogicalToDeviceUnits(6);
            foreach (var c in Perechi())
            {
                if (!noduri.TryGetValue(c.NodPlecareID, out var a) || !noduri.TryGetValue(c.NodSosireID, out var b))
                    continue;
                if (DistantaLaSegment(p, LaEcran(a.X, a.Y), LaEcran(b.X, b.Y)) <= toleranta)
                    return Pereche(c);
            }
            return null;
        }

        private static float Distanta(PointF a, Point b) => MathF.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        private static float DistantaLaSegment(Point p, PointF a, PointF b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            var lung = dx * dx + dy * dy;
            var t = lung == 0 ? 0 : Math.Clamp(((p.X - a.X) * dx + (p.Y - a.Y) * dy) / lung, 0, 1);
            return Distanta(new PointF(a.X + t * dx, a.Y + t * dy), p);
        }

        // ---------- desen ----------

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DeseneazaGrila(g);

            var noduri = _harta.Noduri.ToDictionary(n => n.NodID);
            using var fontMic = Tema.FontNormal(7.5f);
            using var fontNume = Tema.FontNormal(8.5f);

            foreach (var c in Perechi())
            {
                if (!noduri.TryGetValue(c.NodPlecareID, out var a) || !noduri.TryGetValue(c.NodSosireID, out var b))
                    continue;

                var pa = LaEcran(a.X, a.Y);
                var pb = LaEcran(b.X, b.Y);
                var selectata = _conexiuneSelectata == Pereche(c);
                using (var pen = new Pen(selectata ? Tema.Selectie : Tema.Conexiune, LogicalToDeviceUnits(selectata ? 4 : 3)))
                {
                    pen.StartCap = pen.EndCap = LineCap.Round;
                    g.DrawLine(pen, pa, pb);
                }

                var text = $"{c.DistantaKm:0.##} km";
                var marime = TextRenderer.MeasureText(text, fontMic);
                var mijloc = new PointF((pa.X + pb.X) / 2, (pa.Y + pb.Y) / 2);
                var eticheta = new RectangleF(mijloc.X - marime.Width / 2f - 3, mijloc.Y - marime.Height / 2f, marime.Width + 6, marime.Height);
                using (var cale = Desen.Dreptunghi(eticheta, eticheta.Height / 2))
                using (var b2 = new SolidBrush(Color.FromArgb(235, Tema.Suprafata)))
                using (var p2 = new Pen(selectata ? Tema.Selectie : Tema.Bordura))
                {
                    g.FillPath(b2, cale);
                    g.DrawPath(p2, cale);
                }
                Desen.TextCentrat(g, text, fontMic, Tema.TextSecundar, eticheta);
            }

            if (_mod == ModHarta.Conexiune && _nodStartConexiune is { } start)
            {
                using var pen = new Pen(Tema.Primar, LogicalToDeviceUnits(2)) { DashStyle = DashStyle.Dash };
                g.DrawLine(pen, LaEcran(start.X, start.Y), _mouse);
            }

            foreach (var n in _harta.Noduri)
            {
                var p = LaEcran(n.X, n.Y);
                var raza = RazaNod(n);
                var cerc = new RectangleF(p.X - raza, p.Y - raza, raza * 2, raza * 2);

                if (n == _nodSelectat || n == _nodStartConexiune)
                {
                    var halou = raza + LogicalToDeviceUnits(5);
                    using var b = new SolidBrush(Color.FromArgb(90, Tema.Selectie));
                    g.FillEllipse(b, p.X - halou, p.Y - halou, halou * 2, halou * 2);
                }

                using (var b = new SolidBrush(n.Tip == TipNod.Statie ? Tema.NodStatie : Tema.NodIntersectie))
                using (var pen = new Pen(Color.White, LogicalToDeviceUnits(2)))
                {
                    g.FillEllipse(b, cerc);
                    g.DrawEllipse(pen, cerc);
                }

                if (n.Tip == TipNod.Statie || _scara > 0.8f)
                {
                    var marime = TextRenderer.MeasureText(n.Nume, fontNume);
                    var pozitie = new Point((int)(p.X - marime.Width / 2f), (int)(p.Y + raza + LogicalToDeviceUnits(3)));
                    TextRenderer.DrawText(g, n.Nume, fontNume, pozitie,
                        n.Tip == TipNod.Statie ? Tema.Text : Tema.TextSecundar, Tema.Suprafata);
                }
            }

            if (_harta.Noduri.Count == 0)
                TextRenderer.DrawText(g, "Harta este goală. Alegeți „Stație” și dați click pe hartă.", Font, ClientRectangle,
                    Tema.TextSecundar, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DeseneazaGrila(Graphics g)
        {
            var pas = 50f * _scara;
            while (pas < LogicalToDeviceUnits(24)) pas *= 2;
            using var pen = new Pen(Tema.GrilaHarta);
            for (var x = _deplasare.X % pas; x < ClientSize.Width; x += pas)
                g.DrawLine(pen, x, 0, x, ClientSize.Height);
            for (var y = _deplasare.Y % pas; y < ClientSize.Height; y += pas)
                g.DrawLine(pen, 0, y, ClientSize.Width, y);
        }

        // ---------- mouse ----------

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            _aMutat = false;

            if (e.Button is MouseButtons.Right or MouseButtons.Middle)
            {
                _ultimPan = e.Location;
                Cursor = Cursors.SizeAll;
                return;
            }
            if (e.Button != MouseButtons.Left)
                return;

            var nod = NodLa(e.Location);
            switch (_mod)
            {
                case ModHarta.Selectare:
                    if (nod is not null)
                    {
                        Selecteaza(nod, null);
                        _nodTras = nod;
                        var p = LaHarta(e.Location);
                        _decalajTragere = new PointF((float)(nod.X - p.X), (float)(nod.Y - p.Y));
                    }
                    else
                    {
                        Selecteaza(null, ConexiuneLa(e.Location));
                    }
                    break;

                case ModHarta.AdaugaStatie:
                case ModHarta.AdaugaIntersectie:
                    if (nod is not null)
                    {
                        Selecteaza(nod, null);
                        break;
                    }
                    AdaugaNod(e.Location, _mod == ModHarta.AdaugaStatie ? TipNod.Statie : TipNod.Intersectie);
                    break;

                case ModHarta.Conexiune:
                    _nodStartConexiune = nod;
                    _mouse = e.Location;
                    Invalidate();
                    break;

                case ModHarta.Sterge:
                    if (nod is not null)
                        Selecteaza(nod, null);
                    else
                        Selecteaza(null, ConexiuneLa(e.Location));
                    StergeSelectia();
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _mouse = e.Location;

            if (_ultimPan is { } ultim)
            {
                _deplasare = new PointF(_deplasare.X + e.X - ultim.X, _deplasare.Y + e.Y - ultim.Y);
                _vizualizareManuala = true;
                _ultimPan = e.Location;
                Invalidate();
                return;
            }

            if (_nodTras is not null && e.Button == MouseButtons.Left)
            {
                var p = LaHarta(e.Location);
                _nodTras.X = Math.Round(p.X + _decalajTragere.X, 1);
                _nodTras.Y = Math.Round(p.Y + _decalajTragere.Y, 1);
                _aMutat = true;
                Invalidate();
                return;
            }

            if (_nodStartConexiune is not null)
            {
                Invalidate();
                return;
            }

            Cursor = _mod switch
            {
                ModHarta.Selectare => NodLa(e.Location) is not null ? Cursors.SizeAll : ConexiuneLa(e.Location) is not null ? Cursors.Hand : Cursors.Default,
                ModHarta.Sterge => NodLa(e.Location) is not null || ConexiuneLa(e.Location) is not null ? Cursors.Hand : Cursors.Default,
                _ => Cursors.Cross,
            };
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_ultimPan is not null)
            {
                _ultimPan = null;
                Cursor = Cursors.Default;
                return;
            }

            if (_nodTras is not null)
            {
                _nodTras = null;
                if (_aMutat)
                    Marcheaza();
            }

            if (_nodStartConexiune is { } start)
            {
                var final = NodLa(e.Location);
                _nodStartConexiune = null;
                if (final is not null && final != start)
                    CreeazaConexiune(start, final);
                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            var inainte = LaHarta(e.Location);
            _scara = Math.Clamp(_scara * (e.Delta > 0 ? 1.15f : 1 / 1.15f), 0.1f, 8f);
            _deplasare = new PointF(e.X - inainte.X * _scara, e.Y - inainte.Y * _scara);
            _vizualizareManuala = true;
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!_vizualizareManuala)
                Potriveste();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Delete)
                StergeSelectia();
        }

        // ---------- modificari ----------

        private void Selecteaza(NodDto nod, (int, int)? conexiune)
        {
            _nodSelectat = nod;
            _conexiuneSelectata = nod is null ? conexiune : null;
            Invalidate();
            SelectieSchimbata?.Invoke(this, EventArgs.Empty);
        }

        private void AdaugaNod(Point ecran, TipNod tip)
        {
            var p = LaHarta(ecran);
            var id = Math.Min(0, _harta.Noduri.Count == 0 ? 0 : _harta.Noduri.Min(n => n.NodID)) - 1;
            var nr = _harta.Noduri.Count(n => n.Tip == tip && n.NodID < 0) + 1;
            var nod = new NodDto
            {
                NodID = id,
                Tip = tip,
                Nume = tip == TipNod.Statie ? $"Stație nouă {nr}" : $"Intersecție nouă {nr}",
                X = Math.Round(p.X, 1),
                Y = Math.Round(p.Y, 1),
            };
            _harta.Noduri.Add(nod);
            Marcheaza();
            Selecteaza(nod, null);
            NodAdaugat?.Invoke(this, nod);
        }

        private void CreeazaConexiune(NodDto a, NodDto b)
        {
            var pereche = (Math.Min(a.NodID, b.NodID), Math.Max(a.NodID, b.NodID));
            if (Gaseste(pereche.Item1, pereche.Item2) is null)
            {
                var c = new ConexiuneDto
                {
                    NodPlecareID = a.NodID,
                    NodSosireID = b.NodID,
                    DistantaKm = HartaService.EstimeazaDistantaKm(_harta, a.NodID, b.NodID),
                };
                _harta.Conexiuni.Add(c);
                Marcheaza();
                ConexiuneCreata?.Invoke(this, c);
            }

            Selecteaza(null, pereche);
        }

        private void Marcheaza()
        {
            Modificata = true;
            Invalidate();
            HartaModificata?.Invoke(this, EventArgs.Empty);
        }
    }
}
