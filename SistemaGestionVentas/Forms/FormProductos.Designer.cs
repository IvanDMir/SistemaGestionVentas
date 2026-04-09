namespace SistemaGestionVentas.Forms
{
    partial class FormProductos
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
            layoutRoot = new TableLayoutPanel();
            grid = new DataGridView();
            panel1 = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnCompraIngreso = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            layoutRoot.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            //
            // layoutRoot
            //
            layoutRoot.ColumnCount = 1;
            layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layoutRoot.Controls.Add(grid, 0, 0);
            layoutRoot.Controls.Add(panel1, 0, 1);
            layoutRoot.Dock = DockStyle.Fill;
            layoutRoot.Location = new Point(0, 0);
            layoutRoot.Name = "layoutRoot";
            layoutRoot.RowCount = 2;
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            layoutRoot.Size = new Size(884, 466);
            layoutRoot.TabIndex = 0;
            //
            // grid
            //
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(0, 0);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //
            // panel1
            //
            panel1.Controls.Add(btnCerrar);
            panel1.Controls.Add(btnCompraIngreso);
            panel1.Controls.Add(btnEliminar);
            panel1.Controls.Add(btnEditar);
            panel1.Controls.Add(btnNuevo);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 410);
            panel1.Name = "panel1";
            panel1.Size = new Size(884, 56);
            //
            // btnNuevo
            //
            btnNuevo.Location = new Point(16, 12);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(90, 28);
            btnNuevo.Text = "Nuevo";
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnEditar
            //
            btnEditar.Location = new Point(118, 12);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(90, 28);
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;
            //
            // btnEliminar
            //
            btnEliminar.Location = new Point(220, 12);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(90, 28);
            btnEliminar.Text = "Eliminar";
            btnEliminar.Click += btnEliminar_Click;
            //
            // btnCompraIngreso
            //
            btnCompraIngreso.Location = new Point(322, 12);
            btnCompraIngreso.Name = "btnCompraIngreso";
            btnCompraIngreso.Size = new Size(168, 28);
            btnCompraIngreso.Text = "Compra / ingreso";
            btnCompraIngreso.Click += btnCompraIngreso_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Location = new Point(772, 12);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(90, 28);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;
            //
            // FormProductos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 466);
            Controls.Add(layoutRoot);
            MinimumSize = new Size(640, 400);
            Name = "FormProductos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Productos";
            Load += FormProductos_Load;
            layoutRoot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TableLayoutPanel layoutRoot;
        private DataGridView grid;
        private Panel panel1;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnCompraIngreso;
        private Button btnCerrar;
    }
}
