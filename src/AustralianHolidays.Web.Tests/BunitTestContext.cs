namespace AustralianHolidays.Web.Tests;

public class BunitTestContext : BunitContext
{
    public BunitTestContext() =>
        Services.AddScoped<HolidayFilterService>();
}
