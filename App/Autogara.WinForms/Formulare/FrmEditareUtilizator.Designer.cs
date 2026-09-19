namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareUtilizator
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
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "user";
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
            lblTitlu.Text = "Utilizator nou";
            //
            // lblNumeUtilizator
            //
            lblNumeUtilizator.AutoSize = true;
            lblNumeUtilizator.Location = new Point(24, 68);
            lblNumeUtilizator.Name = "lblNumeUtilizator";
            lblNumeUtilizator.Text = "Nume de utilizator";
            //
            // txtNumeUtilizator
            //
            txtNumeUtilizator.CharacterCasing = CharacterCasing.Lower;
            txtNumeUtilizator.Location = new Point(24, 88);
            txtNumeUtilizator.Name = "txtNumeUtilizator";
            txtNumeUtilizator.Size = new Size(372, 23);
            txtNumeUtilizator.TabIndex = 0;
            //
            // lblNume
            //
            lblNume.AutoSize = true;
            lblNume.Location = new Point(24, 122);
            lblNume.Name = "lblNume";
            lblNume.Text = "Nume";
            //
            // txtNume
            //
            txtNume.Location = new Point(24, 142);
            txtNume.Name = "txtNume";
            txtNume.Size = new Size(178, 23);
            txtNume.TabIndex = 1;
            //
            // lblPrenume
            //
            lblPrenume.AutoSize = true;
            lblPrenume.Location = new Point(218, 122);
            lblPrenume.Name = "lblPrenume";
            lblPrenume.Text = "Prenume";
            //
            // txtPrenume
            //
            txtPrenume.Location = new Point(218, 142);
            txtPrenume.Name = "txtPrenume";
            txtPrenume.Size = new Size(178, 23);
            txtPrenume.TabIndex = 2;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(24, 176);
            lblEmail.Name = "lblEmail";
            lblEmail.Text = "Email (opțional)";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(24, 196);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(372, 23);
            txtEmail.TabIndex = 3;
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
            txtTelefon.Size = new Size(178, 23);
            txtTelefon.TabIndex = 4;
            //
            // lblRol
            //
            lblRol.AutoSize = true;
            lblRol.Location = new Point(218, 230);
            lblRol.Name = "lblRol";
            lblRol.Text = "Rol";
            //
            // cmbRol
            //
            cmbRol.DisplayMember = "Text";
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Location = new Point(218, 250);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(178, 23);
            cmbRol.TabIndex = 5;
            //
            // lblParola
            //
            lblParola.AutoSize = true;
            lblParola.Location = new Point(24, 284);
            lblParola.Name = "lblParola";
            lblParola.Text = "Parola inițială";
            //
            // txtParola
            //
            txtParola.Location = new Point(24, 304);
            txtParola.Name = "txtParola";
            txtParola.Size = new Size(372, 23);
            txtParola.TabIndex = 6;
            txtParola.UseSystemPasswordChar = true;
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(176, 352);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(120, 34);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 7;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(304, 352);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 34);
            btnRenunta.TabIndex = 8;
            btnRenunta.Text = "Renunță";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmEditareUtilizator
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 408);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblNumeUtilizator);
            Controls.Add(txtNumeUtilizator);
            Controls.Add(lblNume);
            Controls.Add(txtNume);
            Controls.Add(lblPrenume);
            Controls.Add(txtPrenume);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblTelefon);
            Controls.Add(txtTelefon);
            Controls.Add(lblRol);
            Controls.Add(cmbRol);
            Controls.Add(lblParola);
            Controls.Add(txtParola);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareUtilizator";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Utilizator";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
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
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
