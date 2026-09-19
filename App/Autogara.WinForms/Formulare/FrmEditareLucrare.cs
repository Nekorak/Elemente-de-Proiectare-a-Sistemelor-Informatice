using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru inregistrarea unei lucrari de mentenanta.</summary>
    public partial class FrmEditareLucrare : FormAutogara
    {
        /// <param name="autobuze">Autobuzele din care se alege.</param>
        /// <param name="autobuzId">Autobuzul propus (ex. cel din filtru); null = niciunul.</param>
        public FrmEditareLucrare(IReadOnlyList<ElementLista<int>> autobuze, int? autobuzId)
        {
            InitializeComponent();

            var azi = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            dtpData.MaxDate = azi;
            dtpData.Value = azi;

            cmbAutobuz.DataSource = autobuze.ToList();
            cmbAutobuz.SelectedItem = autobuze.FirstOrDefault(a => a.Valoare == autobuzId);
        }

        private void chkKm_CheckedChanged(object sender, EventArgs e) => numKm.Enabled = chkKm.Checked;

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Conditie(cmbAutobuz.SelectedItem is not null, cmbAutobuz, "Alegeți autobuzul.")
                    .Obligatoriu(txtTipLucrare, "Tipul lucrării")
                    .Verifica(this))
                return;

            var date = new MentenantaEditare
            {
                AutobuzID = ((ElementLista<int>)cmbAutobuz.SelectedItem).Valoare,
                TipLucrare = txtTipLucrare.Text,
                Data = DateOnly.FromDateTime(dtpData.Value),
                Kilometraj = chkKm.Checked ? (int)numKm.Value : null,
                Observatii = txtObservatii.Text,
            };

            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, () => Aplicatie.Backend.Mentenanta.AdaugaInregistrareAsync(date));
            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
