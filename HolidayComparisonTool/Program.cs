global using Date = System.DateOnly;

using AustralianHolidays;
using System.Text;

// Configuration
var years = new[] { 2020, 2021, 2022, 2025, 2026, 2027 };
var outputFile = "HolidayComparison.txt";

// Focus holidays (will filter to these plus their "additional" variants)
var focusHolidays = new[]
{
    "New Year's Day",
    "Australia Day",
    "Anzac Day",
    "Christmas Day",
    "Boxing Day"
};

var output = new StringBuilder();
output.AppendLine("Australian Public Holidays Comparison");
output.AppendLine("=====================================");
output.AppendLine();
output.AppendLine($"Years: {string.Join(", ", years)}");
output.AppendLine($"Focus Holidays: {string.Join(", ", focusHolidays)}");
output.AppendLine();
output.AppendLine("Note: Includes base holidays and their 'additional' day variants");
output.AppendLine();

// Process each year
foreach (var year in years)
{
    output.AppendLine($"═══════════════════════════════════════════════════════════════");
    output.AppendLine($"YEAR {year}");
    output.AppendLine($"═══════════════════════════════════════════════════════════════");
    output.AppendLine();

    // Process National holidays
    output.AppendLine("NATIONAL (Common to all states)");
    output.AppendLine("─────────────────────────────────────────────────────────────");
    var nationalHolidays = Holidays.ForNational(year)
        .Where(h => focusHolidays.Any(f => h.Value.StartsWith(f)))
        .OrderBy(h => h.Key);

    foreach (var (date, name) in nationalHolidays)
    {
        output.AppendLine($"{date:yyyy-MM-dd} ({date.DayOfWeek,-9}) - {name}");
    }
    output.AppendLine();

    // Process each state
    var states = new[]
    {
        (State.NSW, "NEW SOUTH WALES"),
        (State.WA, "WESTERN AUSTRALIA"),
        (State.ACT, "AUSTRALIAN CAPITAL TERRITORY"),
        (State.QLD, "QUEENSLAND")
    };

    foreach (var (state, stateName) in states)
    {
        output.AppendLine(stateName);
        output.AppendLine("─────────────────────────────────────────────────────────────");

        var stateHolidays = Holidays.ForYears(state, year, 1)
            .Where(h => focusHolidays.Any(f => h.name.StartsWith(f)))
            .OrderBy(h => h.date);

        if (!stateHolidays.Any())
        {
            output.AppendLine("  No matching holidays found");
        }
        else
        {
            foreach (var (date, name) in stateHolidays)
            {
                output.AppendLine($"{date:yyyy-MM-dd} ({date.DayOfWeek,-9}) - {name}");
            }
        }
        output.AppendLine();
    }

    output.AppendLine();
}

// Add summary section
output.AppendLine("═══════════════════════════════════════════════════════════════");
output.AppendLine("SUMMARY - Additional Days Analysis");
output.AppendLine("═══════════════════════════════════════════════════════════════");
output.AppendLine();

foreach (var year in years)
{
    output.AppendLine($"Year {year}:");

    // Check what day key holidays fall on
    var newYearsDay = new Date(year, 1, 1);
    var australiaDay = new Date(year, 1, 26);
    var anzacDay = new Date(year, 4, 25);
    var christmasDay = new Date(year, 12, 25);
    var boxingDay = new Date(year, 12, 26);

    output.AppendLine($"  New Year's Day (Jan 1):  {newYearsDay.DayOfWeek}");
    output.AppendLine($"  Australia Day (Jan 26):  {australiaDay.DayOfWeek}");
    output.AppendLine($"  ANZAC Day (Apr 25):      {anzacDay.DayOfWeek}");
    output.AppendLine($"  Christmas Day (Dec 25):  {christmasDay.DayOfWeek}");
    output.AppendLine($"  Boxing Day (Dec 26):     {boxingDay.DayOfWeek}");

    // Check for additional days
    var hasAdditionalDays = false;

    // Check National
    var nationalHols = Holidays.ForNational(year).ToList();
    var nationalAdditional = nationalHols.Where(h => h.Value.Contains("(additional)")).ToList();
    if (nationalAdditional.Count > 0)
    {
        output.AppendLine($"  National Additional Days:");
        foreach (var (date, name) in nationalAdditional)
        {
            output.AppendLine($"    - {name}: {date:yyyy-MM-dd} ({date.DayOfWeek})");
        }
        hasAdditionalDays = true;
    }

    // Check each state for additional days
    var stateList = new[] { State.NSW, State.WA, State.ACT, State.QLD };
    foreach (var state in stateList)
    {
        var stateHols = Holidays.ForYears(state, year, 1).ToList();
        var stateAdditional = stateHols
            .Where(h => focusHolidays.Any(f => h.name.StartsWith(f)) && h.name.Contains("(additional)"))
            .ToList();

        if (stateAdditional.Count > 0)
        {
            output.AppendLine($"  {state} Additional Days:");
            foreach (var (date, name) in stateAdditional)
            {
                output.AppendLine($"    - {name}: {date:yyyy-MM-dd} ({date.DayOfWeek})");
            }
            hasAdditionalDays = true;
        }
    }

    if (!hasAdditionalDays)
    {
        output.AppendLine($"  No additional days for focus holidays");
    }

    output.AppendLine();
}

// Write to file
var fullPath = Path.Combine(Directory.GetCurrentDirectory(), outputFile);
File.WriteAllText(fullPath, output.ToString());

Console.WriteLine($"Holiday comparison report generated successfully!");
Console.WriteLine($"Output saved to: {fullPath}");
Console.WriteLine();
Console.WriteLine("Preview (first 50 lines):");
Console.WriteLine();

var lines = output.ToString().Split('\n');
for (int i = 0; i < Math.Min(50, lines.Length); i++)
{
    Console.WriteLine(lines[i]);
}

if (lines.Length > 50)
{
    Console.WriteLine("... (output truncated, see file for complete report)");
}
