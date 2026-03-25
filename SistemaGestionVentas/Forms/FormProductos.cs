using SistemaGestionVentas.Models;
using SistemaGestionVentas.Services;

namespace SistemaGestionVentas.Forms;

public partial class FormProductos : Form
{
    private readonly ProductService _productService;

    public FormProductos() : this(new ProductService(Program.DbContextOptions))
    {
    }

    public FormProductos(ProductService productService)
    {
        _productService = productService;
        InitializeComponent();
    }

    private void FormProductos_Load(object sender, EventArgs e) => RefreshGrid();

    private void RefreshGrid()
    {
        var list = _productService.GetAll();
        grid.AutoGenerateColumns = false;
        grid.Columns.Clear();
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.Id), HeaderText = "Id", Width = 40, ReadOnly = true });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.Name), HeaderText = "Nombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.Sku), HeaderText = "SKU", Width = 90, ReadOnly = true });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.CostPrice), HeaderText = "Compra", Width = 70, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.ListSalePrice), HeaderText = "Venta lista", Width = 80, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.StockQuantity), HeaderText = "Stock", Width = 55, ReadOnly = true });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.MinStockThreshold), HeaderText = "Mín.", Width = 45, ReadOnly = true });
        grid.DataSource = list;
    }

    private Product? SelectedProduct()
    {
        if (grid.CurrentRow?.DataBoundItem is Product p)
            return p;
        return null;
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        using var dlg = new FormProductoEdit();
        dlg.SetForCreate();
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            _productService.Create(dlg.Nombre, dlg.Sku, dlg.CostPrice, dlg.ListSalePrice, dlg.InitialStock, dlg.MinStockThreshold);
            RefreshGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
        var p = SelectedProduct();
        if (p == null)
        {
            MessageBox.Show("Seleccioná un producto.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var fresh = _productService.GetById(p.Id);
        if (fresh == null) return;

        using var dlg = new FormProductoEdit();
        dlg.SetForEdit(fresh.StockQuantity, fresh.Name, fresh.Sku, fresh.CostPrice, fresh.ListSalePrice, fresh.MinStockThreshold);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            _productService.Update(fresh.Id, dlg.Nombre, dlg.Sku, dlg.CostPrice, dlg.ListSalePrice, dlg.MinStockThreshold);
            RefreshGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        var p = SelectedProduct();
        if (p == null)
        {
            MessageBox.Show("Seleccioná un producto.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (MessageBox.Show($"¿Eliminar \"{p.Name}\"?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        try
        {
            _productService.Delete(p.Id);
            RefreshGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCerrar_Click(object sender, EventArgs e) => Close();
}
