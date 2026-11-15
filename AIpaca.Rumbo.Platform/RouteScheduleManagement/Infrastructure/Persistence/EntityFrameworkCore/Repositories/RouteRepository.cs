using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Repository implementation for the Route aggregate,
/// using Entity Framework Core for database persistence.
/// </summary>
/// <param name="context">The application database context.</param>
public class RouteRepository(AppDbContext context)
    : BaseRepository<Route>(context), IRouteRepository
{
    /// <summary>
    /// Finds a route by its strongly typed identifier.
    /// The returned aggregate is tracked for modifications.
    /// </summary>
    public async Task<Route?> FindByIdAsync(
        RouteId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Route>()
            .AsSplitQuery()
            .FirstOrDefaultAsync(route => route.Id == id, cancellationToken);
    }

    /// <summary>
    /// Finds a route without change tracking.
    /// Intended for read-only operations.
    /// </summary>
    public async Task<Route?> FindByIdAsNoTrackingAsync(
        RouteId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Route>()
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(route => route.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all routes assigned to a given driver.
    /// </summary>
    public async Task<IEnumerable<Route>> FindAllByDriverProfileIdAsync(
        DriverProfileId driverProfileId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Route>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(route => route.DriverProfileId == driverProfileId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Checks whether a route with the given identifier exists.
    /// </summary>
    public async Task<bool> ExistsByIdAsync(
        RouteId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Route>()
            .AnyAsync(route => route.Id == id, cancellationToken);
    }
}
