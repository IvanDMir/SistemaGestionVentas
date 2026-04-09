using SistemaGestionVentas.Models;
using SistemaGestionVentas.Services;
using SistemaGestionVentas.UI;

namespace SistemaGestionVentas.Forms;

public partial class FormMovimientos : Form
{
    private readonly ProductService _productService;
    private readonly InventoryService _inventoryService;
    private readonly int? _preselectProductId;
    private readonly MovementType? _initialMovementType;

    public FormMovimientos()
        : this(new ProductService(Program.DbContextOptions), new InventoryService(Program.DbContextOptions), null, null)
    {
    }

    /// <param name="preselectProductId">Si tiene valor, el combo abre con ese producto.</param>
    /// <param name="initialMovementType">Si tiene valor, selecciona ese tipo de movimiento.</param>
    public FormMovimientos(int? preselectProductId, MovementType? initialMovementType = null)
        : this(new ProductService(Program.DbContextOptions), new InventoryService(Program.DbContextOptions), preselectProductId, initialMovementType)
    {
    }

    public FormMovimientos(ProductService productService, InventoryService inventoryService, int? preselectProductId = null, MovementType? initialMovementType = null)
    {
        _productService = productService;
        _inventoryService = inventoryService;
        _preselectProductId = preselectProductId;
        _initialMovementType = initialMovementType;
        InitializeComponent();
        cboProducto.FormattingEnabled = true;
        cboProducto.Format += CboProducto_Format;
    }

    private void FormMovimientos_Load(object sender, EventArgs e)
    {
        UiTheme.ApplyForm(this);
        UiTheme.StyleCombo(cboProducto);
        UiTheme.StyleCombo(cboTipo);
        UiTheme.StyleNumeric(numCantidad);
        UiTheme.StyleNumeric(numPrecioVenta);
        UiTheme.StyleTextBox(txtNota);
        UiTheme.StylePrimaryButton(btnRegistrar);
        UiTheme.StyleMutedButton(btnCerrar);
        foreach (var l in new[] { lblProducto, lblTipo, lblCantidad, lblPrecioVenta, lblNota })
            UiTheme.StyleFieldLabel(l);

        cboTipo.Items.Clear();
        cboTipo.Items.Add(new MovementListItem(MovementType.Entrada, "Compra / ingreso (suma stock y recalcula costo promedio)"));
        cboTipo.Items.Add(new MovementListItem(MovementType.Salida, "Salida (sin venta)"));
        cboTipo.Items.Add(new MovementListItem(MovementType.Venta, "Venta"));
        cboTipo.Items.Add(new MovementListItem(MovementType.AjustePositivo, "Ajuste: sumar stock (mismo costo promedio)"));
        cboTipo.Items.Add(new MovementListItem(MovementType.AjusteNegativo, "Ajuste: restar stock"));
        if (_initialMovementType is MovementType mt0)
        {
            for (var i = 0; i < cboTipo.Items.Count; i++)
            {
                if (cboTipo.Items[i] is MovementListItem item && item.Type == mt0)
                {
                    cboTipo.SelectedIndex = i;
                    break;
                }
            }
        }
        else
            cboTipo.SelectedIndex = 0;

        ReloadProducts();
        if (_preselectProductId is int pid)
        {
            try
            {
                cboProducto.SelectedValue = pid;
            }
            catch
            {
                /* id inválido o lista vacía */
            }
        }

        UpdatePrecioUnitarioUi();
    }

    private void ReloadProducts()
    {
        var products = _productService.GetAll();
        cboProducto.DataSource = null;
        cboProducto.DisplayMember = nameof(Product.Name);
        cboProducto.ValueMember = nameof(Product.Id);
        cboProducto.DataSource = products;
    }

    private static void CboProducto_Format(object? sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is not Product p)
            return;
        var line = p.ProductGroup?.Name;
        var v = p.VariantLabel;
        if (!string.IsNullOrEmpty(line) && !string.IsNullOrEmpty(v))
            e.Value = $"{line} › {p.Name} — {v}";
        else if (!string.IsNullOrEmpty(line))
            e.Value = $"{line} › {p.Name}";
        else if (!string.IsNullOrEmpty(v))
            e.Value = $"{p.Name} ({v})";
        else
            e.Value = p.Name;
    }

    private void cboTipo_SelectedIndexChanged(object sender, EventArgs e) => UpdatePrecioUnitarioUi();

    private void UpdatePrecioUnitarioUi()
    {
        var tipo = SelectedType();
        var needPrice = tipo is MovementType.Venta or MovementType.Entrada;
        lblPrecioVenta.Enabled = needPrice;
        numPrecioVenta.Enabled = needPrice;
        if (cboProducto.SelectedItem is not Product p)
            return;
        if (tipo == MovementType.Venta)
        {
            lblPrecioVenta.Text = "Precio unitario venta";
            numPrecioVenta.Value = p.ListSalePrice;
        }
        else if (tipo == MovementType.Entrada)
        {
            lblPrecioVenta.Text = "Costo unitario de esta compra (nuevo precio)";
            numPrecioVenta.Value = p.CostPrice;
        }
        else
            lblPrecioVenta.Text = "Precio unitario";
    }

    private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdatePrecioUnitarioUi();
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
        var unitario = tipo is MovementType.Venta or MovementType.Entrada ? numPrecioVenta.Value : 0m;
        var nota = txtNota.Text;

        try
        {
            _inventoryService.RegisterMovement(productId, tipo, qty, unitario, nota);
            var msg = "Movimiento registrado.";
            if (tipo == MovementType.Entrada)
            {
                var p = _productService.GetById(productId);
                if (p != null)
                    msg += $"{Environment.NewLine}Costo promedio actual: {p.CostPrice:N2}{Environment.NewLine}Stock actual: {p.StockQuantity}";
            }

            MessageBox.Show(msg, "Movimientos", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
