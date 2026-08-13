namespace MetroBlazor;

public sealed class MetroCommandItem
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public bool Disabled { get; set; }
}
