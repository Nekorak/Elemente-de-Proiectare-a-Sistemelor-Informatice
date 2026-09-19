namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareAutobuz
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
            lblNrInmatriculare = new Label();
            txtNrInmatriculare = new TextBox();
            lblModel = new Label();
            txtModel = new TextBox();
            lblCapacitate = new Label();
            numCapacitate = new NumericUpDown();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            chkItp = new CheckBox();
            dtpItp = new DateTimePicker();
            lblFisier = new Label();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)numCapacitate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "bus";
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
            lblTitlu.Text = "Autobuz nou";
            //
            // lblNrInmatriculare
            //
            lblNrInmatriculare.AutoSize = true;
            lblNrInmatriculare.Location = new Point(24, 68);
            lblNrInmatriculare.Name = "lblNrInmatriculare";
            lblNrInmatriculare.Text = "Număr de înmatriculare";
            //
            // txtNrInmatriculare
            //
            txtNrInmatriculare.CharacterCasing = CharacterCasing.Upper;
            txtNrInmatriculare.Location = new Point(24, 88);
            txtNrInmatriculare.Name = "txtNrInmatriculare";
            txtNrInmatriculare.Size = new Size(372, 23);
            txtNrInmatriculare.TabIndex = 0;
            //
            // lblModel
            //
            lblModel.AutoSize = true;
            lblModel.Location = new Point(24, 122);
            lblModel.Name = "lblModel";
            lblModel.Text = "Model";
            //
            // txtModel
            //
            txtModel.Location = new Point(24, 142);
            txtModel.Name = "txtModel";
            txtModel.Size = new Size(372, 23);
            txtModel.TabIndex = 1;
            //
            // lblCapacitate
            //
            lblCapacitate.AutoSize = true;
            lblCapacitate.Location = new Point(24, 176);
            lblCapacitate.Name = "lblCapacitate";
            lblCapacitate.Text = "Număr de locuri";
            //
            // numCapacitate
            //
            numCapacitate.Location = new Point(24, 196);
            numCapacitate.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            numCapacitate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numCapacitate.Name = "numCapacitate";
            numCapacitate.Size = new Size(100, 23);
            numCapacitate.TabIndex = 2;
            numCapacitate.Value = new decimal(new int[] { 20, 0, 0, 0 });
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(212, 176);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Status";
            //
            // cmbStatus
            //
            cmbStatus.DisplayMember = "Text";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Location = new Point(212, 196);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(184, 23);
            cmbStatus.TabIndex = 3;
            //
            // chkItp
            //
            chkItp.AutoSize = true;
            chkItp.Location = new Point(24, 234);
            chkItp.Name = "chkItp";
            chkItp.TabIndex = 4;
            chkItp.Text = "ITP valabil până la";
            chkItp.CheckedChanged += chkItp_CheckedChanged;
            //
            // dtpItp
            //
            dtpItp.Format = DateTimePickerFormat.Short;
            dtpItp.Location = new Point(24, 258);
            dtpItp.Name = "dtpItp";
            dtpItp.Size = new Size(160, 23);
            dtpItp.TabIndex = 5;
            //
            // lblFisier
            //
            lblFisier.ForeColor = Color.FromArgb(110, 106, 96);
            lblFisier.Location = new Point(24, 294);
            lblFisier.Name = "lblFisier";
            lblFisier.Size = new Size(372, 40);
            lblFisier.Text = "";
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(176, 348);
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
            btnRenunta.Location = new Point(304, 348);
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
            // FrmEditareAutobuz
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 404);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblNrInmatriculare);
            Controls.Add(txtNrInmatriculare);
            Controls.Add(lblModel);
            Controls.Add(txtModel);
            Controls.Add(lblCapacitate);
            Controls.Add(numCapacitate);
            Controls.Add(lblStatus);
            Controls.Add(cmbStatus);
            Controls.Add(chkItp);
            Controls.Add(dtpItp);
            Controls.Add(lblFisier);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareAutobuz";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Autobuz";
            ((System.ComponentModel.ISupportInitialize)numCapacitate).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblNrInmatriculare;
        private TextBox txtNrInmatriculare;
        private Label lblModel;
        private TextBox txtModel;
        private Label lblCapacitate;
        private NumericUpDown numCapacitate;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private CheckBox chkItp;
        private DateTimePicker dtpItp;
        private Label lblFisier;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
