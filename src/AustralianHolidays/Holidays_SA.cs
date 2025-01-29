namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    ///  Determines if the date is a public holiday in South Australia.
    ///  Reference: https://www.safework.sa.gov.au/resources/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsSaHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsNewYearsDay())
        {
            name = "New Year's Day";
            return true;
        }

        if (date.Month == 1 &&
            date is { DayOfWeek: DayOfWeek.Monday, Day: 27 or 28 })
        {
            name = "Australia Day Holiday";
            return true;
        }

        if (date.IsSecondMonday(Month.March))
        {
            name = "Adelaide Cup Day";
            return true;
        }

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(date.Year);
        if (date == easterFriday)
        {
            name = "Good Friday";
            return true;
        }

        if (date == easterSaturday)
        {
            name = "Easter Saturday";
            return true;
        }

        if (date == easterSunday)
        {
            name = "Easter Sunday";
            return true;
        }

        if (date == easterMonday)
        {
            name = "Easter Monday";
            return true;
        }

        if (date.IsAnzacDay())
        {
            name = "Anzac Day";
            return true;
        }

        if (date.IsMonarchBirthday(out name))
        {
            return true;
        }

        if (date.IsFirstMonday(Month.October))
        {
            name = "Labour Day";
            return true;
        }

        if (date.Month == 12)
        {
            if (date.Day == 26)
            {
                if (date.IsWeekday())
                {
                    name = "Proclamation Day and Boxing Day";
                    return true;
                }

                name = "Proclamation Day";
                return true;
            }

            //When 26 December falls on a Saturday the following Monday is a public holiday
            if (date is { Day: 28, DayOfWeek: DayOfWeek.Monday })
            {
                name = "Proclamation Day (additional)";
                return true;
            }

            //When 26 December falls on a Sunday the following Tuesday is a public holiday
            if (date is { Day: 28, DayOfWeek: DayOfWeek.Tuesday })
            {
                name = "Proclamation Day (additional)";
                return true;
            }
        }

        if (ChristmasCalculator.TryGet(date, out name))
        {
            return true;
        }

        name = null;
        return false;
    }
}