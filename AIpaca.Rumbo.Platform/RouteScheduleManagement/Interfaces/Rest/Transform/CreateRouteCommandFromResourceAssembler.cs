using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;

/// <summary>
/// Transforms a CreateRouteResource into a CreateRouteCommand.
/// </summary>
public static class CreateRouteCommandFromResourceAssembler
{
    public static CreateRouteCommand ToCommandFromResource(
        CreateRouteResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new CreateRouteCommand(
            resource.DriverProfileId,
            resource.Name,
            RouteWireFormat.ToRouteShift(resource.Shift),
            resource.VehicleId,
            RouteWireFormat.ToStops(resource.Stops),
            RouteWireFormat.ToSchedule(resource.Schedule),
            resource.ScheduleConfirmed);
    }
}