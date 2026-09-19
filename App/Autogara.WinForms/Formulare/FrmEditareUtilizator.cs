using Autogara.Business.Dto;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru un cont nou sau modificarea datelor unui cont existent.</summary>
    public partial class FrmEditareUtilizator : FormAutogara
    {
        private readonly UtilizatorRand _utilizator;

        /// <param name="utilizator">Contul de modificat; null pentru unul nou.</param>
        public FrmEditareUtilizator(UtilizatorRand utilizator)
        {
            InitializeComponent();
            _utilizator = utilizator;
            var nou = utilizator is null;

            cmbRol.DataSource = Enum.GetValues<RolTip>().Select(r => new ElementLista<RolTip>(r, r.ToString())).ToList();

            Text = lblTitlu.Text = nou ? "Utilizator nou" : $"Modificare cont — {utilizator.NumeUtilizator}";
            txtNumeUtilizator.Text = utilizator?.NumeUtilizator ?? "";
            txtNume.Text = utilizator?.Nume ?? "";
            txtPrenume.Text = utilizator?.Prenume ?? "";
            txtEmail.Text = utilizator?.Email ?? "";
            txtTelefon.Text = utilizator?.Telefon ?? "";
            cmbRol.SelectedIndex = Array.IndexOf(Enum.GetValues<RolTip>(), utilizator?.Rol ?? RolTip.Casier);

            // Parola unui cont existent se schimba doar prin „Parolă temporară”.
            if (!nou)
            {
                lblParola.Visible = txtParola.Visible = false;
                var sus = txtParola.Bottom - cmbRol.Bottom;
                btnSalveaza.Top -= sus;
                btnRenunta.Top -= sus;
                ClientSize = new Size(ClientSize.Width, ClientSize.Height - sus);
            }
        }

        public Guid IdSalvat { get; private set; }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            var nou = _utilizator is null;
            var verificare = new VerificareFormular(errorProvider)
                .Obligatoriu(txtNumeUtilizator, "Numele de utilizator")
                .Obligatoriu(txtNume, "Numele")
                .Obligatoriu(txtPrenume, "Prenumele")
                .Email(txtEmail)
                .Telefon(txtTelefon);
            if (nou)
                verificare.Obligatoriu(txtParola, "Parola inițială");
            if (!verificare.Verifica(this))
                return;

            var date = new UtilizatorEditare
            {
                NumeUtilizator = txtNumeUtilizator.Text,
                Nume = txtNume.Text,
                Prenume = txtPrenume.Text,
                Email = txtEmail.Text,
                Telefon = txtTelefon.Text,
                Rol = ((ElementLista<RolTip>)cmbRol.SelectedItem).Valoare,
            };

            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                if (nou)
                    IdSalvat = await Aplicatie.Backend.Utilizatori.AdaugaAsync(date, txtParola.Text);
                else
                {
                    await Aplicatie.Backend.Utilizatori.ActualizeazaAsync(_utilizator.UtilizatorID, date, _utilizator.RowVersion);
                    IdSalvat = _utilizator.UtilizatorID;
                }
            });

            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
