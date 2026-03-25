using SistemaGestionVentas.Services;

namespace SistemaGestionVentas.Forms;

public partial class FormReportes : Form
{
    private readonly ReportingService _reporting;
    private readonly ProductService _products;

    public FormReportes()
        : this(new ReportingService(Program.DbContextOptions), new ProductService(Program.DbContextOptions))
    {
    }

    public FormReportes(ReportingService reporting, ProductService products)
    {
        _reporting = reporting;
        _products = products;
        InitializeComponent();
    }

    private void FormReportes_Load(object sender, EventArgs e)
    {
        cboProductoHistorial.Items.Clear();
        cboProductoHistorial.Items.Add(new ProductListItem(null, "(Todos)"));
        foreach (var p in _products.GetAll())
            cboProductoHistorial.Items.Add(new ProductListItem(p.Id, p.Name));
        cboProductoHistorial.SelectedIndex = 0;
        dtpDesde.Value = DateTime.Today.AddMonths(-1);
        dtpHasta.Value = DateTime.Today;
        RefreshAll();
    }

    private void btnActualizar_Click(object sender, EventArgs e) => RefreshAll();

    private void RefreshAll()
    {
        RefreshStockBajo();
        RefreshValorInventario();
        RefreshMasVendidos();
        RefreshMasMovidos();
        RefreshHistorial();
        RefreshMargen();
    }

    private void RefreshStockBajo()
    {
        var rows = _reporting.GetLowStock();
        gridStockBajo.AutoGenerateColumns = false;
        gridStockBajo.Columns.Clear();
        gridStockBajo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LowStockRow.Id), HeaderText = "Id", Width = 40 });
        gridStockBajo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LowStockRow.Name), HeaderText = "Producto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        gridStockBajo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LowStockRow.Stock), HeaderText = "Stock", Width = 60 });
        gridStockBajo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LowStockRow.MinThreshold), HeaderText = "Mínimo", Width = 60 });
        gridStockBajo.DataSource = rows;
    }

    private void RefreshValorInventario()
    {
        var total = _reporting.GetInventoryTotalValue();
        lblValorTotal.Text = $"Valor total del inventario (stock × costo): {total:N2}";
    }

    private void RefreshMasVendidos()
    {
        var rows = _reporting.GetTopSold(30);
        gridVendidos.AutoGenerateColumns = false;
        gridVendidos.Columns.Clear();
        gridVendidos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementStatRow.ProductId), HeaderText = "Id", Width = 40 });
        gridVendidos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementStatRow.ProductName), HeaderText = "Producto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        gridVendidos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementStatRow.TotalUnits), HeaderText = "Unidades vendidas", Width = 120 });
        gridVendidos.DataSource = rows;
    }

    private void RefreshMasMovidos()
    {
        var rows = _reporting.GetTopOutgoingMovementVolume(30);
        gridMovidos.AutoGenerateColumns = false;
        gridMovidos.Columns.Clear();
        gridMovidos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementStatRow.ProductId), HeaderText = "Id", Width = 40 });
        gridMovidos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementStatRow.ProductName), HeaderText = "Producto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        gridMovidos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementStatRow.TotalUnits), HeaderText = "Unidades egresadas", Width = 120 });
        gridMovidos.DataSource = rows;
    }

    private void RefreshHistorial()
    {
        int? pid = (cboProductoHistorial.SelectedItem as ProductListItem)?.Id;
        var from = dtpDesde.Value.Date;
        var to = dtpHasta.Value.Date.AddDays(1).AddTicks(-1);
        var rows = _reporting.GetMovementHistory(pid, from, to);
        gridHistorial.AutoGenerateColumns = false;
        gridHistorial.Columns.Clear();
        gridHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementHistoryRow.OccurredAt), HeaderText = "Fecha", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "g" } });
        gridHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementHistoryRow.ProductName), HeaderText = "Producto", Width = 140 });
        gridHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementHistoryRow.TypeLabel), HeaderText = "Tipo", Width = 90 });
        gridHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementHistoryRow.Quantity), HeaderText = "Cant.", Width = 50 });
        gridHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementHistoryRow.UnitSalePrice), HeaderText = "P. venta u.", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        gridHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementHistoryRow.UnitCostSnapshot), HeaderText = "Costo u.", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        gridHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MovementHistoryRow.Note), HeaderText = "Nota", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        gridHistorial.DataSource = rows;
    }

    private void RefreshMargen()
    {
        var rows = _reporting.GetMarginByProduct();
        gridMargen.AutoGenerateColumns = false;
        gridMargen.Columns.Clear();
        gridMargen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MarginRow.ProductId), HeaderText = "Id", Width = 40 });
        gridMargen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MarginRow.ProductName), HeaderText = "Producto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        gridMargen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MarginRow.TotalRevenue), HeaderText = "Ingresos ventas", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        gridMargen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MarginRow.TotalCostOfSoldUnits), HeaderText = "Costo vendido", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        gridMargen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MarginRow.Profit), HeaderText = "Margen", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        gridMargen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(MarginRow.RoiPercentOnCost), HeaderText = "ROI % s/ costo", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        gridMargen.DataSource = rows;
    }

    private void btnHistorial_Click(object sender, EventArgs e) => RefreshHistorial();

    private void btnCerrar_Click(object sender, EventArgs e) => Close();

    private sealed record ProductListItem(int? Id, string Name)
    {
        public override string ToString() => Name;
    }
}
