namespace Autogara.WinForms.Formulare
{
    partial class FrmRecuperareParola
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
            lblExplicatie = new Label();
            lblUtilizator = new Label();
            txtUtilizator = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            btnTrimite = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "key-round";
            iconTitlu.Location = new Point(24, 22);
            iconTitlu.Name = "iconTitlu";
            iconTitlu.Size = new Size(26, 26);
            //
            // lblTitlu
            //
            lblTitlu.AutoSize = true;
            lblTitlu.Font = new Font("Bahnschrift SemiBold", 13F);
            lblTitlu.Location = new Point(58, 20);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Text = "Recuperarea parolei";
            //
            // lblExplicatie
            //
            lblExplicatie.ForeColor = Color.FromArgb(110, 106, 96);
            lblExplicatie.Location = new Point(24, 60);
            lblExplicatie.Name = "lblExplicatie";
            lblExplicatie.Size = new Size(372, 40);
            lblExplicatie.Text = "Cererea ajunge la administratori, care vă vor comunica o parolă temporară. Emailul este opțional, pentru verificare.";
            //
            // lblUtilizator
            //
            lblUtilizator.AutoSize = true;
            lblUtilizator.Location = new Point(24, 110);
            lblUtilizator.Name = "lblUtilizator";
            lblUtilizator.Text = "Nume de utilizator";
            //
            // txtUtilizator
            //
            txtUtilizator.Location = new Point(24, 130);
            txtUtilizator.Name = "txtUtilizator";
            txtUtilizator.Size = new Size(352, 23);
            txtUtilizator.TabIndex = 0;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(24, 164);
            lblEmail.Name = "lblEmail";
            lblEmail.Text = "Email (opțional)";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(24, 184);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(352, 23);
            txtEmail.TabIndex = 1;
            //
            // btnTrimite
            //
            btnTrimite.Icon = "mail";
            btnTrimite.Location = new Point(156, 232);
            btnTrimite.Name = "btnTrimite";
            btnTrimite.Size = new Size(120, 32);
            btnTrimite.Stil = Controale.StilButon.Primar;
            btnTrimite.TabIndex = 2;
            btnTrimite.Text = " Trimite";
            btnTrimite.Click += btnTrimite_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(284, 232);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 32);
            btnRenunta.TabIndex = 3;
            btnRenunta.Text = "Renunță";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmRecuperareParola
            //
            AcceptButton = btnTrimite;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(400, 284);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblExplicatie);
            Controls.Add(lblUtilizator);
            Controls.Add(txtUtilizator);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(btnTrimite);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRecuperareParola";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Am uitat parola";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblExplicatie;
        private Label lblUtilizator;
        private TextBox txtUtilizator;
        private Label lblEmail;
        private TextBox txtEmail;
        private Controale.ButonIcon btnTrimite;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
