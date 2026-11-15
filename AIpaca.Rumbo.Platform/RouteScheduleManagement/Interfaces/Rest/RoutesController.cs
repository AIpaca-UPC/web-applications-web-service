using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest;

/// <summary>
/// REST API for school route configuration and publication.
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
    [ProducesResponseType(typeof(IEnumerable<RouteResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoutes(
        [FromQuery] int? driverProfileId,
        CancellationToken cancellationToken)
    {
        if (driverProfileId.HasValue && driverProfileId.Value < 1)
            return Invalid("Driver profile ID must be positive.");

        var routes = driverProfileId.HasValue
            ? await queryService.Handle(
                new GetAllRoutesByDriverProfileIdQuery(driverProfileId.Value),
                cancellationToken)
            : await queryService.Handle(
                new GetAllRoutesQuery(),
                cancellationToken);

        return Ok(routes.Select(
            RouteResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    /// Retrieves a route by ID.
    /// </summary>
    [HttpGet("{routeId:int}")]
    [EndpointSummary("Get route by ID")]
    [ProducesResponseType(typeof(RouteResource), StatusCodes.Status200OK)]
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
    /// Creates a configurable route.
    /// </summary>
    [HttpPost]
    [EndpointSummary("Create route")]
    [ProducesResponseType(typeof(RouteResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoute(
        [FromBody] CreateRouteResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                CreateRouteCommandFromResourceAssembler.ToCommandFromResource(resource);

            var result = await commandService.Handle(
                command, cancellationToken);

            return RoutesActionsResultAssembler.ToActionResultFromCommandResult(
                this,
                result,
                problemDetailsFactory,
                created => CreatedAtAction(
                    nameof(GetRouteById),
                    new { routeId = created.Id.Value },
                    RouteResourceFromEntityAssembler.ToResourceFromEntity(created)));
        }
        catch (ArgumentException ex)
        {
            return Invalid(ex.Message);
        }
    }

    /// <summary>
    /// Updates route information, stops and schedule.
    /// </summary>
    [HttpPut("{routeId:int}")]
    [EndpointSummary("Update route")]
    public async Task<IActionResult> UpdateRoute(
        int routeId,
        [FromBody] UpdateRouteResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                UpdateRouteCommandFromResourceAssembler.ToCommandFromResource(
                    routeId, resource);

            var result = await commandService.Handle(
                command, cancellationToken);

            return RouteCommandResponse(result);
        }
        catch (ArgumentException ex)
        {
            return Invalid(ex.Message);
        }
    }

    /// <summary>
    /// Updates a route's weekly schedule.
    /// </summary>
    [HttpPut("{routeId:int}/schedule")]
    [EndpointSummary("Update route schedule")]
    public async Task<IActionResult> UpdateRouteSchedule(
        int routeId,
        [FromBody] UpdateRouteScheduleResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                RouteActionCommandFromResourceAssembler.ToUpdateScheduleCommand(
                    routeId, resource);

            var result = await commandService.Handle(
                command, cancellationToken);

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
    public async Task<IActionResult> AssignStudent(
        int routeId,
        int stopId,
        [FromBody] AssignStudentResource resource,
        CancellationToken cancellationToken)
    {
        var command =
            RouteActionCommandFromResourceAssembler.ToAssignStudentCommand(
                routeId, stopId, resource);

        var result = await commandService.Handle(
            command, cancellationToken);

        return RouteCommandResponse(result);
    }

    /// <summary>
    /// Replaces all student assignments of a route.
    /// </summary>
    [HttpPut("{routeId:int}/assignments")]
    [EndpointSummary("Replace route assignments")]
    public async Task<IActionResult> ReplaceAssignments(
        int routeId,
        [FromBody] ReplaceRouteAssignmentsResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command =
                RouteActionCommandFromResourceAssembler.ToReplaceAssignmentsCommand(
                    routeId, resource);

            var result = await commandService.Handle(
                command, cancellationToken);

            return RouteCommandResponse(result);
        }
        catch (ArgumentException ex)
        {
            return Invalid(ex.Message);
        }
    }

    /// <summary>
    /// Publishes a route when its configuration is complete.
    /// </summary>
    [HttpPost("{routeId:int}/publish")]
    [EndpointSummary("Publish route")]
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
    /// Deletes an existing route.
    /// </summary>
    [HttpDelete("{routeId:int}")]
    [EndpointSummary("Delete route")]
    public async Task<IActionResult> DeleteRoute(
        int routeId,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(
            new DeleteRouteCommand(routeId),
            cancellationToken);

        return RoutesActionsResultAssembler.ToActionResultFromCommandResult(
            this,
            result,
            problemDetailsFactory,
            _ => NoContent());
    }

    /// <summary>
    /// Converts route command results into HTTP responses.
    /// </summary>
    private IActionResult RouteCommandResponse(
        AIpaca.Rumbo.Platform.Shared.Application.Model.Result<
            AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route> result)
    {
        return RoutesActionsResultAssembler.ToActionResultFromCommandResult(
            this,
            result,
            problemDetailsFactory,
            route => Ok(
                RouteResourceFromEntityAssembler.ToResourceFromEntity(route)));
    }

    /// <summary>
    /// Returns a standardized HTTP 400 response.
    /// </summary>
    private IActionResult Invalid(string message)
    {
        return RoutesActionsResultAssembler.ToInvalidRequest(
            this,
            problemDetailsFactory,
            message);
    }
}
