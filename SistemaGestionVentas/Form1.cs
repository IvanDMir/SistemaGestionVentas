using SistemaGestionVentas.Forms;

namespace SistemaGestionVentas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
