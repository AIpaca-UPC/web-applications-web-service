using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.QueryServices;

/// <summary>
///     Defines application queries for retrieving
///     driver subscriptions.
/// </summary>
public interface ISubscriptionQueryService
{
    /// <summary>
    ///     Retrieves subscriptions, optionally filtered by driver.
    /// </summary>
    /// <param name="query">The subscription query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription collection.</returns>
    Task<IEnumerable<Subscription>> Handle(
        GetAllSubscriptionsQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a subscription by its identifier.
    /// </summary>
    /// <param name="query">The subscription identifier query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription if found; otherwise, null.</returns>
    Task<Subscription?> Handle(
        GetSubscriptionByIdQuery query,
        CancellationToken cancellationToken = default);
}