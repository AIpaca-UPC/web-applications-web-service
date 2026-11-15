namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

/// <summary>
/// Represents an ordered route stop in the API.
/// StudentId is null when no student is assigned.
/// </summary>
public record RouteStopResource(
    int? Id,
    int Position,
    string Address,
    int? StudentId
);

/// <summary>
/// Represents a service day and its departure time.
/// Day uses names such as MONDAY or TUESDAY.
/// </summary>
public record RouteScheduleResource(
    string Day,
    string DepartureTime
);

/// <summary>
/// Represents a route returned by the REST API.
/// </summary>
public record RouteResource(
    int Id,
    string Code,
    string Name,
    int DriverProfileId,
    int? VehicleId,
    string Shift,
    string Status,
    string? DepartureTime,
    bool ScheduleConfirmed,
    IReadOnlyCollection<RouteStopResource> Stops,
    IReadOnlyCollection<RouteScheduleResource> Schedule
);