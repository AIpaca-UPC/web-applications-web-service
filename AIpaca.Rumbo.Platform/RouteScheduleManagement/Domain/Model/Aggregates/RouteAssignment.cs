using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;

/// <summary>
/// Associates a student with a stop belonging to a route.
/// </summary>
public class RouteAssignment
{
    private RouteAssignment()
    {
    }

    public RouteAssignment(int stopId, StudentId studentId)
    {
        if (studentId.Value < 1)
            throw new ArgumentOutOfRangeException(nameof(studentId));

        StudentId = studentId;
        MoveToStop(stopId);
    }

    public int Id { get; private set; }

    public int StopId { get; private set; }

    public StudentId StudentId { get; private set; }

    /// <summary>
    /// Moves the assigned student to another stop,
    /// preserving the assignment identifier.
    /// </summary>
    internal void MoveToStop(int stopId)
    {
        if (stopId < 1)
            throw new ArgumentOutOfRangeException(nameof(stopId));

        StopId = stopId;
    }
}