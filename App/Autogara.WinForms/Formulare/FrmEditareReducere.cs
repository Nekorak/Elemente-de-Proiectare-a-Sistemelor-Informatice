using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru un tip de reducere nou sau modificarea unuia existent.</summary>
    public partial class FrmEditareReducere : FormAutogara
    {
        private readonly TipReducereRand _reducere;

        /// <param name="reducere">Reducerea de modificat; null pentru una noua.</param>
        public FrmEditareReducere(TipReducereRand reducere)
        {
            InitializeComponent();
            _reducere = reducere;

            Text = lblTitlu.Text = reducere is null ? "Reducere nouă" : $"Modificare reducere — {reducere.Denumire}";
            txtDenumire.Text = reducere?.Denumire ?? "";
            numProcent.Value = reducere?.ProcentReducere ?? 0;
            chkActiv.Checked = reducere?.Activ ?? true;
            chkActiv.Enabled = reducere is not null;
        }

        public int IdSalvat { get; private set; }

        /// <summary>Reducerea a ramas activa dupa salvare.</summary>
        public bool Activa => chkActiv.Checked;

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider).Obligatoriu(txtDenumire, "Denumirea").Verifica(this))
                return;

            var date = new TipReducereEditare { Denumire = txtDenumire.Text, ProcentReducere = numProcent.Value };
            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                if (_reducere is null)
                    IdSalvat = await Aplicatie.Backend.Reduceri.AdaugaAsync(date);
                else
                {
                    await Aplicatie.Backend.Reduceri.ActualizeazaAsync(_reducere.TipReducereID, date, chkActiv.Checked);
                    IdSalvat = _reducere.TipReducereID;
                }
            });

            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
