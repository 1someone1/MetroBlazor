namespace MetroBlazor;

public sealed class MetroNavigationItem
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Group { get; set; }
    public IReadOnlyList<MetroNavigationItem>? Children { get; set; }
}
