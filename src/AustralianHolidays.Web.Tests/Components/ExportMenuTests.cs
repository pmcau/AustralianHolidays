public class ExportMenuTests : BunitTestContext
{
    public ExportMenuTests() =>
        Services.AddScoped<FileDownloadService>();

    [Test]
    public async Task InitialRender_HasAllExportButtons()
    {
        JSInterop.SetupVoid("fileDownload.downloadFile", _ => true);

        var cut = Render<ExportMenu>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State> {State.NSW})
            .Add(_ => _.StartYear, 2025)
            .Add(_ => _.YearCount, 2));

        var buttons = cut.FindAll(".export-btn");

        await Assert.That(buttons.Count).IsEqualTo(6);

        var buttonTexts = buttons.Select(_ => _.TextContent).ToList();
        await Assert.That(buttonTexts).Contains("JSON");
        await Assert.That(buttonTexts).Contains("CSV");
        await Assert.That(buttonTexts).Contains("XML");
        await Assert.That(buttonTexts).Contains("Markdown");
        await Assert.That(buttonTexts).Contains("ICS");
        await Assert.That(buttonTexts).Contains("Excel");
    }

    [Test]
    public async Task AllButtonsEnabled_WhenNotExporting()
    {
        JSInterop.SetupVoid("fileDownload.downloadFile", _ => true);

        var cut = Render<ExportMenu>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State> {State.NSW})
            .Add(_ => _.StartYear, 2025)
            .Add(_ => _.YearCount, 2));

        var buttons = cut.FindAll(".export-btn");

        foreach (var button in buttons)
        {
            await Assert.That(button.HasAttribute("disabled")).IsFalse();
        }
    }

    [Test]
    public async Task HasExportLabel()
    {
        JSInterop.SetupVoid("fileDownload.downloadFile", _ => true);

        var cut = Render<ExportMenu>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>())
            .Add(_ => _.StartYear, 2025)
            .Add(_ => _.YearCount, 2));

        var label = cut.Find(".export-menu label");
        await Assert.That(label.TextContent).IsEqualTo("Export:");
    }
}
