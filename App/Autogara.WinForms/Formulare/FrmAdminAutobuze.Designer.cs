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
            flpActiuni = new FlowLayoutPanel();
            btnReincarca = new Controale.ButonIcon();
            btnAdauga = new Controale.ButonIcon();
            btnModifica = new Controale.ButonIcon();
            btnLocuri = new Controale.ButonIcon();
            chkInactive = new CheckBox();
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            pnlListaAntet.SuspendLayout();
            flpActiuni.SuspendLayout();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colNr, colModel, colLocuri, colStatus, colItp, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            gridLista.CellDoubleClick += gridLista_CellDoubleClick;
            //
            // colNr
            //
            colNr.DataPropertyName = "NrInmatriculare";
            colNr.HeaderText = "Nr. înmatriculare";
            colNr.Name = "colNr";
            colNr.ReadOnly = true;
            colNr.Width = 130;
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
            colLocuri.Width = 64;
            //
            // colStatus
            //
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 110;
            //
            // colItp
            //
            colItp.DataPropertyName = "DataExpirareITP";
            colItp.HeaderText = "ITP până la";
            colItp.Name = "colItp";
            colItp.ReadOnly = true;
            colItp.Width = 100;
            //
            // colActiv
            //
            colActiv.DataPropertyName = "Activ";
            colActiv.HeaderText = "Activ";
            colActiv.Name = "colActiv";
            colActiv.ReadOnly = true;
            colActiv.Width = 60;
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
            lblLista.Text = "Autobuze";
            //
            // flpActiuni
            //
            flpActiuni.AutoSize = true;
            flpActiuni.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActiuni.Controls.Add(btnReincarca);
            flpActiuni.Controls.Add(btnAdauga);
            flpActiuni.Controls.Add(btnModifica);
            flpActiuni.Controls.Add(btnLocuri);
            flpActiuni.Controls.Add(chkInactive);
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
            btnReincarca.TabIndex = 4;
            btnReincarca.Click += btnReincarca_Click;
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
            // btnLocuri
            //
            btnLocuri.Icon = "armchair";
            btnLocuri.Margin = new Padding(8, 0, 0, 0);
            btnLocuri.Name = "btnLocuri";
            btnLocuri.Size = new Size(96, 32);
            btnLocuri.TabIndex = 3;
            btnLocuri.Text = " Locuri";
            btnLocuri.Click += btnLocuri_Click;
            //
            // chkInactive
            //
            chkInactive.AutoSize = true;
            chkInactive.Margin = new Padding(8, 7, 8, 0);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 5;
            chkInactive.Text = "Arată și cele scoase din uz";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            //
            // FrmAdminAutobuze
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Name = "FrmAdminAutobuze";
            Text = "Autobuze";
            Load += FrmAdminAutobuze_Load;
            flpActiuni.ResumeLayout(false);
            flpActiuni.PerformLayout();
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
        private FlowLayoutPanel flpActiuni;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnAdauga;
        private Controale.ButonIcon btnModifica;
        private Controale.ButonIcon btnLocuri;
        private CheckBox chkInactive;
    }
}
