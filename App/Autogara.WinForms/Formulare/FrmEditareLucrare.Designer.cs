namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareLucrare
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
            lblTipLucrare = new Label();
            txtTipLucrare = new TextBox();
            lblData = new Label();
            dtpData = new DateTimePicker();
            chkKm = new CheckBox();
            numKm = new NumericUpDown();
            lblObservatii = new Label();
            txtObservatii = new TextBox();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)numKm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "wrench";
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
            lblTitlu.Text = "Lucrare nouă";
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
            // lblTipLucrare
            //
            lblTipLucrare.AutoSize = true;
            lblTipLucrare.Location = new Point(24, 122);
            lblTipLucrare.Name = "lblTipLucrare";
            lblTipLucrare.Text = "Lucrarea efectuată";
            //
            // txtTipLucrare
            //
            txtTipLucrare.Location = new Point(24, 142);
            txtTipLucrare.Name = "txtTipLucrare";
            txtTipLucrare.Size = new Size(372, 23);
            txtTipLucrare.TabIndex = 1;
            //
            // lblData
            //
            lblData.AutoSize = true;
            lblData.Location = new Point(24, 176);
            lblData.Name = "lblData";
            lblData.Text = "Data";
            //
            // dtpData
            //
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(24, 196);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(160, 23);
            dtpData.TabIndex = 2;
            //
            // chkKm
            //
            chkKm.AutoSize = true;
            chkKm.Checked = true;
            chkKm.CheckState = CheckState.Checked;
            chkKm.Location = new Point(212, 174);
            chkKm.Name = "chkKm";
            chkKm.TabIndex = 3;
            chkKm.Text = "Kilometraj";
            chkKm.CheckedChanged += chkKm_CheckedChanged;
            //
            // numKm
            //
            numKm.Location = new Point(212, 196);
            numKm.Maximum = new decimal(new int[] { 5000000, 0, 0, 0 });
            numKm.Name = "numKm";
            numKm.Size = new Size(184, 23);
            numKm.TabIndex = 4;
            numKm.ThousandsSeparator = true;
            //
            // lblObservatii
            //
            lblObservatii.AutoSize = true;
            lblObservatii.Location = new Point(24, 230);
            lblObservatii.Name = "lblObservatii";
            lblObservatii.Text = "Observații (opțional)";
            //
            // txtObservatii
            //
            txtObservatii.Location = new Point(24, 250);
            txtObservatii.Multiline = true;
            txtObservatii.Name = "txtObservatii";
            txtObservatii.Size = new Size(372, 64);
            txtObservatii.TabIndex = 5;
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(176, 336);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(120, 34);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 6;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(304, 336);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 34);
            btnRenunta.TabIndex = 7;
            btnRenunta.Text = "Renunță";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmEditareLucrare
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 392);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblAutobuz);
            Controls.Add(cmbAutobuz);
            Controls.Add(lblTipLucrare);
            Controls.Add(txtTipLucrare);
            Controls.Add(lblData);
            Controls.Add(dtpData);
            Controls.Add(chkKm);
            Controls.Add(numKm);
            Controls.Add(lblObservatii);
            Controls.Add(txtObservatii);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareLucrare";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lucrare nouă";
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
        private Label lblTipLucrare;
        private TextBox txtTipLucrare;
        private Label lblData;
        private DateTimePicker dtpData;
        private CheckBox chkKm;
        private NumericUpDown numKm;
        private Label lblObservatii;
        private TextBox txtObservatii;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
