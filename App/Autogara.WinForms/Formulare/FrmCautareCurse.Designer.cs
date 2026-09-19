namespace Autogara.WinForms.Formulare
{
    partial class FrmCautareCurse
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
            pnlRezultate = new Panel();
            gridCurse = new Controale.GridAutogara();
            colPlecare = new DataGridViewTextBoxColumn();
            colSosire = new DataGridViewTextBoxColumn();
            colTraseu = new DataGridViewTextBoxColumn();
            colUrcare = new DataGridViewTextBoxColumn();
            colPeron = new DataGridViewTextBoxColumn();
            colCoborare = new DataGridViewTextBoxColumn();
            colPret = new DataGridViewTextBoxColumn();
            colLibere = new DataGridViewTextBoxColumn();
            colAutobuz = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            pnlRezultateAntet = new Panel();
            lblRezultate = new Label();
            lblNumar = new Label();
            btnAlegeLocul = new Controale.ButonIcon();
            pnlSpatiu = new Panel();
            pnlFiltre = new Panel();
            lblPlecare = new Label();
            cmbPlecare = new ComboBox();
            lblSosire = new Label();
            cmbSosire = new ComboBox();
            lblData = new Label();
            dtpData = new DateTimePicker();
            btnCauta = new Controale.ButonIcon();
            pnlRezultate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCurse).BeginInit();
            pnlRezultateAntet.SuspendLayout();
            pnlSpatiu.SuspendLayout();
            pnlFiltre.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRezultate
            // 
            pnlRezultate.Controls.Add(gridCurse);
            pnlRezultate.Controls.Add(pnlRezultateAntet);
            pnlRezultate.BackColor = Color.FromArgb(250, 249, 246);
            pnlRezultate.Dock = DockStyle.Fill;
            pnlRezultate.Name = "pnlRezultate";
            pnlRezultate.Padding = new Padding(12, 0, 12, 12);
            // 
            // gridCurse
            // 
            gridCurse.Columns.AddRange(new DataGridViewColumn[] { colPlecare, colSosire, colTraseu, colUrcare, colPeron, colCoborare, colPret, colLibere, colAutobuz, colStatus });
            gridCurse.Dock = DockStyle.Fill;
            gridCurse.Name = "gridCurse";
            gridCurse.TabIndex = 0;
            gridCurse.CellDoubleClick += gridCurse_CellDoubleClick;
            // 
            // colPlecare
            // 
            colPlecare.DataPropertyName = "OraPlecare";
            colPlecare.HeaderText = "Plecare";
            colPlecare.Name = "colPlecare";
            colPlecare.ReadOnly = true;
            colPlecare.Width = 70;
            // 
            // colSosire
            // 
            colSosire.DataPropertyName = "OraSosireEstimata";
            colSosire.HeaderText = "Sosire";
            colSosire.Name = "colSosire";
            colSosire.ReadOnly = true;
            colSosire.Width = 70;
            // 
            // colTraseu
            // 
            colTraseu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTraseu.DataPropertyName = "Traseu";
            colTraseu.FillWeight = 170F;
            colTraseu.HeaderText = "Traseu";
            colTraseu.MinimumWidth = 60;
            colTraseu.Name = "colTraseu";
            colTraseu.ReadOnly = true;
            // 
            // colUrcare
            // 
            colUrcare.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colUrcare.DataPropertyName = "StatiePlecare";
            colUrcare.FillWeight = 110F;
            colUrcare.HeaderText = "Urcare";
            colUrcare.MinimumWidth = 60;
            colUrcare.Name = "colUrcare";
            colUrcare.ReadOnly = true;
            // 
            // colPeron
            // 
            colPeron.DataPropertyName = "PeronPlecare";
            colPeron.HeaderText = "Peron";
            colPeron.Name = "colPeron";
            colPeron.ReadOnly = true;
            colPeron.Width = 60;
            // 
            // colCoborare
            // 
            colCoborare.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colCoborare.DataPropertyName = "StatieSosire";
            colCoborare.FillWeight = 110F;
            colCoborare.HeaderText = "Coborâre";
            colCoborare.MinimumWidth = 60;
            colCoborare.Name = "colCoborare";
            colCoborare.ReadOnly = true;
            // 
            // colPret
            // 
            colPret.DataPropertyName = "Pret";
            colPret.HeaderText = "Preț (MDL)";
            colPret.Name = "colPret";
            colPret.ReadOnly = true;
            colPret.Width = 90;
            // 
            // colLibere
            // 
            colLibere.DataPropertyName = "LocuriLibere";
            colLibere.HeaderText = "Libere";
            colLibere.Name = "colLibere";
            colLibere.ReadOnly = true;
            colLibere.Width = 64;
            // 
            // colAutobuz
            // 
            colAutobuz.DataPropertyName = "NrInmatriculare";
            colAutobuz.HeaderText = "Autobuz";
            colAutobuz.Name = "colAutobuz";
            colAutobuz.ReadOnly = true;
            colAutobuz.Width = 90;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 110;
            // 
            // pnlRezultateAntet
            // 
            pnlRezultateAntet.Controls.Add(lblRezultate);
            pnlRezultateAntet.Controls.Add(lblNumar);
            pnlRezultateAntet.Controls.Add(btnAlegeLocul);
            pnlRezultateAntet.Dock = DockStyle.Top;
            pnlRezultateAntet.Name = "pnlRezultateAntet";
            pnlRezultateAntet.Size = new Size(1008, 52);
            // 
            // lblRezultate
            // 
            lblRezultate.AutoSize = true;
            lblRezultate.Font = new Font("Bahnschrift SemiBold", 11F);
            lblRezultate.ForeColor = Color.FromArgb(34, 34, 31);
            lblRezultate.Location = new Point(4, 16);
            lblRezultate.Name = "lblRezultate";
            lblRezultate.Text = "Curse găsite";
            // 
            // lblNumar
            // 
            lblNumar.AutoSize = true;
            lblNumar.ForeColor = Color.FromArgb(110, 106, 96);
            lblNumar.Location = new Point(112, 19);
            lblNumar.Name = "lblNumar";
            lblNumar.Text = "";
            // 
            // btnAlegeLocul
            // 
            btnAlegeLocul.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAlegeLocul.Icon = "armchair";
            btnAlegeLocul.Location = new Point(836, 10);
            btnAlegeLocul.Name = "btnAlegeLocul";
            btnAlegeLocul.Size = new Size(160, 32);
            btnAlegeLocul.Stil = Controale.StilButon.Primar;
            btnAlegeLocul.TabIndex = 1;
            btnAlegeLocul.Text = " Alege locul";
            btnAlegeLocul.Click += btnAlegeLocul_Click;
            // 
            // pnlSpatiu
            // 
            pnlSpatiu.Dock = DockStyle.Top;
            pnlSpatiu.Name = "pnlSpatiu";
            pnlSpatiu.Size = new Size(1008, 16);
            // 
            // pnlFiltre
            // 
            pnlFiltre.Controls.Add(lblPlecare);
            pnlFiltre.Controls.Add(cmbPlecare);
            pnlFiltre.Controls.Add(lblSosire);
            pnlFiltre.Controls.Add(cmbSosire);
            pnlFiltre.Controls.Add(lblData);
            pnlFiltre.Controls.Add(dtpData);
            pnlFiltre.Controls.Add(btnCauta);
            pnlFiltre.BackColor = Color.FromArgb(250, 249, 246);
            pnlFiltre.Dock = DockStyle.Top;
            pnlFiltre.Name = "pnlFiltre";
            pnlFiltre.Size = new Size(1008, 76);
            // 
            // lblPlecare
            // 
            lblPlecare.AutoSize = true;
            lblPlecare.Location = new Point(16, 10);
            lblPlecare.Name = "lblPlecare";
            lblPlecare.Text = "Din stația";
            // 
            // cmbPlecare
            // 
            cmbPlecare.DisplayMember = "Text";
            cmbPlecare.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlecare.Location = new Point(16, 32);
            cmbPlecare.Name = "cmbPlecare";
            cmbPlecare.Size = new Size(240, 23);
            cmbPlecare.TabIndex = 2;
            // 
            // lblSosire
            // 
            lblSosire.AutoSize = true;
            lblSosire.Location = new Point(272, 10);
            lblSosire.Name = "lblSosire";
            lblSosire.Text = "Spre stația";
            // 
            // cmbSosire
            // 
            cmbSosire.DisplayMember = "Text";
            cmbSosire.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSosire.Location = new Point(272, 32);
            cmbSosire.Name = "cmbSosire";
            cmbSosire.Size = new Size(240, 23);
            cmbSosire.TabIndex = 3;
            // 
            // lblData
            // 
            lblData.AutoSize = true;
            lblData.Location = new Point(528, 10);
            lblData.Name = "lblData";
            lblData.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(528, 32);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(130, 23);
            dtpData.TabIndex = 4;
            // 
            // btnCauta
            // 
            btnCauta.Icon = "search";
            btnCauta.Location = new Point(676, 28);
            btnCauta.Name = "btnCauta";
            btnCauta.Size = new Size(110, 32);
            btnCauta.Stil = Controale.StilButon.Primar;
            btnCauta.TabIndex = 5;
            btnCauta.Text = " Caută";
            btnCauta.Click += btnCauta_Click;
            // 
            // FrmCautareCurse
            // 
            AcceptButton = btnCauta;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlRezultate);
            Controls.Add(pnlSpatiu);
            Controls.Add(pnlFiltre);
            Name = "FrmCautareCurse";
            Text = "Vânzare bilete";
            Load += FrmCautareCurse_Load;
            pnlFiltre.ResumeLayout(false);
            pnlFiltre.PerformLayout();
            pnlSpatiu.ResumeLayout(false);
            pnlRezultateAntet.ResumeLayout(false);
            pnlRezultateAntet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridCurse).EndInit();
            pnlRezultate.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlRezultate;
        private Controale.GridAutogara gridCurse;
        private DataGridViewTextBoxColumn colPlecare;
        private DataGridViewTextBoxColumn colSosire;
        private DataGridViewTextBoxColumn colTraseu;
        private DataGridViewTextBoxColumn colUrcare;
        private DataGridViewTextBoxColumn colPeron;
        private DataGridViewTextBoxColumn colCoborare;
        private DataGridViewTextBoxColumn colPret;
        private DataGridViewTextBoxColumn colLibere;
        private DataGridViewTextBoxColumn colAutobuz;
        private DataGridViewTextBoxColumn colStatus;
        private Panel pnlRezultateAntet;
        private Label lblRezultate;
        private Label lblNumar;
        private Controale.ButonIcon btnAlegeLocul;
        private Panel pnlSpatiu;
        private Panel pnlFiltre;
        private Label lblPlecare;
        private ComboBox cmbPlecare;
        private Label lblSosire;
        private ComboBox cmbSosire;
        private Label lblData;
        private DateTimePicker dtpData;
        private Controale.ButonIcon btnCauta;
    }
}
