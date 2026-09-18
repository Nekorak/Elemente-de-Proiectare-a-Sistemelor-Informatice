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
            btnReincarca = new Controale.ButonIcon();
            chkInactive = new CheckBox();
            pnlSpatiuDreapta = new Panel();
            pnlEditare = new Panel();
            lblEditareTitlu = new Label();
            lblNume = new Label();
            txtNume = new TextBox();
            lblAdresa = new Label();
            txtAdresa = new TextBox();
            lblPeron = new Label();
            txtPeron = new TextBox();
            btnSalveaza = new Controale.ButonIcon();
            iconInfo = new Controale.IconImagine();
            lblInfo = new Label();
            pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlEditare.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLista
            // 
            pnlLista.Controls.Add(gridLista);
            pnlLista.Controls.Add(pnlListaAntet);
            pnlLista.BackColor = Color.White;
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
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(btnReincarca);
            pnlListaAntet.Controls.Add(chkInactive);
            pnlListaAntet.Dock = DockStyle.Top;
            pnlListaAntet.Name = "pnlListaAntet";
            pnlListaAntet.Size = new Size(600, 52);
            // 
            // lblLista
            // 
            lblLista.AutoSize = true;
            lblLista.Font = new Font("Segoe UI Semibold", 11F);
            lblLista.ForeColor = Color.FromArgb(31, 41, 55);
            lblLista.Location = new Point(4, 16);
            lblLista.Name = "lblLista";
            lblLista.Text = "Stații";
            // 
            // btnReincarca
            // 
            btnReincarca.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReincarca.Icon = "refresh-cw";
            btnReincarca.Location = new Point(552, 10);
            btnReincarca.Name = "btnReincarca";
            btnReincarca.Size = new Size(36, 32);
            btnReincarca.TabIndex = 1;
            btnReincarca.Click += btnReincarca_Click;
            // 
            // chkInactive
            // 
            chkInactive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkInactive.AutoSize = true;
            chkInactive.Location = new Point(402, 17);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 2;
            chkInactive.Text = "Arată și inactive";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            // 
            // pnlSpatiuDreapta
            // 
            pnlSpatiuDreapta.Dock = DockStyle.Right;
            pnlSpatiuDreapta.Name = "pnlSpatiuDreapta";
            pnlSpatiuDreapta.Size = new Size(16, 680);
            // 
            // pnlEditare
            // 
            pnlEditare.Controls.Add(lblEditareTitlu);
            pnlEditare.Controls.Add(lblNume);
            pnlEditare.Controls.Add(txtNume);
            pnlEditare.Controls.Add(lblAdresa);
            pnlEditare.Controls.Add(txtAdresa);
            pnlEditare.Controls.Add(lblPeron);
            pnlEditare.Controls.Add(txtPeron);
            pnlEditare.Controls.Add(btnSalveaza);
            pnlEditare.Controls.Add(iconInfo);
            pnlEditare.Controls.Add(lblInfo);
            pnlEditare.BackColor = Color.White;
            pnlEditare.Dock = DockStyle.Right;
            pnlEditare.Name = "pnlEditare";
            pnlEditare.Size = new Size(380, 680);
            // 
            // lblEditareTitlu
            // 
            lblEditareTitlu.AutoSize = true;
            lblEditareTitlu.Font = new Font("Segoe UI Semibold", 11F);
            lblEditareTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblEditareTitlu.Location = new Point(20, 18);
            lblEditareTitlu.Name = "lblEditareTitlu";
            lblEditareTitlu.Text = "Detaliile stației";
            // 
            // lblNume
            // 
            lblNume.AutoSize = true;
            lblNume.Location = new Point(20, 56);
            lblNume.Name = "lblNume";
            lblNume.Text = "Stația";
            // 
            // txtNume
            // 
            txtNume.Location = new Point(20, 76);
            txtNume.Name = "txtNume";
            txtNume.ReadOnly = true;
            txtNume.Size = new Size(340, 23);
            txtNume.TabIndex = 3;
            // 
            // lblAdresa
            // 
            lblAdresa.AutoSize = true;
            lblAdresa.Location = new Point(20, 112);
            lblAdresa.Name = "lblAdresa";
            lblAdresa.Text = "Adresa";
            // 
            // txtAdresa
            // 
            txtAdresa.Location = new Point(20, 132);
            txtAdresa.Name = "txtAdresa";
            txtAdresa.Size = new Size(340, 23);
            txtAdresa.TabIndex = 4;
            // 
            // lblPeron
            // 
            lblPeron.AutoSize = true;
            lblPeron.Location = new Point(20, 168);
            lblPeron.Name = "lblPeron";
            lblPeron.Text = "Peron";
            // 
            // txtPeron
            // 
            txtPeron.Location = new Point(20, 188);
            txtPeron.Name = "txtPeron";
            txtPeron.Size = new Size(120, 23);
            txtPeron.TabIndex = 5;
            // 
            // btnSalveaza
            // 
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 232);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(166, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 6;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // iconInfo
            // 
            iconInfo.Culoare = Color.FromArgb(107, 114, 128);
            iconInfo.Icon = "info";
            iconInfo.Location = new Point(20, 288);
            iconInfo.Name = "iconInfo";
            iconInfo.Size = new Size(16, 16);
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.ForeColor = Color.FromArgb(107, 114, 128);
            lblInfo.Location = new Point(42, 287);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Stațiile noi se adaugă din „Harta”:\nfiecare nod de tip Stație devine stație.";
            // 
            // FrmAdminStatii
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Name = "FrmAdminStatii";
            Text = "Stații";
            Load += FrmAdminStatii_Load;
            pnlEditare.ResumeLayout(false);
            pnlEditare.PerformLayout();
            pnlSpatiuDreapta.ResumeLayout(false);
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
        private Controale.ButonIcon btnReincarca;
        private CheckBox chkInactive;
        private Panel pnlSpatiuDreapta;
        private Panel pnlEditare;
        private Label lblEditareTitlu;
        private Label lblNume;
        private TextBox txtNume;
        private Label lblAdresa;
        private TextBox txtAdresa;
        private Label lblPeron;
        private TextBox txtPeron;
        private Controale.ButonIcon btnSalveaza;
        private Controale.IconImagine iconInfo;
        private Label lblInfo;
    }
}
