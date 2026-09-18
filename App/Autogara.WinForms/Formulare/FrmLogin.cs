using System.Diagnostics;
using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    public partial class FrmLogin : FormAutogara
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            lblVersiune.Text = $"v{Aplicatie.Versiune.ToString(3)}";
            Aplicatie.Backend.Conexiune.ConexiuneSchimbata += Conexiune_Schimbata;
            AfiseazaConexiunea(Aplicatie.Backend.Conexiune.StareCurenta);
        }

        private async void FrmLogin_Shown(object sender, EventArgs e)
        {
            txtUtilizator.Focus();
            await VerificaActualizareAsync();
        }

        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e) =>
            Aplicatie.Backend.Conexiune.ConexiuneSchimbata -= Conexiune_Schimbata;

        private void Conexiune_Schimbata(object sender, StareConexiune stare)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(() => AfiseazaConexiunea(stare));
        }

        private void AfiseazaConexiunea(StareConexiune stare)
        {
            indicatorConexiune.Seteaza(stare);
            toolTip.SetToolTip(indicatorConexiune, indicatorConexiune.Detalii);
        }

        private async void btnAutentificare_Click(object sender, EventArgs e)
        {
            var verificare = new VerificareFormular(errorProvider)
                .Obligatoriu(txtUtilizator, "Numele de utilizator")
                .Obligatoriu(txtParola, "Parola");
            if (!verificare.Verifica(this))
                return;

            var reusit = await Mesaje.RuleazaAsync(btnAutentificare,
                () => Aplicatie.Backend.Auth.AutentificaAsync(txtUtilizator.Text, txtParola.Text));

            if (reusit)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            txtParola.SelectAll();
            txtParola.Focus();
        }

        private void lnkAmUitat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var f = new FrmRecuperareParola(txtUtilizator.Text);
            f.ShowDialog(this);
        }

        private void lnkConfigurare_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var f = new FrmSetupWizard(new Common.Configurare.AppSettingsStore(), Aplicatie.Backend.Setari);
            if (f.ShowDialog(this) != DialogResult.OK)
                return;

            Mesaje.Info(this, "Setările au fost salvate. Aplicația se repornește ca să le folosească.");
            Application.Restart();
        }

        private async Task VerificaActualizareAsync()
        {
            var info = await Aplicatie.Backend.Actualizari.VerificaAsync(Aplicatie.Versiune);
            if (info is null)
                return;

            var mesaj = $"Este disponibilă versiunea {info.Versiune} a aplicației (acum rulați {Aplicatie.Versiune.ToString(3)})." +
                        (string.IsNullOrWhiteSpace(info.Note) ? "" : $"\n\n{info.Note}") +
                        "\n\nO descărcați acum?";
            if (!Mesaje.Confirma(this, mesaj))
                return;

            await Mesaje.RuleazaAsync(this, async () =>
            {
                var cale = await Aplicatie.Backend.Actualizari.DescarcaAsync(info);
                Mesaje.Info(this, $"Actualizarea a fost descărcată:\n{cale}\n\nÎnchideți aplicația și instalați versiunea nouă.");
                Process.Start("explorer.exe", $"/select,\"{cale}\"");
            });
        }
    }
}
