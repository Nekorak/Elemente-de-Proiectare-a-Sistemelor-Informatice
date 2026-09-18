using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    public partial class FrmSchimbaParola : FormAutogara
    {
        public FrmSchimbaParola()
        {
            InitializeComponent();
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtParolaVeche, "Parola actuală")
                    .Obligatoriu(txtParolaNoua, "Parola nouă")
                    .Conditie(txtParolaNoua.Text == txtConfirmare.Text, txtConfirmare, "Confirmarea nu coincide cu parola nouă.")
                    .Verifica(this))
                return;

            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, () =>
                Aplicatie.Backend.Auth.SchimbaParolaAsync(txtParolaVeche.Text, txtParolaNoua.Text, txtConfirmare.Text));

            if (reusit)
            {
                Mesaje.Info(this, "Parola a fost schimbată.");
                DialogResult = DialogResult.OK;
            }
        }
    }
}
