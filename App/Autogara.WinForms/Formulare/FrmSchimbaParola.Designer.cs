namespace Autogara.WinForms.Formulare
{
    partial class FrmSchimbaParola
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
            lblReguli = new Label();
            lblParolaVeche = new Label();
            txtParolaVeche = new TextBox();
            lblParolaNoua = new Label();
            txtParolaNoua = new TextBox();
            lblConfirmare = new Label();
            txtConfirmare = new TextBox();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "lock";
            iconTitlu.Location = new Point(24, 22);
            iconTitlu.Name = "iconTitlu";
            iconTitlu.Size = new Size(26, 26);
            //
            // lblTitlu
            //
            lblTitlu.AutoSize = true;
            lblTitlu.Font = new Font("Segoe UI Semibold", 13F);
            lblTitlu.Location = new Point(58, 20);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Text = "Schimbarea parolei";
            //
            // lblReguli
            //
            lblReguli.ForeColor = Color.FromArgb(107, 114, 128);
            lblReguli.Location = new Point(24, 58);
            lblReguli.Name = "lblReguli";
            lblReguli.Size = new Size(352, 36);
            lblReguli.Text = "Cel puțin 8 caractere, cu cel puțin o literă și o cifră; fără numele de utilizator.";
            //
            // lblParolaVeche
            //
            lblParolaVeche.AutoSize = true;
            lblParolaVeche.Location = new Point(24, 100);
            lblParolaVeche.Name = "lblParolaVeche";
            lblParolaVeche.Text = "Parola actuală";
            //
            // txtParolaVeche
            //
            txtParolaVeche.Location = new Point(24, 120);
            txtParolaVeche.Name = "txtParolaVeche";
            txtParolaVeche.Size = new Size(352, 23);
            txtParolaVeche.TabIndex = 0;
            txtParolaVeche.UseSystemPasswordChar = true;
            //
            // lblParolaNoua
            //
            lblParolaNoua.AutoSize = true;
            lblParolaNoua.Location = new Point(24, 154);
            lblParolaNoua.Name = "lblParolaNoua";
            lblParolaNoua.Text = "Parola nouă";
            //
            // txtParolaNoua
            //
            txtParolaNoua.Location = new Point(24, 174);
            txtParolaNoua.Name = "txtParolaNoua";
            txtParolaNoua.Size = new Size(352, 23);
            txtParolaNoua.TabIndex = 1;
            txtParolaNoua.UseSystemPasswordChar = true;
            //
            // lblConfirmare
            //
            lblConfirmare.AutoSize = true;
            lblConfirmare.Location = new Point(24, 208);
            lblConfirmare.Name = "lblConfirmare";
            lblConfirmare.Text = "Confirmarea parolei noi";
            //
            // txtConfirmare
            //
            txtConfirmare.Location = new Point(24, 228);
            txtConfirmare.Name = "txtConfirmare";
            txtConfirmare.Size = new Size(352, 23);
            txtConfirmare.TabIndex = 2;
            txtConfirmare.UseSystemPasswordChar = true;
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(156, 276);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(120, 32);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 3;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(284, 276);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 32);
            btnRenunta.TabIndex = 4;
            btnRenunta.Text = "Renunță";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmSchimbaParola
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnRenunta;
            ClientSize = new Size(400, 328);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblReguli);
            Controls.Add(lblParolaVeche);
            Controls.Add(txtParolaVeche);
            Controls.Add(lblParolaNoua);
            Controls.Add(txtParolaNoua);
            Controls.Add(lblConfirmare);
            Controls.Add(txtConfirmare);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSchimbaParola";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Schimbă parola";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblReguli;
        private Label lblParolaVeche;
        private TextBox txtParolaVeche;
        private Label lblParolaNoua;
        private TextBox txtParolaNoua;
        private Label lblConfirmare;
        private TextBox txtConfirmare;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
