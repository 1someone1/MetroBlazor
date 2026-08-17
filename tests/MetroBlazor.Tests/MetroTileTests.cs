using Bunit;
using MetroBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MetroBlazor.Tests;

public class MetroTileTests
{
    private static IRenderedComponent<MetroTile> Render(BunitContext ctx, Action<ComponentParameterCollectionBuilder<MetroTile>> parameters)
        => ctx.Render(parameters);

    [Fact]
    public void Renders_label_and_icon()
    {
        using var ctx = new BunitContext();
        var cut = Render(ctx, p => p
            .Add(x => x.Label, "照片")
            .Add(x => x.Icon, "📷"));

        Assert.Equal("照片", cut.Find(".metro-tile-label").TextContent);
        Assert.Equal("📷", cut.Find(".metro-tile-icon").TextContent);
    }

    [Fact]
    public void Renders_optional_sublabel()
    {
        using var ctx = new BunitContext();
        var cut = Render(ctx, p => p
            .Add(x => x.Label, "照片")
            .Add(x => x.SubLabel, "Photos"));

        Assert.Equal("Photos", cut.Find(".metro-tile-sublabel").TextContent);
    }

    [Fact]
    public void Applies_accent_color_as_background()
    {
        using var ctx = new BunitContext();
        var cut = Render(ctx, p => p
            .Add(x => x.Label, "照片")
            .Add(x => x.AccentColor, "#0078D7"));

        Assert.Contains("#0078D7", cut.Find(".metro-tile").GetAttribute("style"));
    }

    [Fact]
    public void Marks_selected_tile()
    {
        using var ctx = new BunitContext();
        var cut = Render(ctx, p => p
            .Add(x => x.Label, "照片")
            .Add(x => x.Selected, true));

        Assert.Contains("selected", cut.Find(".metro-tile").ClassName);
    }

