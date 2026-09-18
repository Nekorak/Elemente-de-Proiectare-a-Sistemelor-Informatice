using System.Diagnostics;
using Autogara.Business.Servicii;
using Autogara.Common;
using Autogara.WinForms.Controale;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Rapoartele: vanzari (pe zi si pe interval), ocuparea curselor, comparatia lunara; export CSV.</summary>
    public partial class FrmRapoarte : FormAutogara
    {
        private enum TipRaport { VanzariZi, VanzariInterval, Ocupare, Comparativ }

        private Func<byte[]> _export;
        private string _numeExport;

        public FrmRapoarte()
        {
            InitializeComponent();
        }

        private async void FrmRapoarte_Load(object sender, EventArgs e)
        {
            var azi = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            dtpDeLa.Value = azi.AddDays(-7);
            dtpPanaLa.Value = azi;

            cmbRaport.DataSource = new List<ElementLista<TipRaport>>
            {
                new(TipRaport.VanzariZi, "Vânzări pe o zi (pe traseu și casier)"),
                new(TipRaport.VanzariInterval, "Vânzări pe interval (pe zile)"),
                new(TipRaport.Ocupare, "Ocuparea curselor"),
                new(TipRaport.Comparativ, "Comparație lunară pe trasee"),
            };

            await Mesaje.RuleazaAsync(btnAfiseaza, async () =>
            {
                var trasee = await Aplicatie.Backend.Trasee.ListeazaAsync(includeInactive: true);
                var elemente = trasee.Select(t => new ElementLista<int?>(t.TraseuID, t.Denumire)).ToList();
                elemente.Insert(0, new ElementLista<int?>(null, "(toate traseele)"));
                cmbTraseu.DataSource = elemente;
            });
        }

        private TipRaport Tip => ((ElementLista<TipRaport>)cmbRaport.SelectedItem).Valoare;

        private void cmbRaport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRaport.SelectedItem is null)
                return;

            var tip = Tip;
            var azi = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            if (tip == TipRaport.VanzariZi)
                dtpDeLa.Value = azi;
            else if (dtpDeLa.Value == azi && tip != TipRaport.Comparativ)
                dtpDeLa.Value = azi.AddDays(-7);

            lblDeLa.Text =tip switch { TipRaport.VanzariZi => "Ziua", TipRaport.Comparativ => "Anul (din dată)", _ => "De la" };
            lblPanaLa.Visible = dtpPanaLa.Visible = tip is TipRaport.VanzariInterval or TipRaport.Ocupare;
            lblTraseu.Visible = cmbTraseu.Visible = tip is TipRaport.Ocupare or TipRaport.Comparativ;

            gridVanzariZi.Visible = tip == TipRaport.VanzariZi;
            gridVanzariInterval.Visible = tip == TipRaport.VanzariInterval;
            gridOcupare.Visible = tip == TipRaport.Ocupare;
            gridComparativ.Visible = tip == TipRaport.Comparativ;

            _export = null;
            lblTotal.Text = "Alegeți perioada și apăsați „Afișează”.";
        }

        private async void btnAfiseaza_Click(object sender, EventArgs e) =>
            await Mesaje.RuleazaAsync(btnAfiseaza, async () =>
            {
                var rapoarte = Aplicatie.Backend.Rapoarte;
                var deLa = DateOnly.FromDateTime(dtpDeLa.Value);
                var panaLa = DateOnly.FromDateTime(dtpPanaLa.Value);
                var traseu = (cmbTraseu.SelectedItem as ElementLista<int?>)?.Valoare;

                switch (Tip)
                {
                    case TipRaport.VanzariZi:
                    {
                        var r = await rapoarte.VanzariZilniceAsync(deLa);
                        Afiseaza(gridVanzariZi, r, $"vanzari-{deLa:yyyy-MM-dd}.csv",
                            $"{r.Sum(x => x.BileteVandute)} bilete vândute, {r.Sum(x => x.Rambursari)} rambursări · încasări nete {Afisare.Lei(r.Sum(x => x.IncasariNete))}");
                        break;
                    }
                    case TipRaport.VanzariInterval:
                    {
                        var r = await rapoarte.VanzariPeIntervalAsync(deLa, panaLa);
                        Afiseaza(gridVanzariInterval, r, $"vanzari-{deLa:yyyy-MM-dd}_{panaLa:yyyy-MM-dd}.csv",
                            $"{r.Sum(x => x.BileteVandute)} bilete vândute · încasări nete {Afisare.Lei(r.Sum(x => x.IncasariNete))}");
                        break;
                    }
                    case TipRaport.Ocupare:
                    {
                        var r = await rapoarte.OcupareCurseAsync(deLa, panaLa, traseu);
                        var medie = r.Count == 0 ? 0 : r.Average(x => x.GradOcupareProcent ?? 0);
                        Afiseaza(gridOcupare, r, $"ocupare-{deLa:yyyy-MM-dd}_{panaLa:yyyy-MM-dd}.csv",
                            $"{r.Count} curse · grad mediu de ocupare {medie:0.#}% · încasări {Afisare.Lei(r.Sum(x => x.Incasari))}");
                        break;
                    }
                    case TipRaport.Comparativ:
                    {
                        var r = await rapoarte.RapoarteComparativeAsync(deLa.Year, traseu);
                        Afiseaza(gridComparativ, r, $"comparativ-{deLa.Year}.csv", $"{r.Count} rânduri (traseu × lună) pentru {deLa.Year}");
                        break;
                    }
                }
            });

        private void Afiseaza<T>(GridAutogara grid, List<T> randuri, string numeFisier, string total)
        {
            grid.Afiseaza(randuri);
            _export = () => RaportService.ExportCsv(randuri);
            _numeExport = numeFisier;
            lblTotal.Text = randuri.Count == 0 ? "Nu există date pentru perioada aleasă." : total;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_export is null)
            {
                Mesaje.Atentie(this, "Afișați mai întâi un raport.");
                return;
            }

            dlgSalvare.FileName = _numeExport;
            if (dlgSalvare.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                File.WriteAllBytes(dlgSalvare.FileName, _export());
                if (Mesaje.Confirma(this, "Raportul a fost salvat. Îl deschideți acum (Excel)?"))
                    Process.Start(new ProcessStartInfo(dlgSalvare.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Mesaje.Eroare(this, ex);
            }
        }

        private async void btnArhiveaza_Click(object sender, EventArgs e)
        {
            if (_export is null)
            {
                Mesaje.Atentie(this, "Afișați mai întâi un raport.");
                return;
            }

            await Mesaje.RuleazaAsync(btnArhiveaza, async () =>
            {
                var cale = await Aplicatie.Backend.Rapoarte.ArhiveazaPeFileServerAsync(_numeExport, _export());
                Mesaje.Info(this, $"Raportul a fost arhivat pe file server:\n{cale}");
            });
        }
    }
}
