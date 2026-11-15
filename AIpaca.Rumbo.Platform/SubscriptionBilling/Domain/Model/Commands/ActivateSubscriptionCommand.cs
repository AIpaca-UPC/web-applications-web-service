using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;

/// <summary>
///     Command to activate a subscription for a driver
///     using a selected subscription plan.
/// </summary>
/// <param name="DriverProfileId">
///     The identifier of the subscribing driver profile.
/// </param>
/// <param name="PlanId">
///     The identifier of the selected plan.
/// </param>
/// <param name="StartedAt">
///     The date when the subscription begins.
/// </param>
public record ActivateSubscriptionCommand(
    string DriverProfileId,
    PlanId PlanId,
    DateOnly StartedAt
);