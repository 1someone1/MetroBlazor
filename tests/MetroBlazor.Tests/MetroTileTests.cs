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
}
