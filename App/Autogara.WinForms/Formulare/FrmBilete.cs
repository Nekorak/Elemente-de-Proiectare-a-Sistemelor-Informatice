using Autogara.Business.Dto;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Cautarea unui bilet dupa cod, detaliile lui si anularea cu rambursare.</summary>
    public partial class FrmBilete : FormAutogara
    {
        private BiletDetalii _bilet;
        private bool _seIncarca;

        public FrmBilete()
        {
            InitializeComponent();
        }

        private async void FrmBilete_Load(object sender, EventArgs e)
        {
            lblLista.Text = Aplicatie.EstePasager ? "Biletele mele" : "Bilete vândute de mine (ultimele 90 de zile)";
            chkIntegral.Visible = Aplicatie.EsteAdmin;
            AfiseazaBilet(null, null);
            await Mesaje.RuleazaAsync(btnReincarca, async () =>
            {
                await ReincarcaAsync();
                if (gridBilete.Selectat<BiletDetalii>() is { } primul)
                    await AfiseazaAsync(primul);
            });
        }

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, ReincarcaAsync);

        private async Task ReincarcaAsync()
        {
            _seIncarca = true;
            try
            {
                gridBilete.Afiseaza(await Aplicatie.Backend.Bilete.BileteleMeleAsync(), b => b.BiletID);
            }
            finally
            {
                _seIncarca = false;
            }
        }

        private async void btnCautaCod_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider).Obligatoriu(txtCod, "Codul biletului").Verifica(this))
                return;

            await Mesaje.RuleazaAsync(btnCautaCod, async () =>
            {
                var bilet = await Aplicatie.Backend.Bilete.CautaDupaCodAsync(txtCod.Text);
                if (bilet is null)
                {
                    Mesaje.Atentie(this, "Nu există niciun bilet cu acest cod.");
                    return;
                }
                await AfiseazaAsync(bilet);
            });
        }

        private async void gridBilete_SelectionChanged(object sender, EventArgs e)
        {
            if (_seIncarca || gridBilete.Selectat<BiletDetalii>() is not { } bilet || bilet.BiletID == _bilet?.BiletID)
                return;
            await Mesaje.RuleazaAsync(gridBilete, () => AfiseazaAsync(bilet));
        }

        private async Task AfiseazaAsync(BiletDetalii bilet)
        {
            var estimare = await Aplicatie.Backend.Bilete.EstimeazaAnulareAsync(bilet.BiletID);
            AfiseazaBilet(bilet, estimare);
        }

        private void AfiseazaBilet(BiletDetalii b, EstimareAnulare estimare)
        {
            _bilet = b;
            pnlDetaliiContinut.Visible = b is not null;
            lblFaraBilet.Visible = b is null;
            if (b is null)
                return;

            lblCodBilet.Text = b.CodBilet;
            lblStatus.Text = Afisare.Text(b.Status);
            lblStatus.ForeColor = b.Status == StatusBilet.Activ ? Tema.Succes : Tema.TextSecundar;

            lblDetalii.Text = string.Join(Environment.NewLine,
                $"Pasager:  {b.NumePasager}" + (string.IsNullOrEmpty(b.TelefonPasager) ? "" : $", {b.TelefonPasager}"),
                $"Cursa:  {b.Traseu}",
                $"Plecare:  {Afisare.Text(b.DataCursa)}, ora {Afisare.Text(b.OraPlecare)} ({Afisare.Text(b.StatusCursa)})",
                $"Locul:  {b.NumarLoc}, autobuz {b.NrInmatriculare}",
                $"Preț:  {Afisare.Lei(b.Pret)}" + (b.Reducere is null ? "" : $" (reducere {b.Reducere})"),
                $"Plată:  {(b.MetodaPlata is { } m ? Afisare.Text(m) : "—")}",
                $"Emis:  {Afisare.Text(b.DataEmitereLocal)}, de {b.VandutDe}");

            lblEstimare.Text = estimare is null ? "" :
                estimare.PoateFiAnulat
                    ? $"Dacă se anulează acum: se rambursează {Afisare.Lei(estimare.SumaRambursata)} ({estimare.ProcentRambursare:0}%).\n{estimare.Explicatie}"
                    : estimare.Explicatie;
            btnAnuleaza.Enabled = b.Status == StatusBilet.Activ && (estimare?.PoateFiAnulat ?? false) || (chkIntegral.Checked && b.Status == StatusBilet.Activ);
        }

        private void chkIntegral_CheckedChanged(object sender, EventArgs e)
        {
            if (_bilet is not null)
                btnAnuleaza.Enabled = _bilet.Status == StatusBilet.Activ;
        }

        private async void btnAnuleaza_Click(object sender, EventArgs e)
        {
            if (_bilet is null)
                return;

            var integral = chkIntegral.Visible && chkIntegral.Checked;
            var intrebare = $"Anulați biletul {_bilet.CodBilet} ({_bilet.NumePasager})?" +
                            (integral ? "\n\nSe rambursează integral, în afara politicii de anulare." : "");
            if (!Mesaje.Confirma(this, intrebare))
                return;

            var cod = _bilet.CodBilet;
            await Mesaje.RuleazaAsync(btnAnuleaza, async () =>
            {
                var r = await Aplicatie.Backend.Bilete.AnuleazaBiletAsync(_bilet.BiletID, integral);
                Mesaje.Info(this, r.SumaRambursata > 0
                    ? $"Biletul {r.CodBilet} a fost anulat.\nSuma de rambursat pasagerului: {Afisare.Lei(r.SumaRambursata)}."
                    : $"Biletul {r.CodBilet} a fost anulat, fără rambursare.");

                chkIntegral.Checked = false;
                await ReincarcaAsync();
                var actualizat = await Aplicatie.Backend.Bilete.CautaDupaCodAsync(cod);
                if (actualizat is not null)
                    await AfiseazaAsync(actualizat);
            });
        }
    }
}
