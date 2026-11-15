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
/// Implements the commands of the Route Schedule Management context.
/// </summary>
/// <param name="routeRepository">Repository for route persistence.</param>
/// <param name="unitOfWork">Coordinates database changes.</param>
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
            return Invalid("Driver profile ID must be positive.");

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

        await routeRepository.AddAsync(route, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Route>.Success(route);
    }

    /// <summary>
    /// Updates route information, ordered stops and schedule.
    /// </summary>
    public async Task<Result<Route>> Handle(
        UpdateRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

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
    /// Updates only the weekly schedule of a route.
    /// </summary>
    public async Task<Result<Route>> Handle(
        UpdateRouteScheduleCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        try
        {
            route.UpdateSchedule(command.Schedule, command.Confirm);
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
    /// </summary>
    public async Task<Result<Route>> Handle(
        AssignStudentToRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

        if (!route.HasStop(command.StopId))
            return Result<Route>.Failure(
                RoutesError.StopNotFound,
                $"Stop {command.StopId} does not belong to this route.");

        var studentId = new StudentId(command.StudentId);

        if (route.IsStudentAssigned(studentId))
            return Result<Route>.Failure(
                RoutesError.StudentAlreadyAssigned,
                "Student is already assigned to this route.");

        try
        {
            route.AssignStudent(command.StopId, studentId);
        }
        catch (Exception ex) when (
            ex is ArgumentException or InvalidOperationException)
        {
            return Invalid(ex.Message);
        }

        return await SaveAsync(route, cancellationToken);
    }

    /// <summary>
    /// Replaces student-to-stop assignments of a route.
    /// </summary>
    public async Task<Result<Route>> Handle(
        ReplaceRouteAssignmentsCommand command,
        CancellationToken cancellationToken)
    {
        var route = await FindRouteAsync(
            command.RouteId, cancellationToken);

        if (route is null)
            return RouteNotFound(command.RouteId);

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
    /// Publishes a route that satisfies all publication requirements.
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
    /// Deletes a route by its identifier.
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

        routeRepository.Remove(route);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<int>.Success(command.RouteId);
    }

    /// <summary>
    /// Loads a route for modification.
    /// </summary>
    private async Task<Route?> FindRouteAsync(
        int routeId,
        CancellationToken cancellationToken)
    {
        if (routeId < 1)
            return null;

        return await routeRepository.FindByIdAsync(
            new RouteId(routeId), cancellationToken);
    }

    /// <summary>
    /// Persists aggregate changes and returns the updated route.
    /// </summary>
    private async Task<Result<Route>> SaveAsync(
        Route route,
        CancellationToken cancellationToken)
    {
        routeRepository.Update(route);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Route>.Success(route);
    }

    private static VehicleId? ToVehicleId(int? vehicleId)
    {
        return vehicleId.HasValue
            ? new VehicleId(vehicleId.Value)
            : null;
    }

    private static Result<Route> Invalid(string message)
    {
        return Result<Route>.Failure(
            RoutesError.InvalidRouteData, message);
    }

    private static Result<Route> RouteNotFound(int routeId)
    {
        return Result<Route>.Failure(
            RoutesError.RouteNotFound,
            $"Route {routeId} was not found.");
    }
}
