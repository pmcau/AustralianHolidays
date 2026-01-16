using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Playwright;

namespace AustralianHolidays.Web.Tests;

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

        // Use published output which has complete Blazor WASM package
        var projectPath = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..",
                "AustralianHolidays.Web"));

        var publishPath = Path.Combine(Path.GetTempPath(), "BlazorSnapshotTest", Guid.NewGuid().ToString());
        Directory.CreateDirectory(publishPath);

        // Publish the Blazor app
        var publishProcess = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"publish \"{projectPath}\" -o \"{publishPath}\" -c Release",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });
        await publishProcess!.WaitForExitAsync();

        var wwwrootPath = Path.Combine(publishPath, "wwwroot");

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls($"http://localhost:{_port}");

        _app = builder.Build();

        // Configure MIME types for Blazor WASM
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings[".dll"] = "application/octet-stream";
        contentTypeProvider.Mappings[".wasm"] = "application/wasm";
        contentTypeProvider.Mappings[".dat"] = "application/octet-stream";
        contentTypeProvider.Mappings[".blat"] = "application/octet-stream";
        contentTypeProvider.Mappings[".webcil"] = "application/octet-stream";

        var fileProvider = new PhysicalFileProvider(wwwrootPath);

        _app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = fileProvider });
        _app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider,
            ContentTypeProvider = contentTypeProvider,
            ServeUnknownFileTypes = true
        });

        // Handle SPA fallback for Blazor routing
        _app.MapFallbackToFile("index.html", new StaticFileOptions
        {
            FileProvider = fileProvider
        });

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
