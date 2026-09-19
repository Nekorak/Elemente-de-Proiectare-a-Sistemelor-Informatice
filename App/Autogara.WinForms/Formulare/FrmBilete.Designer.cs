namespace Autogara.WinForms.Formulare
{
    partial class FrmBilete
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
            gridBilete = new Controale.GridAutogara();
            colCod = new DataGridViewTextBoxColumn();
            colPasager = new DataGridViewTextBoxColumn();
            colTraseu = new DataGridViewTextBoxColumn();
            colData = new DataGridViewTextBoxColumn();
            colOra = new DataGridViewTextBoxColumn();
            colLoc = new DataGridViewTextBoxColumn();
            colPret = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            btnReincarca = new Controale.ButonIcon();
            pnlSpatiuDreapta = new Panel();
            pnlDetalii = new Panel();
            lblDetaliiTitlu = new Label();
            lblFaraBilet = new Label();
            pnlDetaliiContinut = new Panel();
            lblCodBilet = new Label();
            lblStatus = new Label();
            lblDetalii = new Label();
            pnlLinie = new Panel();
            lblAnulareTitlu = new Label();
            lblEstimare = new Label();
            chkIntegral = new CheckBox();
            btnAnuleaza = new Controale.ButonIcon();
            pnlSpatiu = new Panel();
            pnlCautare = new Panel();
            lblCod = new Label();
            txtCod = new TextBox();
            btnCautaCod = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridBilete).BeginInit();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlDetalii.SuspendLayout();
            pnlDetaliiContinut.SuspendLayout();
            pnlLinie.SuspendLayout();
            pnlSpatiu.SuspendLayout();
            pnlCautare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlLista
            // 
            pnlLista.Controls.Add(gridBilete);
            pnlLista.Controls.Add(pnlListaAntet);
            pnlLista.BackColor = Color.FromArgb(250, 249, 246);
            pnlLista.Dock = DockStyle.Fill;
            pnlLista.Name = "pnlLista";
            pnlLista.Padding = new Padding(12, 0, 12, 12);
            // 
            // gridBilete
            // 
            gridBilete.Columns.AddRange(new DataGridViewColumn[] { colCod, colPasager, colTraseu, colData, colOra, colLoc, colPret, colStatus });
            gridBilete.Dock = DockStyle.Fill;
            gridBilete.Name = "gridBilete";
            gridBilete.TabIndex = 0;
            gridBilete.SelectionChanged += gridBilete_SelectionChanged;
            // 
            // colCod
            // 
            colCod.DataPropertyName = "CodBilet";
            colCod.HeaderText = "Cod";
            colCod.Name = "colCod";
            colCod.ReadOnly = true;
            colCod.Width = 130;
            // 
            // colPasager
            // 
            colPasager.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPasager.DataPropertyName = "NumePasager";
            colPasager.FillWeight = 120F;
            colPasager.HeaderText = "Pasager";
            colPasager.MinimumWidth = 60;
            colPasager.Name = "colPasager";
            colPasager.ReadOnly = true;
            // 
            // colTraseu
            // 
            colTraseu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTraseu.DataPropertyName = "Traseu";
            colTraseu.FillWeight = 150F;
            colTraseu.HeaderText = "Traseu";
            colTraseu.MinimumWidth = 60;
            colTraseu.Name = "colTraseu";
            colTraseu.ReadOnly = true;
            // 
            // colData
            // 
            colData.DataPropertyName = "DataCursa";
            colData.HeaderText = "Data";
            colData.Name = "colData";
            colData.ReadOnly = true;
            colData.Width = 86;
            // 
            // colOra
            // 
            colOra.DataPropertyName = "OraPlecare";
            colOra.HeaderText = "Ora";
            colOra.Name = "colOra";
            colOra.ReadOnly = true;
            colOra.Width = 56;
            // 
            // colLoc
            // 
            colLoc.DataPropertyName = "NumarLoc";
            colLoc.HeaderText = "Loc";
            colLoc.Name = "colLoc";
            colLoc.ReadOnly = true;
            colLoc.Width = 48;
            // 
            // colPret
            // 
            colPret.DataPropertyName = "Pret";
            colPret.HeaderText = "Preț";
            colPret.Name = "colPret";
            colPret.ReadOnly = true;
            colPret.Width = 76;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 86;
            // 
            // pnlListaAntet
            // 
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(btnReincarca);
            pnlListaAntet.Dock = DockStyle.Top;
            pnlListaAntet.Name = "pnlListaAntet";
            pnlListaAntet.Size = new Size(600, 52);
            // 
            // lblLista
            // 
            lblLista.AutoSize = true;
            lblLista.Font = new Font("Bahnschrift SemiBold", 11F);
            lblLista.ForeColor = Color.FromArgb(34, 34, 31);
            lblLista.Location = new Point(4, 16);
            lblLista.Name = "lblLista";
            lblLista.Text = "Bilete";
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
            // pnlSpatiuDreapta
            // 
            pnlSpatiuDreapta.Dock = DockStyle.Right;
            pnlSpatiuDreapta.Name = "pnlSpatiuDreapta";
            pnlSpatiuDreapta.Size = new Size(16, 588);
            // 
            // pnlDetalii
            // 
            pnlDetalii.Controls.Add(lblDetaliiTitlu);
            pnlDetalii.Controls.Add(lblFaraBilet);
            pnlDetalii.Controls.Add(pnlDetaliiContinut);
            pnlDetalii.BackColor = Color.FromArgb(250, 249, 246);
            pnlDetalii.Dock = DockStyle.Right;
            pnlDetalii.Name = "pnlDetalii";
            pnlDetalii.Size = new Size(392, 588);
            // 
            // lblDetaliiTitlu
            // 
            lblDetaliiTitlu.AutoSize = true;
            lblDetaliiTitlu.Font = new Font("Bahnschrift SemiBold", 11F);
            lblDetaliiTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblDetaliiTitlu.Location = new Point(20, 18);
            lblDetaliiTitlu.Name = "lblDetaliiTitlu";
            lblDetaliiTitlu.Text = "Detaliile biletului";
            // 
            // lblFaraBilet
            // 
            lblFaraBilet.ForeColor = Color.FromArgb(110, 106, 96);
            lblFaraBilet.Location = new Point(20, 52);
            lblFaraBilet.Name = "lblFaraBilet";
            lblFaraBilet.Size = new Size(352, 60);
            lblFaraBilet.Text = "Căutați un bilet după cod sau alegeți unul din listă.";
            // 
            // pnlDetaliiContinut
            // 
            pnlDetaliiContinut.Controls.Add(lblCodBilet);
            pnlDetaliiContinut.Controls.Add(lblStatus);
            pnlDetaliiContinut.Controls.Add(lblDetalii);
            pnlDetaliiContinut.Controls.Add(pnlLinie);
            pnlDetaliiContinut.Controls.Add(lblAnulareTitlu);
            pnlDetaliiContinut.Controls.Add(lblEstimare);
            pnlDetaliiContinut.Controls.Add(chkIntegral);
            pnlDetaliiContinut.Controls.Add(btnAnuleaza);
            pnlDetaliiContinut.Location = new Point(0, 48);
            pnlDetaliiContinut.Name = "pnlDetaliiContinut";
            pnlDetaliiContinut.Size = new Size(392, 520);
            // 
            // lblCodBilet
            // 
            lblCodBilet.AutoSize = true;
            lblCodBilet.Font = new Font("Bahnschrift SemiBold", 15F);
            lblCodBilet.ForeColor = Color.FromArgb(34, 34, 31);
            lblCodBilet.Location = new Point(18, 4);
            lblCodBilet.Name = "lblCodBilet";
            lblCodBilet.Text = "AG000000-00000000";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Bahnschrift SemiBold", 9.5F);
            lblStatus.Location = new Point(20, 38);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Activ";
            // 
            // lblDetalii
            // 
            lblDetalii.Location = new Point(20, 66);
            lblDetalii.Name = "lblDetalii";
            lblDetalii.Size = new Size(352, 160);
            lblDetalii.Text = "";
            // 
            // pnlLinie
            // 
            pnlLinie.BackColor = Color.FromArgb(212, 208, 198);
            pnlLinie.Location = new Point(20, 236);
            pnlLinie.Name = "pnlLinie";
            pnlLinie.Size = new Size(352, 1);
            // 
            // lblAnulareTitlu
            // 
            lblAnulareTitlu.AutoSize = true;
            lblAnulareTitlu.Font = new Font("Bahnschrift SemiBold", 10F);
            lblAnulareTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblAnulareTitlu.Location = new Point(20, 250);
            lblAnulareTitlu.Name = "lblAnulareTitlu";
            lblAnulareTitlu.Text = "Anulare";
            // 
            // lblEstimare
            // 
            lblEstimare.ForeColor = Color.FromArgb(110, 106, 96);
            lblEstimare.Location = new Point(20, 276);
            lblEstimare.Name = "lblEstimare";
            lblEstimare.Size = new Size(352, 56);
            lblEstimare.Text = "";
            // 
            // chkIntegral
            // 
            chkIntegral.AutoSize = true;
            chkIntegral.Location = new Point(20, 338);
            chkIntegral.Name = "chkIntegral";
            chkIntegral.TabIndex = 2;
            chkIntegral.Text = "Rambursare integrală (excepție aprobată de administrator)";
            chkIntegral.CheckedChanged += chkIntegral_CheckedChanged;
            // 
            // btnAnuleaza
            // 
            btnAnuleaza.Icon = "ticket-x";
            btnAnuleaza.Location = new Point(20, 368);
            btnAnuleaza.Name = "btnAnuleaza";
            btnAnuleaza.Size = new Size(352, 36);
            btnAnuleaza.Stil = Controale.StilButon.Pericol;
            btnAnuleaza.TabIndex = 3;
            btnAnuleaza.Text = " Anulează biletul";
            btnAnuleaza.Click += btnAnuleaza_Click;
            // 
            // pnlSpatiu
            // 
            pnlSpatiu.Dock = DockStyle.Top;
            pnlSpatiu.Name = "pnlSpatiu";
            pnlSpatiu.Size = new Size(1008, 16);
            // 
            // pnlCautare
            // 
            pnlCautare.Controls.Add(lblCod);
            pnlCautare.Controls.Add(txtCod);
            pnlCautare.Controls.Add(btnCautaCod);
            pnlCautare.BackColor = Color.FromArgb(250, 249, 246);
            pnlCautare.Dock = DockStyle.Top;
            pnlCautare.Name = "pnlCautare";
            pnlCautare.Size = new Size(1008, 76);
            // 
            // lblCod
            // 
            lblCod.AutoSize = true;
            lblCod.Location = new Point(16, 10);
            lblCod.Name = "lblCod";
            lblCod.Text = "Codul biletului";
            // 
            // txtCod
            // 
            txtCod.CharacterCasing = CharacterCasing.Upper;
            txtCod.Location = new Point(16, 32);
            txtCod.Name = "txtCod";
            txtCod.Size = new Size(240, 23);
            txtCod.TabIndex = 4;
            // 
            // btnCautaCod
            // 
            btnCautaCod.Icon = "search";
            btnCautaCod.Location = new Point(268, 28);
            btnCautaCod.Name = "btnCautaCod";
            btnCautaCod.Size = new Size(110, 32);
            btnCautaCod.Stil = Controale.StilButon.Primar;
            btnCautaCod.TabIndex = 5;
            btnCautaCod.Text = " Caută";
            btnCautaCod.Click += btnCautaCod_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmBilete
            // 
            AcceptButton = btnCautaCod;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlDetalii);
            Controls.Add(pnlSpatiu);
            Controls.Add(pnlCautare);
            Name = "FrmBilete";
            Text = "Bilete";
            Load += FrmBilete_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            pnlCautare.ResumeLayout(false);
            pnlCautare.PerformLayout();
            pnlSpatiu.ResumeLayout(false);
            pnlLinie.ResumeLayout(false);
            pnlDetaliiContinut.ResumeLayout(false);
            pnlDetaliiContinut.PerformLayout();
            pnlDetalii.ResumeLayout(false);
            pnlDetalii.PerformLayout();
            pnlSpatiuDreapta.ResumeLayout(false);
            pnlListaAntet.ResumeLayout(false);
            pnlListaAntet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridBilete).EndInit();
            pnlLista.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLista;
        private Controale.GridAutogara gridBilete;
        private DataGridViewTextBoxColumn colCod;
        private DataGridViewTextBoxColumn colPasager;
        private DataGridViewTextBoxColumn colTraseu;
        private DataGridViewTextBoxColumn colData;
        private DataGridViewTextBoxColumn colOra;
        private DataGridViewTextBoxColumn colLoc;
        private DataGridViewTextBoxColumn colPret;
        private DataGridViewTextBoxColumn colStatus;
        private Panel pnlListaAntet;
        private Label lblLista;
        private Controale.ButonIcon btnReincarca;
        private Panel pnlSpatiuDreapta;
        private Panel pnlDetalii;
        private Label lblDetaliiTitlu;
        private Label lblFaraBilet;
        private Panel pnlDetaliiContinut;
        private Label lblCodBilet;
        private Label lblStatus;
        private Label lblDetalii;
        private Panel pnlLinie;
        private Label lblAnulareTitlu;
        private Label lblEstimare;
        private CheckBox chkIntegral;
        private Controale.ButonIcon btnAnuleaza;
        private Panel pnlSpatiu;
        private Panel pnlCautare;
        private Label lblCod;
        private TextBox txtCod;
        private Controale.ButonIcon btnCautaCod;
        private ErrorProvider errorProvider;
    }
}
