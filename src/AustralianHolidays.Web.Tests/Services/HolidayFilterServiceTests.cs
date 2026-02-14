[TestFixture]
public class HolidayFilterServiceTests
{
    static readonly FakeTimeProvider fakeTime = new(new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero));
    static readonly HolidayFilterService service = new(fakeTime);

    [Test]
    public void GetHolidays_WithSingleState_ReturnsHolidaysForState()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State> { State.NSW };

        var holidays = service.GetHolidays(states, startDate, endDate);

        That(holidays, Is.Not.Empty);
        That(holidays.All(_ => _.States.Contains(State.NSW)), Is.True);
    }

    [Test]
    public void GetHolidays_WithEmptyStates_ReturnsEmptyList()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var holidays = service.GetHolidays(new HashSet<State>(), startDate, endDate);

        That(holidays, Is.Empty);
    }

    [Test]
    public void GetHolidays_WithMultipleStates_ReturnsHolidaysForSelectedStates()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State>
        {
            State.NSW,
            State.VIC
        };

        var holidays = service.GetHolidays(states, startDate, endDate);

        That(holidays, Is.Not.Empty);
        That(holidays.All(_ => _.States.All(_ => _ is State.NSW or State.VIC)), Is.True);
    }

    [Test]
    public void GetHolidays_CombinesSameHolidayAcrossStates()
    {
        var startDate = new Date(2026, 1, 1);
        var endDate = new Date(2026, 1, 31);
        var states = new HashSet<State>
        {
            State.NSW,
            State.VIC
        };

        var holidays = service.GetHolidays(states, startDate, endDate);

        // Australia Day should be combined into one entry with both states
        var australiaDay = holidays.FirstOrDefault(_ => _.Name == "Australia Day");
        That(australiaDay, Is.Not.Null);
        That(australiaDay!.States, Does.Contain(State.NSW));
        That(australiaDay.States, Does.Contain(State.VIC));
    }

    [Test]
    public void GetHolidays_OrderedByDate()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State>
        {
            State.VIC
        };

        var holidays = service.GetHolidays(states, startDate, endDate);

        for (var i = 1; i < holidays.Count; i++)
        {
            That(holidays[i].Date, Is.GreaterThanOrEqualTo(holidays[i - 1].Date));
        }
    }

    [Test]
    public void GetHolidays_WithinDateRange()
    {
        var startDate = new Date(2025, 6, 1);
        var endDate = new Date(2025, 6, 30);
        var states = new HashSet<State>
        {
            State.QLD
        };

        var holidays = service.GetHolidays(states, startDate, endDate);

        foreach (var holiday in holidays)
        {
            That(holiday.Date, Is.GreaterThanOrEqualTo(startDate));
            That(holiday.Date, Is.LessThanOrEqualTo(endDate));
        }
    }

    [Test]
    public void GetDefaultDateRange_ReturnsValidRange()
    {
        var (start, end) = service.GetDefaultDateRange();

        var today = new Date(2026, 1, 15);

        That(start, Is.EqualTo(today.AddDays(-7)));
        That(end, Is.EqualTo(today.AddMonths(12)));
    }

    [Test]
    public void GetExportYearRange_CalculatesCorrectly()
    {
        var startDate = new Date(2025, 6, 1);
        var endDate = new Date(2027, 3, 15);

        var (startYear, yearCount) = HolidayFilterService.GetExportYearRange(startDate, endDate);

        That(startYear, Is.EqualTo(2025));
        That(yearCount, Is.EqualTo(3));
    }

    [Test]
    public void HolidayViewModel_TimeCategory_Past()
    {
        var pastDate = new Date(2026, 1, 5);
        var states = new HashSet<State> { State.NSW };

        var holidays = service.GetHolidays(states, pastDate, pastDate);

        // If there are no holidays on that exact date, test via a broader range
        var allHolidays = service.GetHolidays(states, new Date(2025, 1, 1), new Date(2025, 12, 31));
        That(allHolidays.All(_ => _.TimeCategory == HolidayTimeCategory.Past), Is.True);
    }

    [Test]
    public void HolidayViewModel_TimeCategory_Future()
    {
        var states = new HashSet<State> { State.NSW };

        var holidays = service.GetHolidays(states, new Date(2026, 2, 1), new Date(2026, 12, 31));
        That(holidays.All(_ => _.TimeCategory == HolidayTimeCategory.Future), Is.True);
    }

    [Test]
    public void HolidayViewModel_DayOfWeek_Correct()
    {
        var date = new Date(2025, 1, 1);
        var holiday = new HolidayViewModel(date, "Test", [State.NSW], HolidayTimeCategory.Future);

        That(holiday.DayOfWeek, Is.EqualTo("Wednesday"));
    }
}
