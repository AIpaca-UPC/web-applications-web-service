using System.Globalization;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;

/// <summary>
/// Transforms a Route aggregate into the resource
/// returned by the REST API.
/// </summary>
public static class RouteResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a route entity into its public API representation.
    /// </summary>
    public static RouteResource ToResourceFromEntity(Route entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var stops = entity.Stops
            .OrderBy(stop => stop.Position)
            .Select(stop =>
            {
                var assignment = entity.Assignments
                    .FirstOrDefault(item => item.StopId == stop.Id);

                return new RouteStopResource(
                    stop.Id,
                    stop.Position,
                    stop.Address,
                    assignment?.StudentId.Value);
            })
            .ToList();

        var schedule = entity.Schedule
            .Select(item => new RouteScheduleResource(
                item.DayOfWeek.ToString().ToUpperInvariant(),
                item.DepartureTime.ToString(
                    "HH:mm:ss",
                    CultureInfo.InvariantCulture)))
            .ToList();

        return new RouteResource(
            entity.Id.Value,
            entity.Code,
            entity.Name,
            entity.DriverProfileId.Value,
            entity.VehicleId?.Value,
            RouteWireFormat.ToLiteral(entity.Shift),
            RouteWireFormat.ToLiteral(entity.Status),
            entity.DepartureTime?.ToString(
                "HH:mm:ss",
                CultureInfo.InvariantCulture),
            entity.ScheduleConfirmed,
            stops,
            schedule);
    }
}