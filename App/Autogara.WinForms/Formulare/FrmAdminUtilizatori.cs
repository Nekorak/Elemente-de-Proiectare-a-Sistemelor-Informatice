using Autogara.Business.Dto;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Conturile aplicatiei, cererile de resetare a parolei si parolele temporare.</summary>
    public partial class FrmAdminUtilizatori : FormAutogara
    {
        private UtilizatorRand _selectat;
        private bool _seIncarca;

        public FrmAdminUtilizatori()
        {
            InitializeComponent();
        }

        private async void FrmAdminUtilizatori_Load(object sender, EventArgs e)
        {
            cmbRol.DataSource = Enum.GetValues<RolTip>().Select(r => new ElementLista<RolTip>(r, r.ToString())).ToList();
            await Mesaje.RuleazaAsync(btnReincarca, () => ReincarcaAsync());
        }

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

        private void AfiseazaSelectia()
        {
            _selectat = gridLista.Selectat<UtilizatorRand>();
            var nou = _selectat is null;
            errorProvider.Clear();

            lblEditareTitlu.Text = nou ? "Utilizator nou" : "Editare utilizator";
            txtNumeUtilizator.Text = _selectat?.NumeUtilizator ?? "";
            txtNume.Text = _selectat?.Nume ?? "";
            txtPrenume.Text = _selectat?.Prenume ?? "";
            txtEmail.Text = _selectat?.Email ?? "";
            txtTelefon.Text = _selectat?.Telefon ?? "";
            cmbRol.SelectedIndex = Array.IndexOf(Enum.GetValues<RolTip>(), _selectat?.Rol ?? RolTip.Casier);
            txtParola.Text = "";
            lblParola.Visible = txtParola.Visible = nou;

            btnActiv.Visible = btnReseteaza.Visible = !nou;
            btnActiv.Text = _selectat?.Activ == false ? " Reactivează" : " Dezactivează";
            btnActiv.Icon = _selectat?.Activ == false ? "user-check" : "user-x";
        }

        private void btnNou_Click(object sender, EventArgs e)
        {
            tabLista.SelectedTab = tpUtilizatori;
            gridLista.ClearSelection();
            gridLista.CurrentCell = null;
            AfiseazaSelectia();
            txtNumeUtilizator.Focus();
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            var nou = _selectat is null;
            var verificare = new VerificareFormular(errorProvider)
                .Obligatoriu(txtNumeUtilizator, "Numele de utilizator")
                .Obligatoriu(txtNume, "Numele")
                .Obligatoriu(txtPrenume, "Prenumele")
                .Email(txtEmail)
                .Telefon(txtTelefon);
            if (nou)
                verificare.Obligatoriu(txtParola, "Parola inițială");
            if (!verificare.Verifica(this))
                return;

            var date = new UtilizatorEditare
            {
                NumeUtilizator = txtNumeUtilizator.Text,
                Nume = txtNume.Text,
                Prenume = txtPrenume.Text,
                Email = txtEmail.Text,
                Telefon = txtTelefon.Text,
                Rol = ((ElementLista<RolTip>)cmbRol.SelectedItem).Valoare,
            };

            await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var id = _selectat?.UtilizatorID ?? Guid.Empty;
                if (nou)
                    id = await Aplicatie.Backend.Utilizatori.AdaugaAsync(date, txtParola.Text);
                else
                    await Aplicatie.Backend.Utilizatori.ActualizeazaAsync(id, date, _selectat.RowVersion);
                await ReincarcaAsync(id);
            });
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
