# MetroBlazor

> A Windows 8.1 Metro UI component library for [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor).

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![.NET](https://img.shields.io/badge/.NET-10-512BD4.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/MetroBlazor.svg)](https://www.nuget.org/packages/MetroBlazor)

MetroBlazor brings the flat, squared, touch-first look and feel of Windows 8.1 Metro to
modern browsers. It is built as an independent Razor Class Library that ships as a NuGet
package, with no dependency on a specific host application.

## Highlights

- **Touch-first controls** — flat geometry, strong color, and square corners.
- **Live tiles** — animated tile content with configurable intervals.
- **Tile groups** — drag sorting, removal, and size cycling (Small / Medium / Wide / Large).
- **Built-in icon library** — searchable SVG icons with a stable name-based API.
- **Theming** — Dark, Light, Blue, and Purple variants with a cascading theme state.
- **Accessible** — touch, mouse, keyboard, focus visibility, ARIA, and reduced-motion support.
- **Self-contained** — no Fluent UI icon runtime dependency; icons are embedded SVG paths.

## Requirements

- .NET 10
- [Microsoft.FluentUI.AspNetCore.Components](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components) `4.13.x`
  (used selectively for accessibility and complex infrastructure; not exposed in the public API)

## Installation

```bash
dotnet add package MetroBlazor
```

Register the Fluent UI services in `Program.cs`:

```csharp
builder.Services.AddFluentUIComponents();
```

## Quick start

Add the namespace to your `_Imports.razor`:

```razor
@using MetroBlazor
```

Wrap your app in `MetroTheme` and start using components:

```razor
<MetroTheme>
    <MetroTile Label="Photos" Icon="home" AccentColor="#0078D7" />
</MetroTheme>
```

## Theming

`MetroTheme` establishes the Metro design tokens and cascades a `MetroThemeState` that any
descendant can use to switch themes at runtime.

```razor
<MetroTheme Mode="Dark">
    <App />
</MetroTheme>
```

Available variants:

| Variant  | Description                |
| -------- | -------------------------- |
| `Dark`   | Default dark Metro theme   |
| `Light`  | Light Metro theme          |
| `Blue`   | Blue accent theme          |
| `Purple` | Purple accent theme        |

```csharp
[CascadingParameter] public MetroThemeState? ThemeState { get; set; }

ThemeState!.Variant = MetroThemeVariant.Blue;
```

## Components

### Foundation

- `MetroTheme` — theme root, tokens, and cascading theme state
- `MetroIcon` / `MetroIconCatalog` — searchable embedded SVG icon library
- `MetroText` — Metro typography (Display, Header, Subheader, Body, Label, Caption, Link)
- `MetroPageTransition` — animated content transition

### Surface

- `MetroTile` — live tiles with Small / Medium / Wide / Large sizes
- `MetroTileGroup` — reorderable, resizable tile groups
- `MetroAppGrid` — responsive app launcher grid
- `MetroHub` — spacious grouped content surface
- `MetroSemanticZoom` — grouped overview / detailed view
- `MetroSurface` — reusable surface container

### Forms

- `MetroButton` — solid, quiet, and icon-only (bordered / non-bordered) buttons
- `MetroInput` — underline text field
- `MetroSelect` — native select with Metro treatment
- `MetroToggle` — squared switch
- `MetroCheckbox` — squared checkbox
- `MetroRadio` — squared radio
- `MetroSlider` — thick squared slider
- `MetroSearchBox` — filled search with icon and clear action

### Feedback

- `MetroProgress` — segmented determinate and staggered indeterminate progress
- `MetroBadge` — compact status and count label

### Collections

- `MetroListView` — flat, touch-friendly list
- `MetroDataGrid` — responsive semantic table
- `MetroTreeView` — expandable hierarchy

### Navigation

- `MetroSidebar` — persistent collapsible left blade
- `MetroNavigation` — grouped navigation with expandable items
- `MetroPivot` — Windows 8 style section navigation
- `MetroBreadcrumb` — slash-separated path navigation
- `MetroBlade` — right-side detail surface

### Layout

- `MetroLayout` — responsive app shell: persistent sidebar rail on desktop, overlay drawer with scrim on mobile
- `MetroAppFrame` — application frame with header and content regions
- `MetroSplitView` — pane and content layout

### Commands and overlays

- `MetroCommandBar` — flat command actions
- `MetroAppBar` — bottom command surface
- `MetroFlyout` — contextual surface
- `MetroContentDialog` — centered modal dialog

## Icons

`MetroIcon` renders embedded SVG paths sourced from Microsoft Fluent UI System Icons.

```razor
<MetroIcon Name="settings" Size="MetroIconSize.Large" />
```

Search the icon catalog programmatically:

```csharp
IReadOnlyList<string> names = MetroIconCatalog.Search("arrow");
```

Use `PathOverride` to supply application-owned SVG paths without adding any runtime
dependency.

## Live tiles

```razor
<MetroTile Label="Weather" Live="true" LiveInterval="3000">
    <LiveContent>
        <span>22°C Sunny</span>
    </LiveContent>
</MetroTile>
```

## Tile groups

Tile groups support long-press or right-click edit mode, drag sorting, tile removal, and
size cycling.

```razor
<MetroTileGroup Title="Pinned" Items="tiles"
                ItemMoved="OnTileMoved"
                ItemSizeChanged="OnTileSizeChanged"
                ItemRemoved="OnTileRemoved" />
```

## Showcase

Run the interactive showcase to browse every component with live themes, icons, and
code examples. The showcase itself is built with `MetroLayout`:

```bash
dotnet run --project showcase/MetroBlazor.Showcase
```

## Development

Build, test, and package the library:

```bash
dotnet build MetroBlazor.slnx
dotnet test  MetroBlazor.slnx
dotnet pack  src/MetroBlazor/MetroBlazor.csproj -c Release
```

NuGet packages are written to `artifacts/packages/`.

### Project structure

```
MetroBlazor.slnx
├── src/MetroBlazor/         Razor Class Library (NuGet package)
├── showcase/                Interactive showcase (Blazor Server)
└── tests/                   bUnit tests (xunit)
```

## Browser support

- Desktop Chrome (latest)
- Desktop Firefox (latest)
- Mobile Chrome (latest)
- Mobile Safari (latest)

## License

[Apache 2.0](LICENSE)
