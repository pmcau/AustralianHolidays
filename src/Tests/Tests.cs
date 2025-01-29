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
            if (date.IsPublicHoliday(state, out var name))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)} {name}");
            }
        }

        return Verify(builder);
    }

    [TestCaseSource(nameof(GetStates))]
    public Task WriteByYears(State state)
    {
        List<int> years = [];
        for (var year = DateTime.Now.Year; year <= DateTime.Now.Year + 3; year++)
        {
            years.Add(year);
        }

        var forYears = Holidays.ForYears(state, years);

        var builder = new StringBuilder();
        builder.AppendLine($"| {string.Join(" | ", years)} |");
        builder.Append('|');
        for (var index = 0; index < years.Count; index++)
        {
            builder.Append("------|");
        }
        builder.AppendLine();

        var items = forYears.GroupBy(_ => _.name)
            .OrderBy(_=> _.First().date.Month)
            .ThenBy(_=> _.First().date.Day);

        foreach (var item in items)
        {
            builder.Append("| " + item.Key.PadRight(30) + " | ");
            foreach (var year in years)
            {
                var dates = item.Select(_=>_.date).Where(_ => _.Year == year).ToList();
                if (dates.Count != 0)
                {
                    builder.Append(string.Join(", ", dates.Select(_=>_.ToString("MMM dd ddd", CultureInfo.InvariantCulture))));
                }

                builder.Append(" | ");
            }
            builder.AppendLine();
        }

        return Verify(builder);
    }

    [Test]
    public Task ForYear() =>
        Verify(Holidays.ForYear(2024))
            .DontScrubDateTimes()
            .AddExtraSettings(_ => _.DefaultValueHandling = DefaultValueHandling.Include);

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

    delegate bool IsHoliday(Date date, [NotNullWhen(true)] out string? name);

    static string WriteForState(IsHoliday isHoliday)
    {
        var builder = new StringBuilder();
        for (var year = DateTime.Now.Year - 1; year <= DateTime.Now.Year + 4; year++)
        {
            var start = new Date(year, 1, 1);
            var end = new Date(year, 12, 31);

            builder.AppendLine($"{year}");
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                if (isHoliday(date, out var name))
                {
                    builder.AppendLine($"    {name.PadRight(21)} {date.ToString("MMM dd ddd", CultureInfo.InvariantCulture)}");
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
            if (date.IsPublicHoliday(state))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}");
            }
        }

        return Verify(builder);
    }

    public static IEnumerable<State> GetStates() =>
        Enum.GetValues<State>();
}