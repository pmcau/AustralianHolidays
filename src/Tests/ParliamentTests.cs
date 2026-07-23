[TestFixture]
public class ParliamentTests
{
    [TestCaseSource(nameof(GetChambers))]
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

        return Verify(builder);
    }

    // Guards against transcription errors. Sitting periods are runs of consecutive weekdays that never
    // overlap, and IsSittingDay must agree with them for every day of the year.
    [TestCaseSource(nameof(GetChambers))]
    public void PeriodsAreConsistent(Chamber chamber)
    {
        foreach (var year in Parliament.CoveredYears(chamber))
        {
            var periods = Parliament.GetSittingPeriods(chamber, year);

            for (var i = 0; i < periods.Count; i++)
            {
                var (start, end, name) = periods[i];
                IsTrue(start <= end, $"{chamber} {year} period {name} starts after it ends");
                AreEqual(year, start.Year, $"{chamber} {year} period {name} starts in another year");
                AreEqual(year, end.Year, $"{chamber} {year} period {name} ends in another year");

                if (i > 0)
                {
                    IsTrue(periods[i - 1].end < start, $"{chamber} {year} periods overlap or are out of order at {name}");
                }
            }

            // The published calendar only ever schedules sittings Monday to Friday. A period is stored as
            // a plain range, so a weekend inside one would silently become a sitting day.
            foreach (var date in Parliament.GetSittingDays(chamber, year))
            {
                AreNotEqual(DayOfWeek.Saturday, date.DayOfWeek, $"{chamber} {date:yyyy-MM-dd} is a Saturday");
                AreNotEqual(DayOfWeek.Sunday, date.DayOfWeek, $"{chamber} {date:yyyy-MM-dd} is a Sunday");
            }

            var days = Parliament.GetSittingDays(chamber, year).ToHashSet();
            for (var date = new Date(year, 1, 1); date <= new Date(year, 12, 31); date = date.AddDays(1))
            {
                AreEqual(days.Contains(date), date.IsSittingDay(chamber), $"{chamber} {date:yyyy-MM-dd} IsSittingDay disagrees with GetSittingDays");
            }
        }
    }

    // Estimates are committee hearings, so the Senate never sits during one.
    [Test]
    public void EstimatesNeverClashWithSenateSittings()
    {
        foreach (var year in Parliament.CoveredYears(Chamber.Senate))
        {
            foreach (var (start, end, name) in Parliament.GetSenateEstimates(year))
            {
                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    IsFalse(date.IsSenateSittingDay(), $"{date:yyyy-MM-dd} is both a Senate sitting day and in {name} estimates");
                }
            }
        }
    }

    [Test]
    public void UncoveredYearThrows()
    {
        Throws<ArgumentOutOfRangeException>(() => Parliament.GetSittingPeriods(Chamber.House, 1980));
        Throws<ArgumentOutOfRangeException>(() => Parliament.GetSittingDays(Chamber.Senate, 1980));
        Throws<ArgumentOutOfRangeException>(() => Parliament.GetSenateEstimates(1980));
    }

    [Test]
    public void UncoveredYearIsNotASittingDay()
    {
        IsFalse(new Date(1980, 7, 1).IsSittingDay(Chamber.House));
        IsFalse(new Date(1980, 7, 1).IsSittingDay());
        IsFalse(new Date(1980, 7, 1).IsSenateEstimatesDay());
    }

    [Test]
    public void IsSittingDayUsage()
    {
        #region IsSittingDay

        var date = new Date(2026, 3, 2);

        IsTrue(date.IsSittingDay(Chamber.House));

        #endregion
    }

    [Test]
    public void IsSittingDayNamedUsage()
    {
        #region IsSittingDayNamed

        var date = new Date(2026, 3, 2);

        IsTrue(date.IsSittingDay(Chamber.House, out var name));

        AreEqual("Autumn", name);

        #endregion
    }

    [Test]
    public void IsChamberSittingDayUsage()
    {
        #region IsChamberSittingDay

        var date = new Date(2026, 3, 2);

        IsTrue(date.IsHouseSittingDay());
        IsTrue(date.IsSenateSittingDay());

        #endregion
    }

    [Test]
    public void IsBothChambersSittingDayUsage()
    {
        #region IsBothChambersSittingDay

        // 9 to 12 February 2026 is a House sitting week, but the Senate is in estimates.
        IsFalse(new Date(2026, 2, 9).IsBothChambersSittingDay());

        IsTrue(new Date(2026, 3, 2).IsBothChambersSittingDay());

        #endregion
    }

    [Test]
    public void IsSenateEstimatesDayUsage()
    {
        #region IsSenateEstimatesDay

        var date = new Date(2026, 2, 9);

        IsTrue(date.IsSenateEstimatesDay(out var name));

        AreEqual("Additional", name);

        #endregion
    }

    [Test]
    public void GetSittingPeriodsUsage()
    {
        #region GetSittingPeriods

        var periods = Parliament.GetSittingPeriods(Chamber.House, 2026);
        foreach (var (start, end, name) in periods)
        {
            Console.WriteLine($"{name}: {start} - {end}");
        }

        #endregion

        AreEqual(19, periods.Count);
    }

    [Test]
    public void GetSittingDaysUsage()
    {
        #region GetSittingDays

        var days = Parliament.GetSittingDays(Chamber.Senate, 2026);
        foreach (var day in days)
        {
            Console.WriteLine(day);
        }

        #endregion

        AreEqual(57, days.Count);
    }

    static string Format(Date date) =>
        date.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture);

    public static IEnumerable<Chamber> GetChambers() =>
        Enum.GetValues<Chamber>();
}
