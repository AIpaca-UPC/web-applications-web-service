
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.QueryServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest;

/// <summary>
///     REST controller for managing driver subscriptions.
/// </summary>
/// <param name="commandService">The subscription command service.</param>
/// <param name="queryService">The subscription query service.</param>
/// <param name="problemDetailsFactory">The shared error factory.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Subscriptions")]
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>
    ///     Returns the current date in the America/Lima time zone.
    /// </summary>
    /// <returns>The current date in Lima.</returns>
    private static DateOnly TodayInLima()
    {
        var zone = TimeZoneInfo.FindSystemTimeZoneById("America/Lima");
        var localNow = TimeZoneInfo.ConvertTime(
            DateTimeOffset.UtcNow, zone);

        return DateOnly.FromDateTime(localNow.Date);
    }

    /// <summary>
    ///     Retrieves subscriptions, optionally filtered by driver.
    /// </summary>
    /// <param name="driverProfileId">Optional driver identifier.</param>
    /// <param name="includeCancelled">
    ///     Whether cancelled subscriptions should be included.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription collection.</returns>
    [HttpGet]
    [EndpointSummary("Get driver subscriptions")]
    [ProducesResponseType(
        typeof(IEnumerable<SubscriptionResource>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptions(
        [FromQuery] string? driverProfileId,
        [FromQuery] bool includeCancelled,
        CancellationToken cancellationToken)
    {
        var query = new GetAllSubscriptionsQuery(
            driverProfileId,
            includeCancelled);

        var subscriptions = await queryService.Handle(
            query,
            cancellationToken);

        var today = TodayInLima();

        var resources = subscriptions
            .Select(subscription =>
                SubscriptionResourceFromEntityAssembler
                    .ToResourceFromEntity(subscription, today))
            .ToList();

        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves a subscription by its identifier.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription if found.</returns>
    [HttpGet("{subscriptionId:int}")]
    [EndpointSummary("Get a subscription by ID")]
    [ProducesResponseType(
        typeof(SubscriptionResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionById(
        int subscriptionId,
        CancellationToken cancellationToken)
    {
        if (subscriptionId <= 0)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                SubscriptionsError.InvalidSubscriptionData,
                "Subscription ID must be positive.");
        }

        var subscription = await queryService.Handle(
            new GetSubscriptionByIdQuery(subscriptionId),
            cancellationToken);

        if (subscription is null)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status404NotFound,
                SubscriptionsError.SubscriptionNotFound,
                $"No subscription was found with ID {subscriptionId}.");
        }

        return Ok(
            SubscriptionResourceFromEntityAssembler
                .ToResourceFromEntity(subscription, TodayInLima()));
    }

    /// <summary>
    ///     Activates a subscription for a driver.
    /// </summary>
    /// <param name="resource">The activation resource.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created or existing subscription.</returns>
    [HttpPost]
    [EndpointSummary("Activate a driver subscription")]
    [ProducesResponseType(
        typeof(SubscriptionResource),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(SubscriptionResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActivateSubscription(
        [FromBody] ActivateSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var command =
            ActivateSubscriptionCommandFromResourceAssembler
                .ToCommandFromResource(resource);

        var result = await commandService.Handle(
            command,
            cancellationToken);

        if (result.IsSuccess)
        {
            var activation = result.Value!;
            var subscription = activation.Subscription;

            var response = SubscriptionResourceFromEntityAssembler
                .ToResourceFromEntity(subscription, TodayInLima());

            if (!activation.Created)
                return Ok(response);

            return CreatedAtAction(
                nameof(GetSubscriptionById),
                new { subscriptionId = subscription.Id.Value },
                response);
        }

        var statusCode = result.Error switch
        {
            SubscriptionsError.PlanNotFound =>
                StatusCodes.Status404NotFound,

            SubscriptionsError.PlanUnavailable =>
                StatusCodes.Status409Conflict,

            SubscriptionsError.SubscriptionAlreadyExists =>
                StatusCodes.Status409Conflict,

            SubscriptionsError.InvalidSubscriptionData =>
                StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        return problemDetailsFactory.CreateProblemDetails(
            this,
            statusCode,
            result.Error,
            result.Message);
    }

   
    /// <summary>
    ///     Updates a subscription status or changes its plan.
    ///     Plan changes preserve the previous subscription history.
    /// </summary>
    /// <param name="subscriptionId">
    ///     The current subscription identifier.
    /// </param>
    /// <param name="resource">
    ///     The subscription update resource.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     The updated subscription or the replacement subscription.
    /// </returns>
    [HttpPut("{subscriptionId:int}")]
    [EndpointSummary("Update subscription status or change plan")]
    [ProducesResponseType(
        typeof(SubscriptionResource),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSubscription(
        int subscriptionId,
        [FromBody] UpdateSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        if (subscriptionId <= 0)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                SubscriptionsError.InvalidSubscriptionData,
                "Subscription ID must be positive.");
        }

        // Retrieve the existing subscription.
        var existing = await queryService.Handle(
            new GetSubscriptionByIdQuery(subscriptionId),
            cancellationToken);

        if (existing is null ||
            existing.DriverProfileId != resource.DriverProfileId.Trim())
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status404NotFound,
                SubscriptionsError.SubscriptionNotFound,
                "The requested subscription was not found.");
        }

        // When the plan differs, execute the change plan command.
        if (existing.PlanId.Value != resource.PlanId)
        {
            var changePlanCommand =
                ChangeSubscriptionPlanCommandFromResourceAssembler
                    .ToCommandFromResource(subscriptionId, resource);

            if (changePlanCommand is null)
            {
                return problemDetailsFactory.CreateProblemDetails(
                    this,
                    StatusCodes.Status400BadRequest,
                    SubscriptionsError.InvalidSubscriptionData,
                    "Status must be ACTIVE or PAUSED.");
            }

            var changePlanResult = await commandService.Handle(
                changePlanCommand,
                cancellationToken);

            if (changePlanResult.IsSuccess)
            {
                return Ok(
                    SubscriptionResourceFromEntityAssembler
                        .ToResourceFromEntity(
                            changePlanResult.Value!,
                            TodayInLima()));
            }

            var changePlanStatusCode = changePlanResult.Error switch
            {
                SubscriptionsError.SubscriptionNotFound =>
                    StatusCodes.Status404NotFound,

                SubscriptionsError.PlanNotFound =>
                    StatusCodes.Status404NotFound,

                SubscriptionsError.PlanUnavailable =>
                    StatusCodes.Status409Conflict,

                SubscriptionsError.SubscriptionAlreadyExists =>
                    StatusCodes.Status409Conflict,

                SubscriptionsError.SubscriptionStatusConflict =>
                    StatusCodes.Status409Conflict,

                SubscriptionsError.InvalidSubscriptionData =>
                    StatusCodes.Status400BadRequest,

                _ => StatusCodes.Status500InternalServerError
            };

            return problemDetailsFactory.CreateProblemDetails(
                this,
                changePlanStatusCode,
                changePlanResult.Error,
                changePlanResult.Message);
        }

        // When the plan is unchanged, update only the status.
        var statusCommand =
            ChangeSubscriptionStatusCommandFromResourceAssembler
                .ToCommandFromResource(subscriptionId, resource);

        if (statusCommand is null)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                SubscriptionsError.InvalidSubscriptionData,
                "Status must be ACTIVE or PAUSED.");
        }

        var result = await commandService.Handle(
            statusCommand,
            cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(
                SubscriptionResourceFromEntityAssembler
                    .ToResourceFromEntity(
                        result.Value!,
                        TodayInLima()));
        }

        var statusCode = result.Error switch
        {
            SubscriptionsError.SubscriptionNotFound =>
                StatusCodes.Status404NotFound,

            SubscriptionsError.InvalidSubscriptionData =>
                StatusCodes.Status400BadRequest,

            SubscriptionsError.SubscriptionStatusConflict =>
                StatusCodes.Status409Conflict,

            _ => StatusCodes.Status500InternalServerError
        };

        return problemDetailsFactory.CreateProblemDetails(
            this,
            statusCode,
            result.Error,
            result.Message);
    }


    /// <summary>
    ///     Cancels a subscription without deleting
    ///     its historical information.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content when cancellation succeeds.</returns>
    [HttpDelete("{subscriptionId:int}")]
    [EndpointSummary("Cancel a subscription")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelSubscription(
        int subscriptionId,
        CancellationToken cancellationToken)
    {
        if (subscriptionId <= 0)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                SubscriptionsError.InvalidSubscriptionData,
                "Subscription ID must be positive.");
        }

        var existing = await queryService.Handle(
            new GetSubscriptionByIdQuery(subscriptionId),
            cancellationToken);

        if (existing is null)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status404NotFound,
                SubscriptionsError.SubscriptionNotFound,
                "The requested subscription was not found.");
        }

        var command = new CancelSubscriptionCommand(
            new SubscriptionId(subscriptionId),
            existing.DriverProfileId);

        var result = await commandService.Handle(
            command,
            cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        var statusCode = result.Error switch
        {
            SubscriptionsError.SubscriptionNotFound =>
                StatusCodes.Status404NotFound,

            SubscriptionsError.InvalidSubscriptionData =>
                StatusCodes.Status400BadRequest,

            SubscriptionsError.SubscriptionStatusConflict =>
                StatusCodes.Status409Conflict,

            _ => StatusCodes.Status500InternalServerError
        };

        return problemDetailsFactory.CreateProblemDetails(
            this,
            statusCode,
            result.Error,
            result.Message);
    }
}
