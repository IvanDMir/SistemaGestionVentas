namespace SistemaGestionVentas.UI;

/// <summary>Colores y estilos coherentes para WinForms.</summary>
public static class UiTheme
{
    public static Color Background { get; } = Color.FromArgb(248, 250, 252);
    public static Color Surface { get; } = Color.White;
    public static Color Border { get; } = Color.FromArgb(226, 232, 240);
    public static Color Primary { get; } = Color.FromArgb(37, 99, 235);
    public static Color PrimaryHover { get; } = Color.FromArgb(29, 78, 216);
    public static Color Secondary { get; } = Color.FromArgb(71, 85, 105);
    public static Color SecondaryHover { get; } = Color.FromArgb(51, 65, 85);
    public static Color Danger { get; } = Color.FromArgb(220, 38, 38);
    public static Color DangerHover { get; } = Color.FromArgb(185, 28, 28);
    public static Color TextPrimary { get; } = Color.FromArgb(15, 23, 42);
    public static Color TextMuted { get; } = Color.FromArgb(100, 116, 139);

    public static Font FontBase { get; } = new("Segoe UI", 10F);
    public static Font FontTitle { get; } = new("Segoe UI", 15F, FontStyle.Bold);
    public static Font FontSubtitle { get; } = new("Segoe UI", 9.5F);

    public static void ApplyForm(Form form)
    {
        form.BackColor = Background;
        form.Font = FontBase;
        form.ForeColor = TextPrimary;
    }

    public static void StyleFooterPanel(Panel panel)
    {
        panel.BackColor = Surface;
        panel.Padding = new Padding(16, 10, 16, 10);
        panel.Height = Math.Max(panel.Height, 52);
    }

    public static void StylePrimaryButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = Primary;
        b.ForeColor = Color.White;
        b.Cursor = Cursors.Hand;
        b.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        b.UseVisualStyleBackColor = false;
        WireHover(b, Primary, PrimaryHover);
    }

    public static void StyleSecondaryButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.BorderColor = Border;
        b.BackColor = Surface;
        b.ForeColor = TextPrimary;
        b.Cursor = Cursors.Hand;
        b.UseVisualStyleBackColor = false;
        WireHover(b, Surface, Color.FromArgb(241, 245, 249));
    }

    public static void StyleDangerButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = Danger;
        b.ForeColor = Color.White;
        b.Cursor = Cursors.Hand;
        b.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        b.UseVisualStyleBackColor = false;
        WireHover(b, Danger, DangerHover);
    }

    public static void StyleMutedButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = Color.FromArgb(241, 245, 249);
        b.ForeColor = Secondary;
        b.Cursor = Cursors.Hand;
        b.UseVisualStyleBackColor = false;
        WireHover(b, Color.FromArgb(241, 245, 249), Color.FromArgb(226, 232, 240));
    }

    private static void WireHover(Button b, Color normal, Color hover)
    {
        void Enter(object? s, EventArgs e) => b.BackColor = hover;
        void Leave(object? s, EventArgs e) => b.BackColor = normal;
        b.MouseEnter -= Enter;
        b.MouseLeave -= Leave;
        b.MouseEnter += Enter;
        b.MouseLeave += Leave;
    }

    public static void StyleDataGridView(DataGridView grid)
    {
        grid.BorderStyle = BorderStyle.None;
        grid.BackgroundColor = Surface;
        grid.GridColor = Border;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.EnableHeadersVisualStyles = false;
        grid.RowHeadersVisible = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;

        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = TextPrimary,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Padding = new Padding(8, 4, 8, 4),
            SelectionBackColor = Color.FromArgb(241, 245, 249),
            SelectionForeColor = TextPrimary
        };

        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Surface,
            ForeColor = TextPrimary,
            Font = FontBase,
            Padding = new Padding(8, 4, 8, 4),
            SelectionBackColor = Color.FromArgb(219, 234, 254),
            SelectionForeColor = Color.FromArgb(30, 58, 138)
        };

        grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(252, 252, 254),
            ForeColor = TextPrimary,
            SelectionBackColor = Color.FromArgb(219, 234, 254),
            SelectionForeColor = Color.FromArgb(30, 58, 138)
        };
    }

    public static void StyleLabelMuted(Label label)
    {
        label.ForeColor = TextMuted;
        label.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
    }

    public static void StyleFieldLabel(Label label)
    {
        label.ForeColor = TextMuted;
        label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    public static void StyleTabControl(TabControl tabs)
    {
        tabs.Padding = new Point(16, 6);
        tabs.Font = new Font("Segoe UI", 9.75F);
        tabs.BackColor = Background;
    }

    public static void StyleCombo(ComboBox cbo)
    {
        cbo.FlatStyle = FlatStyle.Flat;
        cbo.BackColor = Surface;
        cbo.ForeColor = TextPrimary;
    }

    public static void StyleNumeric(NumericUpDown n)
    {
        n.BackColor = Surface;
        n.ForeColor = TextPrimary;
    }

    public static void StyleTextBox(TextBox t)
    {
        t.BorderStyle = BorderStyle.FixedSingle;
        t.BackColor = Surface;
        t.ForeColor = TextPrimary;
    }
}
