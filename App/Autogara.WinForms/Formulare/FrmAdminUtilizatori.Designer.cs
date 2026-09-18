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
            components = new System.ComponentModel.Container();
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
            chkInactive = new CheckBox();
            btnNou = new Controale.ButonIcon();
            btnReincarca = new Controale.ButonIcon();
            pnlSpatiuDreapta = new Panel();
            pnlEditare = new Panel();
            lblEditareTitlu = new Label();
            lblNumeUtilizator = new Label();
            txtNumeUtilizator = new TextBox();
            lblNume = new Label();
            txtNume = new TextBox();
            lblPrenume = new Label();
            txtPrenume = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblTelefon = new Label();
            txtTelefon = new TextBox();
            lblRol = new Label();
            cmbRol = new ComboBox();
            lblParola = new Label();
            txtParola = new TextBox();
            btnSalveaza = new Controale.ButonIcon();
            btnReseteaza = new Controale.ButonIcon();
            btnActiv = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlLista.SuspendLayout();
            tabLista.SuspendLayout();
            tpUtilizatori.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLista).BeginInit();
            tpCereri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCereri).BeginInit();
            pnlCereriBara.SuspendLayout();
            pnlListaAntet.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlEditare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlLista
            // 
            pnlLista.Controls.Add(tabLista);
            pnlLista.Controls.Add(pnlListaAntet);
            pnlLista.BackColor = Color.White;
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
            tpUtilizatori.BackColor = Color.White;
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
            colRol.Width = 80;
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
            colTelefon.Width = 120;
            // 
            // colActiv
            // 
            colActiv.DataPropertyName = "Activ";
            colActiv.HeaderText = "Activ";
            colActiv.Name = "colActiv";
            colActiv.ReadOnly = true;
            colActiv.Width = 56;
            // 
            // tpCereri
            // 
            tpCereri.Controls.Add(gridCereri);
            tpCereri.Controls.Add(pnlCereriBara);
            tpCereri.BackColor = Color.White;
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
            pnlListaAntet.Controls.Add(lblLista);
            pnlListaAntet.Controls.Add(chkInactive);
            pnlListaAntet.Controls.Add(btnNou);
            pnlListaAntet.Controls.Add(btnReincarca);
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
            lblLista.Text = "Conturi";
            // 
            // chkInactive
            // 
            chkInactive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkInactive.AutoSize = true;
            chkInactive.Location = new Point(304, 17);
            chkInactive.Name = "chkInactive";
            chkInactive.TabIndex = 4;
            chkInactive.Text = "Arată și inactive";
            chkInactive.CheckedChanged += chkInactive_CheckedChanged;
            // 
            // btnNou
            // 
            btnNou.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNou.Icon = "user-plus";
            btnNou.Location = new Point(454, 10);
            btnNou.Name = "btnNou";
            btnNou.Size = new Size(90, 32);
            btnNou.Stil = Controale.StilButon.Primar;
            btnNou.TabIndex = 5;
            btnNou.Text = " Nou";
            btnNou.Click += btnNou_Click;
            // 
            // btnReincarca
            // 
            btnReincarca.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReincarca.Icon = "refresh-cw";
            btnReincarca.Location = new Point(552, 10);
            btnReincarca.Name = "btnReincarca";
            btnReincarca.Size = new Size(36, 32);
            btnReincarca.TabIndex = 6;
            btnReincarca.Click += btnReincarca_Click;
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
            pnlEditare.Controls.Add(lblNumeUtilizator);
            pnlEditare.Controls.Add(txtNumeUtilizator);
            pnlEditare.Controls.Add(lblNume);
            pnlEditare.Controls.Add(txtNume);
            pnlEditare.Controls.Add(lblPrenume);
            pnlEditare.Controls.Add(txtPrenume);
            pnlEditare.Controls.Add(lblEmail);
            pnlEditare.Controls.Add(txtEmail);
            pnlEditare.Controls.Add(lblTelefon);
            pnlEditare.Controls.Add(txtTelefon);
            pnlEditare.Controls.Add(lblRol);
            pnlEditare.Controls.Add(cmbRol);
            pnlEditare.Controls.Add(lblParola);
            pnlEditare.Controls.Add(txtParola);
            pnlEditare.Controls.Add(btnSalveaza);
            pnlEditare.Controls.Add(btnReseteaza);
            pnlEditare.Controls.Add(btnActiv);
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
            lblEditareTitlu.Text = "Utilizator nou";
            // 
            // lblNumeUtilizator
            // 
            lblNumeUtilizator.AutoSize = true;
            lblNumeUtilizator.Location = new Point(20, 56);
            lblNumeUtilizator.Name = "lblNumeUtilizator";
            lblNumeUtilizator.Text = "Nume de utilizator";
            // 
            // txtNumeUtilizator
            // 
            txtNumeUtilizator.CharacterCasing = CharacterCasing.Lower;
            txtNumeUtilizator.Location = new Point(20, 76);
            txtNumeUtilizator.Name = "txtNumeUtilizator";
            txtNumeUtilizator.Size = new Size(340, 23);
            txtNumeUtilizator.TabIndex = 7;
            // 
            // lblNume
            // 
            lblNume.AutoSize = true;
            lblNume.Location = new Point(20, 112);
            lblNume.Name = "lblNume";
            lblNume.Text = "Nume";
            // 
            // txtNume
            // 
            txtNume.Location = new Point(20, 132);
            txtNume.Name = "txtNume";
            txtNume.Size = new Size(340, 23);
            txtNume.TabIndex = 8;
            // 
            // lblPrenume
            // 
            lblPrenume.AutoSize = true;
            lblPrenume.Location = new Point(20, 168);
            lblPrenume.Name = "lblPrenume";
            lblPrenume.Text = "Prenume";
            // 
            // txtPrenume
            // 
            txtPrenume.Location = new Point(20, 188);
            txtPrenume.Name = "txtPrenume";
            txtPrenume.Size = new Size(340, 23);
            txtPrenume.TabIndex = 9;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(20, 224);
            lblEmail.Name = "lblEmail";
            lblEmail.Text = "Email (opțional)";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(20, 244);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(340, 23);
            txtEmail.TabIndex = 10;
            // 
            // lblTelefon
            // 
            lblTelefon.AutoSize = true;
            lblTelefon.Location = new Point(20, 280);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.Text = "Telefon (opțional)";
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(20, 300);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(340, 23);
            txtTelefon.TabIndex = 11;
            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Location = new Point(20, 336);
            lblRol.Name = "lblRol";
            lblRol.Text = "Rol";
            // 
            // cmbRol
            // 
            cmbRol.DisplayMember = "Text";
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Location = new Point(20, 356);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(160, 23);
            cmbRol.TabIndex = 12;
            // 
            // lblParola
            // 
            lblParola.AutoSize = true;
            lblParola.Location = new Point(20, 392);
            lblParola.Name = "lblParola";
            lblParola.Text = "Parola inițială";
            // 
            // txtParola
            // 
            txtParola.Location = new Point(20, 412);
            txtParola.Name = "txtParola";
            txtParola.Size = new Size(340, 23);
            txtParola.TabIndex = 13;
            txtParola.UseSystemPasswordChar = true;
            // 
            // btnSalveaza
            // 
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(20, 452);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(340, 36);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 14;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // btnReseteaza
            // 
            btnReseteaza.Icon = "key-round";
            btnReseteaza.Location = new Point(20, 496);
            btnReseteaza.Name = "btnReseteaza";
            btnReseteaza.Size = new Size(166, 34);
            btnReseteaza.TabIndex = 15;
            btnReseteaza.Text = " Parolă temporară";
            btnReseteaza.Click += btnReseteaza_Click;
            // 
            // btnActiv
            // 
            btnActiv.Icon = "user-x";
            btnActiv.Location = new Point(194, 496);
            btnActiv.Name = "btnActiv";
            btnActiv.Size = new Size(166, 34);
            btnActiv.Stil = Controale.StilButon.Pericol;
            btnActiv.TabIndex = 16;
            btnActiv.Text = " Dezactivează";
            btnActiv.Click += btnActiv_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmAdminUtilizatori
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlLista);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlEditare);
            Name = "FrmAdminUtilizatori";
            Text = "Utilizatori";
            Load += FrmAdminUtilizatori_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            pnlEditare.ResumeLayout(false);
            pnlEditare.PerformLayout();
            pnlSpatiuDreapta.ResumeLayout(false);
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
        private CheckBox chkInactive;
        private Controale.ButonIcon btnNou;
        private Controale.ButonIcon btnReincarca;
        private Panel pnlSpatiuDreapta;
        private Panel pnlEditare;
        private Label lblEditareTitlu;
        private Label lblNumeUtilizator;
        private TextBox txtNumeUtilizator;
        private Label lblNume;
        private TextBox txtNume;
        private Label lblPrenume;
        private TextBox txtPrenume;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblTelefon;
        private TextBox txtTelefon;
        private Label lblRol;
        private ComboBox cmbRol;
        private Label lblParola;
        private TextBox txtParola;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnReseteaza;
        private Controale.ButonIcon btnActiv;
        private ErrorProvider errorProvider;
    }
}
