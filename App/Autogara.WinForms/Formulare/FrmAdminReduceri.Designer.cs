namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminReduceri
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
            colProcent = new DataGridViewTextBoxColumn();
            colActiv = new DataGridViewTextBoxColumn();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            flpActiuni = new FlowLayoutPanel();
            btnReincarca = new Controale.ButonIcon();
            btnAdauga = new Controale.ButonIcon();
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
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colDenumire, colProcent, colActiv });
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
            colDenumire.FillWeight = 100F;
            colDenumire.HeaderText = "Denumire";
            colDenumire.MinimumWidth = 60;
            colDenumire.Name = "colDenumire";
            colDenumire.ReadOnly = true;
            //
            // colProcent
            //
            colProcent.DataPropertyName = "ProcentReducere";
            colProcent.HeaderText = "Reducere (%)";
            colProcent.Name = "colProcent";
            colProcent.ReadOnly = true;
            colProcent.Width = 110;
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
            lblLista.Text = "Tipuri de reducere";
            //
            // flpActiuni
            //
            flpActiuni.AutoSize = true;
            flpActiuni.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActiuni.Controls.Add(btnReincarca);
            flpActiuni.Controls.Add(btnAdauga);
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
            btnReincarca.TabIndex = 3;
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
            // chkInactive
            //
            chkInactive.AutoSize = true;
            chkInactive.Margin = new Padding(8, 7, 8, 0);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 4;
            chkInactive.Text = "Arată și inactive";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            //
            // FrmAdminReduceri
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Name = "FrmAdminReduceri";
            Text = "Reduceri";
            Load += FrmAdminReduceri_Load;
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
        private DataGridViewTextBoxColumn colProcent;
        private DataGridViewTextBoxColumn colActiv;
        private Panel pnlListaAntet;
        private Label lblLista;
        private FlowLayoutPanel flpActiuni;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnAdauga;
        private Controale.ButonIcon btnModifica;
        private CheckBox chkInactive;
    }
}
