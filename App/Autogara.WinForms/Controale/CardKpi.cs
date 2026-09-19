using System.ComponentModel;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>
    /// Cartonas de dashboard, desenat ca o tablita de peron: banda colorata in stanga,
    /// eticheta scurta cu majuscule si cifra mare in Bahnschrift.
    /// </summary>
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
            var w = ClientSize.Width;
            var h = ClientSize.Height;

            using (var fundal = new SolidBrush(Tema.Suprafata))
                g.FillRectangle(fundal, 0, 0, w, h);
            using (var bordura = new Pen(Tema.Bordura))
                g.DrawRectangle(bordura, 0, 0, w - 1, h - 1);

            var banda = LogicalToDeviceUnits(6);
            using (var accent = new SolidBrush(_accent))
                g.FillRectangle(accent, 0, 0, banda, h);

            var pad = LogicalToDeviceUnits(14);
            var x = banda + pad;
            var latime = w - x - pad;

            // Eticheta sus, iconita mica in dreapta ei.
            var mi = LogicalToDeviceUnits(16);
            if (Iconite.Exista(_icon))
                g.DrawImage(Iconite.Deseneaza(_icon, mi, Tema.TextSecundar), w - pad - mi, pad, mi, mi);

            using var fontTitlu = Tema.FontIndicator(8.5f);
            using var fontValoare = Tema.FontIngrosat(22f);
            TextRenderer.DrawText(g, _titlu.ToUpperInvariant(), fontTitlu, new Rectangle(x, pad, latime - mi - 4, mi + 2),
                Tema.TextSecundar, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, _valoare, fontValoare, new Rectangle(x, pad + mi, latime, h - pad * 2 - mi),
                Tema.Text, TextFormatFlags.Left | TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }
    }
}
