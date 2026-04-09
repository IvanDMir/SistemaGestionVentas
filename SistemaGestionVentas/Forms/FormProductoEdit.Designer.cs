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
            lblGrupo = new Label();
            cboGrupo = new ComboBox();
            lblNuevoGrupo = new Label();
            txtNuevoGrupo = new TextBox();
            lblVariante = new Label();
            txtVariante = new TextBox();
            lblImagen = new Label();
            picImagen = new PictureBox();
            btnCargarImagen = new Button();
            btnPegarPortapapeles = new Button();
            btnQuitarImagen = new Button();
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
            ((System.ComponentModel.ISupportInitialize)picImagen).BeginInit();
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
            txtNombre.Size = new Size(268, 23);
            //
            // lblGrupo
            //
            lblGrupo.AutoSize = true;
            lblGrupo.Location = new Point(12, 62);
            lblGrupo.Name = "lblGrupo";
            lblGrupo.Size = new Size(200, 15);
            lblGrupo.Text = "Línea / grupo (ej. Rompecabezas)";
            //
            // cboGrupo
            //
            cboGrupo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGrupo.FormattingEnabled = true;
            cboGrupo.Location = new Point(12, 80);
            cboGrupo.Name = "cboGrupo";
            cboGrupo.Size = new Size(268, 23);
            //
            // lblNuevoGrupo
            //
            lblNuevoGrupo.AutoSize = true;
            lblNuevoGrupo.Location = new Point(12, 108);
            lblNuevoGrupo.Name = "lblNuevoGrupo";
            lblNuevoGrupo.Size = new Size(260, 15);
            lblNuevoGrupo.Text = "Nuevo grupo (opcional, pisa la selección)";
            //
            // txtNuevoGrupo
            //
            txtNuevoGrupo.Location = new Point(12, 126);
            txtNuevoGrupo.Name = "txtNuevoGrupo";
            txtNuevoGrupo.PlaceholderText = "Escribí acá solo para crear o usar otra línea";
            txtNuevoGrupo.Size = new Size(268, 23);
            //
            // lblVariante
            //
            lblVariante.AutoSize = true;
            lblVariante.Location = new Point(12, 154);
            lblVariante.Name = "lblVariante";
            lblVariante.Size = new Size(220, 15);
            lblVariante.Text = "Modelo / variante (ej. 1000 piezas paisaje)";
            //
            // txtVariante
            //
            txtVariante.Location = new Point(12, 172);
            txtVariante.Name = "txtVariante";
            txtVariante.Size = new Size(268, 23);
            //
            // lblImagen
            //
            lblImagen.AutoSize = true;
            lblImagen.Location = new Point(300, 15);
            lblImagen.Name = "lblImagen";
            lblImagen.Size = new Size(48, 15);
            lblImagen.Text = "Imagen";
            //
            // picImagen
            //
            picImagen.BorderStyle = BorderStyle.FixedSingle;
            picImagen.Location = new Point(300, 33);
            picImagen.Name = "picImagen";
            picImagen.Size = new Size(200, 200);
            picImagen.SizeMode = PictureBoxSizeMode.Zoom;
            picImagen.TabStop = false;
            //
            // btnCargarImagen
            //
            btnCargarImagen.Location = new Point(300, 238);
            btnCargarImagen.Name = "btnCargarImagen";
            btnCargarImagen.Size = new Size(98, 28);
            btnCargarImagen.Text = "Cargar…";
            btnCargarImagen.Click += btnCargarImagen_Click;
            //
            // btnPegarPortapapeles
            //
            btnPegarPortapapeles.Location = new Point(404, 238);
            btnPegarPortapapeles.Name = "btnPegarPortapapeles";
            btnPegarPortapapeles.Size = new Size(96, 28);
            btnPegarPortapapeles.Text = "Pegar Ctrl+V";
            btnPegarPortapapeles.Click += btnPegarPortapapeles_Click;
            //
            // btnQuitarImagen
            //
            btnQuitarImagen.Location = new Point(300, 270);
            btnQuitarImagen.Name = "btnQuitarImagen";
            btnQuitarImagen.Size = new Size(200, 28);
            btnQuitarImagen.Text = "Quitar imagen";
            btnQuitarImagen.Click += btnQuitarImagen_Click;
            //
            // lblCosto
            //
            lblCosto.AutoSize = true;
            lblCosto.Location = new Point(12, 205);
            lblCosto.Name = "lblCosto";
            lblCosto.Size = new Size(270, 15);
            lblCosto.Text = "Costo prom. (se recalcula con cada compra)";
            //
            // numCosto
            //
            numCosto.DecimalPlaces = 2;
            numCosto.Location = new Point(12, 238);
            numCosto.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numCosto.Name = "numCosto";
            numCosto.Size = new Size(120, 23);
            //
            // lblPrecioLista
            //
            lblPrecioLista.AutoSize = true;
            lblPrecioLista.Location = new Point(160, 205);
            lblPrecioLista.Name = "lblPrecioLista";
            lblPrecioLista.Size = new Size(89, 15);
            lblPrecioLista.Text = "Precio de venta";
            //
            // numPrecioLista
            //
            numPrecioLista.DecimalPlaces = 2;
            numPrecioLista.Location = new Point(160, 238);
            numPrecioLista.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numPrecioLista.Name = "numPrecioLista";
            numPrecioLista.Size = new Size(120, 23);
            //
            // lblStockInicial
            //
            lblStockInicial.AutoSize = true;
            lblStockInicial.Location = new Point(12, 270);
            lblStockInicial.Name = "lblStockInicial";
            lblStockInicial.Size = new Size(74, 15);
            lblStockInicial.Text = "Stock inicial";
            //
            // numStockInicial
            //
            numStockInicial.Location = new Point(12, 288);
            numStockInicial.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numStockInicial.Name = "numStockInicial";
            numStockInicial.Size = new Size(120, 23);
            //
            // lblMinimo
            //
            lblMinimo.AutoSize = true;
            lblMinimo.Location = new Point(160, 270);
            lblMinimo.Name = "lblMinimo";
            lblMinimo.Size = new Size(103, 15);
            lblMinimo.Text = "Stock mínimo alerta";
            //
            // numMinimo
            //
            numMinimo.Location = new Point(160, 288);
            numMinimo.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numMinimo.Name = "numMinimo";
            numMinimo.Size = new Size(120, 23);
            //
            // lblStockActual
            //
            lblStockActual.AutoSize = true;
            lblStockActual.Location = new Point(12, 318);
            lblStockActual.Name = "lblStockActual";
            lblStockActual.Size = new Size(38, 15);
            lblStockActual.Text = "—";
            lblStockActual.Visible = false;
            //
            // btnAceptar
            //
            btnAceptar.DialogResult = DialogResult.OK;
            btnAceptar.Location = new Point(339, 377);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(85, 28);
            btnAceptar.Text = "Aceptar";
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(430, 377);
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
            ClientSize = new Size(527, 420);
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
            Controls.Add(btnQuitarImagen);
            Controls.Add(btnPegarPortapapeles);
            Controls.Add(btnCargarImagen);
            Controls.Add(picImagen);
            Controls.Add(lblImagen);
            Controls.Add(txtVariante);
            Controls.Add(lblVariante);
            Controls.Add(txtNuevoGrupo);
            Controls.Add(lblNuevoGrupo);
            Controls.Add(cboGrupo);
            Controls.Add(lblGrupo);
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
            ((System.ComponentModel.ISupportInitialize)picImagen).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblGrupo;
        private ComboBox cboGrupo;
        private Label lblNuevoGrupo;
        private TextBox txtNuevoGrupo;
        private Label lblVariante;
        private TextBox txtVariante;
        private Label lblImagen;
        private PictureBox picImagen;
        private Button btnCargarImagen;
        private Button btnPegarPortapapeles;
        private Button btnQuitarImagen;
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
