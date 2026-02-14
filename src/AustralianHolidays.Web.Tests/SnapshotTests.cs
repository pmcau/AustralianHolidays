[TestFixture]
public class SnapshotTests
{
    static WebApplication? app;
    static int port;
    static IPlaywright? playwright;
    static IBrowser? browser;

    // Fixed date: January 15, 2026 00:00:00 UTC
    const string FakeDateScript = "Date.now = () => 1768435200000; Date.prototype.getTimezoneOffset = () => 0;";

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        port = GetAvailablePort();

        // Use pre-published output from build (see csproj PublishBlazorForTests target)
        var testAssemblyDir = Path.GetDirectoryName(typeof(SnapshotTests).Assembly.Location)!;
        var wwwrootPath = Path.Combine(testAssemblyDir, "..", "blazor-publish", "wwwroot");

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls($"http://localhost:{port}");
        builder.Logging.ClearProviders();

        app = builder.Build();

        var contentTypeProvider = new FileExtensionContentTypeProvider
        {
            Mappings =
            {
                [".wasm"] = "application/wasm"
            }
        };

        var fileProvider = new PhysicalFileProvider(wwwrootPath);

        app.UseDefaultFiles(
            new DefaultFilesOptions
            {
                FileProvider = fileProvider
            });
        app.UseStaticFiles(
            new StaticFileOptions
            {
                FileProvider = fileProvider,
                ContentTypeProvider = contentTypeProvider,
                ServeUnknownFileTypes = true
            });

        app.MapFallbackToFile(
            "index.html",
            new StaticFileOptions
            {
                FileProvider = fileProvider
            });

        await app.StartAsync();

        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (browser != null)
        {
            await browser.CloseAsync();
        }

        playwright?.Dispose();

        if (app != null)
        {
            await app.StopAsync();
            await app.DisposeAsync();
        }
    }

    [Test]
    public async Task HomePage()
    {
        var page = await browser!.NewPageAsync();
        await page.AddInitScriptAsync(FakeDateScript);
        await page.GotoAsync($"http://localhost:{port}/");

        // Wait for Blazor to fully render
        await page.WaitForSelectorAsync(".holiday-table");

        await Verify(page);
    }

    [Test]
    public async Task HomePageMobile()
    {
        var page = await browser!.NewPageAsync();
        await page.SetViewportSizeAsync(375, 667); // iPhone SE size

        await page.AddInitScriptAsync(FakeDateScript);
        await page.GotoAsync($"http://localhost:{port}/");

        // Wait for Blazor to fully render
        await page.WaitForSelectorAsync(".holiday-table");

        await Verify(page);
    }

    [Test]
    public async Task HomePageDarkMode()
    {
        var page = await browser!.NewPageAsync();

        await page.AddInitScriptAsync(FakeDateScript);
        await page.GotoAsync($"http://localhost:{port}/");

        // Set dark theme in localStorage before Blazor initializes
        await page.EvaluateAsync("() => localStorage.setItem('selectedTheme', 'Dark')");

        // Reload to apply theme
        await page.ReloadAsync();

        // Wait for Blazor to fully render
        await page.WaitForSelectorAsync(".holiday-table");

        await Verify(page);
    }

    [Test]
    public async Task HomePageDarkModeMobile()
    {
        var page = await browser!.NewPageAsync();
        await page.SetViewportSizeAsync(375, 667); // iPhone SE size

        await page.AddInitScriptAsync(FakeDateScript);
        await page.GotoAsync($"http://localhost:{port}/");

        // Set dark theme in localStorage before Blazor initializes
        await page.EvaluateAsync("() => localStorage.setItem('selectedTheme', 'Dark')");

        // Reload to apply theme
        await page.ReloadAsync();

        // Wait for Blazor to fully render
        await page.WaitForSelectorAsync(".holiday-table");

        await Verify(page);
    }

    static int GetAvailablePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint) listener.LocalEndpoint).Port;
    }
}
