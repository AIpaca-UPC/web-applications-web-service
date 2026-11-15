namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;

/// <summary>
///     Query to retrieve subscriptions, optionally
///     filtered by a driver profile identifier.
/// </summary>
/// <param name="DriverProfileId">
///     Optional driver profile identifier.
/// </param>
public record GetAllSubscriptionsQuery(
    string? DriverProfileId = null
);