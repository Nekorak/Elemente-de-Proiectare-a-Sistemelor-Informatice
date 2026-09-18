namespace Autogara.WinForms.Formulare
{
    partial class FrmAudit
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
            pnlRezultat = new Panel();
            gridAudit = new Controale.GridAutogara();
            colData = new DataGridViewTextBoxColumn();
            colUtilizator = new DataGridViewTextBoxColumn();
            colActiune = new DataGridViewTextBoxColumn();
            colEntitate = new DataGridViewTextBoxColumn();
            colEntitateID = new DataGridViewTextBoxColumn();
            lblNumar = new Label();
            pnlSpatiu = new Panel();
            pnlFiltre = new Panel();
            lblDeLa = new Label();
            dtpDeLa = new DateTimePicker();
            lblPanaLa = new Label();
            dtpPanaLa = new DateTimePicker();
            lblText = new Label();
            txtText = new TextBox();
            lblMax = new Label();
            numMax = new NumericUpDown();
            btnCauta = new Controale.ButonIcon();
            pnlRezultat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridAudit).BeginInit();
            pnlSpatiu.SuspendLayout();
            pnlFiltre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMax).BeginInit();
            SuspendLayout();
            // 
            // pnlRezultat
            // 
            pnlRezultat.Controls.Add(gridAudit);
            pnlRezultat.Controls.Add(lblNumar);
            pnlRezultat.BackColor = Color.White;
            pnlRezultat.Dock = DockStyle.Fill;
            pnlRezultat.Name = "pnlRezultat";
            pnlRezultat.Padding = new Padding(12, 12, 12, 12);
            // 
            // gridAudit
            // 
            gridAudit.Columns.AddRange(new DataGridViewColumn[] { colData, colUtilizator, colActiune, colEntitate, colEntitateID });
            gridAudit.Dock = DockStyle.Fill;
            gridAudit.Name = "gridAudit";
            gridAudit.TabIndex = 0;
            // 
            // colData
            // 
            colData.DataPropertyName = "DataOraLocal";
            colData.HeaderText = "Data și ora";
            colData.Name = "colData";
            colData.ReadOnly = true;
            colData.Width = 130;
            // 
            // colUtilizator
            // 
            colUtilizator.DataPropertyName = "Utilizator";
            colUtilizator.HeaderText = "Utilizator";
            colUtilizator.Name = "colUtilizator";
            colUtilizator.ReadOnly = true;
            colUtilizator.Width = 120;
            // 
            // colActiune
            // 
            colActiune.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colActiune.DataPropertyName = "Actiune";
            colActiune.FillWeight = 300F;
            colActiune.HeaderText = "Acțiunea";
            colActiune.MinimumWidth = 60;
            colActiune.Name = "colActiune";
            colActiune.ReadOnly = true;
            // 
            // colEntitate
            // 
            colEntitate.DataPropertyName = "Entitate";
            colEntitate.HeaderText = "Entitate";
            colEntitate.Name = "colEntitate";
            colEntitate.ReadOnly = true;
            colEntitate.Width = 110;
            // 
            // colEntitateID
            // 
            colEntitateID.DataPropertyName = "EntitateID";
            colEntitateID.HeaderText = "ID";
            colEntitateID.Name = "colEntitateID";
            colEntitateID.ReadOnly = true;
            colEntitateID.Width = 120;
            // 
            // lblNumar
            // 
            lblNumar.Dock = DockStyle.Bottom;
            lblNumar.ForeColor = Color.FromArgb(107, 114, 128);
            lblNumar.Name = "lblNumar";
            lblNumar.Size = new Size(984, 30);
            lblNumar.Text = "";
            lblNumar.TextAlign = ContentAlignment.MiddleLeft;
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
            pnlFiltre.Controls.Add(lblText);
            pnlFiltre.Controls.Add(txtText);
            pnlFiltre.Controls.Add(lblMax);
            pnlFiltre.Controls.Add(numMax);
            pnlFiltre.Controls.Add(btnCauta);
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
            dtpDeLa.Size = new Size(120, 23);
            dtpDeLa.TabIndex = 1;
            // 
            // lblPanaLa
            // 
            lblPanaLa.AutoSize = true;
            lblPanaLa.Location = new Point(148, 10);
            lblPanaLa.Name = "lblPanaLa";
            lblPanaLa.Text = "Până la";
            // 
            // dtpPanaLa
            // 
            dtpPanaLa.Format = DateTimePickerFormat.Short;
            dtpPanaLa.Location = new Point(148, 32);
            dtpPanaLa.Name = "dtpPanaLa";
            dtpPanaLa.Size = new Size(120, 23);
            dtpPanaLa.TabIndex = 2;
            // 
            // lblText
            // 
            lblText.AutoSize = true;
            lblText.Location = new Point(280, 10);
            lblText.Name = "lblText";
            lblText.Text = "Conține textul";
            // 
            // txtText
            // 
            txtText.Location = new Point(280, 32);
            txtText.Name = "txtText";
            txtText.Size = new Size(240, 23);
            txtText.TabIndex = 3;
            // 
            // lblMax
            // 
            lblMax.AutoSize = true;
            lblMax.Location = new Point(532, 10);
            lblMax.Name = "lblMax";
            lblMax.Text = "Maxim rânduri";
            // 
            // numMax
            // 
            numMax.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numMax.Location = new Point(532, 32);
            numMax.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numMax.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numMax.Name = "numMax";
            numMax.Size = new Size(90, 23);
            numMax.TabIndex = 4;
            numMax.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // btnCauta
            // 
            btnCauta.Icon = "search";
            btnCauta.Location = new Point(636, 28);
            btnCauta.Name = "btnCauta";
            btnCauta.Size = new Size(104, 32);
            btnCauta.Stil = Controale.StilButon.Primar;
            btnCauta.TabIndex = 5;
            btnCauta.Text = " Caută";
            btnCauta.Click += btnCauta_Click;
            // 
            // FrmAudit
            // 
            AcceptButton = btnCauta;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlRezultat);
            Controls.Add(pnlSpatiu);
            Controls.Add(pnlFiltre);
            Name = "FrmAudit";
            Text = "Jurnal de audit";
            Load += FrmAudit_Load;
            ((System.ComponentModel.ISupportInitialize)numMax).EndInit();
            pnlFiltre.ResumeLayout(false);
            pnlFiltre.PerformLayout();
            pnlSpatiu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridAudit).EndInit();
            pnlRezultat.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlRezultat;
        private Controale.GridAutogara gridAudit;
        private DataGridViewTextBoxColumn colData;
        private DataGridViewTextBoxColumn colUtilizator;
        private DataGridViewTextBoxColumn colActiune;
        private DataGridViewTextBoxColumn colEntitate;
        private DataGridViewTextBoxColumn colEntitateID;
        private Label lblNumar;
        private Panel pnlSpatiu;
        private Panel pnlFiltre;
        private Label lblDeLa;
        private DateTimePicker dtpDeLa;
        private Label lblPanaLa;
        private DateTimePicker dtpPanaLa;
        private Label lblText;
        private TextBox txtText;
        private Label lblMax;
        private NumericUpDown numMax;
        private Controale.ButonIcon btnCauta;
    }
}
