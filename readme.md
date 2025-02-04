# <img src="/src/icon.png" height="30px"> AustralianHolidays

[![Build status](https://ci.appveyor.com/api/projects/status/8gp77m7yl3n956qa?svg=true)](https://ci.appveyor.com/project/SimonCropp/australianholidays)
[![NuGet Status](https://img.shields.io/nuget/v/AustralianHolidays.svg?label=AustralianHolidays)](https://www.nuget.org/packages/AustralianHolidays/)

.net library retrieving public holiday dates in Australia.

**See [Milestones](../../milestones?state=closed) for release notes.**

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


## NuGet package

 * https://nuget.org/packages/AustralianHolidays/


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
<sup><a href='/src/Tests/Tests.cs#L124-L130' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHoliday' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/Tests.cs#L161-L169' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHolidayNamed' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Is`state`holiday

The same as [IsHoliday](#isholiday) but a convenience wrapper named method is provided for each state.

<!-- snippet: IsHolidayForState -->
<a id='snippet-IsHolidayForState'></a>
```cs
var date = new Date(2024, 12, 25);

IsTrue(date.IsNswHoliday());
```
<sup><a href='/src/Tests/Tests.cs#L149-L155' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHolidayForState' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/Tests.cs#L136-L143' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsHolidayForStateNamed' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/Tests.cs#L43-L51' title='Snippet source file'>snippet source</a> | <a href='#snippet-ForYears' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/Tests.cs#L57-L65' title='Snippet source file'>snippet source</a> | <a href='#snippet-ForYearsState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### NationalForYears

Gets federal holidays that are common for all states.

<!-- snippet: GetHolidays -->
<a id='snippet-GetHolidays'></a>
```cs
var holidays = Holidays.NationalForYears(2025);
foreach (var (date, name) in holidays)
{
    Console.WriteLine($"date: {date}, name: {name}");
}
```
<sup><a href='/src/Tests/Tests.cs#L71-L79' title='Snippet source file'>snippet source</a> | <a href='#snippet-GetHolidays' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### GetHolidays for state

<!-- snippet: GetHolidaysForState -->
<a id='snippet-GetHolidaysForState'></a>
```cs
var holidays = Holidays.GetNswHolidays(2025);
foreach (var (date, name) in holidays)
{
    Console.WriteLine($"date: {date}, name: {name}");
}
```
<sup><a href='/src/Tests/Tests.cs#L85-L93' title='Snippet source file'>snippet source</a> | <a href='#snippet-GetHolidaysForState' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/Tests.cs#L175-L182' title='Snippet source file'>snippet source</a> | <a href='#snippet-IsFederalGovernmentShutdown' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### GetFederalGovernmentShutdown

<!-- snippet: GetFederalGovernmentShutdown -->
<a id='snippet-GetFederalGovernmentShutdown'></a>
```cs
var (start, end) = Holidays.GetFederalGovernmentShutdown(startYear: 2024);

AreEqual(new Date(2024, 12, 25), start);
AreEqual(new Date(2025, 1, 1), end);
```
<sup><a href='/src/Tests/Tests.cs#L111-L118' title='Snippet source file'>snippet source</a> | <a href='#snippet-GetFederalGovernmentShutdown' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## ExportToMarkdown


### Federal

<!-- snippet: ExportToMarkdown -->
<a id='snippet-ExportToMarkdown'></a>
```cs
var md = Holidays.ExportToMarkdown();
```
<sup><a href='/src/Tests/Tests.cs#L25-L27' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToMarkdown' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Common holidays for all states

<!-- include: Tests.ExportToMarkdown.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` |              | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| Anzac Day<br>(additional)         |              | `Mon 27 Apr` |              |              |              | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
<!-- endInclude -->


### State

<!-- snippet: ExportToMarkdownState -->
<a id='snippet-ExportToMarkdownState'></a>
```cs
var md = Holidays.ExportToMarkdown(state);
```
<sup><a href='/src/Tests/Tests.cs#L34-L36' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExportToMarkdownState' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Australian Capital Territory ([Reference](https://www.cmtedd.act.gov.au/communication/holidays))

<!-- include: Tests.ExportToMarkdown_state=ACT.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Canberra Day                      | `Mon 10 Mar` | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` |              | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| Anzac Day<br>(additional)         |              | `Mon 27 Apr` |              |              |              | 
| Reconciliation Day                | `Mon 02 Jun` | `Mon 01 Jun` | `Mon 31 May` | `Mon 29 May` | `Mon 28 May` | 
| King's Birthday                   | `Mon 09 Jun` | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | 
| Labour Day                        | `Mon 06 Oct` | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
<!-- endInclude -->


#### New South Wales ([Reference](https://www.nsw.gov.au/about-nsw/public-holidays))

<!-- include: Tests.ExportToMarkdown_state=NSW.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| New Year's Day<br>(additional)    |              |              |              | `Mon 03 Jan` |              | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| King's Birthday                   | `Mon 09 Jun` | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | 
| Bank Holiday                      | `Mon 04 Aug` | `Mon 03 Aug` | `Mon 02 Aug` | `Mon 07 Aug` | `Mon 06 Aug` | 
| Labour Day                        | `Mon 06 Oct` | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
<!-- endInclude -->


#### Northern Territory ([Reference](https://nt.gov.au/nt-public-holidays))

<!-- include: Tests.ExportToMarkdown_state=NT.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| May Day                           | `Mon 05 May` | `Mon 04 May` | `Mon 03 May` | `Mon 01 May` | `Mon 07 May` | 
| King's Birthday                   | `Mon 09 Jun` | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | 
| Picnic Day                        | `Mon 04 Aug` | `Mon 03 Aug` | `Mon 02 Aug` | `Mon 07 Aug` | `Mon 06 Aug` | 
| Christmas Eve<br>(partial day)    | `Wed 24 Dec` | `Thu 24 Dec` | `Fri 24 Dec` | `Sun 24 Dec` | `Mon 24 Dec` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
| New Year's Eve<br>(partial day)   | `Wed 31 Dec` | `Thu 31 Dec` | `Fri 31 Dec` | `Sun 31 Dec` | `Mon 31 Dec` | 
<!-- endInclude -->


#### Queensland ([Reference](https://www.qld.gov.au/recreation/travel/holidays/public))

<!-- include: Tests.ExportToMarkdown_state=QLD.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| Labour Day                        | `Mon 05 May` | `Mon 04 May` | `Mon 03 May` | `Mon 01 May` | `Mon 07 May` | 
| King's Birthday                   | `Mon 06 Oct` | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | 
| Christmas Eve<br>(partial day)    | `Wed 24 Dec` | `Thu 24 Dec` | `Fri 24 Dec` | `Sun 24 Dec` | `Mon 24 Dec` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
<!-- endInclude -->


#### South Australia ([Reference](https://www.safework.sa.gov.au/resources/public-holidays))

<!-- include: Tests.ExportToMarkdown_state=SA.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Adelaide Cup Day                  | `Mon 10 Mar` | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| King's Birthday                   | `Mon 09 Jun` | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | 
| Labour Day                        | `Mon 06 Oct` | `Mon 05 Oct` | `Mon 04 Oct` | `Mon 02 Oct` | `Mon 01 Oct` | 
| Christmas Eve<br>(partial day)    | `Wed 24 Dec` | `Thu 24 Dec` | `Fri 24 Dec` | `Sun 24 Dec` | `Mon 24 Dec` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Proclamation and Boxing Day       | `Fri 26 Dec` |              |              | `Tue 26 Dec` | `Wed 26 Dec` | 
| Proclamation Day                  |              | `Sat 26 Dec` | `Sun 26 Dec` |              |              | 
| Proclamation Day<br>(additional)  |              | `Mon 28 Dec` | `Mon 27 Dec` |              |              | 
| New Year's Eve<br>(partial day)   | `Wed 31 Dec` | `Thu 31 Dec` | `Fri 31 Dec` | `Sun 31 Dec` | `Mon 31 Dec` | 
<!-- endInclude -->


#### Tasmania ([Reference](https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays))

<!-- include: Tests.ExportToMarkdown_state=TAS.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Eight Hours Day                   | `Mon 10 Mar` | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Easter Tuesday<br>(Government employees only) | `Tue 22 Apr` | `Tue 07 Apr` | `Tue 30 Mar` | `Tue 18 Apr` | `Tue 03 Apr` | 
| Anzac Day                         | `Fri 25 Apr` | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| King's Birthday                   | `Mon 09 Jun` | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
<!-- endInclude -->


#### Victorian ([Reference](https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2025))

<!-- include: Tests.ExportToMarkdown_state=VIC.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Labour Day                        | `Mon 10 Mar` | `Mon 09 Mar` | `Mon 08 Mar` | `Mon 13 Mar` | `Mon 12 Mar` | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| King's Birthday                   | `Mon 09 Jun` | `Mon 08 Jun` | `Mon 14 Jun` | `Mon 12 Jun` | `Mon 11 Jun` | 
| Friday before AFL Grand Final<br>(Subject to AFL schedule) | `Fri 26 Sep` | `Fri 25 Sep` | `Fri 24 Sep` | `Fri 29 Sep` | `Fri 28 Sep` | 
| Melbourne Cup Day                 | `Tue 04 Nov` | `Tue 03 Nov` | `Tue 02 Nov` | `Tue 07 Nov` | `Tue 06 Nov` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
<!-- endInclude -->


#### Western Australia ([Reference](https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia))

<!-- include: Tests.ExportToMarkdown_state=WA.verified.txt -->
|                                   | 2025         | 2026         | 2027         | 2028         | 2029         |
|-----------------------------------|--------------|--------------|--------------|--------------|--------------|
| New Year's Day                    | `Wed 01 Jan` | `Thu 01 Jan` | `Fri 01 Jan` | `Sat 01 Jan` | `Mon 01 Jan` | 
| Australia Day                     |              | `Mon 26 Jan` | `Tue 26 Jan` | `Wed 26 Jan` | `Fri 26 Jan` | 
| Australia Day<br>(additional)     | `Mon 27 Jan` |              |              |              |              | 
| Labour Day                        | `Mon 03 Mar` | `Mon 02 Mar` | `Mon 01 Mar` | `Mon 06 Mar` | `Mon 05 Mar` | 
| Good Friday                       | `Fri 18 Apr` | `Fri 03 Apr` | `Fri 26 Mar` | `Fri 14 Apr` | `Fri 30 Mar` | 
| Easter Saturday                   | `Sat 19 Apr` | `Sat 04 Apr` | `Sat 27 Mar` | `Sat 15 Apr` | `Sat 31 Mar` | 
| Easter Sunday                     | `Sun 20 Apr` | `Sun 05 Apr` | `Sun 28 Mar` | `Sun 16 Apr` | `Sun 01 Apr` | 
| Easter Monday                     | `Mon 21 Apr` | `Mon 06 Apr` | `Mon 29 Mar` | `Mon 17 Apr` | `Mon 02 Apr` | 
| Anzac Day                         | `Fri 25 Apr` | `Sat 25 Apr` | `Sun 25 Apr` | `Tue 25 Apr` | `Wed 25 Apr` | 
| Anzac Day<br>(additional)         |              | `Mon 27 Apr` | `Mon 26 Apr` |              |              | 
| Western Australia Day             | `Mon 02 Jun` | `Mon 01 Jun` | `Mon 07 Jun` | `Mon 05 Jun` | `Mon 04 Jun` | 
| King's Birthday                   | `Mon 29 Sep` | `Mon 28 Sep` | `Mon 27 Sep` | `Mon 25 Sep` | `Mon 24 Sep` | 
| Christmas Day                     | `Thu 25 Dec` | `Fri 25 Dec` | `Sat 25 Dec` | `Mon 25 Dec` | `Tue 25 Dec` | 
| Boxing Day                        | `Fri 26 Dec` | `Sat 26 Dec` | `Sun 26 Dec` | `Tue 26 Dec` | `Wed 26 Dec` | 
| Christmas<br>(additional)         |              | `Mon 28 Dec` | `Mon 27 Dec`<br>`Tue 28 Dec` | `Wed 27 Dec`<br>`Thu 28 Dec` | `Thu 27 Dec`<br>`Fri 28 Dec` | 
<!-- endInclude -->


## Icon

[Australia](https://thenounproject.com/term/australia/1053571/) designed by [Atterratio Aeternus](https://thenounproject.com/Atterratio/) from [The Noun Project](https://thenounproject.com).
