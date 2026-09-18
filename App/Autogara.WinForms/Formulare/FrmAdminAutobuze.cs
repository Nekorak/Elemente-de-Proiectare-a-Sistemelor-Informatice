using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    public partial class FrmAdminAutobuze : FormAutogara
    {
        private AutobuzRand _selectat;
        private bool _seIncarca;

        public FrmAdminAutobuze()
        {
            InitializeComponent();
        }

        private async void FrmAdminAutobuze_Load(object sender, EventArgs e)
        {
            cmbStatus.DataSource = Enum.GetValues<StatusAutobuz>().Select(s => new ElementLista<StatusAutobuz>(s, Afisare.Text(s))).ToList();
            await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());
        }

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void chkInactive_CheckedChanged(object sender, EventArgs e) => await Mesaje.RuleazaAsync(chkInactive, () => ReincarcaAsync());

        private async Task ReincarcaAsync(int? selecteaza = null)
        {
            _seIncarca = true;
            try
            {
                gridLista.Afiseaza(await Aplicatie.Backend.Autobuze.ListeazaAsync(chkInactive.Checked), a => a.AutobuzID);
                if (selecteaza is { } id)
                    gridLista.SelecteazaRand<AutobuzRand>(a => a.AutobuzID == id);
            }
            finally
            {
                _seIncarca = false;
            }
            AfiseazaSelectia();
        }

        private void gridLista_SelectionChanged(object sender, EventArgs e)
        {
            if (!_seIncarca)
                AfiseazaSelectia();
        }

        private void AfiseazaSelectia()
        {
            _selectat = gridLista.Selectat<AutobuzRand>();
            var nou = _selectat is null;
            errorProvider.Clear();

            lblEditareTitlu.Text = nou ? "Autobuz nou" : "Editare autobuz";
            txtNrInmatriculare.Text = _selectat?.NrInmatriculare ?? "";
            txtModel.Text = _selectat?.Model ?? "";
            numCapacitate.Value = _selectat?.CapacitateLocuri ?? 20;
            numCapacitate.Enabled = nou;
            cmbStatus.SelectedIndex = Array.IndexOf(Enum.GetValues<StatusAutobuz>(), _selectat?.Status ?? StatusAutobuz.Activ);
            chkItp.Checked = nou || _selectat.DataExpirareITP is not null;
            dtpItp.Value = (_selectat?.DataExpirareITP ?? OraLocala.Azi.AddYears(1)).ToDateTime(TimeOnly.MinValue);
            dtpItp.Enabled = chkItp.Checked;
            lblFisier.Text = nou ? "Structura locurilor se creează la salvare (4 pe rând, culoar la mijloc) și se poate modifica apoi."
                                 : $"Structura locurilor: {_selectat.CaleFisierJSON ?? "nesalvată"}";
            btnLocuri.Enabled = !nou;
        }

        private void chkItp_CheckedChanged(object sender, EventArgs e) => dtpItp.Enabled = chkItp.Checked;

        private void btnNou_Click(object sender, EventArgs e)
        {
            gridLista.ClearSelection();
            gridLista.CurrentCell = null;
            AfiseazaSelectia();
            txtNrInmatriculare.Focus();
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtNrInmatriculare, "Numărul de înmatriculare")
                    .Obligatoriu(txtModel, "Modelul")
                    .Verifica(this))
                return;

            var date = new AutobuzEditare
            {
                NrInmatriculare = txtNrInmatriculare.Text,
                Model = txtModel.Text,
                Status = ((ElementLista<StatusAutobuz>)cmbStatus.SelectedItem).Valoare,
                DataExpirareITP = chkItp.Checked ? DateOnly.FromDateTime(dtpItp.Value) : null,
            };

            await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var id = _selectat?.AutobuzID ?? 0;
                if (_selectat is null)
                    id = await Aplicatie.Backend.Autobuze.CreeazaAsync(date, (int)numCapacitate.Value);
                else
                    await Aplicatie.Backend.Autobuze.ActualizeazaAsync(id, date);

                if (date.Status == StatusAutobuz.ScosDinUz && !chkInactive.Checked)
                    chkInactive.Checked = true;
                else
                    await ReincarcaAsync(id);
            });
        }

        private async void btnLocuri_Click(object sender, EventArgs e)
        {
            if (_selectat is null)
                return;

            var id = _selectat.AutobuzID;
            using (var f = new FrmEditorAutobuz(_selectat))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
            }
            await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync(id));
        }
    }
}
