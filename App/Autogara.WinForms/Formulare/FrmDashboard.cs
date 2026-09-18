using Autogara.Common;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Cifrele zilei, reimprospatate automat la 30 de secunde.</summary>
    public partial class FrmDashboard : FormAutogara
    {
        private bool _seIncarca;

        public FrmDashboard()
        {
            InitializeComponent();
        }

        private async void FrmDashboard_Load(object sender, EventArgs e)
        {
            await Mesaje.RuleazaAsync(btnReimprospateaza, IncarcaAsync);
            timerActualizare.Start();
        }

        private void FrmDashboard_FormClosed(object sender, FormClosedEventArgs e) => timerActualizare.Stop();

        private async void btnReimprospateaza_Click(object sender, EventArgs e) =>
            await Mesaje.RuleazaAsync(btnReimprospateaza, IncarcaAsync);

        private async void timerActualizare_Tick(object sender, EventArgs e)
        {
            if (_seIncarca)
                return;
            try
            {
                await IncarcaAsync();
            }
            catch (Exception ex)
            {
                // Actualizarea automata nu deschide ferestre de eroare; doar anunta in antet.
                lblActualizat.Text = ExceptionHandler.MesajPrietenos(ex);
            }
        }

        private async Task IncarcaAsync()
        {
            _seIncarca = true;
            try
            {
                var d = await Aplicatie.Backend.Dashboard.DateLiveAsync(15);
                cardCurseAzi.Valoare = d.CurseAzi.ToString();
                cardInDesfasurare.Valoare = d.CurseInDesfasurare.ToString();
                cardRamase.Valoare = d.CurseRamaseAzi.ToString();
                cardBilete.Valoare = d.BileteVanduteAzi.ToString();
                cardAnulate.Valoare = d.BileteAnulateAzi.ToString();
                cardIncasari.Valoare = Afisare.Lei(d.IncasariNeteAzi);
                cardRezervari.Valoare = d.RezervariActive.ToString();
                cardItp.Valoare = d.AutobuzeItpInUrmatoarele30Zile.ToString();
                gridCurse.Afiseaza(d.UrmatoareleCurse, c => c.CursaID);
                lblActualizat.Text = $"Actualizat la {d.GeneratLa:HH:mm:ss}";
            }
            finally
            {
                _seIncarca = false;
            }
        }
    }
}
