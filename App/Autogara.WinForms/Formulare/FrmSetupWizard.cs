using Autogara.Business;
using Autogara.Common.Configurare;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Configurarea initiala a statiei: adresele serverului SQL si ale file server-ului.</summary>
    public partial class FrmSetupWizard : FormAutogara
    {
        private readonly AppSettingsStore _store;
        private readonly AppSettings _setari;

        public FrmSetupWizard(AppSettingsStore store, AppSettings setari)
        {
            InitializeComponent();
            _store = store;
            _setari = setari;
        }

        private void FrmSetupWizard_Load(object sender, EventArgs e) => Afiseaza(_setari);

        private void Afiseaza(AppSettings s)
        {
            txtSqlServer.Text = s.Sql.Server;
            numSqlPort.Value = s.Sql.Port;
            txtSqlBaza.Text = s.Sql.BazaDeDate;
            txtSqlUtilizator.Text = s.Sql.Utilizator;
            txtSqlParola.Text = s.Sql.Parola;
            txtSftpHost.Text = s.FileServer.Host;
            numSftpPort.Value = s.FileServer.Port;
            txtSftpUtilizator.Text = s.FileServer.Utilizator;
            txtSftpParola.Text = s.FileServer.Parola;
            txtSftpCale.Text = s.FileServer.CaleBaza;
            numDurataRezervare.Value = s.Aplicatie.DurataRezervareMinute;
        }

        private AppSettings DinFormular()
        {
            _setari.Sql.Server = txtSqlServer.Text.Trim();
            _setari.Sql.Port = (int)numSqlPort.Value;
            _setari.Sql.BazaDeDate = txtSqlBaza.Text.Trim();
            _setari.Sql.Utilizator = txtSqlUtilizator.Text.Trim();
            _setari.Sql.Parola = txtSqlParola.Text;
            _setari.FileServer.Host = txtSftpHost.Text.Trim();
            _setari.FileServer.Port = (int)numSftpPort.Value;
            _setari.FileServer.Utilizator = txtSftpUtilizator.Text.Trim();
            _setari.FileServer.Parola = txtSftpParola.Text;
            _setari.FileServer.CaleBaza = txtSftpCale.Text.Trim();
            _setari.Aplicatie.DurataRezervareMinute = (int)numDurataRezervare.Value;
            return _setari;
        }

        private void btnImplicite_Click(object sender, EventArgs e) => Afiseaza(AppSettings.Implicite());

        private async void btnTesteaza_Click(object sender, EventArgs e)
        {
            lblStareSql.Text = "Se testează...";
            lblStareSftp.Text = "Se testează...";
            iconSql.Icon = iconSftp.Icon = "clock";
            iconSql.Culoare = iconSftp.Culoare = Tema.TextSecundar;

            await Mesaje.RuleazaAsync(btnTesteaza, async () =>
            {
                var r = await AutogaraBackend.TesteazaConfigurareaAsync(DinFormular());
                AfiseazaRezultat(iconSql, lblStareSql, r.BazaDeDate, r.MesajBazaDeDate);
                AfiseazaRezultat(iconSftp, lblStareSftp, r.FileServer, r.MesajFileServer);
            });
        }

        private static void AfiseazaRezultat(Controale.IconImagine icon, Label eticheta, bool reusit, string mesaj)
        {
            icon.Icon = reusit ? "circle-check" : "circle-x";
            icon.Culoare = reusit ? Tema.Succes : Tema.Eroare;
            eticheta.Text = mesaj;
        }

        private void btnSalveaza_Click(object sender, EventArgs e)
        {
            var setari = DinFormular();
            var erori = setari.Valideaza();
            if (erori.Count > 0)
            {
                Mesaje.Atentie(this, string.Join(Environment.NewLine, erori));
                return;
            }

            try
            {
                _store.Salveaza(setari);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mesaje.Eroare(this, ex);
            }
        }
    }
}
