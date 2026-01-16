using System.IO.Compression;
using System.Security;

namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to Excel (XLSX) format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A byte array containing the Excel file data.</returns>
    public static async Task<byte[]> ExportToExcel(int? startYear = null, int yearCount = 5)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, startYear, yearCount);
        return stream.ToArray();
    }

    /// <summary>
    /// Exports national public holidays to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToExcel(Stream stream, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToExcel(stream, forYears);
    }

    /// <summary>
    /// Exports public holidays for a specific state to Excel (XLSX) format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A byte array containing the Excel file data.</returns>
    public static async Task<byte[]> ExportToExcel(State state, int? startYear = null, int yearCount = 5)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, state, startYear, yearCount);
        return stream.ToArray();
    }

    /// <summary>
    /// Exports public holidays for a specific state to Excel (XLSX) format, writing to a Stream.
    /// </summary>
    /// <param name="stream">The Stream to write the Excel file data to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToExcel(Stream stream, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToExcel(stream, forYears);
    }

    /// <summary>
    /// Exports national public holidays to an Excel (XLSX) file.
    /// </summary>
    /// <param name="path">The file path where the Excel file will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToExcel(string path, int? startYear = null, int yearCount = 5)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, startYear, yearCount);
    }

    /// <summary>
    /// Exports public holidays for a specific state to an Excel (XLSX) file.
    /// </summary>
    /// <param name="path">The file path where the Excel file will be written.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToExcel(string path, State state, int? startYear = null, int yearCount = 5)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, state, startYear, yearCount);
    }

    static async Task ToExcel(Stream stream, IOrderedEnumerable<(Date date, string name)> forYears)
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
        await templateStream.CopyToAsync(tempStream);
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
                await using var entryStream = outputEntry.Open();
                await WriteSheetXml(entryStream, forYears);
            }
            else
            {
                // Copy other files as-is
                await using var entryStream = entry.Open();
                await using var outputStream = outputEntry.Open();
                await entryStream.CopyToAsync(outputStream);
            }
        }
    }

    static async Task WriteSheetXml(Stream stream, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        await using var writer = new StreamWriter(stream);

        // Write Office Open XML structure for worksheet
        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        await writer.WriteAsync("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");

        // Column widths: Date column ~100px (14 char units), Name column ~500px (70 char units)
        await writer.WriteAsync("<cols>");
        await writer.WriteAsync("<col min=\"1\" max=\"1\" width=\"14\" customWidth=\"1\"/>");
        await writer.WriteAsync("<col min=\"2\" max=\"2\" width=\"70\" customWidth=\"1\"/>");
        await writer.WriteAsync("</cols>");

        await writer.WriteAsync("<sheetData>");

        // Header row
        await writer.WriteAsync("<row r=\"1\">");
        await writer.WriteAsync("<c r=\"A1\" s=\"1\" t=\"inlineStr\"><is><t>Date</t></is></c>");
        await writer.WriteAsync("<c r=\"B1\" s=\"1\" t=\"inlineStr\"><is><t>Name</t></is></c>");
        await writer.WriteAsync("</row>");

        // Data rows
        var rowNum = 2;
        foreach (var (date, name) in forYears)
        {
            await writer.WriteAsync($"<row r=\"{rowNum}\">");

            // Date cell (as inline string with yyyy-MM-dd format)
            var dateString = date.ToString("yyyy-MM-dd");
            await writer.WriteAsync($"<c r=\"A{rowNum}\" t=\"inlineStr\"><is><t>{dateString}</t></is></c>");

            // Name cell (as inline string)
            var escapedName = SecurityElement.Escape(name);
            await writer.WriteAsync($"<c r=\"B{rowNum}\" t=\"inlineStr\"><is><t>{escapedName}</t></is></c>");

            await writer.WriteAsync("</row>");
            rowNum++;
        }

        await writer.WriteAsync("</sheetData>");
        await writer.WriteAsync("</worksheet>");
    }
}
