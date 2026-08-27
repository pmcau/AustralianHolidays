[TestFixture]
public class SchoolHolidaysExportTests
{
    // School data is bounded to a fixed range of years, so an explicit startYear is pinned to keep the
    // snapshots deterministic (an unpinned export would drift and eventually go empty as years pass).
    const int startYear = 2025;

    static readonly State[] multipleStates = [State.NSW, State.VIC];

    [Test]
    public async Task ExportToMarkdownState()
    {
        #region SchoolExportToMarkdown

        var markdown = await SchoolHolidays.ExportToMarkdown(State.NSW, startYear: 2025);

        #endregion

        await Verify(markdown, "md")
            .Snapshot(
                """
                | Season | 2025 | 2026 | 2027 |
                | --- | --- | --- | --- |
                | Summer | `01 Jan 2025 - 05 Feb 2025` | `20 Dec 2025 - 01 Feb 2026` | `18 Dec 2026 - 02 Feb 2027` |
                | Autumn | `12 Apr 2025 - 29 Apr 2025` | `03 Apr 2026 - 21 Apr 2026` | `10 Apr 2027 - 28 Apr 2027` |
                | Winter | `05 Jul 2025 - 21 Jul 2025` | `04 Jul 2026 - 20 Jul 2026` | `03 Jul 2027 - 19 Jul 2027` |
                | Spring | `27 Sep 2025 - 13 Oct 2025` | `26 Sep 2026 - 12 Oct 2026` | `25 Sep 2027 - 11 Oct 2027` |

                """);
    }

    [Test]
    public async Task ExportToMarkdownAllStates()
    {
        var markdown = await SchoolHolidays.ExportToMarkdown(startYear: startYear);
        await Verify(markdown, "md");
    }

    [Test]
    public async Task ExportToMarkdownMultiState()
    {
        var markdown = await SchoolHolidays.ExportToMarkdown(multipleStates, startYear: startYear);
        await Verify(markdown, "md");
    }

    [Test]
    public async Task ExportToIcsState()
    {
        #region SchoolExportToIcs

        var ics = await SchoolHolidays.ExportToIcs(State.NSW, startYear: 2025);

        #endregion

        await Verify(ics, "ics");
    }

    [Test]
    public async Task ExportToIcsAllStates()
    {
        var ics = await SchoolHolidays.ExportToIcs(startYear: startYear);
        await Verify(ics, "ics");
    }

    [Test]
    public async Task ExportToIcsMultiState()
    {
        var ics = await SchoolHolidays.ExportToIcs(multipleStates, startYear: startYear);
        await Verify(ics, "ics");
    }

    [Test]
    public async Task ExportToCsvState()
    {
        #region SchoolExportToCsv

        var csv = await SchoolHolidays.ExportToCsv(State.NSW, startYear: 2025);

        #endregion

        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToCsvAllStates()
    {
        var csv = await SchoolHolidays.ExportToCsv(startYear: startYear);
        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToCsvMultiState()
    {
        var csv = await SchoolHolidays.ExportToCsv(multipleStates, startYear: startYear);
        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToJsonState()
    {
        var json = await SchoolHolidays.ExportToJson(State.NSW, startYear: startYear);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToJsonAllStates()
    {
        var json = await SchoolHolidays.ExportToJson(startYear: startYear);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToJsonMultiState()
    {
        var json = await SchoolHolidays.ExportToJson(multipleStates, startYear: startYear);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToXmlState()
    {
        var xml = await SchoolHolidays.ExportToXml(State.NSW, startYear: startYear);
        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToXmlAllStates()
    {
        var xml = await SchoolHolidays.ExportToXml(startYear: startYear);
        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToXmlMultiState()
    {
        var xml = await SchoolHolidays.ExportToXml(multipleStates, startYear: startYear);
        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToExcelState()
    {
        var bytes = await SchoolHolidays.ExportToExcel(State.NSW, startYear: startYear);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public async Task ExportToExcelAllStates()
    {
        var bytes = await SchoolHolidays.ExportToExcel(startYear: startYear);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public async Task ExportToExcelMultiState()
    {
        var bytes = await SchoolHolidays.ExportToExcel(multipleStates, startYear: startYear);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public async Task ExportToCsvPath()
    {
        using var path = new TempFile();
        await SchoolHolidays.ExportToCsv(path, startYear: startYear);
        var csv = await File.ReadAllTextAsync(path);
        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToJsonPath()
    {
        using var path = new TempFile();
        await SchoolHolidays.ExportToJson(path, startYear: startYear);
        var json = await File.ReadAllTextAsync(path);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToXmlPath()
    {
        using var path = new TempFile();
        await SchoolHolidays.ExportToXml(path, startYear: startYear);
        var xml = await File.ReadAllTextAsync(path);
        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToExcelPath()
    {
        using var path = new TempFile();
        await SchoolHolidays.ExportToExcel(path, startYear: startYear);
        var bytes = await File.ReadAllBytesAsync(path);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public async Task EmptyStatesMatchesAllStates()
    {
        var all = await SchoolHolidays.ExportToCsv(startYear: startYear);
        var empty = await SchoolHolidays.ExportToCsv(Array.Empty<State>(), startYear: startYear);
        AreEqual(all, empty);
    }

    [Test]
    public async Task YearsPastCoverageProduceNoRows()
    {
        // NSW data does not extend to 2035; the export must return only the header, without throwing.
        var csv = await SchoolHolidays.ExportToCsv(State.NSW, startYear: 2035);
        AreEqual("Start,End,Name", csv.Trim());
    }

    [Test]
    public async Task RangeIsClampedToCoveredYears()
    {
        // NSW data ends in 2027; a five-year window from 2027 must yield only 2027 and must not throw for
        // the uncovered 2028-2031 years.
        var csv = await SchoolHolidays.ExportToCsv(State.NSW, startYear: 2027, yearCount: 5);
        IsTrue(csv.Contains("2027"), "expected the covered 2027 year to be present");
        IsFalse(csv.Contains("2028"), "expected uncovered years to be skipped");
    }

    [Test]
    public async Task AllStatesRangePastCoverageProduceNoRows()
    {
        // No state has data in 2050; the all-states export must return an empty JSON array, not throw.
        var json = await SchoolHolidays.ExportToJson(startYear: 2050);
        AreEqual("[]", json);
    }
}
