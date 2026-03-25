namespace SistemaGestionVentas.Forms;

public partial class FormProductoEdit : Form
{
    public FormProductoEdit()
    {
        InitializeComponent();
    }

    public bool IsEditMode { get; private set; }

    public string Nombre => txtNombre.Text.Trim();
    public string? Sku => string.IsNullOrWhiteSpace(txtSku.Text) ? null : txtSku.Text.Trim();
    public decimal CostPrice => numCosto.Value;
    public decimal ListSalePrice => numPrecioLista.Value;
    public int InitialStock => (int)numStockInicial.Value;
    public int MinStockThreshold => (int)numMinimo.Value;

    public void SetForCreate()
    {
        IsEditMode = false;
        Text = "Nuevo producto";
        numStockInicial.Enabled = true;
        numStockInicial.Value = 0;
        lblStockActual.Visible = false;
    }

    public void SetForEdit(int stockActual, string name, string? sku, decimal cost, decimal list, int minStock)
    {
        IsEditMode = true;
        Text = "Editar producto";
        txtNombre.Text = name;
        txtSku.Text = sku ?? string.Empty;
        numCosto.Value = cost;
        numPrecioLista.Value = list;
        numMinimo.Value = Math.Max(0, minStock);
        numStockInicial.Value = Math.Max(0, stockActual);
        numStockInicial.Enabled = false;
        lblStockActual.Text = $"Stock actual (solo movimientos): {stockActual}";
        lblStockActual.Visible = true;
    }

    private void btnAceptar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            MessageBox.Show("Ingresá un nombre.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }
        DialogResult = DialogResult.OK;
    }
}
