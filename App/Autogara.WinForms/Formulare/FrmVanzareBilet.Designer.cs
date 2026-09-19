namespace Autogara.WinForms.Formulare
{
    partial class FrmVanzareBilet
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
            pnlHarta = new Panel();
            harta = new Controale.AutobuzSeatMapControl();
            lblLocuriLibere = new Label();
            pnlDreapta = new Panel();
            lblLocTitlu = new Label();
            lblLoc = new Label();
            iconCronometru = new Controale.IconImagine();
            lblCronometru = new Label();
            pnlLinie = new Panel();
            lblNume = new Label();
            txtNume = new TextBox();
            lblTelefon = new Label();
            txtTelefon = new TextBox();
            lblReducere = new Label();
            cmbReducere = new ComboBox();
            lblMetoda = new Label();
            rbNumerar = new RadioButton();
            rbCard = new RadioButton();
            lblBon = new Label();
            txtBon = new TextBox();
            pnlPret = new Panel();
            lblPretText = new Label();
            lblPret = new Label();
            lblPretDetalii = new Label();
            btnConfirma = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            pnlAntet = new Panel();
            iconCursa = new Controale.IconImagine();
            lblTraseu = new Label();
            lblDetalii = new Label();
            timerRezervare = new System.Windows.Forms.Timer(components);
            errorProvider = new ErrorProvider(components);
            pnlHarta.SuspendLayout();
            pnlDreapta.SuspendLayout();
            pnlLinie.SuspendLayout();
            pnlPret.SuspendLayout();
            pnlAntet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlHarta
            // 
            pnlHarta.Controls.Add(harta);
            pnlHarta.Controls.Add(lblLocuriLibere);
            pnlHarta.Dock = DockStyle.Fill;
            pnlHarta.Name = "pnlHarta";
            pnlHarta.Padding = new Padding(16, 16, 16, 16);
            // 
            // harta
            // 
            harta.Dock = DockStyle.Fill;
            harta.Name = "harta";
            harta.LocSelectat += harta_LocSelectat;
            // 
            // lblLocuriLibere
            // 
            lblLocuriLibere.Dock = DockStyle.Top;
            lblLocuriLibere.ForeColor = Color.FromArgb(110, 106, 96);
            lblLocuriLibere.Name = "lblLocuriLibere";
            lblLocuriLibere.Size = new Size(620, 24);
            lblLocuriLibere.Text = "";
            lblLocuriLibere.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlDreapta
            // 
            pnlDreapta.Controls.Add(lblLocTitlu);
            pnlDreapta.Controls.Add(lblLoc);
            pnlDreapta.Controls.Add(iconCronometru);
            pnlDreapta.Controls.Add(lblCronometru);
            pnlDreapta.Controls.Add(pnlLinie);
            pnlDreapta.Controls.Add(lblNume);
            pnlDreapta.Controls.Add(txtNume);
            pnlDreapta.Controls.Add(lblTelefon);
            pnlDreapta.Controls.Add(txtTelefon);
            pnlDreapta.Controls.Add(lblReducere);
            pnlDreapta.Controls.Add(cmbReducere);
            pnlDreapta.Controls.Add(lblMetoda);
            pnlDreapta.Controls.Add(rbNumerar);
            pnlDreapta.Controls.Add(rbCard);
            pnlDreapta.Controls.Add(lblBon);
            pnlDreapta.Controls.Add(txtBon);
            pnlDreapta.Controls.Add(pnlPret);
            pnlDreapta.Controls.Add(btnConfirma);
            pnlDreapta.Controls.Add(btnRenunta);
            pnlDreapta.BackColor = Color.FromArgb(250, 249, 246);
            pnlDreapta.Dock = DockStyle.Right;
            pnlDreapta.Name = "pnlDreapta";
            pnlDreapta.Size = new Size(380, 608);
            // 
            // lblLocTitlu
            // 
            lblLocTitlu.AutoSize = true;
            lblLocTitlu.Font = new Font("Bahnschrift SemiBold", 10F);
            lblLocTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblLocTitlu.Location = new Point(20, 18);
            lblLocTitlu.Name = "lblLocTitlu";
            lblLocTitlu.Text = "Locul ales";
            // 
            // lblLoc
            // 
            lblLoc.AutoSize = true;
            lblLoc.Font = new Font("Bahnschrift SemiBold", 20F);
            lblLoc.ForeColor = Color.FromArgb(30, 91, 70);
            lblLoc.Location = new Point(18, 40);
            lblLoc.Name = "lblLoc";
            lblLoc.Text = "—";
            // 
            // iconCronometru
            // 
            iconCronometru.Culoare = Color.FromArgb(110, 106, 96);
            iconCronometru.Icon = "clock";
            iconCronometru.Location = new Point(20, 88);
            iconCronometru.Name = "iconCronometru";
            iconCronometru.Size = new Size(16, 16);
            // 
            // lblCronometru
            // 
            lblCronometru.AutoSize = true;
            lblCronometru.ForeColor = Color.FromArgb(110, 106, 96);
            lblCronometru.Location = new Point(42, 88);
            lblCronometru.Name = "lblCronometru";
            lblCronometru.Text = "Alegeți un loc liber pe schema autobuzului.";
            // 
            // pnlLinie
            // 
            pnlLinie.BackColor = Color.FromArgb(212, 208, 198);
            pnlLinie.Location = new Point(20, 118);
            pnlLinie.Name = "pnlLinie";
            pnlLinie.Size = new Size(340, 1);
            // 
            // lblNume
            // 
            lblNume.AutoSize = true;
            lblNume.Location = new Point(20, 132);
            lblNume.Name = "lblNume";
            lblNume.Text = "Numele pasagerului";
            // 
            // txtNume
            // 
            txtNume.Location = new Point(20, 152);
            txtNume.Name = "txtNume";
            txtNume.Size = new Size(340, 23);
            txtNume.TabIndex = 0;
            // 
            // lblTelefon
            // 
            lblTelefon.AutoSize = true;
            lblTelefon.Location = new Point(20, 186);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.Text = "Telefon (opțional)";
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(20, 206);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(340, 23);
            txtTelefon.TabIndex = 1;
            // 
            // lblReducere
            // 
            lblReducere.AutoSize = true;
            lblReducere.Location = new Point(20, 240);
            lblReducere.Name = "lblReducere";
            lblReducere.Text = "Reducere";
            // 
            // cmbReducere
            // 
            cmbReducere.DisplayMember = "Text";
            cmbReducere.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReducere.Location = new Point(20, 260);
            cmbReducere.Name = "cmbReducere";
            cmbReducere.Size = new Size(340, 23);
            cmbReducere.TabIndex = 2;
            cmbReducere.SelectedIndexChanged += cmbReducere_SelectedIndexChanged;
            // 
            // lblMetoda
            // 
            lblMetoda.AutoSize = true;
            lblMetoda.Location = new Point(20, 294);
            lblMetoda.Name = "lblMetoda";
            lblMetoda.Text = "Plata";
            // 
            // rbNumerar
            // 
            rbNumerar.AutoSize = true;
            rbNumerar.Checked = true;
            rbNumerar.Location = new Point(20, 314);
            rbNumerar.Name = "rbNumerar";
            rbNumerar.TabIndex = 3;
            rbNumerar.TabStop = true;
            rbNumerar.Text = "Numerar";
            // 
            // rbCard
            // 
            rbCard.AutoSize = true;
            rbCard.Location = new Point(120, 314);
            rbCard.Name = "rbCard";
            rbCard.TabIndex = 4;
            rbCard.Text = "Card";
            // 
            // lblBon
            // 
            lblBon.AutoSize = true;
            lblBon.Location = new Point(20, 346);
            lblBon.Name = "lblBon";
            lblBon.Text = "Nr. bon fiscal (opțional)";
            // 
            // txtBon
            // 
            txtBon.Location = new Point(20, 366);
            txtBon.Name = "txtBon";
            txtBon.Size = new Size(340, 23);
            txtBon.TabIndex = 5;
            // 
            // pnlPret
            // 
            pnlPret.Controls.Add(lblPretText);
            pnlPret.Controls.Add(lblPret);
            pnlPret.Controls.Add(lblPretDetalii);
            pnlPret.BackColor = Color.FromArgb(238, 236, 230);
            pnlPret.Location = new Point(20, 404);
            pnlPret.Name = "pnlPret";
            pnlPret.Size = new Size(340, 70);
            // 
            // lblPretText
            // 
            lblPretText.AutoSize = true;
            lblPretText.ForeColor = Color.FromArgb(110, 106, 96);
            lblPretText.Location = new Point(12, 8);
            lblPretText.Name = "lblPretText";
            lblPretText.Text = "Total de plată";
            // 
            // lblPret
            // 
            lblPret.AutoSize = true;
            lblPret.Font = new Font("Bahnschrift SemiBold", 16F);
            lblPret.ForeColor = Color.FromArgb(34, 34, 31);
            lblPret.Location = new Point(10, 28);
            lblPret.Name = "lblPret";
            lblPret.Text = "—";
            // 
            // lblPretDetalii
            // 
            lblPretDetalii.ForeColor = Color.FromArgb(110, 106, 96);
            lblPretDetalii.Location = new Point(150, 36);
            lblPretDetalii.Name = "lblPretDetalii";
            lblPretDetalii.Size = new Size(180, 24);
            lblPretDetalii.Text = "";
            lblPretDetalii.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnConfirma
            // 
            btnConfirma.Font = new Font("Bahnschrift SemiBold", 10F);
            btnConfirma.Icon = "check";
            btnConfirma.Location = new Point(20, 490);
            btnConfirma.Name = "btnConfirma";
            btnConfirma.Size = new Size(340, 40);
            btnConfirma.Stil = Controale.StilButon.Primar;
            btnConfirma.TabIndex = 6;
            btnConfirma.Text = " Confirmă vânzarea (F2)";
            btnConfirma.Click += btnConfirma_Click;
            // 
            // btnRenunta
            // 
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Icon = "x";
            btnRenunta.Location = new Point(20, 538);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(340, 32);
            btnRenunta.TabIndex = 7;
            btnRenunta.Text = " Închide";
            // 
            // pnlAntet
            // 
            pnlAntet.Controls.Add(iconCursa);
            pnlAntet.Controls.Add(lblTraseu);
            pnlAntet.Controls.Add(lblDetalii);
            pnlAntet.BackColor = Color.FromArgb(250, 249, 246);
            pnlAntet.Dock = DockStyle.Top;
            pnlAntet.Name = "pnlAntet";
            pnlAntet.Size = new Size(1100, 72);
            // 
            // iconCursa
            // 
            iconCursa.Icon = "bus";
            iconCursa.Location = new Point(20, 20);
            iconCursa.Name = "iconCursa";
            iconCursa.Size = new Size(30, 30);
            // 
            // lblTraseu
            // 
            lblTraseu.AutoSize = true;
            lblTraseu.Font = new Font("Bahnschrift SemiBold", 14F);
            lblTraseu.ForeColor = Color.FromArgb(34, 34, 31);
            lblTraseu.Location = new Point(62, 12);
            lblTraseu.Name = "lblTraseu";
            lblTraseu.Text = "Traseu";
            // 
            // lblDetalii
            // 
            lblDetalii.AutoSize = true;
            lblDetalii.ForeColor = Color.FromArgb(110, 106, 96);
            lblDetalii.Location = new Point(64, 44);
            lblDetalii.Name = "lblDetalii";
            lblDetalii.Text = "";
            // 
            // timerRezervare
            // 
            timerRezervare.Interval = 1000;
            timerRezervare.Tick += timerRezervare_Tick;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmVanzareBilet
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            CancelButton = btnRenunta;
            ClientSize = new Size(1100, 680);
            Controls.Add(pnlHarta);
            Controls.Add(pnlDreapta);
            Controls.Add(pnlAntet);
            KeyPreview = true;
            MinimizeBox = false;
            MinimumSize = new Size(1000, 700);
            Name = "FrmVanzareBilet";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Vânzare bilet";
            FormClosing += FrmVanzareBilet_FormClosing;
            KeyDown += FrmVanzareBilet_KeyDown;
            Load += FrmVanzareBilet_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            pnlAntet.ResumeLayout(false);
            pnlAntet.PerformLayout();
            pnlPret.ResumeLayout(false);
            pnlPret.PerformLayout();
            pnlLinie.ResumeLayout(false);
            pnlDreapta.ResumeLayout(false);
            pnlDreapta.PerformLayout();
            pnlHarta.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHarta;
        private Controale.AutobuzSeatMapControl harta;
        private Label lblLocuriLibere;
        private Panel pnlDreapta;
        private Label lblLocTitlu;
        private Label lblLoc;
        private Controale.IconImagine iconCronometru;
        private Label lblCronometru;
        private Panel pnlLinie;
        private Label lblNume;
        private TextBox txtNume;
        private Label lblTelefon;
        private TextBox txtTelefon;
        private Label lblReducere;
        private ComboBox cmbReducere;
        private Label lblMetoda;
        private RadioButton rbNumerar;
        private RadioButton rbCard;
        private Label lblBon;
        private TextBox txtBon;
        private Panel pnlPret;
        private Label lblPretText;
        private Label lblPret;
        private Label lblPretDetalii;
        private Controale.ButonIcon btnConfirma;
        private Controale.ButonIcon btnRenunta;
        private Panel pnlAntet;
        private Controale.IconImagine iconCursa;
        private Label lblTraseu;
        private Label lblDetalii;
        private System.Windows.Forms.Timer timerRezervare;
        private ErrorProvider errorProvider;
    }
}
