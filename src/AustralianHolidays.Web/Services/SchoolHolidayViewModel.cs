public record SchoolHolidayViewModel(
    State State,
    int Year,
    string Name,
    Date Start,
    Date End,
    HolidayTimeCategory TimeCategory)
{
    public int Days => End.DayNumber - Start.DayNumber + 1;

    public int Weekdays
    {
        get
        {
            var count = 0;
            for (var date = Start; date <= End; date = date.AddDays(1))
            {
                if (date.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
