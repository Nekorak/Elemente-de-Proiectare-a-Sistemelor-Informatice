using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Traseele: denumirea si statiile, in ordinea opririlor.</summary>
    public partial class FrmAdminTrasee : FormAutogara
    {
        private TraseuRand _selectat;
        private bool _seIncarca;

        public FrmAdminTrasee()
        {
            InitializeComponent();
        }

        private async void FrmAdminTrasee_Load(object sender, EventArgs e) =>
            await Mesaje.RuleazaAsync(btnReincarca, async () =>
            {
                var statii = await Aplicatie.Backend.Statii.ListeazaAsync();
                cmbStatie.DataSource = statii.Select(s => new ElementLista<int>(s.StatieID, s.Nume)).ToList();
                await ReincarcaAsync();
            });

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
            await AfiseazaSelectiaAsync();
        }

        private async void gridLista_SelectionChanged(object sender, EventArgs e)
        {
            if (!_seIncarca)
                await Mesaje.RuleazaAsync(gridLista, AfiseazaSelectiaAsync);
        }

        private async Task AfiseazaSelectiaAsync()
        {
            _selectat = gridLista.Selectat<TraseuRand>();
            errorProvider.Clear();
            lblEditareTitlu.Text = _selectat is null ? "Traseu nou" : "Editare traseu";
            txtDenumire.Text = _selectat?.Denumire ?? "";
            lstOpriri.Items.Clear();
            lblDistanta.Text = "";
            btnActiv.Visible = _selectat is not null;
            btnActiv.Text = _selectat?.Activ == false ? " Reactivează" : " Dezactivează";

            if (_selectat is null)
                return;

            var detalii = await Aplicatie.Backend.Trasee.DetaliiAsync(_selectat.TraseuID);
            foreach (var o in detalii.Opriri)
                lstOpriri.Items.Add(new ElementLista<int>(o.StatieID, o.DistantaDePrecedentaKm is { } km ? $"{o.Statie}   (+{km:0.##} km)" : o.Statie));
            lblDistanta.Text = detalii.DistantaTotalaKm is { } total ? $"Distanța totală pe hartă: {total:0.##} km" : "";
        }

        private void btnNou_Click(object sender, EventArgs e)
        {
            gridLista.ClearSelection();
            gridLista.CurrentCell = null;
            _ = AfiseazaSelectiaAsync();
            txtDenumire.Focus();
        }

        private void btnAdaugaOprire_Click(object sender, EventArgs e)
        {
            if (cmbStatie.SelectedItem is not ElementLista<int> statie)
                return;
            if (lstOpriri.Items.Cast<ElementLista<int>>().Any(o => o.Valoare == statie.Valoare))
            {
                Mesaje.Atentie(this, "Stația este deja în traseu.");
                return;
            }
            lstOpriri.Items.Add(new ElementLista<int>(statie.Valoare, statie.Text));
            lblDistanta.Text = "Distanța se recalculează după salvare.";
        }

        private void btnScoate_Click(object sender, EventArgs e)
        {
            if (lstOpriri.SelectedIndex >= 0)
                lstOpriri.Items.RemoveAt(lstOpriri.SelectedIndex);
        }

        private void btnSus_Click(object sender, EventArgs e) => Muta(-1);

        private void btnJos_Click(object sender, EventArgs e) => Muta(1);

        private void Muta(int directie)
        {
            var i = lstOpriri.SelectedIndex;
            var j = i + directie;
            if (i < 0 || j < 0 || j >= lstOpriri.Items.Count)
                return;
            var element = lstOpriri.Items[i];
            lstOpriri.Items.RemoveAt(i);
            lstOpriri.Items.Insert(j, element);
            lstOpriri.SelectedIndex = j;
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtDenumire, "Denumirea traseului")
                    .Conditie(lstOpriri.Items.Count >= 2, lstOpriri, "Traseul trebuie să aibă cel puțin două stații.")
                    .Verifica(this))
                return;

            var statii = lstOpriri.Items.Cast<ElementLista<int>>().Select(o => o.Valoare).ToList();
            await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var id = _selectat?.TraseuID ?? 0;
                if (_selectat is null)
                    id = await Aplicatie.Backend.Trasee.CreeazaAsync(txtDenumire.Text, statii);
                else
                    await Aplicatie.Backend.Trasee.ActualizeazaAsync(id, txtDenumire.Text, statii);
                await ReincarcaAsync(id);
            });
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
