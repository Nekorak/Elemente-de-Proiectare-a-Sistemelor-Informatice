namespace Autogara.WinForms.Formulare
{
    partial class FrmInregistrareItp
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
            lblAutobuz = new Label();
            cmbAutobuz = new ComboBox();
            lblInspectie = new Label();
            dtpInspectie = new DateTimePicker();
            lblExpirare = new Label();
            dtpExpirare = new DateTimePicker();
            chkKm = new CheckBox();
            numKm = new NumericUpDown();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)numKm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "shield-check";
            iconTitlu.Location = new Point(24, 22);
            iconTitlu.Name = "iconTitlu";
            iconTitlu.Size = new Size(26, 26);
            //
            // lblTitlu
            //
            lblTitlu.AutoSize = true;
            lblTitlu.Font = new Font("Bahnschrift SemiBold", 13F);
            lblTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblTitlu.Location = new Point(58, 20);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Text = "ITP nou";
            //
            // lblAutobuz
            //
            lblAutobuz.AutoSize = true;
            lblAutobuz.Location = new Point(24, 68);
            lblAutobuz.Name = "lblAutobuz";
            lblAutobuz.Text = "Autobuz";
            //
            // cmbAutobuz
            //
            cmbAutobuz.DisplayMember = "Text";
            cmbAutobuz.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAutobuz.Location = new Point(24, 88);
            cmbAutobuz.Name = "cmbAutobuz";
            cmbAutobuz.Size = new Size(372, 23);
            cmbAutobuz.TabIndex = 0;
            //
            // lblInspectie
            //
            lblInspectie.AutoSize = true;
            lblInspectie.Location = new Point(24, 122);
            lblInspectie.Name = "lblInspectie";
            lblInspectie.Text = "Data inspecției";
            //
            // dtpInspectie
            //
            dtpInspectie.Format = DateTimePickerFormat.Short;
            dtpInspectie.Location = new Point(24, 142);
            dtpInspectie.Name = "dtpInspectie";
            dtpInspectie.Size = new Size(172, 23);
            dtpInspectie.TabIndex = 1;
            //
            // lblExpirare
            //
            lblExpirare.AutoSize = true;
            lblExpirare.Location = new Point(224, 122);
            lblExpirare.Name = "lblExpirare";
            lblExpirare.Text = "Valabil până la";
            //
            // dtpExpirare
            //
            dtpExpirare.Format = DateTimePickerFormat.Short;
            dtpExpirare.Location = new Point(224, 142);
            dtpExpirare.Name = "dtpExpirare";
            dtpExpirare.Size = new Size(172, 23);
            dtpExpirare.TabIndex = 2;
            //
            // chkKm
            //
            chkKm.AutoSize = true;
            chkKm.Location = new Point(24, 180);
            chkKm.Name = "chkKm";
            chkKm.TabIndex = 3;
            chkKm.Text = "Kilometraj la inspecție";
            chkKm.CheckedChanged += chkKm_CheckedChanged;
            //
            // numKm
            //
            numKm.Enabled = false;
            numKm.Location = new Point(24, 204);
            numKm.Maximum = new decimal(new int[] { 5000000, 0, 0, 0 });
            numKm.Name = "numKm";
            numKm.Size = new Size(172, 23);
            numKm.TabIndex = 4;
            numKm.ThousandsSeparator = true;
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(176, 252);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(120, 34);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 5;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(304, 252);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 34);
            btnRenunta.TabIndex = 6;
            btnRenunta.Text = "Renunță";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmInregistrareItp
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 308);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblAutobuz);
            Controls.Add(cmbAutobuz);
            Controls.Add(lblInspectie);
            Controls.Add(dtpInspectie);
            Controls.Add(lblExpirare);
            Controls.Add(dtpExpirare);
            Controls.Add(chkKm);
            Controls.Add(numKm);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmInregistrareItp";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ITP nou";
            ((System.ComponentModel.ISupportInitialize)numKm).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblAutobuz;
        private ComboBox cmbAutobuz;
        private Label lblInspectie;
        private DateTimePicker dtpInspectie;
        private Label lblExpirare;
        private DateTimePicker dtpExpirare;
        private CheckBox chkKm;
        private NumericUpDown numKm;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
