using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Dialog pentru un text scurt (ex. motivul anularii unei curse).</summary>
    public partial class FrmIntrebare : FormAutogara
    {
        public FrmIntrebare(string titlu, string intrebare)
        {
            InitializeComponent();
            Text = titlu;
            lblIntrebare.Text = intrebare;
        }

        public string Raspuns => txtRaspuns.Text.Trim();

        public static string Cere(IWin32Window proprietar, string titlu, string intrebare)
        {
            using var f = new FrmIntrebare(titlu, intrebare);
            return f.ShowDialog(proprietar) == DialogResult.OK ? f.Raspuns : null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider).Obligatoriu(txtRaspuns, "Răspunsul").Verifica(this))
                return;
            DialogResult = DialogResult.OK;
        }
    }
}
