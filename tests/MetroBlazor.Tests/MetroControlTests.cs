using Bunit;
using MetroBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MetroBlazor.Tests;

public class MetroControlTests
{
    [Fact]
    public void MetroButton_renders_content_and_clicks()
    {
        using var ctx = new BunitContext();
        var invoked = false;
        var cut = ctx.Render<MetroButton>(p => p
            .Add(x => x.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Open")))
            .Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, _ => invoked = true)));

        Assert.Equal("Open", cut.Find("button").TextContent);
        cut.Find("button").Click();
        Assert.True(invoked);
    }

    [Fact]
    public void MetroProgress_clamps_value_and_fills_segments()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroProgress>(p => p
            .Add(x => x.Value, 60)
            .Add(x => x.Segments, 5));

        Assert.Equal("progressbar", cut.Find(".metro-progress").GetAttribute("role"));
        Assert.Equal("3", cut.FindAll(".metro-progress-segment.filled").Count.ToString());
    }

    [Fact]
    public void MetroProgress_supports_indeterminate_state()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroProgress>(p => p.Add(x => x.IsIndeterminate, true));

        Assert.Equal(5, cut.FindAll(".metro-progress-indeterminate .metro-progress-indicator").Count);
        Assert.Null(cut.Find(".metro-progress").GetAttribute("aria-valuenow"));
    }

    [Fact]
    public void MetroBadge_renders_custom_class_and_content()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroBadge>(p => p
            .Add(x => x.Class, "sync-state")
            .Add(x => x.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Ready"))));

        Assert.Contains("sync-state", cut.Find(".metro-badge").ClassName);
        Assert.Equal("Ready", cut.Find(".metro-badge").TextContent);
    }

    [Fact]
    public void MetroInput_emits_value_changes()
    {
        using var ctx = new BunitContext();
        string? value = null;
        var cut = ctx.Render<MetroInput>(p => p
            .Add(x => x.Label, "Name")
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<string?>(this, result => value = result)));

        cut.Find("input").Input("Metro");

        Assert.Equal("Metro", value);
        Assert.Equal("Name", cut.Find("label").TextContent);
    }

    [Fact]
    public void MetroToggle_emits_the_inverse_checked_state()
    {
        using var ctx = new BunitContext();
        bool? value = null;
        var cut = ctx.Render<MetroToggle>(p => p
            .Add(x => x.Label, "Enable sync")
            .Add(x => x.Checked, false)
            .Add(x => x.CheckedChanged, EventCallback.Factory.Create<bool>(this, result => value = result)));

        cut.Find("button").Click();

        Assert.True(value);
        Assert.Equal("false", cut.Find("button").GetAttribute("aria-checked"));
    }

    [Fact]
    public void MetroCheckbox_emits_changed_value()
    {
        using var ctx = new BunitContext();
        bool? value = null;
        var cut = ctx.Render<MetroCheckbox>(p => p
            .Add(x => x.Label, "Use cellular data")
            .Add(x => x.CheckedChanged, EventCallback.Factory.Create<bool>(this, result => value = result)));

        cut.Find("input").Change(true);

        Assert.True(value);
    }

    [Fact]
    public void MetroAppGrid_renders_items_and_emits_selected_item()
    {
        using var ctx = new BunitContext();
        var item = new MetroAppItem { Label = "Photos", Icon = "*" };
        MetroAppItem? selected = null;
        var cut = ctx.Render<MetroAppGrid>(p => p
            .Add(x => x.Items, new[] { item })
            .Add(x => x.ItemClick, EventCallback.Factory.Create<MetroAppItem>(this, result => selected = result)));

        Assert.Single(cut.FindAll(".metro-tile"));
        cut.Find(".metro-tile").Click();

        Assert.Same(item, selected);
    }

    [Fact]
    public void MetroLayout_renders_sidebar_brand_and_main_content()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroLayout>(p => p
            .Add(x => x.Brand, "TESTAPP")
            .Add(x => x.SidebarContent, (RenderFragment)(b => b.AddContent(0, "NAV")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddContent(0, "MAIN"))));

        Assert.Contains("NAV", cut.Find(".metro-sidebar").TextContent);
        Assert.Contains("MAIN", cut.Find(".metro-layout-main").TextContent);
        Assert.Contains("TESTAPP", cut.Find(".metro-sidebar-heading").TextContent);
    }

    [Fact]
    public void MetroLayout_scrim_requests_sidebar_collapse()
    {
        using var ctx = new BunitContext();
        var collapsed = false;
        var cut = ctx.Render<MetroLayout>(p => p
            .Add(x => x.SidebarContent, (RenderFragment)(b => b.AddContent(0, "NAV")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddContent(0, "MAIN")))
            .Add(x => x.SidebarCollapsedChanged, EventCallback.Factory.Create<bool>(this, value => collapsed = value)));

        cut.Find(".metro-layout-scrim").Click();

        Assert.True(collapsed);
    }

    [Fact]
    public void MetroSidebar_only_owns_collapse_shell()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroSidebar>(p => p
            .Add(x => x.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Navigation")))
            .Add(x => x.Collapsed, false));

        Assert.Contains("Navigation", cut.Find(".metro-sidebar-content").TextContent);
        Assert.DoesNotContain("collapsed", cut.Find(".metro-sidebar").ClassName);
    }

    [Fact]
    public void MetroNavigation_groups_items_and_emits_selected_key()
    {
        using var ctx = new BunitContext();
        string? selected = null;
        var items = new[]
        {
            new MetroNavigationItem { Key = "overview", Label = "Overview", Icon = "home", Group = "Getting started" },
            new MetroNavigationItem { Key = "tile", Label = "Tile", Icon = "folder", Group = "Surface" }
        };
        var cut = ctx.Render<MetroNavigation>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.SelectedKey, "overview")
            .Add(x => x.SelectedKeyChanged, EventCallback.Factory.Create<string>(this, value => selected = value)));

        Assert.Equal(2, cut.FindAll(".metro-navigation-item").Count);
        cut.FindAll(".metro-navigation-item")[1].Click();

        Assert.Equal("tile", selected);
        Assert.Contains("selected", cut.FindAll(".metro-navigation-item")[0].ClassName);
        Assert.Equal(2, cut.FindAll(".metro-icon").Count);
    }

    [Fact]
    public void MetroBlade_renders_and_closes_from_scrim()
    {
        using var ctx = new BunitContext();
        var closed = false;
        var cut = ctx.Render<MetroBlade>(p => p
            .Add(x => x.Open, true)
            .Add(x => x.Title, "Tile")
            .Add(x => x.OnClose, EventCallback.Factory.Create(this, () => closed = true)));

        Assert.Equal("Tile", cut.Find(".metro-blade h2").TextContent);
        cut.Find(".metro-blade-scrim").Click();

        Assert.True(closed);
    }

    [Fact]
    public void MetroIcon_renders_known_path_and_override()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroIcon>(p => p.Add(x => x.Name, "home"));

        Assert.False(string.IsNullOrWhiteSpace(cut.Find("path").GetAttribute("d")));

        var overrideCut = ctx.Render<MetroIcon>(p => p.Add(x => x.Name, "custom").Add(x => x.PathOverride, "M0 0h24v24H0z"));
        Assert.Equal("M0 0h24v24H0z", overrideCut.Find("path").GetAttribute("d"));
        Assert.NotEmpty(MetroIconCatalog.Names);
        Assert.Contains("save", MetroIconCatalog.Search("save"));
    }

    [Fact]
    public void MetroSelect_renders_options()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroSelect>(p => p.Add(x => x.Options, new[]
        {
            new MetroSelectOption { Value = "one", Label = "One" },
            new MetroSelectOption { Value = "two", Label = "Two" }
        }));

        Assert.Equal(2, cut.FindAll("option").Count);
    }

    [Fact]
    public void MetroSearchBox_swaps_search_and_clear_affordances()
    {
        using var ctx = new BunitContext();
        var empty = ctx.Render<MetroSearchBox>();

        Assert.Empty(empty.FindAll(".metro-search-clear"));
        Assert.Single(empty.FindAll(".metro-search-box > .metro-icon"));

        var populated = ctx.Render<MetroSearchBox>(p => p.Add(x => x.Value, "query"));
        Assert.Single(populated.FindAll(".metro-search-clear"));
        Assert.Empty(populated.FindAll(".metro-search-box > .metro-icon"));
    }

    [Fact]
    public void MetroButton_supports_accessible_icon_only_variants()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroButton>(p => p
            .Add(x => x.Icon, "add")
            .Add(x => x.IconOnly, true)
            .Add(x => x.Bordered, true)
            .Add(x => x.AriaLabel, "Add item"));

        var button = cut.Find("button");
        Assert.Contains("metro-button-icon-only", button.ClassName);
        Assert.Contains("metro-button-bordered", button.ClassName);
        Assert.Equal("Add item", button.GetAttribute("aria-label"));
        Assert.NotNull(button.QuerySelector("svg"));
    }

    [Fact]
    public void MetroTileGroup_supports_size_change_and_drag_reorder()
    {
        using var ctx = new BunitContext();
        var first = new MetroAppItem { Label = "First", Size = MetroTileSize.Small };
        var second = new MetroAppItem { Label = "Second", Size = MetroTileSize.Small };
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, new List<MetroAppItem> { first, second }));

        Assert.Empty(cut.FindAll(".metro-tile-edit-control"));
        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("oncontextmenu", new MouseEventArgs());
        Assert.Equal("true", cut.Find(".metro-tile-group-item .metro-tile").GetAttribute("draggable"));
        cut.Find(".metro-tile-size-button").Click();
        Assert.Equal(MetroTileSize.Medium, first.Size);


        cut.FindAll(".metro-tile-group-item")[0].TriggerEvent("oncontextmenu", new MouseEventArgs());
        Assert.Empty(cut.FindAll(".metro-tile-edit-control"));

    }
}
