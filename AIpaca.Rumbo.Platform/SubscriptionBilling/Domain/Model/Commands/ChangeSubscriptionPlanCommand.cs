using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;

/// <summary>
///     Command to change the plan of an existing subscription
///     while preserving the previous subscription history.
/// </summary>
/// <param name="SubscriptionId">
///     The identifier of the current subscription.
/// </param>
/// <param name="DriverProfileId">
///     The identifier of the subscribing driver.
/// </param>
/// <param name="NewPlanId">
///     The identifier of the new subscription plan.
/// </param>
/// <param name="RequestedStatus">
///     The requested status for the new subscription.
/// </param>
public record ChangeSubscriptionPlanCommand(
    SubscriptionId SubscriptionId,
    string DriverProfileId,
    PlanId NewPlanId,
    ESubscriptionStatus RequestedStatus
);