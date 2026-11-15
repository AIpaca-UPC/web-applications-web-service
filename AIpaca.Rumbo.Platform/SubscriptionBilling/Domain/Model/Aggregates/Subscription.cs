using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;

/// <summary>
///     Represents a driver's subscription to a selected plan.
///     Encapsulates subscription lifecycle business rules.
/// </summary>
public class Subscription : AggregateRoot
{
    /// <summary>
    ///     Initializes a subscription for persistence reconstruction.
    /// </summary>
    public Subscription()
    {
        DriverProfileId = string.Empty;
    }

    /// <summary>
    ///     Creates an active subscription from a validated command.
    /// </summary>
    /// <param name="command">The activation command.</param>
    /// <param name="plan">The selected subscription plan.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    public Subscription(
        ActivateSubscriptionCommand command,
        Plan plan,
        DateTimeOffset createdAt) : this()
    {
        ArgumentNullException.ThrowIfNull(plan);

        if (string.IsNullOrWhiteSpace(command.DriverProfileId))
            throw new ArgumentException("Driver profile ID is required.");

        if (command.PlanId.Value <= 0 || command.PlanId != plan.Id)
            throw new ArgumentException("Invalid plan ID.");

        if (command.StartedAt == default)
            throw new ArgumentException("Start date is required.");

        if (!plan.IsAvailable)
            throw new InvalidOperationException(
                "The selected plan is not available.");

        PlanId = command.PlanId;
        DriverProfileId = command.DriverProfileId.Trim();

        StartedAt = command.StartedAt;
        CurrentPeriodStart = command.StartedAt;
        CurrentPeriodEnd = command.StartedAt.AddMonths(
            plan.BillingPeriodMonths!.Value);

        VehicleLimit = plan.VehicleLimit;
        Status = ESubscriptionStatus.Active;
        AutoRenew = false;
        CreatedAt = createdAt;
    }

    /// <summary>
    ///     Gets the unique subscription identifier.
    /// </summary>
    public SubscriptionId Id { get; private set; }

    /// <summary>
    ///     Gets the selected plan identifier.
    /// </summary>
    public PlanId PlanId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the subscribing driver.
    /// </summary>
    public string DriverProfileId { get; private set; }

    /// <summary>
    ///     Gets the persisted subscription status.
    /// </summary>
    public ESubscriptionStatus Status { get; private set; }

    /// <summary>
    ///     Gets the subscription start date.
    /// </summary>
    public DateOnly StartedAt { get; private set; }

    /// <summary>
    ///     Gets the beginning of the current billing period.
    /// </summary>
    public DateOnly CurrentPeriodStart { get; private set; }

    /// <summary>
    ///     Gets the end of the current billing period.
    /// </summary>
    public DateOnly CurrentPeriodEnd { get; private set; }

    /// <summary>
    ///     Gets the vehicle limit provided by the plan.
    /// </summary>
    public int? VehicleLimit { get; private set; }

    /// <summary>
    ///     Indicates whether automatic renewal is enabled.
    /// </summary>
    public bool AutoRenew { get; private set; }

    /// <summary>
    ///     Gets the timestamp when the subscription was paused.
    /// </summary>
    public DateTimeOffset? PausedAt { get; private set; }

    /// <summary>
    ///     Gets the timestamp when the subscription was cancelled.
    /// </summary>
    public DateTimeOffset? CancelledAt { get; private set; }

    /// <summary>
    ///     Gets the subscription creation timestamp.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    ///     Determines the subscription lifecycle status
    ///     for a specified date.
    /// </summary>
    /// <param name="today">The date to evaluate.</param>
    /// <returns>The effective subscription status.</returns>
    public ESubscriptionStatus StatusOn(DateOnly today)
    {
        if (Status == ESubscriptionStatus.Cancelled)
            return ESubscriptionStatus.Cancelled;

        if (today < StartedAt)
            return ESubscriptionStatus.Pending;

        if (today >= CurrentPeriodEnd)
            return ESubscriptionStatus.Expired;

        return Status;
    }

    /// <summary>
    ///     Determines whether the subscription is active
    ///     on a specified date.
    /// </summary>
    /// <param name="today">The date to evaluate.</param>
    /// <returns>True when active; otherwise, false.</returns>
    public bool IsActiveOn(DateOnly today)
    {
        return StatusOn(today) == ESubscriptionStatus.Active;
    }

    /// <summary>
    ///     Pauses an active subscription.
    /// </summary>
    /// <param name="today">The current date.</param>
    /// <param name="now">The current timestamp.</param>
    public void Pause(DateOnly today, DateTimeOffset now)
    {
        if (!IsActiveOn(today))
            throw new InvalidOperationException(
                "Only active subscriptions can be paused.");

        Status = ESubscriptionStatus.Paused;
        PausedAt = now;
    }

    /// <summary>
    ///     Resumes a valid paused subscription.
    /// </summary>
    /// <param name="today">The current date.</param>
    public void Resume(DateOnly today)
    {
        if (StatusOn(today) != ESubscriptionStatus.Paused)
            throw new InvalidOperationException(
                "Only valid paused subscriptions can be resumed.");

        Status = ESubscriptionStatus.Active;
        PausedAt = null;
    }

    /// <summary>
    ///     Cancels an active or paused subscription
    ///     while preserving its historical information.
    /// </summary>
    /// <param name="today">The current date.</param>
    /// <param name="now">The cancellation timestamp.</param>
    public void Cancel(DateOnly today, DateTimeOffset now)
    {
        if (Status == ESubscriptionStatus.Cancelled)
            return;

        var currentStatus = StatusOn(today);

        if (currentStatus != ESubscriptionStatus.Active &&
            currentStatus != ESubscriptionStatus.Paused)
            throw new InvalidOperationException(
                "Only active or paused subscriptions can be cancelled.");

        Status = ESubscriptionStatus.Cancelled;
        CancelledAt = now;
        AutoRenew = false;
    }
}
