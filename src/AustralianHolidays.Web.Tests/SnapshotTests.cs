[TestFixture]
public class SnapshotTests
{
    static WebApplication? app;
    static int port;
    static IPlaywright? playwright;
    static IBrowser? browser;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        port = GetAvailablePort();

        var projectPath = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..",
                "AustralianHolidays.Web"));

        var publishPath = Path.Combine(Path.GetTempPath(), "BlazorSnapshotTest");
        Directory.CreateDirectory(publishPath);

        // Publish the Blazor app
        var publishProcess = Process.Start(
            new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"publish \"{projectPath}\" -o \"{publishPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });
        await publishProcess!.WaitForExitAsync();

        var wwwrootPath = Path.Combine(publishPath, "wwwroot");

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

        await page.GotoAsync($"http://localhost:{port}/");

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
