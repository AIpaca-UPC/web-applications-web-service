using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.QueryServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest;

/// <summary>
///     REST API controller for the subscription plan catalog.
/// </summary>
/// <param name="planCommandService">The command service.</param>
/// <param name="planQueryService">The query service.</param>
/// <param name="problemDetailsFactory">The shared error response factory.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Plans")]
public class PlansController(
    IPlanCommandService planCommandService,
    IPlanQueryService planQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>
    ///     Retrieves all subscription plans.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The plan catalog.</returns>
    [HttpGet]
    [EndpointSummary("Get all subscription plans")]
    [ProducesResponseType(
        typeof(IEnumerable<PlanResource>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPlans(
        CancellationToken cancellationToken)
    {
        var plans = await planQueryService.Handle(
            new GetAllPlansQuery(),
            cancellationToken);

        var resources = plans
            .Select(PlanResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();

        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves a subscription plan by its identifier.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested plan.</returns>
    [HttpGet("{planId:int}")]
    [EndpointSummary("Get a subscription plan by ID")]
    [ProducesResponseType(
        typeof(PlanResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlanById(
        int planId,
        CancellationToken cancellationToken)
    {
        if (planId <= 0)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                PlansError.InvalidPlanData,
                "Plan ID must be positive.");
        }

        var plan = await planQueryService.Handle(
            new GetPlanByIdQuery(planId),
            cancellationToken);

        if (plan is null)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status404NotFound,
                PlansError.PlanNotFound,
                $"No plan was found with ID {planId}.");
        }

        return Ok(
            PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
    }

    /// <summary>
    ///     Creates a new subscription plan.
    /// </summary>
    /// <param name="resource">The creation resource.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created plan or an error response.</returns>
    [HttpPost]
    [EndpointSummary("Create a subscription plan")]
    [ProducesResponseType(
        typeof(PlanResource),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePlan(
        [FromBody] CreatePlanResource resource,
        CancellationToken cancellationToken)
    {
        var command =
            CreatePlanCommandFromResourceAssembler
                .ToCommandFromResource(resource);

        var result = await planCommandService.Handle(
            command,
            cancellationToken);

        if (result.IsSuccess)
        {
            var plan = result.Value!;

            return CreatedAtAction(
                nameof(GetPlanById),
                new { planId = plan.Id.Value },
                PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
        }

        var statusCode = result.Error switch
        {
            PlansError.PlanAlreadyExists =>
                StatusCodes.Status409Conflict,

            PlansError.InvalidPlanData =>
                StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        return problemDetailsFactory.CreateProblemDetails(
            this,
            statusCode,
            result.Error,
            result.Message);
    }
}