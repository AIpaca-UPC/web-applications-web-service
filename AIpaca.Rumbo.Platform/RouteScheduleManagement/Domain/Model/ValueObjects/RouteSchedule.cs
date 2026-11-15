namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

/// <summary>
/// Represents the departure time of a route on a specific day.
/// </summary>
public sealed record RouteSchedule
{
    public RouteSchedule(DayOfWeek dayOfWeek, TimeOnly departureTime)
    {
        if (!Enum.IsDefined(dayOfWeek))
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));

        DayOfWeek = dayOfWeek;
        DepartureTime = departureTime;
    }

    public DayOfWeek DayOfWeek { get; }

    public TimeOnly DepartureTime { get; }
}