using System.IO.Compression;
using System.Security;

namespace AustralianHolidays;

public static partial class Holidays
{
    public static async Task<byte[]> ExportToExcel(int? startYear = null, int yearCount = 5)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, startYear, yearCount);
        return stream.ToArray();
    }

    public static Task ExportToExcel(Stream stream, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToExcel(stream, forYears);
    }

    public static async Task<byte[]> ExportToExcel(State state, int? startYear = null, int yearCount = 5)
    {
        var stream = new MemoryStream();
        await ExportToExcel(stream, state, startYear, yearCount);
        return stream.ToArray();
    }

    public static Task ExportToExcel(Stream stream, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToExcel(stream, forYears);
    }

    public static async Task ExportToExcel(string path, int? startYear = null, int yearCount = 5)
    {
        await using var stream = File.Create(path);
        await ExportToExcel(stream, startYear, yearCount);
    }

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

            // Date cell (as date value with date format style)
            var dateValue = new DateTime(date.Year, date.Month, date.Day).ToOADate();
            await writer.WriteAsync($"<c r=\"A{rowNum}\" s=\"2\" t=\"n\"><v>{dateValue}</v></c>");

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
