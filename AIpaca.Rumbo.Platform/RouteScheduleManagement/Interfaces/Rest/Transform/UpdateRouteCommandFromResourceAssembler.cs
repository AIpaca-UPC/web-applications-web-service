using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;

/// <summary>
/// Transforms an UpdateRouteResource into an UpdateRouteCommand.
/// </summary>
public static class UpdateRouteCommandFromResourceAssembler
{
    public static UpdateRouteCommand ToCommandFromResource(
        int routeId,
        UpdateRouteResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new UpdateRouteCommand(
            routeId,
            resource.Name,
            RouteWireFormat.ToRouteShift(resource.Shift),
            resource.VehicleId,
            RouteWireFormat.ToStops(resource.Stops),
            RouteWireFormat.ToSchedule(resource.Schedule),
            resource.ScheduleConfirmed);
    }
}