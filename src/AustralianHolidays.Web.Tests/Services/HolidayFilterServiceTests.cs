public class HolidayFilterServiceTests
{
    static readonly FakeTimeProvider fakeTime = new(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero));
    static readonly HolidayFilterService service = new(fakeTime);

    [Test]
    public async Task GetHolidays_WithSingleState_ReturnsHolidaysForState()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State> { State.NSW };

        var holidays = service.GetHolidays(states, startDate, endDate);

        await Assert.That(holidays).IsNotEmpty();
        await Assert.That(holidays.All(_ => _.States.Contains(State.NSW))).IsTrue();
    }

    [Test]
    public async Task GetHolidays_WithEmptyStates_ReturnsEmptyList()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var holidays = service.GetHolidays(new HashSet<State>(), startDate, endDate);

        await Assert.That(holidays).IsEmpty();
    }

    [Test]
    public async Task GetHolidays_WithMultipleStates_ReturnsHolidaysForSelectedStates()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State>
        {
            State.NSW,
            State.VIC
        };

        var holidays = service.GetHolidays(states, startDate, endDate);

        await Assert.That(holidays).IsNotEmpty();
        await Assert.That(holidays.All(_ => _.States.All(_ => _ is State.NSW or State.VIC))).IsTrue();
    }

    [Test]
    public async Task GetHolidays_CombinesSameHolidayAcrossStates()
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
        await Assert.That(australiaDay).IsNotNull();
        await Assert.That(australiaDay!.States).Contains(State.NSW);
        await Assert.That(australiaDay.States).Contains(State.VIC);
    }

    [Test]
    public async Task GetHolidays_OrderedByDate()
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
            await Assert.That(holidays[i].Date).IsGreaterThanOrEqualTo(holidays[i - 1].Date);
        }
    }

    [Test]
    public async Task GetHolidays_WithinDateRange()
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
            await Assert.That(holiday.Date).IsGreaterThanOrEqualTo(startDate);
            await Assert.That(holiday.Date).IsLessThanOrEqualTo(endDate);
        }
    }

    [Test]
    public async Task GetDefaultDateRange_ReturnsValidRange()
    {
        var (start, end) = service.GetDefaultDateRange();

        var today = new Date(2026, 1, 15);

        await Assert.That(start).IsEqualTo(today.AddDays(-7));
        await Assert.That(end).IsEqualTo(today.AddMonths(12));
    }

    [Test]
    public async Task GetExportYearRange_CalculatesCorrectly()
    {
        var startDate = new Date(2025, 6, 1);
        var endDate = new Date(2027, 3, 15);

        var (startYear, yearCount) = HolidayFilterService.GetExportYearRange(startDate, endDate);

        await Assert.That(startYear).IsEqualTo(2025);
        await Assert.That(yearCount).IsEqualTo(3);
    }

    [Test]
    public async Task HolidayViewModel_TimeCategory_Past()
    {
        var states = new HashSet<State> { State.NSW };

        // If there are no holidays on that exact date, test via a broader range
        var allHolidays = service.GetHolidays(states, new(2025, 1, 1), new(2025, 12, 31));
        await Assert.That(allHolidays.All(_ => _.TimeCategory == HolidayTimeCategory.Past)).IsTrue();
    }

    [Test]
    public async Task HolidayViewModel_TimeCategory_Future()
    {
        var states = new HashSet<State> { State.NSW };

        var holidays = service.GetHolidays(states, new(2026, 2, 1), new(2026, 12, 31));
        await Assert.That(holidays.All(_ => _.TimeCategory == HolidayTimeCategory.Future)).IsTrue();
    }

    [Test]
    public async Task HolidayViewModel_DayOfWeek_Correct()
    {
        var date = new Date(2025, 1, 1);
        var holiday = new HolidayViewModel(date, "Test", [State.NSW], HolidayTimeCategory.Future);

        await Assert.That(holiday.DayOfWeek).IsEqualTo("Wednesday");
    }
}
