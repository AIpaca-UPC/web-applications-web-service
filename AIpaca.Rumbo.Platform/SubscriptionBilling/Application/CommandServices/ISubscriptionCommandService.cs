using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;

/// <summary>
///     Represents the result of activating a subscription.
/// </summary>
/// <param name="Subscription">The resulting subscription.</param>
/// <param name="Created">
///     True when a new subscription was created;
///     false when an existing subscription was returned.
/// </param>
public record SubscriptionActivation(
    Subscription Subscription,
    bool Created
);

/// <summary>
///     Defines application commands for managing
///     driver subscriptions.
/// </summary>
public interface ISubscriptionCommandService
{
    /// <summary>
    ///     Activates a subscription for a driver.
    /// </summary>
    /// <param name="command">The activation command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the activation operation.</returns>
    Task<Result<SubscriptionActivation>> Handle(
        ActivateSubscriptionCommand command,
        CancellationToken cancellationToken = default);
}