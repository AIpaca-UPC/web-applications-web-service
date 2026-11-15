using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;

/// <summary>
///     Command to pause or resume a driver's subscription.
/// </summary>
/// <param name="SubscriptionId">
///     The subscription identifier.
/// </param>
/// <param name="DriverProfileId">
///     The subscribing driver profile identifier.
/// </param>
/// <param name="Status">
///     The requested subscription status.
/// </param>
public record ChangeSubscriptionStatusCommand(
    SubscriptionId SubscriptionId,
    string DriverProfileId,
    ESubscriptionStatus Status
);