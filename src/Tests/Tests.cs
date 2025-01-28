using System.Diagnostics.CodeAnalysis;

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
        var builder = new StringBuilder();
        for (var year = DateTime.Now.Year; year <= DateTime.Now.Year + 3; year++)
        {
            var start = new Date(year, 1, 1);
            var end = new Date(year, 12, 31);

            builder.AppendLine($"{year}");
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                if (date.IsPublicHoliday(state, out var name))
                {
                    builder.AppendLine($"    {name.PadRight(21)} {date.ToString("MMM dd ddd", CultureInfo.InvariantCulture)}");
                }
            }
        }

        return Verify(builder);
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
        for (var year = DateTime.Now.Year; year <= DateTime.Now.Year + 3; year++)
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