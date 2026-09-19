namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminStatii
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
            colNume = new DataGridViewTextBoxColumn();
            colAdresa = new DataGridViewTextBoxColumn();
            colPeron = new DataGridViewTextBoxColumn();
            colActiv = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            iconInfo = new Controale.IconImagine();
            lblInfo = new Label();
            flpActiuni = new FlowLayoutPanel();
            btnReincarca = new Controale.ButonIcon();
            btnModifica = new Controale.ButonIcon();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colNume, colAdresa, colPeron, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            gridLista.CellDoubleClick += gridLista_CellDoubleClick;
            //
            // colNume
            //
            colNume.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNume.DataPropertyName = "Nume";
            colNume.FillWeight = 120F;
            colNume.HeaderText = "Stația";
            colNume.MinimumWidth = 60;
            colNume.Name = "colNume";
            colNume.ReadOnly = true;
            //
            // colAdresa
            //
            colAdresa.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colAdresa.DataPropertyName = "Adresa";
            colAdresa.FillWeight = 160F;
            colAdresa.HeaderText = "Adresa";
            colAdresa.MinimumWidth = 60;
            colAdresa.Name = "colAdresa";
            colAdresa.ReadOnly = true;
            //
            // colPeron
            //
            colPeron.DataPropertyName = "Peron";
            colPeron.HeaderText = "Peron";
            colPeron.Name = "colPeron";
            colPeron.ReadOnly = true;
            colPeron.Width = 70;
            //
            // colActiv
            //
            colActiv.DataPropertyName = "Activ";
            colActiv.HeaderText = "Activă";
            colActiv.Name = "colActiv";
            colActiv.ReadOnly = true;
            colActiv.Width = 70;
            //
            // pnlListaAntet
            //
            pnlListaAntet.Controls.Add(flpActiuni);
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(iconInfo);
            pnlListaAntet.Controls.Add(lblInfo);
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
            lblLista.Text = "Stații";
            //
            // iconInfo
            //
            iconInfo.Culoare = Color.FromArgb(110, 106, 96);
            iconInfo.Icon = "info";
            iconInfo.Location = new Point(70, 19);
            iconInfo.Name = "iconInfo";
            iconInfo.Size = new Size(16, 16);
            //
            // lblInfo
            //
            lblInfo.AutoSize = true;
            lblInfo.ForeColor = Color.FromArgb(110, 106, 96);
            lblInfo.Location = new Point(90, 19);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Stațiile noi se adaugă din „Harta”: fiecare nod de tip Stație devine stație.";
            //
            // flpActiuni
            //
            flpActiuni.AutoSize = true;
            flpActiuni.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActiuni.Controls.Add(btnReincarca);
            flpActiuni.Controls.Add(btnModifica);
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
            btnReincarca.TabIndex = 2;
            btnReincarca.Click += btnReincarca_Click;
            //
            // btnModifica
            //
            btnModifica.Icon = "pencil";
            btnModifica.Margin = new Padding(8, 0, 0, 0);
            btnModifica.Name = "btnModifica";
            btnModifica.Size = new Size(110, 32);
            btnModifica.Stil = Controale.StilButon.Primar;
            btnModifica.TabIndex = 1;
            btnModifica.Text = " Modifică";
            btnModifica.Click += btnModifica_Click;
            //
            // chkInactive
            //
            chkInactive.AutoSize = true;
            chkInactive.Margin = new Padding(8, 7, 8, 0);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 3;
            chkInactive.Text = "Arată și inactive";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            //
            // FrmAdminStatii
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Name = "FrmAdminStatii";
            Text = "Stații";
            Load += FrmAdminStatii_Load;
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
        private DataGridViewTextBoxColumn colNume;
        private DataGridViewTextBoxColumn colAdresa;
        private DataGridViewTextBoxColumn colPeron;
        private DataGridViewTextBoxColumn colActiv;
        private Panel pnlListaAntet;
        private Label lblLista;
        private Controale.IconImagine iconInfo;
        private Label lblInfo;
        private FlowLayoutPanel flpActiuni;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnModifica;
        private CheckBox chkInactive;
    }
}
