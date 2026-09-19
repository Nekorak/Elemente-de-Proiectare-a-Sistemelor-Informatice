namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminTrasee
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
            colDenumire = new DataGridViewTextBoxColumn();
            colPlecare = new DataGridViewTextBoxColumn();
            colSosire = new DataGridViewTextBoxColumn();
            colOpriri = new DataGridViewTextBoxColumn();
            colActiv = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            flpActiuni = new FlowLayoutPanel();
            btnReincarca = new Controale.ButonIcon();
            btnAdauga = new Controale.ButonIcon();
            btnModifica = new Controale.ButonIcon();
            btnActiv = new Controale.ButonIcon();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colDenumire, colPlecare, colSosire, colOpriri, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 0;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            gridLista.CellDoubleClick += gridLista_CellDoubleClick;
            //
            // colDenumire
            //
            colDenumire.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDenumire.DataPropertyName = "Denumire";
            colDenumire.FillWeight = 160F;
            colDenumire.HeaderText = "Denumire";
            colDenumire.MinimumWidth = 60;
            colDenumire.Name = "colDenumire";
            colDenumire.ReadOnly = true;
            //
            // colPlecare
            //
            colPlecare.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPlecare.DataPropertyName = "Plecare";
            colPlecare.FillWeight = 100F;
            colPlecare.HeaderText = "Plecare";
            colPlecare.MinimumWidth = 60;
            colPlecare.Name = "colPlecare";
            colPlecare.ReadOnly = true;
            //
            // colSosire
            //
            colSosire.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colSosire.DataPropertyName = "Sosire";
            colSosire.FillWeight = 100F;
            colSosire.HeaderText = "Sosire";
            colSosire.MinimumWidth = 60;
            colSosire.Name = "colSosire";
            colSosire.ReadOnly = true;
            //
            // colOpriri
            //
            colOpriri.DataPropertyName = "NrOpriri";
            colOpriri.HeaderText = "Opriri";
            colOpriri.Name = "colOpriri";
            colOpriri.ReadOnly = true;
            colOpriri.Width = 64;
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
            lblLista.Text = "Trasee";
            //
            // flpActiuni
            //
            flpActiuni.AutoSize = true;
            flpActiuni.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActiuni.Controls.Add(btnReincarca);
            flpActiuni.Controls.Add(btnAdauga);
            flpActiuni.Controls.Add(btnModifica);
            flpActiuni.Controls.Add(btnActiv);
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
            // btnActiv
            //
            btnActiv.Icon = "ban";
            btnActiv.Margin = new Padding(8, 0, 0, 0);
            btnActiv.Name = "btnActiv";
            btnActiv.Size = new Size(130, 32);
            btnActiv.Stil = Controale.StilButon.Pericol;
            btnActiv.TabIndex = 3;
            btnActiv.Text = " Dezactivează";
            btnActiv.Click += btnActiv_Click;
            //
            // chkInactive
            //
            chkInactive.AutoSize = true;
            chkInactive.Margin = new Padding(8, 7, 8, 0);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 5;
            chkInactive.Text = "Arată și inactive";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            //
            // FrmAdminTrasee
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Name = "FrmAdminTrasee";
            Text = "Trasee";
            Load += FrmAdminTrasee_Load;
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
        private DataGridViewTextBoxColumn colDenumire;
        private DataGridViewTextBoxColumn colPlecare;
        private DataGridViewTextBoxColumn colSosire;
        private DataGridViewTextBoxColumn colOpriri;
        private DataGridViewTextBoxColumn colActiv;
        private Panel pnlListaAntet;
        private Label lblLista;
        private FlowLayoutPanel flpActiuni;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnAdauga;
        private Controale.ButonIcon btnModifica;
        private Controale.ButonIcon btnActiv;
        private CheckBox chkInactive;
    }
}
