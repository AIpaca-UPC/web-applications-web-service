using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;

/// <summary>
///     Command to cancel a driver's subscription
///     while preserving its history.
/// </summary>
/// <param name="SubscriptionId">
///     The subscription identifier.
/// </param>
/// <param name="DriverProfileId">
///     The subscribing driver profile identifier.
/// </param>
public record CancelSubscriptionCommand(
    SubscriptionId SubscriptionId,
    string DriverProfileId
);