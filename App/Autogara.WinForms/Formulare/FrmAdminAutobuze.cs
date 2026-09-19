using Autogara.Business.Dto;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Lista autobuzelor; datele se adauga si se modifica in <see cref="FrmEditareAutobuz"/>,
    /// locurile in <see cref="FrmEditorAutobuz"/>.
    /// </summary>
    public partial class FrmAdminAutobuze : FormAutogara
    {
        private AutobuzRand _selectat;
        private bool _seIncarca;

        public FrmAdminAutobuze()
        {
            InitializeComponent();
        }

        private async void FrmAdminAutobuze_Load(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

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

        private void gridLista_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnModifica.PerformClick();
        }

        private void AfiseazaSelectia()
        {
            _selectat = gridLista.Selectat<AutobuzRand>();
            btnModifica.Enabled = btnLocuri.Enabled = _selectat is not null;
        }

        private async void btnAdauga_Click(object sender, EventArgs e) => await DeschideEditareaAsync(null);

        private async void btnModifica_Click(object sender, EventArgs e)
        {
            if (_selectat is not null)
                await DeschideEditareaAsync(_selectat);
        }

        private async Task DeschideEditareaAsync(AutobuzRand autobuz)
        {
            int id;
            StatusAutobuz status;
            using (var f = new FrmEditareAutobuz(autobuz))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
                (id, status) = (f.IdSalvat, f.Status);
            }

            await Mesaje.RuleazaAsync(btnReincarca, async () =>
            {
                if (status == StatusAutobuz.ScosDinUz && !chkInactive.Checked)
                    chkInactive.Checked = true; // ca sa ramana vizibil in lista
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
