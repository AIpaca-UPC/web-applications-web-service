using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier of a subscription
///     in the Subscription & Billing bounded context.
/// </summary>
/// <param name="Value">The underlying integer identifier.</param>
public readonly record struct SubscriptionId(int Value) : IEntityId<int>
{
    /// <summary>
    ///     Converts a SubscriptionId into its integer value.
    /// </summary>
    public static implicit operator int(SubscriptionId id) => id.Value;

    /// <summary>
    ///     Converts an integer into a SubscriptionId.
    /// </summary>
    public static implicit operator SubscriptionId(int value) => new(value);

    /// <summary>
    ///     Returns the subscription identifier as a string.
    /// </summary>
    public override string ToString() => Value.ToString();
}