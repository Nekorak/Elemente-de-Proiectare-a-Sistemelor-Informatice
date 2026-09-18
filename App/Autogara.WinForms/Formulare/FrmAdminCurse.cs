using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Planificarea curselor: creare, modificare, plecare/sosire, anulare cu rambursare.</summary>
    public partial class FrmAdminCurse : FormAutogara
    {
        private CursaRand _selectat;
        private bool _seIncarca;

        public FrmAdminCurse()
        {
            InitializeComponent();
        }

        private async void FrmAdminCurse_Load(object sender, EventArgs e)
        {
            var azi = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            dtpDeLa.Value = azi;
            dtpPanaLa.Value = azi.AddDays(7);

            await Mesaje.RuleazaAsync(btnAfiseaza, async () =>
            {
                var trasee = await Aplicatie.Backend.Trasee.ListeazaAsync();
                var autobuze = await Aplicatie.Backend.Autobuze.ListeazaAsync();
                var soferi = await Aplicatie.Backend.Soferi.ListeazaAsync();

                _seIncarca = true;
                var filtru = trasee.Select(t => new ElementLista<int?>(t.TraseuID, t.Denumire)).ToList();
                filtru.Insert(0, new ElementLista<int?>(null, "(toate traseele)"));
                cmbFiltruTraseu.DataSource = filtru;
                cmbTraseu.DataSource = trasee.Select(t => new ElementLista<int>(t.TraseuID, t.Denumire)).ToList();
                cmbAutobuz.DataSource = autobuze
                    .Where(a => a.Status == StatusAutobuz.Activ)
                    .Select(a => new ElementLista<int>(a.AutobuzID, $"{a.NrInmatriculare} — {a.Model} ({a.CapacitateLocuri} locuri)"))
                    .ToList();
                cmbSofer.DataSource = soferi.Select(s => new ElementLista<int>(s.SoferID, s.NumeComplet)).ToList();
                _seIncarca = false;

                await ReincarcaAsync();
            });
        }

        private async void btnAfiseaza_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnAfiseaza, () => ReincarcaAsync());

        private async Task ReincarcaAsync(int? selecteaza = null)
        {
            _seIncarca = true;
            try
            {
                var traseu = (cmbFiltruTraseu.SelectedItem as ElementLista<int?>)?.Valoare;
                var curse = await Aplicatie.Backend.Curse.ListeazaAsync(DateOnly.FromDateTime(dtpDeLa.Value), DateOnly.FromDateTime(dtpPanaLa.Value), traseu);
                gridLista.Afiseaza(curse, c => c.CursaID);
                if (selecteaza is { } id)
                    gridLista.SelecteazaRand<CursaRand>(c => c.CursaID == id);
                lblNumar.Text = $"{curse.Count} curse";
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
            _selectat = gridLista.Selectat<CursaRand>();
            var c = _selectat;
            errorProvider.Clear();

            lblEditareTitlu.Text = c is null ? "Cursă nouă" : $"Cursa #{c.CursaID} · {Afisare.Text(c.Status)}";
            if (c is not null)
            {
                Selecteaza(cmbTraseu, c.TraseuID);
                Selecteaza(cmbAutobuz, c.AutobuzID);
                Selecteaza(cmbSofer, c.SoferID);
                dtpData.Value = c.DataCursa.ToDateTime(TimeOnly.MinValue);
                dtpPlecare.Value = DateTime.Today.Add(c.OraPlecare.ToTimeSpan());
                dtpSosire.Value = DateTime.Today.Add(c.OraSosireEstimata.ToTimeSpan());
                numPret.Value = c.Pret;
            }
            else
            {
                dtpData.Value = OraLocala.Azi.AddDays(1).ToDateTime(TimeOnly.MinValue);
            }

            var planificata = c is null || c.Status == StatusCursa.Planificata;
            pnlCampuri.Enabled = btnSalveaza.Enabled = planificata;
            pnlStare.Visible = c is not null;
            btnPlecata.Enabled = c?.Status == StatusCursa.Planificata;
            btnFinalizata.Enabled = c?.Status == StatusCursa.InDesfasurare;
            btnAnuleaza.Enabled = c?.Status == StatusCursa.Planificata;
        }

        private static void Selecteaza(ComboBox cmb, int id)
        {
            foreach (ElementLista<int> e in cmb.Items)
            {
                if (e.Valoare == id)
                {
                    cmb.SelectedItem = e;
                    return;
                }
            }
            cmb.SelectedIndex = -1; // ex. autobuz scos din circulatie intre timp
        }

        private void btnNou_Click(object sender, EventArgs e)
        {
            gridLista.ClearSelection();
            gridLista.CurrentCell = null;
            AfiseazaSelectia();
            cmbTraseu.Focus();
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Conditie(cmbTraseu.SelectedItem is not null, cmbTraseu, "Alegeți traseul.")
                    .Conditie(cmbAutobuz.SelectedItem is not null, cmbAutobuz, "Alegeți un autobuz în circulație.")
                    .Conditie(cmbSofer.SelectedItem is not null, cmbSofer, "Alegeți șoferul.")
                    .Conditie(dtpSosire.Value.TimeOfDay > dtpPlecare.Value.TimeOfDay, dtpSosire, "Ora de sosire trebuie să fie după ora de plecare.")
                    .Verifica(this))
                return;

            var date = new CursaEditare
            {
                TraseuID = ((ElementLista<int>)cmbTraseu.SelectedItem).Valoare,
                AutobuzID = ((ElementLista<int>)cmbAutobuz.SelectedItem).Valoare,
                SoferID = ((ElementLista<int>)cmbSofer.SelectedItem).Valoare,
                DataCursa = DateOnly.FromDateTime(dtpData.Value),
                OraPlecare = new TimeOnly(dtpPlecare.Value.Hour, dtpPlecare.Value.Minute),
                OraSosireEstimata = new TimeOnly(dtpSosire.Value.Hour, dtpSosire.Value.Minute),
                Pret = numPret.Value,
            };

            await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var id = _selectat?.CursaID ?? 0;
                if (_selectat is null)
                    id = await Aplicatie.Backend.Curse.CreeazaCursaAsync(date);
                else
                    await Aplicatie.Backend.Curse.ActualizeazaCursaAsync(id, date);

                // cursa noua poate cadea in afara intervalului afisat
                if (date.DataCursa < DateOnly.FromDateTime(dtpDeLa.Value) || date.DataCursa > DateOnly.FromDateTime(dtpPanaLa.Value))
                    dtpPanaLa.Value = date.DataCursa.ToDateTime(TimeOnly.MinValue);
                await ReincarcaAsync(id);
            });
        }

        private async void btnPlecata_Click(object sender, EventArgs e) => await SchimbaStatusAsync(btnPlecata, StatusCursa.InDesfasurare);

        private async void btnFinalizata_Click(object sender, EventArgs e) => await SchimbaStatusAsync(btnFinalizata, StatusCursa.Finalizata);

        private async Task SchimbaStatusAsync(Control sursa, StatusCursa status)
        {
            if (_selectat is null)
                return;
            var id = _selectat.CursaID;
            await Mesaje.RuleazaAsync(sursa, async () =>
            {
                await Aplicatie.Backend.Curse.SchimbaStatusAsync(id, status);
                await ReincarcaAsync(id);
            });
        }

        private async void btnAnuleaza_Click(object sender, EventArgs e)
        {
            if (_selectat is null)
                return;

            var c = _selectat;
            var motiv = FrmIntrebare.Cere(this, "Anularea cursei",
                $"Cursa {c.Traseu}, {Afisare.Text(c.DataCursa)} ora {Afisare.Text(c.OraPlecare)}.\n" +
                $"Toate biletele active ({c.LocuriOcupate}) se rambursează integral. Motivul anulării:");
            if (motiv is null)
                return;

            await Mesaje.RuleazaAsync(btnAnuleaza, async () =>
            {
                var r = await Aplicatie.Backend.Curse.AnuleazaCursaAsync(c.CursaID, motiv);
                Mesaje.Info(this, $"Cursa a fost anulată.\n{r.BileteRambursate} bilete rambursate, în total {Afisare.Lei(r.SumaTotalaRambursata)}.");
                await ReincarcaAsync(c.CursaID);
            });
        }

        private void btnBilete_Click(object sender, EventArgs e)
        {
            if (_selectat is null)
                return;
            var c = _selectat;
            using var f = new FrmBileteCursa(c.CursaID, $"{c.Traseu} · {Afisare.Text(c.DataCursa)}, ora {Afisare.Text(c.OraPlecare)}");
            f.ShowDialog(this);
        }
    }
}
