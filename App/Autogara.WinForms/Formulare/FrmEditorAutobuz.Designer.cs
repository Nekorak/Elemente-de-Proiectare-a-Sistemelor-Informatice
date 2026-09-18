namespace Autogara.WinForms.Formulare
{
    partial class FrmEditorAutobuz
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlEditor = new Panel();
            editor = new Controale.AutobuzEditorControl();
            pnlStanga = new Panel();
            iconTitlu = new Controale.IconImagine();
            lblTitlu = new Label();
            lblModel = new Label();
            pnlLinie = new Panel();
            lblGrila = new Label();
            lblRanduri = new Label();
            numRanduri = new NumericUpDown();
            lblColoane = new Label();
            numColoane = new NumericUpDown();
            lblCuloar = new Label();
            numCuloar = new NumericUpDown();
            lblCuloarNota = new Label();
            btnAplica = new Controale.ButonIcon();
            lblNumar = new Label();
            lblCapacitate = new Label();
            iconAjutor = new Controale.IconImagine();
            lblAjutor = new Label();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            pnlEditor.SuspendLayout();
            pnlStanga.SuspendLayout();
            pnlLinie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRanduri).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numColoane).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCuloar).BeginInit();
            SuspendLayout();
            // 
            // pnlEditor
            // 
            pnlEditor.Controls.Add(editor);
            pnlEditor.Dock = DockStyle.Fill;
            pnlEditor.Name = "pnlEditor";
            pnlEditor.Padding = new Padding(16, 16, 16, 16);
            // 
            // editor
            // 
            editor.Dock = DockStyle.Fill;
            editor.Name = "editor";
            editor.StructuraSchimbata += editor_StructuraSchimbata;
            // 
            // pnlStanga
            // 
            pnlStanga.Controls.Add(iconTitlu);
            pnlStanga.Controls.Add(lblTitlu);
            pnlStanga.Controls.Add(lblModel);
            pnlStanga.Controls.Add(pnlLinie);
            pnlStanga.Controls.Add(lblGrila);
            pnlStanga.Controls.Add(lblRanduri);
            pnlStanga.Controls.Add(numRanduri);
            pnlStanga.Controls.Add(lblColoane);
            pnlStanga.Controls.Add(numColoane);
            pnlStanga.Controls.Add(lblCuloar);
            pnlStanga.Controls.Add(numCuloar);
            pnlStanga.Controls.Add(lblCuloarNota);
            pnlStanga.Controls.Add(btnAplica);
            pnlStanga.Controls.Add(lblNumar);
            pnlStanga.Controls.Add(lblCapacitate);
            pnlStanga.Controls.Add(iconAjutor);
            pnlStanga.Controls.Add(lblAjutor);
            pnlStanga.Controls.Add(btnSalveaza);
            pnlStanga.Controls.Add(btnRenunta);
            pnlStanga.BackColor = Color.White;
            pnlStanga.Dock = DockStyle.Left;
            pnlStanga.Name = "pnlStanga";
            pnlStanga.Size = new Size(300, 640);
            // 
            // iconTitlu
            // 
            iconTitlu.Icon = "bus";
            iconTitlu.Location = new Point(20, 20);
            iconTitlu.Name = "iconTitlu";
            iconTitlu.Size = new Size(26, 26);
            // 
            // lblTitlu
            // 
            lblTitlu.AutoSize = true;
            lblTitlu.Font = new Font("Segoe UI Semibold", 13F);
            lblTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitlu.Location = new Point(52, 18);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Text = "Autobuz";
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.ForeColor = Color.FromArgb(107, 114, 128);
            lblModel.Location = new Point(22, 50);
            lblModel.Name = "lblModel";
            lblModel.Text = "";
            // 
            // pnlLinie
            // 
            pnlLinie.BackColor = Color.FromArgb(217, 222, 229);
            pnlLinie.Location = new Point(20, 80);
            pnlLinie.Name = "pnlLinie";
            pnlLinie.Size = new Size(260, 1);
            // 
            // lblGrila
            // 
            lblGrila.AutoSize = true;
            lblGrila.Font = new Font("Segoe UI Semibold", 10F);
            lblGrila.ForeColor = Color.FromArgb(31, 41, 55);
            lblGrila.Location = new Point(20, 94);
            lblGrila.Name = "lblGrila";
            lblGrila.Text = "Grila";
            // 
            // lblRanduri
            // 
            lblRanduri.AutoSize = true;
            lblRanduri.Location = new Point(20, 126);
            lblRanduri.Name = "lblRanduri";
            lblRanduri.Text = "Rânduri";
            // 
            // numRanduri
            // 
            numRanduri.Location = new Point(170, 122);
            numRanduri.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numRanduri.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRanduri.Name = "numRanduri";
            numRanduri.Size = new Size(90, 23);
            numRanduri.TabIndex = 0;
            // 
            // lblColoane
            // 
            lblColoane.AutoSize = true;
            lblColoane.Location = new Point(20, 160);
            lblColoane.Name = "lblColoane";
            lblColoane.Text = "Locuri pe rând";
            // 
            // numColoane
            // 
            numColoane.Location = new Point(170, 156);
            numColoane.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            numColoane.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numColoane.Name = "numColoane";
            numColoane.Size = new Size(90, 23);
            numColoane.TabIndex = 1;
            // 
            // lblCuloar
            // 
            lblCuloar.AutoSize = true;
            lblCuloar.Location = new Point(20, 194);
            lblCuloar.Name = "lblCuloar";
            lblCuloar.Text = "Culoar după coloana";
            // 
            // numCuloar
            // 
            numCuloar.Location = new Point(170, 190);
            numCuloar.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numCuloar.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numCuloar.Name = "numCuloar";
            numCuloar.Size = new Size(90, 23);
            numCuloar.TabIndex = 2;
            // 
            // lblCuloarNota
            // 
            lblCuloarNota.AutoSize = true;
            lblCuloarNota.ForeColor = Color.FromArgb(107, 114, 128);
            lblCuloarNota.Location = new Point(20, 218);
            lblCuloarNota.Name = "lblCuloarNota";
            lblCuloarNota.Text = "0 = fără culoar";
            // 
            // btnAplica
            // 
            btnAplica.Icon = "grid-3x3";
            btnAplica.Location = new Point(20, 244);
            btnAplica.Name = "btnAplica";
            btnAplica.Size = new Size(260, 32);
            btnAplica.TabIndex = 3;
            btnAplica.Text = " Aplică grila";
            btnAplica.Click += btnAplica_Click;
            // 
            // lblNumar
            // 
            lblNumar.AutoSize = true;
            lblNumar.Font = new Font("Segoe UI Semibold", 16F);
            lblNumar.ForeColor = Color.FromArgb(31, 41, 55);
            lblNumar.Location = new Point(18, 294);
            lblNumar.Name = "lblNumar";
            lblNumar.Text = "0 locuri";
            // 
            // lblCapacitate
            // 
            lblCapacitate.ForeColor = Color.FromArgb(107, 114, 128);
            lblCapacitate.Location = new Point(20, 330);
            lblCapacitate.Name = "lblCapacitate";
            lblCapacitate.Size = new Size(260, 40);
            lblCapacitate.Text = "";
            // 
            // iconAjutor
            // 
            iconAjutor.Culoare = Color.FromArgb(107, 114, 128);
            iconAjutor.Icon = "mouse-pointer-2";
            iconAjutor.Location = new Point(20, 384);
            iconAjutor.Name = "iconAjutor";
            iconAjutor.Size = new Size(16, 16);
            // 
            // lblAjutor
            // 
            lblAjutor.ForeColor = Color.FromArgb(107, 114, 128);
            lblAjutor.Location = new Point(42, 382);
            lblAjutor.Name = "lblAjutor";
            lblAjutor.Size = new Size(238, 90);
            lblAjutor.Text = "Click pe un loc îl scoate; click pe o poziție goală adaugă un loc. Locurile se numerotează automat, pe rânduri, de la stânga la dreapta.";
            // 
            // btnSalveaza
            // 
            btnSalveaza.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 540);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(260, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 4;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // btnRenunta
            // 
            btnRenunta.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Icon = "x";
            btnRenunta.Location = new Point(20, 584);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(260, 32);
            btnRenunta.TabIndex = 5;
            btnRenunta.Text = " Renunță";
            // 
            // FrmEditorAutobuz
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnRenunta;
            ClientSize = new Size(900, 640);
            Controls.Add(pnlEditor);
            Controls.Add(pnlStanga);
            MinimizeBox = false;
            MinimumSize = new Size(800, 600);
            Name = "FrmEditorAutobuz";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Locurile din autobuz";
            Load += FrmEditorAutobuz_Load;
            ((System.ComponentModel.ISupportInitialize)numCuloar).EndInit();
            ((System.ComponentModel.ISupportInitialize)numColoane).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRanduri).EndInit();
            pnlLinie.ResumeLayout(false);
            pnlStanga.ResumeLayout(false);
            pnlStanga.PerformLayout();
            pnlEditor.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlEditor;
        private Controale.AutobuzEditorControl editor;
        private Panel pnlStanga;
        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblModel;
        private Panel pnlLinie;
        private Label lblGrila;
        private Label lblRanduri;
        private NumericUpDown numRanduri;
        private Label lblColoane;
        private NumericUpDown numColoane;
        private Label lblCuloar;
        private NumericUpDown numCuloar;
        private Label lblCuloarNota;
        private Controale.ButonIcon btnAplica;
        private Label lblNumar;
        private Label lblCapacitate;
        private Controale.IconImagine iconAjutor;
        private Label lblAjutor;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
    }
}
