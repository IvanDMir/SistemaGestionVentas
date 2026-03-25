namespace SistemaGestionVentas.Forms
{
    partial class FormProductoEdit
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblSku = new Label();
            txtSku = new TextBox();
            lblCosto = new Label();
            numCosto = new NumericUpDown();
            lblPrecioLista = new Label();
            numPrecioLista = new NumericUpDown();
            lblStockInicial = new Label();
            numStockInicial = new NumericUpDown();
            lblMinimo = new Label();
            numMinimo = new NumericUpDown();
            lblStockActual = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)numCosto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrecioLista).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStockInicial).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinimo).BeginInit();
            SuspendLayout();
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(12, 15);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(54, 15);
            lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(12, 33);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(360, 23);
            //
            // lblSku
            //
            lblSku.AutoSize = true;
            lblSku.Location = new Point(12, 65);
            lblSku.Name = "lblSku";
            lblSku.Size = new Size(28, 15);
            lblSku.Text = "SKU";
            //
            // txtSku
            //
            txtSku.Location = new Point(12, 83);
            txtSku.Name = "txtSku";
            txtSku.Size = new Size(360, 23);
            //
            // lblCosto
            //
            lblCosto.AutoSize = true;
            lblCosto.Location = new Point(12, 115);
            lblCosto.Name = "lblCosto";
            lblCosto.Size = new Size(95, 15);
            lblCosto.Text = "Precio de compra";
            //
            // numCosto
            //
            numCosto.DecimalPlaces = 2;
            numCosto.Location = new Point(12, 133);
            numCosto.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numCosto.Name = "numCosto";
            numCosto.Size = new Size(120, 23);
            //
            // lblPrecioLista
            //
            lblPrecioLista.AutoSize = true;
            lblPrecioLista.Location = new Point(160, 115);
            lblPrecioLista.Name = "lblPrecioLista";
            lblPrecioLista.Size = new Size(89, 15);
            lblPrecioLista.Text = "Precio de venta";
            //
            // numPrecioLista
            //
            numPrecioLista.DecimalPlaces = 2;
            numPrecioLista.Location = new Point(160, 133);
            numPrecioLista.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numPrecioLista.Name = "numPrecioLista";
            numPrecioLista.Size = new Size(120, 23);
            //
            // lblStockInicial
            //
            lblStockInicial.AutoSize = true;
            lblStockInicial.Location = new Point(12, 165);
            lblStockInicial.Name = "lblStockInicial";
            lblStockInicial.Size = new Size(74, 15);
            lblStockInicial.Text = "Stock inicial";
            //
            // numStockInicial
            //
            numStockInicial.Location = new Point(12, 183);
            numStockInicial.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numStockInicial.Name = "numStockInicial";
            numStockInicial.Size = new Size(120, 23);
            //
            // lblMinimo
            //
            lblMinimo.AutoSize = true;
            lblMinimo.Location = new Point(160, 165);
            lblMinimo.Name = "lblMinimo";
            lblMinimo.Size = new Size(103, 15);
            lblMinimo.Text = "Stock mínimo alerta";
            //
            // numMinimo
            //
            numMinimo.Location = new Point(160, 183);
            numMinimo.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numMinimo.Name = "numMinimo";
            numMinimo.Size = new Size(120, 23);
            //
            // lblStockActual
            //
            lblStockActual.AutoSize = true;
            lblStockActual.Location = new Point(12, 215);
            lblStockActual.Name = "lblStockActual";
            lblStockActual.Size = new Size(38, 15);
            lblStockActual.Text = "—";
            lblStockActual.Visible = false;
            //
            // btnAceptar
            //
            btnAceptar.DialogResult = DialogResult.OK;
            btnAceptar.Location = new Point(198, 250);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(85, 28);
            btnAceptar.Text = "Aceptar";
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(289, 250);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(85, 28);
            btnCancelar.Text = "Cancelar";
            //
            // FormProductoEdit
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(386, 292);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblStockActual);
            Controls.Add(numMinimo);
            Controls.Add(lblMinimo);
            Controls.Add(numStockInicial);
            Controls.Add(lblStockInicial);
            Controls.Add(numPrecioLista);
            Controls.Add(lblPrecioLista);
            Controls.Add(numCosto);
            Controls.Add(lblCosto);
            Controls.Add(txtSku);
            Controls.Add(lblSku);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormProductoEdit";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Producto";
            ((System.ComponentModel.ISupportInitialize)numCosto).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrecioLista).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStockInicial).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinimo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblSku;
        private TextBox txtSku;
        private Label lblCosto;
        private NumericUpDown numCosto;
        private Label lblPrecioLista;
        private NumericUpDown numPrecioLista;
        private Label lblStockInicial;
        private NumericUpDown numStockInicial;
        private Label lblMinimo;
        private NumericUpDown numMinimo;
        private Label lblStockActual;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}
