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
            flpActiuni = new FlowLayoutPanel();
            btnReincarca = new Controale.ButonIcon();
            btnLucrare = new Controale.ButonIcon();
            btnItp = new Controale.ButonIcon();
            cmbFiltru = new ComboBox();
            pnlLista.SuspendLayout();
            tabLista.SuspendLayout();
            tpLucrari.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLucrari).BeginInit();
            tpItp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridItp).BeginInit();
            pnlListaAntet.SuspendLayout();
            flpActiuni.SuspendLayout();
            SuspendLayout();
            //
            // pnlLista
            //
            pnlLista.Controls.Add(tabLista);
            pnlLista.Controls.Add(pnlListaAntet);
            pnlLista.BackColor = Color.FromArgb(250, 249, 246);
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
            tpLucrari.BackColor = Color.FromArgb(250, 249, 246);
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
            colAutobuz.Width = 100;
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
            colKm.Width = 90;
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
            tpItp.BackColor = Color.FromArgb(250, 249, 246);
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
            colItpZile.Width = 100;
            //
            // lblItpNota
            //
            lblItpNota.Dock = DockStyle.Bottom;
            lblItpNota.ForeColor = Color.FromArgb(110, 106, 96);
            lblItpNota.Name = "lblItpNota";
            lblItpNota.Size = new Size(560, 32);
            lblItpNota.Text = "Autobuzele active cu ITP-ul expirat sau care expiră în următoarele 30 de zile (zile negative = expirat). Alegeți unul și apăsați „ITP nou”.";
            lblItpNota.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pnlListaAntet
            //
            pnlListaAntet.Controls.Add(flpActiuni);
            pnlListaAntet.Controls.Add(lblLista);
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
            lblLista.Text = "Istoric";
            //
            // flpActiuni
            //
            flpActiuni.AutoSize = true;
            flpActiuni.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActiuni.Controls.Add(btnReincarca);
            flpActiuni.Controls.Add(btnLucrare);
            flpActiuni.Controls.Add(btnItp);
            flpActiuni.Controls.Add(cmbFiltru);
            flpActiuni.Dock = DockStyle.Right;
            flpActiuni.FlowDirection = FlowDirection.RightToLeft;
            flpActiuni.Name = "flpActiuni";
            flpActiuni.Padding = new Padding(0, 10, 0, 0);
            flpActiuni.WrapContents = false;
            //
            // btnReincarca
            //
            btnReincarca.Icon = "refresh-cw";
            btnReincarca.Margin = new Padding(8, 0, 0, 0);
            btnReincarca.Name = "btnReincarca";
            btnReincarca.Size = new Size(36, 32);
            btnReincarca.TabIndex = 3;
            btnReincarca.Click += btnReincarca_Click;
            //
            // btnLucrare
            //
            btnLucrare.Icon = "plus";
            btnLucrare.Margin = new Padding(8, 0, 0, 0);
            btnLucrare.Name = "btnLucrare";
            btnLucrare.Size = new Size(130, 32);
            btnLucrare.Stil = Controale.StilButon.Primar;
            btnLucrare.TabIndex = 1;
            btnLucrare.Text = " Lucrare nouă";
            btnLucrare.Click += btnLucrare_Click;
            //
            // btnItp
            //
            btnItp.Icon = "shield-check";
            btnItp.Margin = new Padding(8, 0, 0, 0);
            btnItp.Name = "btnItp";
            btnItp.Size = new Size(100, 32);
            btnItp.TabIndex = 2;
            btnItp.Text = " ITP nou";
            btnItp.Click += btnItp_Click;
            //
            // cmbFiltru
            //
            cmbFiltru.DisplayMember = "Text";
            cmbFiltru.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltru.Margin = new Padding(8, 5, 8, 0);
            cmbFiltru.Name = "cmbFiltru";
            cmbFiltru.Size = new Size(260, 23);
            cmbFiltru.TabIndex = 4;
            cmbFiltru.SelectedIndexChanged += cmbFiltru_SelectedIndexChanged;
            //
            // FrmAdminMentenanta
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Name = "FrmAdminMentenanta";
            Text = "Mentenanță";
            Load += FrmAdminMentenanta_Load;
            flpActiuni.ResumeLayout(false);
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
        private FlowLayoutPanel flpActiuni;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnLucrare;
        private Controale.ButonIcon btnItp;
        private ComboBox cmbFiltru;
    }
}
