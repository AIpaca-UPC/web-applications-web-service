using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to create a configurable route.
/// </summary>
public record CreateRouteCommand(
    int DriverProfileId,
    string Name,
    ERouteShift Shift,
    int? VehicleId,
    IReadOnlyCollection<RouteStop> Stops,
    IReadOnlyCollection<RouteSchedule> Schedule,
    bool ScheduleConfirmed
);