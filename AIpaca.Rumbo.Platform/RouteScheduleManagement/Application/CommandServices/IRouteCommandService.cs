using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.CommandServices;

/// <summary>
/// Defines command operations for the
/// Route Schedule Management bounded context.
/// </summary>
public interface IRouteCommandService
{
    /// <summary>
    /// Creates a new configurable route.
    /// </summary>
    Task<Result<Route>> Handle(
        CreateRouteCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing route.
    /// </summary>
    Task<Result<Route>> Handle(
        UpdateRouteCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates the weekly schedule of a route.
    /// </summary>
    Task<Result<Route>> Handle(
        UpdateRouteScheduleCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Assigns a student to a route stop.
    /// </summary>
    Task<Result<Route>> Handle(
        AssignStudentToRouteCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Removes a student assignment from a route stop.
    /// </summary>
    Task<Result<Route>> Handle(
        UnassignStudentFromRouteCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Replaces all student assignments of a route.
    /// </summary>
    Task<Result<Route>> Handle(
        ReplaceRouteAssignmentsCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Publishes a completed route.
    /// </summary>
    Task<Result<Route>> Handle(
        PublishRouteCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a configurable route.
    /// </summary>
    Task<Result<int>> Handle(
        DeleteRouteCommand command,
        CancellationToken cancellationToken);
}
