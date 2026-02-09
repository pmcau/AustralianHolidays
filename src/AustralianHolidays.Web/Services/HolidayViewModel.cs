public record HolidayViewModel(Date Date, string Name, IReadOnlyList<State> States, HolidayTimeCategory TimeCategory)
{
    public string DayOfWeek => Date.DayOfWeek.ToString();
}
