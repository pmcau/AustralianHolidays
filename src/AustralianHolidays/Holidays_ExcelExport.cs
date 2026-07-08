namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to Excel (XLSX) format.
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
    /// Exports national public holidays to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToExcel(Stream stream, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToExcel(stream, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for a specific state to Excel (XLSX) format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
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
    /// Exports public holidays for a specific state to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToExcel(Stream stream, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToExcel(stream, forYears, cancel);
    }

    /// <summary>
    /// Exports national public holidays to an Excel (XLSX) file.
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
    /// Exports public holidays for a specific state to an Excel (XLSX) file.
    /// </summary>
    /// <param name="path">The file path where the Excel file will be written.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToExcel(string path, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, state, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to Excel (XLSX) format.
    /// </summary>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A byte array containing the Excel file data with Date, State, and Name columns.</returns>
    public static async Task<byte[]> ExportToExcel(IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, states, startYear, yearCount, cancel);
        return stream.ToArray();
    }

    /// <summary>
    /// Exports public holidays for multiple states to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToExcel(Stream stream, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var forYears = ForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToExcelMultiState(stream, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to an Excel (XLSX) file.
    /// </summary>
    /// <param name="path">The file path where the Excel file will be written.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToExcel(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, states, startYear, yearCount, cancel);
    }

    static async Task ToExcel(Stream stream, IOrderedEnumerable<(Date date, string name)> forYears, Cancel cancel)
    {
        // Load embedded template
        var assembly = typeof(Holidays).Assembly;
        await using var templateStream = assembly.GetManifestResourceStream("AustralianHolidays.Resources.HolidayTemplate.xlsx");

        if (templateStream == null)
        {
            throw new InvalidOperationException("Could not load embedded Excel template resource.");
        }

        // Create a temporary memory stream for the template
        using var tempStream = new MemoryStream();
        await templateStream.CopyToAsync(tempStream, cancel);
        tempStream.Position = 0;

        using var archive = new ZipArchive(tempStream, ZipArchiveMode.Read);
        using var outputArchive = new ZipArchive(stream, ZipArchiveMode.Create, true);

        // Copy all files from template to output
        foreach (var entry in archive.Entries)
        {
            var outputEntry = outputArchive.CreateEntry(entry.FullName);

            if (entry.FullName == "xl/worksheets/sheet1.xml")
            {
                // Inject holiday data into sheet XML
                await using var entryStream = await outputEntry.OpenAsync(cancel);
                // ReSharper disable once PossibleMultipleEnumeration
                await WriteSheetXml(entryStream, forYears, cancel);
            }
            else
            {
                // Copy other files as-is
                await using var entryStream = await entry.OpenAsync(cancel);
                await using var outputStream = await outputEntry.OpenAsync(cancel);
                await entryStream.CopyToAsync(outputStream, cancel);
            }
        }
    }

    static async Task WriteSheetXml(Stream stream, IOrderedEnumerable<(Date date, string name)> forYears, Cancel cancel = default)
    {
        await using var writer = new StreamWriter(stream);

        // Write Office Open XML structure for worksheet
        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", cancel);
        await writer.WriteAsync("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">", cancel);

        // Column widths: Date column ~100px (14 char units), Name column ~500px (70 char units)
        await writer.WriteAsync("<cols>", cancel);
        await writer.WriteAsync("<col min=\"1\" max=\"1\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"2\" max=\"2\" width=\"70\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("</cols>", cancel);

        await writer.WriteAsync("<sheetData>", cancel);

        // Header row
        await writer.WriteAsync("<row r=\"1\">", cancel);
        await writer.WriteAsync("<c r=\"A1\" s=\"1\" t=\"inlineStr\"><is><t>Date</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"B1\" s=\"1\" t=\"inlineStr\"><is><t>Name</t></is></c>", cancel);
        await writer.WriteAsync("</row>", cancel);

        // Data rows
        var rowNum = 2;
        foreach (var (date, name) in forYears)
        {
            await writer.WriteAsync($"<row r=\"{rowNum}\">", cancel);

            // Date cell (as inline string with yyyy-MM-dd format)
            var dateString = date.ToString("yyyy-MM-dd");
            await writer.WriteAsync($"<c r=\"A{rowNum}\" t=\"inlineStr\"><is><t>{dateString}</t></is></c>", cancel);

            // Name cell (as inline string)
            var escapedName = SecurityElement.Escape(name);
            await writer.WriteAsync($"<c r=\"B{rowNum}\" t=\"inlineStr\"><is><t>{escapedName}</t></is></c>", cancel);

            await writer.WriteAsync("</row>", cancel);
            rowNum++;
        }

        await writer.WriteAsync("</sheetData>", cancel);
        await writer.WriteAsync("</worksheet>", cancel);
    }

    static async Task ToExcelMultiState(Stream stream, IEnumerable<(Date date, State state, string name)> forYears, Cancel cancel)
    {
        // Load embedded template
        var assembly = typeof(Holidays).Assembly;
        await using var templateStream = assembly.GetManifestResourceStream("AustralianHolidays.Resources.HolidayTemplate.xlsx");

        if (templateStream == null)
        {
            throw new InvalidOperationException("Could not load embedded Excel template resource.");
        }

        using var archive = new ZipArchive(templateStream, ZipArchiveMode.Read);
        using var outputArchive = new ZipArchive(stream, ZipArchiveMode.Create, true);

        // Copy all files from template to output
        foreach (var entry in archive.Entries)
        {
            var outputEntry = outputArchive.CreateEntry(entry.FullName);

            if (entry.FullName == "xl/worksheets/sheet1.xml")
            {
                // Inject holiday data into sheet XML
                await using var entryStream = await outputEntry.OpenAsync(cancel);
                // ReSharper disable once PossibleMultipleEnumeration
                await WriteSheetXmlMultiState(entryStream, forYears, cancel);
            }
            else
            {
                // Copy other files as-is
                await using var entryStream = await entry.OpenAsync(cancel);
                await using var outputStream = await outputEntry.OpenAsync(cancel);
                await entryStream.CopyToAsync(outputStream, cancel);
            }
        }
    }

    static async Task WriteSheetXmlMultiState(Stream stream, IEnumerable<(Date date, State state, string name)> forYears, Cancel cancel = default)
    {
        await using var writer = new StreamWriter(stream);

        // Write Office Open XML structure for worksheet
        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", cancel);
        await writer.WriteAsync("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">", cancel);

        // Column widths: Date column ~100px (14 char units), State column ~50px (8 char units), Name column ~500px (70 char units)
        await writer.WriteAsync("<cols>", cancel);
        await writer.WriteAsync("<col min=\"1\" max=\"1\" width=\"14\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"2\" max=\"2\" width=\"8\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("<col min=\"3\" max=\"3\" width=\"70\" customWidth=\"1\"/>", cancel);
        await writer.WriteAsync("</cols>");

        await writer.WriteAsync("<sheetData>", cancel);

        // Header row
        await writer.WriteAsync("<row r=\"1\">", cancel);
        await writer.WriteAsync("<c r=\"A1\" s=\"1\" t=\"inlineStr\"><is><t>Date</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"B1\" s=\"1\" t=\"inlineStr\"><is><t>State</t></is></c>", cancel);
        await writer.WriteAsync("<c r=\"C1\" s=\"1\" t=\"inlineStr\"><is><t>Name</t></is></c>", cancel);
        await writer.WriteAsync("</row>", cancel);

        // Data rows
        var rowNum = 2;
        foreach (var (date, state, name) in forYears)
        {
            await writer.WriteAsync($"<row r=\"{rowNum}\">", cancel);

            // Date cell (as inline string with yyyy-MM-dd format)
            var dateString = date.ToString("yyyy-MM-dd");
            await writer.WriteAsync($"<c r=\"A{rowNum}\" t=\"inlineStr\"><is><t>{dateString}</t></is></c>", cancel);

            // State cell (as inline string)
            await writer.WriteAsync($"<c r=\"B{rowNum}\" t=\"inlineStr\"><is><t>{state}</t></is></c>", cancel);

            // Name cell (as inline string)
            var escapedName = SecurityElement.Escape(name);
            await writer.WriteAsync($"<c r=\"C{rowNum}\" t=\"inlineStr\"><is><t>{escapedName}</t></is></c>", cancel);

            await writer.WriteAsync("</row>", cancel);
            rowNum++;
        }

        await writer.WriteAsync("</sheetData>", cancel);
        await writer.WriteAsync("</worksheet>", cancel);
    }
}
