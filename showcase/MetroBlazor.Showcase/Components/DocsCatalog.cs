using MetroBlazor;

namespace MetroBlazor.Showcase.Components;

/// <summary>A single documentation target: a guide page or a component page.</summary>
public sealed record DocsEntry(string Key, string Title, string Group, string Icon, string Route, string Description);

/// <summary>
/// The single source of truth for the docs site structure. The sidebar navigation,
/// the header breadcrumb/title, and the overview page all derive from this catalog.
/// Groups follow the README component categories.
/// </summary>
public static class DocsCatalog
{
    public const string ComponentsRootKey = "components";

    public static readonly IReadOnlyList<DocsEntry> Entries =
    [
        new("overview", "Overview", "Getting started", "home", "/",
            "Windows 8.1 Metro controls for modern browsers, built touch-first and kept deliberately flat."),
        new("getting-started", "Getting started", "Getting started", "book", "/getting-started",
            "Install the package, register services, and wrap your app in MetroTheme."),
        new("theming", "Theming", "Getting started", "color", "/theming",
            "Dark, Light, Blue, and Purple variants with a cascading theme state."),
        new("icons", "Icons", "Getting started", "icon", "/icons",
            "Searchable embedded SVG icons with a stable name-based API."),

        new("theme", "Theme", "Foundation", "settings", "/components/theme",
            "MetroTheme establishes the Metro design tokens and cascades a MetroThemeState."),
        new("typography", "Typography", "Foundation", "type", "/components/typography",
            "Windows 8.1 uses a clear Segoe UI hierarchy: light display titles, compact headers, readable body copy, and restrained captions."),
        new("icon", "Icon", "Foundation", "shapes", "/components/icon",
            "MetroIcon renders embedded SVG paths through stable searchable names and currentColor rendering."),
        new("transition", "Page transition", "Foundation", "transition", "/components/transition",
            "A reusable content transition for app navigation. Application routing remains outside the component library."),

        new("tile", "Tile", "Surface", "tile", "/components/tile",
            "Live tiles in four canonical sizes with a shared touch, mouse, and keyboard interaction model."),
        new("tile-group", "Tile group", "Surface", "group", "/components/tile-group",
            "A named group for organizing tiles on a Start surface: drag sorting, removal, and size cycling — or a read-only launcher grid with the ReadOnly switch."),
        new("hub", "Hub", "Surface", "hub", "/components/hub",
            "A spacious content surface for grouped Metro sections."),
        new("semantic-zoom", "Semantic zoom", "Surface", "zoom", "/components/semantic-zoom",
            "The Windows 8 grouped overview and detailed tile surface represent the same content at different zoom levels."),
        new("surface", "Surface", "Surface", "square", "/components/surface",
            "A reusable themed surface container for page regions."),

        new("button", "Button", "Forms", "cursor-click", "/components/button",
            "Solid, quiet, and icon-only actions with square geometry and pressed feedback."),
        new("input", "Input", "Forms", "input", "/components/input",
            "The classic Metro underline field with a clear focus transition."),
        new("select", "Select", "Forms", "select", "/components/select",
            "A native select with Metro underline treatment and accessible label association."),
        new("toggle", "Toggle", "Forms", "toggle", "/components/toggle",
            "A squared switch that exposes its state through the switch role and aria-checked."),
        new("checkbox", "Checkbox", "Forms", "check", "/components/checkbox",
            "A touch-sized checkbox with a visible Metro check state."),
        new("radio", "Radio", "Forms", "radio", "/components/radio",
            "Squared radio controls with a visible selection indicator."),
        new("slider", "Slider", "Forms", "slider", "/components/slider",
            "A touch-sized range control with explicit value feedback."),
        new("search", "Search box", "Forms", "search", "/components/search",
            "Search input with icon affordance and clear action."),
        new("rating", "Rating", "Forms", "star", "/components/rating",
            "A star rating control with pointer and keyboard input."),

        new("progress", "Progress", "Feedback", "progress", "/components/progress",
            "Segmented determinate progress and staggered square-block indeterminate motion."),
        new("badge", "Badge", "Feedback", "badge", "/components/badge",
            "Compact uppercase status and count labels for tiles and commands."),
        new("tooltip", "Tooltip", "Feedback", "info", "/components/tooltip",
            "A hover and focus hint that stays attached to its trigger."),

        new("list", "List view", "Collections", "list", "/components/list",
            "A flat, touch-friendly list with full-row activation."),
        new("data-grid", "Data grid", "Collections", "table", "/components/data-grid",
            "A responsive semantic table with row activation and horizontal overflow on narrow screens."),
        new("tree", "Tree view", "Collections", "tree", "/components/tree",
            "Expandable hierarchical navigation for folders and settings categories."),

        new("sidebar", "Sidebar", "Navigation", "sidebar", "/components/sidebar",
            "The collapsible navigation rail with a pinned hamburger toggle, composed by MetroLayout."),
        new("navigation", "Navigation", "Navigation", "navigation", "/components/navigation",
            "Grouped navigation with expandable items, used by this very sidebar."),
        new("pivot", "Pivot", "Navigation", "pivot", "/components/pivot",
            "Horizontal section navigation with an active underline and overflow scrolling."),
        new("breadcrumb", "Breadcrumb", "Navigation", "breadcrumb", "/components/breadcrumb",
            "Flat path navigation for file and settings surfaces."),
        new("blade", "Blade", "Navigation", "blade", "/components/blade",
            "The Windows 8 right-side detail surface, with a scrim and fade-in slide motion."),

        new("layout", "Layout", "Layout", "panel-left", "/components/layout",
            "The responsive app shell: a persistent sidebar rail on desktop, a push drawer with scrim on mobile."),
        new("frame", "App frame", "Layout", "frame", "/components/frame",
            "Deprecated. A non-modal application frame with header and content regions; use MetroLayout with HeaderTemplate instead."),

        new("command-bar", "Command bar", "Commands & overlays", "commands", "/components/command-bar",
            "Flat command actions with optional icons and disabled states."),
        new("app-bar", "App bar", "Commands & overlays", "appbar", "/components/app-bar",
            "A bottom command surface that reveals and hides from the lower edge."),
        new("context-menu", "Context menu", "Commands & overlays", "menu", "/components/context-menu",
            "A right-click command menu that opens at the pointer."),
        new("flyout", "Flyout", "Commands & overlays", "flyout", "/components/flyout",
            "A contextual surface with a scrim and entrance motion."),
        new("dialog", "Content dialog", "Commands & overlays", "dialog", "/components/dialog",
            "A centered modal surface with explicit primary and secondary actions."),
    ];

    /// <summary>Group header icons for the sidebar. Distinct from every entry icon.</summary>
    public static readonly IReadOnlyDictionary<string, string> GroupIcons = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["Getting started"] = "rocket",
        ["Foundation"] = "layer",
        ["Surface"] = "apps",
        ["Forms"] = "form",
        ["Feedback"] = "megaphone",
        ["Collections"] = "library",
        ["Navigation"] = "compass",
        ["Layout"] = "board",
        ["Commands & overlays"] = "window",
    };

    public static IReadOnlyList<MetroNavigationItem> NavigationItems { get; } = Entries
        .Select(entry => new MetroNavigationItem { Key = entry.Key, Label = entry.Title, Icon = entry.Icon, Group = entry.Group })
        .ToArray();

    public static IEnumerable<IGrouping<string, DocsEntry>> ComponentGroups => Entries
        .Where(entry => entry.Route.StartsWith("/components/", StringComparison.Ordinal))
        .GroupBy(entry => entry.Group);

    public static DocsEntry? FindByPath(string? path)
    {
        var normalized = string.IsNullOrEmpty(path) ? "/" : path;
        return Entries.FirstOrDefault(entry => string.Equals(entry.Route, normalized, StringComparison.OrdinalIgnoreCase));
    }
}
