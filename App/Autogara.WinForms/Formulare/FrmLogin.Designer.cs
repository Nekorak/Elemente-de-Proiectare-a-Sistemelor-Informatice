namespace Autogara.WinForms.Formulare
{
    partial class FrmLogin
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
            iconLogo = new Controale.IconImagine();
            lblTitlu = new Label();
            lblSubtitlu = new Label();
            lblUtilizator = new Label();
            txtUtilizator = new TextBox();
            lblParola = new Label();
            txtParola = new TextBox();
            btnAutentificare = new Controale.ButonIcon();
            lnkAmUitat = new LinkLabel();
            pnlSubsol = new Panel();
            indicatorConexiune = new Controale.IndicatorConexiune();
            lnkConfigurare = new LinkLabel();
            lblVersiune = new Label();
            errorProvider = new ErrorProvider(components);
            toolTip = new ToolTip(components);
            pnlSubsol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconLogo
            //
            iconLogo.Icon = "bus-front";
            iconLogo.Location = new Point(172, 36);
            iconLogo.Name = "iconLogo";
            iconLogo.Size = new Size(56, 56);
            //
            // lblTitlu
            //
            lblTitlu.Font = new Font("Segoe UI Semibold", 18F);
            lblTitlu.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitlu.Location = new Point(0, 100);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Size = new Size(400, 40);
            lblTitlu.Text = "Autogara";
            lblTitlu.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblSubtitlu
            //
            lblSubtitlu.ForeColor = Color.FromArgb(107, 114, 128);
            lblSubtitlu.Location = new Point(0, 140);
            lblSubtitlu.Name = "lblSubtitlu";
            lblSubtitlu.Size = new Size(400, 22);
            lblSubtitlu.Text = "Autentificați-vă pentru a continua";
            lblSubtitlu.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblUtilizator
            //
            lblUtilizator.AutoSize = true;
            lblUtilizator.Location = new Point(48, 186);
            lblUtilizator.Name = "lblUtilizator";
            lblUtilizator.Text = "Nume de utilizator";
            //
            // txtUtilizator
            //
            txtUtilizator.Font = new Font("Segoe UI", 10.5F);
            txtUtilizator.Location = new Point(48, 206);
            txtUtilizator.Name = "txtUtilizator";
            txtUtilizator.Size = new Size(304, 26);
            txtUtilizator.TabIndex = 0;
            //
            // lblParola
            //
            lblParola.AutoSize = true;
            lblParola.Location = new Point(48, 246);
            lblParola.Name = "lblParola";
            lblParola.Text = "Parola";
            //
            // txtParola
            //
            txtParola.Font = new Font("Segoe UI", 10.5F);
            txtParola.Location = new Point(48, 266);
            txtParola.Name = "txtParola";
            txtParola.Size = new Size(304, 26);
            txtParola.TabIndex = 1;
            txtParola.UseSystemPasswordChar = true;
            //
            // btnAutentificare
            //
            btnAutentificare.Font = new Font("Segoe UI Semibold", 10F);
            btnAutentificare.Icon = "log-in";
            btnAutentificare.Location = new Point(48, 316);
            btnAutentificare.Name = "btnAutentificare";
            btnAutentificare.Size = new Size(304, 40);
            btnAutentificare.Stil = Controale.StilButon.Primar;
            btnAutentificare.TabIndex = 2;
            btnAutentificare.Text = " Autentificare";
            btnAutentificare.Click += btnAutentificare_Click;
            //
            // lnkAmUitat
            //
            lnkAmUitat.ActiveLinkColor = Color.FromArgb(29, 78, 216);
            lnkAmUitat.LinkColor = Color.FromArgb(37, 99, 235);
            lnkAmUitat.Location = new Point(48, 368);
            lnkAmUitat.Name = "lnkAmUitat";
            lnkAmUitat.Size = new Size(304, 20);
            lnkAmUitat.TabIndex = 3;
            lnkAmUitat.TabStop = true;
            lnkAmUitat.Text = "Am uitat parola";
            lnkAmUitat.TextAlign = ContentAlignment.MiddleCenter;
            lnkAmUitat.LinkClicked += lnkAmUitat_LinkClicked;
            //
            // pnlSubsol
            //
            pnlSubsol.BackColor = Color.FromArgb(245, 247, 250);
            pnlSubsol.Controls.Add(indicatorConexiune);
            pnlSubsol.Controls.Add(lnkConfigurare);
            pnlSubsol.Controls.Add(lblVersiune);
            pnlSubsol.Dock = DockStyle.Bottom;
            pnlSubsol.Location = new Point(0, 436);
            pnlSubsol.Name = "pnlSubsol";
            pnlSubsol.Size = new Size(400, 44);
            pnlSubsol.TabIndex = 4;
            //
            // indicatorConexiune
            //
            indicatorConexiune.ForeColor = Color.FromArgb(75, 85, 99);
            indicatorConexiune.Location = new Point(14, 10);
            indicatorConexiune.Name = "indicatorConexiune";
            indicatorConexiune.Size = new Size(170, 24);
            //
            // lnkConfigurare
            //
            lnkConfigurare.ActiveLinkColor = Color.FromArgb(29, 78, 216);
            lnkConfigurare.AutoSize = true;
            lnkConfigurare.LinkColor = Color.FromArgb(37, 99, 235);
            lnkConfigurare.Location = new Point(236, 14);
            lnkConfigurare.Name = "lnkConfigurare";
            lnkConfigurare.TabIndex = 0;
            lnkConfigurare.TabStop = true;
            lnkConfigurare.Text = "Setări conexiune";
            lnkConfigurare.LinkClicked += lnkConfigurare_LinkClicked;
            //
            // lblVersiune
            //
            lblVersiune.AutoSize = true;
            lblVersiune.ForeColor = Color.FromArgb(107, 114, 128);
            lblVersiune.Location = new Point(350, 14);
            lblVersiune.Name = "lblVersiune";
            lblVersiune.Text = "v1.0.0";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmLogin
            //
            AcceptButton = btnAutentificare;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(400, 480);
            Controls.Add(iconLogo);
            Controls.Add(lblTitlu);
            Controls.Add(lblSubtitlu);
            Controls.Add(lblUtilizator);
            Controls.Add(txtUtilizator);
            Controls.Add(lblParola);
            Controls.Add(txtParola);
            Controls.Add(btnAutentificare);
            Controls.Add(lnkAmUitat);
            Controls.Add(pnlSubsol);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Autogara — autentificare";
            FormClosed += FrmLogin_FormClosed;
            Load += FrmLogin_Load;
            Shown += FrmLogin_Shown;
            pnlSubsol.ResumeLayout(false);
            pnlSubsol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconLogo;
        private Label lblTitlu;
        private Label lblSubtitlu;
        private Label lblUtilizator;
        private TextBox txtUtilizator;
        private Label lblParola;
        private TextBox txtParola;
        private Controale.ButonIcon btnAutentificare;
        private LinkLabel lnkAmUitat;
        private Panel pnlSubsol;
        private Controale.IndicatorConexiune indicatorConexiune;
        private LinkLabel lnkConfigurare;
        private Label lblVersiune;
        private ErrorProvider errorProvider;
        private ToolTip toolTip;
    }
}
