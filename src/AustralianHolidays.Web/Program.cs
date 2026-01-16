using AustralianHolidays.Web;
using AustralianHolidays.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<GeolocationService>();
builder.Services.AddScoped<StatePreferenceService>();
builder.Services.AddScoped<HolidayFilterService>();
builder.Services.AddScoped<FileDownloadService>();

await builder.Build().RunAsync();
