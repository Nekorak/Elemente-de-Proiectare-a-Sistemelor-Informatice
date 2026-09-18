namespace Autogara.WinForms.Formulare
{
    partial class FrmSetupWizard
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
            pnlAntet = new Panel();
            iconAntet = new Controale.IconImagine();
            lblTitlu = new Label();
            lblSubtitlu = new Label();
            grpSql = new GroupBox();
            lblSqlServer = new Label();
            txtSqlServer = new TextBox();
            lblSqlPort = new Label();
            numSqlPort = new NumericUpDown();
            lblSqlBaza = new Label();
            txtSqlBaza = new TextBox();
            lblSqlUtilizator = new Label();
            txtSqlUtilizator = new TextBox();
            lblSqlParola = new Label();
            txtSqlParola = new TextBox();
            grpSftp = new GroupBox();
            lblSftpHost = new Label();
            txtSftpHost = new TextBox();
            lblSftpPort = new Label();
            numSftpPort = new NumericUpDown();
            lblSftpUtilizator = new Label();
            txtSftpUtilizator = new TextBox();
            lblSftpParola = new Label();
            txtSftpParola = new TextBox();
            lblSftpCale = new Label();
            txtSftpCale = new TextBox();
            grpAplicatie = new GroupBox();
            lblDurata = new Label();
            numDurataRezervare = new NumericUpDown();
            iconSql = new Controale.IconImagine();
            lblStareSql = new Label();
            iconSftp = new Controale.IconImagine();
            lblStareSftp = new Label();
            pnlButoane = new Panel();
            btnImplicite = new Controale.ButonIcon();
            btnTesteaza = new Controale.ButonIcon();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            pnlAntet.SuspendLayout();
            grpSql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSqlPort).BeginInit();
            grpSftp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSftpPort).BeginInit();
            grpAplicatie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDurataRezervare).BeginInit();
            pnlButoane.SuspendLayout();
            SuspendLayout();
            //
            // pnlAntet
            //
            pnlAntet.BackColor = Color.White;
            pnlAntet.Controls.Add(iconAntet);
            pnlAntet.Controls.Add(lblTitlu);
            pnlAntet.Controls.Add(lblSubtitlu);
            pnlAntet.Dock = DockStyle.Top;
            pnlAntet.Location = new Point(0, 0);
            pnlAntet.Name = "pnlAntet";
            pnlAntet.Size = new Size(640, 76);
            pnlAntet.TabIndex = 0;
            //
            // iconAntet
            //
            iconAntet.Icon = "settings";
            iconAntet.Location = new Point(20, 22);
            iconAntet.Name = "iconAntet";
            iconAntet.Size = new Size(32, 32);
            //
            // lblTitlu
            //
            lblTitlu.AutoSize = true;
            lblTitlu.Font = new Font("Segoe UI Semibold", 13F);
            lblTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitlu.Location = new Point(64, 12);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Text = "Configurarea stației de lucru";
            //
            // lblSubtitlu
            //
            lblSubtitlu.AutoSize = true;
            lblSubtitlu.ForeColor = Color.FromArgb(107, 114, 128);
            lblSubtitlu.Location = new Point(66, 44);
            lblSubtitlu.Name = "lblSubtitlu";
            lblSubtitlu.Text = "Datele de conectare la serverul autogării. Se salvează doar pe acest calculator, cu parolele criptate.";
            //
            // grpSql
            //
            grpSql.Controls.Add(lblSqlServer);
            grpSql.Controls.Add(txtSqlServer);
            grpSql.Controls.Add(lblSqlPort);
            grpSql.Controls.Add(numSqlPort);
            grpSql.Controls.Add(lblSqlBaza);
            grpSql.Controls.Add(txtSqlBaza);
            grpSql.Controls.Add(lblSqlUtilizator);
            grpSql.Controls.Add(txtSqlUtilizator);
            grpSql.Controls.Add(lblSqlParola);
            grpSql.Controls.Add(txtSqlParola);
            grpSql.Location = new Point(16, 90);
            grpSql.Name = "grpSql";
            grpSql.Size = new Size(608, 140);
            grpSql.TabIndex = 1;
            grpSql.TabStop = false;
            grpSql.Text = "Baza de date (SQL Server)";
            //
            // lblSqlServer
            //
            lblSqlServer.AutoSize = true;
            lblSqlServer.Location = new Point(16, 32);
            lblSqlServer.Name = "lblSqlServer";
            lblSqlServer.Text = "Server";
            //
            // txtSqlServer
            //
            txtSqlServer.Location = new Point(110, 28);
            txtSqlServer.Name = "txtSqlServer";
            txtSqlServer.Size = new Size(180, 23);
            txtSqlServer.TabIndex = 0;
            //
            // lblSqlPort
            //
            lblSqlPort.AutoSize = true;
            lblSqlPort.Location = new Point(320, 32);
            lblSqlPort.Name = "lblSqlPort";
            lblSqlPort.Text = "Port";
            //
            // numSqlPort
            //
            numSqlPort.Location = new Point(410, 28);
            numSqlPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numSqlPort.Name = "numSqlPort";
            numSqlPort.Size = new Size(90, 23);
            numSqlPort.TabIndex = 1;
            //
            // lblSqlBaza
            //
            lblSqlBaza.AutoSize = true;
            lblSqlBaza.Location = new Point(16, 66);
            lblSqlBaza.Name = "lblSqlBaza";
            lblSqlBaza.Text = "Baza de date";
            //
            // txtSqlBaza
            //
            txtSqlBaza.Location = new Point(110, 62);
            txtSqlBaza.Name = "txtSqlBaza";
            txtSqlBaza.Size = new Size(180, 23);
            txtSqlBaza.TabIndex = 2;
            //
            // lblSqlUtilizator
            //
            lblSqlUtilizator.AutoSize = true;
            lblSqlUtilizator.Location = new Point(320, 66);
            lblSqlUtilizator.Name = "lblSqlUtilizator";
            lblSqlUtilizator.Text = "Utilizator";
            //
            // txtSqlUtilizator
            //
            txtSqlUtilizator.Location = new Point(410, 62);
            txtSqlUtilizator.Name = "txtSqlUtilizator";
            txtSqlUtilizator.Size = new Size(180, 23);
            txtSqlUtilizator.TabIndex = 3;
            //
            // lblSqlParola
            //
            lblSqlParola.AutoSize = true;
            lblSqlParola.Location = new Point(16, 100);
            lblSqlParola.Name = "lblSqlParola";
            lblSqlParola.Text = "Parola";
            //
            // txtSqlParola
            //
            txtSqlParola.Location = new Point(110, 96);
            txtSqlParola.Name = "txtSqlParola";
            txtSqlParola.Size = new Size(180, 23);
            txtSqlParola.TabIndex = 4;
            txtSqlParola.UseSystemPasswordChar = true;
            //
            // grpSftp
            //
            grpSftp.Controls.Add(lblSftpHost);
            grpSftp.Controls.Add(txtSftpHost);
            grpSftp.Controls.Add(lblSftpPort);
            grpSftp.Controls.Add(numSftpPort);
            grpSftp.Controls.Add(lblSftpUtilizator);
            grpSftp.Controls.Add(txtSftpUtilizator);
            grpSftp.Controls.Add(lblSftpParola);
            grpSftp.Controls.Add(txtSftpParola);
            grpSftp.Controls.Add(lblSftpCale);
            grpSftp.Controls.Add(txtSftpCale);
            grpSftp.Location = new Point(16, 242);
            grpSftp.Name = "grpSftp";
            grpSftp.Size = new Size(608, 140);
            grpSftp.TabIndex = 2;
            grpSftp.TabStop = false;
            grpSftp.Text = "File server (SFTP)";
            //
            // lblSftpHost
            //
            lblSftpHost.AutoSize = true;
            lblSftpHost.Location = new Point(16, 32);
            lblSftpHost.Name = "lblSftpHost";
            lblSftpHost.Text = "Server";
            //
            // txtSftpHost
            //
            txtSftpHost.Location = new Point(110, 28);
            txtSftpHost.Name = "txtSftpHost";
            txtSftpHost.Size = new Size(180, 23);
            txtSftpHost.TabIndex = 0;
            //
            // lblSftpPort
            //
            lblSftpPort.AutoSize = true;
            lblSftpPort.Location = new Point(320, 32);
            lblSftpPort.Name = "lblSftpPort";
            lblSftpPort.Text = "Port";
            //
            // numSftpPort
            //
            numSftpPort.Location = new Point(410, 28);
            numSftpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numSftpPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSftpPort.Name = "numSftpPort";
            numSftpPort.Size = new Size(90, 23);
            numSftpPort.TabIndex = 1;
            numSftpPort.Value = new decimal(new int[] { 22, 0, 0, 0 });
            //
            // lblSftpUtilizator
            //
            lblSftpUtilizator.AutoSize = true;
            lblSftpUtilizator.Location = new Point(16, 66);
            lblSftpUtilizator.Name = "lblSftpUtilizator";
            lblSftpUtilizator.Text = "Utilizator";
            //
            // txtSftpUtilizator
            //
            txtSftpUtilizator.Location = new Point(110, 62);
            txtSftpUtilizator.Name = "txtSftpUtilizator";
            txtSftpUtilizator.Size = new Size(180, 23);
            txtSftpUtilizator.TabIndex = 2;
            //
            // lblSftpParola
            //
            lblSftpParola.AutoSize = true;
            lblSftpParola.Location = new Point(320, 66);
            lblSftpParola.Name = "lblSftpParola";
            lblSftpParola.Text = "Parola";
            //
            // txtSftpParola
            //
            txtSftpParola.Location = new Point(410, 62);
            txtSftpParola.Name = "txtSftpParola";
            txtSftpParola.Size = new Size(180, 23);
            txtSftpParola.TabIndex = 3;
            txtSftpParola.UseSystemPasswordChar = true;
            //
            // lblSftpCale
            //
            lblSftpCale.AutoSize = true;
            lblSftpCale.Location = new Point(16, 100);
            lblSftpCale.Name = "lblSftpCale";
            lblSftpCale.Text = "Folder de bază";
            //
            // txtSftpCale
            //
            txtSftpCale.Location = new Point(110, 96);
            txtSftpCale.Name = "txtSftpCale";
            txtSftpCale.Size = new Size(480, 23);
            txtSftpCale.TabIndex = 4;
            //
            // grpAplicatie
            //
            grpAplicatie.Controls.Add(lblDurata);
            grpAplicatie.Controls.Add(numDurataRezervare);
            grpAplicatie.Location = new Point(16, 394);
            grpAplicatie.Name = "grpAplicatie";
            grpAplicatie.Size = new Size(608, 64);
            grpAplicatie.TabIndex = 3;
            grpAplicatie.TabStop = false;
            grpAplicatie.Text = "Aplicație";
            //
            // lblDurata
            //
            lblDurata.AutoSize = true;
            lblDurata.Location = new Point(16, 30);
            lblDurata.Name = "lblDurata";
            lblDurata.Text = "Durata rezervării provizorii (minute)";
            //
            // numDurataRezervare
            //
            numDurataRezervare.Location = new Point(250, 26);
            numDurataRezervare.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            numDurataRezervare.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDurataRezervare.Name = "numDurataRezervare";
            numDurataRezervare.Size = new Size(70, 23);
            numDurataRezervare.TabIndex = 0;
            numDurataRezervare.Value = new decimal(new int[] { 10, 0, 0, 0 });
            //
            // iconSql
            //
            iconSql.Culoare = Color.FromArgb(107, 114, 128);
            iconSql.Icon = "database";
            iconSql.Location = new Point(20, 474);
            iconSql.Name = "iconSql";
            iconSql.Size = new Size(18, 18);
            //
            // lblStareSql
            //
            lblStareSql.AutoSize = true;
            lblStareSql.Location = new Point(44, 475);
            lblStareSql.Name = "lblStareSql";
            lblStareSql.Text = "Baza de date: netestată";
            //
            // iconSftp
            //
            iconSftp.Culoare = Color.FromArgb(107, 114, 128);
            iconSftp.Icon = "server";
            iconSftp.Location = new Point(20, 500);
            iconSftp.Name = "iconSftp";
            iconSftp.Size = new Size(18, 18);
            //
            // lblStareSftp
            //
            lblStareSftp.AutoSize = true;
            lblStareSftp.Location = new Point(44, 501);
            lblStareSftp.Name = "lblStareSftp";
            lblStareSftp.Text = "File server: netestat";
            //
            // pnlButoane
            //
            pnlButoane.BackColor = Color.White;
            pnlButoane.Controls.Add(btnImplicite);
            pnlButoane.Controls.Add(btnTesteaza);
            pnlButoane.Controls.Add(btnSalveaza);
            pnlButoane.Controls.Add(btnRenunta);
            pnlButoane.Dock = DockStyle.Bottom;
            pnlButoane.Location = new Point(0, 536);
            pnlButoane.Name = "pnlButoane";
            pnlButoane.Size = new Size(640, 64);
            pnlButoane.TabIndex = 4;
            //
            // btnImplicite
            //
            btnImplicite.Icon = "rotate-ccw";
            btnImplicite.Location = new Point(16, 16);
            btnImplicite.Name = "btnImplicite";
            btnImplicite.Size = new Size(150, 32);
            btnImplicite.TabIndex = 0;
            btnImplicite.Text = " Valorile echipei";
            btnImplicite.Click += btnImplicite_Click;
            //
            // btnTesteaza
            //
            btnTesteaza.Icon = "wifi";
            btnTesteaza.Location = new Point(176, 16);
            btnTesteaza.Name = "btnTesteaza";
            btnTesteaza.Size = new Size(170, 32);
            btnTesteaza.TabIndex = 1;
            btnTesteaza.Text = " Testează conexiunea";
            btnTesteaza.Click += btnTesteaza_Click;
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(400, 16);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(116, 32);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 2;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Icon = "x";
            btnRenunta.Location = new Point(524, 16);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(100, 32);
            btnRenunta.TabIndex = 3;
            btnRenunta.Text = " Renunță";
            //
            // FrmSetupWizard
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = btnRenunta;
            ClientSize = new Size(640, 600);
            Controls.Add(grpSql);
            Controls.Add(grpSftp);
            Controls.Add(grpAplicatie);
            Controls.Add(iconSql);
            Controls.Add(lblStareSql);
            Controls.Add(iconSftp);
            Controls.Add(lblStareSftp);
            Controls.Add(pnlButoane);
            Controls.Add(pnlAntet);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSetupWizard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Autogara — configurarea stației";
            Load += FrmSetupWizard_Load;
            pnlAntet.ResumeLayout(false);
            pnlAntet.PerformLayout();
            grpSql.ResumeLayout(false);
            grpSql.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSqlPort).EndInit();
            grpSftp.ResumeLayout(false);
            grpSftp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSftpPort).EndInit();
            grpAplicatie.ResumeLayout(false);
            grpAplicatie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDurataRezervare).EndInit();
            pnlButoane.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlAntet;
        private Controale.IconImagine iconAntet;
        private Label lblTitlu;
        private Label lblSubtitlu;
        private GroupBox grpSql;
        private Label lblSqlServer;
        private TextBox txtSqlServer;
        private Label lblSqlPort;
        private NumericUpDown numSqlPort;
        private Label lblSqlBaza;
        private TextBox txtSqlBaza;
        private Label lblSqlUtilizator;
        private TextBox txtSqlUtilizator;
        private Label lblSqlParola;
        private TextBox txtSqlParola;
        private GroupBox grpSftp;
        private Label lblSftpHost;
        private TextBox txtSftpHost;
        private Label lblSftpPort;
        private NumericUpDown numSftpPort;
        private Label lblSftpUtilizator;
        private TextBox txtSftpUtilizator;
        private Label lblSftpParola;
        private TextBox txtSftpParola;
        private Label lblSftpCale;
        private TextBox txtSftpCale;
        private GroupBox grpAplicatie;
        private Label lblDurata;
        private NumericUpDown numDurataRezervare;
        private Controale.IconImagine iconSql;
        private Label lblStareSql;
        private Controale.IconImagine iconSftp;
        private Label lblStareSftp;
        private Panel pnlButoane;
        private Controale.ButonIcon btnImplicite;
        private Controale.ButonIcon btnTesteaza;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
    }
}
