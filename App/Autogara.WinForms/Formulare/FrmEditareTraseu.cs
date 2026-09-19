using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru un traseu nou sau modificarea unuia existent: denumirea si opririle, in ordine.</summary>
    public partial class FrmEditareTraseu : FormAutogara
    {
        private readonly TraseuRand _traseu;

        /// <param name="traseu">Traseul de modificat; null pentru unul nou.</param>
        public FrmEditareTraseu(TraseuRand traseu)
        {
            InitializeComponent();
            _traseu = traseu;

            Text = lblTitlu.Text = traseu is null ? "Traseu nou" : $"Modificare traseu — {traseu.Denumire}";
            txtDenumire.Text = traseu?.Denumire ?? "";
        }

        public int IdSalvat { get; private set; }

        private async void FrmEditareTraseu_Load(object sender, EventArgs e)
        {
            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var statii = await Aplicatie.Backend.Statii.ListeazaAsync();
                cmbStatie.DataSource = statii.Select(s => new ElementLista<int>(s.StatieID, s.Nume)).ToList();

                if (_traseu is null)
                    return;

                var detalii = await Aplicatie.Backend.Trasee.DetaliiAsync(_traseu.TraseuID);
                foreach (var o in detalii.Opriri)
                    lstOpriri.Items.Add(new ElementLista<int>(o.StatieID, o.DistantaDePrecedentaKm is { } km ? $"{o.Statie}   (+{km:0.##} km)" : o.Statie));
                lblDistanta.Text = detalii.DistantaTotalaKm is { } total ? $"Distanța totală pe hartă: {total:0.##} km" : "";
            });

            // Fara statii sau fara opririle traseului nu are rost editarea.
            if (!reusit)
                DialogResult = DialogResult.Cancel;
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
            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                if (_traseu is null)
                    IdSalvat = await Aplicatie.Backend.Trasee.CreeazaAsync(txtDenumire.Text, statii);
                else
                {
                    await Aplicatie.Backend.Trasee.ActualizeazaAsync(_traseu.TraseuID, txtDenumire.Text, statii);
                    IdSalvat = _traseu.TraseuID;
                }
            });

            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
