using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Istoricul lucrarilor pe autobuze, alertele de ITP si inregistrarea unui ITP nou.</summary>
    public partial class FrmAdminMentenanta : FormAutogara
    {
        private bool _seIncarca;

        public FrmAdminMentenanta()
        {
            InitializeComponent();
        }

        private async void FrmAdminMentenanta_Load(object sender, EventArgs e)
        {
            var azi = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            dtpData.Value = dtpItpInspectie.Value = azi;
            dtpItpExpirare.Value = azi.AddYears(1);
            dtpData.MaxDate = azi;

            await Mesaje.RuleazaAsync(btnReincarca, async () =>
            {
                var autobuze = await Aplicatie.Backend.Autobuze.ListeazaAsync(includeInactive: true);
                var elemente = autobuze.Select(a => new ElementLista<int>(a.AutobuzID, $"{a.NrInmatriculare} — {a.Model}")).ToList();
                _seIncarca = true;
                var filtru = elemente.Select(x => new ElementLista<int?>(x.Valoare, x.Text)).ToList();
                filtru.Insert(0, new ElementLista<int?>(null, "(toate autobuzele)"));
                cmbFiltru.DataSource = filtru;
                cmbAutobuz.DataSource = elemente.ToList();
                _seIncarca = false;
                await ReincarcaAsync();
            });
        }

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, ReincarcaAsync);

        private async void cmbFiltru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_seIncarca)
                await Mesaje.RuleazaAsync(cmbFiltru, ReincarcaAsync);
        }

        private async Task ReincarcaAsync()
        {
            var autobuz = (cmbFiltru.SelectedItem as ElementLista<int?>)?.Valoare;
            gridLucrari.Afiseaza(await Aplicatie.Backend.Mentenanta.ListeazaAsync(autobuz));
            var itp = await Aplicatie.Backend.Mentenanta.AutobuzeCuItpApropiatAsync(30);
            gridItp.Afiseaza(itp);
            tpItp.Text = itp.Count == 0 ? "ITP" : $"ITP ({itp.Count})";
        }

        private void gridItp_SelectionChanged(object sender, EventArgs e)
        {
            // Alegerea unui autobuz din lista ITP il preselecteaza in formularul de inregistrare.
            if (gridItp.Selectat<ItpRand>() is { } r)
                SelecteazaAutobuz(r.AutobuzID);
        }

        private void SelecteazaAutobuz(int autobuzId)
        {
            foreach (ElementLista<int> e in cmbAutobuz.Items)
                if (e.Valoare == autobuzId)
                    cmbAutobuz.SelectedItem = e;
        }

        private void chkKm_CheckedChanged(object sender, EventArgs e) => numKm.Enabled = chkKm.Checked;

        private async void btnAdauga_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Conditie(cmbAutobuz.SelectedItem is not null, cmbAutobuz, "Alegeți autobuzul.")
                    .Obligatoriu(txtTipLucrare, "Tipul lucrării")
                    .Verifica(this))
                return;

            var date = new MentenantaEditare
            {
                AutobuzID = ((ElementLista<int>)cmbAutobuz.SelectedItem).Valoare,
                TipLucrare = txtTipLucrare.Text,
                Data = DateOnly.FromDateTime(dtpData.Value),
                Kilometraj = chkKm.Checked ? (int)numKm.Value : null,
                Observatii = txtObservatii.Text,
            };

            await Mesaje.RuleazaAsync(btnAdauga, async () =>
            {
                await Aplicatie.Backend.Mentenanta.AdaugaInregistrareAsync(date);
                txtTipLucrare.Clear();
                txtObservatii.Clear();
                tabLista.SelectedTab = tpLucrari;
                await ReincarcaAsync();
            });
        }

        private async void btnItp_Click(object sender, EventArgs e)
        {
            if (cmbAutobuz.SelectedItem is not ElementLista<int> autobuz)
            {
                Mesaje.Atentie(this, "Alegeți autobuzul.");
                return;
            }

            var km = chkKm.Checked ? (int?)numKm.Value : null;
            await Mesaje.RuleazaAsync(btnItp, async () =>
            {
                await Aplicatie.Backend.Mentenanta.InregistreazaItpAsync(autobuz.Valoare,
                    DateOnly.FromDateTime(dtpItpInspectie.Value), DateOnly.FromDateTime(dtpItpExpirare.Value), km);
                Mesaje.Info(this, $"ITP înregistrat pentru {autobuz.Text}, valabil până la {dtpItpExpirare.Value:dd.MM.yyyy}.");
                await ReincarcaAsync();
            });
        }
    }
}
