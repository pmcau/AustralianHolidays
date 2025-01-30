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
        for (var year = DateTime.Now.Year; year <= DateTime.Now.Year + 4; year++)
        {
            years.Add(year);
        }

        var forYears = Holidays.ForYears(state, DateTime.Now.Year, 4);

        var builder = new StringBuilder();
        builder.AppendLine($"|      | {string.Join(" | ", years)} |");
        builder.Append('|');
        for (var index = 0; index < years.Count + 1; index++)
        {
            builder.Append("------|");
        }

        builder.AppendLine();

        var items = forYears.GroupBy(_ => _.name)
            .OrderBy(_ => _.First().date.Month)
            .ThenBy(_ => _.First().date.Day);

        foreach (var item in items)
        {
            builder.Append("| " + item.Key.Replace(" (", "<br>(").PadRight(30) + " | ");
            foreach (var year in years)
            {
                var dates = item.Select(_ => _.date).Where(_ => _.Year == year).ToList();
                if (dates.Count != 0)
                {
                    builder.Append(string.Join("<br>", dates.Select(_ => _.ToString("`ddd dd MMM`", CultureInfo.InvariantCulture))));
                }

                builder.Append(" | ");
            }

            builder.AppendLine();
        }

        return Verify(builder);
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
    public Task ForYears() =>
        Verify(Holidays.ForYears(2024))
            .DontScrubDateTimes()
            .AddExtraSettings(_ => _.DefaultValueHandling = DefaultValueHandling.Include);

    [Test]
    public void GetFederalGovernmentShutdown()
    {
        #region GetFederalGovernmentShutdown

        var (start, end) = Holidays.GetFederalGovernmentShutdown(yearStart: 2024);

        AreEqual(new Date(2024, 12, 25), start);
        AreEqual(new Date(2025, 1, 1), end);

        #endregion
    }

    [Test]
    public void IsPublicHoliday()
    {
        #region IsPublicHoliday

        var date = new Date(2024, 12, 25);

        IsTrue(date.IsPublicHoliday(State.NSW));

        #endregion
    }
    [Test]
    public void IsPublicHolidayNamed()
    {
        #region IsPublicHolidayNamed

        var date = new Date(2024, 12, 25);

        IsTrue(date.IsPublicHoliday(State.NSW, out var name));

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