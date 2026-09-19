namespace Autogara.WinForms.Formulare
{
    partial class FrmEditorHarta
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
            pnlHarta = new Panel();
            canvas = new Controale.HartaCanvasControl();
            pnlStareHarta = new Panel();
            lblStare = new Label();
            lblNumarare = new Label();
            lblSugestie = new Label();
            pnlSpatiuDreapta = new Panel();
            pnlProprietati = new Panel();
            lblProprietatiTitlu = new Label();
            lblFaraSelectie = new Label();
            pnlNod = new Panel();
            lblNume = new Label();
            txtNume = new TextBox();
            lblTip = new Label();
            cmbTip = new ComboBox();
            lblPozitie = new Label();
            btnAplicaNod = new Controale.ButonIcon();
            pnlConexiune = new Panel();
            lblCapete = new Label();
            lblDistanta = new Label();
            numDistanta = new NumericUpDown();
            btnAplicaConexiune = new Controale.ButonIcon();
            btnStergeSelectia = new Controale.ButonIcon();
            pnlLinie = new Panel();
            lblAjutorTitlu = new Label();
            lblAjutor = new Label();
            pnlSpatiu = new Panel();
            pnlUnelte = new Panel();
            btnModSelectare = new Controale.ButonIcon();
            btnModStatie = new Controale.ButonIcon();
            btnModIntersectie = new Controale.ButonIcon();
            btnModConexiune = new Controale.ButonIcon();
            btnModSterge = new Controale.ButonIcon();
            pnlSeparator = new Panel();
            btnPotriveste = new Controale.ButonIcon();
            btnReincarca = new Controale.ButonIcon();
            btnSalveaza = new Controale.ButonIcon();
            errorProvider = new ErrorProvider(components);
            pnlHarta.SuspendLayout();
            pnlStareHarta.SuspendLayout();
            pnlSpatiuDreapta.SuspendLayout();
            pnlProprietati.SuspendLayout();
            pnlNod.SuspendLayout();
            pnlConexiune.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDistanta).BeginInit();
            pnlLinie.SuspendLayout();
            pnlSpatiu.SuspendLayout();
            pnlUnelte.SuspendLayout();
            pnlSeparator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlHarta
            // 
            pnlHarta.Controls.Add(canvas);
            pnlHarta.Controls.Add(pnlStareHarta);
            pnlHarta.BackColor = Color.FromArgb(250, 249, 246);
            pnlHarta.Dock = DockStyle.Fill;
            pnlHarta.Name = "pnlHarta";
            pnlHarta.Padding = new Padding(1, 1, 1, 1);
            // 
            // canvas
            // 
            canvas.Dock = DockStyle.Fill;
            canvas.Name = "canvas";
            canvas.HartaModificata += canvas_HartaModificata;
            canvas.NodAdaugat += canvas_NodAdaugat;
            canvas.SelectieSchimbata += canvas_SelectieSchimbata;
            // 
            // pnlStareHarta
            // 
            pnlStareHarta.Controls.Add(lblStare);
            pnlStareHarta.Controls.Add(lblNumarare);
            pnlStareHarta.Controls.Add(lblSugestie);
            pnlStareHarta.BackColor = Color.FromArgb(238, 236, 230);
            pnlStareHarta.Dock = DockStyle.Bottom;
            pnlStareHarta.Name = "pnlStareHarta";
            pnlStareHarta.Size = new Size(700, 30);
            // 
            // lblStare
            // 
            lblStare.AutoSize = true;
            lblStare.Font = new Font("Bahnschrift SemiBold", 9F);
            lblStare.Location = new Point(10, 7);
            lblStare.Name = "lblStare";
            lblStare.Text = "Harta este salvată";
            // 
            // lblNumarare
            // 
            lblNumarare.AutoSize = true;
            lblNumarare.ForeColor = Color.FromArgb(110, 106, 96);
            lblNumarare.Location = new Point(170, 7);
            lblNumarare.Name = "lblNumarare";
            lblNumarare.Text = "";
            // 
            // lblSugestie
            // 
            lblSugestie.Dock = DockStyle.Right;
            lblSugestie.ForeColor = Color.FromArgb(110, 106, 96);
            lblSugestie.Name = "lblSugestie";
            lblSugestie.Padding = new Padding(0, 0, 10, 0);
            lblSugestie.Size = new Size(420, 30);
            lblSugestie.Text = "";
            lblSugestie.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlSpatiuDreapta
            // 
            pnlSpatiuDreapta.Dock = DockStyle.Right;
            pnlSpatiuDreapta.Name = "pnlSpatiuDreapta";
            pnlSpatiuDreapta.Size = new Size(16, 588);
            // 
            // pnlProprietati
            // 
            pnlProprietati.Controls.Add(lblProprietatiTitlu);
            pnlProprietati.Controls.Add(lblFaraSelectie);
            pnlProprietati.Controls.Add(pnlNod);
            pnlProprietati.Controls.Add(pnlConexiune);
            pnlProprietati.Controls.Add(btnStergeSelectia);
            pnlProprietati.Controls.Add(pnlLinie);
            pnlProprietati.Controls.Add(lblAjutorTitlu);
            pnlProprietati.Controls.Add(lblAjutor);
            pnlProprietati.BackColor = Color.FromArgb(250, 249, 246);
            pnlProprietati.Dock = DockStyle.Right;
            pnlProprietati.Name = "pnlProprietati";
            pnlProprietati.Size = new Size(300, 588);
            // 
            // lblProprietatiTitlu
            // 
            lblProprietatiTitlu.AutoSize = true;
            lblProprietatiTitlu.Font = new Font("Bahnschrift SemiBold", 11F);
            lblProprietatiTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblProprietatiTitlu.Location = new Point(20, 18);
            lblProprietatiTitlu.Name = "lblProprietatiTitlu";
            lblProprietatiTitlu.Text = "Proprietăți";
            // 
            // lblFaraSelectie
            // 
            lblFaraSelectie.ForeColor = Color.FromArgb(110, 106, 96);
            lblFaraSelectie.Location = new Point(20, 52);
            lblFaraSelectie.Name = "lblFaraSelectie";
            lblFaraSelectie.Size = new Size(260, 44);
            lblFaraSelectie.Text = "Selectați un nod sau un drum pentru a-l modifica.";
            // 
            // pnlNod
            // 
            pnlNod.Controls.Add(lblNume);
            pnlNod.Controls.Add(txtNume);
            pnlNod.Controls.Add(lblTip);
            pnlNod.Controls.Add(cmbTip);
            pnlNod.Controls.Add(lblPozitie);
            pnlNod.Controls.Add(btnAplicaNod);
            pnlNod.Location = new Point(0, 48);
            pnlNod.Name = "pnlNod";
            pnlNod.Size = new Size(300, 200);
            // 
            // lblNume
            // 
            lblNume.AutoSize = true;
            lblNume.Location = new Point(20, 4);
            lblNume.Name = "lblNume";
            lblNume.Text = "Nume";
            // 
            // txtNume
            // 
            txtNume.Location = new Point(20, 24);
            txtNume.MaxLength = 100;
            txtNume.Name = "txtNume";
            txtNume.Size = new Size(260, 23);
            txtNume.TabIndex = 0;
            txtNume.KeyDown += txtNume_KeyDown;
            // 
            // lblTip
            // 
            lblTip.AutoSize = true;
            lblTip.Location = new Point(20, 58);
            lblTip.Name = "lblTip";
            lblTip.Text = "Tip";
            // 
            // cmbTip
            // 
            cmbTip.DisplayMember = "Text";
            cmbTip.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTip.Location = new Point(20, 78);
            cmbTip.Name = "cmbTip";
            cmbTip.Size = new Size(160, 23);
            cmbTip.TabIndex = 1;
            // 
            // lblPozitie
            // 
            lblPozitie.AutoSize = true;
            lblPozitie.ForeColor = Color.FromArgb(110, 106, 96);
            lblPozitie.Location = new Point(20, 110);
            lblPozitie.Name = "lblPozitie";
            lblPozitie.Text = "";
            // 
            // btnAplicaNod
            // 
            btnAplicaNod.Icon = "check";
            btnAplicaNod.Location = new Point(20, 136);
            btnAplicaNod.Name = "btnAplicaNod";
            btnAplicaNod.Size = new Size(260, 32);
            btnAplicaNod.Stil = Controale.StilButon.Primar;
            btnAplicaNod.TabIndex = 2;
            btnAplicaNod.Text = " Aplică";
            btnAplicaNod.Click += btnAplicaNod_Click;
            // 
            // pnlConexiune
            // 
            pnlConexiune.Controls.Add(lblCapete);
            pnlConexiune.Controls.Add(lblDistanta);
            pnlConexiune.Controls.Add(numDistanta);
            pnlConexiune.Controls.Add(btnAplicaConexiune);
            pnlConexiune.Location = new Point(0, 48);
            pnlConexiune.Name = "pnlConexiune";
            pnlConexiune.Size = new Size(300, 200);
            // 
            // lblCapete
            // 
            lblCapete.AutoSize = true;
            lblCapete.Font = new Font("Bahnschrift SemiBold", 9.5F);
            lblCapete.Location = new Point(20, 4);
            lblCapete.Name = "lblCapete";
            lblCapete.Text = "A\n↔ B";
            // 
            // lblDistanta
            // 
            lblDistanta.AutoSize = true;
            lblDistanta.Location = new Point(20, 58);
            lblDistanta.Name = "lblDistanta";
            lblDistanta.Text = "Distanța (km)";
            // 
            // numDistanta
            // 
            numDistanta.DecimalPlaces = 1;
            numDistanta.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numDistanta.Location = new Point(20, 78);
            numDistanta.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numDistanta.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numDistanta.Name = "numDistanta";
            numDistanta.Size = new Size(120, 23);
            numDistanta.TabIndex = 3;
            numDistanta.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAplicaConexiune
            // 
            btnAplicaConexiune.Icon = "check";
            btnAplicaConexiune.Location = new Point(20, 136);
            btnAplicaConexiune.Name = "btnAplicaConexiune";
            btnAplicaConexiune.Size = new Size(260, 32);
            btnAplicaConexiune.Stil = Controale.StilButon.Primar;
            btnAplicaConexiune.TabIndex = 4;
            btnAplicaConexiune.Text = " Aplică";
            btnAplicaConexiune.Click += btnAplicaConexiune_Click;
            // 
            // btnStergeSelectia
            // 
            btnStergeSelectia.Icon = "trash-2";
            btnStergeSelectia.Location = new Point(20, 256);
            btnStergeSelectia.Name = "btnStergeSelectia";
            btnStergeSelectia.Size = new Size(260, 32);
            btnStergeSelectia.Stil = Controale.StilButon.Pericol;
            btnStergeSelectia.TabIndex = 5;
            btnStergeSelectia.Text = " Șterge selecția";
            btnStergeSelectia.Click += btnStergeSelectia_Click;
            // 
            // pnlLinie
            // 
            pnlLinie.BackColor = Color.FromArgb(212, 208, 198);
            pnlLinie.Location = new Point(20, 306);
            pnlLinie.Name = "pnlLinie";
            pnlLinie.Size = new Size(260, 1);
            // 
            // lblAjutorTitlu
            // 
            lblAjutorTitlu.AutoSize = true;
            lblAjutorTitlu.Font = new Font("Bahnschrift SemiBold", 10F);
            lblAjutorTitlu.ForeColor = Color.FromArgb(34, 34, 31);
            lblAjutorTitlu.Location = new Point(20, 318);
            lblAjutorTitlu.Name = "lblAjutorTitlu";
            lblAjutorTitlu.Text = "Cum se folosește";
            // 
            // lblAjutor
            // 
            lblAjutor.ForeColor = Color.FromArgb(110, 106, 96);
            lblAjutor.Location = new Point(20, 344);
            lblAjutor.Name = "lblAjutor";
            lblAjutor.Size = new Size(260, 200);
            lblAjutor.Text = "• Stație / Intersecție: click pe hartă adaugă nodul.\n• Conexiune: trageți de la un nod la altul; distanța se propune după hartă.\n• Selectare: trageți nodurile ca să le mutați.\n• Rotița mouse-ului: zoom. Click dreapta + tragere: deplasare.\n• Delete: șterge selecția.\n\nDrumurile se salvează în ambele sensuri.";
            // 
            // pnlSpatiu
            // 
            pnlSpatiu.Dock = DockStyle.Top;
            pnlSpatiu.Name = "pnlSpatiu";
            pnlSpatiu.Size = new Size(1008, 16);
            // 
            // pnlUnelte
            // 
            pnlUnelte.Controls.Add(btnModSelectare);
            pnlUnelte.Controls.Add(btnModStatie);
            pnlUnelte.Controls.Add(btnModIntersectie);
            pnlUnelte.Controls.Add(btnModConexiune);
            pnlUnelte.Controls.Add(btnModSterge);
            pnlUnelte.Controls.Add(pnlSeparator);
            pnlUnelte.Controls.Add(btnPotriveste);
            pnlUnelte.Controls.Add(btnReincarca);
            pnlUnelte.Controls.Add(btnSalveaza);
            pnlUnelte.BackColor = Color.FromArgb(250, 249, 246);
            pnlUnelte.Dock = DockStyle.Top;
            pnlUnelte.Name = "pnlUnelte";
            pnlUnelte.Size = new Size(1008, 60);
            // 
            // btnModSelectare
            // 
            btnModSelectare.Icon = "mouse-pointer-2";
            btnModSelectare.Location = new Point(16, 14);
            btnModSelectare.Name = "btnModSelectare";
            btnModSelectare.Size = new Size(110, 32);
            btnModSelectare.TabIndex = 6;
            btnModSelectare.Text = " Selectare";
            btnModSelectare.Click += btnModSelectare_Click;
            // 
            // btnModStatie
            // 
            btnModStatie.Icon = "map-pin";
            btnModStatie.Location = new Point(132, 14);
            btnModStatie.Name = "btnModStatie";
            btnModStatie.Size = new Size(96, 32);
            btnModStatie.TabIndex = 7;
            btnModStatie.Text = " Stație";
            btnModStatie.Click += btnModStatie_Click;
            // 
            // btnModIntersectie
            // 
            btnModIntersectie.Icon = "circle-dot";
            btnModIntersectie.Location = new Point(234, 14);
            btnModIntersectie.Name = "btnModIntersectie";
            btnModIntersectie.Size = new Size(118, 32);
            btnModIntersectie.TabIndex = 8;
            btnModIntersectie.Text = " Intersecție";
            btnModIntersectie.Click += btnModIntersectie_Click;
            // 
            // btnModConexiune
            // 
            btnModConexiune.Icon = "spline";
            btnModConexiune.Location = new Point(358, 14);
            btnModConexiune.Name = "btnModConexiune";
            btnModConexiune.Size = new Size(116, 32);
            btnModConexiune.TabIndex = 9;
            btnModConexiune.Text = " Conexiune";
            btnModConexiune.Click += btnModConexiune_Click;
            // 
            // btnModSterge
            // 
            btnModSterge.Icon = "eraser";
            btnModSterge.Location = new Point(480, 14);
            btnModSterge.Name = "btnModSterge";
            btnModSterge.Size = new Size(96, 32);
            btnModSterge.TabIndex = 10;
            btnModSterge.Text = " Șterge";
            btnModSterge.Click += btnModSterge_Click;
            // 
            // pnlSeparator
            // 
            pnlSeparator.BackColor = Color.FromArgb(212, 208, 198);
            pnlSeparator.Location = new Point(590, 16);
            pnlSeparator.Name = "pnlSeparator";
            pnlSeparator.Size = new Size(1, 28);
            // 
            // btnPotriveste
            // 
            btnPotriveste.Icon = "maximize";
            btnPotriveste.Location = new Point(604, 14);
            btnPotriveste.Name = "btnPotriveste";
            btnPotriveste.Size = new Size(114, 32);
            btnPotriveste.TabIndex = 11;
            btnPotriveste.Text = " Potrivește";
            btnPotriveste.Click += btnPotriveste_Click;
            // 
            // btnReincarca
            // 
            btnReincarca.Icon = "refresh-cw";
            btnReincarca.Location = new Point(724, 14);
            btnReincarca.Name = "btnReincarca";
            btnReincarca.Size = new Size(112, 32);
            btnReincarca.TabIndex = 12;
            btnReincarca.Text = " Reîncarcă";
            btnReincarca.Click += btnReincarca_Click;
            // 
            // btnSalveaza
            // 
            btnSalveaza.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalveaza.Icon = "save";
            btnSalveaza.Location = new Point(852, 14);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(140, 32);
            btnSalveaza.Stil = Controale.StilButon.Primar;
            btnSalveaza.TabIndex = 13;
            btnSalveaza.Text = " Salvează harta";
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmEditorHarta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 236, 230);
            ClientSize = new Size(1008, 680);
            Controls.Add(pnlHarta);
            Controls.Add(pnlSpatiuDreapta);
            Controls.Add(pnlProprietati);
            Controls.Add(pnlSpatiu);
            Controls.Add(pnlUnelte);
            Name = "FrmEditorHarta";
            Text = "Harta stațiilor";
            Load += FrmEditorHarta_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            pnlSeparator.ResumeLayout(false);
            pnlUnelte.ResumeLayout(false);
            pnlSpatiu.ResumeLayout(false);
            pnlLinie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numDistanta).EndInit();
            pnlConexiune.ResumeLayout(false);
            pnlConexiune.PerformLayout();
            pnlNod.ResumeLayout(false);
            pnlNod.PerformLayout();
            pnlProprietati.ResumeLayout(false);
            pnlProprietati.PerformLayout();
            pnlSpatiuDreapta.ResumeLayout(false);
            pnlStareHarta.ResumeLayout(false);
            pnlStareHarta.PerformLayout();
            pnlHarta.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHarta;
        private Controale.HartaCanvasControl canvas;
        private Panel pnlStareHarta;
        private Label lblStare;
        private Label lblNumarare;
        private Label lblSugestie;
        private Panel pnlSpatiuDreapta;
        private Panel pnlProprietati;
        private Label lblProprietatiTitlu;
        private Label lblFaraSelectie;
        private Panel pnlNod;
        private Label lblNume;
        private TextBox txtNume;
        private Label lblTip;
        private ComboBox cmbTip;
        private Label lblPozitie;
        private Controale.ButonIcon btnAplicaNod;
        private Panel pnlConexiune;
        private Label lblCapete;
        private Label lblDistanta;
        private NumericUpDown numDistanta;
        private Controale.ButonIcon btnAplicaConexiune;
        private Controale.ButonIcon btnStergeSelectia;
        private Panel pnlLinie;
        private Label lblAjutorTitlu;
        private Label lblAjutor;
        private Panel pnlSpatiu;
        private Panel pnlUnelte;
        private Controale.ButonIcon btnModSelectare;
        private Controale.ButonIcon btnModStatie;
        private Controale.ButonIcon btnModIntersectie;
        private Controale.ButonIcon btnModConexiune;
        private Controale.ButonIcon btnModSterge;
        private Panel pnlSeparator;
        private Controale.ButonIcon btnPotriveste;
        private Controale.ButonIcon btnReincarca;
        private Controale.ButonIcon btnSalveaza;
        private ErrorProvider errorProvider;
    }
}
