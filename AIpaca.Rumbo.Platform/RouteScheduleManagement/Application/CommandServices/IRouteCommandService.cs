using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.CommandServices;

/// <summary>
/// Defines command operations for the Route Schedule Management context.
/// </summary>
public interface IRouteCommandService
{
    Task<Result<Route>> Handle(
        CreateRouteCommand command, CancellationToken cancellationToken);

    Task<Result<Route>> Handle(
        UpdateRouteCommand command, CancellationToken cancellationToken);

    Task<Result<Route>> Handle(
        UpdateRouteScheduleCommand command, CancellationToken cancellationToken);

    Task<Result<Route>> Handle(
        AssignStudentToRouteCommand command, CancellationToken cancellationToken);

    Task<Result<Route>> Handle(
        ReplaceRouteAssignmentsCommand command, CancellationToken cancellationToken);

    Task<Result<Route>> Handle(
        PublishRouteCommand command, CancellationToken cancellationToken);

    Task<Result<int>> Handle(
        DeleteRouteCommand command, CancellationToken cancellationToken);
}