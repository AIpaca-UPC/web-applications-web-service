namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

/// <summary>
/// Request body for updating only a route's weekly schedule.
/// </summary>
public record UpdateRouteScheduleResource(
    IReadOnlyCollection<RouteScheduleResource> Schedule,
    bool Confirm
);