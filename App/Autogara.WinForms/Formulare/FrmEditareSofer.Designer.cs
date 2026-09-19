namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareSofer
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
            iconTitlu = new Controale.IconImagine();
            lblTitlu = new Label();
            lblNume = new Label();
            txtNume = new TextBox();
            lblPrenume = new Label();
            txtPrenume = new TextBox();
            lblNrPermis = new Label();
            txtNrPermis = new TextBox();
            lblTelefon = new Label();
            txtTelefon = new TextBox();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "id-card";
            iconTitlu.Location = new Point(24, 22);
            iconTitlu.Name = "iconTitlu";
            iconTitlu.Size = new Size(26, 26);
            //
            // lblTitlu
            //
            lblTitlu.AutoEllipsis = true;
            lblTitlu.Font = new Font("Bahnschrift SemiBold", 13F);
            lblTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblTitlu.Location = new Point(58, 20);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Size = new Size(338, 30);
            lblTitlu.Text = "Șofer nou";
            //
            // lblNume
            //
            lblNume.AutoSize = true;
            lblNume.Location = new Point(24, 68);
            lblNume.Name = "lblNume";
            lblNume.Text = "Nume";
            //
            // txtNume
            //
            txtNume.Location = new Point(24, 88);
            txtNume.Name = "txtNume";
            txtNume.Size = new Size(372, 23);
            txtNume.TabIndex = 0;
            //
            // lblPrenume
            //
            lblPrenume.AutoSize = true;
            lblPrenume.Location = new Point(24, 122);
            lblPrenume.Name = "lblPrenume";
            lblPrenume.Text = "Prenume";
            //
            // txtPrenume
            //
            txtPrenume.Location = new Point(24, 142);
            txtPrenume.Name = "txtPrenume";
            txtPrenume.Size = new Size(372, 23);
            txtPrenume.TabIndex = 1;
            //
            // lblNrPermis
            //
            lblNrPermis.AutoSize = true;
            lblNrPermis.Location = new Point(24, 176);
            lblNrPermis.Name = "lblNrPermis";
            lblNrPermis.Text = "Numărul permisului de conducere";
            //
            // txtNrPermis
            //
            txtNrPermis.CharacterCasing = CharacterCasing.Upper;
            txtNrPermis.Location = new Point(24, 196);
            txtNrPermis.Name = "txtNrPermis";
            txtNrPermis.Size = new Size(372, 23);
            txtNrPermis.TabIndex = 2;
            //
            // lblTelefon
            //
            lblTelefon.AutoSize = true;
            lblTelefon.Location = new Point(24, 230);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.Text = "Telefon (opțional)";
            //
            // txtTelefon
            //
            txtTelefon.Location = new Point(24, 250);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(372, 23);
            txtTelefon.TabIndex = 3;
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(176, 300);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(120, 34);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 4;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(304, 300);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 34);
            btnRenunta.TabIndex = 5;
            btnRenunta.Text = "Renunță";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmEditareSofer
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 356);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblNume);
            Controls.Add(txtNume);
            Controls.Add(lblPrenume);
            Controls.Add(txtPrenume);
            Controls.Add(lblNrPermis);
            Controls.Add(txtNrPermis);
            Controls.Add(lblTelefon);
            Controls.Add(txtTelefon);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareSofer";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Șofer";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblNume;
        private TextBox txtNume;
        private Label lblPrenume;
        private TextBox txtPrenume;
        private Label lblNrPermis;
        private TextBox txtNrPermis;
        private Label lblTelefon;
        private TextBox txtTelefon;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
