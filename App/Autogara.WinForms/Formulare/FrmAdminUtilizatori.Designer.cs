namespace Autogara.WinForms.Formulare
{
    partial class FrmAdminUtilizatori
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
            tpUtilizatori = new TabPage();
            gridLista = new Controale.GridAutogara();
            colNumeUtilizator = new DataGridViewTextBoxColumn();
            colNume = new DataGridViewTextBoxColumn();
            colRol = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colTelefon = new DataGridViewTextBoxColumn();
            colActiv = new DataGridViewTextBoxColumn();
            tpCereri = new TabPage();
            gridCereri = new Controale.GridAutogara();
            colCerereUtilizator = new DataGridViewTextBoxColumn();
            colCerereData = new DataGridViewTextBoxColumn();
            pnlCereriBara = new Panel();
            btnReseteazaCerere = new Controale.ButonIcon();
            pnlListaAntet = new Panel();
            lblLista = new Label();
            flpActiuni = new FlowLayoutPanel();
            btnReincarca = new Controale.ButonIcon();
            btnAdauga = new Controale.ButonIcon();
            btnModifica = new Controale.ButonIcon();
            btnReseteaza = new Controale.ButonIcon();
            btnActiv = new Controale.ButonIcon();
            chkInactive = new CheckBox();
            pnlLista.SuspendLayout();
            tabLista.SuspendLayout();
            tpUtilizatori.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            tpCereri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCereri).BeginInit();
            pnlCereriBara.SuspendLayout();
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
            tabLista.Controls.Add(tpUtilizatori);
            tabLista.Controls.Add(tpCereri);
            tabLista.Dock = DockStyle.Fill;
            tabLista.Name = "tabLista";
            tabLista.TabIndex = 0;
            //
            // tpUtilizatori
            //
            tpUtilizatori.Controls.Add(gridLista);
            tpUtilizatori.BackColor = Color.FromArgb(250, 249, 246);
            tpUtilizatori.Name = "tpUtilizatori";
            tpUtilizatori.Padding = new Padding(0, 8, 0, 0);
            tpUtilizatori.Text = "Utilizatori";
            tpUtilizatori.UseVisualStyleBackColor = false;
            //
            // gridLista
            //
            gridLista.Columns.AddRange(new DataGridViewColumn[] { colNumeUtilizator, colNume, colRol, colEmail, colTelefon, colActiv });
            gridLista.Dock = DockStyle.Fill;
            gridLista.Name = "gridLista";
            gridLista.TabIndex = 1;
            gridLista.SelectionChanged += gridLista_SelectionChanged;
            gridLista.CellDoubleClick += gridLista_CellDoubleClick;
            //
            // colNumeUtilizator
            //
            colNumeUtilizator.DataPropertyName = "NumeUtilizator";
            colNumeUtilizator.HeaderText = "Utilizator";
            colNumeUtilizator.Name = "colNumeUtilizator";
            colNumeUtilizator.ReadOnly = true;
            colNumeUtilizator.Width = 140;
            //
            // colNume
            //
            colNume.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNume.DataPropertyName = "NumeComplet";
            colNume.FillWeight = 120F;
            colNume.HeaderText = "Nume";
            colNume.MinimumWidth = 60;
            colNume.Name = "colNume";
            colNume.ReadOnly = true;
            //
            // colRol
            //
            colRol.DataPropertyName = "Rol";
            colRol.HeaderText = "Rol";
            colRol.Name = "colRol";
            colRol.ReadOnly = true;
            colRol.Width = 90;
            //
            // colEmail
            //
            colEmail.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colEmail.DataPropertyName = "Email";
            colEmail.FillWeight = 130F;
            colEmail.HeaderText = "Email";
            colEmail.MinimumWidth = 60;
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            //
            // colTelefon
            //
            colTelefon.DataPropertyName = "Telefon";
            colTelefon.HeaderText = "Telefon";
            colTelefon.Name = "colTelefon";
            colTelefon.ReadOnly = true;
            colTelefon.Width = 130;
            //
            // colActiv
            //
            colActiv.DataPropertyName = "Activ";
            colActiv.HeaderText = "Activ";
            colActiv.Name = "colActiv";
            colActiv.ReadOnly = true;
            colActiv.Width = 60;
            //
            // tpCereri
            //
            tpCereri.Controls.Add(gridCereri);
            tpCereri.Controls.Add(pnlCereriBara);
            tpCereri.BackColor = Color.FromArgb(250, 249, 246);
            tpCereri.Name = "tpCereri";
            tpCereri.Padding = new Padding(0, 8, 0, 0);
            tpCereri.Text = "Cereri de resetare";
            tpCereri.UseVisualStyleBackColor = false;
            //
            // gridCereri
            //
            gridCereri.Columns.AddRange(new DataGridViewColumn[] { colCerereUtilizator, colCerereData });
            gridCereri.Dock = DockStyle.Fill;
            gridCereri.Name = "gridCereri";
            gridCereri.TabIndex = 2;
            //
            // colCerereUtilizator
            //
            colCerereUtilizator.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colCerereUtilizator.DataPropertyName = "NumeUtilizator";
            colCerereUtilizator.FillWeight = 100F;
            colCerereUtilizator.HeaderText = "Utilizator";
            colCerereUtilizator.MinimumWidth = 60;
            colCerereUtilizator.Name = "colCerereUtilizator";
            colCerereUtilizator.ReadOnly = true;
            //
            // colCerereData
            //
            colCerereData.DataPropertyName = "DataCerere";
            colCerereData.HeaderText = "Cerută la";
            colCerereData.Name = "colCerereData";
            colCerereData.ReadOnly = true;
            colCerereData.Width = 150;
            //
            // pnlCereriBara
            //
            pnlCereriBara.Controls.Add(btnReseteazaCerere);
            pnlCereriBara.Dock = DockStyle.Bottom;
            pnlCereriBara.Name = "pnlCereriBara";
            pnlCereriBara.Size = new Size(560, 48);
            //
            // btnReseteazaCerere
            //
            btnReseteazaCerere.Icon = "key-round";
            btnReseteazaCerere.Location = new Point(0, 10);
            btnReseteazaCerere.Name = "btnReseteazaCerere";
            btnReseteazaCerere.Size = new Size(230, 32);
            btnReseteazaCerere.Stil = Controale.StilButon.Primar;
            btnReseteazaCerere.TabIndex = 3;
            btnReseteazaCerere.Text = " Generează parolă temporară";
            btnReseteazaCerere.Click += btnReseteazaCerere_Click;
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
            lblLista.Text = "Conturi";
            //
            // flpActiuni
            //
            flpActiuni.AutoSize = true;
            flpActiuni.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActiuni.Controls.Add(btnReincarca);
            flpActiuni.Controls.Add(btnAdauga);
            flpActiuni.Controls.Add(btnModifica);
            flpActiuni.Controls.Add(btnReseteaza);
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
            btnReincarca.TabIndex = 9;
            btnReincarca.Click += btnReincarca_Click;
            //
            // btnAdauga
            //
            btnAdauga.Icon = "user-plus";
            btnAdauga.Margin = new Padding(8, 0, 0, 0);
            btnAdauga.Name = "btnAdauga";
            btnAdauga.Size = new Size(104, 32);
            btnAdauga.Stil = Controale.StilButon.Primar;
            btnAdauga.TabIndex = 4;
            btnAdauga.Text = " Adaugă";
            btnAdauga.Click += btnAdauga_Click;
            //
            // btnModifica
            //
            btnModifica.Icon = "pencil";
            btnModifica.Margin = new Padding(8, 0, 0, 0);
            btnModifica.Name = "btnModifica";
            btnModifica.Size = new Size(110, 32);
            btnModifica.TabIndex = 5;
            btnModifica.Text = " Modifică";
            btnModifica.Click += btnModifica_Click;
            //
            // btnReseteaza
            //
            btnReseteaza.Icon = "key-round";
            btnReseteaza.Margin = new Padding(8, 0, 0, 0);
            btnReseteaza.Name = "btnReseteaza";
            btnReseteaza.Size = new Size(156, 32);
            btnReseteaza.TabIndex = 6;
            btnReseteaza.Text = " Parolă temporară";
            btnReseteaza.Click += btnReseteaza_Click;
            //
            // btnActiv
            //
            btnActiv.Icon = "user-x";
            btnActiv.Margin = new Padding(8, 0, 0, 0);
            btnActiv.Name = "btnActiv";
            btnActiv.Size = new Size(130, 32);
            btnActiv.Stil = Controale.StilButon.Pericol;
            btnActiv.TabIndex = 7;
            btnActiv.Text = " Dezactivează";
            btnActiv.Click += btnActiv_Click;
            //
            // chkInactive
            //
            chkInactive.AutoSize = true;
            chkInactive.Margin = new Padding(8, 7, 8, 0);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 8;
            chkInactive.Text = "Arată și inactive";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            //
            // FrmAdminUtilizatori
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Name = "FrmAdminUtilizatori";
            Text = "Utilizatori";
            Load += FrmAdminUtilizatori_Load;
            flpActiuni.ResumeLayout(false);
            flpActiuni.PerformLayout();
            pnlListaAntet.ResumeLayout(false);
            pnlListaAntet.PerformLayout();
            pnlCereriBara.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridCereri).EndInit();
            tpCereri.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridLista).EndInit();
            tpUtilizatori.ResumeLayout(false);
            tabLista.ResumeLayout(false);
            pnlLista.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLista;
        private TabControl tabLista;
        private TabPage tpUtilizatori;
        private Controale.GridAutogara gridLista;
        private DataGridViewTextBoxColumn colNumeUtilizator;
        private DataGridViewTextBoxColumn colNume;
        private DataGridViewTextBoxColumn colRol;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colTelefon;
        private DataGridViewTextBoxColumn colActiv;
        private TabPage tpCereri;
        private Controale.GridAutogara gridCereri;
        private DataGridViewTextBoxColumn colCerereUtilizator;
        private DataGridViewTextBoxColumn colCerereData;
        private Panel pnlCereriBara;
        private Controale.ButonIcon btnReseteazaCerere;
        private Panel pnlListaAntet;
        private Label lblLista;
        private FlowLayoutPanel flpActiuni;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnAdauga;
        private Controale.ButonIcon btnModifica;
        private Controale.ButonIcon btnReseteaza;
        private Controale.ButonIcon btnActiv;
        private CheckBox chkInactive;
    }
}
