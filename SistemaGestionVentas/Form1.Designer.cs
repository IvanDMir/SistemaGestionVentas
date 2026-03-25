namespace SistemaGestionVentas
{
    partial class Form1
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

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            btnProductos = new Button();
            btnMovimientos = new Button();
            btnReportes = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 24);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(280, 21);
            lblTitulo.Text = "Sistema de gestión de ventas";
            //
            // btnProductos
            //
            btnProductos.Location = new Point(24, 64);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(280, 36);
            btnProductos.Text = "Productos";
            btnProductos.Click += btnProductos_Click;
            //
            // btnMovimientos
            //
            btnMovimientos.Location = new Point(24, 112);
            btnMovimientos.Name = "btnMovimientos";
            btnMovimientos.Size = new Size(280, 36);
            btnMovimientos.Text = "Movimientos de inventario";
            btnMovimientos.Click += btnMovimientos_Click;
            //
            // btnReportes
            //
            btnReportes.Location = new Point(24, 160);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(280, 36);
            btnReportes.Text = "Reportes";
            btnReportes.Click += btnReportes_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 221);
            Controls.Add(btnReportes);
            Controls.Add(btnMovimientos);
            Controls.Add(btnProductos);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema gestión ventas";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo;
        private Button btnProductos;
        private Button btnMovimientos;
        private Button btnReportes;
    }
}
