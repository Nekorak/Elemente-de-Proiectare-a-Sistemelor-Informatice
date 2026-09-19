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
            flpActiuni = new FlowLayoutPanel();
            btnAdauga = new Controale.ButonIcon();
            btnModifica = new Controale.ButonIcon();
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
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            pnlListaAntet.SuspendLayout();
            flpActiuni.SuspendLayout();
            pnlFiltre.SuspendLayout();
            SuspendLayout();
            //
            // pnlLista
            //
            pnlLista.Controls.Add(gridLista);
            pnlLista.Controls.Add(pnlListaAntet);
            pnlLista.BackColor = Color.FromArgb(250, 249, 246);
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
            gridLista.CellDoubleClick += gridLista_CellDoubleClick;
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
            colPlecare.HeaderText = "Ora";
            colPlecare.Name = "colPlecare";
            colPlecare.ReadOnly = true;
            colPlecare.Width = 60;
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
            colAutobuz.Width = 100;
            //
            // colPret
            //
            colPret.DataPropertyName = "Pret";
            colPret.HeaderText = "Preț";
            colPret.Name = "colPret";
            colPret.ReadOnly = true;
            colPret.Width = 80;
            //
            // colLibere
            //
            colLibere.DataPropertyName = "LocuriLibere";
            colLibere.HeaderText = "Libere";
            colLibere.Name = "colLibere";
            colLibere.ReadOnly = true;
            colLibere.Width = 64;
            //
            // colStatus
            //
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 110;
            //
            // pnlListaAntet
            //
            pnlListaAntet.Controls.Add(flpActiuni);
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(lblNumar);
            pnlListaAntet.Dock = DockStyle.Top;
            pnlListaAntet.Name = "pnlListaAntet";
            pnlListaAntet.Size = new Size(984, 52);
            //
            // lblLista
            //
            lblLista.AutoSize = true;
            lblLista.Font = new Font("Bahnschrift SemiBold", 11F);
            lblLista.ForeColor = Color.FromArgb(34, 34, 31);
            lblLista.Location = new Point(4, 16);
            lblLista.Name = "lblLista";
            lblLista.Text = "Curse";
            //
            // lblNumar
            //
            lblNumar.AutoSize = true;
            lblNumar.ForeColor = Color.FromArgb(110, 106, 96);
            lblNumar.Location = new Point(60, 19);
            lblNumar.Name = "lblNumar";
            lblNumar.Text = "";
            //
            // flpActiuni
            //
            flpActiuni.AutoSize = true;
            flpActiuni.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActiuni.Controls.Add(btnAdauga);
            flpActiuni.Controls.Add(btnModifica);
            flpActiuni.Controls.Add(btnPlecata);
            flpActiuni.Controls.Add(btnFinalizata);
            flpActiuni.Controls.Add(btnBilete);
            flpActiuni.Controls.Add(btnAnuleaza);
            flpActiuni.Dock = DockStyle.Right;
            flpActiuni.FlowDirection = FlowDirection.RightToLeft;
            flpActiuni.Name = "flpActiuni";
            flpActiuni.Padding = new Padding(0, 10, 0, 0);
            flpActiuni.WrapContents = false;
            //
            // btnAdauga
            //
            btnAdauga.Icon = "plus";
            btnAdauga.Margin = new Padding(8, 0, 0, 0);
            btnAdauga.Name = "btnAdauga";
            btnAdauga.Size = new Size(104, 32);
            btnAdauga.Stil = Controale.StilButon.Primar;
            btnAdauga.TabIndex = 1;
            btnAdauga.Text = " Adaugă";
            btnAdauga.Click += btnAdauga_Click;
            //
            // btnModifica
            //
            btnModifica.Icon = "pencil";
            btnModifica.Margin = new Padding(8, 0, 0, 0);
            btnModifica.Name = "btnModifica";
            btnModifica.Size = new Size(110, 32);
            btnModifica.TabIndex = 2;
            btnModifica.Text = " Modifică";
            btnModifica.Click += btnModifica_Click;
            //
            // btnPlecata
            //
            btnPlecata.Icon = "play";
            btnPlecata.Margin = new Padding(8, 0, 0, 0);
            btnPlecata.Name = "btnPlecata";
            btnPlecata.Size = new Size(106, 32);
            btnPlecata.TabIndex = 3;
            btnPlecata.Text = " A plecat";
            btnPlecata.Click += btnPlecata_Click;
            //
            // btnFinalizata
            //
            btnFinalizata.Icon = "flag";
            btnFinalizata.Margin = new Padding(8, 0, 0, 0);
            btnFinalizata.Name = "btnFinalizata";
            btnFinalizata.Size = new Size(106, 32);
            btnFinalizata.TabIndex = 4;
            btnFinalizata.Text = " A ajuns";
            btnFinalizata.Click += btnFinalizata_Click;
            //
            // btnBilete
            //
            btnBilete.Icon = "users";
            btnBilete.Margin = new Padding(8, 0, 0, 0);
            btnBilete.Name = "btnBilete";
            btnBilete.Size = new Size(110, 32);
            btnBilete.TabIndex = 5;
            btnBilete.Text = " Pasagerii";
            btnBilete.Click += btnBilete_Click;
            //
            // btnAnuleaza
            //
            btnAnuleaza.Icon = "ban";
            btnAnuleaza.Margin = new Padding(8, 0, 0, 0);
            btnAnuleaza.Name = "btnAnuleaza";
            btnAnuleaza.Size = new Size(116, 32);
            btnAnuleaza.Stil = Controale.StilButon.Pericol;
            btnAnuleaza.TabIndex = 6;
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
            pnlFiltre.BackColor = Color.FromArgb(250, 249, 246);
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
            btnAfiseaza.TabIndex = 17;
            btnAfiseaza.Text = " Afișează";
            btnAfiseaza.Click += btnAfiseaza_Click;
            //
            // FrmAdminCurse
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiu);
            Controls.Add(pnlFiltre);
            Name = "FrmAdminCurse";
            Text = "Curse";
            Load += FrmAdminCurse_Load;
            pnlFiltre.ResumeLayout(false);
            pnlFiltre.PerformLayout();
            flpActiuni.ResumeLayout(false);
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
        private FlowLayoutPanel flpActiuni;
        private Controale.ButonIcon btnAdauga;
        private Controale.ButonIcon btnModifica;
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
    }
}
