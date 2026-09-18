using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Lista pasagerilor unei curse (biletele emise, cu locul si statusul).</summary>
    public partial class FrmBileteCursa : FormAutogara
    {
        private readonly int _cursaId;

        public FrmBileteCursa(int cursaId, string descriere)
        {
            InitializeComponent();
            _cursaId = cursaId;
            lblTitlu.Text = descriere;
        }

        private async void FrmBileteCursa_Load(object sender, EventArgs e) =>
            await Mesaje.RuleazaAsync(this, async () =>
            {
                var bilete = await Aplicatie.Backend.Bilete.BileteCursaAsync(_cursaId);
                gridBilete.Afiseaza(bilete);
                var active = bilete.Count(b => b.Status == Domain.Enumerari.StatusBilet.Activ);
                lblSumar.Text = $"{active} bilete active, {bilete.Count - active} anulate sau rambursate";
            });
    }
}
