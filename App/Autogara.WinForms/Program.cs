using Autogara.Business;
using Autogara.Common;
using Autogara.Common.Configurare;
using Autogara.WinForms.Formulare;
using Autogara.WinForms.Ui;
using Serilog;

namespace Autogara.WinForms
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => Mesaje.Eroare(Form.ActiveForm, e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (_, e) => Log.Fatal(e.ExceptionObject as Exception, "Eroare fatală");

            var store = new AppSettingsStore();
            var setari = CitesteSetari(store);
            if (setari is null)
                return;

            using var backend = AutogaraBackend.Creeaza(setari);
            Aplicatie.Backend = backend;
            backend.Conexiune.Porneste();

            while (true)
            {
                using (var login = new FrmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        break;
                }

                using var main = new FrmMain();
                Application.Run(main);
                if (!main.Deconectare)
                    break;
            }
        }

        /// <summary>Setarile statiei; la prima pornire (sau daca lipsesc parolele) deschide configurarea.</summary>
        private static AppSettings CitesteSetari(AppSettingsStore store)
        {
            AppSettings setari = null;
            try
            {
                if (store.Exista)
                    setari = store.Citeste();
            }
            catch (Exception ex)
            {
                Mesaje.Eroare(null, new AutogaraException("Fișierul de configurare nu a putut fi citit; reluați configurarea.", ex));
            }

            if (setari is not null && setari.Valideaza().Count == 0)
                return setari;

            using var wizard = new FrmSetupWizard(store, setari ?? AppSettings.Implicite());
            return wizard.ShowDialog() == DialogResult.OK ? store.Citeste() : null;
        }
    }
}
