[TestFixture]
public class EasterTests
{
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

    [Test]
    public void GetEasterMonday()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterMonday = EasterCalculator.GetEasterMonday(i);
            var expected = DateBuilder.EasterFridays[i - 2025].AddDays(3);
            AreEqual(expected, easterMonday);
        }
    }

    [Test]
    public void IsEasterFriday_ValidDates()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterFriday = EasterCalculator.GetEasterFriday(i);
            IsTrue(easterFriday.IsEasterFriday());
        }
    }

    [Test]
    public void IsEasterFriday_InvalidDates()
    {
        // Test day before Easter Friday
        var dayBefore = EasterCalculator.GetEasterFriday(2026).AddDays(-1);
        IsFalse(dayBefore.IsEasterFriday());

        // Test day after Easter Friday
        var dayAfter = EasterCalculator.GetEasterFriday(2025).AddDays(1);
        IsFalse(dayAfter.IsEasterFriday());

        // Test random date
        IsFalse(new Date(2025, 1, 1).IsEasterFriday());
    }

    [Test]
    public void IsEasterSunday_ValidDates()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var (_, _, sunday, _) = EasterCalculator.ForYear(i);
            IsTrue(sunday.IsEasterSunday());
        }
    }

    [Test]
    public void IsEasterSunday_InvalidDates()
    {
        var (friday, saturday, _, monday) = EasterCalculator.ForYear(2025);

        IsFalse(friday.IsEasterSunday());
        IsFalse(saturday.IsEasterSunday());
        IsFalse(monday.IsEasterSunday());
        IsFalse(new Date(2025, 1, 1).IsEasterSunday());
    }

    [Test]
    public void IsEasterMonday_ValidDates()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterMonday = EasterCalculator.GetEasterMonday(i);
            IsTrue(easterMonday.IsEasterMonday());
        }
    }

    [Test]
    public void IsEasterMonday_InvalidDates()
    {
        // Test day before Easter Monday
        var dayBefore = EasterCalculator.GetEasterMonday(2025).AddDays(-1);
        IsFalse(dayBefore.IsEasterMonday());

        // Test day after Easter Monday
        var dayAfter = EasterCalculator.GetEasterMonday(2025).AddDays(1);
        IsFalse(dayAfter.IsEasterMonday());

        // Test random date
        IsFalse(new Date(2025, 7, 1).IsEasterMonday());
    }

    [Test]
    public Task EdgeCases_EarlyAndLateEaster()
    {
        var builder = new StringBuilder();

        // Years with Easter in March
        var marchYears = new[] { 2008, 2016, 2024, 2035 };
        foreach (var year in marchYears)
        {
            var (friday, saturday, sunday, monday) = EasterCalculator.ForYear(year);
            builder.AppendLine(
                $"""
                 {year} (March Easter)
                    friday: {friday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                    saturday: {saturday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                    sunday: {sunday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                    monday: {monday.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}
                 """);
        }

        return Verify(builder);
    }

    [Test]
    public Task HistoricalAndFutureYears()
    {
        var builder = new StringBuilder();

        // Historical years
        var years = new[] { 1900, 1950, 2000, 2010, 2050, 2100 };
        foreach (var year in years)
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
}
