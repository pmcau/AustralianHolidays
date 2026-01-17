using System.Diagnostics.CodeAnalysis;
using Argon;

[TestFixture]
public class Tests
{
    [TestCaseSource(nameof(GetStates))]
    public Task IsPublicHolidayWithName(State state)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsHoliday(state, out var name))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)} {name}");
            }
        }

        return Verify(builder);
    }

    [Test]
    public async Task ExportToMarkdown()
    {
        #region ExportToMarkdown

        var md = await Holidays.ExportToMarkdown();

        #endregion

        await Verify(md, "md");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToMarkdown(State state)
    {
        #region ExportToMarkdownState

        var md = await Holidays.ExportToMarkdown(state);

        #endregion

        await Verify(md, "md");
    }

    [Test]
    public async Task ExportToIcs()
    {
        #region ExportToIcs

        var ics = await Holidays.ExportToIcs();

        #endregion

        await Verify(ics, "ics");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToIcs(State state)
    {
        #region ExportToIcsState

        var ics = await Holidays.ExportToIcs(state);

        #endregion

        await Verify(ics, "ics");
    }

    [Test]
    public async Task ExportToJson()
    {
        #region ExportToJson

        var json = await Holidays.ExportToJson();

        #endregion

        await Verify(json, "json");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToJson(State state)
    {
        #region ExportToJsonState

        var json = await Holidays.ExportToJson(state);

        #endregion

        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToXml()
    {
        #region ExportToXml

        var xml = await Holidays.ExportToXml();

        #endregion

        await Verify(xml, "xml");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToXml(State state)
    {
        #region ExportToXmlState

        var xml = await Holidays.ExportToXml(state);

        #endregion

        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToCsv()
    {
        #region ExportToCsv

        var csv = await Holidays.ExportToCsv();

        #endregion

        await Verify(csv, "csv");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToCsv(State state)
    {
        #region ExportToCsvState

        var csv = await Holidays.ExportToCsv(state);

        #endregion

        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToExcel()
    {
        #region ExportToExcel

        var bytes = await Holidays.ExportToExcel();

        #endregion

        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToExcel(State state)
    {
        #region ExportToExcelState

        var bytes = await Holidays.ExportToExcel(state);

        #endregion

        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public async Task ExportToJsonPath()
    {
        using var path = new TempFile();
        await Holidays.ExportToJson(path);
        var json = await File.ReadAllTextAsync(path);
        await Verify(json, "json");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToJsonPath(State state)
    {
        using var path = new TempFile();
        await Holidays.ExportToJson(path, state);
        var json = await File.ReadAllTextAsync(path);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToXmlPath()
    {
        using var path = new TempFile();
        await Holidays.ExportToXml(path);
        var xml = await File.ReadAllTextAsync(path);
        await Verify(xml, "xml");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToXmlPath(State state)
    {
        using var path = new TempFile();
        await Holidays.ExportToXml(path, state);
        var xml = await File.ReadAllTextAsync(path);
        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToCsvPath()
    {
        using var path = new TempFile();
        await Holidays.ExportToCsv(path);
        var csv = await File.ReadAllTextAsync(path);
        await Verify(csv, "csv");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToCsvPath(State state)
    {
        using var path = new TempFile();
        await Holidays.ExportToCsv(path, state);
        var csv = await File.ReadAllTextAsync(path);
        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToExcelPath()
    {
        using var path = new TempFile();
        await Holidays.ExportToExcel(path);
        var bytes = await File.ReadAllBytesAsync(path);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [TestCaseSource(nameof(GetStates))]
    public async Task ExportToExcelPath(State state)
    {
        using var path = new TempFile();
        await Holidays.ExportToExcel(path, state);
        var bytes = await File.ReadAllBytesAsync(path);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public void ForYearsSnippet()
    {
        #region ForYears

        var holidays = Holidays.ForYears(startYear: 2025, yearCount: 2);
        foreach (var (date, state, name) in holidays)
        {
            Console.WriteLine($"date: {date}, state: {state}, name: {name}");
        }

        #endregion
    }

    [Test]
    public void ForYearsStateSnippet()
    {
        #region ForYearsState

        var holidays = Holidays.ForYears(State.NSW, startYear: 2025, yearCount: 2);
        foreach (var (date, name) in holidays)
        {
            Console.WriteLine($"date: {date}, name: {name}");
        }

        #endregion
    }

    [Test]
    public void ForNational()
    {
        #region ForNational

        var holidays = Holidays.ForNational(2025);
        foreach (var (date, name) in holidays)
        {
            Console.WriteLine($"date: {date}, name: {name}");
        }

        #endregion
    }

    [Test]
    public void ForState()
    {
        #region ForState

        var holidays = Holidays.ForNsw(2025);
        foreach (var (date, name) in holidays)
        {
            Console.WriteLine($"date: {date}, name: {name}");
        }

        #endregion
    }

    [Test]
    public Task ForYears() =>
        Verify(Holidays.ForYears(2024))
            .DontScrubDateTimes()
            .AddExtraSettings(_ => _.DefaultValueHandling = DefaultValueHandling.Include);

    [Test]
    public Task ForYearsState() =>
        Verify(Holidays.ForYears(State.NT, 2024))
            .DontScrubDateTimes()
            .AddExtraSettings(_ => _.DefaultValueHandling = DefaultValueHandling.Include);

    [Test]
    public void GetFederalGovernmentShutdown()
    {
        #region GetFederalGovernmentShutdown

        var (start, end) = Holidays.GetFederalGovernmentShutdown(startYear: 2024);

        AreEqual(new Date(2024, 12, 25), start);
        AreEqual(new Date(2025, 1, 1), end);

        #endregion
    }

    [Test]
    public void IsHoliday()
    {
        #region IsHoliday

        var date = new Date(2024, 12, 25);

        IsTrue(date.IsHoliday(State.NSW));

        #endregion
    }

    [Test]
    public void IsHolidayForStateNamed()
    {
        #region IsHolidayForStateNamed

        var date = new Date(2024, 12, 25);

        IsTrue(date.IsNswHoliday(out var name));
        AreEqual("Christmas Day", name);

        #endregion
    }

    [Test]
    public void IsHolidayForState()
    {
        #region IsHolidayForState

        var date = new Date(2024, 12, 25);

        IsTrue(date.IsNswHoliday());

        #endregion
    }

    [Test]
    public void IsHolidayNamed()
    {
        #region IsHolidayNamed

        var date = new Date(2024, 12, 25);

        IsTrue(date.IsHoliday(State.NSW, out var name));

        AreEqual("Christmas Day", name);

        #endregion
    }

    [Test]
    public void IsFederalGovernmentShutdown()
    {
        #region IsFederalGovernmentShutdown

        var date = new Date(2025, 12, 30);
        var result = date.IsFederalGovernmentShutdown();

        IsTrue(result);

        #endregion
    }

    [Test]
    public Task WriteNsw() =>
        Verify(
            WriteForState(Holidays.IsNswHoliday));

    [Test]
    public Task WriteAct() =>
        Verify(
            WriteForState(Holidays.IsActHoliday));

    [Test]
    public Task WriteWa() =>
        Verify(
            WriteForState(Holidays.IsWaHoliday));

    [Test]
    public Task WriteSa() =>
        Verify(
            WriteForState(Holidays.IsSaHoliday));

    [Test]
    public Task WriteTas() =>
        Verify(
            WriteForState(Holidays.IsTasHoliday));

    [Test]
    public Task WriteNt() =>
        Verify(
            WriteForState(Holidays.IsNtHoliday));

    [Test]
    public Task WriteVic() =>
        Verify(
            WriteForState(Holidays.IsVicHoliday));

    [Test]
    public Task WriteQld() =>
        Verify(
            WriteForState(Holidays.IsQldHoliday));

    delegate bool HolidayCheck(Date date, [NotNullWhen(true)] out string? name);

    static string WriteForState(HolidayCheck isHoliday)
    {
        var builder = new StringBuilder();
        var now = DateTime.Now;
        for (var year = now.Year - 1; year <= DateTime.Now.Year + 4; year++)
        {
            var start = new Date(year, 1, 1);
            var end = new Date(year, 12, 31);

            builder.AppendLine($"{year}");
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                if (isHoliday(date, out var name))
                {
                    builder.AppendLine($"    {date.ToString("MMM dd ddd", CultureInfo.InvariantCulture)} - {name}");
                }
            }
        }

        return builder.ToString();
    }

    [TestCaseSource(nameof(GetStates))]
    public Task IsPublicHoliday(State state)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsHoliday(state))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}");
            }
        }

        return Verify(builder);
    }

    public static IEnumerable<State> GetStates() =>
        Enum.GetValues<State>();

    static readonly State[] multipleStates = [State.NSW, State.VIC];

    [Test]
    public async Task ExportToJsonMultiState()
    {
        var json = await Holidays.ExportToJson(multipleStates);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToJsonMultiStatePath()
    {
        using var path = new TempFile();
        await Holidays.ExportToJson(path, multipleStates);
        var json = await File.ReadAllTextAsync(path);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToCsvMultiState()
    {
        var csv = await Holidays.ExportToCsv(multipleStates);
        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToCsvMultiStatePath()
    {
        using var path = new TempFile();
        await Holidays.ExportToCsv(path, multipleStates);
        var csv = await File.ReadAllTextAsync(path);
        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToXmlMultiState()
    {
        var xml = await Holidays.ExportToXml(multipleStates);
        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToXmlMultiStatePath()
    {
        using var path = new TempFile();
        await Holidays.ExportToXml(path, multipleStates);
        var xml = await File.ReadAllTextAsync(path);
        await Verify(xml, "xml");
    }

    [Test]
    public async Task ExportToMarkdownMultiState()
    {
        var md = await Holidays.ExportToMarkdown(multipleStates);
        await Verify(md, "md");
    }

    [Test]
    public async Task ExportToIcsMultiState()
    {
        var ics = await Holidays.ExportToIcs(multipleStates);
        await Verify(ics, "ics");
    }

    [Test]
    public async Task ExportToIcsAllStates()
    {
        var allStates = Enum.GetValues<State>();
        var ics = await Holidays.ExportToIcs(allStates);
        await Verify(ics, "ics");
    }

    [Test]
    public async Task ExportToExcelMultiState()
    {
        var bytes = await Holidays.ExportToExcel(multipleStates);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public async Task ExportToExcelMultiStatePath()
    {
        using var path = new TempFile();
        await Holidays.ExportToExcel(path, multipleStates);
        var bytes = await File.ReadAllBytesAsync(path);
        var stream = new MemoryStream(bytes);
        await Verify(stream, "xlsx");
    }

    [Test]
    public async Task ExportToJsonAllStates()
    {
        var allStates = Enum.GetValues<State>();
        var json = await Holidays.ExportToJson(allStates);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToCsvAllStates()
    {
        var allStates = Enum.GetValues<State>();
        var csv = await Holidays.ExportToCsv(allStates);
        await Verify(csv, "csv");
    }

    [Test]
    public async Task ExportToJsonEmptyStates()
    {
        var emptyStates = Array.Empty<State>();
        var json = await Holidays.ExportToJson(emptyStates);
        await Verify(json, "json");
    }

    [Test]
    public async Task ExportToCsvEmptyStates()
    {
        var emptyStates = Array.Empty<State>();
        var csv = await Holidays.ExportToCsv(emptyStates);
        await Verify(csv, "csv");
    }
}
