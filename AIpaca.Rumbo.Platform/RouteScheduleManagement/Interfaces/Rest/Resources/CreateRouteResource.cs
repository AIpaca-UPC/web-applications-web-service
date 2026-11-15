namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

/// <summary>
/// Request body for creating a route.
/// </summary>
public record CreateRouteResource(
    int DriverProfileId,
    string Name,
    string Shift,
    int? VehicleId,
    IReadOnlyCollection<RouteStopResource>? Stops,
    IReadOnlyCollection<RouteScheduleResource>? Schedule,
    bool ScheduleConfirmed
);