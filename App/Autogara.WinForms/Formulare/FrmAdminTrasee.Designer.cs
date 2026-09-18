namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminTrasee
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
            colPlecare = new DataGridViewTextBoxColumn();
            colSosire = new DataGridViewTextBoxColumn();
            colOpriri = new DataGridViewTextBoxColumn();
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
            lblOpriri = new Label();
            lstOpriri = new ListBox();
            btnSus = new Controale.ButonIcon();
            btnJos = new Controale.ButonIcon();
            btnScoate = new Controale.ButonIcon();
            cmbStatie = new ComboBox();
            btnAdaugaOprire = new Controale.ButonIcon();
            lblDistanta = new Label();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colDenumire, colPlecare, colSosire, colOpriri, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            // 
            // colDenumire
            // 
            colDenumire.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDenumire.DataPropertyName = "Denumire";
            colDenumire.FillWeight = 160F;
            colDenumire.HeaderText = "Denumire";
            colDenumire.MinimumWidth = 60;
            colDenumire.Name = "colDenumire";
            colDenumire.ReadOnly = true;
            // 
            // colPlecare
            // 
            colPlecare.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPlecare.DataPropertyName = "Plecare";
            colPlecare.FillWeight = 100F;
            colPlecare.HeaderText = "Plecare";
            colPlecare.MinimumWidth = 60;
            colPlecare.Name = "colPlecare";
            colPlecare.ReadOnly = true;
            // 
            // colSosire
            // 
            colSosire.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colSosire.DataPropertyName = "Sosire";
            colSosire.FillWeight = 100F;
            colSosire.HeaderText = "Sosire";
            colSosire.MinimumWidth = 60;
            colSosire.Name = "colSosire";
            colSosire.ReadOnly = true;
            // 
            // colOpriri
            // 
            colOpriri.DataPropertyName = "NrOpriri";
            colOpriri.HeaderText = "Opriri";
            colOpriri.Name = "colOpriri";
            colOpriri.ReadOnly = true;
            colOpriri.Width = 64;
            // 
            // colActiv
            // 
            colActiv.DataPropertyName = "Activ";
            colActiv.HeaderText = "Activ";
            colActiv.Name = "colActiv";
            colActiv.ReadOnly = true;
            colActiv.Width = 56;
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
            lblLista.Text = "Trasee";
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
            pnlEditare.Controls.Add(lblOpriri);
            pnlEditare.Controls.Add(lstOpriri);
            pnlEditare.Controls.Add(btnSus);
            pnlEditare.Controls.Add(btnJos);
            pnlEditare.Controls.Add(btnScoate);
            pnlEditare.Controls.Add(cmbStatie);
            pnlEditare.Controls.Add(btnAdaugaOprire);
            pnlEditare.Controls.Add(lblDistanta);
            pnlEditare.Controls.Add(btnSalveaza);
            pnlEditare.Controls.Add(btnActiv);
            pnlEditare.BackColor = Color.White;
            pnlEditare.Dock = DockStyle.Right;
            pnlEditare.Name = "pnlEditare";
            pnlEditare.Size = new Size(400, 680);
            // 
            // lblEditareTitlu
            // 
            lblEditareTitlu.AutoSize = true;
            lblEditareTitlu.Font = new Font("Segoe UI Semibold", 11F);
            lblEditareTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblEditareTitlu.Location = new Point(20, 18);
            lblEditareTitlu.Name = "lblEditareTitlu";
            lblEditareTitlu.Text = "Traseu nou";
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
            txtDenumire.Size = new Size(360, 23);
            txtDenumire.TabIndex = 4;
            // 
            // lblOpriri
            // 
            lblOpriri.AutoSize = true;
            lblOpriri.Location = new Point(20, 112);
            lblOpriri.Name = "lblOpriri";
            lblOpriri.Text = "Opririle, în ordine";
            // 
            // lstOpriri
            // 
            lstOpriri.FormattingEnabled = true;
            lstOpriri.IntegralHeight = false;
            lstOpriri.Location = new Point(20, 132);
            lstOpriri.Name = "lstOpriri";
            lstOpriri.Size = new Size(316, 200);
            lstOpriri.TabIndex = 5;
            // 
            // btnSus
            // 
            btnSus.Icon = "arrow-up";
            btnSus.Location = new Point(344, 132);
            btnSus.Name = "btnSus";
            btnSus.Size = new Size(36, 32);
            btnSus.TabIndex = 6;
            btnSus.Click += btnSus_Click;
            // 
            // btnJos
            // 
            btnJos.Icon = "arrow-down";
            btnJos.Location = new Point(344, 170);
            btnJos.Name = "btnJos";
            btnJos.Size = new Size(36, 32);
            btnJos.TabIndex = 7;
            btnJos.Click += btnJos_Click;
            // 
            // btnScoate
            // 
            btnScoate.Icon = "minus";
            btnScoate.Location = new Point(344, 208);
            btnScoate.Name = "btnScoate";
            btnScoate.Size = new Size(36, 32);
            btnScoate.Stil = Controale.StilButon.Pericol;
            btnScoate.TabIndex = 8;
            btnScoate.Click += btnScoate_Click;
            // 
            // cmbStatie
            // 
            cmbStatie.DisplayMember = "Text";
            cmbStatie.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatie.Location = new Point(20, 344);
            cmbStatie.Name = "cmbStatie";
            cmbStatie.Size = new Size(256, 23);
            cmbStatie.TabIndex = 9;
            // 
            // btnAdaugaOprire
            // 
            btnAdaugaOprire.Icon = "plus";
            btnAdaugaOprire.Location = new Point(284, 340);
            btnAdaugaOprire.Name = "btnAdaugaOprire";
            btnAdaugaOprire.Size = new Size(96, 32);
            btnAdaugaOprire.TabIndex = 10;
            btnAdaugaOprire.Text = " Adaugă";
            btnAdaugaOprire.Click += btnAdaugaOprire_Click;
            // 
            // lblDistanta
            // 
            lblDistanta.AutoSize = true;
            lblDistanta.ForeColor = Color.FromArgb(107, 114, 128);
            lblDistanta.Location = new Point(20, 384);
            lblDistanta.Name = "lblDistanta";
            lblDistanta.Text = "";
            // 
            // btnSalveaza
            // 
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 416);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(176, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 11;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // btnActiv
            // 
            btnActiv.Icon = "ban";
            btnActiv.Location = new Point(204, 416);
            btnActiv.Name = "btnActiv";
            btnActiv.Size = new Size(176, 36);
            btnActiv.Stil = Controale.StilButon.Pericol;
            btnActiv.TabIndex = 12;
            btnActiv.Text = " Dezactivează";
            btnActiv.Click += btnActiv_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmAdminTrasee
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Name = "FrmAdminTrasee";
            Text = "Trasee";
            Load += FrmAdminTrasee_Load;
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
        private DataGridViewTextBoxColumn colDenumire;
        private DataGridViewTextBoxColumn colPlecare;
        private DataGridViewTextBoxColumn colSosire;
        private DataGridViewTextBoxColumn colOpriri;
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
        private Label lblOpriri;
        private ListBox lstOpriri;
        private Controale.ButonIcon btnSus;
        private Controale.ButonIcon btnJos;
        private Controale.ButonIcon btnScoate;
        private ComboBox cmbStatie;
        private Controale.ButonIcon btnAdaugaOprire;
        private Label lblDistanta;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnActiv;
        private ErrorProvider errorProvider;
    }
}
