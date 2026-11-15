using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to update an existing route.
/// </summary>
public record UpdateRouteCommand(
    int RouteId,
    string Name,
    ERouteShift Shift,
    int? VehicleId,
    IReadOnlyCollection<RouteStop> Stops,
    IReadOnlyCollection<RouteSchedule> Schedule,
    bool ScheduleConfirmed
);