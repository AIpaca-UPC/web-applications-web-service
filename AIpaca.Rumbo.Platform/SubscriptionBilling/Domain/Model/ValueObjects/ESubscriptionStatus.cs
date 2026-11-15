namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a driver subscription
///     within the Subscription & Billing bounded context.
/// </summary>
public enum ESubscriptionStatus
{
    /// <summary>
    ///     The subscription has not started yet.
    /// </summary>
    Pending = 0,

    /// <summary>
    ///     The subscription is currently active.
    /// </summary>
    Active = 1,

    /// <summary>
    ///     The subscription has been temporarily paused.
    /// </summary>
    Paused = 2,

    /// <summary>
    ///     The subscription billing period has expired.
    /// </summary>
    Expired = 3,

    /// <summary>
    ///     The subscription has been cancelled.
    /// </summary>
    Cancelled = 4
}