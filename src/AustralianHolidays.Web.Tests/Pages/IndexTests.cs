[TestFixture]
public class IndexTests : BunitTestContext
{
    public IndexTests()
    {
        Services.AddScoped<StatePreferenceService>();
        Services.AddScoped<FileDownloadService>();
        Services.AddScoped(_ => new HttpClient { BaseAddress = new("http://localhost/") });
        JSInterop.SetupVoid("statePreference.get", _ => true);
        JSInterop.SetupVoid("fileDownload.downloadFile", _ => true);
    }

    [Test]
    public Task LayoutStructure()
    {
        JSInterop.Setup<string?>("statePreference.get", "selectedState").SetResult(null);

        var cut = Render<AustralianHolidays.Web.Pages.Index>();

        return Verify(cut.Markup);
    }
}
