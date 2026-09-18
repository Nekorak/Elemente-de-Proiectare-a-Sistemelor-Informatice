using Autogara.Business.Dto;
using Autogara.Business.Servicii;
using Autogara.Common;
using Autogara.FileServer.Modele;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Editorul asezarii locurilor unui autobuz; la salvare backend-ul scrie autobuz_{id}.json pe
    /// file server (cu backup) si actualizeaza capacitatea.
    /// </summary>
    public partial class FrmEditorAutobuz : FormAutogara
    {
        private readonly AutobuzRand _autobuz;
        private AutobuzStructura _structura;

        public FrmEditorAutobuz(AutobuzRand autobuz)
        {
            InitializeComponent();
            _autobuz = autobuz;
        }

        private async void FrmEditorAutobuz_Load(object sender, EventArgs e)
        {
            lblTitlu.Text = _autobuz.NrInmatriculare;
            lblModel.Text = _autobuz.Model;

            await Mesaje.RuleazaAsync(this, async () =>
            {
                try
                {
                    _structura = await Aplicatie.Backend.Autobuze.IncarcaStructuraAsync(_autobuz.AutobuzID);
                }
                catch (NegasitException)
                {
                    _structura = AutobuzService.StructuraImplicita(_autobuz.CapacitateLocuri);
                    Mesaje.Info(this, "Autobuzul nu are încă o structură salvată; s-a propus grila implicită.");
                }
            });

            _structura ??= AutobuzService.StructuraImplicita(_autobuz.CapacitateLocuri);
            editor.Structura = _structura;
            numRanduri.Value = _structura.Randuri;
            numColoane.Value = _structura.Coloane;
            numCuloar.Value = _structura.CuloarDupaColoana;
            AfiseazaNumarul();
        }

        private void AfiseazaNumarul()
        {
            var n = _structura?.Locuri.Count ?? 0;
            lblNumar.Text = $"{n} locuri";
            lblCapacitate.Text = n == _autobuz.CapacitateLocuri
                ? $"La fel ca acum în baza de date ({_autobuz.CapacitateLocuri})."
                : $"Acum în baza de date: {_autobuz.CapacitateLocuri} locuri. Capacitatea se va schimba.";
            lblCapacitate.ForeColor = n == _autobuz.CapacitateLocuri ? Tema.TextSecundar : Tema.Avertisment;
        }

        private void editor_StructuraSchimbata(object sender, EventArgs e) => AfiseazaNumarul();

        private void btnAplica_Click(object sender, EventArgs e)
        {
            if (_structura is null)
                return;
            try
            {
                AutobuzService.Redimensioneaza(_structura, (int)numRanduri.Value, (int)numColoane.Value, (int)numCuloar.Value);
                numCuloar.Value = _structura.CuloarDupaColoana;
                editor.Invalidate();
                AfiseazaNumarul();
            }
            catch (Exception ex)
            {
                Mesaje.Eroare(this, ex);
            }
        }

        private async void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (_structura is null)
                return;

            var n = _structura.Locuri.Count;
            if (n != _autobuz.CapacitateLocuri &&
                !Mesaje.Confirma(this, $"Capacitatea autobuzului se schimbă din {_autobuz.CapacitateLocuri} în {n} locuri. Continuați?"))
                return;

            var reusit = await Mesaje.RuleazaAsync(btnSalveaza,
                () => Aplicatie.Backend.Autobuze.ActualizeazaStructuraAsync(_autobuz.AutobuzID, _structura));
            if (reusit)
                DialogResult = DialogResult.OK;
        }
    }
}
