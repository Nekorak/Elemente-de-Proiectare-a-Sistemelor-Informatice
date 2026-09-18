using System.ComponentModel;
using System.Drawing.Drawing2D;
using Autogara.Business.Servicii;
using Autogara.FileServer.Modele;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>
    /// Editorul asezarii locurilor: toata grila e desenata; click pe o pozitie goala adauga un loc,
    /// click pe un loc il scoate. Numerotarea o face backend-ul (AutobuzService.Renumeroteaza).
    /// </summary>
    public class AutobuzEditorControl : Control
    {
        private AutobuzStructura _structura;
        private (int Rand, int Coloana)? _subMouse;

        public AutobuzEditorControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            BackColor = Tema.Fundal;
            Size = new Size(360, 520);
        }

        public event EventHandler StructuraSchimbata;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AutobuzStructura Structura
        {
            get => _structura;
            set
            {
                _structura = value;
                Invalidate();
            }
        }

        private GeometrieAutobuz Geometrie() =>
            new(_structura.Randuri, _structura.Coloane, _structura.CuloarDupaColoana, ClientSize, LogicalToDeviceUnits);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_structura is null)
                return;

            var geo = Geometrie();
            geo.DeseneazaCaroserie(g, LogicalToDeviceUnits);
            var locuri = _structura.Locuri.ToDictionary(l => (l.Rand, l.Coloana));

            using var fontNumar = Tema.FontIngrosat(Math.Clamp(geo.MarimeLoc / DeviceDpi * 96f / 3.2f, 7f, 11f));
            using var punctat = new Pen(Tema.Bordura, LogicalToDeviceUnits(1)) { DashStyle = DashStyle.Dash };

            for (var r = 1; r <= _structura.Randuri; r++)
            {
                for (var c = 1; c <= _structura.Coloane; c++)
                {
                    var zona = geo.Loc(r, c);
                    var subMouse = _subMouse == (r, c);
                    using var cale = Desen.Dreptunghi(zona, geo.MarimeLoc * 0.18f);

                    if (locuri.TryGetValue((r, c), out var loc))
                    {
                        using var b = new SolidBrush(subMouse ? Color.FromArgb(254, 226, 226) : Tema.PrimarDeschis);
                        using var p = new Pen(subMouse ? Tema.Eroare : Tema.Primar, LogicalToDeviceUnits(1));
                        g.FillPath(b, cale);
                        g.DrawPath(p, cale);
                        Desen.TextCentrat(g, loc.NumarLoc.ToString(), fontNumar, Tema.PrimarInchis, zona);
                    }
                    else
                    {
                        if (subMouse)
                        {
                            using var b = new SolidBrush(Tema.LocLiber);
                            g.FillPath(b, cale);
                        }
                        g.DrawPath(punctat, cale);
                        if (subMouse)
                            Desen.TextCentrat(g, "+", fontNumar, Tema.Succes, zona);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var p = _structura is null ? null : Geometrie().Pozitie(e.Location, _structura.Randuri);
            Cursor = p is null ? Cursors.Default : Cursors.Hand;
            if (p != _subMouse)
            {
                _subMouse = p;
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
            if (e.Button != MouseButtons.Left || _structura is null)
                return;
            if (Geometrie().Pozitie(e.Location, _structura.Randuri) is not { } p)
                return;

            AutobuzService.ComutaLoc(_structura, p.Rand, p.Coloana);
            Invalidate();
            StructuraSchimbata?.Invoke(this, EventArgs.Empty);
        }
    }
}
