using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

[TestFixture]
public class HolidayServiceTests
{
    #region HolidayServiceUsage

    [Test]
    public void Usage()
    {
        var holidayService = new HolidayService(TimeProvider.System);
        var holidays = holidayService.ForYears(startYear: 2026, yearCount: 2);
        foreach (var (date, state, name) in holidays)
        {
            Console.WriteLine($"date: {date}, state: {state}, name: {name}");
        }
    }

    #endregion

    #region DependencyInjectionUsage

    [Test]
    public void DependencyInjectionUsage()
    {
        var services = new ServiceCollection();
        services.AddSingleton<HolidayService>();
        services.AddSingleton(TimeProvider.System);
        services.AddTransient<ClassUsingHolidays>();

        using var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<ClassUsingHolidays>();
        service.WriteHolidays();
    }

    public class ClassUsingHolidays(HolidayService holidayService)
    {
        public void WriteHolidays()
        {
            var holidays = holidayService.ForYears(startYear: 2026, yearCount: 2);
            foreach (var (date, state, name) in holidays)
            {
                Console.WriteLine($"date: {date}, state: {state}, name: {name}");
            }
        }
    }

    #endregion

    // When no start year is supplied the service must resolve "the current year" from its injected
    // TimeProvider, not the process clock, so time can be controlled in tests and via DI.
    [Test]
    public void DefaultYearResolvesViaInjectedTimeProvider()
    {
        var timeProvider = new FakeTimeProvider(new(2031, 6, 15, 0, 0, 0, TimeSpan.Zero));
        var service = new HolidayService(timeProvider);

        var years = service.NationalForYears()
            .Select(_ => _.date.Year)
            .Distinct()
            .ToList();

        AreEqual(1, years.Count);
        AreEqual(2031, years[0]);
    }
}
