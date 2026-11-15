using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.QueryServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.Internal.QueryServices;

/// <summary>
///     Implements subscription queries using
///     the subscription repository.
/// </summary>
/// <param name="subscriptionRepository">
///     The subscription repository.
/// </param>
public class SubscriptionQueryService(
    ISubscriptionRepository subscriptionRepository)
    : ISubscriptionQueryService
{
    /// <summary>
    ///     Retrieves subscriptions, optionally filtering
    ///     by driver and cancellation status.
    /// </summary>
    /// <param name="query">The subscription query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The retrieved subscriptions.</returns>
    public async Task<IEnumerable<Subscription>> Handle(
        GetAllSubscriptionsQuery query,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Subscription> subscriptions;

        if (string.IsNullOrWhiteSpace(query.DriverProfileId))
        {
            subscriptions = await subscriptionRepository
                .ListAsNoTrackingAsync(cancellationToken);
        }
        else
        {
            subscriptions = await subscriptionRepository
                .FindByDriverProfileIdAsNoTrackingAsync(
                    query.DriverProfileId.Trim(),
                    cancellationToken);
        }

        if (!query.IncludeCancelled)
        {
            subscriptions = subscriptions.Where(
                subscription =>
                    subscription.Status != ESubscriptionStatus.Cancelled);
        }

        return subscriptions.ToList();
    }

    /// <summary>
    ///     Retrieves a subscription by its identifier.
    /// </summary>
    /// <param name="query">The identifier query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription if found; otherwise, null.</returns>
    public async Task<Subscription?> Handle(
        GetSubscriptionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository
            .FindByIdAsNoTrackingAsync(
                new SubscriptionId(query.SubscriptionId),
                cancellationToken);
    }
}
