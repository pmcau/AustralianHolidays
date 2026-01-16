namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to iCalendar (ICS) format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the ICS-formatted calendar data.</returns>
    public static async Task<string> ExportToIcs(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports national public holidays to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToIcs(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);

        return ToIcs(writer, null, forYears);
    }

    /// <summary>
    /// Exports public holidays for a specific state to iCalendar (ICS) format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the ICS-formatted calendar data.</returns>
    public static async Task<string> ExportToIcs(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports public holidays for a specific state to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToIcs(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);

        return ToIcs(writer, state, forYears);
    }

    static async Task ToIcs(TextWriter writer, State? state, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR");
        await writer.WriteLineAsync("VERSION:2.0");

        foreach (var item in forYears)
        {
            await writer.WriteLineAsync("BEGIN:VEVENT");
            await writer.WriteLineAsync($"SUMMARY:{item.name}");
            await writer.WriteLineAsync($"UID:{item.name}_{state}@AustralianHolidays");
            await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{item.date:yyyyMMdd}");
            await writer.WriteLineAsync($"DTEND;VALUE=DATE:{item.date.AddDays(1):yyyyMMdd}");
            await writer.WriteLineAsync("END:VEVENT");
        }

        await writer.WriteLineAsync("END:VCALENDAR");
    }
}