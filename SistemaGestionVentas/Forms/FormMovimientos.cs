using SistemaGestionVentas.Models;
using SistemaGestionVentas.Services;

namespace SistemaGestionVentas.Forms;

public partial class FormMovimientos : Form
{
    private readonly ProductService _productService;
    private readonly InventoryService _inventoryService;

    public FormMovimientos()
        : this(new ProductService(Program.DbContextOptions), new InventoryService(Program.DbContextOptions))
    {
    }

    public FormMovimientos(ProductService productService, InventoryService inventoryService)
    {
        _productService = productService;
        _inventoryService = inventoryService;
        InitializeComponent();
    }

    private void FormMovimientos_Load(object sender, EventArgs e)
    {
        cboTipo.Items.Clear();
        cboTipo.Items.Add(new MovementListItem(MovementType.Entrada, "Entrada (compra / ingreso)"));
        cboTipo.Items.Add(new MovementListItem(MovementType.Salida, "Salida (sin venta)"));
        cboTipo.Items.Add(new MovementListItem(MovementType.Venta, "Venta"));
        cboTipo.Items.Add(new MovementListItem(MovementType.AjustePositivo, "Ajuste: sumar stock"));
        cboTipo.Items.Add(new MovementListItem(MovementType.AjusteNegativo, "Ajuste: restar stock"));
        cboTipo.SelectedIndex = 0;
        ReloadProducts();
        UpdatePrecioVentaEnabled();
    }

    private void ReloadProducts()
    {
        var products = _productService.GetAll();
        cboProducto.DataSource = products;
        cboProducto.DisplayMember = nameof(Product.Name);
        cboProducto.ValueMember = nameof(Product.Id);
    }

    private void cboTipo_SelectedIndexChanged(object sender, EventArgs e) => UpdatePrecioVentaEnabled();

    private void UpdatePrecioVentaEnabled()
    {
        var isVenta = SelectedType() == MovementType.Venta;
        lblPrecioVenta.Enabled = isVenta;
        numPrecioVenta.Enabled = isVenta;
        if (cboProducto.SelectedItem is Product p && isVenta)
            numPrecioVenta.Value = p.ListSalePrice;
    }

    private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SelectedType() == MovementType.Venta && cboProducto.SelectedItem is Product p)
            numPrecioVenta.Value = p.ListSalePrice;
    }

    private MovementType SelectedType() =>
        (cboTipo.SelectedItem as MovementListItem)?.Type ?? MovementType.Entrada;

    private void btnRegistrar_Click(object sender, EventArgs e)
    {
        if (cboProducto.SelectedValue is not int productId)
        {
            MessageBox.Show("Seleccioná un producto.", "Movimientos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var tipo = SelectedType();
        var qty = (int)numCantidad.Value;
        var precioVenta = tipo == MovementType.Venta ? numPrecioVenta.Value : 0m;
        var nota = txtNota.Text;

        try
        {
            _inventoryService.RegisterMovement(productId, tipo, qty, precioVenta, nota);
            MessageBox.Show("Movimiento registrado.", "Movimientos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReloadProducts();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCerrar_Click(object sender, EventArgs e) => Close();

    private sealed record MovementListItem(MovementType Type, string Label)
    {
        public override string ToString() => Label;
    }
}
