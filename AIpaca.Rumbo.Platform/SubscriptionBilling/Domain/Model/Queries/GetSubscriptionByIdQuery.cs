namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a subscription by its identifier.
/// </summary>
/// <param name="SubscriptionId">
///     The unique subscription identifier.
/// </param>
public record GetSubscriptionByIdQuery(
    int SubscriptionId
);