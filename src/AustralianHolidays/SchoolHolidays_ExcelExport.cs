namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <summary>
    /// Exports school holiday periods for all states to Excel (XLSX) format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A byte array containing the Excel file data.</returns>
    public static async Task<byte[]> ExportToExcel(int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, startYear, yearCount, cancel);
        return stream.ToArray();
    }

    /// <summary>
    /// Exports school holiday periods for all states to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToExcel(Stream stream, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(startYear, yearCount);
        return ToExcelMultiState(stream, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to Excel (XLSX) format.
    /// </summary>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A byte array containing the Excel file data.</returns>
    public static async Task<byte[]> ExportToExcel(State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, state, startYear, yearCount, cancel);
        return stream.ToArray();
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToExcel(Stream stream, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(state, startYear, yearCount);
        return ToExcel(stream, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for all states to an Excel (XLSX) file.
    /// </summary>
    /// <param name="path">The file path where the Excel file will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToExcel(string path, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to an Excel (XLSX) file.
    /// </summary>
    /// <param name="path">The file path where the Excel file will be written.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToExcel(string path, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, state, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to Excel (XLSX) format.
    /// </summary>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A byte array containing the Excel file data with Start, End, State, and Name columns.</returns>
    public static async Task<byte[]> ExportToExcel(IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, states, startYear, yearCount, cancel);
        return stream.ToArray();
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToExcel(Stream stream, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var holidays = HolidaysForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToExcelMultiState(stream, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to an Excel (XLSX) file.
    /// </summary>
    /// <param name="path">The file path where the Excel file will be written.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToExcel(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, states, startYear, yearCount, cancel);
    }

    static async Task ToExcel(Stream stream, IEnumerable<(Date start, Date end, string name)> holidays, Cancel cancel)
    {
        var assembly = typeof(SchoolHolidays).Assembly;
        await using var templateStream = assembly.GetManifestResourceStream("AustralianHolidays.Resources.HolidayTemplate.xlsx");

        if (templateStream == null)
        {
            throw new InvalidOperationException("Could not load embedded Excel template resource.");
        }

        using var tempStream = new MemoryStream();
        await templateStream.CopyToAsync(tempStream, cancel);
        tempStream.Position = 0;

        using var archive = new ZipArchive(tempStream, ZipArchiveMode.Read);
        using var outputArchive = new ZipArchive(stream, ZipArchiveMode.Create, true);

        foreach (var entry in archive.Entries)
        {
            var outputEntry = outputArchive.CreateEntry(entry.FullName);

            if (entry.FullName == "xl/worksheets/sheet1.xml")
            {
                await using var entryStream = await outputEntry.OpenAsync(cancel);
                await WriteSheetXml(entryStream, holidays, cancel);
            }
            else
            {
                await using var entryStream = await entry.OpenAsync(cancel);
                await using var outputStream = await outputEntry.OpenAsync(cancel);
                await entryStream.CopyToAsync(outputStream, cancel);
            }
        }
    }

    static async Task WriteSheetXml(Stream stream, IEnumerable<(Date start, Date end, string name)> holidays, Cancel cancel)
    {
        await using var writer = new StreamWriter(stream);

        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", cancel);
        await writer.WriteAsync("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">", cancel);

        await writer.WriteAsync("<cols>", cancel);
        await writer.WriteAsync("<col min=\"1\" max=\"1\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"2\" max=\"2\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"3\" max=\"3\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("</cols>", cancel);

        await writer.WriteAsync("<sheetData>", cancel);

        await writer.WriteAsync("<row r=\"1\">", cancel);
        await writer.WriteAsync("<c r=\"A1\" s=\"1\" t=\"inlineStr\"><is><t>Start</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"B1\" s=\"1\" t=\"inlineStr\"><is><t>End</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"C1\" s=\"1\" t=\"inlineStr\"><is><t>Name</t></is></c>", cancel);
        await writer.WriteAsync("</row>", cancel);

        var rowNum = 2;
        foreach (var (start, end, name) in holidays)
        {
            await writer.WriteAsync($"<row r=\"{rowNum}\">", cancel);
            await writer.WriteAsync($"<c r=\"A{rowNum}\" t=\"inlineStr\"><is><t>{start.ToString("yyyy-MM-dd")}</t></is></c>", cancel);
            await writer.WriteAsync($"<c r=\"B{rowNum}\" t=\"inlineStr\"><is><t>{end.ToString("yyyy-MM-dd")}</t></is></c>", cancel);
            await writer.WriteAsync($"<c r=\"C{rowNum}\" t=\"inlineStr\"><is><t>{SecurityElement.Escape(name)}</t></is></c>", cancel);
            await writer.WriteAsync("</row>", cancel);
            rowNum++;
        }

        await writer.WriteAsync("</sheetData>", cancel);
        await writer.WriteAsync("</worksheet>", cancel);
    }

    static async Task ToExcelMultiState(Stream stream, IEnumerable<(Date start, Date end, State state, string name)> holidays, Cancel cancel)
    {
        var assembly = typeof(SchoolHolidays).Assembly;
        await using var templateStream = assembly.GetManifestResourceStream("AustralianHolidays.Resources.HolidayTemplate.xlsx");

        if (templateStream == null)
        {
            throw new InvalidOperationException("Could not load embedded Excel template resource.");
        }

        using var tempStream = new MemoryStream();
        await templateStream.CopyToAsync(tempStream, cancel);
        tempStream.Position = 0;

        using var archive = new ZipArchive(tempStream, ZipArchiveMode.Read);
        using var outputArchive = new ZipArchive(stream, ZipArchiveMode.Create, true);

        foreach (var entry in archive.Entries)
        {
            var outputEntry = outputArchive.CreateEntry(entry.FullName);

            if (entry.FullName == "xl/worksheets/sheet1.xml")
            {
                await using var entryStream = await outputEntry.OpenAsync(cancel);
                await WriteSheetXmlMultiState(entryStream, holidays, cancel);
            }
            else
            {
                await using var entryStream = await entry.OpenAsync(cancel);
                await using var outputStream = await outputEntry.OpenAsync(cancel);
                await entryStream.CopyToAsync(outputStream, cancel);
            }
        }
    }

    static async Task WriteSheetXmlMultiState(Stream stream, IEnumerable<(Date start, Date end, State state, string name)> holidays, Cancel cancel)
    {
        await using var writer = new StreamWriter(stream);

        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", cancel);
        await writer.WriteAsync("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">", cancel);

        await writer.WriteAsync("<cols>", cancel);
        await writer.WriteAsync("<col min=\"1\" max=\"1\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"2\" max=\"2\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"3\" max=\"3\" width=\"8\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"4\" max=\"4\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("</cols>", cancel);

        await writer.WriteAsync("<sheetData>", cancel);

        await writer.WriteAsync("<row r=\"1\">", cancel);
        await writer.WriteAsync("<c r=\"A1\" s=\"1\" t=\"inlineStr\"><is><t>Start</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"B1\" s=\"1\" t=\"inlineStr\"><is><t>End</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"C1\" s=\"1\" t=\"inlineStr\"><is><t>State</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"D1\" s=\"1\" t=\"inlineStr\"><is><t>Name</t></is></c>", cancel);
        await writer.WriteAsync("</row>", cancel);

        var rowNum = 2;
        foreach (var (start, end, state, name) in holidays)
        {
            await writer.WriteAsync($"<row r=\"{rowNum}\">", cancel);
            await writer.WriteAsync($"<c r=\"A{rowNum}\" t=\"inlineStr\"><is><t>{start.ToString("yyyy-MM-dd")}</t></is></c>", cancel);
            await writer.WriteAsync($"<c r=\"B{rowNum}\" t=\"inlineStr\"><is><t>{end.ToString("yyyy-MM-dd")}</t></is></c>", cancel);
            await writer.WriteAsync($"<c r=\"C{rowNum}\" t=\"inlineStr\"><is><t>{state}</t></is></c>", cancel);
            await writer.WriteAsync($"<c r=\"D{rowNum}\" t=\"inlineStr\"><is><t>{SecurityElement.Escape(name)}</t></is></c>", cancel);
            await writer.WriteAsync("</row>", cancel);
            rowNum++;
        }

        await writer.WriteAsync("</sheetData>", cancel);
        await writer.WriteAsync("</worksheet>", cancel);
    }
}
