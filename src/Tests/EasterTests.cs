[TestFixture]
public class EasterTests
{
    [TestCaseSource(nameof(GetStates))]
    public Task TryGetPublicHoliday(State state)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (EasterCalculator.TryGetPublicHoliday(date, state, out var name))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)} {name}");
            }
        }

        return Verify(builder);
    }

    public static IEnumerable<State> GetStates() =>
        Enum.GetValues<State>();

    [Test]
    public Task ForYear()
    {
        var builder = new StringBuilder();
        for (var year = 2024; year <= 2035; year++)
        {
            var (friday, saturday, sunday, monday) = EasterCalculator.ForYear(year);
            builder.AppendLine(
                $"""
                 {year}
                    friday: {friday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                    saturday: {saturday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                    sunday: {sunday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                    monday: {monday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                 """);
        }

        return Verify(builder);
    }

    [Test]
    public void GetEasterFriday()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterFriday = EasterCalculator.GetEasterFriday(i);
            AreEqual(DateBuilder.EasterFridays[i - 2025], easterFriday);
        }
    }

    [Test]
    public Task GetEaster() =>
        VerifyTuple(() => EasterCalculator.ForYear(2020))
            .DontScrubDateTimes();
}