namespace SistemaGestionVentas.Forms
{
    partial class FormReportes
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
            tabs = new TabControl();
            tabStockBajo = new TabPage();
            gridStockBajo = new DataGridView();
            tabValor = new TabPage();
            lblValorTotal = new Label();
            tabVendidos = new TabPage();
            gridVendidos = new DataGridView();
            tabMovidos = new TabPage();
            gridMovidos = new DataGridView();
            tabHistorial = new TabPage();
            lblProdHist = new Label();
            cboProductoHistorial = new ComboBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnHistorial = new Button();
            gridHistorial = new DataGridView();
            tabMargen = new TabPage();
            gridMargen = new DataGridView();
            panelBottom = new Panel();
            btnActualizar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)gridStockBajo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridVendidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridMovidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridHistorial).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridMargen).BeginInit();
            tabs.SuspendLayout();
            tabStockBajo.SuspendLayout();
            tabValor.SuspendLayout();
            tabVendidos.SuspendLayout();
            tabMovidos.SuspendLayout();
            tabHistorial.SuspendLayout();
            tabMargen.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            //
            // tabs
            //
            tabs.Controls.Add(tabStockBajo);
            tabs.Controls.Add(tabValor);
            tabs.Controls.Add(tabVendidos);
            tabs.Controls.Add(tabMovidos);
            tabs.Controls.Add(tabHistorial);
            tabs.Controls.Add(tabMargen);
            tabs.Dock = DockStyle.Fill;
            tabs.Location = new Point(0, 0);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(900, 506);
            //
            // tabStockBajo
            //
            tabStockBajo.Controls.Add(gridStockBajo);
            tabStockBajo.Location = new Point(4, 24);
            tabStockBajo.Name = "tabStockBajo";
            tabStockBajo.Padding = new Padding(3);
            tabStockBajo.Size = new Size(892, 478);
            tabStockBajo.Text = "Stock bajo";
            tabStockBajo.UseVisualStyleBackColor = true;
            //
            // gridStockBajo
            //
            gridStockBajo.AllowUserToAddRows = false;
            gridStockBajo.AllowUserToDeleteRows = false;
            gridStockBajo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridStockBajo.Dock = DockStyle.Fill;
            gridStockBajo.Location = new Point(3, 3);
            gridStockBajo.Name = "gridStockBajo";
            gridStockBajo.ReadOnly = true;
            gridStockBajo.RowHeadersVisible = false;
            gridStockBajo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridStockBajo.Size = new Size(886, 472);
            //
            // tabValor
            //
            tabValor.Controls.Add(lblValorTotal);
            tabValor.Location = new Point(4, 24);
            tabValor.Name = "tabValor";
            tabValor.Padding = new Padding(3);
            tabValor.Size = new Size(892, 478);
            tabValor.Text = "Valor inventario";
            tabValor.UseVisualStyleBackColor = true;
            //
            // lblValorTotal
            //
            lblValorTotal.AutoSize = true;
            lblValorTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblValorTotal.Location = new Point(16, 20);
            lblValorTotal.Name = "lblValorTotal";
            lblValorTotal.Size = new Size(18, 20);
            lblValorTotal.Text = "—";
            //
            // tabVendidos
            //
            tabVendidos.Controls.Add(gridVendidos);
            tabVendidos.Location = new Point(4, 24);
            tabVendidos.Name = "tabVendidos";
            tabVendidos.Padding = new Padding(3);
            tabVendidos.Size = new Size(892, 478);
            tabVendidos.Text = "Más vendidos";
            tabVendidos.UseVisualStyleBackColor = true;
            //
            // gridVendidos
            //
            gridVendidos.AllowUserToAddRows = false;
            gridVendidos.AllowUserToDeleteRows = false;
            gridVendidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridVendidos.Dock = DockStyle.Fill;
            gridVendidos.Location = new Point(3, 3);
            gridVendidos.Name = "gridVendidos";
            gridVendidos.ReadOnly = true;
            gridVendidos.RowHeadersVisible = false;
            gridVendidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridVendidos.Size = new Size(886, 472);
            //
            // tabMovidos
            //
            tabMovidos.Controls.Add(gridMovidos);
            tabMovidos.Location = new Point(4, 24);
            tabMovidos.Name = "tabMovidos";
            tabMovidos.Padding = new Padding(3);
            tabMovidos.Size = new Size(892, 478);
            tabMovidos.Text = "Más movidos (egresos)";
            tabMovidos.UseVisualStyleBackColor = true;
            //
            // gridMovidos
            //
            gridMovidos.AllowUserToAddRows = false;
            gridMovidos.AllowUserToDeleteRows = false;
            gridMovidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridMovidos.Dock = DockStyle.Fill;
            gridMovidos.Location = new Point(3, 3);
            gridMovidos.Name = "gridMovidos";
            gridMovidos.ReadOnly = true;
            gridMovidos.RowHeadersVisible = false;
            gridMovidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridMovidos.Size = new Size(886, 472);
            //
            // tabHistorial
            //
            tabHistorial.Controls.Add(gridHistorial);
            tabHistorial.Controls.Add(btnHistorial);
            tabHistorial.Controls.Add(dtpHasta);
            tabHistorial.Controls.Add(lblHasta);
            tabHistorial.Controls.Add(dtpDesde);
            tabHistorial.Controls.Add(lblDesde);
            tabHistorial.Controls.Add(cboProductoHistorial);
            tabHistorial.Controls.Add(lblProdHist);
            tabHistorial.Location = new Point(4, 24);
            tabHistorial.Name = "tabHistorial";
            tabHistorial.Padding = new Padding(3);
            tabHistorial.Size = new Size(892, 478);
            tabHistorial.Text = "Historial";
            tabHistorial.UseVisualStyleBackColor = true;
            //
            // lblProdHist
            //
            lblProdHist.AutoSize = true;
            lblProdHist.Location = new Point(6, 10);
            lblProdHist.Name = "lblProdHist";
            lblProdHist.Size = new Size(55, 15);
            lblProdHist.Text = "Producto";
            //
            // cboProductoHistorial
            //
            cboProductoHistorial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProductoHistorial.FormattingEnabled = true;
            cboProductoHistorial.Location = new Point(6, 28);
            cboProductoHistorial.Name = "cboProductoHistorial";
            cboProductoHistorial.Size = new Size(280, 23);
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(300, 10);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(300, 28);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(120, 23);
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(430, 10);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(37, 15);
            lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(430, 28);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(120, 23);
            //
            // btnHistorial
            //
            btnHistorial.Location = new Point(560, 24);
            btnHistorial.Name = "btnHistorial";
            btnHistorial.Size = new Size(120, 28);
            btnHistorial.Text = "Aplicar filtros";
            btnHistorial.Click += btnHistorial_Click;
            //
            // gridHistorial
            //
            gridHistorial.AllowUserToAddRows = false;
            gridHistorial.AllowUserToDeleteRows = false;
            gridHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridHistorial.Location = new Point(3, 60);
            gridHistorial.Name = "gridHistorial";
            gridHistorial.ReadOnly = true;
            gridHistorial.RowHeadersVisible = false;
            gridHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridHistorial.Size = new Size(886, 415);
            //
            // tabMargen
            //
            tabMargen.Controls.Add(gridMargen);
            tabMargen.Location = new Point(4, 24);
            tabMargen.Name = "tabMargen";
            tabMargen.Padding = new Padding(3);
            tabMargen.Size = new Size(892, 478);
            tabMargen.Text = "Margen / ROI";
            tabMargen.UseVisualStyleBackColor = true;
            //
            // gridMargen
            //
            gridMargen.AllowUserToAddRows = false;
            gridMargen.AllowUserToDeleteRows = false;
            gridMargen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridMargen.Dock = DockStyle.Fill;
            gridMargen.Location = new Point(3, 3);
            gridMargen.Name = "gridMargen";
            gridMargen.ReadOnly = true;
            gridMargen.RowHeadersVisible = false;
            gridMargen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridMargen.Size = new Size(886, 472);
            //
            // panelBottom
            //
            panelBottom.Controls.Add(btnCerrar);
            panelBottom.Controls.Add(btnActualizar);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 494);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(900, 56);
            //
            // btnActualizar
            //
            btnActualizar.Location = new Point(16, 12);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(150, 34);
            btnActualizar.Text = "Actualizar todo";
            btnActualizar.Click += btnActualizar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Location = new Point(734, 12);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 34);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;
            //
            // FormReportes
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 562);
            Controls.Add(tabs);
            Controls.Add(panelBottom);
            Name = "FormReportes";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reportes";
            Load += FormReportes_Load;
            ((System.ComponentModel.ISupportInitialize)gridStockBajo).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridVendidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridMovidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridHistorial).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridMargen).EndInit();
            tabStockBajo.ResumeLayout(false);
            tabValor.ResumeLayout(false);
            tabValor.PerformLayout();
            tabVendidos.ResumeLayout(false);
            tabMovidos.ResumeLayout(false);
            tabHistorial.ResumeLayout(false);
            tabHistorial.PerformLayout();
            tabMargen.ResumeLayout(false);
            tabs.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TabControl tabs;
        private TabPage tabStockBajo;
        private DataGridView gridStockBajo;
        private TabPage tabValor;
        private Label lblValorTotal;
        private TabPage tabVendidos;
        private DataGridView gridVendidos;
        private TabPage tabMovidos;
        private DataGridView gridMovidos;
        private TabPage tabHistorial;
        private Label lblProdHist;
        private ComboBox cboProductoHistorial;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnHistorial;
        private DataGridView gridHistorial;
        private TabPage tabMargen;
        private DataGridView gridMargen;
        private Panel panelBottom;
        private Button btnActualizar;
        private Button btnCerrar;
    }
}
