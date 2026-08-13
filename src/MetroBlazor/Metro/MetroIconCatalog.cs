namespace MetroBlazor;

public static class MetroIconCatalog
{
    private static readonly IReadOnlyDictionary<string, string> Paths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["home"] = "M3 11.5 12 3l9 8.5V21h-6v-6H9v6H3v-9.5Z",
        ["menu"] = "M3 5h18v2H3V5Zm0 6h18v2H3v-2Zm0 6h18v2H3v-2Z",
        ["search"] = "m10.5 3a7.5 7.5 0 1 0 4.7 13.35l4.73 4.72 1.41-1.41-4.72-4.73A7.5 7.5 0 0 0 10.5 3Zm0 2a5.5 5.5 0 1 1 0 11 5.5 5.5 0 0 1 0-11Z",
        ["settings"] = "m19.43 12.98 1.25.98-1.5 2.6-1.5-.62a7.2 7.2 0 0 1-1.7.98L15.75 18.5h-3l-.23-1.58a7.2 7.2 0 0 1-1.7-.98l-1.5.62-1.5-2.6 1.25-.98A6.8 6.8 0 0 1 9 12c0-.68.1-1.34.28-1.95L8.03 9.07l1.5-2.6 1.5.62c.52-.4 1.1-.73 1.7-.98L12.75 4.5h3l.23 1.58c.6.25 1.18.58 1.7.98l1.5-.62 1.5 2.6-1.25.98c.18.61.28 1.27.28 1.95s-.1 1.34-.28 1.95ZM14.25 9a3 3 0 1 0 0 6 3 3 0 0 0 0-6Z",
        ["add"] = "M11 3h2v8h8v2h-8v8h-2v-8H3v-2h8V3Z",
        ["close"] = "m5.4 4 6.6 6.6L18.6 4 20 5.4 13.4 12l6.6 6.6-1.4 1.4-6.6-6.6L5.4 20 4 18.6l6.6-6.6L4 5.4 5.4 4Z",
        ["check"] = "m9.2 17.2-5-5 1.6-1.6 3.4 3.4 8-8 1.6 1.6-9.6 9.6Z",
        ["arrow-left"] = "m11 5-1.4 1.4 4.6 4.6H3v2h11.2l-4.6 4.6L11 19l7-7-7-7Z",
        ["arrow-right"] = "m13 5 1.4 1.4-4.6 4.6H21v2h-11.2l4.6 4.6L13 19l-7-7 7-7Z",
        ["mail"] = "M3 5h18v14H3V5Zm2 2v.5l7 4.67 7-4.67V7l-7 4.67L5 7Zm0 2.9V17h14V9.9l-7 4.67L5 9.9Z",
        ["calendar"] = "M5 3h2v2h10V3h2v2h2v16H3V5h2V3Zm0 6v10h14V9H5Zm2 3h3v3H7v-3Zm5 0h3v3h-3v-3Z",
        ["folder"] = "M3 5h7l2 2h9v12H3V5Zm2 4v8h14V9H5Z",
        ["document"] = "M5 3h9l5 5v13H5V3Zm2 2v14h10V9h-4V5H7Zm2 7h6v2H9v-2Zm0 4h6v2H9v-2Z",
        ["image"] = "M4 4h16v16H4V4Zm2 2v12h12V6H6Zm2 8 2-2 2 2 2-3 2 3v2H8v-2Zm1-5a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3Z",
        ["video"] = "M3 6h13v12H3V6Zm15 3 3-2v10l-3-2V9Z",
        ["music"] = "M8 18.5a3.5 3.5 0 1 1 2-3.32V6l9-2v10.5a3.5 3.5 0 1 1-2-3.32V6l-5 1.11V18.5a3.5 3.5 0 0 1-4 0Z",
        ["people"] = "M8 12a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm8-1a3 3 0 1 0 0-6 3 3 0 0 0 0 6ZM2 20a6 6 0 0 1 12 0H2Zm13 0a5 5 0 0 1 3-4.58A5 5 0 0 1 22 20h-7Z",
        ["delete"] = "M6 7h12l-1 14H7L6 7Zm3-4h6l1 2h4v2H4V5h4l1-2Zm1 5h2v10h-2V8Zm4 0h2v10h-2V8Z",
        ["edit"] = "m14 5 5 5-10 10H4v-5L14 5Zm1.4-1.4 1.4-1.4a2 2 0 0 1 2.8 0l2.2 2.2a2 2 0 0 1 0 2.8l-1.4 1.4-5-5Z",
        ["save"] = "M5 3h12l4 4v14H3V3h2Zm2 2v5h8V5H7Zm-2 8v6h14v-6H5Z",
        ["download"] = "M11 3h2v9l3-3 1.4 1.4-5.4 5.4-5.4-5.4L8 9l3 3V3ZM4 18h16v2H4v-2Z",
        ["upload"] = "M11 21h2v-9l3 3 1.4-1.4-5.4-5.4-5.4 5.4L8 15l3-3v9ZM4 4h16v2H4V4Z",
        ["info"] = "M11 10h2v7h-2v-7Zm0-4h2v2h-2V6Zm1-3a9 9 0 1 0 0 18 9 9 0 0 0 0-18Z",
        ["warning"] = "m12 3 10 18H2L12 3Zm-1 6h2v6h-2V9Zm0 8h2v2h-2v-2Z",
        ["question"] = "M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18Zm0 16a7 7 0 1 1 0-14 7 7 0 0 1 0 14Zm-1-4h2v2h-2v-2Zm1-9c-1.66 0-3 1.34-3 3h2a1 1 0 1 1 2 0c0-.55-.45-1-1-1a1 1 0 0 0-1 1H9c0-1.66 1.34-3 3-3Z",
        ["tile"] = "M3 3h8v8H3V3Zm10 0h8v8h-8V3ZM3 13h8v8H3v-8Zm10 0h8v8h-8v-8Z",
        ["grid"] = "M3 3h8v8H3V3Zm10 0h8v8h-8V3ZM3 13h8v8H3v-8Zm10 0h8v8h-8v-8Z",
        ["group"] = "M3 3h8v8H3V3Zm10 0h8v8h-8V3ZM3 13h18v2H3v-2Zm0 4h18v2H3v-2Z",
        ["zoom"] = "m10.5 3a7.5 7.5 0 1 0 4.7 13.35l4.73 4.72 1.41-1.41-4.72-4.73A7.5 7.5 0 0 0 10.5 3Zm0 2a5.5 5.5 0 1 1 0 11 5.5 5.5 0 0 1 0-11Z",
        ["type"] = "M5 4h14v2h-6v14h-2V6H5V4Z",
        ["input"] = "M3 5h18v14H3V5Zm2 2v10h14V7H5Z",
        ["toggle"] = "M4 7h16v10H4V7Zm4 2a3 3 0 1 0 0 6 3 3 0 0 0 0-6Z",
        ["progress"] = "M3 5h18v3H3V5Zm0 5h12v3H3v-3Zm0 5h7v3H3v-3Z",
        ["badge"] = "M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18Z",
        ["sidebar"] = "M3 4h18v16H3V4Zm2 2v12h4V6H5Z",
        ["blade"] = "M3 4h18v16H3V4Zm12 2v12h4V6h-4Z",
        ["icon"] = "m12 2 2.1 6.4H21l-5.6 3.9 2.1 6.4-5.5-4-5.5 4 2.1-6.4L3 8.4h6.9L12 2Z",
        ["transition"] = "M3 11h12l-4-4 1.4-1.4L19.8 12l-7.4 6.4L11 17l4-4H3v-2Z",
        ["frame"] = "M3 4h18v16H3V4Zm2 4v10h14V8H5Z",
        ["select"] = "M7 9h10l-5 6-5-6Z",
        ["radio"] = "M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18Zm0 5a4 4 0 1 1 0 8 4 4 0 0 1 0-8Z",
        ["slider"] = "M3 11h18v2H3v-2Zm4-4h2v10H7V7Zm8 0h2v10h-2V7Z",
        ["list"] = "M4 5h2v2H4V5Zm4 0h12v2H8V5ZM4 11h2v2H4v-2Zm4 0h12v2H8v-2ZM4 17h2v2H4v-2Zm4 0h12v2H8v-2Z",
        ["table"] = "M3 4h18v16H3V4Zm2 2v3h14V6H5Zm0 5v3h14v-3H5Zm0 5v2h14v-2H5Z",
        ["pivot"] = "m4 7 1.4-1.4L9.8 10H21v2H9.8l-4.4 4.4L4 15l3-3-3-3Zm16 6-1.4 1.4-4.4-4.4H3V8h11.2l4.4-4.4L20 5l-3 3 3 3-3 3 3 3Z",
        ["hub"] = "M3 3h8v8H3V3Zm10 0h8v8h-8V3ZM3 13h8v8H3v-8Zm10 0h8v8h-8v-8Z",
        ["navigation"] = "M3 5h18v2H3V5Zm0 6h18v2H3v-2Zm0 6h18v2H3v-2Z",
        ["breadcrumb"] = "M8 4 16 12 8 20l-1.4-1.4 6.6-6.6-6.6-6.6L8 4Z",
        ["tree"] = "M5 3h2v5h5v2H7v5h5v2H7v4H5V3Zm8 5h6v2h-6V8Zm0 7h6v2h-6v-2Z",
        ["split"] = "M3 4h18v16H3V4Zm2 2v12h6V6H5Z",
        ["commands"] = "M5 11h3v3H5v-3Zm6 0h3v3h-3v-3Zm6 0h3v3h-3v-3Z",
        ["appbar"] = "M3 18h18v3H3v-3ZM3 3h18v2H3V3Z",
        ["flyout"] = "M4 5h16v14H4V5Zm2 2v10h12V7H6Z",
        ["dialog"] = "M3 4h18v16H3V4Zm2 2v12h14V6H5Z"
    };

    public static IReadOnlyList<string> Names => Paths.Keys.OrderBy(name => name).ToArray();

    public static IReadOnlyList<string> Search(string? query, int limit = 72)
    {
        var normalized = Normalize(query);
        return Names.Where(name => string.IsNullOrEmpty(normalized) || Normalize(name).Contains(normalized, StringComparison.Ordinal)).Take(Math.Max(limit, 1)).ToArray();
    }

    internal static string? ResolvePath(string name)
        => Paths.TryGetValue(name.Trim().ToLowerInvariant(), out var path) ? path : null;

    private static string Normalize(string? value) => new string((value ?? string.Empty).Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
}
