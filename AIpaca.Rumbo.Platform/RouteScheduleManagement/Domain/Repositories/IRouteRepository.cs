using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Repositories;

/// <summary>
/// Defines the persistence contract for the Route aggregate.
/// </summary>
public interface IRouteRepository : IBaseRepository<Route>
{
    /// <summary>
    /// Finds a route by its strongly typed identifier.
    /// </summary>
    Task<Route?> FindByIdAsync(
        RouteId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a route without Entity Framework change tracking.
    /// </summary>
    Task<Route?> FindByIdAsNoTrackingAsync(
        RouteId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves routes belonging to a driver.
    /// </summary>
    Task<IEnumerable<Route>> FindAllByDriverProfileIdAsync(
        DriverProfileId driverProfileId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a route exists.
    /// </summary>
    Task<bool> ExistsByIdAsync(
        RouteId id,
        CancellationToken cancellationToken = default);
}