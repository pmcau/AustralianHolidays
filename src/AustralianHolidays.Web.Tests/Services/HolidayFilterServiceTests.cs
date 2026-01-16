namespace AustralianHolidays.Web.Tests.Services;

[TestFixture]
public class HolidayFilterServiceTests
{
    [Test]
    public void GetHolidays_WithSingleState_ReturnsHolidaysForState()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State> { State.NSW };

        var holidays = HolidayFilterService.GetHolidays(states, startDate, endDate);

        That(holidays, Is.Not.Empty);
        That(holidays.All(h => h.State == State.NSW), Is.True);
    }

    [Test]
    public void GetHolidays_WithEmptyStates_ReturnsHolidaysForAllStates()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State>();

        var holidays = HolidayFilterService.GetHolidays(states, startDate, endDate);

        That(holidays, Is.Not.Empty);

        var returnedStates = holidays.Select(h => h.State).Distinct().ToList();
        That(returnedStates.Count, Is.GreaterThan(1));
    }

    [Test]
    public void GetHolidays_WithMultipleStates_ReturnsHolidaysForSelectedStates()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State> { State.NSW, State.VIC };

        var holidays = HolidayFilterService.GetHolidays(states, startDate, endDate);

        That(holidays, Is.Not.Empty);
        That(holidays.All(h => h.State == State.NSW || h.State == State.VIC), Is.True);
    }

    [Test]
    public void GetHolidays_OrderedByDate()
    {
        var startDate = new Date(2025, 1, 1);
        var endDate = new Date(2025, 12, 31);
        var states = new HashSet<State> { State.VIC };

        var holidays = HolidayFilterService.GetHolidays(states, startDate, endDate);

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
        var states = new HashSet<State> { State.QLD };

        var holidays = HolidayFilterService.GetHolidays(states, startDate, endDate);

        foreach (var holiday in holidays)
        {
            That(holiday.Date, Is.GreaterThanOrEqualTo(startDate));
            That(holiday.Date, Is.LessThanOrEqualTo(endDate));
        }
    }

    [Test]
    public void GetDefaultDateRange_ReturnsValidRange()
    {
        var (start, end) = HolidayFilterService.GetDefaultDateRange();

        var today = Date.FromDateTime(DateTime.Today);

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
        var pastDate = Date.FromDateTime(DateTime.Today.AddDays(-10));
        var holiday = new HolidayViewModel(pastDate, "Test", State.NSW);

        That(holiday.TimeCategory, Is.EqualTo(HolidayTimeCategory.Past));
    }

    [Test]
    public void HolidayViewModel_TimeCategory_Today()
    {
        var today = Date.FromDateTime(DateTime.Today);
        var holiday = new HolidayViewModel(today, "Test", State.NSW);

        That(holiday.TimeCategory, Is.EqualTo(HolidayTimeCategory.Today));
    }

    [Test]
    public void HolidayViewModel_TimeCategory_Future()
    {
        var futureDate = Date.FromDateTime(DateTime.Today.AddDays(10));
        var holiday = new HolidayViewModel(futureDate, "Test", State.NSW);

        That(holiday.TimeCategory, Is.EqualTo(HolidayTimeCategory.Future));
    }

    [Test]
    public void HolidayViewModel_DayOfWeek_Correct()
    {
        var date = new Date(2025, 1, 1);
        var holiday = new HolidayViewModel(date, "Test", State.NSW);

        That(holiday.DayOfWeek, Is.EqualTo("Wednesday"));
    }
}
