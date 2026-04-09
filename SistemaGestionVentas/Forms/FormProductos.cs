using System.Drawing;
using SistemaGestionVentas.Models;
using SistemaGestionVentas.Services;
using SistemaGestionVentas.UI;

namespace SistemaGestionVentas.Forms;

public partial class FormProductos : Form
{
    private const string ColFotoName = "colFoto";
    private const string ColGrupoName = "colGrupo";

    private readonly ProductService _productService;
    private readonly Dictionary<int, Image> _thumbCache = new();

    public FormProductos() : this(new ProductService(Program.DbContextOptions))
    {
    }

    public FormProductos(ProductService productService)
    {
        _productService = productService;
        InitializeComponent();
        grid.CellFormatting += Grid_CellFormatting;
        FormClosed += (_, _) => ClearThumbCache();
    }

    private void FormProductos_Load(object sender, EventArgs e)
    {
        UiTheme.ApplyForm(this);
        UiTheme.StyleFooterPanel(panel1);
        UiTheme.StylePrimaryButton(btnNuevo);
        UiTheme.StyleSecondaryButton(btnEditar);
        UiTheme.StyleDangerButton(btnEliminar);
        UiTheme.StyleSecondaryButton(btnCompraIngreso);
        UiTheme.StyleMutedButton(btnCerrar);
        RefreshGrid();
    }

    private void ClearThumbCache()
    {
        foreach (var img in _thumbCache.Values)
            img.Dispose();
        _thumbCache.Clear();
    }

    private void RebuildThumbnails(IEnumerable<Product> list)
    {
        ClearThumbCache();
        foreach (var p in list)
        {
            if (p.ImageData is not { Length: > 0 } data)
                continue;
            try
            {
                using var ms = new MemoryStream(data);
                using var full = Image.FromStream(ms);
                _thumbCache[p.Id] = new Bitmap(full, new Size(48, 48));
            }
            catch
            {
                // imagen corrupta
            }
        }
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex < 0 || e.RowIndex < 0)
            return;
        var colName = grid.Columns[e.ColumnIndex].Name;
        if (grid.Rows[e.RowIndex].DataBoundItem is not Product p)
            return;
        if (colName == ColFotoName)
            e.Value = _thumbCache.TryGetValue(p.Id, out var img) ? img : null;
        else if (colName == ColGrupoName)
            e.Value = p.ProductGroup?.Name ?? "—";
    }

    private void RefreshGrid()
    {
        var list = _productService.GetAll();
        RebuildThumbnails(list);
        grid.AutoGenerateColumns = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.Columns.Clear();
        grid.RowTemplate.Height = 52;

        static DataGridViewTextBoxColumn Txt(string prop, string header, int minW, int fill, string? format = null)
        {
            var c = new DataGridViewTextBoxColumn
            {
                DataPropertyName = prop,
                HeaderText = header,
                MinimumWidth = minW,
                FillWeight = fill,
                ReadOnly = true
            };
            if (format != null)
                c.DefaultCellStyle = new DataGridViewCellStyle { Format = format };
            return c;
        }

        var colFoto = new DataGridViewImageColumn
        {
            Name = ColFotoName,
            HeaderText = "Foto",
            ImageLayout = DataGridViewImageCellLayout.Zoom,
            MinimumWidth = 56,
            FillWeight = 48,
            ReadOnly = true
        };
        grid.Columns.Add(colFoto);
        grid.Columns.Add(Txt(nameof(Product.Id), "Id", 44, 36));
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = ColGrupoName,
            HeaderText = "Línea",
            MinimumWidth = 92,
            FillWeight = 100,
            ReadOnly = true
        });
        grid.Columns.Add(Txt(nameof(Product.Name), "Nombre", 110, 200));
        grid.Columns.Add(Txt(nameof(Product.VariantLabel), "Modelo", 100, 130));
        grid.Columns.Add(Txt(nameof(Product.CostPrice), "Costo prom.", 102, 110, "N2"));
        grid.Columns.Add(Txt(nameof(Product.ListSalePrice), "Venta lista", 96, 105, "N2"));
        grid.Columns.Add(Txt(nameof(Product.StockQuantity), "Stock", 56, 55));
        grid.Columns.Add(Txt(nameof(Product.MinStockThreshold), "Mín.", 52, 48));

        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.DataSource = list;
        UiTheme.StyleDataGridView(grid);
    }

    private Product? SelectedProduct()
    {
        if (grid.CurrentRow?.DataBoundItem is Product p)
            return p;
        return null;
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        using var dlg = new FormProductoEdit(_productService);
        dlg.SetForCreate();
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            var groupId = _productService.ResolveProductGroupId(dlg.NuevoGrupoNombre, dlg.GrupoSeleccionadoId);
            _productService.Create(groupId, dlg.Nombre, dlg.Variante, dlg.ImageData, dlg.CostPrice, dlg.ListSalePrice, dlg.InitialStock, dlg.MinStockThreshold);
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

        using var dlg = new FormProductoEdit(_productService);
        dlg.SetForEdit(fresh.StockQuantity, fresh.ProductGroupId, fresh.Name, fresh.VariantLabel, fresh.ImageData, fresh.CostPrice, fresh.ListSalePrice, fresh.MinStockThreshold);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            var groupId = _productService.ResolveProductGroupId(dlg.NuevoGrupoNombre, dlg.GrupoSeleccionadoId);
            _productService.Update(fresh.Id, groupId, dlg.Nombre, dlg.Variante, dlg.ImageData, dlg.CostPrice, dlg.ListSalePrice, dlg.MinStockThreshold);
            RefreshGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCompraIngreso_Click(object sender, EventArgs e)
    {
        var p = SelectedProduct();
        if (p == null)
        {
            MessageBox.Show("Seleccioná un producto para registrar la compra y el costo de esa entrada.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var f = new FormMovimientos(p.Id, MovementType.Entrada);
        f.ShowDialog(this);
        RefreshGrid();
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
