using System.ComponentModel;
using System.Drawing.Drawing2D;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>Cartonas de dashboard: iconita, titlu si o valoare mare.</summary>
    public class CardKpi : Control
    {
        private string _titlu = "Titlu";
        private string _valoare = "–";
        private string _icon = string.Empty;
        private Color _accent = Tema.Primar;

        public CardKpi()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(220, 96);
            TabStop = false;
        }

        [Category("Autogara"), DefaultValue("Titlu")]
        public string Titlu { get => _titlu; set { _titlu = value; Invalidate(); } }

        [Category("Autogara"), DefaultValue("–")]
        public string Valoare { get => _valoare; set { _valoare = value; Invalidate(); } }

        [Category("Autogara"), DefaultValue("")]
        public string Icon { get => _icon; set { _icon = value ?? string.Empty; Invalidate(); } }

        [Category("Autogara")]
        public Color Accent { get => _accent; set { _accent = value; Invalidate(); } }

        private bool ShouldSerializeAccent() => _accent != Tema.Primar;
        private void ResetAccent() => Accent = Tema.Primar;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var r = new RectangleF(0.5f, 0.5f, ClientSize.Width - 1.5f, ClientSize.Height - 1.5f);
            using (var cale = Desen.Dreptunghi(r, LogicalToDeviceUnits(8)))
            using (var fundal = new SolidBrush(Tema.Suprafata))
            using (var bordura = new Pen(Tema.Bordura))
            {
                g.FillPath(fundal, cale);
                g.DrawPath(bordura, cale);
            }

            var pad = LogicalToDeviceUnits(16);
            var cerc = LogicalToDeviceUnits(40);
            var yCerc = (ClientSize.Height - cerc) / 2;
            using (var fundalIcon = new SolidBrush(Color.FromArgb(28, _accent)))
                g.FillEllipse(fundalIcon, pad, yCerc, cerc, cerc);
            if (Iconite.Exista(_icon))
            {
                var mi = LogicalToDeviceUnits(20);
                g.DrawImage(Iconite.Deseneaza(_icon, mi, _accent), pad + (cerc - mi) / 2, yCerc + (cerc - mi) / 2, mi, mi);
            }

            var x = pad * 2 + cerc - LogicalToDeviceUnits(4);
            var latime = ClientSize.Width - x - pad / 2;
            using var fontTitlu = Tema.FontNormal(9f);
            using var fontValoare = Tema.FontIngrosat(17f);
            TextRenderer.DrawText(g, _titlu, fontTitlu, new Rectangle(x, pad - 2, latime, ClientSize.Height / 2 - pad + 4),
                Tema.TextSecundar, TextFormatFlags.Left | TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(g, _valoare, fontValoare, new Rectangle(x, ClientSize.Height / 2, latime, ClientSize.Height / 2 - pad / 2),
                Tema.Text, TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);
        }
    }
}
