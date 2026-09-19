namespace Autogara.WinForms.Formulare
{
    partial class FrmRapoarte
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
            pnlRezultat = new Panel();
            gridVanzariZi = new Controale.GridAutogara();
            colZiTraseu = new DataGridViewTextBoxColumn();
            colZiCasier = new DataGridViewTextBoxColumn();
            colZiMetoda = new DataGridViewTextBoxColumn();
            colZiBilete = new DataGridViewTextBoxColumn();
            colZiRambursari = new DataGridViewTextBoxColumn();
            colZiIncasat = new DataGridViewTextBoxColumn();
            colZiRambursat = new DataGridViewTextBoxColumn();
            colZiNet = new DataGridViewTextBoxColumn();
            gridVanzariInterval = new Controale.GridAutogara();
            colIntData = new DataGridViewTextBoxColumn();
            colIntMetoda = new DataGridViewTextBoxColumn();
            colIntBilete = new DataGridViewTextBoxColumn();
            colIntRambursari = new DataGridViewTextBoxColumn();
            colIntIncasat = new DataGridViewTextBoxColumn();
            colIntRambursat = new DataGridViewTextBoxColumn();
            colIntNet = new DataGridViewTextBoxColumn();
            gridOcupare = new Controale.GridAutogara();
            colOcData = new DataGridViewTextBoxColumn();
            colOcOra = new DataGridViewTextBoxColumn();
            colOcTraseu = new DataGridViewTextBoxColumn();
            colOcAutobuz = new DataGridViewTextBoxColumn();
            colOcStatus = new DataGridViewTextBoxColumn();
            colOcLocuri = new DataGridViewTextBoxColumn();
            colOcOcupate = new DataGridViewTextBoxColumn();
            colOcGrad = new DataGridViewTextBoxColumn();
            colOcIncasari = new DataGridViewTextBoxColumn();
            gridComparativ = new Controale.GridAutogara();
            colCmpTraseu = new DataGridViewTextBoxColumn();
            colCmpLuna = new DataGridViewTextBoxColumn();
            colCmpCurse = new DataGridViewTextBoxColumn();
            colCmpBilete = new DataGridViewTextBoxColumn();
            colCmpBileteAnt = new DataGridViewTextBoxColumn();
            colCmpIncasari = new DataGridViewTextBoxColumn();
            colCmpVariatie = new DataGridViewTextBoxColumn();
            colCmpOcupare = new DataGridViewTextBoxColumn();
            colCmpVarOcupare = new DataGridViewTextBoxColumn();
            lblTotal = new Label();
            pnlSpatiu = new Panel();
            pnlFiltre = new Panel();
            lblRaport = new Label();
            cmbRaport = new ComboBox();
            lblDeLa = new Label();
            dtpDeLa = new DateTimePicker();
            lblPanaLa = new Label();
            dtpPanaLa = new DateTimePicker();
            lblTraseu = new Label();
            cmbTraseu = new ComboBox();
            btnAfiseaza = new Controale.ButonIcon();
            btnExport = new Controale.ButonIcon();
            btnArhiveaza = new Controale.ButonIcon();
            toolTip = new ToolTip(components);
            dlgSalvare = new SaveFileDialog();
            pnlRezultat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridVanzariZi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridVanzariInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridOcupare).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridComparativ).BeginInit();
            pnlSpatiu.SuspendLayout();
            pnlFiltre.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRezultat
            // 
            pnlRezultat.Controls.Add(gridVanzariZi);
            pnlRezultat.Controls.Add(gridVanzariInterval);
            pnlRezultat.Controls.Add(gridOcupare);
            pnlRezultat.Controls.Add(gridComparativ);
            pnlRezultat.Controls.Add(lblTotal);
            pnlRezultat.BackColor = Color.FromArgb(250, 249, 246);
            pnlRezultat.Dock = DockStyle.Fill;
            pnlRezultat.Name = "pnlRezultat";
            pnlRezultat.Padding = new Padding(12, 12, 12, 12);
            // 
            // gridVanzariZi
            // 
            gridVanzariZi.Columns.AddRange(new DataGridViewColumn[] { colZiTraseu, colZiCasier, colZiMetoda, colZiBilete, colZiRambursari, colZiIncasat, colZiRambursat, colZiNet });
            gridVanzariZi.Dock = DockStyle.Fill;
            gridVanzariZi.Name = "gridVanzariZi";
            gridVanzariZi.TabIndex = 0;
            // 
            // colZiTraseu
            // 
            colZiTraseu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colZiTraseu.DataPropertyName = "Traseu";
            colZiTraseu.FillWeight = 150F;
            colZiTraseu.HeaderText = "Traseu";
            colZiTraseu.MinimumWidth = 60;
            colZiTraseu.Name = "colZiTraseu";
            colZiTraseu.ReadOnly = true;
            // 
            // colZiCasier
            // 
            colZiCasier.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colZiCasier.DataPropertyName = "Casier";
            colZiCasier.FillWeight = 110F;
            colZiCasier.HeaderText = "Casier";
            colZiCasier.MinimumWidth = 60;
            colZiCasier.Name = "colZiCasier";
            colZiCasier.ReadOnly = true;
            // 
            // colZiMetoda
            // 
            colZiMetoda.DataPropertyName = "MetodaPlata";
            colZiMetoda.HeaderText = "Plata";
            colZiMetoda.Name = "colZiMetoda";
            colZiMetoda.ReadOnly = true;
            colZiMetoda.Width = 80;
            // 
            // colZiBilete
            // 
            colZiBilete.DataPropertyName = "BileteVandute";
            colZiBilete.HeaderText = "Bilete";
            colZiBilete.Name = "colZiBilete";
            colZiBilete.ReadOnly = true;
            colZiBilete.Width = 64;
            // 
            // colZiRambursari
            // 
            colZiRambursari.DataPropertyName = "Rambursari";
            colZiRambursari.HeaderText = "Rambursări";
            colZiRambursari.Name = "colZiRambursari";
            colZiRambursari.ReadOnly = true;
            colZiRambursari.Width = 84;
            // 
            // colZiIncasat
            // 
            colZiIncasat.DataPropertyName = "SumaIncasata";
            colZiIncasat.HeaderText = "Încasat";
            colZiIncasat.Name = "colZiIncasat";
            colZiIncasat.ReadOnly = true;
            colZiIncasat.Width = 90;
            // 
            // colZiRambursat
            // 
            colZiRambursat.DataPropertyName = "SumaRambursata";
            colZiRambursat.HeaderText = "Rambursat";
            colZiRambursat.Name = "colZiRambursat";
            colZiRambursat.ReadOnly = true;
            colZiRambursat.Width = 90;
            // 
            // colZiNet
            // 
            colZiNet.DataPropertyName = "IncasariNete";
            colZiNet.HeaderText = "Net";
            colZiNet.Name = "colZiNet";
            colZiNet.ReadOnly = true;
            colZiNet.Width = 90;
            // 
            // gridVanzariInterval
            // 
            gridVanzariInterval.Columns.AddRange(new DataGridViewColumn[] { colIntData, colIntMetoda, colIntBilete, colIntRambursari, colIntIncasat, colIntRambursat, colIntNet });
            gridVanzariInterval.Dock = DockStyle.Fill;
            gridVanzariInterval.Name = "gridVanzariInterval";
            gridVanzariInterval.TabIndex = 1;
            gridVanzariInterval.Visible = false;
            // 
            // colIntData
            // 
            colIntData.DataPropertyName = "Data";
            colIntData.HeaderText = "Data";
            colIntData.Name = "colIntData";
            colIntData.ReadOnly = true;
            colIntData.Width = 100;
            // 
            // colIntMetoda
            // 
            colIntMetoda.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colIntMetoda.DataPropertyName = "MetodaPlata";
            colIntMetoda.FillWeight = 80F;
            colIntMetoda.HeaderText = "Plata";
            colIntMetoda.MinimumWidth = 60;
            colIntMetoda.Name = "colIntMetoda";
            colIntMetoda.ReadOnly = true;
            // 
            // colIntBilete
            // 
            colIntBilete.DataPropertyName = "BileteVandute";
            colIntBilete.HeaderText = "Bilete";
            colIntBilete.Name = "colIntBilete";
            colIntBilete.ReadOnly = true;
            colIntBilete.Width = 80;
            // 
            // colIntRambursari
            // 
            colIntRambursari.DataPropertyName = "Rambursari";
            colIntRambursari.HeaderText = "Rambursări";
            colIntRambursari.Name = "colIntRambursari";
            colIntRambursari.ReadOnly = true;
            colIntRambursari.Width = 90;
            // 
            // colIntIncasat
            // 
            colIntIncasat.DataPropertyName = "SumaIncasata";
            colIntIncasat.HeaderText = "Încasat";
            colIntIncasat.Name = "colIntIncasat";
            colIntIncasat.ReadOnly = true;
            colIntIncasat.Width = 100;
            // 
            // colIntRambursat
            // 
            colIntRambursat.DataPropertyName = "SumaRambursata";
            colIntRambursat.HeaderText = "Rambursat";
            colIntRambursat.Name = "colIntRambursat";
            colIntRambursat.ReadOnly = true;
            colIntRambursat.Width = 100;
            // 
            // colIntNet
            // 
            colIntNet.DataPropertyName = "IncasariNete";
            colIntNet.HeaderText = "Net";
            colIntNet.Name = "colIntNet";
            colIntNet.ReadOnly = true;
            colIntNet.Width = 100;
            // 
            // gridOcupare
            // 
            gridOcupare.Columns.AddRange(new DataGridViewColumn[] { colOcData, colOcOra, colOcTraseu, colOcAutobuz, colOcStatus, colOcLocuri, colOcOcupate, colOcGrad, colOcIncasari });
            gridOcupare.Dock = DockStyle.Fill;
            gridOcupare.Name = "gridOcupare";
            gridOcupare.TabIndex = 2;
            gridOcupare.Visible = false;
            // 
            // colOcData
            // 
            colOcData.DataPropertyName = "DataCursa";
            colOcData.HeaderText = "Data";
            colOcData.Name = "colOcData";
            colOcData.ReadOnly = true;
            colOcData.Width = 86;
            // 
            // colOcOra
            // 
            colOcOra.DataPropertyName = "OraPlecare";
            colOcOra.HeaderText = "Ora";
            colOcOra.Name = "colOcOra";
            colOcOra.ReadOnly = true;
            colOcOra.Width = 56;
            // 
            // colOcTraseu
            // 
            colOcTraseu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colOcTraseu.DataPropertyName = "Traseu";
            colOcTraseu.FillWeight = 150F;
            colOcTraseu.HeaderText = "Traseu";
            colOcTraseu.MinimumWidth = 60;
            colOcTraseu.Name = "colOcTraseu";
            colOcTraseu.ReadOnly = true;
            // 
            // colOcAutobuz
            // 
            colOcAutobuz.DataPropertyName = "NrInmatriculare";
            colOcAutobuz.HeaderText = "Autobuz";
            colOcAutobuz.Name = "colOcAutobuz";
            colOcAutobuz.ReadOnly = true;
            colOcAutobuz.Width = 80;
            // 
            // colOcStatus
            // 
            colOcStatus.DataPropertyName = "StatusCursa";
            colOcStatus.HeaderText = "Status";
            colOcStatus.Name = "colOcStatus";
            colOcStatus.ReadOnly = true;
            colOcStatus.Width = 100;
            // 
            // colOcLocuri
            // 
            colOcLocuri.DataPropertyName = "CapacitateLocuri";
            colOcLocuri.HeaderText = "Locuri";
            colOcLocuri.Name = "colOcLocuri";
            colOcLocuri.ReadOnly = true;
            colOcLocuri.Width = 56;
            // 
            // colOcOcupate
            // 
            colOcOcupate.DataPropertyName = "LocuriOcupate";
            colOcOcupate.HeaderText = "Ocupate";
            colOcOcupate.Name = "colOcOcupate";
            colOcOcupate.ReadOnly = true;
            colOcOcupate.Width = 64;
            // 
            // colOcGrad
            // 
            colOcGrad.DataPropertyName = "GradOcupareProcent";
            colOcGrad.HeaderText = "Ocupare %";
            colOcGrad.Name = "colOcGrad";
            colOcGrad.ReadOnly = true;
            colOcGrad.Width = 80;
            // 
            // colOcIncasari
            // 
            colOcIncasari.DataPropertyName = "Incasari";
            colOcIncasari.HeaderText = "Încasări";
            colOcIncasari.Name = "colOcIncasari";
            colOcIncasari.ReadOnly = true;
            colOcIncasari.Width = 90;
            // 
            // gridComparativ
            // 
            gridComparativ.Columns.AddRange(new DataGridViewColumn[] { colCmpTraseu, colCmpLuna, colCmpCurse, colCmpBilete, colCmpBileteAnt, colCmpIncasari, colCmpVariatie, colCmpOcupare, colCmpVarOcupare });
            gridComparativ.Dock = DockStyle.Fill;
            gridComparativ.Name = "gridComparativ";
            gridComparativ.TabIndex = 3;
            gridComparativ.Visible = false;
            // 
            // colCmpTraseu
            // 
            colCmpTraseu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colCmpTraseu.DataPropertyName = "Traseu";
            colCmpTraseu.FillWeight = 150F;
            colCmpTraseu.HeaderText = "Traseu";
            colCmpTraseu.MinimumWidth = 60;
            colCmpTraseu.Name = "colCmpTraseu";
            colCmpTraseu.ReadOnly = true;
            // 
            // colCmpLuna
            // 
            colCmpLuna.DataPropertyName = "Luna";
            colCmpLuna.HeaderText = "Luna";
            colCmpLuna.Name = "colCmpLuna";
            colCmpLuna.ReadOnly = true;
            colCmpLuna.Width = 50;
            // 
            // colCmpCurse
            // 
            colCmpCurse.DataPropertyName = "NrCurse";
            colCmpCurse.HeaderText = "Curse";
            colCmpCurse.Name = "colCmpCurse";
            colCmpCurse.ReadOnly = true;
            colCmpCurse.Width = 56;
            // 
            // colCmpBilete
            // 
            colCmpBilete.DataPropertyName = "BileteVandute";
            colCmpBilete.HeaderText = "Bilete";
            colCmpBilete.Name = "colCmpBilete";
            colCmpBilete.ReadOnly = true;
            colCmpBilete.Width = 64;
            // 
            // colCmpBileteAnt
            // 
            colCmpBileteAnt.DataPropertyName = "BileteLunaPrecedenta";
            colCmpBileteAnt.HeaderText = "Luna trecută";
            colCmpBileteAnt.Name = "colCmpBileteAnt";
            colCmpBileteAnt.ReadOnly = true;
            colCmpBileteAnt.Width = 90;
            // 
            // colCmpIncasari
            // 
            colCmpIncasari.DataPropertyName = "Incasari";
            colCmpIncasari.HeaderText = "Încasări";
            colCmpIncasari.Name = "colCmpIncasari";
            colCmpIncasari.ReadOnly = true;
            colCmpIncasari.Width = 90;
            // 
            // colCmpVariatie
            // 
            colCmpVariatie.DataPropertyName = "VariatieIncasariProcent";
            colCmpVariatie.HeaderText = "Variație %";
            colCmpVariatie.Name = "colCmpVariatie";
            colCmpVariatie.ReadOnly = true;
            colCmpVariatie.Width = 80;
            // 
            // colCmpOcupare
            // 
            colCmpOcupare.DataPropertyName = "GradOcupareMediu";
            colCmpOcupare.HeaderText = "Ocupare %";
            colCmpOcupare.Name = "colCmpOcupare";
            colCmpOcupare.ReadOnly = true;
            colCmpOcupare.Width = 80;
            // 
            // colCmpVarOcupare
            // 
            colCmpVarOcupare.DataPropertyName = "VariatieOcuparePuncte";
            colCmpVarOcupare.HeaderText = "Variație (p.p.)";
            colCmpVarOcupare.Name = "colCmpVarOcupare";
            colCmpVarOcupare.ReadOnly = true;
            colCmpVarOcupare.Width = 100;
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.Font = new Font("Bahnschrift SemiBold", 9.5F);
            lblTotal.ForeColor = Color.FromArgb(34, 34, 31);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(984, 36);
            lblTotal.Text = "Alegeți perioada și apăsați „Afișează”.";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlSpatiu
            // 
            pnlSpatiu.Dock = DockStyle.Top;
            pnlSpatiu.Name = "pnlSpatiu";
            pnlSpatiu.Size = new Size(1008, 16);
            // 
            // pnlFiltre
            // 
            pnlFiltre.Controls.Add(lblRaport);
            pnlFiltre.Controls.Add(cmbRaport);
            pnlFiltre.Controls.Add(lblDeLa);
            pnlFiltre.Controls.Add(dtpDeLa);
            pnlFiltre.Controls.Add(lblPanaLa);
            pnlFiltre.Controls.Add(dtpPanaLa);
            pnlFiltre.Controls.Add(lblTraseu);
            pnlFiltre.Controls.Add(cmbTraseu);
            pnlFiltre.Controls.Add(btnAfiseaza);
            pnlFiltre.Controls.Add(btnExport);
            pnlFiltre.Controls.Add(btnArhiveaza);
            pnlFiltre.BackColor = Color.FromArgb(250, 249, 246);
            pnlFiltre.Dock = DockStyle.Top;
            pnlFiltre.Name = "pnlFiltre";
            pnlFiltre.Size = new Size(1008, 76);
            // 
            // lblRaport
            // 
            lblRaport.AutoSize = true;
            lblRaport.Location = new Point(16, 10);
            lblRaport.Name = "lblRaport";
            lblRaport.Text = "Raportul";
            // 
            // cmbRaport
            // 
            cmbRaport.DisplayMember = "Text";
            cmbRaport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRaport.Location = new Point(16, 32);
            cmbRaport.Name = "cmbRaport";
            cmbRaport.Size = new Size(250, 23);
            cmbRaport.TabIndex = 4;
            cmbRaport.SelectedIndexChanged += cmbRaport_SelectedIndexChanged;
            // 
            // lblDeLa
            // 
            lblDeLa.AutoSize = true;
            lblDeLa.Location = new Point(278, 10);
            lblDeLa.Name = "lblDeLa";
            lblDeLa.Text = "De la";
            // 
            // dtpDeLa
            // 
            dtpDeLa.Format = DateTimePickerFormat.Short;
            dtpDeLa.Location = new Point(278, 32);
            dtpDeLa.Name = "dtpDeLa";
            dtpDeLa.Size = new Size(116, 23);
            dtpDeLa.TabIndex = 5;
            // 
            // lblPanaLa
            // 
            lblPanaLa.AutoSize = true;
            lblPanaLa.Location = new Point(404, 10);
            lblPanaLa.Name = "lblPanaLa";
            lblPanaLa.Text = "Până la";
            // 
            // dtpPanaLa
            // 
            dtpPanaLa.Format = DateTimePickerFormat.Short;
            dtpPanaLa.Location = new Point(404, 32);
            dtpPanaLa.Name = "dtpPanaLa";
            dtpPanaLa.Size = new Size(116, 23);
            dtpPanaLa.TabIndex = 6;
            // 
            // lblTraseu
            // 
            lblTraseu.AutoSize = true;
            lblTraseu.Location = new Point(530, 10);
            lblTraseu.Name = "lblTraseu";
            lblTraseu.Text = "Traseu";
            // 
            // cmbTraseu
            // 
            cmbTraseu.DisplayMember = "Text";
            cmbTraseu.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTraseu.Location = new Point(530, 32);
            cmbTraseu.Name = "cmbTraseu";
            cmbTraseu.Size = new Size(170, 23);
            cmbTraseu.TabIndex = 7;
            // 
            // btnAfiseaza
            // 
            btnAfiseaza.Icon = "search";
            btnAfiseaza.Location = new Point(710, 28);
            btnAfiseaza.Name = "btnAfiseaza";
            btnAfiseaza.Size = new Size(104, 32);
            btnAfiseaza.Stil = Controale.StilButon.Primar;
            btnAfiseaza.TabIndex = 8;
            btnAfiseaza.Text = " Afișează";
            btnAfiseaza.Click += btnAfiseaza_Click;
            // 
            // btnExport
            // 
            toolTip.SetToolTip(btnExport, "Export pentru Excel (CSV)");
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.Icon = "file-spreadsheet";
            btnExport.Location = new Point(902, 28);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(40, 32);
            btnExport.TabIndex = 9;
            btnExport.Click += btnExport_Click;
            // 
            // btnArhiveaza
            // 
            toolTip.SetToolTip(btnArhiveaza, "Arhivează raportul pe file server");
            btnArhiveaza.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnArhiveaza.Icon = "archive";
            btnArhiveaza.Location = new Point(950, 28);
            btnArhiveaza.Name = "btnArhiveaza";
            btnArhiveaza.Size = new Size(40, 32);
            btnArhiveaza.TabIndex = 10;
            btnArhiveaza.Click += btnArhiveaza_Click;
            // 
            // toolTip
            // 
            // 
            // dlgSalvare
            // 
            dlgSalvare.DefaultExt = "csv";
            dlgSalvare.Filter = "Fișier CSV pentru Excel (*.csv)|*.csv";
            dlgSalvare.Title = "Salvează raportul";
            // 
            // FrmRapoarte
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlRezultat);
            Controls.Add(pnlSpatiu);
            Controls.Add(pnlFiltre);
            Name = "FrmRapoarte";
            Text = "Rapoarte";
            Load += FrmRapoarte_Load;
            pnlFiltre.ResumeLayout(false);
            pnlFiltre.PerformLayout();
            pnlSpatiu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridComparativ).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridOcupare).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridVanzariInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridVanzariZi).EndInit();
            pnlRezultat.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlRezultat;
        private Controale.GridAutogara gridVanzariZi;
        private DataGridViewTextBoxColumn colZiTraseu;
        private DataGridViewTextBoxColumn colZiCasier;
        private DataGridViewTextBoxColumn colZiMetoda;
        private DataGridViewTextBoxColumn colZiBilete;
        private DataGridViewTextBoxColumn colZiRambursari;
        private DataGridViewTextBoxColumn colZiIncasat;
        private DataGridViewTextBoxColumn colZiRambursat;
        private DataGridViewTextBoxColumn colZiNet;
        private Controale.GridAutogara gridVanzariInterval;
        private DataGridViewTextBoxColumn colIntData;
        private DataGridViewTextBoxColumn colIntMetoda;
        private DataGridViewTextBoxColumn colIntBilete;
        private DataGridViewTextBoxColumn colIntRambursari;
        private DataGridViewTextBoxColumn colIntIncasat;
        private DataGridViewTextBoxColumn colIntRambursat;
        private DataGridViewTextBoxColumn colIntNet;
        private Controale.GridAutogara gridOcupare;
        private DataGridViewTextBoxColumn colOcData;
        private DataGridViewTextBoxColumn colOcOra;
        private DataGridViewTextBoxColumn colOcTraseu;
        private DataGridViewTextBoxColumn colOcAutobuz;
        private DataGridViewTextBoxColumn colOcStatus;
        private DataGridViewTextBoxColumn colOcLocuri;
        private DataGridViewTextBoxColumn colOcOcupate;
        private DataGridViewTextBoxColumn colOcGrad;
        private DataGridViewTextBoxColumn colOcIncasari;
        private Controale.GridAutogara gridComparativ;
        private DataGridViewTextBoxColumn colCmpTraseu;
        private DataGridViewTextBoxColumn colCmpLuna;
        private DataGridViewTextBoxColumn colCmpCurse;
        private DataGridViewTextBoxColumn colCmpBilete;
        private DataGridViewTextBoxColumn colCmpBileteAnt;
        private DataGridViewTextBoxColumn colCmpIncasari;
        private DataGridViewTextBoxColumn colCmpVariatie;
        private DataGridViewTextBoxColumn colCmpOcupare;
        private DataGridViewTextBoxColumn colCmpVarOcupare;
        private Label lblTotal;
        private Panel pnlSpatiu;
        private Panel pnlFiltre;
        private Label lblRaport;
        private ComboBox cmbRaport;
        private Label lblDeLa;
        private DateTimePicker dtpDeLa;
        private Label lblPanaLa;
        private DateTimePicker dtpPanaLa;
        private Label lblTraseu;
        private ComboBox cmbTraseu;
        private Controale.ButonIcon btnAfiseaza;
        private Controale.ButonIcon btnExport;
        private Controale.ButonIcon btnArhiveaza;
        private ToolTip toolTip;
        private SaveFileDialog dlgSalvare;
    }
}
