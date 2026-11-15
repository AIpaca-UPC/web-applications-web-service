using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.QueryServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.Internal.QueryServices;

/// <summary>
///     Implements queries for the subscription plan catalog.
/// </summary>
/// <param name="planRepository">The plan repository.</param>
public class PlanQueryService(IPlanRepository planRepository)
    : IPlanQueryService
{
    /// <summary>
    ///     Retrieves all subscription plans.
    /// </summary>
    /// <param name="query">The catalog query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The complete plan collection.</returns>
    public async Task<IEnumerable<Plan>> Handle(
        GetAllPlansQuery query,
        CancellationToken cancellationToken = default)
    {
        return await planRepository.ListAsNoTrackingAsync(
            cancellationToken);
    }

    /// <summary>
    ///     Retrieves a plan by its strongly typed identifier.
    /// </summary>
    /// <param name="query">The plan identifier query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The plan if found; otherwise, null.</returns>
    public async Task<Plan?> Handle(
        GetPlanByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await planRepository.FindByIdAsNoTrackingAsync(
            new PlanId(query.PlanId),
            cancellationToken);
    }
}