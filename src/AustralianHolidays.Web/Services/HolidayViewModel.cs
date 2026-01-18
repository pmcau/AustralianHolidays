public record HolidayViewModel(Date Date, string Name, IReadOnlyList<State> States)
{
    public string DayOfWeek => Date.DayOfWeek.ToString();

    public HolidayTimeCategory TimeCategory
    {
        get
        {
            var today = Date.FromDateTime(DateTime.Today);
            if (Date < today)
            {
                return HolidayTimeCategory.Past;
            }

            if (Date == today)
            {
                return HolidayTimeCategory.Today;
            }

            return HolidayTimeCategory.Future;
        }
    }
}
