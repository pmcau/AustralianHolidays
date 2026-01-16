using AustralianHolidays.Web.Pages;
using AustralianHolidays.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AustralianHolidays.Web.Tests.Pages;

[TestFixture]
public class IndexTests : BunitTestContext
{
    public IndexTests()
    {
        Services.AddScoped<StatePreferenceService>();
        Services.AddScoped<FileDownloadService>();
        Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("http://localhost/") });
        JSInterop.SetupVoid("statePreference.get", _ => true);
    }

    [Test]
    public Task LayoutStructure()
    {
        JSInterop.Setup<string?>("statePreference.get", "selectedState").SetResult(null);
        JSInterop.Setup<string?>("statePreference.get", "locationPrompted").SetResult("true");

        var cut = Render<AustralianHolidays.Web.Pages.Index>();

        return Verify(cut.Markup);
    }
}
