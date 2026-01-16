# Holiday Comparison Tool

This is a C# console application that uses the AustralianHolidays library to generate a comprehensive comparison report of Australian public holidays across multiple years and states.

## Purpose

This tool generates a detailed report showing how the five key public holidays (and their additional days) are observed across different Australian states and years, with particular focus on:

- New Year's Day (January 1)
- Australia Day (January 26)
- ANZAC Day (April 25)
- Christmas Day (December 25)
- Boxing Day (December 26)

## States Covered

- National (common to all states)
- New South Wales (NSW)
- Western Australia (WA)
- Australian Capital Territory (ACT)
- Queensland (QLD)

## Years Analyzed

The tool currently analyzes these years: 2020, 2021, 2022, 2025, 2026, 2027

## How to Run

From the HolidayComparisonTool directory:

```bash
dotnet run
```

Or from the repository root:

```bash
dotnet run --project HolidayComparisonTool
```

## Output

The tool generates a file named `HolidayComparison.txt` with:

1. **Year-by-year breakdown**: For each year, shows all focus holidays for National and each state, including:
   - Date in YYYY-MM-DD format
   - Day of the week
   - Holiday name (including "additional" day variants)

2. **Summary section**: Provides analysis of additional days by year, showing:
   - What day of the week each base holiday falls on
   - Which states provide additional days
   - Dates and days of the week for all additional holidays

## Key Findings

The report helps identify patterns such as:

- **ANZAC Day (April 25)** additional days:
  - WA provides an additional Monday when ANZAC Day falls on Saturday OR Sunday
  - ACT provides an additional Monday when ANZAC Day falls on Saturday OR Sunday
  - QLD provides an additional Monday only when ANZAC Day falls on Sunday
  - NSW does NOT provide additional days for ANZAC Day

- **New Year's Day (January 1)** additional days:
  - NSW provides an additional Monday when New Year's Day falls on Saturday (Monday Jan 3) or Sunday (Monday Jan 2)
  - Other states do NOT provide additional days

- **Australia Day (January 26)** additional days:
  - ALL states provide an additional Monday when Australia Day falls on a weekend

- **Christmas/Boxing Day** additional days:
  - Most states provide additional weekday holidays when these fall on weekends

## Customization

To modify the tool:

- Edit `Program.cs` to change:
  - `years` array: Add or remove years to analyze
  - `focusHolidays` array: Change which holidays to track
  - `states` array: Add or remove states to analyze
  - `outputFile`: Change the output filename
