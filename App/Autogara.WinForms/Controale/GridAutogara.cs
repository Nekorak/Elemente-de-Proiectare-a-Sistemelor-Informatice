using System.ComponentModel;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    /// <summary>
    /// DataGridView cu aspectul aplicatiei: doar citire, selectie pe rand, fara coloane generate
    /// automat (coloanele se definesc in Designer, cu DataPropertyName). Statusurile, datele si
    /// sumele se afiseaza prin <see cref="Afisare"/>.
    /// </summary>
    public class GridAutogara : DataGridView
    {
        public GridAutogara()
        {
            AutoGenerateColumns = false;
            ReadOnly = true;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            MultiSelect = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RowHeadersVisible = false;
            BorderStyle = BorderStyle.None;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            BackgroundColor = Tema.Suprafata;
            GridColor = Tema.Bordura;
            EnableHeadersVisualStyles = false;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersHeight = 36;
            RowTemplate.Height = 32;

            ColumnHeadersDefaultCellStyle.BackColor = Tema.Fundal;
            ColumnHeadersDefaultCellStyle.ForeColor = Tema.TextSecundar;
            ColumnHeadersDefaultCellStyle.SelectionBackColor = Tema.Fundal;
            ColumnHeadersDefaultCellStyle.Font = Tema.FontIngrosat();
            ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            DefaultCellStyle.ForeColor = Tema.Text;
            DefaultCellStyle.SelectionBackColor = Tema.PrimarDeschis;
            DefaultCellStyle.SelectionForeColor = Tema.Text;
            DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        }

        // Aspectul vine din constructor; Designer-ul salveaza doar coloanele.
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellStyle ColumnHeadersDefaultCellStyle => base.ColumnHeadersDefaultCellStyle;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellStyle DefaultCellStyle => base.DefaultCellStyle;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewRow RowTemplate => base.RowTemplate;

        /// <summary>Obiectul de pe randul selectat (sau null).</summary>
        public T Selectat<T>() where T : class => CurrentRow?.DataBoundItem as T;

        /// <summary>Pune lista in tabel si pastreaza, daca se poate, randul selectat inainte.</summary>
        public void Afiseaza<T>(IReadOnlyList<T> randuri, Func<T, object> cheie = null)
        {
            var cheieSelectata = cheie is not null && CurrentRow?.DataBoundItem is T vechi ? cheie(vechi) : null;
            DataSource = randuri.ToList();

            if (cheieSelectata is null)
                return;

            foreach (DataGridViewRow r in Rows)
            {
                if (r.DataBoundItem is T t && Equals(cheie(t), cheieSelectata))
                {
                    CurrentCell = r.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                    break;
                }
            }
        }

        /// <summary>Selecteaza primul rand care respecta conditia (ex. inregistrarea abia salvata).</summary>
        public void SelecteazaRand<T>(Func<T, bool> conditie)
        {
            foreach (DataGridViewRow r in Rows)
            {
                if (r.DataBoundItem is T t && conditie(t))
                {
                    CurrentCell = r.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                    return;
                }
            }
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            base.OnCellFormatting(e);

            // Statusul curselor din view-urile SQL vine ca text ("In desfasurare").
            if (e.Value is string text && Columns[e.ColumnIndex].DataPropertyName is "Status" or "StatusCursa")
            {
                e.Value = Afisare.StatusCursaSql(text);
                e.FormattingApplied = true;
                return;
            }

            if (e.Value is null or string)
                return;

            e.Value = Afisare.Text(e.Value);
            e.FormattingApplied = true;
        }

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            // Formatarea e facuta de noi; nu afisam dialogul standard.
            e.ThrowException = false;
        }
    }
}
