namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminAutobuze
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
            colNr = new DataGridViewTextBoxColumn();
            colModel = new DataGridViewTextBoxColumn();
            colLocuri = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colItp = new DataGridViewTextBoxColumn();
            colActiv = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            btnReincarca = new Controale.ButonIcon();
            btnNou = new Controale.ButonIcon();
            chkInactive = new CheckBox();
            pnlSpatiuDreapta = new Panel();
            pnlEditare = new Panel();
            lblEditareTitlu = new Label();
            lblNrInmatriculare = new Label();
            txtNrInmatriculare = new TextBox();
            lblModel = new Label();
            txtModel = new TextBox();
            lblCapacitate = new Label();
            numCapacitate = new NumericUpDown();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            chkItp = new CheckBox();
            dtpItp = new DateTimePicker();
            lblFisier = new Label();
            btnSalveaza = new Controale.ButonIcon();
            btnLocuri = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlEditare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCapacitate).BeginInit();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colNr, colModel, colLocuri, colStatus, colItp, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            // 
            // colNr
            // 
            colNr.DataPropertyName = "NrInmatriculare";
            colNr.HeaderText = "Nr. înmatriculare";
            colNr.Name = "colNr";
            colNr.ReadOnly = true;
            colNr.Width = 120;
            // 
            // colModel
            // 
            colModel.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colModel.DataPropertyName = "Model";
            colModel.FillWeight = 160F;
            colModel.HeaderText = "Model";
            colModel.MinimumWidth = 60;
            colModel.Name = "colModel";
            colModel.ReadOnly = true;
            // 
            // colLocuri
            // 
            colLocuri.DataPropertyName = "CapacitateLocuri";
            colLocuri.HeaderText = "Locuri";
            colLocuri.Name = "colLocuri";
            colLocuri.ReadOnly = true;
            colLocuri.Width = 60;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 100;
            // 
            // colItp
            // 
            colItp.DataPropertyName = "DataExpirareITP";
            colItp.HeaderText = "ITP până la";
            colItp.Name = "colItp";
            colItp.ReadOnly = true;
            colItp.Width = 96;
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
            lblLista.Text = "Autobuze";
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
            pnlEditare.Controls.Add(lblNrInmatriculare);
            pnlEditare.Controls.Add(txtNrInmatriculare);
            pnlEditare.Controls.Add(lblModel);
            pnlEditare.Controls.Add(txtModel);
            pnlEditare.Controls.Add(lblCapacitate);
            pnlEditare.Controls.Add(numCapacitate);
            pnlEditare.Controls.Add(lblStatus);
            pnlEditare.Controls.Add(cmbStatus);
            pnlEditare.Controls.Add(chkItp);
            pnlEditare.Controls.Add(dtpItp);
            pnlEditare.Controls.Add(lblFisier);
            pnlEditare.Controls.Add(btnSalveaza);
            pnlEditare.Controls.Add(btnLocuri);
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
            lblEditareTitlu.Text = "Autobuz nou";
            // 
            // lblNrInmatriculare
            // 
            lblNrInmatriculare.AutoSize = true;
            lblNrInmatriculare.Location = new Point(20, 56);
            lblNrInmatriculare.Name = "lblNrInmatriculare";
            lblNrInmatriculare.Text = "Număr de înmatriculare";
            // 
            // txtNrInmatriculare
            // 
            txtNrInmatriculare.CharacterCasing = CharacterCasing.Upper;
            txtNrInmatriculare.Location = new Point(20, 76);
            txtNrInmatriculare.Name = "txtNrInmatriculare";
            txtNrInmatriculare.Size = new Size(340, 23);
            txtNrInmatriculare.TabIndex = 4;
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.Location = new Point(20, 112);
            lblModel.Name = "lblModel";
            lblModel.Text = "Model";
            // 
            // txtModel
            // 
            txtModel.Location = new Point(20, 132);
            txtModel.Name = "txtModel";
            txtModel.Size = new Size(340, 23);
            txtModel.TabIndex = 5;
            // 
            // lblCapacitate
            // 
            lblCapacitate.AutoSize = true;
            lblCapacitate.Location = new Point(20, 168);
            lblCapacitate.Name = "lblCapacitate";
            lblCapacitate.Text = "Număr de locuri";
            // 
            // numCapacitate
            // 
            numCapacitate.Location = new Point(20, 188);
            numCapacitate.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            numCapacitate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numCapacitate.Name = "numCapacitate";
            numCapacitate.Size = new Size(100, 23);
            numCapacitate.TabIndex = 6;
            numCapacitate.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(20, 224);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Status";
            // 
            // cmbStatus
            // 
            cmbStatus.DisplayMember = "Text";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Location = new Point(20, 244);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(160, 23);
            cmbStatus.TabIndex = 7;
            // 
            // chkItp
            // 
            chkItp.AutoSize = true;
            chkItp.Location = new Point(20, 280);
            chkItp.Name = "chkItp";
            chkItp.TabIndex = 8;
            chkItp.Text = "ITP valabil până la";
            chkItp.CheckedChanged += chkItp_CheckedChanged;
            // 
            // dtpItp
            // 
            dtpItp.Format = DateTimePickerFormat.Short;
            dtpItp.Location = new Point(20, 304);
            dtpItp.Name = "dtpItp";
            dtpItp.Size = new Size(160, 23);
            dtpItp.TabIndex = 9;
            // 
            // lblFisier
            // 
            lblFisier.ForeColor = Color.FromArgb(107, 114, 128);
            lblFisier.Location = new Point(20, 344);
            lblFisier.Name = "lblFisier";
            lblFisier.Size = new Size(340, 40);
            lblFisier.Text = "";
            // 
            // btnSalveaza
            // 
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 392);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(166, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 10;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // btnLocuri
            // 
            btnLocuri.Icon = "armchair";
            btnLocuri.Location = new Point(194, 392);
            btnLocuri.Name = "btnLocuri";
            btnLocuri.Size = new Size(166, 36);
            btnLocuri.TabIndex = 11;
            btnLocuri.Text = " Editează locurile";
            btnLocuri.Click += btnLocuri_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmAdminAutobuze
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Name = "FrmAdminAutobuze";
            Text = "Autobuze";
            Load += FrmAdminAutobuze_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCapacitate).EndInit();
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
        private DataGridViewTextBoxColumn colNr;
        private DataGridViewTextBoxColumn colModel;
        private DataGridViewTextBoxColumn colLocuri;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colItp;
        private DataGridViewTextBoxColumn colActiv;
        private Panel pnlListaAntet;
        private Label lblLista;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnNou;
        private CheckBox chkInactive;
        private Panel pnlSpatiuDreapta;
        private Panel pnlEditare;
        private Label lblEditareTitlu;
        private Label lblNrInmatriculare;
        private TextBox txtNrInmatriculare;
        private Label lblModel;
        private TextBox txtModel;
        private Label lblCapacitate;
        private NumericUpDown numCapacitate;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private CheckBox chkItp;
        private DateTimePicker dtpItp;
        private Label lblFisier;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnLocuri;
        private ErrorProvider errorProvider;
    }
}
