namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareCursa
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
            lblTraseu = new Label();
            cmbTraseu = new ComboBox();
            lblAutobuz = new Label();
            cmbAutobuz = new ComboBox();
            lblSofer = new Label();
            cmbSofer = new ComboBox();
            lblData = new Label();
            dtpData = new DateTimePicker();
            lblPlecare = new Label();
            dtpPlecare = new DateTimePicker();
            lblSosire = new Label();
            dtpSosire = new DateTimePicker();
            lblPret = new Label();
            numPret = new NumericUpDown();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)numPret).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "calendar";
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
            lblTitlu.Text = "Cursă nouă";
            //
            // lblTraseu
            //
            lblTraseu.AutoSize = true;
            lblTraseu.Location = new Point(24, 68);
            lblTraseu.Name = "lblTraseu";
            lblTraseu.Text = "Traseu";
            //
            // cmbTraseu
            //
            cmbTraseu.DisplayMember = "Text";
            cmbTraseu.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTraseu.Location = new Point(24, 88);
            cmbTraseu.Name = "cmbTraseu";
            cmbTraseu.Size = new Size(372, 23);
            cmbTraseu.TabIndex = 0;
            //
            // lblAutobuz
            //
            lblAutobuz.AutoSize = true;
            lblAutobuz.Location = new Point(24, 122);
            lblAutobuz.Name = "lblAutobuz";
            lblAutobuz.Text = "Autobuz";
            //
            // cmbAutobuz
            //
            cmbAutobuz.DisplayMember = "Text";
            cmbAutobuz.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAutobuz.Location = new Point(24, 142);
            cmbAutobuz.Name = "cmbAutobuz";
            cmbAutobuz.Size = new Size(372, 23);
            cmbAutobuz.TabIndex = 1;
            //
            // lblSofer
            //
            lblSofer.AutoSize = true;
            lblSofer.Location = new Point(24, 176);
            lblSofer.Name = "lblSofer";
            lblSofer.Text = "Șofer";
            //
            // cmbSofer
            //
            cmbSofer.DisplayMember = "Text";
            cmbSofer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSofer.Location = new Point(24, 196);
            cmbSofer.Name = "cmbSofer";
            cmbSofer.Size = new Size(372, 23);
            cmbSofer.TabIndex = 2;
            //
            // lblData
            //
            lblData.AutoSize = true;
            lblData.Location = new Point(24, 230);
            lblData.Name = "lblData";
            lblData.Text = "Data";
            //
            // dtpData
            //
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(24, 250);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(140, 23);
            dtpData.TabIndex = 3;
            //
            // lblPlecare
            //
            lblPlecare.AutoSize = true;
            lblPlecare.Location = new Point(180, 230);
            lblPlecare.Name = "lblPlecare";
            lblPlecare.Text = "Plecare";
            //
            // dtpPlecare
            //
            dtpPlecare.CustomFormat = "HH:mm";
            dtpPlecare.Format = DateTimePickerFormat.Custom;
            dtpPlecare.Location = new Point(180, 250);
            dtpPlecare.Name = "dtpPlecare";
            dtpPlecare.ShowUpDown = true;
            dtpPlecare.Size = new Size(100, 23);
            dtpPlecare.TabIndex = 4;
            //
            // lblSosire
            //
            lblSosire.AutoSize = true;
            lblSosire.Location = new Point(296, 230);
            lblSosire.Name = "lblSosire";
            lblSosire.Text = "Sosire";
            //
            // dtpSosire
            //
            dtpSosire.CustomFormat = "HH:mm";
            dtpSosire.Format = DateTimePickerFormat.Custom;
            dtpSosire.Location = new Point(296, 250);
            dtpSosire.Name = "dtpSosire";
            dtpSosire.ShowUpDown = true;
            dtpSosire.Size = new Size(100, 23);
            dtpSosire.TabIndex = 5;
            //
            // lblPret
            //
            lblPret.AutoSize = true;
            lblPret.Location = new Point(24, 284);
            lblPret.Name = "lblPret";
            lblPret.Text = "Preț (MDL)";
            //
            // numPret
            //
            numPret.DecimalPlaces = 2;
            numPret.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            numPret.Location = new Point(24, 304);
            numPret.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numPret.Name = "numPret";
            numPret.Size = new Size(140, 23);
            numPret.TabIndex = 6;
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
            // FrmEditareCursa
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 408);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblTraseu);
            Controls.Add(cmbTraseu);
            Controls.Add(lblAutobuz);
            Controls.Add(cmbAutobuz);
            Controls.Add(lblSofer);
            Controls.Add(cmbSofer);
            Controls.Add(lblData);
            Controls.Add(dtpData);
            Controls.Add(lblPlecare);
            Controls.Add(dtpPlecare);
            Controls.Add(lblSosire);
            Controls.Add(dtpSosire);
            Controls.Add(lblPret);
            Controls.Add(numPret);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareCursa";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cursă";
            Load += FrmEditareCursa_Load;
            ((System.ComponentModel.ISupportInitialize)numPret).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblTraseu;
        private ComboBox cmbTraseu;
        private Label lblAutobuz;
        private ComboBox cmbAutobuz;
        private Label lblSofer;
        private ComboBox cmbSofer;
        private Label lblData;
        private DateTimePicker dtpData;
        private Label lblPlecare;
        private DateTimePicker dtpPlecare;
        private Label lblSosire;
        private DateTimePicker dtpSosire;
        private Label lblPret;
        private NumericUpDown numPret;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
