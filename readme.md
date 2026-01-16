# <img src="/src/icon.png" height="30px"> AustralianHolidays

[![Build status](https://img.shields.io/appveyor/build/SimonCropp/australianholidays)](https://ci.appveyor.com/project/SimonCropp/australianholidays)
[![NuGet Status](https://img.shields.io/nuget/v/AustralianHolidays.svg?label=AustralianHolidays)](https://www.nuget.org/packages/AustralianHolidays/)

.net library retrieving public holiday dates in Australia.

**See [Milestones](../../milestones?state=closed) for release notes.**


## NuGet package

 * https://nuget.org/packages/AustralianHolidays/


<!-- include: holiday-logic. path: /holiday-logic.include.md -->
**The holiday dates provided are based on best efforts to ensure accuracy. However, dates are not guarantee to be correct or up-to-date. Consumers should verify the holiday dates with official sources before relying on them for any critical purposes. The authors and contributors of this project are not responsible for any inaccuracies or any consequences arising from the use of this information.**


## Official sources

 * [Australian Capital Territory](https://www.cmtedd.act.gov.au/communication/holidays)
 * [New South Wales](https://www.nsw.gov.au/about-nsw/public-holidays)
 * [Northern Territory](https://nt.gov.au/nt-public-holidays)
 * [Queensland](https://www.qld.gov.au/recreation/travel/holidays/public)
 * [South Australia](https://www.safework.sa.gov.au/resources/public-holidays)
 * [Tasmania](https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays)
 * [Victoria](https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2025)
 * [Western Australia](https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia)


## Public Holiday Substitution Rules

When public holidays fall on weekends, Australian states and territories have different rules about whether a substitute weekday holiday is provided. This section documents these substitution rules for all major public holidays.


### New Year's Day (January 1)

**New South Wales** - Additional day when on weekend

- When January 1 falls on a weekday → observed on January 1
- When January 1 falls on Saturday → **Both** Saturday (Jan 1) **and** Monday (Jan 3) are public holidays
- When January 1 falls on Sunday → **Both** Sunday (Jan 1) **and** Monday (Jan 2) are public holidays
- **Important**: This is **additional**, not substitution - both the actual January 1 date AND the Monday are public holidays
- **Reference**: [NSW Public Holidays](https://www.nsw.gov.au/about-nsw/public-holidays)

**All Other States**

- New Year's Day is always observed on January 1, regardless of the day of the week
- No substitute holidays are provided when it falls on a weekend


### Australia Day (January 26)

**All States and Territories** - Substitution (Monday replaces weekend day)

- When January 26 falls on a weekday → observed on January 26
- When January 26 falls on Saturday → **Only** Monday (Jan 28) is a public holiday, labeled "Australia Day (observed)"
- When January 26 falls on Sunday → **Only** Monday (Jan 27) is a public holiday, labeled "Australia Day (observed)"
- **Important**: This is **substitution**, not additional - the actual January 26 date is NOT a public holiday when it falls on a weekend, only the Monday substitute counts
- This is consistent across all Australian states and territories

**References:**
- NSW: "there will be no public holiday on that day and instead the following Monday is to be the public holiday" - [NSW Public Holidays](https://www.nsw.gov.au/about-nsw/public-holidays)
- WA: "the first Monday following 26 January is the public holiday" - [WA Public Holidays](https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia)
- QLD: "it shall be observed the following Monday" - [QLD Public Holidays](https://www.qld.gov.au/recreation/travel/holidays/public)

**Historical Weekend Occurrences**:

- **2025**: Sunday - Jan 27 (Mon) was the observed holiday
- **2030**: Saturday (upcoming) - Jan 28 (Mon) will be the observed holiday


### ANZAC Day (April 25)

ANZAC Day is observed on April 25 each year. When April 25 falls on a weekend (Saturday or Sunday), some Australian states and territories provide an **additional** public holiday on the following Monday.

**Important**: The Monday is an "additional" holiday, not a "substitution" - both the actual April 25 **and** the Monday are public holidays. The Monday provides workers with an extra day off since the actual ANZAC Day fell on a non-working day.


#### States with Additional Day Off

**Western Australia** - Both Saturday and Sunday

- When ANZAC Day falls on Saturday (Apr 25) → Monday (Apr 27) is "Anzac Day (additional)"
- When ANZAC Day falls on Sunday (Apr 25) → Monday (Apr 26) is "Anzac Day (additional)"
- **Established**: 1972 via Public and Bank Holidays Act 1972
- **Reference**: [WA Public Holidays](https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia)

**Queensland** - Sunday only

- When ANZAC Day falls on Sunday (Apr 25) → Monday (Apr 26) is "Anzac Day (additional)"
- No substitute when ANZAC Day falls on Saturday
- **Established**: Holidays Act 1983
- **Reference**: [QLD Public Holidays](https://www.qld.gov.au/recreation/travel/holidays/public)

**Australian Capital Territory** - Both Saturday and Sunday

- When ANZAC Day falls on Saturday (Apr 25) → Monday (Apr 27) is "Anzac Day (additional)"
- When ANZAC Day falls on Sunday (Apr 25) → Monday (Apr 26) is "Anzac Day (additional)"
- **Established**: Approximately 2020, confirmed for 2026
- **Reference**: [ACT Public Holidays](https://www.cmtedd.act.gov.au/communication/holidays)

**Northern Territory** - Sunday only

- When ANZAC Day falls on Sunday (Apr 25) → Monday (Apr 26) is "Anzac Day (additional)"
- No substitute when ANZAC Day falls on Saturday
- **Reference**: [NT Public Holidays](https://nt.gov.au/nt-public-holidays)


#### States Without Additional Day

ANZAC Day is always observed on April 25, regardless of the day of the week. No additional Monday holiday is provided when April 25 falls on a weekend.

 * New South Wales - [Reference](https://www.nsw.gov.au/about-nsw/public-holidays)
 * Victoria - [Reference](https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2025)
 * Tasmania - [Reference](https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays)
 * South Australia - [Reference](https://www.safework.sa.gov.au/resources/public-holidays)


**Historical Weekend Occurrences**:

- **2004**: Sunday - QLD, NT, WA provided Monday substitute
- **2009**: Saturday - WA only provided Monday substitute
- **2010**: Sunday - QLD, NT, WA provided Monday substitute
- **2015**: Saturday - WA only provided Monday substitute
- **2020**: Saturday - ACT, WA provided Monday substitute
- **2021**: Sunday - ACT, QLD, NT, WA provided Monday substitute
- **2026**: Saturday (upcoming) - ACT, WA will provide Monday substitute
- **2027**: Sunday (upcoming) - ACT, QLD, NT, WA will provide Monday substitute
- **2032**: Sunday (future) - ACT, QLD, NT, WA will provide Monday substitute


### Christmas Day (December 25) and Boxing Day (December 26)

**Most States** (NSW, VIC, TAS, WA, ACT, QLD, NT)

- Christmas Day (Dec 25) and Boxing Day (Dec 26) are always observed on their actual dates
- When either or both fall on a weekend, additional weekday holidays are provided:
  - If only **one** falls on weekend → One "Christmas (additional)" on the next weekday after Boxing Day
  - If **both** fall on weekend (Sat/Sun) → Two "Christmas (additional)" days on the following Monday and Tuesday
- Examples:
  - **2026**: Christmas Friday, Boxing Day Saturday → Monday Dec 28 is "Christmas (additional)"
  - **2027**: Christmas Saturday, Boxing Day Sunday → Monday Dec 27 and Tuesday Dec 28 are both "Christmas (additional)"

**South Australia** - Special "Proclamation Day" rules

- December 25 is always "Christmas Day"
- December 26 is called "Proclamation Day" (instead of Boxing Day)
- Substitution rules:
  - If Dec 26 is a weekday → "Proclamation and Boxing Day"
  - If Dec 26 is Saturday → "Proclamation Day" + Monday Dec 28 "Proclamation Day (additional)"
  - If Dec 26 is Sunday → "Proclamation Day" + Monday Dec 27 "Proclamation Day (additional)"
- Note: Only one additional day even when Christmas is also on weekend
- **Reference**: [SA Public Holidays](https://www.safework.sa.gov.au/resources/public-holidays)

**Historical Weekend Occurrences**:

- **2021**: Christmas Saturday, Boxing Day Sunday - Two additional days (Mon 27, Tue 28)
- **2022**: Christmas Sunday, Boxing Day Monday - One additional day (Tue 27)
- **2026**: Christmas Friday, Boxing Day Saturday - One additional day (Mon 28)
- **2027**: Christmas Saturday, Boxing Day Sunday - Two additional days (Mon 27, Tue 28)<!-- endInclude -->


## Usage

The main entry point is `AustralianHolidays.Holidays`


### IsHoliday

Determines if a given date is a public holiday in a specified Australian state.

<!-- snippet: IsHoliday -->
<a id='snippet-IsHoliday'></a>
```cs
var date = new Date(2024, 12, 25);

IsTrue(date.IsHoliday(State.NSW));
```
<sup><a href='/src/Tests/Tests.cs#L326-L332' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHoliday' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### With name

Determines if a specific date is a recognized public holiday in a specified state and retrieving the name of the holiday if it is.

<!-- snippet: IsHolidayNamed -->
<a id='snippet-IsHolidayNamed'></a>
```cs
var date = new Date(2024, 12, 25);

IsTrue(date.IsHoliday(State.NSW, out var name));

AreEqual("Christmas Day", name);
```
<sup><a href='/src/Tests/Tests.cs#L363-L371' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHolidayNamed' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Is`state`holiday

The same as [IsHoliday](#isholiday) but a convenience wrapper named method is provided for each state.

<!-- snippet: IsHolidayForState -->
<a id='snippet-IsHolidayForState'></a>
```cs
var date = new Date(2024, 12, 25);

IsTrue(date.IsNswHoliday());
```
<sup><a href='/src/Tests/Tests.cs#L351-L357' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHolidayForState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### With name

The same as [IsHoliday with name](#is-state-holiday) but a convenience wrapper named method is provided for each state.

<!-- snippet: IsHolidayForStateNamed -->
<a id='snippet-IsHolidayForStateNamed'></a>
```cs
var date = new Date(2024, 12, 25);

IsTrue(date.IsNswHoliday(out var name));
AreEqual("Christmas Day", name);
```
<sup><a href='/src/Tests/Tests.cs#L338-L345' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHolidayForStateNamed' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### ForYears

Retrieves public holidays for all states over a specified range of years.

<!-- snippet: ForYears -->
<a id='snippet-ForYears'></a>
```cs
var holidays = Holidays.ForYears(startYear: 2025, yearCount: 2);
foreach (var (date, state, name) in holidays)
{
    Console.WriteLine($"date: {date}, state: {state}, name: {name}");
}
```
<sup><a href='/src/Tests/Tests.cs#L245-L253' title='Snippet source file'>snippet source</a> | <a href='#snippet-ForYears' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### For single state

Retrieves public holidays for a specified state over a given range of years.

<!-- snippet: ForYearsState -->
<a id='snippet-ForYearsState'></a>
```cs
var holidays = Holidays.ForYears(State.NSW, startYear: 2025, yearCount: 2);
foreach (var (date, name) in holidays)
{
    Console.WriteLine($"date: {date}, name: {name}");
}
```
<sup><a href='/src/Tests/Tests.cs#L259-L267' title='Snippet source file'>snippet source</a> | <a href='#snippet-ForYearsState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### NationalForYears

Gets federal holidays that are common for all states.

<!-- snippet: ForNational -->
<a id='snippet-ForNational'></a>
```cs
var holidays = Holidays.ForNational(2025);
foreach (var (date, name) in holidays)
{
    Console.WriteLine($"date: {date}, name: {name}");
}
```
<sup><a href='/src/Tests/Tests.cs#L273-L281' title='Snippet source file'>snippet source</a> | <a href='#snippet-ForNational' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Holidays For State

<!-- snippet: ForState -->
<a id='snippet-ForState'></a>
```cs
var holidays = Holidays.ForNsw(2025);
foreach (var (date, name) in holidays)
{
    Console.WriteLine($"date: {date}, name: {name}");
}
```
<sup><a href='/src/Tests/Tests.cs#L287-L295' title='Snippet source file'>snippet source</a> | <a href='#snippet-ForState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Federal government shutdown


#### IsFederalGovernmentShutdown

<!-- snippet: IsFederalGovernmentShutdown -->
<a id='snippet-IsFederalGovernmentShutdown'></a>
```cs
var date = new Date(2025, 12, 30);
var result = date.IsFederalGovernmentShutdown();

IsTrue(result);
```
<sup><a href='/src/Tests/Tests.cs#L377-L384' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsFederalGovernmentShutdown' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### GetFederalGovernmentShutdown

<!-- snippet: GetFederalGovernmentShutdown -->
<a id='snippet-GetFederalGovernmentShutdown'></a>
```cs
var (start, end) = Holidays.GetFederalGovernmentShutdown(startYear: 2024);

AreEqual(new Date(2024, 12, 25), start);
AreEqual(new Date(2025, 1, 1), end);
```
<sup><a href='/src/Tests/Tests.cs#L313-L320' title='Snippet source file'>snippet source</a> | <a href='#snippet-GetFederalGovernmentShutdown' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## ExportToMarkdown


### Federal

<!-- snippet: ExportToMarkdown -->
<a id='snippet-ExportToMarkdown'></a>
```cs
var md = await Holidays.ExportToMarkdown();
```
<sup><a href='/src/Tests/Tests.cs#L25-L29' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToMarkdown' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Common holidays for all states

<!-- include: Tests.ExportToMarkdown.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| Anzac Day<br>(additional)         | `Mon 27 Apr` |              |              |              |              | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
<!-- endInclude -->


### State

<!-- snippet: ExportToMarkdownState -->
<a id='snippet-ExportToMarkdownState'></a>
```cs
var md = await Holidays.ExportToMarkdown(state);
```
<sup><a href='/src/Tests/Tests.cs#L37-L41' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToMarkdownState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Australian Capital Territory ([Reference](https://www.cmtedd.act.gov.au/communication/holidays))

<!-- include: Tests.ExportToMarkdown_state=ACT.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(not holiday)    |              |              |              |              | `Sat 26 Jan` | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Canberra Day                      | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | `Mon 11 Mar` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| Anzac Day<br>(additional)         | `Mon 27 Apr` | `Mon 26 Apr` |              |              |              | 
| Reconciliation Day                | `Mon 01 Jun` | `Mon 31 May` | `Mon 29 May` | `Mon 28 May` | `Mon 27 May` | 
| King's Birthday                   | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | `Mon 10 Jun` | 
| Labour Day                        | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | `Mon 07 Oct` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
<!-- endInclude -->


#### New South Wales ([Reference](https://www.nsw.gov.au/about-nsw/public-holidays))

<!-- include: Tests.ExportToMarkdown_state=NSW.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| New Year's Day<br>(additional)    |              |              | `Mon 03 Jan` |              |              | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| King's Birthday                   | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | `Mon 10 Jun` | 
| Bank Holiday                      | `Mon 03 Aug` | `Mon 02 Aug` | `Mon 07 Aug` | `Mon 06 Aug` | `Mon 05 Aug` | 
| Labour Day                        | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | `Mon 07 Oct` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
<!-- endInclude -->


#### Northern Territory ([Reference](https://nt.gov.au/nt-public-holidays))

<!-- include: Tests.ExportToMarkdown_state=NT.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| Anzac Day<br>(additional)         |              | `Mon 26 Apr` |              |              |              | 
| May Day                           | `Mon 04 May` | `Mon 03 May` | `Mon 01 May` | `Mon 07 May` | `Mon 06 May` | 
| King's Birthday                   | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | `Mon 10 Jun` | 
| Picnic Day                        | `Mon 03 Aug` | `Mon 02 Aug` | `Mon 07 Aug` | `Mon 06 Aug` | `Mon 05 Aug` | 
| Christmas Eve<br>(partial day)    | `Thu 24 Dec` | `Fri 24 Dec` | `Sun 24 Dec` | `Mon 24 Dec` | `Tue 24 Dec` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
| New Year's Eve<br>(partial day)   | `Thu 31 Dec` | `Fri 31 Dec` | `Sun 31 Dec` | `Mon 31 Dec` | `Tue 31 Dec` | 
<!-- endInclude -->


#### Queensland ([Reference](https://www.qld.gov.au/recreation/travel/holidays/public))

<!-- include: Tests.ExportToMarkdown_state=QLD.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| Anzac Day<br>(additional)         |              | `Mon 26 Apr` |              |              |              | 
| Labour Day                        | `Mon 04 May` | `Mon 03 May` | `Mon 01 May` | `Mon 07 May` | `Mon 06 May` | 
| King's Birthday                   | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | `Mon 07 Oct` | 
| Christmas Eve<br>(partial day)    | `Thu 24 Dec` | `Fri 24 Dec` | `Sun 24 Dec` | `Mon 24 Dec` | `Tue 24 Dec` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
<!-- endInclude -->


#### South Australia ([Reference](https://www.safework.sa.gov.au/resources/public-holidays))

<!-- include: Tests.ExportToMarkdown_state=SA.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Adelaide Cup Day                  | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | `Mon 11 Mar` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| King's Birthday                   | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | `Mon 10 Jun` | 
| Labour Day                        | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | `Mon 07 Oct` | 
| Christmas Eve<br>(partial day)    | `Thu 24 Dec` | `Fri 24 Dec` | `Sun 24 Dec` | `Mon 24 Dec` | `Tue 24 Dec` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Proclamation Day                  | `Sat 26 Dec` | `Sun 26 Dec` |              |              |              | 
| Proclamation and Boxing Day       |              |              | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Proclamation Day<br>(additional)  | `Mon 28 Dec` | `Mon 27 Dec` |              |              |              | 
| New Year's Eve<br>(partial day)   | `Thu 31 Dec` | `Fri 31 Dec` | `Sun 31 Dec` | `Mon 31 Dec` | `Tue 31 Dec` | 
<!-- endInclude -->


#### Tasmania ([Reference](https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays))

<!-- include: Tests.ExportToMarkdown_state=TAS.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Eight Hours Day                   | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | `Mon 11 Mar` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Easter Tuesday<br>(Government employees only) | `Tue 07 Apr` | `Tue 30 Mar` | `Tue 18 Apr` | `Tue 03 Apr` | `Tue 23 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| King's Birthday                   | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | `Mon 10 Jun` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
<!-- endInclude -->


#### Victorian ([Reference](https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2025))

<!-- include: Tests.ExportToMarkdown_state=VIC.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Labour Day                        | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | `Mon 11 Mar` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| King's Birthday                   | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | `Mon 10 Jun` | 
| Friday before AFL Grand Final<br>(Subject to AFL schedule) | `Fri 25 Sep` | `Fri 24 Sep` | `Fri 29 Sep` | `Fri 28 Sep` | `Fri 27 Sep` | 
| Melbourne Cup Day                 | `Tue 03 Nov` | `Tue 02 Nov` | `Tue 07 Nov` | `Tue 06 Nov` | `Tue 05 Nov` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
<!-- endInclude -->


#### Western Australia ([Reference](https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia))

<!-- include: Tests.ExportToMarkdown_state=WA.verified.md -->
|                                   | 2026         | 2027         | 2028         | 2029         | 2030         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | `Tue 01 Jan` | 
| Australia Day                     | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` |              | 
| Australia Day<br>(observed)       |              |              |              |              | `Mon 28 Jan` | 
| Labour Day                        | `Mon 02 Mar` | `Mon 01 Mar` | `Mon 06 Mar` | `Mon 05 Mar` | `Mon 04 Mar` | 
| Good Friday                       | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | `Fri 19 Apr` | 
| Easter Saturday                   | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | `Sat 20 Apr` | 
| Easter Sunday                     | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | `Sun 21 Apr` | 
| Easter Monday                     | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | `Mon 22 Apr` | 
| Anzac Day                         | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | `Thu 25 Apr` | 
| Anzac Day<br>(additional)         | `Mon 27 Apr` | `Mon 26 Apr` |              |              |              | 
| Western Australia Day             | `Mon 01 Jun` | `Mon 07 Jun` | `Mon 05 Jun` | `Mon 04 Jun` | `Mon 03 Jun` | 
| King's Birthday                   | `Mon 28 Sep` | `Mon 27 Sep` | `Mon 25 Sep` | `Mon 24 Sep` | `Mon 23 Sep` | 
| Christmas Day                     | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | `Wed 25 Dec` | 
| Boxing Day                        | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | `Thu 26 Dec` | 
| Christmas<br>(additional)         | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` |              |              |              | 
<!-- endInclude -->


## ExportToIcs

Export holidays to ICS (iCalendar) format for importing into calendar applications.


### National

<!-- snippet: ExportToIcs -->
<a id='snippet-ExportToIcs'></a>
```cs
var ics = await Holidays.ExportToIcs();
```
<sup><a href='/src/Tests/Tests.cs#L49-L53' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToIcs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### State

<!-- snippet: ExportToIcsState -->
<a id='snippet-ExportToIcsState'></a>
```cs
var ics = await Holidays.ExportToIcs(state);
```
<sup><a href='/src/Tests/Tests.cs#L61-L65' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToIcsState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## ExportToJson

Export holidays to JSON format.


### National

<!-- snippet: ExportToJson -->
<a id='snippet-ExportToJson'></a>
```cs
var json = await Holidays.ExportToJson();
```
<sup><a href='/src/Tests/Tests.cs#L73-L77' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToJson' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### State

<!-- snippet: ExportToJsonState -->
<a id='snippet-ExportToJsonState'></a>
```cs
var json = await Holidays.ExportToJson(state);
```
<sup><a href='/src/Tests/Tests.cs#L85-L89' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToJsonState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## ExportToXml

Export holidays to XML format.


### National

<!-- snippet: ExportToXml -->
<a id='snippet-ExportToXml'></a>
```cs
var xml = await Holidays.ExportToXml();
```
<sup><a href='/src/Tests/Tests.cs#L97-L101' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToXml' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### State

<!-- snippet: ExportToXmlState -->
<a id='snippet-ExportToXmlState'></a>
```cs
var xml = await Holidays.ExportToXml(state);
```
<sup><a href='/src/Tests/Tests.cs#L109-L113' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToXmlState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## ExportToCsv

Export holidays to CSV format.


### National

<!-- snippet: ExportToCsv -->
<a id='snippet-ExportToCsv'></a>
```cs
var csv = await Holidays.ExportToCsv();
```
<sup><a href='/src/Tests/Tests.cs#L121-L125' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToCsv' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### State

<!-- snippet: ExportToCsvState -->
<a id='snippet-ExportToCsvState'></a>
```cs
var csv = await Holidays.ExportToCsv(state);
```
<sup><a href='/src/Tests/Tests.cs#L133-L137' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToCsvState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## ExportToExcel

Export holidays to Excel format (.xlsx).


### National

<!-- snippet: ExportToExcel -->
<a id='snippet-ExportToExcel'></a>
```cs
var bytes = await Holidays.ExportToExcel();
```
<sup><a href='/src/Tests/Tests.cs#L145-L149' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToExcel' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### State

<!-- snippet: ExportToExcelState -->
<a id='snippet-ExportToExcelState'></a>
```cs
var bytes = await Holidays.ExportToExcel(state);
```
<sup><a href='/src/Tests/Tests.cs#L158-L162' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToExcelState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Dependency Injection API

The above APIs are all static. This means they are not Dependency Injection or test friendly.

`HolidayService` is an instance based wrapper for the above APIs that can be used in Dependency Injection and is test friendly.

All members are virtual so it can be mocked.


### Usage 

<!-- snippet: HolidayServiceUsage -->
<a id='snippet-HolidayServiceUsage'></a>
```cs
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
```
<sup><a href='/src/Tests/HolidayServiceTests.cs#L6-L19' title='Snippet source file'>snippet source</a> | <a href='#snippet-HolidayServiceUsage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Dependency Injection Usage

<!-- snippet: DependencyInjectionUsage -->
<a id='snippet-DependencyInjectionUsage'></a>
```cs
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
```
<sup><a href='/src/Tests/HolidayServiceTests.cs#L21-L48' title='Snippet source file'>snippet source</a> | <a href='#snippet-DependencyInjectionUsage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Testing


#### AlwaysHolidayService

`AlwaysHolidayService` treats every day as a holiday

<!-- snippet: AlwaysHolidayServiceUsage -->
<a id='snippet-AlwaysHolidayServiceUsage'></a>
```cs
[Test]
public void AlwaysHolidayServiceUsage()
{
    var service = new AlwaysHolidayService();
    var result = service.ForYears(2023, 1).ToList();

    AreEqual(8 * 365, result.Count); // 8 states * 365 days
    IsTrue(result.All(item => item.name == "Holiday"));
}
```
<sup><a href='/src/Tests/AlwaysHolidayServiceTests.cs#L4-L16' title='Snippet source file'>snippet source</a> | <a href='#snippet-AlwaysHolidayServiceUsage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### NeverHolidayService

`NeverHolidayService` treats every day as not a holiday

<!-- snippet: NeverHolidayServiceUsage -->
<a id='snippet-NeverHolidayServiceUsage'></a>
```cs
[Test]
public void NeverHolidayServiceUsage()
{
    var service = new NeverHolidayService();
    var result = service.ForYears(2023, 1).ToList();

    IsEmpty(result);

    var date = new Date(2020, 1, 2);
    IsFalse(service.IsNswHoliday(date));
}
```
<sup><a href='/src/Tests/NeverHolidayServiceTests.cs#L4-L18' title='Snippet source file'>snippet source</a> | <a href='#snippet-NeverHolidayServiceUsage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Australia](https://thenounproject.com/term/australia/1053571/) designed by [Atterratio Aeternus](https://thenounproject.com/Atterratio/) from [The Noun Project](https://thenounproject.com).
