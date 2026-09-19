using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Lista soferilor; adaugarea si modificarea se fac in <see cref="FrmEditareSofer"/>.</summary>
    public partial class FrmAdminSoferi : FormAutogara
    {
        private SoferRand _selectat;
        private bool _seIncarca;

        public FrmAdminSoferi()
        {
            InitializeComponent();
        }

        private async void FrmAdminSoferi_Load(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void chkInactive_CheckedChanged(object sender, EventArgs e) => await Mesaje.RuleazaAsync(chkInactive, () => ReincarcaAsync());

        private async Task ReincarcaAsync(int? selecteaza = null)
        {
            _seIncarca = true;
            try
            {
                gridLista.Afiseaza(await Aplicatie.Backend.Soferi.ListeazaAsync(chkInactive.Checked), s => s.SoferID);
                if (selecteaza is { } id)
                    gridLista.SelecteazaRand<SoferRand>(s => s.SoferID == id);
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
            _selectat = gridLista.Selectat<SoferRand>();
            btnModifica.Enabled = btnActiv.Enabled = _selectat is not null;
            btnActiv.Text = _selectat?.Activ == false ? " Reactivează" : " Dezactivează";
            btnActiv.Icon = _selectat?.Activ == false ? "user-check" : "user-x";
        }

        private async void btnAdauga_Click(object sender, EventArgs e) => await DeschideEditareaAsync(null);

        private async void btnModifica_Click(object sender, EventArgs e)
        {
            if (_selectat is not null)
                await DeschideEditareaAsync(_selectat);
        }

        private async Task DeschideEditareaAsync(SoferRand sofer)
        {
            int id;
            using (var f = new FrmEditareSofer(sofer))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
                id = f.IdSalvat;
            }
            await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync(id));
        }

        private async void btnActiv_Click(object sender, EventArgs e)
        {
            if (_selectat is null)
                return;
            var activ = !_selectat.Activ;
            if (!activ && !Mesaje.Confirma(this, $"Dezactivați șoferul {_selectat.NumeComplet}?"))
                return;

            var id = _selectat.SoferID;
            await Mesaje.RuleazaAsync(btnActiv, async () =>
            {
                await Aplicatie.Backend.Soferi.SeteazaActivAsync(id, activ);
                if (!activ && !chkInactive.Checked)
                    chkInactive.Checked = true; // ca sa ramana vizibil in lista
                else
                    await ReincarcaAsync(id);
            });
        }
    }
}
