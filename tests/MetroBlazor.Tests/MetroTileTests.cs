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
    public void TileGroup_rows_mode_emits_two_base_tracks_per_visual_row()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroAppItem> { new() { Label = "a" } };
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2));

        Assert.Contains("--metro-tile-group-tracks: 4", cut.Find(".metro-tile-group-grid").GetAttribute("style"));
    }

    [Fact]
    public void TileGroup_drop_on_bottom_half_inserts_after_target()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroAppItem>
        {
            new() { Label = "a" }, new() { Label = "b" }, new() { Label = "c" }, new() { Label = "d" },
        };
        IReadOnlyList<MetroAppItem>? layout = null;
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.LayoutChanged, EventCallback.Factory.Create<IReadOnlyList<MetroAppItem>>(this, l => layout = l)));

        cut.FindAll(".metro-tile-group-item")[1].TriggerEvent("ondragstart", new DragEventArgs());
        // Medium tiles are 128px tall in Rows mode; past the 64px midpoint = insert after.
        cut.FindAll(".metro-tile-group-item")[3].TriggerEvent("ondrop", new DragEventArgs { OffsetY = 100 });

        Assert.Equal(new[] { "a", "c", "d", "b" }, layout!.Select(i => i.Label).ToArray());
    }

    [Fact]
    public void TileGroup_drop_on_top_half_inserts_before_target()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroAppItem>
        {
            new() { Label = "a" }, new() { Label = "b" }, new() { Label = "c" }, new() { Label = "d" },
        };
        IReadOnlyList<MetroAppItem>? layout = null;
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.LayoutChanged, EventCallback.Factory.Create<IReadOnlyList<MetroAppItem>>(this, l => layout = l)));

        cut.FindAll(".metro-tile-group-item")[1].TriggerEvent("ondragstart", new DragEventArgs());
        cut.FindAll(".metro-tile-group-item")[3].TriggerEvent("ondrop", new DragEventArgs { OffsetY = 10 });

        Assert.Equal(new[] { "a", "c", "b", "d" }, layout!.Select(i => i.Label).ToArray());
    }

    [Fact]
    public void TileGroup_drop_past_the_last_tile_appends_to_end()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroAppItem>
        {
            new() { Label = "a" }, new() { Label = "b" }, new() { Label = "c" }, new() { Label = "d" },
        };
        IReadOnlyList<MetroAppItem>? layout = null;
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.LayoutChanged, EventCallback.Factory.Create<IReadOnlyList<MetroAppItem>>(this, l => layout = l)));

        cut.FindAll(".metro-tile-group-item")[1].TriggerEvent("ondragstart", new DragEventArgs());
        // Empty area to the right of the last column (OffsetX ~ 3.7 cells of 136px).
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 500, OffsetY = 10 });

        Assert.Equal(new[] { "a", "c", "d", "b" }, layout!.Select(i => i.Label).ToArray());
    }

    [Fact]
    public void TileGroup_drop_in_a_gap_inserts_at_the_matching_slot()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroAppItem>
        {
            new() { Label = "a" }, new() { Label = "b" }, new() { Label = "c" }, new() { Label = "d" },
        };
        IReadOnlyList<MetroAppItem>? layout = null;
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.LayoutChanged, EventCallback.Factory.Create<IReadOnlyList<MetroAppItem>>(this, l => layout = l)));

        cut.FindAll(".metro-tile-group-item")[3].TriggerEvent("ondragstart", new DragEventArgs());
        // Gap right below the first tile: column 0, row 0 + 1 = slot 1.
        cut.Find(".metro-tile-group-grid").TriggerEvent("ondrop", new DragEventArgs { OffsetX = 80, OffsetY = 10 });

        Assert.Equal(new[] { "a", "d", "b", "c" }, layout!.Select(i => i.Label).ToArray());
    }
}
