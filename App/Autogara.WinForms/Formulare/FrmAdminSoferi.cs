using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
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

        private void AfiseazaSelectia()
        {
            _selectat = gridLista.Selectat<SoferRand>();
            errorProvider.Clear();
            lblEditareTitlu.Text = _selectat is null ? "Șofer nou" : "Editare șofer";
            txtNume.Text = _selectat?.Nume ?? "";
            txtPrenume.Text = _selectat?.Prenume ?? "";
            txtNrPermis.Text = _selectat?.NrPermis ?? "";
            txtTelefon.Text = _selectat?.Telefon ?? "";
            btnActiv.Visible = _selectat is not null;
            btnActiv.Text = _selectat?.Activ == false ? " Reactivează" : " Dezactivează";
            btnActiv.Icon = _selectat?.Activ == false ? "user-check" : "user-x";
        }

        private void btnNou_Click(object sender, EventArgs e)
        {
            gridLista.ClearSelection();
            gridLista.CurrentCell = null;
            AfiseazaSelectia();
            txtNume.Focus();
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtNume, "Numele")
                    .Obligatoriu(txtPrenume, "Prenumele")
                    .Obligatoriu(txtNrPermis, "Numărul permisului")
                    .Telefon(txtTelefon)
                    .Verifica(this))
                return;

            var date = new SoferEditare { Nume = txtNume.Text, Prenume = txtPrenume.Text, NrPermis = txtNrPermis.Text, Telefon = txtTelefon.Text };
            await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var id = _selectat?.SoferID ?? 0;
                if (_selectat is null)
                    id = await Aplicatie.Backend.Soferi.AdaugaAsync(date);
                else
                    await Aplicatie.Backend.Soferi.ActualizeazaAsync(id, date);
                await ReincarcaAsync(id);
            });
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
