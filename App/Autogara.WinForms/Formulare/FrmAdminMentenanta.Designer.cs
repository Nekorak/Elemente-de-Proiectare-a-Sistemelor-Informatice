namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminMentenanta
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
            tabLista = new TabControl();
            tpLucrari = new TabPage();
            gridLucrari = new Controale.GridAutogara();
            colData = new DataGridViewTextBoxColumn();
            colAutobuz = new DataGridViewTextBoxColumn();
            colLucrare = new DataGridViewTextBoxColumn();
            colKm = new DataGridViewTextBoxColumn();
            colObservatii = new DataGridViewTextBoxColumn();
            tpItp = new TabPage();
            gridItp = new Controale.GridAutogara();
            colItpAutobuz = new DataGridViewTextBoxColumn();
            colItpModel = new DataGridViewTextBoxColumn();
            colItpStatus = new DataGridViewTextBoxColumn();
            colItpData = new DataGridViewTextBoxColumn();
            colItpZile = new DataGridViewTextBoxColumn();
            lblItpNota = new Label();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            cmbFiltru = new ComboBox();
            btnReincarca = new Controale.ButonIcon();
            pnlSpatiuDreapta = new Panel();
            pnlEditare = new Panel();
            lblEditareTitlu = new Label();
            lblAutobuz = new Label();
            cmbAutobuz = new ComboBox();
            lblTipLucrare = new Label();
            txtTipLucrare = new TextBox();
            lblData = new Label();
            dtpData = new DateTimePicker();
            chkKm = new CheckBox();
            numKm = new NumericUpDown();
            lblObservatii = new Label();
            txtObservatii = new TextBox();
            btnAdauga = new Controale.ButonIcon();
            pnlLinie = new Panel();
            lblItpTitlu = new Label();
            lblItpInspectie = new Label();
            dtpItpInspectie = new DateTimePicker();
            lblItpExpirare = new Label();
            dtpItpExpirare = new DateTimePicker();
            btnItp = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlLista.SuspendLayout();
            tabLista.SuspendLayout();
            tpLucrari.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLucrari).BeginInit();
            tpItp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridItp).BeginInit();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlEditare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numKm).BeginInit();
            pnlLinie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlLista
            // 
            pnlLista.Controls.Add(tabLista);
            pnlLista.Controls.Add(pnlListaAntet);
            pnlLista.BackColor = Color.White;
            pnlLista.Dock = DockStyle.Fill;
            pnlLista.Name = "pnlLista";
            pnlLista.Padding = new Padding(12, 0, 12, 12);
            // 
            // tabLista
            // 
            tabLista.Controls.Add(tpLucrari);
            tabLista.Controls.Add(tpItp);
            tabLista.Dock = DockStyle.Fill;
            tabLista.Name = "tabLista";
            tabLista.TabIndex = 0;
            // 
            // tpLucrari
            // 
            tpLucrari.Controls.Add(gridLucrari);
            tpLucrari.BackColor = Color.White;
            tpLucrari.Name = "tpLucrari";
            tpLucrari.Padding = new Padding(0, 8, 0, 0);
            tpLucrari.Text = "Lucrări";
            tpLucrari.UseVisualStyleBackColor = false;
            // 
            // gridLucrari
            // 
            gridLucrari.Columns.AddRange(new DataGridViewColumn[] { colData, colAutobuz, colLucrare, colKm, colObservatii });
            gridLucrari.Dock = DockStyle.Fill;
            gridLucrari.Name = "gridLucrari";
            gridLucrari.TabIndex = 1;
            // 
            // colData
            // 
            colData.DataPropertyName = "Data";
            colData.HeaderText = "Data";
            colData.Name = "colData";
            colData.ReadOnly = true;
            colData.Width = 90;
            // 
            // colAutobuz
            // 
            colAutobuz.DataPropertyName = "NrInmatriculare";
            colAutobuz.HeaderText = "Autobuz";
            colAutobuz.Name = "colAutobuz";
            colAutobuz.ReadOnly = true;
            colAutobuz.Width = 90;
            // 
            // colLucrare
            // 
            colLucrare.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colLucrare.DataPropertyName = "TipLucrare";
            colLucrare.FillWeight = 150F;
            colLucrare.HeaderText = "Lucrarea";
            colLucrare.MinimumWidth = 60;
            colLucrare.Name = "colLucrare";
            colLucrare.ReadOnly = true;
            // 
            // colKm
            // 
            colKm.DataPropertyName = "Kilometraj";
            colKm.HeaderText = "Km";
            colKm.Name = "colKm";
            colKm.ReadOnly = true;
            colKm.Width = 80;
            // 
            // colObservatii
            // 
            colObservatii.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colObservatii.DataPropertyName = "Observatii";
            colObservatii.FillWeight = 150F;
            colObservatii.HeaderText = "Observații";
            colObservatii.MinimumWidth = 60;
            colObservatii.Name = "colObservatii";
            colObservatii.ReadOnly = true;
            // 
            // tpItp
            // 
            tpItp.Controls.Add(gridItp);
            tpItp.Controls.Add(lblItpNota);
            tpItp.BackColor = Color.White;
            tpItp.Name = "tpItp";
            tpItp.Padding = new Padding(0, 8, 0, 0);
            tpItp.Text = "ITP";
            tpItp.UseVisualStyleBackColor = false;
            // 
            // gridItp
            // 
            gridItp.Columns.AddRange(new DataGridViewColumn[] { colItpAutobuz, colItpModel, colItpStatus, colItpData, colItpZile });
            gridItp.Dock = DockStyle.Fill;
            gridItp.Name = "gridItp";
            gridItp.TabIndex = 2;
            gridItp.SelectionChanged += gridItp_SelectionChanged;
            // 
            // colItpAutobuz
            // 
            colItpAutobuz.DataPropertyName = "NrInmatriculare";
            colItpAutobuz.HeaderText = "Autobuz";
            colItpAutobuz.Name = "colItpAutobuz";
            colItpAutobuz.ReadOnly = true;
            colItpAutobuz.Width = 100;
            // 
            // colItpModel
            // 
            colItpModel.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colItpModel.DataPropertyName = "Model";
            colItpModel.FillWeight = 150F;
            colItpModel.HeaderText = "Model";
            colItpModel.MinimumWidth = 60;
            colItpModel.Name = "colItpModel";
            colItpModel.ReadOnly = true;
            // 
            // colItpStatus
            // 
            colItpStatus.DataPropertyName = "Status";
            colItpStatus.HeaderText = "Status";
            colItpStatus.Name = "colItpStatus";
            colItpStatus.ReadOnly = true;
            colItpStatus.Width = 100;
            // 
            // colItpData
            // 
            colItpData.DataPropertyName = "DataExpirareITP";
            colItpData.HeaderText = "ITP până la";
            colItpData.Name = "colItpData";
            colItpData.ReadOnly = true;
            colItpData.Width = 100;
            // 
            // colItpZile
            // 
            colItpZile.DataPropertyName = "ZileRamase";
            colItpZile.HeaderText = "Zile rămase";
            colItpZile.Name = "colItpZile";
            colItpZile.ReadOnly = true;
            colItpZile.Width = 90;
            // 
            // lblItpNota
            // 
            lblItpNota.Dock = DockStyle.Bottom;
            lblItpNota.ForeColor = Color.FromArgb(107, 114, 128);
            lblItpNota.Name = "lblItpNota";
            lblItpNota.Size = new Size(560, 32);
            lblItpNota.Text = "Autobuzele active cu ITP-ul expirat sau care expiră în următoarele 30 de zile (zile negative = expirat).";
            lblItpNota.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlListaAntet
            // 
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(cmbFiltru);
            pnlListaAntet.Controls.Add(btnReincarca);
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
            lblLista.Text = "Istoric";
            // 
            // cmbFiltru
            // 
            cmbFiltru.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbFiltru.DisplayMember = "Text";
            cmbFiltru.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltru.Location = new Point(300, 14);
            cmbFiltru.Name = "cmbFiltru";
            cmbFiltru.Size = new Size(244, 23);
            cmbFiltru.TabIndex = 3;
            cmbFiltru.SelectedIndexChanged += cmbFiltru_SelectedIndexChanged;
            // 
            // btnReincarca
            // 
            btnReincarca.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReincarca.Icon = "refresh-cw";
            btnReincarca.Location = new Point(552, 10);
            btnReincarca.Name = "btnReincarca";
            btnReincarca.Size = new Size(36, 32);
            btnReincarca.TabIndex = 4;
            btnReincarca.Click += btnReincarca_Click;
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
            pnlEditare.Controls.Add(lblAutobuz);
            pnlEditare.Controls.Add(cmbAutobuz);
            pnlEditare.Controls.Add(lblTipLucrare);
            pnlEditare.Controls.Add(txtTipLucrare);
            pnlEditare.Controls.Add(lblData);
            pnlEditare.Controls.Add(dtpData);
            pnlEditare.Controls.Add(chkKm);
            pnlEditare.Controls.Add(numKm);
            pnlEditare.Controls.Add(lblObservatii);
            pnlEditare.Controls.Add(txtObservatii);
            pnlEditare.Controls.Add(btnAdauga);
            pnlEditare.Controls.Add(pnlLinie);
            pnlEditare.Controls.Add(lblItpTitlu);
            pnlEditare.Controls.Add(lblItpInspectie);
            pnlEditare.Controls.Add(dtpItpInspectie);
            pnlEditare.Controls.Add(lblItpExpirare);
            pnlEditare.Controls.Add(dtpItpExpirare);
            pnlEditare.Controls.Add(btnItp);
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
            lblEditareTitlu.Text = "Lucrare nouă";
            // 
            // lblAutobuz
            // 
            lblAutobuz.AutoSize = true;
            lblAutobuz.Location = new Point(20, 52);
            lblAutobuz.Name = "lblAutobuz";
            lblAutobuz.Text = "Autobuz";
            // 
            // cmbAutobuz
            // 
            cmbAutobuz.DisplayMember = "Text";
            cmbAutobuz.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAutobuz.Location = new Point(20, 72);
            cmbAutobuz.Name = "cmbAutobuz";
            cmbAutobuz.Size = new Size(340, 23);
            cmbAutobuz.TabIndex = 5;
            // 
            // lblTipLucrare
            // 
            lblTipLucrare.AutoSize = true;
            lblTipLucrare.Location = new Point(20, 108);
            lblTipLucrare.Name = "lblTipLucrare";
            lblTipLucrare.Text = "Lucrarea efectuată";
            // 
            // txtTipLucrare
            // 
            txtTipLucrare.Location = new Point(20, 128);
            txtTipLucrare.Name = "txtTipLucrare";
            txtTipLucrare.Size = new Size(340, 23);
            txtTipLucrare.TabIndex = 6;
            // 
            // lblData
            // 
            lblData.AutoSize = true;
            lblData.Location = new Point(20, 164);
            lblData.Name = "lblData";
            lblData.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(20, 184);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(150, 23);
            dtpData.TabIndex = 7;
            // 
            // chkKm
            // 
            chkKm.AutoSize = true;
            chkKm.CheckState = CheckState.Checked;
            chkKm.Checked = true;
            chkKm.Location = new Point(190, 162);
            chkKm.Name = "chkKm";
            chkKm.TabIndex = 8;
            chkKm.Text = "Kilometraj";
            chkKm.CheckedChanged += chkKm_CheckedChanged;
            // 
            // numKm
            // 
            numKm.Location = new Point(190, 184);
            numKm.Maximum = new decimal(new int[] { 5000000, 0, 0, 0 });
            numKm.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numKm.Name = "numKm";
            numKm.Size = new Size(170, 23);
            numKm.TabIndex = 9;
            numKm.ThousandsSeparator = true;
            // 
            // lblObservatii
            // 
            lblObservatii.AutoSize = true;
            lblObservatii.Location = new Point(20, 220);
            lblObservatii.Name = "lblObservatii";
            lblObservatii.Text = "Observații (opțional)";
            // 
            // txtObservatii
            // 
            txtObservatii.Location = new Point(20, 240);
            txtObservatii.Multiline = true;
            txtObservatii.Name = "txtObservatii";
            txtObservatii.Size = new Size(340, 56);
            txtObservatii.TabIndex = 10;
            // 
            // btnAdauga
            // 
            btnAdauga.Icon = "plus";
            btnAdauga.Location = new Point(20, 306);
            btnAdauga.Name = "btnAdauga";
            btnAdauga.Size = new Size(340, 36);
            btnAdauga.Stil = Controale.StilButon.Primar;
            btnAdauga.TabIndex = 11;
            btnAdauga.Text = " Adaugă lucrarea";
            btnAdauga.Click += btnAdauga_Click;
            // 
            // pnlLinie
            // 
            pnlLinie.BackColor = Color.FromArgb(217, 222, 229);
            pnlLinie.Location = new Point(20, 358);
            pnlLinie.Name = "pnlLinie";
            pnlLinie.Size = new Size(340, 1);
            // 
            // lblItpTitlu
            // 
            lblItpTitlu.AutoSize = true;
            lblItpTitlu.Font = new Font("Segoe UI Semibold", 10F);
            lblItpTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblItpTitlu.Location = new Point(20, 370);
            lblItpTitlu.Name = "lblItpTitlu";
            lblItpTitlu.Text = "ITP nou (pentru autobuzul ales)";
            // 
            // lblItpInspectie
            // 
            lblItpInspectie.AutoSize = true;
            lblItpInspectie.Location = new Point(20, 398);
            lblItpInspectie.Name = "lblItpInspectie";
            lblItpInspectie.Text = "Data inspecției";
            // 
            // dtpItpInspectie
            // 
            dtpItpInspectie.Format = DateTimePickerFormat.Short;
            dtpItpInspectie.Location = new Point(20, 418);
            dtpItpInspectie.Name = "dtpItpInspectie";
            dtpItpInspectie.Size = new Size(160, 23);
            dtpItpInspectie.TabIndex = 12;
            // 
            // lblItpExpirare
            // 
            lblItpExpirare.AutoSize = true;
            lblItpExpirare.Location = new Point(200, 398);
            lblItpExpirare.Name = "lblItpExpirare";
            lblItpExpirare.Text = "Valabil până la";
            // 
            // dtpItpExpirare
            // 
            dtpItpExpirare.Format = DateTimePickerFormat.Short;
            dtpItpExpirare.Location = new Point(200, 418);
            dtpItpExpirare.Name = "dtpItpExpirare";
            dtpItpExpirare.Size = new Size(160, 23);
            dtpItpExpirare.TabIndex = 13;
            // 
            // btnItp
            // 
            btnItp.Icon = "shield-check";
            btnItp.Location = new Point(20, 456);
            btnItp.Name = "btnItp";
            btnItp.Size = new Size(340, 36);
            btnItp.TabIndex = 14;
            btnItp.Text = " Înregistrează ITP";
            btnItp.Click += btnItp_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmAdminMentenanta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Name = "FrmAdminMentenanta";
            Text = "Mentenanță";
            Load += FrmAdminMentenanta_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            pnlLinie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numKm).EndInit();
            pnlEditare.ResumeLayout(false);
            pnlEditare.PerformLayout();
            pnlSpatiuDreapta.ResumeLayout(false);
            pnlListaAntet.ResumeLayout(false);
            pnlListaAntet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridItp).EndInit();
            tpItp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridLucrari).EndInit();
            tpLucrari.ResumeLayout(false);
            tabLista.ResumeLayout(false);
            pnlLista.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLista;
        private TabControl tabLista;
        private TabPage tpLucrari;
        private Controale.GridAutogara gridLucrari;
        private DataGridViewTextBoxColumn colData;
        private DataGridViewTextBoxColumn colAutobuz;
        private DataGridViewTextBoxColumn colLucrare;
        private DataGridViewTextBoxColumn colKm;
        private DataGridViewTextBoxColumn colObservatii;
        private TabPage tpItp;
        private Controale.GridAutogara gridItp;
        private DataGridViewTextBoxColumn colItpAutobuz;
        private DataGridViewTextBoxColumn colItpModel;
        private DataGridViewTextBoxColumn colItpStatus;
        private DataGridViewTextBoxColumn colItpData;
        private DataGridViewTextBoxColumn colItpZile;
        private Label lblItpNota;
        private Panel pnlListaAntet;
        private Label lblLista;
        private ComboBox cmbFiltru;
        private Controale.ButonIcon btnReincarca;
        private Panel pnlSpatiuDreapta;
        private Panel pnlEditare;
        private Label lblEditareTitlu;
        private Label lblAutobuz;
        private ComboBox cmbAutobuz;
        private Label lblTipLucrare;
        private TextBox txtTipLucrare;
        private Label lblData;
        private DateTimePicker dtpData;
        private CheckBox chkKm;
        private NumericUpDown numKm;
        private Label lblObservatii;
        private TextBox txtObservatii;
        private Controale.ButonIcon btnAdauga;
        private Panel pnlLinie;
        private Label lblItpTitlu;
        private Label lblItpInspectie;
        private DateTimePicker dtpItpInspectie;
        private Label lblItpExpirare;
        private DateTimePicker dtpItpExpirare;
        private Controale.ButonIcon btnItp;
        private ErrorProvider errorProvider;
    }
}
