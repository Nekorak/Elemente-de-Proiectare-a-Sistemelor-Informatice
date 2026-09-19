using Autogara.Business.Dto;
using Autogara.WinForms.Controale;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Fereastra principala: meniul din stanga (adaptat pe rol) deschide sectiunile in zona de continut.
    /// </summary>
    public partial class FrmMain : FormAutogara
    {
        private Form _sectiune;

        public FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>True daca fereastra s-a inchis prin „Ieșire” (se revine la login).</summary>
        public bool Deconectare { get; private set; }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var u = Aplicatie.Utilizator;
            lblUtilizator.Text = $"{u.NumeComplet}\n{u.Rol}";
            lblVersiuneMeniu.Text = $"Versiunea {Aplicatie.Versiune.ToString(3)}";

            var personal = !Aplicatie.EstePasager;
            btnDashboard.Visible = personal;
            lblGrupAdministrare.Visible = btnCurse.Visible = btnTrasee.Visible = btnStatii.Visible = btnHarta.Visible =
                btnAutobuze.Visible = btnMentenanta.Visible = btnSoferi.Visible = btnReduceri.Visible = btnUtilizatori.Visible = Aplicatie.EsteAdmin;
            lblGrupRapoarte.Visible = btnRapoarte.Visible = btnAudit.Visible = Aplicatie.EsteAdmin;

            if (Aplicatie.EstePasager)
            {
                btnVanzare.Text = " Cumpără bilet";
                btnBilete.Text = " Biletele mele";
            }

            Aplicatie.Backend.Conexiune.ConexiuneSchimbata += Conexiune_Schimbata;
            AfiseazaConexiunea(Aplicatie.Backend.Conexiune.StareCurenta);

            if (personal)
                btnDashboard.PerformClick();
            else
                btnVanzare.PerformClick();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PoateInchideSectiunea())
            {
                e.Cancel = true;
                return;
            }
            Aplicatie.Backend.Conexiune.ConexiuneSchimbata -= Conexiune_Schimbata;
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnVanzare.PerformClick();
                e.Handled = true;
            }
        }

        private void Conexiune_Schimbata(object sender, StareConexiune stare)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(() => AfiseazaConexiunea(stare));
        }

        private void AfiseazaConexiunea(StareConexiune stare)
        {
            indicatorConexiune.Seteaza(stare);
            toolTip.SetToolTip(indicatorConexiune, indicatorConexiune.Detalii);
        }

        // ---------- navigare ----------

        private bool PoateInchideSectiunea() => _sectiune is not ISectiune s || s.PoateInchide();

        private void Deschide(ButonIcon buton, string titlu, Func<Form> creeaza)
        {
            if (buton.Activ && _sectiune is not null)
                return;
            if (!PoateInchideSectiunea())
                return;

            var noua = creeaza();
            noua.TopLevel = false;
            noua.FormBorderStyle = FormBorderStyle.None;
            noua.Dock = DockStyle.Fill;

            pnlContinut.SuspendLayout();
            _sectiune?.Close();
            _sectiune?.Dispose();
            _sectiune = noua;
            pnlContinut.Controls.Add(noua);
            pnlContinut.ResumeLayout();
            noua.Show();

            foreach (var b in flpMeniu.Controls.OfType<ButonIcon>())
                b.Activ = b == buton;
            lblSectiune.Text = titlu.ToUpperInvariant();
        }

        private void btnDashboard_Click(object sender, EventArgs e) => Deschide(btnDashboard, "Dashboard", () => new FrmDashboard());
        private void btnVanzare_Click(object sender, EventArgs e) => Deschide(btnVanzare, Aplicatie.EstePasager ? "Cumpără bilet" : "Vânzare bilete", () => new FrmCautareCurse());
        private void btnBilete_Click(object sender, EventArgs e) => Deschide(btnBilete, Aplicatie.EstePasager ? "Biletele mele" : "Bilete", () => new FrmBilete());
        private void btnCurse_Click(object sender, EventArgs e) => Deschide(btnCurse, "Curse", () => new FrmAdminCurse());
        private void btnTrasee_Click(object sender, EventArgs e) => Deschide(btnTrasee, "Trasee", () => new FrmAdminTrasee());
        private void btnStatii_Click(object sender, EventArgs e) => Deschide(btnStatii, "Stații", () => new FrmAdminStatii());
        private void btnHarta_Click(object sender, EventArgs e) => Deschide(btnHarta, "Harta stațiilor", () => new FrmEditorHarta());
        private void btnAutobuze_Click(object sender, EventArgs e) => Deschide(btnAutobuze, "Autobuze", () => new FrmAdminAutobuze());
        private void btnMentenanta_Click(object sender, EventArgs e) => Deschide(btnMentenanta, "Mentenanță și ITP", () => new FrmAdminMentenanta());
        private void btnSoferi_Click(object sender, EventArgs e) => Deschide(btnSoferi, "Șoferi", () => new FrmAdminSoferi());
        private void btnReduceri_Click(object sender, EventArgs e) => Deschide(btnReduceri, "Reduceri", () => new FrmAdminReduceri());
        private void btnUtilizatori_Click(object sender, EventArgs e) => Deschide(btnUtilizatori, "Utilizatori", () => new FrmAdminUtilizatori());
        private void btnRapoarte_Click(object sender, EventArgs e) => Deschide(btnRapoarte, "Rapoarte", () => new FrmRapoarte());
        private void btnAudit_Click(object sender, EventArgs e) => Deschide(btnAudit, "Jurnal de audit", () => new FrmAudit());

        // ---------- cont ----------

        private void btnSchimbaParola_Click(object sender, EventArgs e)
        {
            using var f = new FrmSchimbaParola();
            f.ShowDialog(this);
        }

        private async void btnDeconectare_Click(object sender, EventArgs e)
        {
            if (!PoateInchideSectiunea())
                return;

            await Mesaje.RuleazaAsync(btnDeconectare, () => Aplicatie.Backend.Auth.DeconecteazaAsync());
            _sectiune?.Close();
            _sectiune = null;
            Deconectare = true;
            Close();
        }
    }

    /// <summary>Sectiunile cu modificari nesalvate (ex. editorul de harta) pot opri schimbarea sectiunii.</summary>
    public interface ISectiune
    {
        bool PoateInchide();
    }
}
