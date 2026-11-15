namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

/// <summary>
///     Represents a driver subscription returned by the REST API.
/// </summary>
/// <param name="Id">The subscription identifier.</param>
/// <param name="PlanId">The selected plan identifier.</param>
/// <param name="DriverProfileId">The driver profile identifier.</param>
/// <param name="Status">The effective subscription status.</param>
/// <param name="StartedAt">The subscription start date.</param>
/// <param name="CurrentPeriodStart">The current period start.</param>
/// <param name="CurrentPeriodEnd">The current period end.</param>
/// <param name="AutoRenew">Whether renewal is enabled.</param>
/// <param name="PausedAt">The pause timestamp.</param>
/// <param name="CancelledAt">The cancellation timestamp.</param>
/// <param name="CreatedAt">The creation timestamp.</param>
public record SubscriptionResource(
    int Id,
    int PlanId,
    string DriverProfileId,
    string Status,
    DateOnly StartedAt,
    DateOnly CurrentPeriodStart,
    DateOnly CurrentPeriodEnd,
    bool AutoRenew,
    DateTimeOffset? PausedAt,
    DateTimeOffset? CancelledAt,
    DateTimeOffset CreatedAt
);