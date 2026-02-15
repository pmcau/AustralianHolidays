# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

.NET library for calculating Australian public holidays across all 8 states/territories (ACT, NSW, NT, QLD, SA, TAS, VIC, WA). Includes a Blazor WebAssembly website. Published as the `AustralianHolidays` NuGet package.

## Build & Test

Requires .NET SDK 10.0+ (see `src/global.json`). All commands run from the repo root:

```bash
dotnet build src --configuration Release
dotnet test src --configuration Release --no-build --no-restore --filter Category!=Integration
```

To run a single test:
```bash
dotnet test src/Tests --filter "FullyQualifiedName~TestMethodName"
```

Tests use **NUnit** with **Verify** for snapshot testing. When holiday logic changes, snapshot files (`.verified.txt`, `.verified.md`, `.verified.csv`, etc.) need updating. Failed snapshots produce `.received.*` files — use a diff tool or accept them to update baselines.

## Architecture

### Core Library (`src/AustralianHolidays/`)

The `Holidays` class is a **static partial class** split across files by concern:

- **`Holidays.cs`** — Main entry point, caching (ConcurrentDictionary → FrozenDictionary), dispatch logic
- **`Holidays_National.cs`** — Federal holidays common to all states
- **`Holidays_{State}.cs`** — Per-state holiday rules (8 files: ACT, NSW, NT, QLD, SA, TAS, VIC, WA)
- **`Holidays_{Format}Export.cs`** — Export to Markdown, ICS, JSON, XML, CSV, Excel
- **`Holidays_GovShutdown.cs`** — Federal government shutdown period calculation

Holiday calculators:
- **`EasterCalculator.cs`** — Computus algorithm for Easter Sunday
- **`ChristmasCalculator.cs`** — Christmas/Boxing Day weekend substitution logic
- **`AnzacDayCalculator.cs`** — ANZAC Day (April 25)
- **`MonarchBirthdayCalculator.cs`** — King's Birthday (varies by state)
- **`Extensions.cs`** — Date helpers: `GetFirstMonday`, `GetSecondMonday`, etc.

`HolidayService` wraps the static API for DI/testing. `AlwaysHolidayService` and `NeverHolidayService` are test doubles.

### Blazor Website (`src/AustralianHolidays.Web/`)

Blazor WASM app deployed to GitHub Pages. Components in `Components/`, pages in `Pages/`, services in `Services/`.

### Key Patterns

- Holiday rules vary significantly by state — each state has unique substitution logic for weekends
- Cache is populated in the static constructor for current year ± range, using FrozenDictionary for immutable read-only access
- `Date` (DateOnly alias) is used throughout, not DateTime
- The library targets net8.0, net9.0, and net10.0
- `TreatWarningsAsErrors` is enabled; `LangVersion` is `preview`
- Assembly is strong-named (`key.snk`); tests use `InternalsVisibleTo`
- MarkdownSnippets.MsBuild auto-generates README code samples from test snippets
