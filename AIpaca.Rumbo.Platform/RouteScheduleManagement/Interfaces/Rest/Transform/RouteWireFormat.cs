
using System.Globalization;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;

/// <summary>
/// Converts REST wire values into route domain types
/// and formats domain values for API responses.
/// </summary>
public static class RouteWireFormat
{
    /// <summary>
    /// Converts MORNING or AFTERNOON to the route shift enum.
    /// </summary>
    public static ERouteShift ToRouteShift(string value)
    {
        if (string.IsNullOrWhiteSpace(value) ||
            !Enum.TryParse<ERouteShift>(value, true, out var shift) ||
            !Enum.IsDefined(shift))
            throw new ArgumentException("Invalid route shift.");

        return shift;
    }

    /// <summary>
    /// Converts route status into its uppercase wire representation.
    /// </summary>
    public static string ToLiteral(ERouteStatus status) =>
        status.ToString().ToUpperInvariant();

    /// <summary>
    /// Converts route shift into its uppercase wire representation.
    /// </summary>
    public static string ToLiteral(ERouteShift shift) =>
        shift.ToString().ToUpperInvariant();

    /// <summary>
    /// Converts input stops to domain entities.
    /// The order of the collection determines their position.
    /// </summary>
    public static IReadOnlyCollection<RouteStop> ToStops(
        IReadOnlyCollection<RouteStopResource>? resources)
    {
        var stops = new List<RouteStop>();

        foreach (var resource in resources ?? [])
        {
            if (resource is null)
                throw new ArgumentException("Stops cannot contain null.");

            if (resource.Id is < 0)
                throw new ArgumentException("Invalid stop ID.");

            // Assignments require an existing, persisted stop.
            // Do not silently discard student IDs sent in the request.
            if (resource.StudentId.HasValue)
                throw new ArgumentException(
                    "Student assignments must be managed separately " +
                    "after the route and its stops have been saved.");

            var position = stops.Count + 1;

            stops.Add(resource.Id is > 0
                ? new RouteStop(resource.Id.Value, resource.Address, position)
                : new RouteStop(resource.Address, position));
        }

        return stops;
    }

    /// <summary>
    /// Converts weekly schedule resources to domain value objects.
    /// </summary>
    public static IReadOnlyCollection<RouteSchedule> ToSchedule(
        IReadOnlyCollection<RouteScheduleResource>? resources)
    {
        var schedule = new List<RouteSchedule>();

        foreach (var resource in resources ?? [])
        {
            if (resource is null)
                throw new ArgumentException("Schedule cannot contain null.");

            if (!Enum.TryParse<DayOfWeek>(
                    resource.Day, true, out var day) ||
                !Enum.IsDefined(day))
                throw new ArgumentException("Invalid schedule day.");

            if (!TimeOnly.TryParseExact(
                    resource.DepartureTime,
                    ["HH:mm", "HH:mm:ss"],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var departureTime))
                throw new ArgumentException("Invalid departure time.");

            schedule.Add(new RouteSchedule(day, departureTime));
        }

        return schedule;
    }
}
