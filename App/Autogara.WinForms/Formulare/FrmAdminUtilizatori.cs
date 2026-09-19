using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Conturile aplicatiei, cererile de resetare a parolei si parolele temporare.
    /// Conturile se adauga si se modifica in <see cref="FrmEditareUtilizator"/>.
    /// </summary>
    public partial class FrmAdminUtilizatori : FormAutogara
    {
        private UtilizatorRand _selectat;
        private bool _seIncarca;

        public FrmAdminUtilizatori()
        {
            InitializeComponent();
        }

        private async void FrmAdminUtilizatori_Load(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());

        private async void chkInactive_CheckedChanged(object sender, EventArgs e) => await Mesaje.RuleazaAsync(chkInactive, () => ReincarcaAsync());

        private async Task ReincarcaAsync(Guid? selecteaza = null)
        {
            _seIncarca = true;
            try
            {
                var utilizatori = await Aplicatie.Backend.Utilizatori.ListeazaAsync(doarActivi: !chkInactive.Checked);
                gridLista.Afiseaza(utilizatori, u => u.UtilizatorID);
                if (selecteaza is { } id)
                    gridLista.SelecteazaRand<UtilizatorRand>(u => u.UtilizatorID == id);

                var cereri = await Aplicatie.Backend.Auth.CereriResetareAsync();
                gridCereri.Afiseaza(cereri);
                tpCereri.Text = cereri.Count == 0 ? "Cereri de resetare" : $"Cereri de resetare ({cereri.Count})";
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
            _selectat = gridLista.Selectat<UtilizatorRand>();
            btnModifica.Enabled = btnActiv.Enabled = btnReseteaza.Enabled = _selectat is not null;
            btnActiv.Text = _selectat?.Activ == false ? " Reactivează" : " Dezactivează";
            btnActiv.Icon = _selectat?.Activ == false ? "user-check" : "user-x";
        }

        private async void btnAdauga_Click(object sender, EventArgs e) => await DeschideEditareaAsync(null);

        private async void btnModifica_Click(object sender, EventArgs e)
        {
            if (_selectat is not null)
                await DeschideEditareaAsync(_selectat);
        }

        private async Task DeschideEditareaAsync(UtilizatorRand utilizator)
        {
            Guid id;
            using (var f = new FrmEditareUtilizator(utilizator))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
                id = f.IdSalvat;
            }
            tabLista.SelectedTab = tpUtilizatori;
            await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync(id));
        }

        private async void btnActiv_Click(object sender, EventArgs e)
        {
            if (_selectat is null)
                return;

            var u = _selectat;
            if (u.Activ && !Mesaje.Confirma(this, $"Dezactivați contul {u.NumeUtilizator}? Rezervările lui în curs se eliberează."))
                return;

            await Mesaje.RuleazaAsync(btnActiv, async () =>
            {
                if (u.Activ)
                    await Aplicatie.Backend.Utilizatori.DezactiveazaAsync(u.UtilizatorID);
                else
                    await Aplicatie.Backend.Utilizatori.ReactiveazaAsync(u.UtilizatorID);

                if (u.Activ && !chkInactive.Checked)
                    chkInactive.Checked = true;
                else
                    await ReincarcaAsync(u.UtilizatorID);
            });
        }

        private async void btnReseteaza_Click(object sender, EventArgs e)
        {
            if (_selectat is not null)
                await ReseteazaAsync(btnReseteaza, _selectat.UtilizatorID, _selectat.NumeUtilizator);
        }

        private async void btnReseteazaCerere_Click(object sender, EventArgs e)
        {
            var cerere = gridCereri.Selectat<CerereResetareParola>();
            if (cerere?.UtilizatorID is { } id)
                await ReseteazaAsync(btnReseteazaCerere, id, cerere.NumeUtilizator);
            else
                Mesaje.Atentie(this, "Alegeți o cerere din listă.");
        }

        private async Task ReseteazaAsync(Control sursa, Guid id, string nume)
        {
            if (!Mesaje.Confirma(this, $"Generați o parolă temporară pentru {nume}? Parola actuală nu va mai funcționa."))
                return;

            await Mesaje.RuleazaAsync(sursa, async () =>
            {
                var parola = await Aplicatie.Backend.Auth.ReseteazaParolaAsync(id);
                Clipboard.SetText(parola);
                Mesaje.Info(this, $"Parola temporară pentru {nume}:\n\n{parola}\n\nA fost copiată în clipboard. Comunicați-o utilizatorului; o poate schimba după autentificare.");
                await ReincarcaAsync(id);
            });
        }
    }
}
