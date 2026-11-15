namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to remove the student assigned
/// to a specific route stop.
/// </summary>
/// <param name="RouteId">Identifier of the route.</param>
/// <param name="StopId">Identifier of the route stop.</param>
public record UnassignStudentFromRouteCommand(
    int RouteId,
    int StopId
);