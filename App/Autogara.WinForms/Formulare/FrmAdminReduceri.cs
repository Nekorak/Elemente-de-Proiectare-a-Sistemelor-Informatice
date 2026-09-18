using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
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

        private void AfiseazaSelectia()
        {
            _selectat = gridLista.Selectat<TipReducereRand>();
            errorProvider.Clear();
            lblEditareTitlu.Text = _selectat is null ? "Reducere nouă" : "Editare reducere";
            txtDenumire.Text = _selectat?.Denumire ?? "";
            numProcent.Value = _selectat?.ProcentReducere ?? 0;
            chkActiv.Checked = _selectat?.Activ ?? true;
            chkActiv.Enabled = _selectat is not null;
        }

        private void btnNou_Click(object sender, EventArgs e)
        {
            gridLista.ClearSelection();
            gridLista.CurrentCell = null;
            AfiseazaSelectia();
            txtDenumire.Focus();
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider).Obligatoriu(txtDenumire, "Denumirea").Verifica(this))
                return;

            var date = new TipReducereEditare { Denumire = txtDenumire.Text, ProcentReducere = numProcent.Value };
            await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var id = _selectat?.TipReducereID ?? 0;
                if (_selectat is null)
                    id = await Aplicatie.Backend.Reduceri.AdaugaAsync(date);
                else
                    await Aplicatie.Backend.Reduceri.ActualizeazaAsync(id, date, chkActiv.Checked);
                if (!chkActiv.Checked && !chkInactive.Checked)
                    chkInactive.Checked = true;
                else
                    await ReincarcaAsync(id);
            });
        }
    }
}
