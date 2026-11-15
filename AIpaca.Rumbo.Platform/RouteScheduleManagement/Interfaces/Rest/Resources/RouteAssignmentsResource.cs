namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

/// <summary>
/// Request body for assigning a student to a stop.
/// </summary>
public record AssignStudentResource(int StudentId);

/// <summary>
/// Represents one student-to-stop assignment.
/// </summary>
public record RouteAssignmentResource(
    int StopId,
    int StudentId
);

/// <summary>
/// Request body for replacing all assignments of a route.
/// </summary>
public record ReplaceRouteAssignmentsResource(
    IReadOnlyCollection<RouteAssignmentResource> Assignments
);