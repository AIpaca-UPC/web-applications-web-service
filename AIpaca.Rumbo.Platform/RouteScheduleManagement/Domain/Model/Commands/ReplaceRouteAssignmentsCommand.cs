using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to replace the student assignments of a route.
/// </summary>
public record ReplaceRouteAssignmentsCommand(
    int RouteId,
    IReadOnlyCollection<RouteAssignment> Assignments
);