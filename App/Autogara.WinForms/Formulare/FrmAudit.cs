using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Jurnalul de audit: cine, ce si cand, cu filtre.</summary>
    public partial class FrmAudit : FormAutogara
    {
        public FrmAudit()
        {
            InitializeComponent();
        }

        private async void FrmAudit_Load(object sender, EventArgs e)
        {
            var azi = OraLocala.Azi.ToDateTime(TimeOnly.MinValue);
            dtpDeLa.Value = azi.AddDays(-7);
            dtpPanaLa.Value = azi;
            await Mesaje.RuleazaAsync(btnCauta, CautaAsync);
        }

        private async void btnCauta_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnCauta, CautaAsync);

        private async Task CautaAsync()
        {
            var randuri = await Aplicatie.Backend.Audit.ListeazaAsync(new FiltruAudit
            {
                DeLa = DateOnly.FromDateTime(dtpDeLa.Value),
                PanaLa = DateOnly.FromDateTime(dtpPanaLa.Value),
                Text = string.IsNullOrWhiteSpace(txtText.Text) ? null : txtText.Text.Trim(),
                MaxRanduri = (int)numMax.Value,
            });
            gridAudit.Afiseaza(randuri);
            lblNumar.Text = randuri.Count >= numMax.Value ? $"Primele {randuri.Count} înregistrări (cele mai noi)" : $"{randuri.Count} înregistrări";
        }
    }
}
