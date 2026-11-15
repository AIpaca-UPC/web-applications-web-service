namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

/// <summary>
/// Request body for updating route information,
/// stops and weekly schedule.
/// </summary>
public record UpdateRouteResource(
    string Name,
    string Shift,
    int? VehicleId,
    IReadOnlyCollection<RouteStopResource>? Stops,
    IReadOnlyCollection<RouteScheduleResource>? Schedule,
    bool ScheduleConfirmed
);