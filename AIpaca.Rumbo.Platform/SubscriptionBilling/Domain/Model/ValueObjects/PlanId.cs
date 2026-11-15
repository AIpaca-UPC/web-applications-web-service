using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier of a subscription plan
///     in the Subscription & Billing bounded context.
/// </summary>
/// <param name="Value">The underlying integer identifier.</param>
public readonly record struct PlanId(int Value) : IEntityId<int>
{
    /// <summary>
    ///     Converts a PlanId into its integer value.
    /// </summary>
    public static implicit operator int(PlanId id) => id.Value;

    /// <summary>
    ///     Converts an integer into a PlanId.
    /// </summary>
    public static implicit operator PlanId(int value) => new(value);

    /// <summary>
    ///     Returns the plan identifier as a string.
    /// </summary>
    public override string ToString() => Value.ToString();
}