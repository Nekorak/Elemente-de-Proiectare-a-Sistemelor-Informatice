using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Lista tipurilor de reducere; adaugarea si modificarea se fac in <see cref="FrmEditareReducere"/>.</summary>
    public partial class FrmAdminReduceri : FormAutogara
    {
        private TipReducereRand _selectat;
        private bool _seIncarca;

        public FrmAdminReduceri()
        {
            InitializeComponent();
        }

        private async void FrmAdminReduceri_Load(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void chkInactive_CheckedChanged(object sender, EventArgs e) => await Mesaje.RuleazaAsync(chkInactive, () => ReincarcaAsync());

        private async Task ReincarcaAsync(int? selecteaza = null)
        {
            _seIncarca = true;
            try
            {
                gridLista.Afiseaza(await Aplicatie.Backend.Reduceri.ListeazaAsync(chkInactive.Checked), r => r.TipReducereID);
                if (selecteaza is { } id)
                    gridLista.SelecteazaRand<TipReducereRand>(r => r.TipReducereID == id);
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
            _selectat = gridLista.Selectat<TipReducereRand>();
            btnModifica.Enabled = _selectat is not null;
        }

        private async void btnAdauga_Click(object sender, EventArgs e) => await DeschideEditareaAsync(null);

        private async void btnModifica_Click(object sender, EventArgs e)
        {
            if (_selectat is not null)
                await DeschideEditareaAsync(_selectat);
        }

        private async Task DeschideEditareaAsync(TipReducereRand reducere)
        {
            int id;
            bool activa;
            using (var f = new FrmEditareReducere(reducere))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
                (id, activa) = (f.IdSalvat, f.Activa);
            }

            await Mesaje.RuleazaAsync(btnReincarca, async () =>
            {
                if (!activa && !chkInactive.Checked)
                    chkInactive.Checked = true; // ca sa ramana vizibila in lista
                else
                    await ReincarcaAsync(id);
            });
        }
    }
}
