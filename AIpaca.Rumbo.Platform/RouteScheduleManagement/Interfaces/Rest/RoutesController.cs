using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest;

/// <summary>
/// REST API for school route configuration,
/// student assignments and route publication.
/// </summary>
[ApiController]
[Route("api/v1/routes")]
[Produces("application/json")]
[Tags("Routes")]
public class RoutesController(
    IRouteCommandService commandService,
    IRouteQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>
    /// Retrieves all routes or filters them by driver.
    /// </summary>
    [HttpGet]
    [EndpointSummary("Get routes")]
    [ProducesResponseType(
        typeof(IEnumerable<RouteResource>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoutes(
        [FromQuery] int? driverProfileId,
        CancellationToken cancellationToken)
    {
        if (driverProfileId.HasValue &&
            driverProfileId.Value < 1)
            return Invalid(
                "Driver profile ID must be positive.");

        var routes = driverProfileId.HasValue
            ? await queryService.Handle(
                new GetAllRoutesByDriverProfileIdQuery(
                    driverProfileId.Value),
                cancellationToken)
            : await queryService.Handle(
                new GetAllRoutesQuery(),
                cancellationToken);

        return Ok(routes.Select(
            RouteResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    /// Retrieves a route by its unique identifier.
    /// </summary>
    [HttpGet("{routeId:int}")]
    [EndpointSummary("Get route by ID")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRouteById(
        int routeId,
        CancellationToken cancellationToken)
    {
        var route = await queryService.Handle(
            new GetRouteByIdQuery(routeId),
            cancellationToken);

        if (route is null)
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status404NotFound,
                RoutesError.RouteNotFound,
                $"Route {routeId} was not found.");

        return Ok(
            RouteResourceFromEntityAssembler.ToResourceFromEntity(route));
    }

    /// <summary>
    /// Creates a configurable route with stops and schedule.
    /// </summary>
    [HttpPost]
    [EndpointSummary("Create route")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoute(
        [FromBody] CreateRouteResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                CreateRouteCommandFromResourceAssembler
                    .ToCommandFromResource(resource);

            var result = await commandService.Handle(
                command,
                cancellationToken);

            return RoutesActionsResultAssembler
                .ToActionResultFromCommandResult(
                    this,
                    result,
                    problemDetailsFactory,
                    created => CreatedAtAction(
                        nameof(GetRouteById),
                        new { routeId = created.Id.Value },
                        RouteResourceFromEntityAssembler
                            .ToResourceFromEntity(created)));
        }
        catch (ArgumentException ex)
        {
            return Invalid(ex.Message);
        }
    }

    /// <summary>
    /// Updates route information, stops and weekly schedule.
    /// </summary>
    [HttpPut("{routeId:int}")]
    [EndpointSummary("Update route")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateRoute(
        int routeId,
        [FromBody] UpdateRouteResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                UpdateRouteCommandFromResourceAssembler
                    .ToCommandFromResource(routeId, resource);

            var result = await commandService.Handle(
                command,
                cancellationToken);

            return RouteCommandResponse(result);
        }
        catch (ArgumentException ex)
        {
            return Invalid(ex.Message);
        }
    }

    /// <summary>
    /// Updates only the weekly schedule of a route.
    /// </summary>
    [HttpPut("{routeId:int}/schedule")]
    [EndpointSummary("Update route schedule")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateRouteSchedule(
        int routeId,
        [FromBody] UpdateRouteScheduleResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                RouteActionCommandFromResourceAssembler
                    .ToUpdateScheduleCommand(routeId, resource);

            var result = await commandService.Handle(
                command,
                cancellationToken);

            return RouteCommandResponse(result);
        }
        catch (ArgumentException ex)
        {
            return Invalid(ex.Message);
        }
    }

    /// <summary>
    /// Assigns a student to an existing route stop.
    /// </summary>
    [HttpPut("{routeId:int}/stops/{stopId:int}/student")]
    [EndpointSummary("Assign student to stop")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignStudent(
        int routeId,
        int stopId,
        [FromBody] AssignStudentResource resource,
        CancellationToken cancellationToken)
    {
        var command =
            RouteActionCommandFromResourceAssembler
                .ToAssignStudentCommand(
                    routeId,
                    stopId,
                    resource);

        var result = await commandService.Handle(
            command,
            cancellationToken);

        return RouteCommandResponse(result);
    }

    /// <summary>
    /// Removes a student assignment without deleting the stop.
    /// </summary>
    [HttpDelete("{routeId:int}/stops/{stopId:int}/student")]
    [EndpointSummary("Unassign student from route stop")]
    [EndpointDescription(
        "Removes the student assignment without deleting the stop.")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UnassignStudent(
        int routeId,
        int stopId,
        CancellationToken cancellationToken)
    {
        var command = new UnassignStudentFromRouteCommand(
            routeId,
            stopId);

        var result = await commandService.Handle(
            command,
            cancellationToken);

        return RouteCommandResponse(result);
    }

    /// <summary>
    /// Replaces every student-to-stop assignment of a route.
    /// </summary>
    [HttpPut("{routeId:int}/assignments")]
    [EndpointSummary("Replace route assignments")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReplaceAssignments(
        int routeId,
        [FromBody] ReplaceRouteAssignmentsResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                RouteActionCommandFromResourceAssembler
                    .ToReplaceAssignmentsCommand(
                        routeId,
                        resource);

            var result = await commandService.Handle(
                command,
                cancellationToken);

            return RouteCommandResponse(result);
        }
        catch (ArgumentException ex)
        {
            return Invalid(ex.Message);
        }
    }

    /// <summary>
    /// Publishes a route when all requirements are satisfied.
    /// </summary>
    [HttpPost("{routeId:int}/publish")]
    [EndpointSummary("Publish route")]
    [ProducesResponseType(
        typeof(RouteResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PublishRoute(
        int routeId,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(
            new PublishRouteCommand(routeId),
            cancellationToken);

        return RouteCommandResponse(result);
    }

    /// <summary>
    /// Deletes a configurable route.
    /// </summary>
    [HttpDelete("{routeId:int}")]
    [EndpointSummary("Delete route")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteRoute(
        int routeId,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(
            new DeleteRouteCommand(routeId),
            cancellationToken);

        return RoutesActionsResultAssembler
            .ToActionResultFromCommandResult(
                this,
                result,
                problemDetailsFactory,
                _ => NoContent());
    }

    /// <summary>
    /// Converts route command results into HTTP responses.
    /// </summary>
    private IActionResult RouteCommandResponse(
        Result<Route> result)
    {
        return RoutesActionsResultAssembler
            .ToActionResultFromCommandResult(
                this,
                result,
                problemDetailsFactory,
                route => Ok(
                    RouteResourceFromEntityAssembler
                        .ToResourceFromEntity(route)));
    }

    /// <summary>
    /// Returns an HTTP 400 response for invalid input.
    /// </summary>
    private IActionResult Invalid(string message)
    {
        return RoutesActionsResultAssembler.ToInvalidRequest(
            this,
            problemDetailsFactory,
            message);
    }
}
