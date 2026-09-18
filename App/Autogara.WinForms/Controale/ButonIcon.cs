using System.ComponentModel;
using Autogara.WinForms.Ui;

namespace Autogara.WinForms.Controale
{
    public enum StilButon
    {
        Secundar,
        Primar,
        Pericol,
        Transparent,
        Meniu,
    }

    /// <summary>
    /// Buton cu iconita SVG si culori din <see cref="Tema"/>. In Designer se aleg doar
    /// <see cref="Icon"/> (numele iconitei Lucide) si <see cref="Stil"/>.
    /// </summary>
    public class ButonIcon : Button
    {
        private string _icon = string.Empty;
        private StilButon _stil = StilButon.Secundar;
        private int _marimeIcon = 16;
        private bool _activ;

        public ButonIcon()
        {
            base.FlatStyle = FlatStyle.Flat;
            base.Cursor = Cursors.Hand;
            UseVisualStyleBackColor = false;
            TextImageRelation = TextImageRelation.ImageBeforeText;
            AplicaStil();
        }

        [Category("Autogara"), DefaultValue("")]
        [Description("Numele iconitei din Resurse/Iconite, fara .svg (ex. \"search\").")]
        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value ?? string.Empty;
                ActualizeazaImagine();
            }
        }

        [Category("Autogara"), DefaultValue(StilButon.Secundar)]
        public StilButon Stil
        {
            get => _stil;
            set
            {
                _stil = value;
                AplicaStil();
            }
        }

        [Category("Autogara"), DefaultValue(16)]
        public int MarimeIcon
        {
            get => _marimeIcon;
            set
            {
                _marimeIcon = Math.Max(8, value);
                ActualizeazaImagine();
            }
        }

        /// <summary>Pentru butoanele din meniu: sectiunea deschisa acum.</summary>
        [Category("Autogara"), DefaultValue(false)]
        public bool Activ
        {
            get => _activ;
            set
            {
                _activ = value;
                AplicaStil();
            }
        }

        // Culorile si imaginea vin din stil, nu se salveaza in Designer.
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image Image { get => base.Image; set => base.Image = value; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle { get => base.FlatStyle; set => base.FlatStyle = value; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatButtonAppearance FlatAppearance => base.FlatAppearance;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Cursor Cursor { get => base.Cursor; set => base.Cursor = value; }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            AplicaStil();
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            ActualizeazaImagine();
        }

        private void AplicaStil()
        {
            var (fundal, text, bordura, hover) = _stil switch
            {
                StilButon.Primar => (Tema.Primar, Color.White, Tema.Primar, Tema.PrimarInchis),
                StilButon.Pericol => (Color.White, Tema.Eroare, Tema.Eroare, Color.FromArgb(254, 242, 242)),
                // WinForms nu accepta culori transparente pe butoanele Flat: fundalul e cel al cardurilor.
                StilButon.Transparent => (Tema.Suprafata, Tema.Text, Tema.Suprafata, Tema.Fundal),
                StilButon.Meniu => _activ
                    ? (Tema.MeniuActiv, Color.White, Tema.MeniuActiv, Tema.MeniuActiv)
                    : (Tema.Meniu, Tema.MeniuText, Tema.Meniu, Tema.MeniuActiv),
                _ => (Color.White, Tema.Text, Tema.Bordura, Tema.Fundal),
            };

            if (!Enabled)
            {
                text = Tema.TextSecundar;
                if (_stil == StilButon.Primar)
                    (fundal, bordura) = (Tema.Bordura, Tema.Bordura);
            }

            base.BackColor = fundal;
            base.ForeColor = text;
            base.FlatAppearance.BorderColor = bordura;
            base.FlatAppearance.BorderSize = _stil is StilButon.Transparent or StilButon.Meniu ? 0 : 1;
            base.FlatAppearance.MouseOverBackColor = hover;
            base.FlatAppearance.MouseDownBackColor = hover;
            ActualizeazaImagine();
        }

        private void ActualizeazaImagine()
        {
            base.Image = Iconite.Exista(_icon)
                ? Iconite.Deseneaza(_icon, LogicalToDeviceUnits(_marimeIcon), base.ForeColor)
                : null;
        }
    }
}
