namespace MetroBlazor;

/// <summary>
/// Visual weight of a MetroIcon. Windows 8.1 defaults to solid filled glyphs;
/// Outline is reserved for quieter contexts such as navigation group headers.
/// Falls back to the filled path when an icon has no outline variant.
/// </summary>
public enum MetroIconVariant
{
    Filled,
    Outline
}
