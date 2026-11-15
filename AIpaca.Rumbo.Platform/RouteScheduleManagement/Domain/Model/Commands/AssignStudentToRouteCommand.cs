namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to assign a student to a route stop.
/// </summary>
public record AssignStudentToRouteCommand(
    int RouteId,
    int StopId,
    int StudentId
);