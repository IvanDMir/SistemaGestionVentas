using System.Drawing.Imaging;
using SistemaGestionVentas.Services;
using SistemaGestionVentas.UI;

namespace SistemaGestionVentas.Forms;

public partial class FormProductoEdit : Form
{
    private const int MaxImageBytes = 5 * 1024 * 1024;

    private readonly ProductService _productService;
    private byte[]? _imageBytes;

    public FormProductoEdit() : this(new ProductService(Program.DbContextOptions))
    {
    }

    public FormProductoEdit(ProductService productService)
    {
        _productService = productService;
        InitializeComponent();
        Load += FormProductoEdit_Load;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.V))
        {
            if (ActiveControl is TextBoxBase or ComboBox)
                return base.ProcessCmdKey(ref msg, keyData);
            if (TryPasteImageFromClipboard(showErrors: true))
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void FormProductoEdit_Load(object? sender, EventArgs e)
    {
        UiTheme.ApplyForm(this);
        UiTheme.StylePrimaryButton(btnAceptar);
        UiTheme.StyleMutedButton(btnCancelar);
        UiTheme.StyleSecondaryButton(btnCargarImagen);
        UiTheme.StyleSecondaryButton(btnPegarPortapapeles);
        UiTheme.StyleMutedButton(btnQuitarImagen);
        UiTheme.StyleCombo(cboGrupo);
        UiTheme.StyleTextBox(txtNombre);
        UiTheme.StyleTextBox(txtNuevoGrupo);
        UiTheme.StyleTextBox(txtVariante);
        UiTheme.StyleNumeric(numCosto);
        UiTheme.StyleNumeric(numPrecioLista);
        UiTheme.StyleNumeric(numStockInicial);
        UiTheme.StyleNumeric(numMinimo);
        foreach (var l in new[]
                 {
                     lblNombre, lblGrupo, lblNuevoGrupo, lblVariante, lblImagen, lblCosto, lblPrecioLista,
                     lblStockInicial, lblMinimo
                 })
            UiTheme.StyleFieldLabel(l);
        ReloadGrupoCombo();
    }

    private void ReloadGrupoCombo()
    {
        var sel = (cboGrupo.SelectedItem as ComboGroupItem)?.Id;
        cboGrupo.Items.Clear();
        cboGrupo.Items.Add(new ComboGroupItem(null, "(Sin línea / grupo)"));
        foreach (var g in _productService.GetAllGroups())
            cboGrupo.Items.Add(new ComboGroupItem(g.Id, g.Name));
        cboGrupo.DisplayMember = nameof(ComboGroupItem.Display);
        if (sel is int id)
        {
            for (var i = 0; i < cboGrupo.Items.Count; i++)
            {
                if (cboGrupo.Items[i] is ComboGroupItem c && c.Id == id)
                {
                    cboGrupo.SelectedIndex = i;
                    return;
                }
            }
        }
        cboGrupo.SelectedIndex = 0;
    }

    public bool IsEditMode { get; private set; }

    public string Nombre => txtNombre.Text.Trim();
    public string? Variante => string.IsNullOrWhiteSpace(txtVariante.Text) ? null : txtVariante.Text.Trim();
    public string? NuevoGrupoNombre => string.IsNullOrWhiteSpace(txtNuevoGrupo.Text) ? null : txtNuevoGrupo.Text.Trim();
    public int? GrupoSeleccionadoId => (cboGrupo.SelectedItem as ComboGroupItem)?.Id;

    public byte[]? ImageData => _imageBytes;
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
        txtNuevoGrupo.Clear();
        txtVariante.Clear();
        _imageBytes = null;
        SetPicturePreview(null);
        ReloadGrupoCombo();
    }

    public void SetForEdit(int stockActual, int? productGroupId, string name, string? variantLabel, byte[]? imageData, decimal cost, decimal list, int minStock)
    {
        IsEditMode = true;
        Text = "Editar producto";
        txtNombre.Text = name;
        txtVariante.Text = variantLabel ?? string.Empty;
        txtNuevoGrupo.Clear();
        ReloadGrupoCombo();
        if (productGroupId is int gid)
        {
            for (var i = 0; i < cboGrupo.Items.Count; i++)
            {
                if (cboGrupo.Items[i] is ComboGroupItem c && c.Id == gid)
                {
                    cboGrupo.SelectedIndex = i;
                    break;
                }
            }
        }
        _imageBytes = imageData == null ? null : (byte[])imageData.Clone();
        SetPicturePreview(_imageBytes);
        numCosto.Value = cost;
        numPrecioLista.Value = list;
        numMinimo.Value = Math.Max(0, minStock);
        numStockInicial.Value = Math.Max(0, stockActual);
        numStockInicial.Enabled = false;
        lblStockActual.Text = $"Stock actual (solo movimientos): {stockActual}";
        lblStockActual.Visible = true;
    }

    private void btnCargarImagen_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Elegir imagen del producto",
            Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Todos los archivos|*.*"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            var info = new FileInfo(dlg.FileName);
            if (info.Length > MaxImageBytes)
            {
                MessageBox.Show($"La imagen supera el máximo permitido ({MaxImageBytes / (1024 * 1024)} MB).", "Imagen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bytes = File.ReadAllBytes(dlg.FileName);
            if (!IsValidImage(bytes))
            {
                MessageBox.Show("No se pudo leer como imagen. Probá con PNG o JPEG.", "Imagen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _imageBytes = bytes;
            SetPicturePreview(bytes);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar el archivo: {ex.Message}", "Imagen", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnPegarPortapapeles_Click(object sender, EventArgs e) => TryPasteImageFromClipboard(showErrors: true);

    private void btnQuitarImagen_Click(object sender, EventArgs e)
    {
        _imageBytes = null;
        SetPicturePreview(null);
    }

    private bool TryPasteImageFromClipboard(bool showErrors)
    {
        try
        {
            if (!Clipboard.ContainsImage())
            {
                if (showErrors)
                    MessageBox.Show("El portapapeles no tiene una imagen. Copiá una imagen antes de pegar.", "Portapapeles", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            using var img = Clipboard.GetImage();
            if (img == null)
            {
                if (showErrors)
                    MessageBox.Show("No se pudo leer la imagen del portapapeles.", "Portapapeles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            using var ms = new MemoryStream();
            using (var bmp = new Bitmap(img))
                bmp.Save(ms, ImageFormat.Png);
            var bytes = ms.ToArray();

            if (bytes.Length > MaxImageBytes)
            {
                if (showErrors)
                    MessageBox.Show($"La imagen supera el máximo permitido ({MaxImageBytes / (1024 * 1024)} MB).", "Portapapeles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidImage(bytes))
            {
                if (showErrors)
                    MessageBox.Show("La imagen pegada no es válida.", "Portapapeles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _imageBytes = bytes;
            SetPicturePreview(bytes);
            return true;
        }
        catch (Exception ex)
        {
            if (showErrors)
                MessageBox.Show($"No se pudo pegar: {ex.Message}", "Portapapeles", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    private static bool IsValidImage(byte[] bytes)
    {
        try
        {
            using var ms = new MemoryStream(bytes);
            using var _ = Image.FromStream(ms);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void SetPicturePreview(byte[]? bytes)
    {
        picImagen.Image?.Dispose();
        picImagen.Image = null;
        if (bytes == null || bytes.Length == 0)
            return;
        try
        {
            using var ms = new MemoryStream(bytes);
            using var loaded = Image.FromStream(ms);
            picImagen.Image = new Bitmap(loaded);
        }
        catch
        {
            _imageBytes = null;
        }
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

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        picImagen.Image?.Dispose();
        picImagen.Image = null;
        base.OnFormClosed(e);
    }

    private sealed class ComboGroupItem(int? id, string display)
    {
        public int? Id { get; } = id;
        public string Display { get; } = display;
    }
}
