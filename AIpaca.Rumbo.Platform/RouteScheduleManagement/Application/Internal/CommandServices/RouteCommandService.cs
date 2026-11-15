using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Repositories;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.Internal.CommandServices;

/// <summary>
/// Implements the commands of the
/// Route Schedule Management bounded context.
/// </summary>
/// <param name="routeRepository">
/// Repository for route persistence.
/// </param>
/// <param name="unitOfWork">
/// Coordinates database changes.
/// </param>
public class RouteCommandService(
    IRouteRepository routeRepository,
    IUnitOfWork unitOfWork) : IRouteCommandService
{
    /// <summary>
    /// Creates a new route with its stops and weekly schedule.
    /// </summary>
    public async Task<Result<Route>> Handle(
        CreateRouteCommand command,
        CancellationToken cancellationToken)
    {
        if (command.DriverProfileId < 1)
            return Invalid(
                "Driver profile ID must be positive.");

        Route route;

        try
        {
            route = new Route(
                new DriverProfileId(command.DriverProfileId),
                command.Name,
                command.Shift,
                ToVehicleId(command.VehicleId),
                command.Stops,
                command.Schedule,
                command.ScheduleConfirmed);
        }
        catch (Exception ex) when (
            ex is ArgumentException or InvalidOperationException)
        {
            return Invalid(ex.Message);
        }

        await routeRepository.AddAsync(
            route, cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Route>.Success(route);
    }

    /// <summary>
    /// Updates route information, ordered stops and schedule.
    /// Published routes cannot be modified.
    /// </summary>
    public async Task<Result<Route>> Handle(
        UpdateRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        if (route.IsPublished)
            return PublishedRoute();

        try
        {
            route.UpdateInformation(
                command.Name,
                command.Shift,
                ToVehicleId(command.VehicleId));

            route.ReplaceStops(command.Stops);

            route.UpdateSchedule(
                command.Schedule,
                command.ScheduleConfirmed);
        }
        catch (Exception ex) when (
            ex is ArgumentException or InvalidOperationException)
        {
            return Invalid(ex.Message);
        }

        return await SaveAsync(route, cancellationToken);
    }

    /// <summary>
    /// Updates only the weekly schedule of a configurable route.
    /// </summary>
    public async Task<Result<Route>> Handle(
        UpdateRouteScheduleCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        if (route.IsPublished)
            return PublishedRoute();

        try
        {
            route.UpdateSchedule(
                command.Schedule,
                command.Confirm);
        }
        catch (Exception ex) when (
            ex is ArgumentException or InvalidOperationException)
        {
            return Invalid(ex.Message);
        }

        return await SaveAsync(route, cancellationToken);
    }

    /// <summary>
    /// Assigns a student to an existing route stop.
    /// Repeating the same assignment is idempotent.
    /// </summary>
    public async Task<Result<Route>> Handle(
        AssignStudentToRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        if (route.IsPublished)
            return PublishedRoute();

        if (!route.HasStop(command.StopId))
            return Result<Route>.Failure(
                RoutesError.StopNotFound,
                $"Stop {command.StopId} does not belong to this route.");

        if (command.StudentId < 1)
            return Invalid(
                "Student ID must be positive.");

        var studentId = new StudentId(command.StudentId);

        var existingAssignment =
            route.FindAssignment(studentId);

        if (existingAssignment is not null)
        {
            if (existingAssignment.StopId == command.StopId)
                return Result<Route>.Success(route);

            return Result<Route>.Failure(
                RoutesError.StudentAlreadyAssigned,
                "Student is already assigned to another stop of this route.");
        }

        if (route.Assignments.Any(
                assignment => assignment.StopId == command.StopId))
            return Result<Route>.Failure(
                RoutesError.StopHasAssignments,
                "Stop already has an assigned student.");

        try
        {
            route.AssignStudent(
                command.StopId,
                studentId);
        }
        catch (Exception ex) when (
            ex is ArgumentException or InvalidOperationException)
        {
            return Invalid(ex.Message);
        }

        return await SaveAsync(route, cancellationToken);
    }

    /// <summary>
    /// Removes the student assignment from an existing
    /// stop of a configurable route.
    /// </summary>
    public async Task<Result<Route>> Handle(
        UnassignStudentFromRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        if (route.IsPublished)
            return PublishedRoute();

        if (!route.HasStop(command.StopId))
            return Result<Route>.Failure(
                RoutesError.StopNotFound,
                $"Stop {command.StopId} does not belong to this route.");

        route.UnassignStudent(command.StopId);

        return await SaveAsync(route, cancellationToken);
    }

    /// <summary>
    /// Replaces student-to-stop assignments of a route.
    /// Published routes cannot be reconfigured.
    /// </summary>
    public async Task<Result<Route>> Handle(
        ReplaceRouteAssignmentsCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        if (route.IsPublished)
            return PublishedRoute();

        try
        {
            route.ReplaceAssignments(command.Assignments);
        }
        catch (Exception ex) when (
            ex is ArgumentException or InvalidOperationException)
        {
            return Invalid(ex.Message);
        }

        return await SaveAsync(route, cancellationToken);
    }

    /// <summary>
    /// Publishes a route that satisfies all
    /// publication requirements.
    /// </summary>
    public async Task<Result<Route>> Handle(
        PublishRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        if (route.IsPublished)
            return Result<Route>.Failure(
                RoutesError.RouteAlreadyPublished,
                "Route has already been published.");

        try
        {
            route.Publish();
        }
        catch (InvalidOperationException ex)
        {
            return Result<Route>.Failure(
                RoutesError.RouteNotReadyForPublication,
                ex.Message);
        }

        return await SaveAsync(route, cancellationToken);
    }

    /// <summary>
    /// Deletes a configurable route by its identifier.
    /// Published routes cannot be deleted.
    /// </summary>
    public async Task<Result<int>> Handle(
        DeleteRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return Result<int>.Failure(
                RoutesError.RouteNotFound,
                $"Route {command.RouteId} was not found.");

        if (route.IsPublished)
            return Result<int>.Failure(
                RoutesError.RouteAlreadyPublished,
                "Published routes cannot be deleted.");

        routeRepository.Remove(route);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<int>.Success(command.RouteId);
    }

    /// <summary>
    /// Loads a route for modification using its strongly
    /// typed identifier.
    /// </summary>
    private async Task<Route?> FindRouteAsync(
        int routeId,
        CancellationToken cancellationToken)
    {
        if (routeId < 1)
            return null;

        return await routeRepository.FindByIdAsync(
            new RouteId(routeId),
            cancellationToken);
    }

    /// <summary>
    /// Persists changes and returns the updated route.
    /// </summary>
    private async Task<Result<Route>> SaveAsync(
        Route route,
        CancellationToken cancellationToken)
    {
        routeRepository.Update(route);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Route>.Success(route);
    }

    /// <summary>
    /// Converts an optional vehicle identifier into
    /// its strongly typed value object.
    /// </summary>
    private static VehicleId? ToVehicleId(int? vehicleId)
    {
        return vehicleId.HasValue
            ? new VehicleId(vehicleId.Value)
            : null;
    }

    /// <summary>
    /// Returns an invalid route data result.
    /// </summary>
    private static Result<Route> Invalid(string message)
    {
        return Result<Route>.Failure(
            RoutesError.InvalidRouteData,
            message);
    }

    /// <summary>
    /// Returns a route not found result.
    /// </summary>
    private static Result<Route> RouteNotFound(int routeId)
    {
        return Result<Route>.Failure(
            RoutesError.RouteNotFound,
            $"Route {routeId} was not found.");
    }

    /// <summary>
    /// Returns a conflict when a published route
    /// is modified.
    /// </summary>
    private static Result<Route> PublishedRoute()
    {
        return Result<Route>.Failure(
            RoutesError.RouteAlreadyPublished,
            "Published routes cannot be modified.");
    }
}
