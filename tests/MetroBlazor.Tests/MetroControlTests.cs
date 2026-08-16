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
    public void MetroTileGroup_readonly_mode_emits_item_click_without_edit_controls()
    {
        using var ctx = new BunitContext();
        var item = new MetroAppItem { Label = "Photos", Icon = "image" };
        MetroAppItem? selected = null;
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, new List<MetroAppItem> { item })
            .Add(x => x.ReadOnly, true)
            .Add(x => x.Columns, 4)
            .Add(x => x.ItemClick, EventCallback.Factory.Create<MetroAppItem>(this, result => selected = result)));

        Assert.Single(cut.FindAll(".metro-tile"));
        Assert.Contains("columns", cut.Find(".metro-tile-group-grid").ClassName);
        cut.Find(".metro-tile").Click();
        Assert.Same(item, selected);

        // Read-only mode renders no edit-mode machinery at all.
        Assert.Empty(cut.FindAll(".metro-tile-edit-control"));
        Assert.Empty(cut.FindAll(".metro-tile-group-item"));
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
    public void MetroNavigation_group_header_activates_first_item()
    {
        using var ctx = new BunitContext();
        string? selected = null;
        MetroNavigationItem? clicked = null;
        var items = new[]
        {
            new MetroNavigationItem { Key = "tile", Label = "Tile", Icon = "tile", Group = "Surface" },
            new MetroNavigationItem { Key = "hub", Label = "Hub", Icon = "hub", Group = "Surface" }
        };
        var groupIcons = new Dictionary<string, string> { ["Surface"] = "apps" };
        var cut = ctx.Render<MetroNavigation>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.GroupIcons, groupIcons)
            .Add(x => x.SelectedKeyChanged, EventCallback.Factory.Create<string>(this, value => selected = value))
            .Add(x => x.ItemClick, EventCallback.Factory.Create<MetroNavigationItem>(this, item => clicked = item)));

        Assert.Single(cut.FindAll(".metro-navigation-group"));
        Assert.Single(cut.FindAll(".metro-navigation-group .metro-icon"));
        cut.Find(".metro-navigation-group").Click();

        Assert.Equal("tile", selected);
        Assert.Equal("tile", clicked?.Key);
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
    public void MetroIcon_variant_picks_outline_path_and_childcontent_projects_svg()
    {
        using var ctx = new BunitContext();
        var filled = ctx.Render<MetroIcon>(p => p.Add(x => x.Name, "settings"));
        var outline = ctx.Render<MetroIcon>(p => p.Add(x => x.Name, "settings").Add(x => x.Variant, MetroIconVariant.Outline));

        Assert.NotEqual(filled.Find("path").GetAttribute("d"), outline.Find("path").GetAttribute("d"));

        // Icons without an outline variant fall back to the filled path.
        var homeFilled = ctx.Render<MetroIcon>(p => p.Add(x => x.Name, "home"));
        var fallback = ctx.Render<MetroIcon>(p => p.Add(x => x.Name, "home").Add(x => x.Variant, MetroIconVariant.Outline));
        Assert.Equal(homeFilled.Find("path").GetAttribute("d"), fallback.Find("path").GetAttribute("d"));

        var custom = ctx.Render<MetroIcon>(p => p.AddChildContent("<svg viewBox=\"0 0 24 24\"><rect width=\"24\" height=\"24\" /></svg>"));
        Assert.Single(custom.FindAll("rect"));
    }

    [Fact]
    public void MetroNavigation_collapsible_groups_toggle_items()
    {
        using var ctx = new BunitContext();
        string? selected = null;
        var items = new[]
        {
            new MetroNavigationItem { Key = "tile", Label = "Tile", Icon = "tile", Group = "Surface" },
            new MetroNavigationItem { Key = "hub", Label = "Hub", Icon = "hub", Group = "Surface" },
            new MetroNavigationItem { Key = "button", Label = "Button", Icon = "cursor-click", Group = "Forms" }
        };
        var cut = ctx.Render<MetroNavigation>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.CollapsibleGroups, true)
            .Add(x => x.SelectedKeyChanged, EventCallback.Factory.Create<string>(this, value => selected = value)));

        // All groups start collapsed when nothing is selected.
        Assert.Empty(cut.FindAll(".metro-navigation-item"));

        // Expanding a group opens straight into its first item.
        cut.FindAll(".metro-navigation-group")[0].Click();
        Assert.Equal(2, cut.FindAll(".metro-navigation-item").Count);
        Assert.Equal("tile", selected);

        // Clicking the open group collapses it again.
        cut.FindAll(".metro-navigation-group")[0].Click();
        Assert.Empty(cut.FindAll(".metro-navigation-item"));
    }

    [Fact]
    public void MetroNavigation_collapsible_groups_close_others_when_one_opens()
    {
        using var ctx = new BunitContext();
        var items = new[]
        {
            new MetroNavigationItem { Key = "tile", Label = "Tile", Icon = "tile", Group = "Surface" },
            new MetroNavigationItem { Key = "button", Label = "Button", Icon = "cursor-click", Group = "Forms" }
        };
        var cut = ctx.Render<MetroNavigation>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.CollapsibleGroups, true));

        cut.FindAll(".metro-navigation-group")[0].Click();
        Assert.Single(cut.FindAll(".metro-navigation-item"));

        // Accordion: opening Forms closes Surface.
        cut.FindAll(".metro-navigation-group")[1].Click();
        var visible = cut.FindAll(".metro-navigation-item");
        Assert.Single(visible);
        Assert.Contains("Button", visible[0].TextContent);
    }

    [Fact]
    public void MetroNavigation_collapsible_groups_keep_selected_group_open()
    {
        using var ctx = new BunitContext();
        var items = new[]
        {
            new MetroNavigationItem { Key = "tile", Label = "Tile", Icon = "tile", Group = "Surface" },
            new MetroNavigationItem { Key = "button", Label = "Button", Icon = "cursor-click", Group = "Forms" }
        };
        var cut = ctx.Render<MetroNavigation>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.CollapsibleGroups, true)
            .Add(x => x.SelectedKey, "button"));

        Assert.Single(cut.FindAll(".metro-navigation-item"));
        Assert.Equal("true", cut.FindAll(".metro-navigation-group")[1].GetAttribute("aria-expanded"));
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

    [Fact]
    public void MetroTileGroup_reorders_via_direct_drag_without_edit_mode()
    {
        using var ctx = new BunitContext();
        var first = new MetroAppItem { Label = "First", Size = MetroTileSize.Small };
        var second = new MetroAppItem { Label = "Second", Size = MetroTileSize.Small };
        var third = new MetroAppItem { Label = "Third", Size = MetroTileSize.Small };
        MetroAppItem? moved = null;
        var cut = ctx.Render<MetroTileGroup>(p => p
            .Add(x => x.Items, new List<MetroAppItem> { first, second, third })
            .Add(x => x.ItemMoved, EventCallback.Factory.Create<MetroAppItem>(this, item => moved = item)));

        // No edit mode entered: tiles are always draggable, like the Windows 8.1 Start screen.
        Assert.Equal("true", cut.Find(".metro-tile-group-item .metro-tile").GetAttribute("draggable"));

        cut.FindAll(".metro-tile-group-item .metro-tile")[0].TriggerEvent("ondragstart", new DragEventArgs());
        // Small tiles are 60px wide; dropping past the 30px midpoint inserts after the target.
        cut.FindAll(".metro-tile-group-item")[2].TriggerEvent("ondrop", new DragEventArgs { OffsetX = 40 });

        Assert.Same(first, moved);
        var labels = cut.FindAll(".metro-tile-group-item .metro-tile .metro-tile-label").Select(e => e.TextContent).ToArray();
        Assert.Equal(["Second", "Third", "First"], labels);
    }

    [Fact]
    public void MetroTooltip_renders_tooltip_text_with_role()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroTooltip>(p => p
            .Add(x => x.Text, "Save document")
            .Add(x => x.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Hover me"))));

        var tooltip = cut.Find("[role=tooltip]");
        Assert.Equal("Save document", tooltip.TextContent);
        Assert.Contains("metro-tooltip-top", cut.Find(".metro-tooltip-host").ClassName);
    }

    [Fact]
    public void MetroTooltip_omits_tooltip_when_text_is_empty()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroTooltip>(p => p
            .Add(x => x.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Plain"))));

        Assert.Empty(cut.FindAll("[role=tooltip]"));
    }

    [Fact]
    public void MetroContextMenu_opens_at_pointer_and_invokes_item()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroCommandItem>
        {
            new() { Key = "open", Label = "Open", Icon = "folder" },
            new() { Key = "delete", Label = "Delete", Icon = "delete" },
        };
        MetroCommandItem? clicked = null;
        var cut = ctx.Render<MetroContextMenu>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.ItemClick, EventCallback.Factory.Create<MetroCommandItem>(this, item => clicked = item))
            .Add(x => x.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Target"))));

        Assert.Empty(cut.FindAll(".metro-context-menu"));

        cut.Find(".metro-context-menu-host").TriggerEvent("oncontextmenu", new MouseEventArgs { ClientX = 40, ClientY = 24 });

        var menu = cut.Find(".metro-context-menu");
        Assert.Equal("menu", menu.GetAttribute("role"));
        Assert.Contains("left: 40px", menu.GetAttribute("style"));
        Assert.Contains("top: 24px", menu.GetAttribute("style"));
        Assert.Equal(2, cut.FindAll(".metro-context-menu-item").Count);

        cut.FindAll(".metro-context-menu-item")[0].Click();
        Assert.Equal("open", clicked?.Key);
        Assert.Empty(cut.FindAll(".metro-context-menu"));
    }

    [Fact]
    public void MetroContextMenu_ignores_disabled_items()
    {
        using var ctx = new BunitContext();
        var items = new List<MetroCommandItem> { new() { Key = "x", Label = "Nope", Disabled = true } };
        var invoked = false;
        var cut = ctx.Render<MetroContextMenu>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.ItemClick, EventCallback.Factory.Create<MetroCommandItem>(this, _ => invoked = true))
            .Add(x => x.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Target"))));

        cut.Find(".metro-context-menu-host").TriggerEvent("oncontextmenu", new MouseEventArgs());
        cut.Find(".metro-context-menu-item").Click();
        Assert.False(invoked);
    }

    [Fact]
    public void MetroRating_fills_stars_up_to_value_and_updates_on_click()
    {
        using var ctx = new BunitContext();
        var value = 3;
        var cut = ctx.Render<MetroRating>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.Max, 5)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<int>(this, v => value = v)));

        Assert.Equal("slider", cut.Find(".metro-rating").GetAttribute("role"));
        Assert.Equal(5, cut.FindAll(".metro-rating-star").Count);
        Assert.Equal(3, cut.FindAll(".metro-rating-star.filled").Count);

        cut.FindAll(".metro-rating-star")[4].Click();
        Assert.Equal(5, value);

        // Simulate two-way binding pushing the new value back in, then clear by re-clicking.
        var cut2 = ctx.Render<MetroRating>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<int>(this, v => value = v)));
        cut2.FindAll(".metro-rating-star")[4].Click();
        Assert.Equal(0, value);
    }

    [Fact]
    public void MetroRating_previews_tentative_value_on_hover()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroRating>(p => p
            .Add(x => x.Value, 2)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        Assert.Equal(2, cut.FindAll(".metro-rating-star.filled").Count);

        cut.FindAll(".metro-rating-star")[3].TriggerEvent("onmouseenter", new MouseEventArgs());
        Assert.Equal(4, cut.FindAll(".metro-rating-star.tentative").Count);
        Assert.Empty(cut.FindAll(".metro-rating-star.filled"));

        cut.Find(".metro-rating").TriggerEvent("onmouseleave", new MouseEventArgs());
        Assert.Empty(cut.FindAll(".metro-rating-star.tentative"));
        Assert.Equal(2, cut.FindAll(".metro-rating-star.filled").Count);
    }

    [Fact]
    public void MetroRating_supports_keyboard_adjustment()
    {
        using var ctx = new BunitContext();
        var value = 2;
        var cut = ctx.Render<MetroRating>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<int>(this, v => value = v)));

        cut.Find(".metro-rating").KeyDown(new KeyboardEventArgs { Key = "ArrowRight" });
        Assert.Equal(3, value);

        var cut2 = ctx.Render<MetroRating>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<int>(this, v => value = v)));
        cut2.Find(".metro-rating").KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });
        Assert.Equal(2, value);
    }

    [Fact]
    public void MetroTile_live_text_splits_on_word_boundaries()
    {
        var pages = MetroTile.SplitLiveText("alpha beta gamma delta", 12).ToArray();
        Assert.Equal(["alpha beta", "gamma delta"], pages);
    }

    [Fact]
    public void MetroTile_live_text_hard_splits_words_longer_than_a_page()
    {
        var pages = MetroTile.SplitLiveText("abcdefghijklmno", 10).ToArray();
        Assert.Equal(["abcdefghij", "klmno"], pages);
    }

    [Fact]
    public void MetroTile_live_text_keeps_short_text_on_one_page()
    {
        Assert.Single(MetroTile.SplitLiveText("short text", 70));
    }

    [Fact]
    public void MetroTile_live_wraps_default_face_for_flip_back_animation()
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render<MetroTile>(p => p
            .Add(x => x.Label, "Mail")
            .Add(x => x.Live, true)
            .Add(x => x.LiveText, "3 unread messages"));

        Assert.Equal("Mail", cut.Find(".metro-tile-front-content .metro-tile-label").TextContent);

        var plain = ctx.Render<MetroTile>(p => p.Add(x => x.Label, "Mail"));
        Assert.Empty(plain.FindAll(".metro-tile-front-content"));
    }

    [Fact]
    public void MetroDataGrid_row_context_menu_carries_item_and_position()
    {
        using var ctx = new BunitContext();
        (string Item, double X, double Y)? received = null;
        var cut = ctx.Render<MetroDataGrid<string>>(p => p
            .Add(x => x.Items, new[] { "one", "two" })
            .Add(x => x.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, "h")))
            .Add(x => x.RowTemplate, (RenderFragment<string>)(item => b => b.AddContent(0, item)))
            .Add(x => x.RowContextMenu, EventCallback.Factory.Create<(string, MouseEventArgs)>(this,
                e => received = (e.Item1, e.Item2.ClientX, e.Item2.ClientY))));

        cut.FindAll("tbody tr")[1].ContextMenu(new MouseEventArgs { ClientX = 40, ClientY = 55 });

        Assert.Equal(("two", 40d, 55d), received);
    }

}
