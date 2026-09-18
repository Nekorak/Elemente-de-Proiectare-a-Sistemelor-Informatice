namespace Autogara.WinForms.Formulare
{
    partial class FrmMain
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
            pnlMeniu = new Panel();
            flpMeniu = new FlowLayoutPanel();
            btnDashboard = new Controale.ButonIcon();
            btnVanzare = new Controale.ButonIcon();
            btnBilete = new Controale.ButonIcon();
            lblGrupAdministrare = new Label();
            btnCurse = new Controale.ButonIcon();
            btnTrasee = new Controale.ButonIcon();
            btnStatii = new Controale.ButonIcon();
            btnHarta = new Controale.ButonIcon();
            btnAutobuze = new Controale.ButonIcon();
            btnMentenanta = new Controale.ButonIcon();
            btnSoferi = new Controale.ButonIcon();
            btnReduceri = new Controale.ButonIcon();
            btnUtilizatori = new Controale.ButonIcon();
            lblGrupRapoarte = new Label();
            btnRapoarte = new Controale.ButonIcon();
            btnAudit = new Controale.ButonIcon();
            pnlLogo = new Panel();
            iconLogo = new Controale.IconImagine();
            lblAplicatie = new Label();
            lblVersiuneMeniu = new Label();
            pnlAntet = new Panel();
            lblSectiune = new Label();
            indicatorConexiune = new Controale.IndicatorConexiune();
            lblUtilizator = new Label();
            btnSchimbaParola = new Controale.ButonIcon();
            btnDeconectare = new Controale.ButonIcon();
            pnlLinie = new Panel();
            pnlContinut = new Panel();
            toolTip = new ToolTip(components);
            pnlMeniu.SuspendLayout();
            flpMeniu.SuspendLayout();
            pnlLogo.SuspendLayout();
            pnlAntet.SuspendLayout();
            SuspendLayout();
            //
            // pnlMeniu
            //
            pnlMeniu.BackColor = Color.FromArgb(15, 23, 42);
            pnlMeniu.Controls.Add(flpMeniu);
            pnlMeniu.Controls.Add(lblVersiuneMeniu);
            pnlMeniu.Controls.Add(pnlLogo);
            pnlMeniu.Dock = DockStyle.Left;
            pnlMeniu.Location = new Point(0, 0);
            pnlMeniu.Name = "pnlMeniu";
            pnlMeniu.Size = new Size(232, 800);
            pnlMeniu.TabIndex = 2;
            //
            // flpMeniu
            //
            flpMeniu.AutoScroll = true;
            flpMeniu.Controls.Add(btnDashboard);
            flpMeniu.Controls.Add(btnVanzare);
            flpMeniu.Controls.Add(btnBilete);
            flpMeniu.Controls.Add(lblGrupAdministrare);
            flpMeniu.Controls.Add(btnCurse);
            flpMeniu.Controls.Add(btnTrasee);
            flpMeniu.Controls.Add(btnStatii);
            flpMeniu.Controls.Add(btnHarta);
            flpMeniu.Controls.Add(btnAutobuze);
            flpMeniu.Controls.Add(btnMentenanta);
            flpMeniu.Controls.Add(btnSoferi);
            flpMeniu.Controls.Add(btnReduceri);
            flpMeniu.Controls.Add(btnUtilizatori);
            flpMeniu.Controls.Add(lblGrupRapoarte);
            flpMeniu.Controls.Add(btnRapoarte);
            flpMeniu.Controls.Add(btnAudit);
            flpMeniu.Dock = DockStyle.Fill;
            flpMeniu.FlowDirection = FlowDirection.TopDown;
            flpMeniu.Location = new Point(0, 72);
            flpMeniu.Name = "flpMeniu";
            flpMeniu.Padding = new Padding(8, 8, 8, 8);
            flpMeniu.Size = new Size(232, 696);
            flpMeniu.TabIndex = 0;
            flpMeniu.WrapContents = false;
            // 
            // btnDashboard
            // 
            btnDashboard.Font = new Font("Segoe UI", 9.75F);
            btnDashboard.Icon = "layout-dashboard";
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Margin = new Padding(0, 0, 0, 2);
            btnDashboard.MarimeIcon = 18;
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(12, 0, 0, 0);
            btnDashboard.Size = new Size(212, 36);
            btnDashboard.Stil = Controale.StilButon.Meniu;
            btnDashboard.TabIndex = 0;
            btnDashboard.Text = "  Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // btnVanzare
            // 
            btnVanzare.Font = new Font("Segoe UI", 9.75F);
            btnVanzare.Icon = "ticket";
            btnVanzare.ImageAlign = ContentAlignment.MiddleLeft;
            btnVanzare.Margin = new Padding(0, 0, 0, 2);
            btnVanzare.MarimeIcon = 18;
            btnVanzare.Name = "btnVanzare";
            btnVanzare.Padding = new Padding(12, 0, 0, 0);
            btnVanzare.Size = new Size(212, 36);
            btnVanzare.Stil = Controale.StilButon.Meniu;
            btnVanzare.TabIndex = 1;
            btnVanzare.Text = "  Vânzare bilete";
            btnVanzare.TextAlign = ContentAlignment.MiddleLeft;
            btnVanzare.Click += btnVanzare_Click;
            // 
            // btnBilete
            // 
            btnBilete.Font = new Font("Segoe UI", 9.75F);
            btnBilete.Icon = "ticket-check";
            btnBilete.ImageAlign = ContentAlignment.MiddleLeft;
            btnBilete.Margin = new Padding(0, 0, 0, 2);
            btnBilete.MarimeIcon = 18;
            btnBilete.Name = "btnBilete";
            btnBilete.Padding = new Padding(12, 0, 0, 0);
            btnBilete.Size = new Size(212, 36);
            btnBilete.Stil = Controale.StilButon.Meniu;
            btnBilete.TabIndex = 2;
            btnBilete.Text = "  Bilete";
            btnBilete.TextAlign = ContentAlignment.MiddleLeft;
            btnBilete.Click += btnBilete_Click;
            // 
            // lblGrupAdministrare
            // 
            lblGrupAdministrare.Font = new Font("Segoe UI Semibold", 7.5F);
            lblGrupAdministrare.ForeColor = Color.FromArgb(100, 116, 139);
            lblGrupAdministrare.Margin = new Padding(0, 8, 0, 2);
            lblGrupAdministrare.Name = "lblGrupAdministrare";
            lblGrupAdministrare.Padding = new Padding(14, 0, 0, 0);
            lblGrupAdministrare.Size = new Size(212, 22);
            lblGrupAdministrare.Text = "ADMINISTRARE";
            lblGrupAdministrare.TextAlign = ContentAlignment.BottomLeft;
            // 
            // btnCurse
            // 
            btnCurse.Font = new Font("Segoe UI", 9.75F);
            btnCurse.Icon = "calendar";
            btnCurse.ImageAlign = ContentAlignment.MiddleLeft;
            btnCurse.Margin = new Padding(0, 0, 0, 2);
            btnCurse.MarimeIcon = 18;
            btnCurse.Name = "btnCurse";
            btnCurse.Padding = new Padding(12, 0, 0, 0);
            btnCurse.Size = new Size(212, 36);
            btnCurse.Stil = Controale.StilButon.Meniu;
            btnCurse.TabIndex = 4;
            btnCurse.Text = "  Curse";
            btnCurse.TextAlign = ContentAlignment.MiddleLeft;
            btnCurse.Click += btnCurse_Click;
            // 
            // btnTrasee
            // 
            btnTrasee.Font = new Font("Segoe UI", 9.75F);
            btnTrasee.Icon = "route";
            btnTrasee.ImageAlign = ContentAlignment.MiddleLeft;
            btnTrasee.Margin = new Padding(0, 0, 0, 2);
            btnTrasee.MarimeIcon = 18;
            btnTrasee.Name = "btnTrasee";
            btnTrasee.Padding = new Padding(12, 0, 0, 0);
            btnTrasee.Size = new Size(212, 36);
            btnTrasee.Stil = Controale.StilButon.Meniu;
            btnTrasee.TabIndex = 5;
            btnTrasee.Text = "  Trasee";
            btnTrasee.TextAlign = ContentAlignment.MiddleLeft;
            btnTrasee.Click += btnTrasee_Click;
            // 
            // btnStatii
            // 
            btnStatii.Font = new Font("Segoe UI", 9.75F);
            btnStatii.Icon = "map-pin";
            btnStatii.ImageAlign = ContentAlignment.MiddleLeft;
            btnStatii.Margin = new Padding(0, 0, 0, 2);
            btnStatii.MarimeIcon = 18;
            btnStatii.Name = "btnStatii";
            btnStatii.Padding = new Padding(12, 0, 0, 0);
            btnStatii.Size = new Size(212, 36);
            btnStatii.Stil = Controale.StilButon.Meniu;
            btnStatii.TabIndex = 6;
            btnStatii.Text = "  Stații";
            btnStatii.TextAlign = ContentAlignment.MiddleLeft;
            btnStatii.Click += btnStatii_Click;
            // 
            // btnHarta
            // 
            btnHarta.Font = new Font("Segoe UI", 9.75F);
            btnHarta.Icon = "map";
            btnHarta.ImageAlign = ContentAlignment.MiddleLeft;
            btnHarta.Margin = new Padding(0, 0, 0, 2);
            btnHarta.MarimeIcon = 18;
            btnHarta.Name = "btnHarta";
            btnHarta.Padding = new Padding(12, 0, 0, 0);
            btnHarta.Size = new Size(212, 36);
            btnHarta.Stil = Controale.StilButon.Meniu;
            btnHarta.TabIndex = 7;
            btnHarta.Text = "  Harta";
            btnHarta.TextAlign = ContentAlignment.MiddleLeft;
            btnHarta.Click += btnHarta_Click;
            // 
            // btnAutobuze
            // 
            btnAutobuze.Font = new Font("Segoe UI", 9.75F);
            btnAutobuze.Icon = "bus";
            btnAutobuze.ImageAlign = ContentAlignment.MiddleLeft;
            btnAutobuze.Margin = new Padding(0, 0, 0, 2);
            btnAutobuze.MarimeIcon = 18;
            btnAutobuze.Name = "btnAutobuze";
            btnAutobuze.Padding = new Padding(12, 0, 0, 0);
            btnAutobuze.Size = new Size(212, 36);
            btnAutobuze.Stil = Controale.StilButon.Meniu;
            btnAutobuze.TabIndex = 8;
            btnAutobuze.Text = "  Autobuze";
            btnAutobuze.TextAlign = ContentAlignment.MiddleLeft;
            btnAutobuze.Click += btnAutobuze_Click;
            // 
            // btnMentenanta
            // 
            btnMentenanta.Font = new Font("Segoe UI", 9.75F);
            btnMentenanta.Icon = "wrench";
            btnMentenanta.ImageAlign = ContentAlignment.MiddleLeft;
            btnMentenanta.Margin = new Padding(0, 0, 0, 2);
            btnMentenanta.MarimeIcon = 18;
            btnMentenanta.Name = "btnMentenanta";
            btnMentenanta.Padding = new Padding(12, 0, 0, 0);
            btnMentenanta.Size = new Size(212, 36);
            btnMentenanta.Stil = Controale.StilButon.Meniu;
            btnMentenanta.TabIndex = 9;
            btnMentenanta.Text = "  Mentenanță";
            btnMentenanta.TextAlign = ContentAlignment.MiddleLeft;
            btnMentenanta.Click += btnMentenanta_Click;
            // 
            // btnSoferi
            // 
            btnSoferi.Font = new Font("Segoe UI", 9.75F);
            btnSoferi.Icon = "id-card";
            btnSoferi.ImageAlign = ContentAlignment.MiddleLeft;
            btnSoferi.Margin = new Padding(0, 0, 0, 2);
            btnSoferi.MarimeIcon = 18;
            btnSoferi.Name = "btnSoferi";
            btnSoferi.Padding = new Padding(12, 0, 0, 0);
            btnSoferi.Size = new Size(212, 36);
            btnSoferi.Stil = Controale.StilButon.Meniu;
            btnSoferi.TabIndex = 10;
            btnSoferi.Text = "  Șoferi";
            btnSoferi.TextAlign = ContentAlignment.MiddleLeft;
            btnSoferi.Click += btnSoferi_Click;
            // 
            // btnReduceri
            // 
            btnReduceri.Font = new Font("Segoe UI", 9.75F);
            btnReduceri.Icon = "percent";
            btnReduceri.ImageAlign = ContentAlignment.MiddleLeft;
            btnReduceri.Margin = new Padding(0, 0, 0, 2);
            btnReduceri.MarimeIcon = 18;
            btnReduceri.Name = "btnReduceri";
            btnReduceri.Padding = new Padding(12, 0, 0, 0);
            btnReduceri.Size = new Size(212, 36);
            btnReduceri.Stil = Controale.StilButon.Meniu;
            btnReduceri.TabIndex = 11;
            btnReduceri.Text = "  Reduceri";
            btnReduceri.TextAlign = ContentAlignment.MiddleLeft;
            btnReduceri.Click += btnReduceri_Click;
            // 
            // btnUtilizatori
            // 
            btnUtilizatori.Font = new Font("Segoe UI", 9.75F);
            btnUtilizatori.Icon = "users";
            btnUtilizatori.ImageAlign = ContentAlignment.MiddleLeft;
            btnUtilizatori.Margin = new Padding(0, 0, 0, 2);
            btnUtilizatori.MarimeIcon = 18;
            btnUtilizatori.Name = "btnUtilizatori";
            btnUtilizatori.Padding = new Padding(12, 0, 0, 0);
            btnUtilizatori.Size = new Size(212, 36);
            btnUtilizatori.Stil = Controale.StilButon.Meniu;
            btnUtilizatori.TabIndex = 12;
            btnUtilizatori.Text = "  Utilizatori";
            btnUtilizatori.TextAlign = ContentAlignment.MiddleLeft;
            btnUtilizatori.Click += btnUtilizatori_Click;
            // 
            // lblGrupRapoarte
            // 
            lblGrupRapoarte.Font = new Font("Segoe UI Semibold", 7.5F);
            lblGrupRapoarte.ForeColor = Color.FromArgb(100, 116, 139);
            lblGrupRapoarte.Margin = new Padding(0, 8, 0, 2);
            lblGrupRapoarte.Name = "lblGrupRapoarte";
            lblGrupRapoarte.Padding = new Padding(14, 0, 0, 0);
            lblGrupRapoarte.Size = new Size(212, 22);
            lblGrupRapoarte.Text = "RAPOARTE";
            lblGrupRapoarte.TextAlign = ContentAlignment.BottomLeft;
            // 
            // btnRapoarte
            // 
            btnRapoarte.Font = new Font("Segoe UI", 9.75F);
            btnRapoarte.Icon = "chart-column";
            btnRapoarte.ImageAlign = ContentAlignment.MiddleLeft;
            btnRapoarte.Margin = new Padding(0, 0, 0, 2);
            btnRapoarte.MarimeIcon = 18;
            btnRapoarte.Name = "btnRapoarte";
            btnRapoarte.Padding = new Padding(12, 0, 0, 0);
            btnRapoarte.Size = new Size(212, 36);
            btnRapoarte.Stil = Controale.StilButon.Meniu;
            btnRapoarte.TabIndex = 14;
            btnRapoarte.Text = "  Rapoarte";
            btnRapoarte.TextAlign = ContentAlignment.MiddleLeft;
            btnRapoarte.Click += btnRapoarte_Click;
            // 
            // btnAudit
            // 
            btnAudit.Font = new Font("Segoe UI", 9.75F);
            btnAudit.Icon = "scroll-text";
            btnAudit.ImageAlign = ContentAlignment.MiddleLeft;
            btnAudit.Margin = new Padding(0, 0, 0, 2);
            btnAudit.MarimeIcon = 18;
            btnAudit.Name = "btnAudit";
            btnAudit.Padding = new Padding(12, 0, 0, 0);
            btnAudit.Size = new Size(212, 36);
            btnAudit.Stil = Controale.StilButon.Meniu;
            btnAudit.TabIndex = 15;
            btnAudit.Text = "  Jurnal audit";
            btnAudit.TextAlign = ContentAlignment.MiddleLeft;
            btnAudit.Click += btnAudit_Click;
            //
            // pnlLogo
            //
            pnlLogo.Controls.Add(iconLogo);
            pnlLogo.Controls.Add(lblAplicatie);
            pnlLogo.Dock = DockStyle.Top;
            pnlLogo.Location = new Point(0, 0);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(232, 72);
            pnlLogo.TabIndex = 1;
            //
            // iconLogo
            //
            iconLogo.Culoare = Color.FromArgb(96, 165, 250);
            iconLogo.Icon = "bus-front";
            iconLogo.Location = new Point(22, 20);
            iconLogo.Name = "iconLogo";
            iconLogo.Size = new Size(30, 30);
            //
            // lblAplicatie
            //
            lblAplicatie.AutoSize = true;
            lblAplicatie.Font = new Font("Segoe UI Semibold", 14F);
            lblAplicatie.ForeColor = Color.White;
            lblAplicatie.Location = new Point(60, 21);
            lblAplicatie.Name = "lblAplicatie";
            lblAplicatie.Text = "Autogara";
            //
            // lblVersiuneMeniu
            //
            lblVersiuneMeniu.Dock = DockStyle.Bottom;
            lblVersiuneMeniu.ForeColor = Color.FromArgb(100, 116, 139);
            lblVersiuneMeniu.Location = new Point(0, 768);
            lblVersiuneMeniu.Name = "lblVersiuneMeniu";
            lblVersiuneMeniu.Padding = new Padding(22, 0, 0, 0);
            lblVersiuneMeniu.Size = new Size(232, 32);
            lblVersiuneMeniu.Text = "Versiunea 1.0.0";
            lblVersiuneMeniu.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pnlAntet
            //
            pnlAntet.BackColor = Color.White;
            pnlAntet.Controls.Add(lblSectiune);
            pnlAntet.Controls.Add(indicatorConexiune);
            pnlAntet.Controls.Add(lblUtilizator);
            pnlAntet.Controls.Add(btnSchimbaParola);
            pnlAntet.Controls.Add(btnDeconectare);
            pnlAntet.Controls.Add(pnlLinie);
            pnlAntet.Dock = DockStyle.Top;
            pnlAntet.Location = new Point(232, 0);
            pnlAntet.Name = "pnlAntet";
            pnlAntet.Size = new Size(1048, 64);
            pnlAntet.TabIndex = 1;
            //
            // lblSectiune
            //
            lblSectiune.AutoSize = true;
            lblSectiune.Font = new Font("Segoe UI Semibold", 15F);
            lblSectiune.ForeColor = Color.FromArgb(31, 41, 55);
            lblSectiune.Location = new Point(24, 16);
            lblSectiune.Name = "lblSectiune";
            lblSectiune.Text = "Dashboard";
            //
            // indicatorConexiune
            //
            indicatorConexiune.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            indicatorConexiune.ForeColor = Color.FromArgb(75, 85, 99);
            indicatorConexiune.Location = new Point(520, 20);
            indicatorConexiune.Name = "indicatorConexiune";
            indicatorConexiune.Size = new Size(160, 24);
            //
            // lblUtilizator
            //
            lblUtilizator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUtilizator.ForeColor = Color.FromArgb(55, 65, 81);
            lblUtilizator.Location = new Point(688, 12);
            lblUtilizator.Name = "lblUtilizator";
            lblUtilizator.Size = new Size(200, 40);
            lblUtilizator.Text = "Utilizator\nRol";
            lblUtilizator.TextAlign = ContentAlignment.MiddleRight;
            //
            // btnSchimbaParola
            //
            btnSchimbaParola.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSchimbaParola.Icon = "key-round";
            btnSchimbaParola.Location = new Point(896, 14);
            btnSchimbaParola.MarimeIcon = 18;
            btnSchimbaParola.Name = "btnSchimbaParola";
            btnSchimbaParola.Size = new Size(36, 36);
            btnSchimbaParola.Stil = Controale.StilButon.Transparent;
            btnSchimbaParola.TabIndex = 0;
            toolTip.SetToolTip(btnSchimbaParola, "Schimbă parola");
            btnSchimbaParola.Click += btnSchimbaParola_Click;
            //
            // btnDeconectare
            //
            btnDeconectare.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDeconectare.Icon = "log-out";
            btnDeconectare.Location = new Point(936, 14);
            btnDeconectare.Name = "btnDeconectare";
            btnDeconectare.Size = new Size(96, 36);
            btnDeconectare.Stil = Controale.StilButon.Transparent;
            btnDeconectare.TabIndex = 1;
            btnDeconectare.Text = " Ieșire";
            toolTip.SetToolTip(btnDeconectare, "Deconectare");
            btnDeconectare.Click += btnDeconectare_Click;
            //
            // pnlLinie
            //
            pnlLinie.BackColor = Color.FromArgb(217, 222, 229);
            pnlLinie.Dock = DockStyle.Bottom;
            pnlLinie.Location = new Point(0, 63);
            pnlLinie.Name = "pnlLinie";
            pnlLinie.Size = new Size(1048, 1);
            pnlLinie.TabIndex = 2;
            //
            // pnlContinut
            //
            pnlContinut.BackColor = Color.FromArgb(245, 247, 250);
            pnlContinut.Dock = DockStyle.Fill;
            pnlContinut.Location = new Point(232, 64);
            pnlContinut.Name = "pnlContinut";
            pnlContinut.Padding = new Padding(20);
            pnlContinut.Size = new Size(1048, 736);
            pnlContinut.TabIndex = 0;
            //
            // FrmMain
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1280, 800);
            Controls.Add(pnlContinut);
            Controls.Add(pnlAntet);
            Controls.Add(pnlMeniu);
            KeyPreview = true;
            MinimumSize = new Size(1100, 700);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Autogara";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            KeyDown += FrmMain_KeyDown;
            pnlMeniu.ResumeLayout(false);
            flpMeniu.ResumeLayout(false);
            pnlLogo.ResumeLayout(false);
            pnlLogo.PerformLayout();
            pnlAntet.ResumeLayout(false);
            pnlAntet.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMeniu;
        private FlowLayoutPanel flpMeniu;
        private Controale.ButonIcon btnDashboard;
        private Controale.ButonIcon btnVanzare;
        private Controale.ButonIcon btnBilete;
        private Label lblGrupAdministrare;
        private Controale.ButonIcon btnCurse;
        private Controale.ButonIcon btnTrasee;
        private Controale.ButonIcon btnStatii;
        private Controale.ButonIcon btnHarta;
        private Controale.ButonIcon btnAutobuze;
        private Controale.ButonIcon btnMentenanta;
        private Controale.ButonIcon btnSoferi;
        private Controale.ButonIcon btnReduceri;
        private Controale.ButonIcon btnUtilizatori;
        private Label lblGrupRapoarte;
        private Controale.ButonIcon btnRapoarte;
        private Controale.ButonIcon btnAudit;
        private Panel pnlLogo;
        private Controale.IconImagine iconLogo;
        private Label lblAplicatie;
        private Label lblVersiuneMeniu;
        private Panel pnlAntet;
        private Label lblSectiune;
        private Controale.IndicatorConexiune indicatorConexiune;
        private Label lblUtilizator;
        private Controale.ButonIcon btnSchimbaParola;
        private Controale.ButonIcon btnDeconectare;
        private Panel pnlLinie;
        private Panel pnlContinut;
        private ToolTip toolTip;
    }
}
