namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareTraseu
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
            lblOpriri = new Label();
            lstOpriri = new ListBox();
            btnSus = new Controale.ButonIcon();
            btnJos = new Controale.ButonIcon();
            btnScoate = new Controale.ButonIcon();
            cmbStatie = new ComboBox();
            btnAdaugaOprire = new Controale.ButonIcon();
            lblDistanta = new Label();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "route";
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
            lblTitlu.Size = new Size(378, 30);
            lblTitlu.Text = "Traseu nou";
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
            txtDenumire.Size = new Size(412, 23);
            txtDenumire.TabIndex = 0;
            //
            // lblOpriri
            //
            lblOpriri.AutoSize = true;
            lblOpriri.Location = new Point(24, 122);
            lblOpriri.Name = "lblOpriri";
            lblOpriri.Text = "Opririle, în ordine";
            //
            // lstOpriri
            //
            lstOpriri.FormattingEnabled = true;
            lstOpriri.IntegralHeight = false;
            lstOpriri.Location = new Point(24, 142);
            lstOpriri.Name = "lstOpriri";
            lstOpriri.Size = new Size(368, 220);
            lstOpriri.TabIndex = 1;
            //
            // btnSus
            //
            btnSus.Icon = "arrow-up";
            btnSus.Location = new Point(400, 142);
            btnSus.Name = "btnSus";
            btnSus.Size = new Size(36, 32);
            btnSus.TabIndex = 2;
            btnSus.Click += btnSus_Click;
            //
            // btnJos
            //
            btnJos.Icon = "arrow-down";
            btnJos.Location = new Point(400, 180);
            btnJos.Name = "btnJos";
            btnJos.Size = new Size(36, 32);
            btnJos.TabIndex = 3;
            btnJos.Click += btnJos_Click;
            //
            // btnScoate
            //
            btnScoate.Icon = "minus";
            btnScoate.Location = new Point(400, 218);
            btnScoate.Name = "btnScoate";
            btnScoate.Size = new Size(36, 32);
            btnScoate.Stil = Controale.StilButon.Pericol;
            btnScoate.TabIndex = 4;
            btnScoate.Click += btnScoate_Click;
            //
            // cmbStatie
            //
            cmbStatie.DisplayMember = "Text";
            cmbStatie.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatie.Location = new Point(24, 376);
            cmbStatie.Name = "cmbStatie";
            cmbStatie.Size = new Size(308, 23);
            cmbStatie.TabIndex = 5;
            //
            // btnAdaugaOprire
            //
            btnAdaugaOprire.Icon = "plus";
            btnAdaugaOprire.Location = new Point(340, 372);
            btnAdaugaOprire.Name = "btnAdaugaOprire";
            btnAdaugaOprire.Size = new Size(96, 32);
            btnAdaugaOprire.TabIndex = 6;
            btnAdaugaOprire.Text = " Adaugă";
            btnAdaugaOprire.Click += btnAdaugaOprire_Click;
            //
            // lblDistanta
            //
            lblDistanta.AutoSize = true;
            lblDistanta.ForeColor = Color.FromArgb(110, 106, 96);
            lblDistanta.Location = new Point(24, 414);
            lblDistanta.Name = "lblDistanta";
            lblDistanta.Text = "";
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(216, 450);
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
            btnRenunta.Location = new Point(344, 450);
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
            // FrmEditareTraseu
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(460, 506);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblDenumire);
            Controls.Add(txtDenumire);
            Controls.Add(lblOpriri);
            Controls.Add(lstOpriri);
            Controls.Add(btnSus);
            Controls.Add(btnJos);
            Controls.Add(btnScoate);
            Controls.Add(cmbStatie);
            Controls.Add(btnAdaugaOprire);
            Controls.Add(lblDistanta);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareTraseu";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Traseu";
            Load += FrmEditareTraseu_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblDenumire;
        private TextBox txtDenumire;
        private Label lblOpriri;
        private ListBox lstOpriri;
        private Controale.ButonIcon btnSus;
        private Controale.ButonIcon btnJos;
        private Controale.ButonIcon btnScoate;
        private ComboBox cmbStatie;
        private Controale.ButonIcon btnAdaugaOprire;
        private Label lblDistanta;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
