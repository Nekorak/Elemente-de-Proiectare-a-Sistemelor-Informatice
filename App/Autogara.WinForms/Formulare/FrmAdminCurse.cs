using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Planificarea curselor: lista pe interval, plecare/sosire, anulare cu rambursare.
    /// Cursele se creeaza si se modifica in <see cref="FrmEditareCursa"/>.
    /// </summary>
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
                var filtru = trasee.Select(t => new ElementLista<int?>(t.TraseuID, t.Denumire)).ToList();
                filtru.Insert(0, new ElementLista<int?>(null, "(toate traseele)"));
                cmbFiltruTraseu.DataSource = filtru;
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

        private void gridLista_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnModifica.Enabled)
                btnModifica.PerformClick();
        }

        private void AfiseazaSelectia()
        {
            _selectat = gridLista.Selectat<CursaRand>();
            var c = _selectat;

            // Doar cursele planificate se mai pot modifica.
            btnModifica.Enabled = c?.Status == StatusCursa.Planificata;
            btnPlecata.Enabled = c?.Status == StatusCursa.Planificata;
            btnFinalizata.Enabled = c?.Status == StatusCursa.InDesfasurare;
            btnAnuleaza.Enabled = c?.Status == StatusCursa.Planificata;
            btnBilete.Enabled = c is not null;
        }

        private async void btnAdauga_Click(object sender, EventArgs e) => await DeschideEditareaAsync(null);

        private async void btnModifica_Click(object sender, EventArgs e)
        {
            if (_selectat?.Status == StatusCursa.Planificata)
                await DeschideEditareaAsync(_selectat);
        }

        private async Task DeschideEditareaAsync(CursaRand cursa)
        {
            int id;
            DateOnly data;
            using (var f = new FrmEditareCursa(cursa))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
                (id, data) = (f.IdSalvat, f.DataSalvata);
            }

            // cursa poate cadea in afara intervalului afisat
            if (data < DateOnly.FromDateTime(dtpDeLa.Value))
                dtpDeLa.Value = data.ToDateTime(TimeOnly.MinValue);
            if (data > DateOnly.FromDateTime(dtpPanaLa.Value))
                dtpPanaLa.Value = data.ToDateTime(TimeOnly.MinValue);
            await Mesaje.RuleazaAsync(btnAfiseaza, () => ReincarcaAsync(id));
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
