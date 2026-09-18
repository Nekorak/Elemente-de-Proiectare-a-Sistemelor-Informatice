using System.ComponentModel;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>O iconita SVG afisata singura (antete, carduri, mesaje).</summary>
    public class IconImagine : Control
    {
        private string _icon = string.Empty;
        private Color _culoare = Tema.Primar;

        public IconImagine()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(24, 24);
            TabStop = false;
        }

        [Category("Autogara"), DefaultValue("")]
        public string Icon
        {
            get => _icon;
            set { _icon = value ?? string.Empty; Invalidate(); }
        }

        [Category("Autogara")]
        public Color Culoare
        {
            get => _culoare;
            set { _culoare = value; Invalidate(); }
        }

        private bool ShouldSerializeCuloare() => _culoare != Tema.Primar;
        private void ResetCuloare() => Culoare = Tema.Primar;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!Iconite.Exista(_icon))
                return;

            var marime = Math.Min(ClientSize.Width, ClientSize.Height);
            if (marime <= 0)
                return;
            var img = Iconite.Deseneaza(_icon, marime, _culoare);
            e.Graphics.DrawImage(img, (ClientSize.Width - marime) / 2, (ClientSize.Height - marime) / 2, marime, marime);
        }
    }
}
