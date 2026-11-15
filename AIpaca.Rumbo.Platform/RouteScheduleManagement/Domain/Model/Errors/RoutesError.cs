namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Errors;

/// <summary>
/// Defines errors reported by the Route Schedule Management context.
/// </summary>
public enum RoutesError
{
    None,
    RouteNotFound,
    RouteAlreadyPublished,
    InvalidRouteData,
    DuplicateScheduleDay,
    StudentAlreadyAssigned,
    StopNotFound,
    StopHasAssignments,
    RouteNotReadyForPublication,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}