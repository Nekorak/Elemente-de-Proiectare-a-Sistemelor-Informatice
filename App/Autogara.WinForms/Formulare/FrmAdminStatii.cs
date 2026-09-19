using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Lista statiilor; adresa si peronul se modifica in <see cref="FrmEditareStatie"/>.
    /// Statiile noi se adauga pe harta.
    /// </summary>
    public partial class FrmAdminStatii : FormAutogara
    {
        private StatieRand _selectat;
        private bool _seIncarca;

        public FrmAdminStatii()
        {
            InitializeComponent();
        }

        private async void FrmAdminStatii_Load(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void chkInactive_CheckedChanged(object sender, EventArgs e) => await Mesaje.RuleazaAsync(chkInactive, () => ReincarcaAsync());

        private async Task ReincarcaAsync(int? selecteaza = null)
        {
            _seIncarca = true;
            try
            {
                gridLista.Afiseaza(await Aplicatie.Backend.Statii.ListeazaAsync(chkInactive.Checked), s => s.StatieID);
                if (selecteaza is { } id)
                    gridLista.SelecteazaRand<StatieRand>(s => s.StatieID == id);
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
            _selectat = gridLista.Selectat<StatieRand>();
            btnModifica.Enabled = _selectat is not null;
        }

        private async void btnModifica_Click(object sender, EventArgs e)
        {
            if (_selectat is null)
                return;

            var id = _selectat.StatieID;
            using (var f = new FrmEditareStatie(_selectat))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
            }
            await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync(id));
        }
    }
}
