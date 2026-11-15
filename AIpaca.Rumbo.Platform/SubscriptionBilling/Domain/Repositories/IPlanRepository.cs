using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;

/// <summary>
///     Persistence contract for subscription plans
///     within the Subscription & Billing bounded context.
/// </summary>
public interface IPlanRepository : IBaseRepository<Plan>
{
    /// <summary>
    ///     Finds a subscription plan by its strongly typed identifier.
    /// </summary>
    /// <param name="id">The unique plan identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The plan if found; otherwise, null.</returns>
    Task<Plan?> FindByIdAsync(
        PlanId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds a plan by its identifier without change tracking.
    /// </summary>
    /// <param name="id">The unique plan identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The plan if found; otherwise, null.</returns>
    Task<Plan?> FindByIdAsNoTrackingAsync(
        PlanId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Determines whether a subscription plan
    ///     with the given name already exists.
    /// </summary>
    /// <param name="name">The plan name to verify.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the plan exists; otherwise, false.</returns>
    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}