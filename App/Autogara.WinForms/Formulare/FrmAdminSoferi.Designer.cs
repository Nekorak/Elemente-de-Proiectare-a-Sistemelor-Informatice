namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminSoferi
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
            colNume = new DataGridViewTextBoxColumn();
            colPrenume = new DataGridViewTextBoxColumn();
            colPermis = new DataGridViewTextBoxColumn();
            colTelefon = new DataGridViewTextBoxColumn();
            colActiv = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            btnReincarca = new Controale.ButonIcon();
            btnNou = new Controale.ButonIcon();
            chkInactive = new CheckBox();
            pnlSpatiuDreapta = new Panel();
            pnlEditare = new Panel();
            lblEditareTitlu = new Label();
            lblNume = new Label();
            txtNume = new TextBox();
            lblPrenume = new Label();
            txtPrenume = new TextBox();
            lblNrPermis = new Label();
            txtNrPermis = new TextBox();
            lblTelefon = new Label();
            txtTelefon = new TextBox();
            btnSalveaza = new Controale.ButonIcon();
            btnActiv = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlEditare.SuspendLayout();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colNume, colPrenume, colPermis, colTelefon, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            // 
            // colNume
            // 
            colNume.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNume.DataPropertyName = "Nume";
            colNume.FillWeight = 100F;
            colNume.HeaderText = "Nume";
            colNume.MinimumWidth = 60;
            colNume.Name = "colNume";
            colNume.ReadOnly = true;
            // 
            // colPrenume
            // 
            colPrenume.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPrenume.DataPropertyName = "Prenume";
            colPrenume.FillWeight = 100F;
            colPrenume.HeaderText = "Prenume";
            colPrenume.MinimumWidth = 60;
            colPrenume.Name = "colPrenume";
            colPrenume.ReadOnly = true;
            // 
            // colPermis
            // 
            colPermis.DataPropertyName = "NrPermis";
            colPermis.HeaderText = "Nr. permis";
            colPermis.Name = "colPermis";
            colPermis.ReadOnly = true;
            colPermis.Width = 120;
            // 
            // colTelefon
            // 
            colTelefon.DataPropertyName = "Telefon";
            colTelefon.HeaderText = "Telefon";
            colTelefon.Name = "colTelefon";
            colTelefon.ReadOnly = true;
            colTelefon.Width = 130;
            // 
            // colActiv
            // 
            colActiv.DataPropertyName = "Activ";
            colActiv.HeaderText = "Activ";
            colActiv.Name = "colActiv";
            colActiv.ReadOnly = true;
            colActiv.Width = 60;
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
            lblLista.Text = "Șoferi";
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
            pnlEditare.Controls.Add(lblNume);
            pnlEditare.Controls.Add(txtNume);
            pnlEditare.Controls.Add(lblPrenume);
            pnlEditare.Controls.Add(txtPrenume);
            pnlEditare.Controls.Add(lblNrPermis);
            pnlEditare.Controls.Add(txtNrPermis);
            pnlEditare.Controls.Add(lblTelefon);
            pnlEditare.Controls.Add(txtTelefon);
            pnlEditare.Controls.Add(btnSalveaza);
            pnlEditare.Controls.Add(btnActiv);
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
            lblEditareTitlu.Text = "Șofer nou";
            // 
            // lblNume
            // 
            lblNume.AutoSize = true;
            lblNume.Location = new Point(20, 56);
            lblNume.Name = "lblNume";
            lblNume.Text = "Nume";
            // 
            // txtNume
            // 
            txtNume.Location = new Point(20, 76);
            txtNume.Name = "txtNume";
            txtNume.Size = new Size(340, 23);
            txtNume.TabIndex = 4;
            // 
            // lblPrenume
            // 
            lblPrenume.AutoSize = true;
            lblPrenume.Location = new Point(20, 112);
            lblPrenume.Name = "lblPrenume";
            lblPrenume.Text = "Prenume";
            // 
            // txtPrenume
            // 
            txtPrenume.Location = new Point(20, 132);
            txtPrenume.Name = "txtPrenume";
            txtPrenume.Size = new Size(340, 23);
            txtPrenume.TabIndex = 5;
            // 
            // lblNrPermis
            // 
            lblNrPermis.AutoSize = true;
            lblNrPermis.Location = new Point(20, 168);
            lblNrPermis.Name = "lblNrPermis";
            lblNrPermis.Text = "Numărul permisului de conducere";
            // 
            // txtNrPermis
            // 
            txtNrPermis.CharacterCasing = CharacterCasing.Upper;
            txtNrPermis.Location = new Point(20, 188);
            txtNrPermis.Name = "txtNrPermis";
            txtNrPermis.Size = new Size(340, 23);
            txtNrPermis.TabIndex = 6;
            // 
            // lblTelefon
            // 
            lblTelefon.AutoSize = true;
            lblTelefon.Location = new Point(20, 224);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.Text = "Telefon (opțional)";
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(20, 244);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(340, 23);
            txtTelefon.TabIndex = 7;
            // 
            // btnSalveaza
            // 
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 288);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(166, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 8;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // btnActiv
            // 
            btnActiv.Icon = "user-x";
            btnActiv.Location = new Point(194, 288);
            btnActiv.Name = "btnActiv";
            btnActiv.Size = new Size(166, 36);
            btnActiv.Stil = Controale.StilButon.Pericol;
            btnActiv.TabIndex = 9;
            btnActiv.Text = " Dezactivează";
            btnActiv.Click += btnActiv_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmAdminSoferi
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Name = "FrmAdminSoferi";
            Text = "Șoferi";
            Load += FrmAdminSoferi_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
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
        private DataGridViewTextBoxColumn colNume;
        private DataGridViewTextBoxColumn colPrenume;
        private DataGridViewTextBoxColumn colPermis;
        private DataGridViewTextBoxColumn colTelefon;
        private DataGridViewTextBoxColumn colActiv;
        private Panel pnlListaAntet;
        private Label lblLista;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnNou;
        private CheckBox chkInactive;
        private Panel pnlSpatiuDreapta;
        private Panel pnlEditare;
        private Label lblEditareTitlu;
        private Label lblNume;
        private TextBox txtNume;
        private Label lblPrenume;
        private TextBox txtPrenume;
        private Label lblNrPermis;
        private TextBox txtNrPermis;
        private Label lblTelefon;
        private TextBox txtTelefon;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnActiv;
        private ErrorProvider errorProvider;
    }
}
