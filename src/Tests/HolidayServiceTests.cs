using Microsoft.Extensions.DependencyInjection;

[TestFixture]
public class HolidayServiceTests
{
    #region HolidayServiceUsage

    [Test]
    public void Usage()
    {
        var holidayService = new HolidayService(TimeProvider.System);
        var holidays = holidayService.ForYears(startYear: 2025, yearCount: 2);
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
            var holidays = holidayService.ForYears(startYear: 2025, yearCount: 2);
            foreach (var (date, state, name) in holidays)
            {
                Console.WriteLine($"date: {date}, state: {state}, name: {name}");
            }
        }
    }

    #endregion
}