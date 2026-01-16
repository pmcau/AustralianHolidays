using AustralianHolidays.Web.Services;

namespace AustralianHolidays.Web.Tests.Services;

[TestFixture]
public class HolidayFilterServiceTests
{
    [Test]
    public void GetHolidays_WithState_ReturnsHolidaysForState()
    {
        var startDate = new DateOnly(2025, 1, 1);
        var endDate = new DateOnly(2025, 12, 31);

        var holidays = HolidayFilterService.GetHolidays(State.NSW, startDate, endDate);

        That(holidays, Is.Not.Empty);
        That(holidays.All(h => h.State == State.NSW), Is.True);
    }

    [Test]
    public void GetHolidays_WithoutState_ReturnsHolidaysForAllStates()
    {
        var startDate = new DateOnly(2025, 1, 1);
        var endDate = new DateOnly(2025, 12, 31);

        var holidays = HolidayFilterService.GetHolidays(null, startDate, endDate);

        That(holidays, Is.Not.Empty);

        var states = holidays.Select(h => h.State).Distinct().ToList();
        That(states.Count, Is.GreaterThan(1));
    }

    [Test]
    public void GetHolidays_OrderedByDate()
    {
        var startDate = new DateOnly(2025, 1, 1);
        var endDate = new DateOnly(2025, 12, 31);

        var holidays = HolidayFilterService.GetHolidays(State.VIC, startDate, endDate);

        for (var i = 1; i < holidays.Count; i++)
        {
            That(holidays[i].Date, Is.GreaterThanOrEqualTo(holidays[i - 1].Date));
        }
    }

    [Test]
    public void GetHolidays_WithinDateRange()
    {
        var startDate = new DateOnly(2025, 6, 1);
        var endDate = new DateOnly(2025, 6, 30);

        var holidays = HolidayFilterService.GetHolidays(State.QLD, startDate, endDate);

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

        var today = DateOnly.FromDateTime(DateTime.Today);

        // Start should be 7 days ago
        That(start, Is.EqualTo(today.AddDays(-7)));

        // End should be approximately 12 months from today
        That(end, Is.EqualTo(today.AddMonths(12)));
    }

    [Test]
    public void GetExportYearRange_CalculatesCorrectly()
    {
        var startDate = new DateOnly(2025, 6, 1);
        var endDate = new DateOnly(2027, 3, 15);

        var (startYear, yearCount) = HolidayFilterService.GetExportYearRange(startDate, endDate);

        That(startYear, Is.EqualTo(2025));
        That(yearCount, Is.EqualTo(3)); // 2025, 2026, 2027
    }

    [Test]
    public void HolidayViewModel_TimeCategory_Past()
    {
        var pastDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-10));
        var holiday = new HolidayViewModel(pastDate, "Test", State.NSW);

        That(holiday.TimeCategory, Is.EqualTo(HolidayTimeCategory.Past));
    }

    [Test]
    public void HolidayViewModel_TimeCategory_Today()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var holiday = new HolidayViewModel(today, "Test", State.NSW);

        That(holiday.TimeCategory, Is.EqualTo(HolidayTimeCategory.Today));
    }

    [Test]
    public void HolidayViewModel_TimeCategory_Future()
    {
        var futureDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10));
        var holiday = new HolidayViewModel(futureDate, "Test", State.NSW);

        That(holiday.TimeCategory, Is.EqualTo(HolidayTimeCategory.Future));
    }

    [Test]
    public void HolidayViewModel_DayOfWeek_Correct()
    {
        var date = new DateOnly(2025, 1, 1); // Wednesday
        var holiday = new HolidayViewModel(date, "Test", State.NSW);

        That(holiday.DayOfWeek, Is.EqualTo("Wednesday"));
    }
}
