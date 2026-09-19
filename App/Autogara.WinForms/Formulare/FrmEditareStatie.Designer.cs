namespace Autogara.WinForms.Formulare
{
    partial class FrmEditareStatie
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
            iconTitlu = new Controale.IconImagine();
            lblTitlu = new Label();
            lblNume = new Label();
            txtNume = new TextBox();
            lblAdresa = new Label();
            txtAdresa = new TextBox();
            lblPeron = new Label();
            txtPeron = new TextBox();
            btnSalveaza = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            SuspendLayout();
            //
            // iconTitlu
            //
            iconTitlu.Icon = "map-pin";
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
            lblTitlu.Text = "Modificare stație";
            //
            // lblNume
            //
            lblNume.AutoSize = true;
            lblNume.Location = new Point(24, 68);
            lblNume.Name = "lblNume";
            lblNume.Text = "Stația (se redenumește din „Harta”)";
            //
            // txtNume
            //
            txtNume.Location = new Point(24, 88);
            txtNume.Name = "txtNume";
            txtNume.ReadOnly = true;
            txtNume.Size = new Size(372, 23);
            txtNume.TabStop = false;
            //
            // lblAdresa
            //
            lblAdresa.AutoSize = true;
            lblAdresa.Location = new Point(24, 122);
            lblAdresa.Name = "lblAdresa";
            lblAdresa.Text = "Adresa";
            //
            // txtAdresa
            //
            txtAdresa.Location = new Point(24, 142);
            txtAdresa.Name = "txtAdresa";
            txtAdresa.Size = new Size(372, 23);
            txtAdresa.TabIndex = 0;
            //
            // lblPeron
            //
            lblPeron.AutoSize = true;
            lblPeron.Location = new Point(24, 176);
            lblPeron.Name = "lblPeron";
            lblPeron.Text = "Peron";
            //
            // txtPeron
            //
            txtPeron.Location = new Point(24, 196);
            txtPeron.Name = "txtPeron";
            txtPeron.Size = new Size(120, 23);
            txtPeron.TabIndex = 1;
            //
            // btnSalveaza
            //
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(176, 246);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(120, 34);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 2;
            btnSalveaza.Text = " Salvează";
            btnSalveaza.Click += btnSalveaza_Click;
            //
            // btnRenunta
            //
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(304, 246);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 34);
            btnRenunta.TabIndex = 3;
            btnRenunta.Text = "Renunță";
            //
            // FrmEditareStatie
            //
            AcceptButton = btnSalveaza;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 249, 246);
            CancelButton = btnRenunta;
            ClientSize = new Size(420, 302);
            Controls.Add(iconTitlu);
            Controls.Add(lblTitlu);
            Controls.Add(lblNume);
            Controls.Add(txtNume);
            Controls.Add(lblAdresa);
            Controls.Add(txtAdresa);
            Controls.Add(lblPeron);
            Controls.Add(txtPeron);
            Controls.Add(btnSalveaza);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditareStatie";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Stație";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controale.IconImagine iconTitlu;
        private Label lblTitlu;
        private Label lblNume;
        private TextBox txtNume;
        private Label lblAdresa;
        private TextBox txtAdresa;
        private Label lblPeron;
        private TextBox txtPeron;
        private Controale.ButonIcon btnSalveaza;
        private Controale.ButonIcon btnRenunta;
    }
}
