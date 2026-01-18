[TestFixture]
public class ThemePreferenceServiceTests : BunitTestContext
{
    [Test]
    public async Task GetSavedThemeAsync_WithSavedTheme_ReturnsTheme()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<string?>("statePreference.get", "selectedTheme").SetResult("Dark");

        var service = new ThemePreferenceService(JSInterop.JSRuntime);
        var theme = await service.GetSavedThemeAsync();

        That(theme, Is.EqualTo(ThemeType.Dark));
    }

    [Test]
    public async Task GetSavedThemeAsync_WithNoSavedTheme_ReturnsLight()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<string?>("statePreference.get", "selectedTheme").SetResult(null);

        var service = new ThemePreferenceService(JSInterop.JSRuntime);
        var theme = await service.GetSavedThemeAsync();

        That(theme, Is.EqualTo(ThemeType.Light));
    }

    [Test]
    public async Task GetSavedThemeAsync_WithInvalidValue_ReturnsLight()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<string?>("statePreference.get", "selectedTheme").SetResult("InvalidTheme");

        var service = new ThemePreferenceService(JSInterop.JSRuntime);
        var theme = await service.GetSavedThemeAsync();

        That(theme, Is.EqualTo(ThemeType.Light));
    }

    [Test]
    public async Task SaveThemeAsync_SavesThemeValue()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.SetupVoid("statePreference.set", "selectedTheme", "Dark").SetVoidResult();

        var service = new ThemePreferenceService(JSInterop.JSRuntime);
        await service.SaveThemeAsync(ThemeType.Dark);

        var invocations = JSInterop.Invocations["statePreference.set"];
        That(invocations, Has.Count.EqualTo(1));
        That(invocations[0].Arguments[0], Is.EqualTo("selectedTheme"));
        That(invocations[0].Arguments[1], Is.EqualTo("Dark"));
    }
}
