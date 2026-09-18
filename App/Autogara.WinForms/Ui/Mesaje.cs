using Autogara.Business.Reguli;
using Autogara.Common;

namespace Autogara.WinForms.Ui
{
    /// <summary>
    /// Rularea apelurilor catre backend din formulare: cursor de asteptare, butonul dezactivat
    /// cat dureaza, iar orice eroare apare ca mesaj pentru utilizator.
    /// </summary>
    public static class Mesaje
    {
        private const string Titlu = "Autogara";

        /// <returns>true daca actiunea s-a terminat fara eroare.</returns>
        public static async Task<bool> RuleazaAsync(Control sursa, Func<Task> actiune)
        {
            var forma = sursa?.FindForm();
            var cursor = forma?.Cursor ?? Cursors.Default;
            if (sursa is not null && sursa is not Form)
                sursa.Enabled = false;
            if (forma is not null)
                forma.Cursor = Cursors.WaitCursor;

            try
            {
                await actiune();
                return true;
            }
            catch (Exception ex)
            {
                Eroare(forma, ex);
                return false;
            }
            finally
            {
                if (sursa is not null && sursa is not Form && !sursa.IsDisposed)
                    sursa.Enabled = true;
                if (forma is not null && !forma.IsDisposed)
                    forma.Cursor = cursor;
            }
        }

        public static void Eroare(IWin32Window proprietar, Exception ex)
        {
            var mesaj = ExceptionHandler.MesajPrietenos(ex);
            var icon = ex switch
            {
                ValidareException or RegulaException => MessageBoxIcon.Warning,
                AccesInterzisException => MessageBoxIcon.Stop,
                _ => MessageBoxIcon.Error,
            };
            MessageBox.Show(proprietar, mesaj, Titlu, MessageBoxButtons.OK, icon);
        }

        public static void Info(IWin32Window proprietar, string mesaj) =>
            MessageBox.Show(proprietar, mesaj, Titlu, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Atentie(IWin32Window proprietar, string mesaj) =>
            MessageBox.Show(proprietar, mesaj, Titlu, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static bool Confirma(IWin32Window proprietar, string mesaj) =>
            MessageBox.Show(proprietar, mesaj, Titlu, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
    }

    /// <summary>
    /// Validarea de format pe formular, cu ErrorProvider (campuri obligatorii, telefon, email).
    /// Regulile de business raman in backend; aici se prind doar greselile evidente de completare.
    /// </summary>
    public sealed class VerificareFormular
    {
        private readonly ErrorProvider _erori;
        private readonly List<string> _mesaje = [];

        public VerificareFormular(ErrorProvider erori)
        {
            _erori = erori;
            _erori.Clear();
        }

        public bool Valid => _mesaje.Count == 0;

        public VerificareFormular Obligatoriu(Control c, string camp)
        {
            if (string.IsNullOrWhiteSpace(c.Text))
                Seteaza(c, $"{camp} este obligatoriu.");
            return this;
        }

        public VerificareFormular Telefon(Control c)
        {
            var erori = new List<string>();
            Validari.Telefon(erori, c.Text);
            if (erori.Count > 0) Seteaza(c, erori[0]);
            return this;
        }

        public VerificareFormular Email(Control c)
        {
            var erori = new List<string>();
            Validari.Email(erori, c.Text);
            if (erori.Count > 0) Seteaza(c, erori[0]);
            return this;
        }

        public VerificareFormular Conditie(bool corect, Control c, string mesaj)
        {
            if (!corect) Seteaza(c, mesaj);
            return this;
        }

        /// <summary>Daca exista erori, le arata si intoarce false.</summary>
        public bool Verifica(IWin32Window proprietar)
        {
            if (Valid)
                return true;
            Mesaje.Atentie(proprietar, string.Join(Environment.NewLine, _mesaje));
            return false;
        }

        private void Seteaza(Control c, string mesaj)
        {
            _erori.SetError(c, mesaj);
            _mesaje.Add(mesaj);
        }
    }
}
