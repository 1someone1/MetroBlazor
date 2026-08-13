namespace MetroBlazor;

public sealed class MetroAppItem
{
    public string Label { get; set; } = string.Empty;
    public string? SubLabel { get; set; }
    public string? Icon { get; set; }
    public string? AccentColor { get; set; }
    public MetroTileSize Size { get; set; } = MetroTileSize.Medium;
}
