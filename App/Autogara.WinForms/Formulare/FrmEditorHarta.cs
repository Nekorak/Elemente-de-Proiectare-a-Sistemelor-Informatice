using Autogara.Domain.Enumerari;
using Autogara.WinForms.Controale;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Editorul hartii statiilor. Salvarea scrie nodurile si conexiunile in baza de date,
    /// apoi backend-ul rescrie harta.json pe file server.
    /// </summary>
    public partial class FrmEditorHarta : FormAutogara, ISectiune
    {
        public FrmEditorHarta()
        {
            InitializeComponent();
        }

        private async void FrmEditorHarta_Load(object sender, EventArgs e)
        {
            cmbTip.DataSource = Enum.GetValues<TipNod>().Select(t => new ElementLista<TipNod>(t, Afisare.Text(t))).ToList();
            SeteazaMod(ModHarta.Selectare);
            await Mesaje.RuleazaAsync(btnReincarca, IncarcaAsync);
        }

        public bool PoateInchide() =>
            !canvas.Modificata || Mesaje.Confirma(this, "Harta are modificări nesalvate. Le abandonați?");

        private async Task IncarcaAsync()
        {
            canvas.Harta = await Aplicatie.Backend.Harta.IncarcaHartaAsync();
            AfiseazaProprietatile();
            AfiseazaStarea();
        }

        // ---------- moduri ----------

        private void btnModSelectare_Click(object sender, EventArgs e) => SeteazaMod(ModHarta.Selectare);
        private void btnModStatie_Click(object sender, EventArgs e) => SeteazaMod(ModHarta.AdaugaStatie);
        private void btnModIntersectie_Click(object sender, EventArgs e) => SeteazaMod(ModHarta.AdaugaIntersectie);
        private void btnModConexiune_Click(object sender, EventArgs e) => SeteazaMod(ModHarta.Conexiune);
        private void btnModSterge_Click(object sender, EventArgs e) => SeteazaMod(ModHarta.Sterge);

        private void SeteazaMod(ModHarta mod)
        {
            canvas.Mod = mod;
            (ButonIcon Buton, ModHarta Mod)[] butoane =
            [
                (btnModSelectare, ModHarta.Selectare),
                (btnModStatie, ModHarta.AdaugaStatie),
                (btnModIntersectie, ModHarta.AdaugaIntersectie),
                (btnModConexiune, ModHarta.Conexiune),
                (btnModSterge, ModHarta.Sterge),
            ];
            foreach (var (buton, m) in butoane)
                buton.Stil = m == mod ? StilButon.Primar : StilButon.Secundar;

            lblSugestie.Text = mod switch
            {
                ModHarta.AdaugaStatie => "Click pe hartă adaugă o stație.",
                ModHarta.AdaugaIntersectie => "Click pe hartă adaugă o intersecție.",
                ModHarta.Conexiune => "Trageți de la un nod la altul pentru a crea un drum.",
                ModHarta.Sterge => "Click pe un nod sau pe un drum îl șterge.",
                _ => "Click pentru selectare, trageți nodurile pentru a le muta.",
            };
            canvas.Focus();
        }

        // ---------- selectie si proprietati ----------

        private void canvas_SelectieSchimbata(object sender, EventArgs e) => AfiseazaProprietatile();

        private void canvas_HartaModificata(object sender, EventArgs e) => AfiseazaStarea();

        private void canvas_NodAdaugat(object sender, Business.Dto.NodDto nod)
        {
            txtNume.Focus();
            txtNume.SelectAll();
        }

        private void AfiseazaProprietatile()
        {
            var nod = canvas.NodSelectat;
            var conexiune = nod is null ? canvas.ConexiuneSelectata : null;

            pnlNod.Visible = nod is not null;
            pnlConexiune.Visible = conexiune is not null;
            lblFaraSelectie.Visible = nod is null && conexiune is null;
            btnStergeSelectia.Enabled = nod is not null || conexiune is not null;

            if (nod is not null)
            {
                txtNume.Text = nod.Nume;
                cmbTip.SelectedIndex = Array.IndexOf(Enum.GetValues<TipNod>(), nod.Tip);
                lblPozitie.Text = nod.NodID > 0 ? $"Nodul #{nod.NodID} · poziția {nod.X:0}, {nod.Y:0}" : $"Nod nou · poziția {nod.X:0}, {nod.Y:0}";
            }

            if (conexiune is not null)
            {
                var nume = canvas.Harta.Noduri.ToDictionary(n => n.NodID, n => n.Nume);
                lblCapete.Text = $"{nume.GetValueOrDefault(conexiune.NodPlecareID)}\n↔ {nume.GetValueOrDefault(conexiune.NodSosireID)}";
                numDistanta.Value = Math.Clamp(conexiune.DistantaKm, numDistanta.Minimum, numDistanta.Maximum);
            }
        }

        private void AfiseazaStarea()
        {
            lblStare.Text = canvas.Modificata ? "Modificări nesalvate" : "Harta este salvată";
            lblStare.ForeColor = canvas.Modificata ? Tema.Avertisment : Tema.Succes;
            lblNumarare.Text = $"{canvas.Harta.Noduri.Count(n => n.Tip == TipNod.Statie)} stații, " +
                               $"{canvas.Harta.Noduri.Count(n => n.Tip == TipNod.Intersectie)} intersecții, " +
                               $"{canvas.Harta.Conexiuni.Select(c => (Math.Min(c.NodPlecareID, c.NodSosireID), Math.Max(c.NodPlecareID, c.NodSosireID))).Distinct().Count()} drumuri";
        }

        private void btnAplicaNod_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider).Obligatoriu(txtNume, "Numele").Verifica(this))
                return;
            canvas.SeteazaNod(txtNume.Text.Trim(), ((ElementLista<TipNod>)cmbTip.SelectedItem).Valoare);
            AfiseazaProprietatile();
        }

        private void txtNume_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAplicaNod.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void btnAplicaConexiune_Click(object sender, EventArgs e) => canvas.SeteazaDistanta(numDistanta.Value);

        private void btnStergeSelectia_Click(object sender, EventArgs e) => canvas.StergeSelectia();

        // ---------- salvare ----------

        private void btnPotriveste_Click(object sender, EventArgs e) => canvas.Potriveste();

        private async void btnReincarca_Click(object sender, EventArgs e)
        {
            if (canvas.Modificata && !Mesaje.Confirma(this, "Renunțați la modificările nesalvate și reîncărcați harta?"))
                return;
            await Mesaje.RuleazaAsync(btnReincarca, IncarcaAsync);
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var r = await Aplicatie.Backend.Harta.SalveazaHartaAsync(canvas.Harta);
                var mesaj = "Harta a fost salvată.";
                if (r.NoduriNoi.Count > 0) mesaj += $"\nNoduri noi: {r.NoduriNoi.Count}.";
                if (r.NoduriDezactivate > 0) mesaj += $"\nNoduri scoase de pe hartă: {r.NoduriDezactivate}.";
                if (!r.FisierActualizat) mesaj += $"\n\n{r.AvertismentFisier}";

                if (r.FisierActualizat)
                    Mesaje.Info(this, mesaj);
                else
                    Mesaje.Atentie(this, mesaj);

                await IncarcaAsync();
            });
        }
    }
}
