using System.Drawing.Drawing2D;
using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>ConnectionStatusIndicator: punct colorat + text (online / doar baza de date / mod local).</summary>
    public class IndicatorConexiune : Control
    {
        private StareConexiune _stare;

        public IndicatorConexiune()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(190, 24);
            TabStop = false;
        }

        /// <summary>Textul pentru ToolTip: starea fiecarui server.</summary>
        public string Detalii => _stare is null
            ? "Se verifică conexiunea..."
            : $"Baza de date: {(_stare.BazaDeDate ? "conectată" : "indisponibilă")}\n" +
              $"File server: {(_stare.FileServer ? "conectat" : "indisponibil")}\n" +
              $"Verificat la {_stare.VerificatLa:HH:mm:ss}";

        public void Seteaza(StareConexiune stare)
        {
            _stare = stare;
            AccessibleDescription = Detalii;
            Invalidate();
        }

        private (Color Culoare, string Text) Aspect() => _stare switch
        {
            null => (Tema.TextSecundar, "Se verifică..."),
            { Online: true } => (Tema.Succes, "Online"),
            { BazaDeDate: true } => (Tema.Avertisment, "Fără file server"),
            _ => (Tema.Eroare, "Mod local (offline)"),
        };

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var (culoare, text) = Aspect();
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var d = LogicalToDeviceUnits(10);
            var y = (ClientSize.Height - d) / 2f;
            using (var halou = new SolidBrush(Color.FromArgb(50, culoare)))
                g.FillEllipse(halou, 0, y - d / 4f, d * 1.5f, d * 1.5f);
            using (var punct = new SolidBrush(culoare))
                g.FillEllipse(punct, d / 4f, y, d, d);

            var x = d * 2;
            TextRenderer.DrawText(g, text, Font, new Rectangle(x, 0, ClientSize.Width - x, ClientSize.Height), ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
    }
}
