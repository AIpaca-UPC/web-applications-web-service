using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;

/// <summary>
///     Converts Subscription aggregates into REST resources.
/// </summary>
public static class SubscriptionResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a subscription into its API representation.
    /// </summary>
    /// <param name="entity">The subscription aggregate.</param>
    /// <param name="today">The date used to evaluate its status.</param>
    /// <returns>The subscription response resource.</returns>
    public static SubscriptionResource ToResourceFromEntity(
        Subscription entity,
        DateOnly today)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new SubscriptionResource(
            entity.Id.Value,
            entity.PlanId.Value,
            entity.DriverProfileId,
            entity.StatusOn(today).ToString().ToUpperInvariant(),
            entity.StartedAt,
            entity.CurrentPeriodStart,
            entity.CurrentPeriodEnd,
            entity.AutoRenew,
            entity.PausedAt,
            entity.CancelledAt,
            entity.CreatedAt);
    }
}