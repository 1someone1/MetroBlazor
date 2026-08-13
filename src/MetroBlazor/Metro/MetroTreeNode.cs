namespace MetroBlazor;

public sealed class MetroTreeNode
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public IReadOnlyList<MetroTreeNode> Children { get; set; } = Array.Empty<MetroTreeNode>();
}
