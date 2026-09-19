using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru adresa si peronul unei statii (numele vine de pe harta).</summary>
    public partial class FrmEditareStatie : FormAutogara
    {
        private readonly StatieRand _statie;

        public FrmEditareStatie(StatieRand statie)
        {
            InitializeComponent();
            _statie = statie;

            Text = lblTitlu.Text = $"Modificare stație — {statie.Nume}";
            txtNume.Text = statie.Nume;
            txtAdresa.Text = statie.Adresa ?? "";
            txtPeron.Text = statie.Peron ?? "";
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, () =>
                Aplicatie.Backend.Statii.ActualizeazaAsync(_statie.StatieID, txtAdresa.Text, txtPeron.Text));

            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
