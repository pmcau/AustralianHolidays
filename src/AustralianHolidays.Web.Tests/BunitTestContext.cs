using AustralianHolidays.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AustralianHolidays.Web.Tests;

public class BunitTestContext : Bunit.BunitContext
{
    public BunitTestContext() =>
        Services.AddScoped<HolidayFilterService>();
}
