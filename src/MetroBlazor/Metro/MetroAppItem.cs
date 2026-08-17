namespace MetroBlazor;

public sealed class MetroAppItem
{
    public string Label { get; set; } = string.Empty;
    public string? SubLabel { get; set; }
    public string? Icon { get; set; }
    public string? AccentColor { get; set; }
    public MetroTileSize Size { get; set; } = MetroTileSize.Medium;

    /// <summary>Explicit cell coordinates (60px base tracks) for free placement in a
    /// Rows-mode MetroTileGroup. Null means "auto-place me" in column-major order.</summary>
    public int? GridX { get; set; }
    public int? GridY { get; set; }
}
