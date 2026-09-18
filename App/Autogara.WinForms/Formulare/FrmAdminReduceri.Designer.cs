namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminReduceri
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
            components = new System.ComponentModel.Container();
            pnlLista = new Panel();
            gridLista = new Controale.GridAutogara();
            colDenumire = new DataGridViewTextBoxColumn();
            colProcent = new DataGridViewTextBoxColumn();
            colActiv = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            btnReincarca = new Controale.ButonIcon();
            btnNou = new Controale.ButonIcon();
            chkInactive = new CheckBox();
            pnlSpatiuDreapta = new Panel();
            pnlEditare = new Panel();
            lblEditareTitlu = new Label();
            lblDenumire = new Label();
            txtDenumire = new TextBox();
            lblProcent = new Label();
            numProcent = new NumericUpDown();
            chkActiv = new CheckBox();
            lblNota = new Label();
            btnSalveaza = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlEditare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numProcent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlLista
            // 
            pnlLista.Controls.Add(gridLista);
            pnlLista.Controls.Add(pnlListaAntet);
            pnlLista.BackColor = Color.White;
            pnlLista.Dock = DockStyle.Fill;
            pnlLista.Name = "pnlLista";
            pnlLista.Padding = new Padding(12, 0, 12, 12);
            // 
            // gridLista
            // 
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colDenumire, colProcent, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            // 
            // colDenumire
            // 
            colDenumire.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDenumire.DataPropertyName = "Denumire";
            colDenumire.FillWeight = 100F;
            colDenumire.HeaderText = "Denumire";
            colDenumire.MinimumWidth = 60;
            colDenumire.Name = "colDenumire";
            colDenumire.ReadOnly = true;
            // 
            // colProcent
            // 
            colProcent.DataPropertyName = "ProcentReducere";
            colProcent.HeaderText = "Reducere (%)";
            colProcent.Name = "colProcent";
            colProcent.ReadOnly = true;
            colProcent.Width = 110;
            // 
            // colActiv
            // 
            colActiv.DataPropertyName = "Activ";
            colActiv.HeaderText = "Activă";
            colActiv.Name = "colActiv";
            colActiv.ReadOnly = true;
            colActiv.Width = 70;
            // 
            // pnlListaAntet
            // 
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(btnReincarca);
            pnlListaAntet.Controls.Add(btnNou);
            pnlListaAntet.Controls.Add(chkInactive);
            pnlListaAntet.Dock = DockStyle.Top;
            pnlListaAntet.Name = "pnlListaAntet";
            pnlListaAntet.Size = new Size(600, 52);
            // 
            // lblLista
            // 
            lblLista.AutoSize = true;
            lblLista.Font = new Font("Segoe UI Semibold", 11F);
            lblLista.ForeColor = Color.FromArgb(31, 41, 55);
            lblLista.Location = new Point(4, 16);
            lblLista.Name = "lblLista";
            lblLista.Text = "Tipuri de reducere";
            // 
            // btnReincarca
            // 
            btnReincarca.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReincarca.Icon = "refresh-cw";
            btnReincarca.Location = new Point(552, 10);
            btnReincarca.Name = "btnReincarca";
            btnReincarca.Size = new Size(36, 32);
            btnReincarca.TabIndex = 1;
            btnReincarca.Click += btnReincarca_Click;
            // 
            // btnNou
            // 
            btnNou.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNou.Icon = "plus";
            btnNou.Location = new Point(454, 10);
            btnNou.Name = "btnNou";
            btnNou.Size = new Size(90, 32);
            btnNou.Stil = Controale.StilButon.Primar;
            btnNou.TabIndex = 2;
            btnNou.Text = " Nou";
            btnNou.Click += btnNou_Click;
            // 
            // chkInactive
            // 
            chkInactive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkInactive.AutoSize = true;
            chkInactive.Location = new Point(304, 17);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 3;
            chkInactive.Text = "Arată și inactive";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            // 
            // pnlSpatiuDreapta
            // 
            pnlSpatiuDreapta.Dock = DockStyle.Right;
            pnlSpatiuDreapta.Name = "pnlSpatiuDreapta";
            pnlSpatiuDreapta.Size = new Size(16, 680);
            // 
            // pnlEditare
            // 
            pnlEditare.Controls.Add(lblEditareTitlu);
            pnlEditare.Controls.Add(lblDenumire);
            pnlEditare.Controls.Add(txtDenumire);
            pnlEditare.Controls.Add(lblProcent);
            pnlEditare.Controls.Add(numProcent);
            pnlEditare.Controls.Add(chkActiv);
            pnlEditare.Controls.Add(lblNota);
            pnlEditare.Controls.Add(btnSalveaza);
            pnlEditare.BackColor = Color.White;
            pnlEditare.Dock = DockStyle.Right;
            pnlEditare.Name = "pnlEditare";
            pnlEditare.Size = new Size(380, 680);
            // 
            // lblEditareTitlu
            // 
            lblEditareTitlu.AutoSize = true;
            lblEditareTitlu.Font = new Font("Segoe UI Semibold", 11F);
            lblEditareTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblEditareTitlu.Location = new Point(20, 18);
            lblEditareTitlu.Name = "lblEditareTitlu";
            lblEditareTitlu.Text = "Reducere nouă";
            // 
            // lblDenumire
            // 
            lblDenumire.AutoSize = true;
            lblDenumire.Location = new Point(20, 56);
            lblDenumire.Name = "lblDenumire";
            lblDenumire.Text = "Denumire";
            // 
            // txtDenumire
            // 
            txtDenumire.Location = new Point(20, 76);
            txtDenumire.Name = "txtDenumire";
            txtDenumire.Size = new Size(340, 23);
            txtDenumire.TabIndex = 4;
            // 
            // lblProcent
            // 
            lblProcent.AutoSize = true;
            lblProcent.Location = new Point(20, 112);
            lblProcent.Name = "lblProcent";
            lblProcent.Text = "Reducere (%)";
            // 
            // numProcent
            // 
            numProcent.DecimalPlaces = 2;
            numProcent.Location = new Point(20, 132);
            numProcent.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numProcent.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numProcent.Name = "numProcent";
            numProcent.Size = new Size(120, 23);
            numProcent.TabIndex = 5;
            // 
            // chkActiv
            // 
            chkActiv.AutoSize = true;
            chkActiv.Location = new Point(20, 168);
            chkActiv.Name = "chkActiv";
            chkActiv.TabIndex = 6;
            chkActiv.Text = "Activă (se poate alege la vânzare)";
            // 
            // lblNota
            // 
            lblNota.AutoSize = true;
            lblNota.ForeColor = Color.FromArgb(107, 114, 128);
            lblNota.Location = new Point(20, 200);
            lblNota.Name = "lblNota";
            lblNota.Text = "Biletele vândute își păstrează prețul;\nprocentul nou se aplică doar de acum.";
            // 
            // btnSalveaza
            // 
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 248);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(166, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 7;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmAdminReduceri
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Name = "FrmAdminReduceri";
            Text = "Reduceri";
            Load += FrmAdminReduceri_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)numProcent).EndInit();
            pnlEditare.ResumeLayout(false);
            pnlEditare.PerformLayout();
            pnlSpatiuDreapta.ResumeLayout(false);
            pnlListaAntet.ResumeLayout(false);
            pnlListaAntet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).EndInit();
            pnlLista.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLista;
        private Controale.GridAutogara gridLista;
        private DataGridViewTextBoxColumn colDenumire;
        private DataGridViewTextBoxColumn colProcent;
        private DataGridViewTextBoxColumn colActiv;
        private Panel pnlListaAntet;
        private Label lblLista;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnNou;
        private CheckBox chkInactive;
        private Panel pnlSpatiuDreapta;
        private Panel pnlEditare;
        private Label lblEditareTitlu;
        private Label lblDenumire;
        private TextBox txtDenumire;
        private Label lblProcent;
        private NumericUpDown numProcent;
        private CheckBox chkActiv;
        private Label lblNota;
        private Controale.ButonIcon btnSalveaza;
        private ErrorProvider errorProvider;
    }
}
