namespace AustralianHolidays;

public static partial class Holidays
{
    public static async Task<string> ExportToIcs(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            writer.NewLine = "\r\n";
            await ExportToIcs(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToIcs(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);

        return ToIcs(writer, forYears);
    }

    public static async Task<string> ExportToIcs(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            writer.NewLine = "\r\n";
            await ExportToIcs(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToIcs(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);

        return ToIcs(writer, forYears);
    }

    static async Task ToIcs(TextWriter writer, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        await writer.WriteLineAsync(
            """
            BEGIN:VCALENDAR
            VERSION:2.0
            """);

        foreach (var (date, name) in forYears)
        {
            await writer.WriteLineAsync(
                $"""
                 BEGIN:VEVENT
                 SUMMARY:{name}
                 DTSTART;VALUE=DATE:{date:yyyyMMdd}
                 DTEND;VALUE=DATE:{date.AddDays(1):yyyyMMdd}
                 END:VEVENT
                 """);
        }

        await writer.WriteLineAsync("END:VCALENDAR");
    }
}