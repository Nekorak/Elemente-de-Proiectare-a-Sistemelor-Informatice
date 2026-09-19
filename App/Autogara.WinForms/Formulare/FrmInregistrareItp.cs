using Autogara.Common;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru inregistrarea unui ITP nou.</summary>
    public partial class FrmInregistrareItp : FormAutogara
    {
        /// <param name="autobuze">Autobuzele din care se alege.</param>
        /// <param name="autobuzId">Autobuzul propus (ex. cel ales in lista ITP); null = niciunul.</param>
        public FrmInregistrareItp(IReadOnlyList<ElementLista<int>> autobuze, int? autobuzId)
        {
            InitializeComponent();

            var azi = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            dtpInspectie.Value = azi;
            dtpExpirare.Value = azi.AddYears(1);

            cmbAutobuz.DataSource = autobuze.ToList();
            cmbAutobuz.SelectedItem = autobuze.FirstOrDefault(a => a.Valoare == autobuzId);
        }

        private void chkKm_CheckedChanged(object sender, EventArgs e) => numKm.Enabled = chkKm.Checked;

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Conditie(cmbAutobuz.SelectedItem is not null, cmbAutobuz, "Alegeți autobuzul.")
                    .Conditie(dtpExpirare.Value > dtpInspectie.Value, dtpExpirare, "Data expirării trebuie să fie după data inspecției.")
                    .Verifica(this))
                return;

            var autobuz = (ElementLista<int>)cmbAutobuz.SelectedItem;
            var km = chkKm.Checked ? (int?)numKm.Value : null;
            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, () =>
                Aplicatie.Backend.Mentenanta.InregistreazaItpAsync(autobuz.Valoare,
                    DateOnly.FromDateTime(dtpInspectie.Value), DateOnly.FromDateTime(dtpExpirare.Value), km));

            if (reusit)
            {
                Mesaje.Info(this, $"ITP înregistrat pentru {autobuz.Text}, valabil până la {dtpExpirare.Value:dd.MM.yyyy}.");
                DialogResult = DialogResult.OK;
            }
        }
    }
}
