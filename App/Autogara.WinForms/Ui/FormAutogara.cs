namespace Autogara.WinForms.Ui
{
    /// <summary>Baza tuturor ferestrelor: iconita aplicatiei (desenata din SVG, nu dintr-un .ico).</summary>
    public class FormAutogara : Form
    {
        public FormAutogara()
        {
            Icon = Iconite.IconAplicatie;
        }
    }
}
