using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Cautarea curselor dupa statia de urcare, de coborare si data; de aici se deschide vanzarea.</summary>
    public partial class FrmCautareCurse : FormAutogara
    {
        public FrmCautareCurse()
        {
            InitializeComponent();
        }

        private async void FrmCautareCurse_Load(object sender, EventArgs e)
        {
            dtpData.MinDate = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            dtpData.Value = dtpData.MinDate;

            await Mesaje.RuleazaAsync(btnCauta, async () =>
            {
                var statii = await Aplicatie.Backend.Statii.ListeazaAsync();
                var elemente = statii.Select(s => new ElementLista<int?>(s.StatieID, s.Nume)).ToList();
                elemente.Insert(0, new ElementLista<int?>(null, "(oricare)"));
                cmbPlecare.DataSource = elemente.ToList();
                cmbSosire.DataSource = elemente.ToList();
                await CautaAsync();
            });
        }

        private async void btnCauta_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnCauta, CautaAsync);

        private async Task CautaAsync()
        {
            var plecare = (cmbPlecare.SelectedItem as ElementLista<int?>)?.Valoare;
            var sosire = (cmbSosire.SelectedItem as ElementLista<int?>)?.Valoare;
            var curse = await Aplicatie.Backend.Curse.CautaCurseAsync(plecare, sosire, DateOnly.FromDateTime(dtpData.Value));

            gridCurse.Afiseaza(curse, c => c.CursaID);
            lblNumar.Text = curse.Count switch
            {
                0 => "Nicio cursă pentru criteriile alese.",
                1 => "1 cursă",
                _ => $"{curse.Count} curse",
            };
        }

        private void btnAlegeLocul_Click(object sender, EventArgs e) => DeschideVanzarea();

        private void gridCurse_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                DeschideVanzarea();
        }

        private async void DeschideVanzarea()
        {
            var cursa = gridCurse.Selectat<CursaGasita>();
            if (cursa is null)
            {
                Mesaje.Atentie(this, "Alegeți o cursă din listă.");
                return;
            }
            if (!cursa.DisponibilaPentruVanzare)
            {
                Mesaje.Atentie(this, "Cursa nu mai este disponibilă pentru vânzare: a plecat, nu mai are locuri libere sau a fost anulată.");
                return;
            }

            using (var f = new FrmVanzareBilet(cursa))
                f.ShowDialog(this);

            await Mesaje.RuleazaAsync(btnCauta, CautaAsync);
        }
    }
}
