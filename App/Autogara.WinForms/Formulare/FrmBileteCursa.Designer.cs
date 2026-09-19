namespace Autogara.WinForms.Formulare
{
    partial class FrmBileteCursa
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
            pnlLista = new Panel();
            gridBilete = new Controale.GridAutogara();
            colLoc = new DataGridViewTextBoxColumn();
            colCod = new DataGridViewTextBoxColumn();
            colPasager = new DataGridViewTextBoxColumn();
            colTelefon = new DataGridViewTextBoxColumn();
            colPret = new DataGridViewTextBoxColumn();
            colReducere = new DataGridViewTextBoxColumn();
            colPlata = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colVandutDe = new DataGridViewTextBoxColumn();
            pnlAntet = new Panel();
            iconTitlu = new Controale.IconImagine();
            lblTitlu = new Label();
            lblSumar = new Label();
            pnlButoane = new Panel();
            btnInchide = new Controale.ButonIcon();
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridBilete).BeginInit();
            pnlAntet.SuspendLayout();
            pnlButoane.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLista
            // 
            pnlLista.Controls.Add(gridBilete);
            pnlLista.BackColor = Color.FromArgb(250, 249, 246);
            pnlLista.Dock = DockStyle.Fill;
            pnlLista.Name = "pnlLista";
            pnlLista.Padding = new Padding(12, 12, 12, 12);
            // 
            // gridBilete
            // 
            gridBilete.Columns.AddRange(new DataGridViewColumn[] { colLoc, colCod, colPasager, colTelefon, colPret, colReducere, colPlata, colStatus, colVandutDe });
            gridBilete.Dock = DockStyle.Fill;
            gridBilete.Name = "gridBilete";
            gridBilete.TabIndex = 0;
            // 
            // colLoc
            // 
            colLoc.DataPropertyName = "NumarLoc";
            colLoc.HeaderText = "Loc";
            colLoc.Name = "colLoc";
            colLoc.ReadOnly = true;
            colLoc.Width = 48;
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
            colPasager.FillWeight = 140F;
            colPasager.HeaderText = "Pasager";
            colPasager.MinimumWidth = 60;
            colPasager.Name = "colPasager";
            colPasager.ReadOnly = true;
            // 
            // colTelefon
            // 
            colTelefon.DataPropertyName = "TelefonPasager";
            colTelefon.HeaderText = "Telefon";
            colTelefon.Name = "colTelefon";
            colTelefon.ReadOnly = true;
            colTelefon.Width = 120;
            // 
            // colPret
            // 
            colPret.DataPropertyName = "Pret";
            colPret.HeaderText = "Preț";
            colPret.Name = "colPret";
            colPret.ReadOnly = true;
            colPret.Width = 76;
            // 
            // colReducere
            // 
            colReducere.DataPropertyName = "Reducere";
            colReducere.HeaderText = "Reducere";
            colReducere.Name = "colReducere";
            colReducere.ReadOnly = true;
            colReducere.Width = 90;
            // 
            // colPlata
            // 
            colPlata.DataPropertyName = "MetodaPlata";
            colPlata.HeaderText = "Plata";
            colPlata.Name = "colPlata";
            colPlata.ReadOnly = true;
            colPlata.Width = 76;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 86;
            // 
            // colVandutDe
            // 
            colVandutDe.DataPropertyName = "VandutDe";
            colVandutDe.HeaderText = "Vândut de";
            colVandutDe.Name = "colVandutDe";
            colVandutDe.ReadOnly = true;
            colVandutDe.Width = 130;
            // 
            // pnlAntet
            // 
            pnlAntet.Controls.Add(iconTitlu);
            pnlAntet.Controls.Add(lblTitlu);
            pnlAntet.Controls.Add(lblSumar);
            pnlAntet.BackColor = Color.FromArgb(250, 249, 246);
            pnlAntet.Dock = DockStyle.Top;
            pnlAntet.Name = "pnlAntet";
            pnlAntet.Size = new Size(900, 64);
            // 
            // iconTitlu
            // 
            iconTitlu.Icon = "users";
            iconTitlu.Location = new Point(20, 20);
            iconTitlu.Name = "iconTitlu";
            iconTitlu.Size = new Size(24, 24);
            // 
            // lblTitlu
            // 
            lblTitlu.AutoSize = true;
            lblTitlu.Font = new Font("Bahnschrift SemiBold", 12F);
            lblTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblTitlu.Location = new Point(54, 10);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Text = "Cursa";
            // 
            // lblSumar
            // 
            lblSumar.AutoSize = true;
            lblSumar.ForeColor = Color.FromArgb(110, 106, 96);
            lblSumar.Location = new Point(56, 38);
            lblSumar.Name = "lblSumar";
            lblSumar.Text = "";
            // 
            // pnlButoane
            // 
            pnlButoane.Controls.Add(btnInchide);
            pnlButoane.BackColor = Color.FromArgb(250, 249, 246);
            pnlButoane.Dock = DockStyle.Bottom;
            pnlButoane.Name = "pnlButoane";
            pnlButoane.Size = new Size(900, 56);
            // 
            // btnInchide
            // 
            btnInchide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInchide.DialogResult = DialogResult.Cancel;
            btnInchide.Icon = "x";
            btnInchide.Location = new Point(784, 12);
            btnInchide.Name = "btnInchide";
            btnInchide.Size = new Size(100, 32);
            btnInchide.TabIndex = 1;
            btnInchide.Text = " Închide";
            // 
            // FrmBileteCursa
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            CancelButton = btnInchide;
            ClientSize = new Size(900, 560);
            Controls.Add(pnlLista);
            Controls.Add(pnlAntet);
            Controls.Add(pnlButoane);
            MinimizeBox = false;
            Name = "FrmBileteCursa";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Biletele cursei";
            Load += FrmBileteCursa_Load;
            pnlButoane.ResumeLayout(false);
            pnlAntet.ResumeLayout(false);
            pnlAntet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridBilete).EndInit();
            pnlLista.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLista;
        private Controale.GridAutogara gridBilete;
        private DataGridViewTextBoxColumn colLoc;
        private DataGridViewTextBoxColumn colCod;
        private DataGridViewTextBoxColumn colPasager;
        private DataGridViewTextBoxColumn colTelefon;
        private DataGridViewTextBoxColumn colPret;
        private DataGridViewTextBoxColumn colReducere;
        private DataGridViewTextBoxColumn colPlata;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colVandutDe;
        private Panel pnlAntet;
        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblSumar;
        private Panel pnlButoane;
        private Controale.ButonIcon btnInchide;
    }
}
