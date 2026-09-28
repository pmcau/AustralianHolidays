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
    public async Task GetEasterFriday()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterFriday = EasterCalculator.GetEasterFriday(i);
            await Assert.That(easterFriday).IsEqualTo(DateBuilder.EasterFridays[i - 2025]);
        }
    }

    [Test]
    public Task GetEaster() =>
        VerifyTuple(() => EasterCalculator.ForYear(2020))
            .DontScrubDateTimes()
            .Snapshot(
                """
                {
                  friday: 2020-04-10,
                  monday: 2020-04-13,
                  saturday: 2020-04-11,
                  sunday: 2020-04-12
                }
                """);

    [Test]
    public async Task GetEasterMonday()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterMonday = EasterCalculator.GetEasterMonday(i);
            var expected = DateBuilder.EasterFridays[i - 2025].AddDays(3);
            await Assert.That(easterMonday).IsEqualTo(expected);
        }
    }

    [Test]
    public async Task IsEasterFriday_ValidDates()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterFriday = EasterCalculator.GetEasterFriday(i);
            await Assert.That(easterFriday.IsEasterFriday()).IsTrue();
        }
    }

    [Test]
    public async Task IsEasterFriday_InvalidDates()
    {
        // Test day before Easter Friday
        var dayBefore = EasterCalculator.GetEasterFriday(2026).AddDays(-1);
        await Assert.That(dayBefore.IsEasterFriday()).IsFalse();

        // Test day after Easter Friday
        var dayAfter = EasterCalculator.GetEasterFriday(2025).AddDays(1);
        await Assert.That(dayAfter.IsEasterFriday()).IsFalse();

        // Test random date
        await Assert.That(new Date(2025, 1, 1).IsEasterFriday()).IsFalse();
    }

    [Test]
    public async Task IsEasterSunday_ValidDates()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var (_, _, sunday, _) = EasterCalculator.ForYear(i);
            await Assert.That(sunday.IsEasterSunday()).IsTrue();
        }
    }

    [Test]
    public async Task IsEasterSunday_InvalidDates()
    {
        var (friday, saturday, _, monday) = EasterCalculator.ForYear(2025);

        await Assert.That(friday.IsEasterSunday()).IsFalse();
        await Assert.That(saturday.IsEasterSunday()).IsFalse();
        await Assert.That(monday.IsEasterSunday()).IsFalse();
        await Assert.That(new Date(2025, 1, 1).IsEasterSunday()).IsFalse();
    }

    [Test]
    public async Task IsEasterMonday_ValidDates()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterMonday = EasterCalculator.GetEasterMonday(i);
            await Assert.That(easterMonday.IsEasterMonday()).IsTrue();
        }
    }

    [Test]
    public async Task IsEasterMonday_InvalidDates()
    {
        // Test day before Easter Monday
        var dayBefore = EasterCalculator.GetEasterMonday(2025).AddDays(-1);
        await Assert.That(dayBefore.IsEasterMonday()).IsFalse();

        // Test day after Easter Monday
        var dayAfter = EasterCalculator.GetEasterMonday(2025).AddDays(1);
        await Assert.That(dayAfter.IsEasterMonday()).IsFalse();

        // Test random date
        await Assert.That(new Date(2025, 7, 1).IsEasterMonday()).IsFalse();
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
