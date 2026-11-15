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
    /// Converts a route aggregate into its REST representation,
    /// including ordered stops and student assignments.
    /// </summary>
    /// <param name="entity">Route aggregate to transform.</param>
    /// <returns>Resource exposed by the API.</returns>
    public static RouteResource ToResourceFromEntity(Route entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        // Stops are presented in the order configured by the driver.
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

        // The frontend expects Monday, Tuesday, etc.
        // Keep the days ordered from Monday to Sunday.
        var schedule = entity.Schedule
            .OrderBy(item => ((int)item.DayOfWeek + 6) % 7)
            .Select(item => new RouteScheduleResource(
                item.DayOfWeek.ToString(),
                item.DepartureTime.ToString(
                    "HH:mm:ss",
                    CultureInfo.InvariantCulture)))
            .ToList();

        // Convert domain values to the API contract.
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
