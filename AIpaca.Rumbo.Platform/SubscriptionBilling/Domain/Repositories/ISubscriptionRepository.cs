
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;

/// <summary>
///     Persistence contract for driver subscriptions
///     within the Subscription & Billing bounded context.
/// </summary>
public interface ISubscriptionRepository
    : IBaseRepository<Subscription>
{
    /// <summary>
    ///     Finds a subscription by its strongly typed identifier.
    /// </summary>
    /// <param name="id">The subscription identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription if found; otherwise, null.</returns>
    Task<Subscription?> FindByIdAsync(
        SubscriptionId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds a subscription without change tracking.
    /// </summary>
    /// <param name="id">The subscription identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription if found; otherwise, null.</returns>
    Task<Subscription?> FindByIdAsNoTrackingAsync(
        SubscriptionId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the subscription history of a driver.
    /// </summary>
    /// <param name="driverProfileId">The driver profile identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The driver's subscriptions.</returns>
    Task<IEnumerable<Subscription>>
        FindByDriverProfileIdAsNoTrackingAsync(
            string driverProfileId,
            CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds a driver's currently active or paused
    ///     subscription within its valid billing period.
    /// </summary>
    /// <param name="driverProfileId">The driver profile identifier.</param>
    /// <param name="today">The date to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The current subscription if found; otherwise, null.</returns>
    Task<Subscription?> FindCurrentByDriverProfileIdAsync(
        string driverProfileId,
        DateOnly today,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds the latest subscription associated
    ///     with a driver and a specific plan.
    /// </summary>
    /// <param name="driverProfileId">The driver profile identifier.</param>
    /// <param name="planId">The plan identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The latest subscription if found; otherwise, null.</returns>
    Task<Subscription?> FindLatestByDriverAndPlanAsync(
        string driverProfileId,
        PlanId planId,
        CancellationToken cancellationToken = default);
}
