[TestFixture]
public class ExportMenuTests : BunitTestContext
{
    public ExportMenuTests() =>
        Services.AddScoped<FileDownloadService>();

    [Test]
    public void InitialRender_HasAllExportButtons()
    {
        JSInterop.SetupVoid("fileDownload.downloadFile", _ => true);

        var cut = Render<ExportMenu>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State> {State.NSW})
            .Add(_ => _.StartYear, 2025)
            .Add(_ => _.YearCount, 2));

        var buttons = cut.FindAll(".export-btn");

        That(buttons.Count, Is.EqualTo(6));

        var buttonTexts = buttons.Select(b => b.TextContent).ToList();
        That(buttonTexts, Does.Contain("JSON"));
        That(buttonTexts, Does.Contain("CSV"));
        That(buttonTexts, Does.Contain("XML"));
        That(buttonTexts, Does.Contain("Markdown"));
        That(buttonTexts, Does.Contain("ICS"));
        That(buttonTexts, Does.Contain("Excel"));
    }

    [Test]
    public void AllButtonsEnabled_WhenNotExporting()
    {
        JSInterop.SetupVoid("fileDownload.downloadFile", _ => true);

        var cut = Render<ExportMenu>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State> {State.NSW})
            .Add(_ => _.StartYear, 2025)
            .Add(_ => _.YearCount, 2));

        var buttons = cut.FindAll(".export-btn");

        foreach (var button in buttons)
        {
            That(button.HasAttribute("disabled"), Is.False);
        }
    }

    [Test]
    public void HasExportLabel()
    {
        JSInterop.SetupVoid("fileDownload.downloadFile", _ => true);

        var cut = Render<ExportMenu>(_ => _
            .Add(_ => _.SelectedStates, new HashSet<State>())
            .Add(_ => _.StartYear, 2025)
            .Add(_ => _.YearCount, 2));

        var label = cut.Find(".export-menu label");
        That(label.TextContent, Is.EqualTo("Export:"));
    }
}