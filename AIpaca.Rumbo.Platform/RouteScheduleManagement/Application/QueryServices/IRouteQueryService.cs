using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Queries;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.QueryServices;

/// <summary>
/// Defines read operations for the Route Schedule Management context.
/// </summary>
public interface IRouteQueryService
{
    Task<IEnumerable<Route>> Handle(
        GetAllRoutesQuery query, CancellationToken cancellationToken);

    Task<Route?> Handle(
        GetRouteByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<Route>> Handle(
        GetAllRoutesByDriverProfileIdQuery query,
        CancellationToken cancellationToken);
}