    [Fact]
    public void Invokes_click_callback()
    {
        using var ctx = new BunitContext();
        MouseEventArgs? received = null;
        var cut = Render(ctx, p => p
            .Add(x => x.Label, "照片")
            .Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, e => received = e)));

        cut.Find(".metro-tile").Click();

        Assert.NotNull(received);
    }

    [Fact]
    public void Does_not_throw_when_click_callback_not_delegated()
    {
        using var ctx = new BunitContext();
        var cut = Render(ctx, p => p.Add(x => x.Label, "照片"));

        cut.Find(".metro-tile").Click();
    }

    [Fact]
    public void Renders_as_an_accessible_button_with_size()
    {
        using var ctx = new BunitContext();
        var cut = Render(ctx, p => p
            .Add(x => x.Label, "照片")
            .Add(x => x.Size, MetroTileSize.Wide));

        var tile = cut.Find("button.metro-tile");
        Assert.Equal("button", tile.GetAttribute("type"));
        Assert.Equal("照片", tile.GetAttribute("aria-label"));
        Assert.Equal("false", tile.GetAttribute("aria-pressed"));
        Assert.Contains("metro-tile-wide", tile.ClassName);
    }

    [Fact]
    public void Does_not_invoke_click_callback_when_disabled()
    {
        using var ctx = new BunitContext();
        var invoked = false;
        var cut = Render(ctx, p => p
            .Add(x => x.Label, "照片")
            .Add(x => x.Disabled, true)
            .Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, _ => invoked = true)));

        cut.Find(".metro-tile").Click();

        Assert.False(invoked);
    }

    [Fact]
    public void TileGroup_rows_mode_track_count_follows_the_layout()
    {
        using var ctx = new BunitContext();
        // A single medium tile occupies one visual row: 2 base tracks.
        var single = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, new List<MetroAppItem> { new() { Label = "a" } })
            .Add(x => x.Rows, 2));
        Assert.Contains("--metro-tile-group-tracks: 2", single.Find(".metro-tile-group-grid").GetAttribute("style"));

        // Four mediums fill both initial rows: 4 base tracks.
        var full = RenderGroup(ctx, FourMediumTiles(), default);
        Assert.Contains("--metro-tile-group-tracks: 4", full.Find(".metro-tile-group-grid").GetAttribute("style"));
    }

    private static IRenderedComponent<MetroTileGroup> RenderGroup(BunitContext ctx, List<MetroAppItem> items,
        EventCallback<IReadOnlyList<MetroAppItem>> layoutChanged)
        => ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.LayoutChanged, layoutChanged));

    private static List<MetroAppItem> FourMediumTiles() =>
    [
        new() { Label = "a" }, new() { Label = "b" }, new() { Label = "c" }, new() { Label = "d" },
    ];

    [Fact]
    public void TileGroup_rows_mode_auto_places_tiles_in_column_major_order()
    {
        using var ctx = new BunitContext();
        var items = FourMediumTiles();
        var cut = RenderGroup(ctx, items, default);

        // Medium = 2x2 tracks in a 4-track (2 visual rows) grid.
        Assert.Equal((0, 0), (items[0].GridX, items[0].GridY));
        Assert.Equal((0, 2), (items[1].GridX, items[1].GridY));
        Assert.Equal((2, 0), (items[2].GridX, items[2].GridY));
        Assert.Equal((2, 2), (items[3].GridX, items[3].GridY));

        var style = cut.FindAll(".metro-tile-group-item")[1].GetAttribute("style");
        Assert.Contains("grid-column: 1 / span 2", style);
        Assert.Contains("grid-row: 3 / span 2", style);
    }

    [Fact]
    public void TileGroup_clamped_legacy_coordinates_do_not_overlap()
    {
        using var ctx = new BunitContext();
        // Saved before a column cap existed: both tiles clamp onto the same cell.
        var items = new List<MetroAppItem>
        {
            new() { Label = "a", GridX = 20, GridY = 0 },
            new() { Label = "b", GridX = 22, GridY = 0 },
        };
        ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 1)
            .Add(x => x.MaxColumns, 2));

        Assert.Equal((2, 0), (items[0].GridX, items[0].GridY));
        Assert.Equal((0, 0), (items[1].GridX, items[1].GridY)); // loser moves to the first free cell
    }

    [Fact]
    public void TileGroup_drop_on_empty_area_moves_tile_to_that_cell()
    {
        using var ctx = new BunitContext();
        var items = FourMediumTiles();
        var moved = false;
        var cut = RenderGroup(ctx, items, EventCallback.Factory.Create<IReadOnlyList<MetroAppItem>>(this, _ => moved = true));

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("ondragstart", new DragEventArgs());
        // Empty area starting at the third column: cell (4, 0) in 68px units.
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 280, OffsetY = 10 });

        Assert.True(moved);
        Assert.Equal((4, 0), (items[0].GridX, items[0].GridY));
        // Free placement keeps list order; only coordinates change.
        Assert.Equal(new[] { "a", "b", "c", "d" }, items.Select(i => i.Label).ToArray());
    }

    [Fact]
    public void TileGroup_drop_on_an_occupied_cell_pushes_the_cover_tile_aside()
    {
        using var ctx = new BunitContext();
        var items = FourMediumTiles(); // a(0,0) b(0,2) c(2,0) d(2,2)
        var cut = RenderGroup(ctx, items, default);

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("ondragstart", new DragEventArgs());
        // Drop a onto b's cell (0,2): b slides to a's now-free cell (0,0).
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 10, OffsetY = 140 });

        Assert.Equal((0, 2), (items[0].GridX, items[0].GridY));
        Assert.Equal((0, 0), (items[1].GridX, items[1].GridY));
        Assert.Equal((2, 0), (items[2].GridX, items[2].GridY));
        Assert.Equal((2, 2), (items[3].GridX, items[3].GridY));
    }

    [Fact]
    public void TileGroup_dragover_previews_pushed_tiles_and_accent_ghost()
    {
        using var ctx = new BunitContext();
        var items = FourMediumTiles();
        items[0].AccentColor = "#CA5010";
        var cut = RenderGroup(ctx, items, default);

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("ondragstart", new DragEventArgs());
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondragover", new DragEventArgs { OffsetX = 10, OffsetY = 140 });

        // Ghost sits on cell (0,2) tinted with the dragged tile's accent color...
        var ghost = cut.Find(".metro-tile-group-ghost");
        Assert.Contains("grid-row: 3 / span 2", ghost.GetAttribute("style"));
        Assert.Contains("--metro-tile-accent: #CA5010", ghost.GetAttribute("style"));

        // ...and tile b already renders at its preview cell (0,0).
        Assert.Contains("grid-column: 1 / span 2; grid-row: 1 / span 2",
            cut.FindAll(".metro-tile-group-item")[1].GetAttribute("style"));
    }

    [Fact]
    public void TileGroup_rows_grow_and_shrink_with_the_tiles()
    {
        using var ctx = new BunitContext();
        var items = FourMediumTiles();
        var cut = RenderGroup(ctx, items, default);

        // Two rows of mediums: 4 base tracks.
        Assert.Contains("--metro-tile-group-tracks: 4", cut.Find(".metro-tile-group-grid").GetAttribute("style"));

        // Drag a below the second row: the group grows to 3 visual rows (6 tracks).
        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("ondragstart", new DragEventArgs());
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 10, OffsetY = 280 });
        Assert.Contains("--metro-tile-group-tracks: 6", cut.Find(".metro-tile-group-grid").GetAttribute("style"));

        // Move everything into the top row: the group shrinks to one visual row.
        for (var i = 0; i < items.Count; i++)
        {
            items[i].GridX = i * 2;
            items[i].GridY = 0;
        }
        cut.Render();
        Assert.Contains("--metro-tile-group-tracks: 2", cut.Find(".metro-tile-group-grid").GetAttribute("style"));
    }

    [Fact]
    public void TileGroup_large_tile_fits_by_growing_the_rows()
    {
        using var ctx = new BunitContext();
        var large = new MetroAppItem { Label = "l", Size = MetroTileSize.Large };
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, new List<MetroAppItem> { large })
            .Add(x => x.Rows, 1));

        Assert.Equal((0, 0), (large.GridX, large.GridY));
        Assert.Contains("--metro-tile-group-tracks: 4", cut.Find(".metro-tile-group-grid").GetAttribute("style"));
    }

    [Fact]
    public void TileGroup_max_columns_clamps_the_drop_cell()
    {
        using var ctx = new BunitContext();
        var items = FourMediumTiles(); // a(0,0) b(0,2) c(2,0) d(2,2)
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.MaxColumns, 2)); // 4 base tracks wide

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("ondragstart", new DragEventArgs());
        // Way past the right edge: clamps to the last legal column (x=2), pushing c aside.
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 2000, OffsetY = 10 });

        Assert.Equal((2, 0), (items[0].GridX, items[0].GridY));
        Assert.Equal((0, 0), (items[2].GridX, items[2].GridY)); // c slid into a's vacated cell
        Assert.Equal((0, 2), (items[1].GridX, items[1].GridY));
        Assert.Equal((2, 2), (items[3].GridX, items[3].GridY));
    }

    [Fact]
    public void TileGroup_drop_beyond_max_rows_is_clamped_to_the_cap()
    {
        using var ctx = new BunitContext();
        var items = FourMediumTiles();
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.MaxRows, 2));

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("ondragstart", new DragEventArgs());
        // Below the cap: y clamps to track 2 (second visual row), into the free column x=4.
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 300, OffsetY = 900 });

        Assert.Equal((4, 2), (items[0].GridX, items[0].GridY));
        Assert.Contains("--metro-tile-group-tracks: 4", cut.Find(".metro-tile-group-grid").GetAttribute("style"));
    }

    [Fact]
    public void TileGroup_drop_that_cannot_fit_pushed_tiles_within_bounds_is_rejected()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroAppItem>
        {
            new() { Label = "a" }, // medium at (0,0)
            new() { Label = "L", Size = MetroTileSize.Large }, // large at (2,0)
        };
        var moved = false;
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.MaxRows, 2)
            .Add(x => x.MaxColumns, 3)
            .Add(x => x.LayoutChanged, EventCallback.Factory.Create<IReadOnlyList<MetroAppItem>>(this, _ => moved = true)));

        Assert.Equal((2, 0), (items[1].GridX, items[1].GridY));

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("ondragstart", new DragEventArgs());
        // Drop a onto L's cell: L (4x4) has no free cell left inside the 3x2 cap.
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 140, OffsetY = 10 });

        Assert.False(moved);
        Assert.Equal((0, 0), (items[0].GridX, items[0].GridY));
        Assert.Equal((2, 0), (items[1].GridX, items[1].GridY));
    }

    [Fact]
    public void TileGroup_cycle_size_wraps_around_when_large_exceeds_max_rows()
    {
        using var ctx = new BunitContext();
        var wide = new MetroAppItem { Label = "w", Size = MetroTileSize.Wide };
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, new List<MetroAppItem> { wide })
            .Add(x => x.Rows, 1)
            .Add(x => x.MaxRows, 1));

        cut.Find(".metro-tile-group-item").TriggerEvent("oncontextmenu", new MouseEventArgs());
        cut.Find(".metro-tile-size-button").Click();

        Assert.Equal(MetroTileSize.Small, wide.Size);
    }

    [Fact]
    public void TileGroup_cycle_size_pushes_the_covered_tile_aside()
    {
        using var ctx = new BunitContext();
        // a(0,0) b(0,2) c(2,0) after column-major auto-placement; growing a to
        // wide (4x2 tracks) covers c's cell.
        var a = new MetroAppItem { Label = "a", Size = MetroTileSize.Medium };
        var b = new MetroAppItem { Label = "b", Size = MetroTileSize.Medium };
        var c = new MetroAppItem { Label = "c", Size = MetroTileSize.Medium };
        var items = new List<MetroAppItem> { a, b, c };
        var cut = RenderGroup(ctx, items, default);

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("oncontextmenu", new MouseEventArgs());
        cut.FindAll(".metro-tile-size-button")[0].Click();

        Assert.Equal(MetroTileSize.Wide, a.Size);
        Assert.Equal((0, 0), (a.GridX, a.GridY));
        Assert.Equal((0, 2), (b.GridX, b.GridY)); // untouched
        Assert.Equal((2, 2), (c.GridX, c.GridY)); // pushed to the first free cell, not overlapped
    }

    [Fact]
    public void TileGroup_cycle_size_reverts_when_no_room_to_push()
    {
        using var ctx = new BunitContext();
        // a(0,0) b(2,0) in a 2-column 1-row box: growing a to wide cannot fit both.
        var a = new MetroAppItem { Label = "a", Size = MetroTileSize.Medium };
        var b = new MetroAppItem { Label = "b", Size = MetroTileSize.Medium };
        var items = new List<MetroAppItem> { a, b };
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 1)
            .Add(x => x.MaxRows, 1)
            .Add(x => x.MaxColumns, 2));

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("oncontextmenu", new MouseEventArgs());
        cut.FindAll(".metro-tile-size-button")[0].Click();

        Assert.Equal(MetroTileSize.Medium, a.Size); // reverted
        Assert.Equal((2, 0), (b.GridX, b.GridY)); // untouched
    }

    [Fact]
    public void TileGroup_exit_edit_mode_from_js_hides_the_controls()
    {
        using var ctx = new BunitContext();
        var cut = RenderGroup(ctx, FourMediumTiles(), default);

        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("oncontextmenu", new MouseEventArgs());
        Assert.NotEmpty(cut.FindAll(".metro-tile-edit-control"));

        // The document-level dismiss listener (clicks outside any tile) calls this.
        cut.InvokeAsync(() => cut.Instance.ExitEditModeFromJs());
        Assert.Empty(cut.FindAll(".metro-tile-edit-control"));
    }
}
