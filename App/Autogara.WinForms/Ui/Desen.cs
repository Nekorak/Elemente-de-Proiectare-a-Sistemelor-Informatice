using System.Drawing.Drawing2D;

namespace Autogara.WinForms.Ui
{
    internal static class Desen
    {
        public static GraphicsPath Dreptunghi(RectangleF r, float raza)
        {
            var d = Math.Min(raza * 2, Math.Min(r.Width, r.Height));
            var cale = new GraphicsPath();
            if (d <= 0)
            {
                cale.AddRectangle(r);
                return cale;
            }

            cale.AddArc(r.X, r.Y, d, d, 180, 90);
            cale.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            cale.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            cale.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            cale.CloseFigure();
            return cale;
        }

        public static void TextCentrat(Graphics g, string text, Font font, Color culoare, RectangleF r)
        {
            TextRenderer.DrawText(g, text, font, Rectangle.Round(r), culoare,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
        }
    }
}
