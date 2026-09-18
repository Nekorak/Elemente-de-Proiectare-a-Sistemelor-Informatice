namespace Autogara.WinForms.Formulare
{
    partial class FrmIntrebare
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
            iconIntrebare = new Controale.IconImagine();
            lblIntrebare = new Label();
            txtRaspuns = new TextBox();
            btnOk = new Controale.ButonIcon();
            btnRenunta = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // iconIntrebare
            // 
            iconIntrebare.Icon = "circle-alert";
            iconIntrebare.Location = new Point(24, 24);
            iconIntrebare.Name = "iconIntrebare";
            iconIntrebare.Size = new Size(24, 24);
            // 
            // lblIntrebare
            // 
            lblIntrebare.Location = new Point(58, 24);
            lblIntrebare.Name = "lblIntrebare";
            lblIntrebare.Size = new Size(378, 64);
            lblIntrebare.Text = "Întrebare";
            // 
            // txtRaspuns
            // 
            txtRaspuns.Location = new Point(24, 96);
            txtRaspuns.MaxLength = 150;
            txtRaspuns.Name = "txtRaspuns";
            txtRaspuns.Size = new Size(412, 23);
            txtRaspuns.TabIndex = 0;
            // 
            // btnOk
            // 
            btnOk.Icon = "check";
            btnOk.Location = new Point(216, 140);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(120, 32);
            btnOk.Stil = Controale.StilButon.Primar;
            btnOk.TabIndex = 1;
            btnOk.Text = " Confirmă";
            btnOk.Click += btnOk_Click;
            // 
            // btnRenunta
            // 
            btnRenunta.DialogResult = DialogResult.Cancel;
            btnRenunta.Location = new Point(344, 140);
            btnRenunta.Name = "btnRenunta";
            btnRenunta.Size = new Size(92, 32);
            btnRenunta.TabIndex = 2;
            btnRenunta.Text = "Renunță";
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmIntrebare
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnRenunta;
            ClientSize = new Size(460, 190);
            Controls.Add(iconIntrebare);
            Controls.Add(lblIntrebare);
            Controls.Add(txtRaspuns);
            Controls.Add(btnOk);
            Controls.Add(btnRenunta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmIntrebare";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Întrebare";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Controale.IconImagine iconIntrebare;
        private Label lblIntrebare;
        private TextBox txtRaspuns;
        private Controale.ButonIcon btnOk;
        private Controale.ButonIcon btnRenunta;
        private ErrorProvider errorProvider;
    }
}
