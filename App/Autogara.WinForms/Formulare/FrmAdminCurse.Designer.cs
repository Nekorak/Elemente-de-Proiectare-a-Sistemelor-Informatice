namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminCurse
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
            colData = new DataGridViewTextBoxColumn();
            colPlecare = new DataGridViewTextBoxColumn();
            colTraseu = new DataGridViewTextBoxColumn();
            colAutobuz = new DataGridViewTextBoxColumn();
            colPret = new DataGridViewTextBoxColumn();
            colLibere = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            lblNumar = new Label();
            btnNou = new Controale.ButonIcon();
            pnlSpatiuDreapta = new Panel();
            pnlEditare = new Panel();
            lblEditareTitlu = new Label();
            pnlCampuri = new Panel();
            lblTraseu = new Label();
            cmbTraseu = new ComboBox();
            lblAutobuz = new Label();
            cmbAutobuz = new ComboBox();
            lblSofer = new Label();
            cmbSofer = new ComboBox();
            lblData = new Label();
            dtpData = new DateTimePicker();
            lblPlecare = new Label();
            dtpPlecare = new DateTimePicker();
            lblSosire = new Label();
            dtpSosire = new DateTimePicker();
            lblPret = new Label();
            numPret = new NumericUpDown();
            btnSalveaza = new Controale.ButonIcon();
            pnlStare = new Panel();
            pnlLinie = new Panel();
            lblStareTitlu = new Label();
            btnPlecata = new Controale.ButonIcon();
            btnFinalizata = new Controale.ButonIcon();
            btnBilete = new Controale.ButonIcon();
            btnAnuleaza = new Controale.ButonIcon();
            pnlSpatiu = new Panel();
            pnlFiltre = new Panel();
            lblDeLa = new Label();
            dtpDeLa = new DateTimePicker();
            lblPanaLa = new Label();
            dtpPanaLa = new DateTimePicker();
            lblFiltruTraseu = new Label();
            cmbFiltruTraseu = new ComboBox();
            btnAfiseaza = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlEditare.SuspendLayout();
            pnlCampuri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPret).BeginInit();
            pnlStare.SuspendLayout();
            pnlLinie.SuspendLayout();
            pnlSpatiu.SuspendLayout();
            pnlFiltre.SuspendLayout();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colData, colPlecare, colTraseu, colAutobuz, colPret, colLibere, colStatus });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            // 
            // colData
            // 
            colData.DataPropertyName = "DataCursa";
            colData.HeaderText = "Data";
            colData.Name = "colData";
            colData.ReadOnly = true;
            colData.Width = 82;
            // 
            // colPlecare
            // 
            colPlecare.DataPropertyName = "OraPlecare";
            colPlecare.HeaderText = "Ora";
            colPlecare.Name = "colPlecare";
            colPlecare.ReadOnly = true;
            colPlecare.Width = 50;
            // 
            // colTraseu
            // 
            colTraseu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTraseu.DataPropertyName = "Traseu";
            colTraseu.FillWeight = 100F;
            colTraseu.HeaderText = "Traseu";
            colTraseu.MinimumWidth = 60;
            colTraseu.Name = "colTraseu";
            colTraseu.ReadOnly = true;
            // 
            // colAutobuz
            // 
            colAutobuz.DataPropertyName = "NrInmatriculare";
            colAutobuz.HeaderText = "Autobuz";
            colAutobuz.Name = "colAutobuz";
            colAutobuz.ReadOnly = true;
            colAutobuz.Width = 72;
            // 
            // colPret
            // 
            colPret.DataPropertyName = "Pret";
            colPret.HeaderText = "Preț";
            colPret.Name = "colPret";
            colPret.ReadOnly = true;
            colPret.Width = 64;
            // 
            // colLibere
            // 
            colLibere.DataPropertyName = "LocuriLibere";
            colLibere.HeaderText = "Libere";
            colLibere.Name = "colLibere";
            colLibere.ReadOnly = true;
            colLibere.Width = 54;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 96;
            // 
            // pnlListaAntet
            // 
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(lblNumar);
            pnlListaAntet.Controls.Add(btnNou);
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
            lblLista.Text = "Curse";
            // 
            // lblNumar
            // 
            lblNumar.AutoSize = true;
            lblNumar.ForeColor = Color.FromArgb(107, 114, 128);
            lblNumar.Location = new Point(60, 19);
            lblNumar.Name = "lblNumar";
            lblNumar.Text = "";
            // 
            // btnNou
            // 
            btnNou.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNou.Icon = "plus";
            btnNou.Location = new Point(468, 10);
            btnNou.Name = "btnNou";
            btnNou.Size = new Size(120, 32);
            btnNou.Stil = Controale.StilButon.Primar;
            btnNou.TabIndex = 1;
            btnNou.Text = " Cursă nouă";
            btnNou.Click += btnNou_Click;
            // 
            // pnlSpatiuDreapta
            // 
            pnlSpatiuDreapta.Dock = DockStyle.Right;
            pnlSpatiuDreapta.Name = "pnlSpatiuDreapta";
            pnlSpatiuDreapta.Size = new Size(16, 588);
            // 
            // pnlEditare
            // 
            pnlEditare.Controls.Add(lblEditareTitlu);
            pnlEditare.Controls.Add(pnlCampuri);
            pnlEditare.Controls.Add(btnSalveaza);
            pnlEditare.Controls.Add(pnlStare);
            pnlEditare.BackColor = Color.White;
            pnlEditare.Dock = DockStyle.Right;
            pnlEditare.Name = "pnlEditare";
            pnlEditare.Size = new Size(380, 588);
            // 
            // lblEditareTitlu
            // 
            lblEditareTitlu.AutoSize = true;
            lblEditareTitlu.Font = new Font("Segoe UI Semibold", 11F);
            lblEditareTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblEditareTitlu.Location = new Point(20, 18);
            lblEditareTitlu.Name = "lblEditareTitlu";
            lblEditareTitlu.Text = "Cursă nouă";
            // 
            // pnlCampuri
            // 
            pnlCampuri.Controls.Add(lblTraseu);
            pnlCampuri.Controls.Add(cmbTraseu);
            pnlCampuri.Controls.Add(lblAutobuz);
            pnlCampuri.Controls.Add(cmbAutobuz);
            pnlCampuri.Controls.Add(lblSofer);
            pnlCampuri.Controls.Add(cmbSofer);
            pnlCampuri.Controls.Add(lblData);
            pnlCampuri.Controls.Add(dtpData);
            pnlCampuri.Controls.Add(lblPlecare);
            pnlCampuri.Controls.Add(dtpPlecare);
            pnlCampuri.Controls.Add(lblSosire);
            pnlCampuri.Controls.Add(dtpSosire);
            pnlCampuri.Controls.Add(lblPret);
            pnlCampuri.Controls.Add(numPret);
            pnlCampuri.Location = new Point(0, 44);
            pnlCampuri.Name = "pnlCampuri";
            pnlCampuri.Size = new Size(380, 284);
            // 
            // lblTraseu
            // 
            lblTraseu.AutoSize = true;
            lblTraseu.Location = new Point(20, 4);
            lblTraseu.Name = "lblTraseu";
            lblTraseu.Text = "Traseu";
            // 
            // cmbTraseu
            // 
            cmbTraseu.DisplayMember = "Text";
            cmbTraseu.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTraseu.Location = new Point(20, 24);
            cmbTraseu.Name = "cmbTraseu";
            cmbTraseu.Size = new Size(340, 23);
            cmbTraseu.TabIndex = 2;
            // 
            // lblAutobuz
            // 
            lblAutobuz.AutoSize = true;
            lblAutobuz.Location = new Point(20, 60);
            lblAutobuz.Name = "lblAutobuz";
            lblAutobuz.Text = "Autobuz";
            // 
            // cmbAutobuz
            // 
            cmbAutobuz.DisplayMember = "Text";
            cmbAutobuz.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAutobuz.Location = new Point(20, 80);
            cmbAutobuz.Name = "cmbAutobuz";
            cmbAutobuz.Size = new Size(340, 23);
            cmbAutobuz.TabIndex = 3;
            // 
            // lblSofer
            // 
            lblSofer.AutoSize = true;
            lblSofer.Location = new Point(20, 116);
            lblSofer.Name = "lblSofer";
            lblSofer.Text = "Șofer";
            // 
            // cmbSofer
            // 
            cmbSofer.DisplayMember = "Text";
            cmbSofer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSofer.Location = new Point(20, 136);
            cmbSofer.Name = "cmbSofer";
            cmbSofer.Size = new Size(340, 23);
            cmbSofer.TabIndex = 4;
            // 
            // lblData
            // 
            lblData.AutoSize = true;
            lblData.Location = new Point(20, 172);
            lblData.Name = "lblData";
            lblData.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(20, 192);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(124, 23);
            dtpData.TabIndex = 5;
            // 
            // lblPlecare
            // 
            lblPlecare.AutoSize = true;
            lblPlecare.Location = new Point(156, 172);
            lblPlecare.Name = "lblPlecare";
            lblPlecare.Text = "Plecare";
            // 
            // dtpPlecare
            // 
            dtpPlecare.CustomFormat = "HH:mm";
            dtpPlecare.Format = DateTimePickerFormat.Custom;
            dtpPlecare.Location = new Point(156, 192);
            dtpPlecare.Name = "dtpPlecare";
            dtpPlecare.ShowUpDown = true;
            dtpPlecare.Size = new Size(96, 23);
            dtpPlecare.TabIndex = 6;
            // 
            // lblSosire
            // 
            lblSosire.AutoSize = true;
            lblSosire.Location = new Point(264, 172);
            lblSosire.Name = "lblSosire";
            lblSosire.Text = "Sosire";
            // 
            // dtpSosire
            // 
            dtpSosire.CustomFormat = "HH:mm";
            dtpSosire.Format = DateTimePickerFormat.Custom;
            dtpSosire.Location = new Point(264, 192);
            dtpSosire.Name = "dtpSosire";
            dtpSosire.ShowUpDown = true;
            dtpSosire.Size = new Size(96, 23);
            dtpSosire.TabIndex = 7;
            // 
            // lblPret
            // 
            lblPret.AutoSize = true;
            lblPret.Location = new Point(20, 228);
            lblPret.Name = "lblPret";
            lblPret.Text = "Preț (MDL)";
            // 
            // numPret
            // 
            numPret.DecimalPlaces = 2;
            numPret.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            numPret.Location = new Point(20, 248);
            numPret.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numPret.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numPret.Name = "numPret";
            numPret.Size = new Size(124, 23);
            numPret.TabIndex = 8;
            // 
            // btnSalveaza
            // 
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 332);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(340, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 9;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // pnlStare
            // 
            pnlStare.Controls.Add(pnlLinie);
            pnlStare.Controls.Add(lblStareTitlu);
            pnlStare.Controls.Add(btnPlecata);
            pnlStare.Controls.Add(btnFinalizata);
            pnlStare.Controls.Add(btnBilete);
            pnlStare.Controls.Add(btnAnuleaza);
            pnlStare.Location = new Point(0, 382);
            pnlStare.Name = "pnlStare";
            pnlStare.Size = new Size(380, 124);
            // 
            // pnlLinie
            // 
            pnlLinie.BackColor = Color.FromArgb(217, 222, 229);
            pnlLinie.Location = new Point(20, 0);
            pnlLinie.Name = "pnlLinie";
            pnlLinie.Size = new Size(340, 1);
            // 
            // lblStareTitlu
            // 
            lblStareTitlu.AutoSize = true;
            lblStareTitlu.Font = new Font("Segoe UI Semibold", 10F);
            lblStareTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblStareTitlu.Location = new Point(20, 12);
            lblStareTitlu.Name = "lblStareTitlu";
            lblStareTitlu.Text = "Desfășurarea cursei";
            // 
            // btnPlecata
            // 
            btnPlecata.Icon = "play";
            btnPlecata.Location = new Point(20, 40);
            btnPlecata.Name = "btnPlecata";
            btnPlecata.Size = new Size(166, 34);
            btnPlecata.TabIndex = 10;
            btnPlecata.Text = " A plecat";
            btnPlecata.Click += btnPlecata_Click;
            // 
            // btnFinalizata
            // 
            btnFinalizata.Icon = "flag";
            btnFinalizata.Location = new Point(194, 40);
            btnFinalizata.Name = "btnFinalizata";
            btnFinalizata.Size = new Size(166, 34);
            btnFinalizata.TabIndex = 11;
            btnFinalizata.Text = " A ajuns";
            btnFinalizata.Click += btnFinalizata_Click;
            // 
            // btnBilete
            // 
            btnBilete.Icon = "users";
            btnBilete.Location = new Point(20, 82);
            btnBilete.Name = "btnBilete";
            btnBilete.Size = new Size(166, 34);
            btnBilete.TabIndex = 12;
            btnBilete.Text = " Pasagerii";
            btnBilete.Click += btnBilete_Click;
            // 
            // btnAnuleaza
            // 
            btnAnuleaza.Icon = "ban";
            btnAnuleaza.Location = new Point(194, 82);
            btnAnuleaza.Name = "btnAnuleaza";
            btnAnuleaza.Size = new Size(166, 34);
            btnAnuleaza.Stil = Controale.StilButon.Pericol;
            btnAnuleaza.TabIndex = 13;
            btnAnuleaza.Text = " Anulează…";
            btnAnuleaza.Click += btnAnuleaza_Click;
            // 
            // pnlSpatiu
            // 
            pnlSpatiu.Dock = DockStyle.Top;
            pnlSpatiu.Name = "pnlSpatiu";
            pnlSpatiu.Size = new Size(1008, 16);
            // 
            // pnlFiltre
            // 
            pnlFiltre.Controls.Add(lblDeLa);
            pnlFiltre.Controls.Add(dtpDeLa);
            pnlFiltre.Controls.Add(lblPanaLa);
            pnlFiltre.Controls.Add(dtpPanaLa);
            pnlFiltre.Controls.Add(lblFiltruTraseu);
            pnlFiltre.Controls.Add(cmbFiltruTraseu);
            pnlFiltre.Controls.Add(btnAfiseaza);
            pnlFiltre.BackColor = Color.White;
            pnlFiltre.Dock = DockStyle.Top;
            pnlFiltre.Name = "pnlFiltre";
            pnlFiltre.Size = new Size(1008, 76);
            // 
            // lblDeLa
            // 
            lblDeLa.AutoSize = true;
            lblDeLa.Location = new Point(16, 10);
            lblDeLa.Name = "lblDeLa";
            lblDeLa.Text = "De la";
            // 
            // dtpDeLa
            // 
            dtpDeLa.Format = DateTimePickerFormat.Short;
            dtpDeLa.Location = new Point(16, 32);
            dtpDeLa.Name = "dtpDeLa";
            dtpDeLa.Size = new Size(130, 23);
            dtpDeLa.TabIndex = 14;
            // 
            // lblPanaLa
            // 
            lblPanaLa.AutoSize = true;
            lblPanaLa.Location = new Point(158, 10);
            lblPanaLa.Name = "lblPanaLa";
            lblPanaLa.Text = "Până la";
            // 
            // dtpPanaLa
            // 
            dtpPanaLa.Format = DateTimePickerFormat.Short;
            dtpPanaLa.Location = new Point(158, 32);
            dtpPanaLa.Name = "dtpPanaLa";
            dtpPanaLa.Size = new Size(130, 23);
            dtpPanaLa.TabIndex = 15;
            // 
            // lblFiltruTraseu
            // 
            lblFiltruTraseu.AutoSize = true;
            lblFiltruTraseu.Location = new Point(300, 10);
            lblFiltruTraseu.Name = "lblFiltruTraseu";
            lblFiltruTraseu.Text = "Traseu";
            // 
            // cmbFiltruTraseu
            // 
            cmbFiltruTraseu.DisplayMember = "Text";
            cmbFiltruTraseu.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltruTraseu.Location = new Point(300, 32);
            cmbFiltruTraseu.Name = "cmbFiltruTraseu";
            cmbFiltruTraseu.Size = new Size(280, 23);
            cmbFiltruTraseu.TabIndex = 16;
            // 
            // btnAfiseaza
            // 
            btnAfiseaza.Icon = "search";
            btnAfiseaza.Location = new Point(596, 28);
            btnAfiseaza.Name = "btnAfiseaza";
            btnAfiseaza.Size = new Size(116, 32);
            btnAfiseaza.Stil = Controale.StilButon.Primar;
            btnAfiseaza.TabIndex = 17;
            btnAfiseaza.Text = " Afișează";
            btnAfiseaza.Click += btnAfiseaza_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmAdminCurse
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Controls.Add(pnlSpatiu);
            Controls.Add(pnlFiltre);
            Name = "FrmAdminCurse";
            Text = "Curse";
            Load += FrmAdminCurse_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            pnlFiltre.ResumeLayout(false);
            pnlFiltre.PerformLayout();
            pnlSpatiu.ResumeLayout(false);
            pnlLinie.ResumeLayout(false);
            pnlStare.ResumeLayout(false);
            pnlStare.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPret).EndInit();
            pnlCampuri.ResumeLayout(false);
            pnlCampuri.PerformLayout();
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
        private DataGridViewTextBoxColumn colData;
        private DataGridViewTextBoxColumn colPlecare;
        private DataGridViewTextBoxColumn colTraseu;
        private DataGridViewTextBoxColumn colAutobuz;
        private DataGridViewTextBoxColumn colPret;
        private DataGridViewTextBoxColumn colLibere;
        private DataGridViewTextBoxColumn colStatus;
        private Panel pnlListaAntet;
        private Label lblLista;
        private Label lblNumar;
        private Controale.ButonIcon btnNou;
        private Panel pnlSpatiuDreapta;
        private Panel pnlEditare;
        private Label lblEditareTitlu;
        private Panel pnlCampuri;
        private Label lblTraseu;
        private ComboBox cmbTraseu;
        private Label lblAutobuz;
        private ComboBox cmbAutobuz;
        private Label lblSofer;
        private ComboBox cmbSofer;
        private Label lblData;
        private DateTimePicker dtpData;
        private Label lblPlecare;
        private DateTimePicker dtpPlecare;
        private Label lblSosire;
        private DateTimePicker dtpSosire;
        private Label lblPret;
        private NumericUpDown numPret;
        private Controale.ButonIcon btnSalveaza;
        private Panel pnlStare;
        private Panel pnlLinie;
        private Label lblStareTitlu;
        private Controale.ButonIcon btnPlecata;
        private Controale.ButonIcon btnFinalizata;
        private Controale.ButonIcon btnBilete;
        private Controale.ButonIcon btnAnuleaza;
        private Panel pnlSpatiu;
        private Panel pnlFiltre;
        private Label lblDeLa;
        private DateTimePicker dtpDeLa;
        private Label lblPanaLa;
        private DateTimePicker dtpPanaLa;
        private Label lblFiltruTraseu;
        private ComboBox cmbFiltruTraseu;
        private Controale.ButonIcon btnAfiseaza;
        private ErrorProvider errorProvider;
    }
}
