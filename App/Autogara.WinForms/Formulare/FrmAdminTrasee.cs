using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Lista traseelor; adaugarea si modificarea se fac in <see cref="FrmEditareTraseu"/>.</summary>
    public partial class FrmAdminTrasee : FormAutogara
    {
        private TraseuRand _selectat;
        private bool _seIncarca;

        public FrmAdminTrasee()
        {
            InitializeComponent();
        }

        private async void FrmAdminTrasee_Load(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void chkInactive_CheckedChanged(object sender, EventArgs e) => await Mesaje.RuleazaAsync(chkInactive, () => ReincarcaAsync());

        private async Task ReincarcaAsync(int? selecteaza = null)
        {
            _seIncarca = true;
            try
            {
                gridLista.Afiseaza(await Aplicatie.Backend.Trasee.ListeazaAsync(chkInactive.Checked), t => t.TraseuID);
                if (selecteaza is { } id)
                    gridLista.SelecteazaRand<TraseuRand>(t => t.TraseuID == id);
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
            _selectat = gridLista.Selectat<TraseuRand>();
            btnModifica.Enabled = btnActiv.Enabled = _selectat is not null;
            btnActiv.Text = _selectat?.Activ == false ? " Reactivează" : " Dezactivează";
            btnActiv.Icon = _selectat?.Activ == false ? "check" : "ban";
        }

        private async void btnAdauga_Click(object sender, EventArgs e) => await DeschideEditareaAsync(null);

        private async void btnModifica_Click(object sender, EventArgs e)
        {
            if (_selectat is not null)
                await DeschideEditareaAsync(_selectat);
        }

        private async Task DeschideEditareaAsync(TraseuRand traseu)
        {
            int id;
            using (var f = new FrmEditareTraseu(traseu))
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
            var t = _selectat;
            if (t.Activ && !Mesaje.Confirma(this, $"Dezactivați traseul „{t.Denumire}”?"))
                return;

            await Mesaje.RuleazaAsync(btnActiv, async () =>
            {
                await Aplicatie.Backend.Trasee.SeteazaActivAsync(t.TraseuID, !t.Activ);
                if (t.Activ && !chkInactive.Checked)
                    chkInactive.Checked = true;
                else
                    await ReincarcaAsync(t.TraseuID);
            });
        }
    }
}
