using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to update the weekly route schedule.
/// </summary>
public record UpdateRouteScheduleCommand(
    int RouteId,
    IReadOnlyCollection<RouteSchedule> Schedule,
    bool Confirm
);