using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Cererea de resetare a parolei; administratorul o vede si genereaza o parola temporara.</summary>
    public partial class FrmRecuperareParola : FormAutogara
    {
        public FrmRecuperareParola(string numeUtilizator)
        {
            InitializeComponent();
            txtUtilizator.Text = numeUtilizator;
        }

        private async void btnTrimite_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtUtilizator, "Numele de utilizator")
                    .Email(txtEmail)
                    .Verifica(this))
                return;

            string raspuns = null;
            var reusit = await Mesaje.RuleazaAsync(btnTrimite, async () =>
                raspuns = await Aplicatie.Backend.Auth.CerereResetareParolaAsync(txtUtilizator.Text, txtEmail.Text));

            if (reusit)
            {
                Mesaje.Info(this, raspuns);
                Close();
            }
        }
    }
}
