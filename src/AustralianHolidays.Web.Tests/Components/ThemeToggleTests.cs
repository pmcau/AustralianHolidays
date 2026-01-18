[TestFixture]
public class ThemeToggleTests : BunitTestContext
{
    [Test]
    public void InitialRender_WithLightTheme_ShowsDarkButton()
    {
        var cut = Render<ThemeToggle>(_ => _
            .Add(_ => _.CurrentTheme, ThemeType.Light));

        var button = cut.Find(".theme-toggle-btn");
        That(button.TextContent, Does.Contain("Dark"));
    }

    [Test]
    public void InitialRender_WithDarkTheme_ShowsLightButton()
    {
        var cut = Render<ThemeToggle>(_ => _
            .Add(_ => _.CurrentTheme, ThemeType.Dark));

        var button = cut.Find(".theme-toggle-btn");
        That(button.TextContent, Does.Contain("Light"));
    }

    [Test]
    public async Task ClickButton_WithLightTheme_InvokesDarkTheme()
    {
        ThemeType? newTheme = null;
        var cut = Render<ThemeToggle>(_ => _
            .Add(_ => _.CurrentTheme, ThemeType.Light)
            .Add(_ => _.OnThemeChanged, (ThemeType t) => newTheme = t));

        var button = cut.Find(".theme-toggle-btn");
        await button.ClickAsync(new());

        That(newTheme, Is.EqualTo(ThemeType.Dark));
    }

    [Test]
    public async Task ClickButton_WithDarkTheme_InvokesLightTheme()
    {
        ThemeType? newTheme = null;
        var cut = Render<ThemeToggle>(_ => _
            .Add(_ => _.CurrentTheme, ThemeType.Dark)
            .Add(_ => _.OnThemeChanged, (ThemeType t) => newTheme = t));

        var button = cut.Find(".theme-toggle-btn");
        await button.ClickAsync(new());

        That(newTheme, Is.EqualTo(ThemeType.Light));
    }

    [Test]
    public void Button_HasAriaLabel()
    {
        var cut = Render<ThemeToggle>(_ => _
            .Add(_ => _.CurrentTheme, ThemeType.Light));

        var button = cut.Find(".theme-toggle-btn");
        That(button.GetAttribute("aria-label"), Is.EqualTo("Toggle theme"));
    }
}
