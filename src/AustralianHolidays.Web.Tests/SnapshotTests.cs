using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

[TestFixture]
public class SnapshotTests
{
    static WebApplication? _app;
    static int _port;
    static IPlaywright? _playwright;
    static IBrowser? _browser;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _port = GetAvailablePort();

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
        builder.WebHost.UseUrls($"http://localhost:{_port}");
        builder.Logging.ClearProviders();

        _app = builder.Build();

        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings[".wasm"] = "application/wasm";

        var fileProvider = new PhysicalFileProvider(wwwrootPath);

        _app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = fileProvider });
        _app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider,
            ContentTypeProvider = contentTypeProvider,
            ServeUnknownFileTypes = true
        });

        _app.MapFallbackToFile("index.html", new StaticFileOptions { FileProvider = fileProvider });

        await _app.StartAsync();

        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();

        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }

    [Test]
    public async Task HomePage()
    {
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"http://localhost:{_port}/");

        // Wait for Blazor to fully render
        await page.WaitForSelectorAsync(".holiday-table");

        await Verify(page);
    }

    static int GetAvailablePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
