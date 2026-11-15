namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;

/// <summary>
///     Query to retrieve subscriptions, optionally filtered
///     by driver and cancellation status.
/// </summary>
/// <param name="DriverProfileId">
///     Optional driver profile identifier.
/// </param>
/// <param name="IncludeCancelled">
///     Indicates whether cancelled subscriptions are included.
/// </param>
public record GetAllSubscriptionsQuery(
    string? DriverProfileId = null,
    bool IncludeCancelled = false
);