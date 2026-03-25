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
            grid = new DataGridView();
            panel1 = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
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
            panel1.Controls.Add(btnEliminar);
            panel1.Controls.Add(btnEditar);
            panel1.Controls.Add(btnNuevo);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 410);
            panel1.Name = "panel1";
            panel1.Size = new Size(884, 44);
            //
            // btnNuevo
            //
            btnNuevo.Location = new Point(12, 8);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(90, 28);
            btnNuevo.Text = "Nuevo";
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnEditar
            //
            btnEditar.Location = new Point(108, 8);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(90, 28);
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;
            //
            // btnEliminar
            //
            btnEliminar.Location = new Point(204, 8);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(90, 28);
            btnEliminar.Text = "Eliminar";
            btnEliminar.Click += btnEliminar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Location = new Point(782, 8);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(90, 28);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;
            //
            // FormProductos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 454);
            Controls.Add(grid);
            Controls.Add(panel1);
            Name = "FormProductos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Productos";
            Load += FormProductos_Load;
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private DataGridView grid;
        private Panel panel1;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnCerrar;
    }
}
