namespace SistemaGestionVentas.Forms
{
    partial class FormMovimientos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblProducto = new Label();
            cboProducto = new ComboBox();
            lblTipo = new Label();
            cboTipo = new ComboBox();
            lblCantidad = new Label();
            numCantidad = new NumericUpDown();
            lblPrecioVenta = new Label();
            numPrecioVenta = new NumericUpDown();
            lblNota = new Label();
            txtNota = new TextBox();
            btnRegistrar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrecioVenta).BeginInit();
            SuspendLayout();
            //
            // lblProducto
            //
            lblProducto.AutoSize = true;
            lblProducto.Location = new Point(12, 15);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(55, 15);
            lblProducto.Text = "Producto";
            //
            // cboProducto
            //
            cboProducto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProducto.FormattingEnabled = true;
            cboProducto.Location = new Point(12, 33);
            cboProducto.Name = "cboProducto";
            cboProducto.Size = new Size(420, 25);
            cboProducto.SelectedIndexChanged += cboProducto_SelectedIndexChanged;
            //
            // lblTipo
            //
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(12, 65);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(30, 15);
            lblTipo.Text = "Tipo";
            //
            // cboTipo
            //
            cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipo.FormattingEnabled = true;
            cboTipo.Location = new Point(12, 83);
            cboTipo.Name = "cboTipo";
            cboTipo.Size = new Size(420, 25);
            cboTipo.SelectedIndexChanged += cboTipo_SelectedIndexChanged;
            //
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(12, 115);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(55, 15);
            lblCantidad.Text = "Cantidad";
            //
            // numCantidad
            //
            numCantidad.Location = new Point(12, 133);
            numCantidad.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numCantidad.Name = "numCantidad";
            numCantidad.Size = new Size(120, 23);
            numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblPrecioVenta
            //
            lblPrecioVenta.AutoSize = true;
            lblPrecioVenta.Location = new Point(160, 115);
            lblPrecioVenta.Name = "lblPrecioVenta";
            lblPrecioVenta.Size = new Size(118, 15);
            lblPrecioVenta.Text = "Precio unitario venta";
            //
            // numPrecioVenta
            //
            numPrecioVenta.DecimalPlaces = 2;
            numPrecioVenta.Location = new Point(160, 133);
            numPrecioVenta.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numPrecioVenta.Name = "numPrecioVenta";
            numPrecioVenta.Size = new Size(120, 23);
            //
            // lblNota
            //
            lblNota.AutoSize = true;
            lblNota.Location = new Point(12, 165);
            lblNota.Name = "lblNota";
            lblNota.Size = new Size(33, 15);
            lblNota.Text = "Nota";
            //
            // txtNota
            //
            txtNota.Location = new Point(12, 183);
            txtNota.Name = "txtNota";
            txtNota.Size = new Size(420, 23);
            //
            // btnRegistrar
            //
            btnRegistrar.Location = new Point(12, 224);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(140, 38);
            btnRegistrar.Text = "Registrar";
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(292, 224);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(140, 38);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;
            //
            // FormMovimientos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(456, 288);
            Controls.Add(btnCerrar);
            Controls.Add(btnRegistrar);
            Controls.Add(txtNota);
            Controls.Add(lblNota);
            Controls.Add(numPrecioVenta);
            Controls.Add(lblPrecioVenta);
            Controls.Add(numCantidad);
            Controls.Add(lblCantidad);
            Controls.Add(cboTipo);
            Controls.Add(lblTipo);
            Controls.Add(cboProducto);
            Controls.Add(lblProducto);
            Name = "FormMovimientos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Movimientos de inventario";
            Load += FormMovimientos_Load;
            ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrecioVenta).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblProducto;
        private ComboBox cboProducto;
        private Label lblTipo;
        private ComboBox cboTipo;
        private Label lblCantidad;
        private NumericUpDown numCantidad;
        private Label lblPrecioVenta;
        private NumericUpDown numPrecioVenta;
        private Label lblNota;
        private TextBox txtNota;
        private Button btnRegistrar;
        private Button btnCerrar;
    }
}
