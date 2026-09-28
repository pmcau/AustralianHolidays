public class ParliamentTests
{
    [Test]
    [MethodDataSource(nameof(GetChambers))]
    public Task SittingCalendar(Chamber chamber)
    {
        var builder = new StringBuilder();
        foreach (var year in Parliament.CoveredYears(chamber))
        {
            builder.AppendLine(year.ToString());
            foreach (var (start, end, name) in Parliament.GetSittingPeriods(chamber, year))
            {
                builder.AppendLine($"  {name}: {Format(start)} - {Format(end)}");
            }
        }

        return Verify(builder);
    }

    [Test]
    public Task SenateEstimates()
    {
        var builder = new StringBuilder();
        foreach (var year in Parliament.CoveredYears(Chamber.Senate))
        {
            builder.AppendLine(year.ToString());
            foreach (var (start, end, name) in Parliament.GetSenateEstimates(year))
            {
                builder.AppendLine($"  {name}: {Format(start)} - {Format(end)}");
            }
        }

        return Verify(builder)
            .Snapshot(
                """
                2026
                  Additional: Mon 09 Feb 2026 - Thu 12 Feb 2026
                  Budget: Mon 25 May 2026 - Thu 28 May 2026
                  Budget: Tue 02 Jun 2026 - Fri 05 Jun 2026
                  Supplementary Budget: Mon 26 Oct 2026 - Thu 29 Oct 2026

                """);
    }

    // Guards against transcription errors. Sitting periods are runs of consecutive weekdays that never
    // overlap, and IsSittingDay must agree with them for every day of the year.
    [Test]
    [MethodDataSource(nameof(GetChambers))]
    public async Task PeriodsAreConsistent(Chamber chamber)
    {
        foreach (var year in Parliament.CoveredYears(chamber))
        {
            var periods = Parliament.GetSittingPeriods(chamber, year);

            for (var i = 0; i < periods.Count; i++)
            {
                var (start, end, name) = periods[i];
                await Assert.That(start <= end).IsTrue().Because($"{chamber} {year} period {name} starts after it ends");
                await Assert.That(start.Year).IsEqualTo(year).Because($"{chamber} {year} period {name} starts in another year");
                await Assert.That(end.Year).IsEqualTo(year).Because($"{chamber} {year} period {name} ends in another year");

                if (i > 0)
                {
                    await Assert.That(periods[i - 1].end < start).IsTrue().Because($"{chamber} {year} periods overlap or are out of order at {name}");
                }
            }

            // The published calendar only ever schedules sittings Monday to Friday. A period is stored as
            // a plain range, so a weekend inside one would silently become a sitting day.
            foreach (var date in Parliament.GetSittingDays(chamber, year))
            {
                await Assert.That(date.DayOfWeek).IsNotEqualTo(DayOfWeek.Saturday).Because($"{chamber} {date:yyyy-MM-dd} is a Saturday");
                await Assert.That(date.DayOfWeek).IsNotEqualTo(DayOfWeek.Sunday).Because($"{chamber} {date:yyyy-MM-dd} is a Sunday");
            }

            var days = Parliament.GetSittingDays(chamber, year).ToHashSet();
            for (var date = new Date(year, 1, 1); date <= new Date(year, 12, 31); date = date.AddDays(1))
            {
                await Assert.That(date.IsSittingDay(chamber)).IsEqualTo(days.Contains(date)).Because($"{chamber} {date:yyyy-MM-dd} IsSittingDay disagrees with GetSittingDays");
            }
        }
    }

    // Estimates are committee hearings, so the Senate never sits during one.
    [Test]
    public async Task EstimatesNeverClashWithSenateSittings()
    {
        foreach (var year in Parliament.CoveredYears(Chamber.Senate))
        {
            foreach (var (start, end, name) in Parliament.GetSenateEstimates(year))
            {
                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    await Assert.That(date.IsSenateSittingDay()).IsFalse().Because($"{date:yyyy-MM-dd} is both a Senate sitting day and in {name} estimates");
                }
            }
        }
    }

    [Test]
    public async Task UncoveredYearThrows()
    {
        await Assert.That(() => Parliament.GetSittingPeriods(Chamber.House, 1980)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => Parliament.GetSittingDays(Chamber.Senate, 1980)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => Parliament.GetSenateEstimates(1980)).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task UncoveredYearIsNotASittingDay()
    {
        await Assert.That(new Date(1980, 7, 1).IsSittingDay(Chamber.House)).IsFalse();
        await Assert.That(new Date(1980, 7, 1).IsSittingDay()).IsFalse();
        await Assert.That(new Date(1980, 7, 1).IsSenateEstimatesDay()).IsFalse();
    }

    [Test]
    public async Task IsSittingDayUsage()
    {
        #region IsSittingDay

        var date = new Date(2026, 3, 2);

        await Assert.That(date.IsSittingDay(Chamber.House)).IsTrue();

        #endregion
    }

    [Test]
    public async Task IsSittingDayNamedUsage()
    {
        #region IsSittingDayNamed

        var date = new Date(2026, 3, 2);

        await Assert.That(date.IsSittingDay(Chamber.House, out var name)).IsTrue();

        await Assert.That(name).IsEqualTo("Autumn");

        #endregion
    }

    [Test]
    public async Task IsChamberSittingDayUsage()
    {
        #region IsChamberSittingDay

        var date = new Date(2026, 3, 2);

        await Assert.That(date.IsHouseSittingDay()).IsTrue();
        await Assert.That(date.IsSenateSittingDay()).IsTrue();

        #endregion
    }

    [Test]
    public async Task IsBothChambersSittingDayUsage()
    {
        #region IsBothChambersSittingDay

        // 9 to 12 February 2026 is a House sitting week, but the Senate is in estimates.
        await Assert.That(new Date(2026, 2, 9).IsBothChambersSittingDay()).IsFalse();

        await Assert.That(new Date(2026, 3, 2).IsBothChambersSittingDay()).IsTrue();

        #endregion
    }

    [Test]
    public async Task IsSenateEstimatesDayUsage()
    {
        #region IsSenateEstimatesDay

        var date = new Date(2026, 2, 9);

        await Assert.That(date.IsSenateEstimatesDay(out var name)).IsTrue();

        await Assert.That(name).IsEqualTo("Additional");

        #endregion
    }

    [Test]
    public async Task GetSittingPeriodsUsage()
    {
        #region GetSittingPeriods

        var periods = Parliament.GetSittingPeriods(Chamber.House, 2026);
        foreach (var (start, end, name) in periods)
        {
            Console.WriteLine($"{name}: {start} - {end}");
        }

        #endregion

        await Assert.That(periods.Count).IsEqualTo(19);
    }

    [Test]
    public async Task GetSittingDaysUsage()
    {
        #region GetSittingDays

        var days = Parliament.GetSittingDays(Chamber.Senate, 2026);
        foreach (var day in days)
        {
            Console.WriteLine(day);
        }

        #endregion

        await Assert.That(days.Count).IsEqualTo(57);
    }

    static string Format(Date date) =>
        date.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture);

    public static IEnumerable<Chamber> GetChambers() =>
        Enum.GetValues<Chamber>();
}
