namespace Autogara.WinForms.Formulare
{
    partial class FrmDashboard
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
            pnlUrmatoare = new Panel();
            gridCurse = new Controale.GridAutogara();
            colData = new DataGridViewTextBoxColumn();
            colPlecare = new DataGridViewTextBoxColumn();
            colTraseu = new DataGridViewTextBoxColumn();
            colDeLa = new DataGridViewTextBoxColumn();
            colPeron = new DataGridViewTextBoxColumn();
            colAutobuz = new DataGridViewTextBoxColumn();
            colSofer = new DataGridViewTextBoxColumn();
            colLibere = new DataGridViewTextBoxColumn();
            colOcupate = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            pnlUrmatoareAntet = new Panel();
            lblUrmatoare = new Label();
            lblActualizat = new Label();
            btnReimprospateaza = new Controale.ButonIcon();
            pnlSpatiu = new Panel();
            flpCarduri = new FlowLayoutPanel();
            cardCurseAzi = new Controale.CardKpi();
            cardInDesfasurare = new Controale.CardKpi();
            cardRamase = new Controale.CardKpi();
            cardBilete = new Controale.CardKpi();
            cardAnulate = new Controale.CardKpi();
            cardIncasari = new Controale.CardKpi();
            cardRezervari = new Controale.CardKpi();
            cardItp = new Controale.CardKpi();
            timerActualizare = new System.Windows.Forms.Timer(components);
            pnlUrmatoare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCurse).BeginInit();
            pnlUrmatoareAntet.SuspendLayout();
            pnlSpatiu.SuspendLayout();
            flpCarduri.SuspendLayout();
            SuspendLayout();
            // 
            // pnlUrmatoare
            // 
            pnlUrmatoare.Controls.Add(gridCurse);
            pnlUrmatoare.Controls.Add(pnlUrmatoareAntet);
            pnlUrmatoare.BackColor = Color.FromArgb(250, 249, 246);
            pnlUrmatoare.Dock = DockStyle.Fill;
            pnlUrmatoare.Name = "pnlUrmatoare";
            pnlUrmatoare.Padding = new Padding(12, 0, 12, 12);
            // 
            // gridCurse
            // 
            gridCurse.Columns.AddRange(new DataGridViewColumn[] { colData, colPlecare, colTraseu, colDeLa, colPeron, colAutobuz, colSofer, colLibere, colOcupate, colStatus });
            gridCurse.Dock = DockStyle.Fill;
            gridCurse.Name = "gridCurse";
            gridCurse.TabIndex = 0;
            // 
            // colData
            // 
            colData.DataPropertyName = "DataCursa";
            colData.HeaderText = "Data";
            colData.Name = "colData";
            colData.ReadOnly = true;
            colData.Width = 90;
            // 
            // colPlecare
            // 
            colPlecare.DataPropertyName = "OraPlecare";
            colPlecare.HeaderText = "Plecare";
            colPlecare.Name = "colPlecare";
            colPlecare.ReadOnly = true;
            colPlecare.Width = 70;
            // 
            // colTraseu
            // 
            colTraseu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTraseu.DataPropertyName = "Traseu";
            colTraseu.FillWeight = 160F;
            colTraseu.HeaderText = "Traseu";
            colTraseu.MinimumWidth = 60;
            colTraseu.Name = "colTraseu";
            colTraseu.ReadOnly = true;
            // 
            // colDeLa
            // 
            colDeLa.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDeLa.DataPropertyName = "StatiePlecare";
            colDeLa.FillWeight = 110F;
            colDeLa.HeaderText = "De la";
            colDeLa.MinimumWidth = 60;
            colDeLa.Name = "colDeLa";
            colDeLa.ReadOnly = true;
            // 
            // colPeron
            // 
            colPeron.DataPropertyName = "PeronPlecare";
            colPeron.HeaderText = "Peron";
            colPeron.Name = "colPeron";
            colPeron.ReadOnly = true;
            colPeron.Width = 60;
            // 
            // colAutobuz
            // 
            colAutobuz.DataPropertyName = "NrInmatriculare";
            colAutobuz.HeaderText = "Autobuz";
            colAutobuz.Name = "colAutobuz";
            colAutobuz.ReadOnly = true;
            colAutobuz.Width = 90;
            // 
            // colSofer
            // 
            colSofer.DataPropertyName = "Sofer";
            colSofer.HeaderText = "Șofer";
            colSofer.Name = "colSofer";
            colSofer.ReadOnly = true;
            colSofer.Width = 140;
            // 
            // colLibere
            // 
            colLibere.DataPropertyName = "LocuriLibere";
            colLibere.HeaderText = "Libere";
            colLibere.Name = "colLibere";
            colLibere.ReadOnly = true;
            colLibere.Width = 64;
            // 
            // colOcupate
            // 
            colOcupate.DataPropertyName = "LocuriOcupate";
            colOcupate.HeaderText = "Ocupate";
            colOcupate.Name = "colOcupate";
            colOcupate.ReadOnly = true;
            colOcupate.Width = 70;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 110;
            // 
            // pnlUrmatoareAntet
            // 
            pnlUrmatoareAntet.Controls.Add(lblUrmatoare);
            pnlUrmatoareAntet.Controls.Add(lblActualizat);
            pnlUrmatoareAntet.Controls.Add(btnReimprospateaza);
            pnlUrmatoareAntet.Dock = DockStyle.Top;
            pnlUrmatoareAntet.Name = "pnlUrmatoareAntet";
            pnlUrmatoareAntet.Size = new Size(1008, 52);
            // 
            // lblUrmatoare
            // 
            lblUrmatoare.AutoSize = true;
            lblUrmatoare.Font = new Font("Bahnschrift SemiBold", 11F);
            lblUrmatoare.ForeColor = Color.FromArgb(34, 34, 31);
            lblUrmatoare.Location = new Point(4, 16);
            lblUrmatoare.Name = "lblUrmatoare";
            lblUrmatoare.Text = "Următoarele curse";
            // 
            // lblActualizat
            // 
            lblActualizat.AutoSize = true;
            lblActualizat.ForeColor = Color.FromArgb(110, 106, 96);
            lblActualizat.Location = new Point(170, 19);
            lblActualizat.Name = "lblActualizat";
            lblActualizat.Text = "";
            // 
            // btnReimprospateaza
            // 
            btnReimprospateaza.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReimprospateaza.Icon = "refresh-cw";
            btnReimprospateaza.Location = new Point(836, 10);
            btnReimprospateaza.Name = "btnReimprospateaza";
            btnReimprospateaza.Size = new Size(160, 32);
            btnReimprospateaza.TabIndex = 1;
            btnReimprospateaza.Text = " Reîmprospătează";
            btnReimprospateaza.Click += btnReimprospateaza_Click;
            // 
            // pnlSpatiu
            // 
            pnlSpatiu.Dock = DockStyle.Top;
            pnlSpatiu.Name = "pnlSpatiu";
            pnlSpatiu.Size = new Size(1008, 8);
            // 
            // flpCarduri
            // 
            flpCarduri.Controls.Add(cardCurseAzi);
            flpCarduri.Controls.Add(cardInDesfasurare);
            flpCarduri.Controls.Add(cardRamase);
            flpCarduri.Controls.Add(cardBilete);
            flpCarduri.Controls.Add(cardAnulate);
            flpCarduri.Controls.Add(cardIncasari);
            flpCarduri.Controls.Add(cardRezervari);
            flpCarduri.Controls.Add(cardItp);
            flpCarduri.Dock = DockStyle.Top;
            flpCarduri.Name = "flpCarduri";
            flpCarduri.Size = new Size(1008, 224);
            // 
            // cardCurseAzi
            // 
            cardCurseAzi.Icon = "calendar";
            cardCurseAzi.Margin = new Padding(0, 0, 16, 16);
            cardCurseAzi.Name = "cardCurseAzi";
            cardCurseAzi.Size = new Size(236, 96);
            cardCurseAzi.Titlu = "Curse azi";
            // 
            // cardInDesfasurare
            // 
            cardInDesfasurare.Icon = "play";
            cardInDesfasurare.Margin = new Padding(0, 0, 16, 16);
            cardInDesfasurare.Name = "cardInDesfasurare";
            cardInDesfasurare.Size = new Size(236, 96);
            cardInDesfasurare.Titlu = "În desfășurare";
            // 
            // cardRamase
            // 
            cardRamase.Icon = "clock";
            cardRamase.Margin = new Padding(0, 0, 16, 16);
            cardRamase.Name = "cardRamase";
            cardRamase.Size = new Size(236, 96);
            cardRamase.Titlu = "Plecări rămase azi";
            // 
            // cardBilete
            // 
            cardBilete.Icon = "ticket";
            cardBilete.Margin = new Padding(0, 0, 16, 16);
            cardBilete.Name = "cardBilete";
            cardBilete.Size = new Size(236, 96);
            cardBilete.Titlu = "Bilete vândute azi";
            // 
            // cardAnulate
            // 
            cardAnulate.Accent = Color.FromArgb(179, 38, 30);
            cardAnulate.Icon = "ticket-x";
            cardAnulate.Margin = new Padding(0, 0, 16, 16);
            cardAnulate.Name = "cardAnulate";
            cardAnulate.Size = new Size(236, 96);
            cardAnulate.Titlu = "Bilete anulate azi";
            // 
            // cardIncasari
            // 
            cardIncasari.Accent = Color.FromArgb(46, 125, 50);
            cardIncasari.Icon = "banknote";
            cardIncasari.Margin = new Padding(0, 0, 16, 16);
            cardIncasari.Name = "cardIncasari";
            cardIncasari.Size = new Size(236, 96);
            cardIncasari.Titlu = "Încasări nete azi";
            // 
            // cardRezervari
            // 
            cardRezervari.Accent = Color.FromArgb(183, 121, 31);
            cardRezervari.Icon = "armchair";
            cardRezervari.Margin = new Padding(0, 0, 16, 16);
            cardRezervari.Name = "cardRezervari";
            cardRezervari.Size = new Size(236, 96);
            cardRezervari.Titlu = "Rezervări active";
            // 
            // cardItp
            // 
            cardItp.Accent = Color.FromArgb(183, 121, 31);
            cardItp.Icon = "triangle-alert";
            cardItp.Margin = new Padding(0, 0, 16, 16);
            cardItp.Name = "cardItp";
            cardItp.Size = new Size(236, 96);
            cardItp.Titlu = "ITP în următoarele 30 de zile";
            // 
            // timerActualizare
            // 
            timerActualizare.Interval = 30000;
            timerActualizare.Tick += timerActualizare_Tick;
            // 
            // FrmDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlUrmatoare);
            Controls.Add(pnlSpatiu);
            Controls.Add(flpCarduri);
            Name = "FrmDashboard";
            Text = "Dashboard";
            FormClosed += FrmDashboard_FormClosed;
            Load += FrmDashboard_Load;
            flpCarduri.ResumeLayout(false);
            pnlSpatiu.ResumeLayout(false);
            pnlUrmatoareAntet.ResumeLayout(false);
            pnlUrmatoareAntet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridCurse).EndInit();
            pnlUrmatoare.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlUrmatoare;
        private Controale.GridAutogara gridCurse;
        private DataGridViewTextBoxColumn colData;
        private DataGridViewTextBoxColumn colPlecare;
        private DataGridViewTextBoxColumn colTraseu;
        private DataGridViewTextBoxColumn colDeLa;
        private DataGridViewTextBoxColumn colPeron;
        private DataGridViewTextBoxColumn colAutobuz;
        private DataGridViewTextBoxColumn colSofer;
        private DataGridViewTextBoxColumn colLibere;
        private DataGridViewTextBoxColumn colOcupate;
        private DataGridViewTextBoxColumn colStatus;
        private Panel pnlUrmatoareAntet;
        private Label lblUrmatoare;
        private Label lblActualizat;
        private Controale.ButonIcon btnReimprospateaza;
        private Panel pnlSpatiu;
        private FlowLayoutPanel flpCarduri;
        private Controale.CardKpi cardCurseAzi;
        private Controale.CardKpi cardInDesfasurare;
        private Controale.CardKpi cardRamase;
        private Controale.CardKpi cardBilete;
        private Controale.CardKpi cardAnulate;
        private Controale.CardKpi cardIncasari;
        private Controale.CardKpi cardRezervari;
        private Controale.CardKpi cardItp;
        private System.Windows.Forms.Timer timerActualizare;
    }
}
