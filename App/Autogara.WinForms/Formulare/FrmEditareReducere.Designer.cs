namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareReducere
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
            lblDenumire = new Label();
            txtDenumire = new TextBox();
            lblProcent = new Label();
            numProcent = new NumericUpDown();
            chkActiv = new CheckBox();
            lblNota = new Label();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)numProcent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "percent";
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
            lblTitlu.Text = "Reducere nouă";
            //
            // lblDenumire
            //
            lblDenumire.AutoSize = true;
            lblDenumire.Location = new Point(24, 68);
            lblDenumire.Name = "lblDenumire";
            lblDenumire.Text = "Denumire";
            //
            // txtDenumire
            //
            txtDenumire.Location = new Point(24, 88);
            txtDenumire.Name = "txtDenumire";
            txtDenumire.Size = new Size(372, 23);
            txtDenumire.TabIndex = 0;
            //
            // lblProcent
            //
            lblProcent.AutoSize = true;
            lblProcent.Location = new Point(24, 122);
            lblProcent.Name = "lblProcent";
            lblProcent.Text = "Reducere (%)";
            //
            // numProcent
            //
            numProcent.DecimalPlaces = 2;
            numProcent.Location = new Point(24, 142);
            numProcent.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numProcent.Name = "numProcent";
            numProcent.Size = new Size(120, 23);
            numProcent.TabIndex = 1;
            //
            // chkActiv
            //
            chkActiv.AutoSize = true;
            chkActiv.Location = new Point(24, 178);
            chkActiv.Name = "chkActiv";
            chkActiv.TabIndex = 2;
            chkActiv.Text = "Activă (se poate alege la vânzare)";
            //
            // lblNota
            //
            lblNota.AutoSize = true;
            lblNota.ForeColor = Color.FromArgb(110, 106, 96);
            lblNota.Location = new Point(24, 208);
            lblNota.Name = "lblNota";
            lblNota.Text = "Biletele vândute își păstrează prețul;\nprocentul nou se aplică doar de acum.";
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(176, 262);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(120, 34);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 3;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(304, 262);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 34);
            btnRenunta.TabIndex = 4;
            btnRenunta.Text = "Renunță";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmEditareReducere
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 318);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblDenumire);
            Controls.Add(txtDenumire);
            Controls.Add(lblProcent);
            Controls.Add(numProcent);
            Controls.Add(chkActiv);
            Controls.Add(lblNota);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareReducere";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reducere";
            ((System.ComponentModel.ISupportInitialize)numProcent).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblDenumire;
        private TextBox txtDenumire;
        private Label lblProcent;
        private NumericUpDown numProcent;
        private CheckBox chkActiv;
        private Label lblNota;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
