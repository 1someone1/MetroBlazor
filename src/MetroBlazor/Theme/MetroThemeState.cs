namespace MetroBlazor;

public sealed class MetroThemeState
{
    private MetroThemeVariant variant;

    public MetroThemeState(MetroThemeVariant initialVariant)
    {
        variant = initialVariant;
    }

    public event Action? Changed;

    public MetroThemeVariant Variant
    {
        get => variant;
        set
        {
            if (variant == value)
            {
                return;
            }

            variant = value;
            Changed?.Invoke();
        }
    }
}
