public class BunitTestContext : BunitContext
{
    public BunitTestContext()
    {
        var fakeTime = new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero));
        Services.AddSingleton<TimeProvider>(fakeTime);
        Services.AddScoped<HolidayFilterService>();
    }
}
