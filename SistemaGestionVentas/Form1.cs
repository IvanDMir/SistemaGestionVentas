using SistemaGestionVentas.Forms;
using SistemaGestionVentas.UI;

namespace SistemaGestionVentas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            UiTheme.ApplyForm(this);
            UiTheme.StylePrimaryButton(btnProductos);
            UiTheme.StylePrimaryButton(btnMovimientos);
            UiTheme.StylePrimaryButton(btnReportes);
            lblTitulo.Font = UiTheme.FontTitle;
            lblTitulo.ForeColor = UiTheme.TextPrimary;
            lblSubtitulo.ForeColor = UiTheme.TextMuted;
            lblSubtitulo.Font = UiTheme.FontSubtitle;
            panelStrip.BackColor = UiTheme.Primary;
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            using var f = new FormProductos();
            f.ShowDialog(this);
        }

        private void btnMovimientos_Click(object sender, EventArgs e)
        {
            using var f = new FormMovimientos();
            f.ShowDialog(this);
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            using var f = new FormReportes();
            f.ShowDialog(this);
        }
    }
}
