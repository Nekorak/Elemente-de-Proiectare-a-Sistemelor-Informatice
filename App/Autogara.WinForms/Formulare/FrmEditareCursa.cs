using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.Domain.Enumerari;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>Fereastra modala pentru o cursa noua sau modificarea unei curse planificate.</summary>
    public partial class FrmEditareCursa : FormAutogara
    {
        private readonly CursaRand _cursa;

        /// <param name="cursa">Cursa de modificat (doar planificata); null pentru una noua.</param>
        public FrmEditareCursa(CursaRand cursa)
        {
            InitializeComponent();
            _cursa = cursa;
            Text = lblTitlu.Text = cursa is null ? "Cursă nouă" : $"Modificare cursa #{cursa.CursaID}";
        }

        public int IdSalvat { get; private set; }

        /// <summary>Data cursei salvate, ca lista sa o poata cuprinde in interval.</summary>
        public DateOnly DataSalvata { get; private set; }

        private async void FrmEditareCursa_Load(object sender, EventArgs e)
        {
            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                var trasee = await Aplicatie.Backend.Trasee.ListeazaAsync();
                var autobuze = await Aplicatie.Backend.Autobuze.ListeazaAsync();
                var soferi = await Aplicatie.Backend.Soferi.ListeazaAsync();

                cmbTraseu.DataSource = trasee.Select(t => new ElementLista<int>(t.TraseuID, t.Denumire)).ToList();
                cmbAutobuz.DataSource = autobuze
                    .Where(a => a.Status == StatusAutobuz.Activ)
                    .Select(a => new ElementLista<int>(a.AutobuzID, $"{a.NrInmatriculare} — {a.Model} ({a.CapacitateLocuri} locuri)"))
                    .ToList();
                cmbSofer.DataSource = soferi.Select(s => new ElementLista<int>(s.SoferID, s.NumeComplet)).ToList();
            });
            if (!reusit)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            if (_cursa is { } c)
            {
                Selecteaza(cmbTraseu, c.TraseuID);
                Selecteaza(cmbAutobuz, c.AutobuzID);
                Selecteaza(cmbSofer, c.SoferID);
                dtpData.Value = c.DataCursa.ToDateTime(TimeOnly.MinValue);
                dtpPlecare.Value = DateTime.Today.Add(c.OraPlecare.ToTimeSpan());
                dtpSosire.Value = DateTime.Today.Add(c.OraSosireEstimata.ToTimeSpan());
                numPret.Value = c.Pret;
            }
            else
            {
                dtpData.Value = OraLocala.Azi.AddDays(1).ToDateTime(TimeOnly.MinValue);
            }
        }

        private static void Selecteaza(ComboBox cmb, int id)
        {
            foreach (ElementLista<int> e in cmb.Items)
            {
                if (e.Valoare == id)
                {
                    cmb.SelectedItem = e;
                    return;
                }
            }
            cmb.SelectedIndex = -1; // ex. autobuz scos din circulatie intre timp
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!new VerificareFormular(errorProvider)
                    .Conditie(cmbTraseu.SelectedItem is not null, cmbTraseu, "Alegeți traseul.")
                    .Conditie(cmbAutobuz.SelectedItem is not null, cmbAutobuz, "Alegeți un autobuz în circulație.")
                    .Conditie(cmbSofer.SelectedItem is not null, cmbSofer, "Alegeți șoferul.")
                    .Conditie(dtpSosire.Value.TimeOfDay > dtpPlecare.Value.TimeOfDay, dtpSosire, "Ora de sosire trebuie să fie după ora de plecare.")
                    .Verifica(this))
                return;

            var date = new CursaEditare
            {
                TraseuID = ((ElementLista<int>)cmbTraseu.SelectedItem).Valoare,
                AutobuzID = ((ElementLista<int>)cmbAutobuz.SelectedItem).Valoare,
                SoferID = ((ElementLista<int>)cmbSofer.SelectedItem).Valoare,
                DataCursa = DateOnly.FromDateTime(dtpData.Value),
                OraPlecare = new TimeOnly(dtpPlecare.Value.Hour, dtpPlecare.Value.Minute),
                OraSosireEstimata = new TimeOnly(dtpSosire.Value.Hour, dtpSosire.Value.Minute),
                Pret = numPret.Value,
            };

            var reusit = await Mesaje.RuleazaAsync(btnSalveaza, async () =>
            {
                if (_cursa is null)
                    IdSalvat = await Aplicatie.Backend.Curse.CreeazaCursaAsync(date);
                else
                {
                    await Aplicatie.Backend.Curse.ActualizeazaCursaAsync(_cursa.CursaID, date);
                    IdSalvat = _cursa.CursaID;
                }
                DataSalvata = date.DataCursa;
            });

            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
