using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Repositories;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.Internal.QueryServices;

/// <summary>
/// Implements read operations for the Route aggregate.
/// </summary>
/// <param name="routeRepository">Route persistence contract.</param>
public class RouteQueryService(IRouteRepository routeRepository)
    : IRouteQueryService
{
    /// <summary>
    /// Retrieves all routes without entity tracking.
    /// </summary>
    public async Task<IEnumerable<Route>> Handle(
        GetAllRoutesQuery query,
        CancellationToken cancellationToken)
    {
        return await routeRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a route by its unique identifier.
    /// </summary>
    public async Task<Route?> Handle(
        GetRouteByIdQuery query,
        CancellationToken cancellationToken)
    {
        if (query.RouteId < 1)
            return null;

        return await routeRepository.FindByIdAsNoTrackingAsync(
            new RouteId(query.RouteId),
            cancellationToken);
    }

    /// <summary>
    /// Retrieves the routes assigned to a driver.
    /// </summary>
    public async Task<IEnumerable<Route>> Handle(
        GetAllRoutesByDriverProfileIdQuery query,
        CancellationToken cancellationToken)
    {
        if (query.DriverProfileId < 1)
            return [];

        return await routeRepository.FindAllByDriverProfileIdAsync(
            new DriverProfileId(query.DriverProfileId),
            cancellationToken);
    }
}