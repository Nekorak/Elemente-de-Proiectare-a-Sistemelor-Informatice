using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru adaugarea unui sofer nou sau modificarea unuia existent.</summary>
    public partial class FrmEditareSofer : FormAutogara
    {
        private readonly SoferRand _sofer;

        /// <param name="sofer">Soferul de modificat; null pentru unul nou.</param>
        public FrmEditareSofer(SoferRand sofer)
        {
            InitializeComponent();
            _sofer = sofer;

            Text = lblTitlu.Text = sofer is null ? "Șofer nou" : $"Modificare șofer — {sofer.NumeComplet}";
            txtNume.Text = sofer?.Nume ?? "";
            txtPrenume.Text = sofer?.Prenume ?? "";
            txtNrPermis.Text = sofer?.NrPermis ?? "";
            txtTelefon.Text = sofer?.Telefon ?? "";
        }

        /// <summary>ID-ul soferului salvat (nou sau modificat).</summary>
        public int IdSalvat { get; private set; }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Obligatoriu(txtNume, "Numele")
                    .Obligatoriu(txtPrenume, "Prenumele")
                    .Obligatoriu(txtNrPermis, "Numărul permisului")
                    .Telefon(txtTelefon)
                    .Verifica(this))
                return;

            var date = new SoferEditare { Nume = txtNume.Text, Prenume = txtPrenume.Text, NrPermis = txtNrPermis.Text, Telefon = txtTelefon.Text };
            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                if (_sofer is null)
                    IdSalvat = await Aplicatie.Backend.Soferi.AdaugaAsync(date);
                else
                {
                    await Aplicatie.Backend.Soferi.ActualizeazaAsync(_sofer.SoferID, date);
                    IdSalvat = _sofer.SoferID;
                }
            });

            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
