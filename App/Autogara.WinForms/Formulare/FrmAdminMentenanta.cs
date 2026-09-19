using Autogara.Business.Dto;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Formulare
{
    /// <summary>
    /// Istoricul lucrarilor pe autobuze si alertele de ITP. Lucrarile si ITP-urile noi se
    /// inregistreaza in <see cref="FrmEditareLucrare"/> si <see cref="FrmInregistrareItp"/>.
    /// </summary>
    public partial class FrmAdminMentenanta : FormAutogara
    {
        private List<ElementLista<int>> _autobuze = [];
        private bool _seIncarca;

        public FrmAdminMentenanta()
        {
            InitializeComponent();
        }

        private async void FrmAdminMentenanta_Load(object sender, EventArgs e)
        {
            await Mesaje.RuleazaAsync(btnReincarca, async () =>
            {
                var autobuze = await Aplicatie.Backend.Autobuze.ListeazaAsync(includeInactive: true);
                _autobuze = autobuze.Select(a => new ElementLista<int>(a.AutobuzID, $"{a.NrInmatriculare} — {a.Model}")).ToList();
                _seIncarca = true;
                var filtru = _autobuze.Select(x => new ElementLista<int?>(x.Valoare, x.Text)).ToList();
                filtru.Insert(0, new ElementLista<int?>(null, "(toate autobuzele)"));
                cmbFiltru.DataSource = filtru;
                _seIncarca = false;
                await ReincarcaAsync();
            });
        }

        private async void btnReincarca_Click(object sender, EventArgs e) => await Mesaje.RuleazaAsync(btnReincarca, ReincarcaAsync);

        private async void cmbFiltru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_seIncarca)
                await Mesaje.RuleazaAsync(cmbFiltru, ReincarcaAsync);
        }

        private int? AutobuzFiltrat => (cmbFiltru.SelectedItem as ElementLista<int?>)?.Valoare;

        private async Task ReincarcaAsync()
        {
            gridLucrari.Afiseaza(await Aplicatie.Backend.Mentenanta.ListeazaAsync(AutobuzFiltrat));
            var itp = await Aplicatie.Backend.Mentenanta.AutobuzeCuItpApropiatAsync(30);
            gridItp.Afiseaza(itp);
            tpItp.Text = itp.Count == 0 ? "ITP" : $"ITP ({itp.Count})";
        }

        private async void btnLucrare_Click(object sender, EventArgs e)
        {
            using (var f = new FrmEditareLucrare(_autobuze, AutobuzFiltrat))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
            }
            tabLista.SelectedTab = tpLucrari;
            await Mesaje.RuleazaAsync(btnReincarca, ReincarcaAsync);
        }

        private async void btnItp_Click(object sender, EventArgs e)
        {
            // Din lista ITP se propune autobuzul ales acolo; altfel cel din filtru.
            var propus = tabLista.SelectedTab == tpItp ? gridItp.Selectat<ItpRand>()?.AutobuzID : AutobuzFiltrat;
            using (var f = new FrmInregistrareItp(_autobuze, propus))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;
            }
            await Mesaje.RuleazaAsync(btnReincarca, ReincarcaAsync);
        }
    }
}
