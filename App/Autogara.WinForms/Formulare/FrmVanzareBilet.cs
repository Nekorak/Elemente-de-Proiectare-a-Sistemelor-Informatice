using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Vanzarea unui bilet pe o cursa: click pe un loc liber il rezerva provizoriu (cu cronometru),
    /// apoi se completeaza datele pasagerului si se confirma. F2 = confirmare, Esc = inchidere.
    /// </summary>
    public partial class FrmVanzareBilet : FormAutogara
    {
        private readonly CursaGasita _cursa;
        private RezervareDto _rezervare;
        private LocHarta _loc;
        private bool _inchidereGata;

        public FrmVanzareBilet(CursaGasita cursa)
        {
            InitializeComponent();
            _cursa = cursa;
        }

        private async void FrmVanzareBilet_Load(object sender, EventArgs e)
        {
            lblTraseu.Text = _cursa.Traseu;
            lblDetalii.Text = $"{Afisare.Text(_cursa.DataCursa)}  ·  plecare {Afisare.Text(_cursa.OraPlecare)} din {_cursa.StatiePlecare}" +
                              (string.IsNullOrEmpty(_cursa.PeronPlecare) ? "" : $" (peron {_cursa.PeronPlecare})") +
                              $"  →  {_cursa.StatieSosire}, sosire {Afisare.Text(_cursa.OraSosireEstimata)}  ·  autobuz {_cursa.NrInmatriculare}";

            if (Aplicatie.EstePasager)
            {
                rbCard.Checked = true;
                rbNumerar.Enabled = false;
                txtBon.Enabled = false;
                txtNume.Text = Aplicatie.Utilizator.NumeComplet;
            }

            AfiseazaLocul();
            await Mesaje.RuleazaAsync(harta, async () =>
            {
                var reduceri = await Aplicatie.Backend.Reduceri.ListeazaAsync();
                var elemente = reduceri.Select(r => new ElementLista<int?>(r.TipReducereID, $"{r.Denumire} (−{r.ProcentReducere:0.##}%)")).ToList();
                elemente.Insert(0, new ElementLista<int?>(null, "Fără reducere"));
                cmbReducere.DataSource = elemente;
                await IncarcaLocurileAsync();
                await ActualizeazaPretulAsync();
            });
        }

        private async void FrmVanzareBilet_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerRezervare.Stop();
            if (_rezervare is null || _inchidereGata)
                return;

            // Locul rezervat si nevandut se elibereaza imediat, nu dupa expirare.
            e.Cancel = true;
            var r = _rezervare;
            _rezervare = null;
            try { await Aplicatie.Backend.Rezervari.ElibereazaRezervareAsync(r.RezervareID); }
            catch (Exception ex) { Serilog.Log.Warning(ex, "Rezervarea nu a putut fi eliberată la închidere"); }
            _inchidereGata = true;
            Close();
        }

        private void FrmVanzareBilet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnConfirma.PerformClick();
                e.Handled = true;
            }
        }

        private async Task IncarcaLocurileAsync()
        {
            harta.Harta = await Aplicatie.Backend.Curse.HartaLocuriAsync(_cursa.CursaID);
            harta.LocSelectatID = _rezervare?.LocID;
            var libere = harta.Harta.Locuri.Count(l => l.Status == StatusLoc.Liber);
            lblLocuriLibere.Text = $"{libere} locuri libere din {harta.Harta.Locuri.Count}";
        }

        private async void harta_LocSelectat(object sender, LocHarta loc)
        {
            if (_rezervare?.LocID == loc.LocID)
                return;

            await Mesaje.RuleazaAsync(harta, async () =>
            {
                if (_rezervare is not null)
                {
                    await Aplicatie.Backend.Rezervari.ElibereazaRezervareAsync(_rezervare.RezervareID);
                    _rezervare = null;
                    _loc = null;
                }

                _rezervare = await Aplicatie.Backend.Rezervari.RezervaLocAsync(loc.LocID);
                _loc = loc;
                timerRezervare.Start();
            });

            AfiseazaLocul();
            await Mesaje.RuleazaAsync(harta, IncarcaLocurileAsync);
            txtNume.Focus();
        }

        private void AfiseazaLocul()
        {
            lblLoc.Text = _loc is null ? "—" : $"Locul {_loc.NumarLoc}";
            lblCronometru.Text = _rezervare is null ? "Alegeți un loc liber pe schema autobuzului." : $"Rezervat încă {Afisare.Durata(_rezervare.SecundeRamase)}";
            lblCronometru.ForeColor = _rezervare is null ? Tema.TextSecundar : Tema.Avertisment;
            iconCronometru.Culoare = lblCronometru.ForeColor;
        }

        private async void timerRezervare_Tick(object sender, EventArgs e)
        {
            if (_rezervare is null)
            {
                timerRezervare.Stop();
                return;
            }

            if (_rezervare.SecundeRamase > 0)
            {
                AfiseazaLocul();
                return;
            }

            timerRezervare.Stop();
            _rezervare = null;
            _loc = null;
            AfiseazaLocul();
            Mesaje.Atentie(this, "Rezervarea provizorie a expirat, iar locul a fost eliberat. Alegeți din nou locul.");
            await Mesaje.RuleazaAsync(harta, IncarcaLocurileAsync);
        }

        private async void cmbReducere_SelectedIndexChanged(object sender, EventArgs e) =>
            await Mesaje.RuleazaAsync(cmbReducere, ActualizeazaPretulAsync);

        private async Task ActualizeazaPretulAsync()
        {
            var reducere = (cmbReducere.SelectedItem as ElementLista<int?>)?.Valoare;
            var pret = await Aplicatie.Backend.Bilete.CalculeazaPretAsync(_cursa.CursaID, reducere);
            lblPret.Text = Afisare.Lei(pret.PretFinal);
            lblPretDetalii.Text = pret.ProcentReducere > 0
                ? $"{Afisare.Lei(pret.PretBaza)} − {pret.ProcentReducere:0.##}% ({pret.Reducere})"
                : "Preț întreg";
        }

        private async void btnConfirma_Click(object sender, EventArgs e)
        {
            if (_rezervare is null)
            {
                Mesaje.Atentie(this, "Alegeți mai întâi un loc liber pe schema autobuzului.");
                return;
            }
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtNume, "Numele pasagerului")
                    .Telefon(txtTelefon)
                    .Verifica(this))
                return;

            var date = new VanzareDto
            {
                NumePasager = txtNume.Text,
                TelefonPasager = txtTelefon.Text,
                TipReducereID = (cmbReducere.SelectedItem as ElementLista<int?>)?.Valoare,
                MetodaPlata = rbCard.Checked ? MetodaPlata.Card : MetodaPlata.Numerar,
                NumarBonFiscal = txtBon.Text,
            };

            BiletEmis bilet = null;
            var reusit = await Mesaje.RuleazaAsync(btnConfirma, async () =>
            {
                try
                {
                    bilet = await Aplicatie.Backend.Bilete.ConfirmaVanzareAsync(_rezervare.RezervareID, date);
                }
                catch (RegulaException ex) when (ex.CodSql is 50032 or 50037)
                {
                    // rezervarea a expirat intre timp: locul nu mai e al nostru
                    _rezervare = null;
                    _loc = null;
                    throw;
                }
            });

            if (reusit)
            {
                timerRezervare.Stop();
                _rezervare = null;
                _loc = null;
                Mesaje.Info(this, $"Biletul a fost emis.\n\nCod: {bilet.CodBilet}\nLocul {bilet.NumarLoc}, {Afisare.Lei(bilet.Pret)}");

                if (!Aplicatie.EstePasager)
                {
                    txtNume.Clear();
                    txtTelefon.Clear();
                    txtBon.Clear();
                    cmbReducere.SelectedIndex = 0;
                }
            }

            AfiseazaLocul();
            await Mesaje.RuleazaAsync(harta, IncarcaLocurileAsync);
        }
    }
}
