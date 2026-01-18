var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped(_ =>
    new HttpClient
    {
        BaseAddress = new(builder.HostEnvironment.BaseAddress)
    });
builder.Services.AddScoped<StatePreferenceService>();
builder.Services.AddScoped<ThemePreferenceService>();
builder.Services.AddScoped<HolidayFilterService>();
builder.Services.AddScoped<FileDownloadService>();

await builder.Build().RunAsync();
