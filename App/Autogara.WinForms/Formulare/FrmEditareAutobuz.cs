using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Fereastra modala pentru un autobuz nou sau modificarea datelor unuia existent.
    /// Locurile se aranjeaza separat, in <see cref="FrmEditorAutobuz"/>.
    /// </summary>
    public partial class FrmEditareAutobuz : FormAutogara
    {
        private readonly AutobuzRand _autobuz;

        /// <param name="autobuz">Autobuzul de modificat; null pentru unul nou.</param>
        public FrmEditareAutobuz(AutobuzRand autobuz)
        {
            InitializeComponent();
            _autobuz = autobuz;
            var nou = autobuz is null;

            cmbStatus.DataSource = Enum.GetValues<StatusAutobuz>().Select(s => new ElementLista<StatusAutobuz>(s, Afisare.Text(s))).ToList();

            Text = lblTitlu.Text = nou ? "Autobuz nou" : $"Modificare autobuz — {autobuz.NrInmatriculare}";
            txtNrInmatriculare.Text = autobuz?.NrInmatriculare ?? "";
            txtModel.Text = autobuz?.Model ?? "";
            numCapacitate.Value = autobuz?.CapacitateLocuri ?? 20;
            numCapacitate.Enabled = nou;
            cmbStatus.SelectedIndex = Array.IndexOf(Enum.GetValues<StatusAutobuz>(), autobuz?.Status ?? StatusAutobuz.Activ);
            chkItp.Checked = nou || autobuz.DataExpirareITP is not null;
            dtpItp.Value = (autobuz?.DataExpirareITP ?? OraLocala.Azi.AddYears(1)).ToDateTime(TimeOnly.MinValue);
            dtpItp.Enabled = chkItp.Checked;
            lblFisier.Text = nou
                ? "Structura locurilor se creează la salvare (4 pe rând, culoar la mijloc) și se poate modifica apoi din „Locuri”."
                : $"Structura locurilor: {autobuz.CaleFisierJSON ?? "nesalvată"}. Numărul de locuri se schimbă din „Locuri”.";
        }

        public int IdSalvat { get; private set; }

        /// <summary>Statusul ales la salvare.</summary>
        public StatusAutobuz Status => ((ElementLista<StatusAutobuz>)cmbStatus.SelectedItem).Valoare;

        private void chkItp_CheckedChanged(object sender, EventArgs e) => dtpItp.Enabled = chkItp.Checked;

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtNrInmatriculare, "Numărul de înmatriculare")
                    .Obligatoriu(txtModel, "Modelul")
                    .Verifica(this))
                return;

            var date = new AutobuzEditare
            {
                NrInmatriculare = txtNrInmatriculare.Text,
                Model = txtModel.Text,
                Status = Status,
                DataExpirareITP = chkItp.Checked ? DateOnly.FromDateTime(dtpItp.Value) : null,
            };

            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                if (_autobuz is null)
                    IdSalvat = await Aplicatie.Backend.Autobuze.CreeazaAsync(date, (int)numCapacitate.Value);
                else
                {
                    await Aplicatie.Backend.Autobuze.ActualizeazaAsync(_autobuz.AutobuzID, date);
                    IdSalvat = _autobuz.AutobuzID;
                }
            });

            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
