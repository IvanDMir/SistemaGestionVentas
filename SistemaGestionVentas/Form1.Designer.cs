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
            panelStrip = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            btnProductos = new Button();
            btnMovimientos = new Button();
            btnReportes = new Button();
            SuspendLayout();
            //
            // panelStrip
            //
            panelStrip.BackColor = Color.FromArgb(37, 99, 235);
            panelStrip.Dock = DockStyle.Top;
            panelStrip.Location = new Point(0, 0);
            panelStrip.Name = "panelStrip";
            panelStrip.Size = new Size(400, 4);
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.Location = new Point(28, 24);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(320, 28);
            lblTitulo.Text = "Gestión de ventas";
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitulo.Location = new Point(30, 56);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(320, 17);
            lblSubtitulo.Text = "Inventario local · tus datos en SQLite";
            //
            // btnProductos
            //
            btnProductos.Location = new Point(28, 92);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(328, 44);
            btnProductos.TabIndex = 0;
            btnProductos.Text = "Productos";
            btnProductos.Click += btnProductos_Click;
            //
            // btnMovimientos
            //
            btnMovimientos.Location = new Point(28, 146);
            btnMovimientos.Name = "btnMovimientos";
            btnMovimientos.Size = new Size(328, 44);
            btnMovimientos.TabIndex = 1;
            btnMovimientos.Text = "Movimientos de inventario";
            btnMovimientos.Click += btnMovimientos_Click;
            //
            // btnReportes
            //
            btnReportes.Location = new Point(28, 200);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(328, 44);
            btnReportes.TabIndex = 2;
            btnReportes.Text = "Reportes y estadísticas";
            btnReportes.Click += btnReportes_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(384, 268);
            Controls.Add(btnReportes);
            Controls.Add(btnMovimientos);
            Controls.Add(btnProductos);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblTitulo);
            Controls.Add(panelStrip);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema gestión ventas";
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel panelStrip;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnProductos;
        private Button btnMovimientos;
        private Button btnReportes;
    }
}
