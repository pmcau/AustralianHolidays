using System.Net.Http.Json;

// Checks the hardcoded sitting data in Parliament_House.cs / Parliament_Senate.cs against the official
// record, so transcription errors and later changes to the calendar get caught without re-reading the
// source by hand. Hits the network, so it is excluded from the default test run:
//
//   dotnet test src --filter Category!=Integration
//
// To run it:
//
//   dotnet test src/Tests --filter "FullyQualifiedName~ParliamentSourceTests"
[TestFixture]
[Category("Integration")]
public class ParliamentSourceTests
{
    // The Parliamentary Handbook exposes one row per sitting day as plain JSON over HTTP, with no bot
    // protection, which is what makes it checkable from a test. It is the API behind
    // https://handbook.aph.gov.au/resources/sitting-calendars.
    //
    // It records sittings as they happen rather than publishing the year ahead, so it confirms the past
    // but says nothing about dates still in the future. The PM&C calendar the data is transcribed from
    // covers the whole year, but cannot be fetched from code: pmc.gov.au sits behind an Incapsula JS
    // challenge that returns a ~212 byte stub to any plain HTTP client, whatever user agent it sends.
    // So topping up a new year stays a manual step; this test then verifies it as the year plays out.
    const string sittingDaysUrl = "https://handbookapi.aph.gov.au/api/StatisticalInformation/SittingDaysForYear?year=";

    [Test]
    public async Task SittingDaysMatchOfficialRecord()
    {
        using var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        List<string> mismatches = [];
        var verifiedYears = 0;

        foreach (var year in CoveredYears())
        {
            IReadOnlyList<SittingDay> recorded;
            try
            {
                recorded = await Fetch(client, year);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
            {
                Assert.Inconclusive($"Could not reach {sittingDaysUrl}{year}: {exception.Message}");
                return;
            }

            if (recorded.Count == 0)
            {
                await TestContext.Out.WriteLineAsync($"{year}: not yet in the official record, so nothing to check.");
                continue;
            }

            verifiedYears++;

            var byDate = recorded.ToDictionary(_ => Date.FromDateTime(_.Date));
            var lastRecorded = byDate.Keys.Max();

            // Only compare as far as the record goes. Anything later is scheduled but has not happened,
            // so an absence there means "not yet recorded", not "not a sitting day".
            for (var date = new Date(year, 1, 1); date <= lastRecorded; date = date.AddDays(1))
            {
                byDate.TryGetValue(date, out var official);

                Compare(mismatches, date, "House sitting", date.IsHouseSittingDay(), official?.RepsSitting ?? false);
                Compare(mismatches, date, "Senate sitting", date.IsSenateSittingDay(), official?.SenateSitting ?? false);
                Compare(mismatches, date, "Senate estimates", date.IsSenateEstimatesDay(), official?.SenateEstimates ?? false);
            }

            await TestContext.Out.WriteLineAsync(
                $"{year}: checked 1 Jan to {lastRecorded:yyyy-MM-dd} against {recorded.Count} recorded sitting days. " +
                $"Dates after {lastRecorded:yyyy-MM-dd} are not in the official record yet and cannot be checked.");
        }

        if (verifiedYears == 0)
        {
            Assert.Inconclusive("None of the covered years are in the official record yet, so nothing was verified.");
        }

        if (mismatches.Count > 0)
        {
            Fail(
                $"""
                 {mismatches.Count} date(s) disagree with the official record at {sittingDaysUrl}<year>.

                 {string.Join(Environment.NewLine, mismatches)}

                 Either the hardcoded data has a transcription error, or the sitting calendar changed
                 after it was transcribed (sittings get added, as with the 19-20 January 2026 recall, and
                 cancelled). Update Parliament_House.cs / Parliament_Senate.cs to match, then accept the
                 ParliamentTests snapshots.
                 """);
        }
    }

    static void Compare(List<string> mismatches, Date date, string what, bool library, bool official)
    {
        if (library == official)
        {
            return;
        }

        var expected = official ? "yes" : "no";
        var actual = library ? "yes" : "no";
        mismatches.Add($"  {date:yyyy-MM-dd} ({date.DayOfWeek}) {what}: official record says {expected}, library says {actual}");
    }

    static async Task<IReadOnlyList<SittingDay>> Fetch(HttpClient client, int year)
    {
        var days = await client.GetFromJsonAsync<List<SittingDay>>($"{sittingDaysUrl}{year}");
        return days ?? [];
    }

    static IEnumerable<int> CoveredYears() =>
        Parliament.CoveredYears(Chamber.House)
            .Union(Parliament.CoveredYears(Chamber.Senate))
            .Order();

    // The payload carries a Json.NET "$id" alongside these; unmapped properties are ignored.
    record SittingDay(DateTime Date, bool RepsSitting, bool SenateSitting, bool SenateEstimates);
}
