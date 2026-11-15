
using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;

/// <summary>
///     Represents a subscription plan and its commercial
///     conditions within the Subscription & Billing context.
/// </summary>
public class Plan : AggregateRoot
{
    /// <summary>
    ///     Initializes a plan for persistence reconstruction.
    /// </summary>
    public Plan()
    {
        Name = string.Empty;
        Description = string.Empty;
        Currency = "PEN";
    }

    /// <summary>
    ///     Creates a subscription plan from a command.
    /// </summary>
    /// <param name="command">
    ///     The command containing the plan information.
    /// </param>
    public Plan(CreatePlanCommand command) : this()
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Plan name is required.");

        if (command.ReferencePrice < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (string.IsNullOrWhiteSpace(command.Currency))
            throw new ArgumentException("Currency is required.");

        if (command.BillingPeriodMonths is <= 0)
            throw new ArgumentException("Billing period must be positive.");

        if (command.VehicleLimit is <= 0)
            throw new ArgumentException("Vehicle limit must be positive.");

        Name = command.Name.Trim();
        Description = command.Description ?? string.Empty;
        ReferencePrice = command.ReferencePrice;
        Currency = command.Currency.Trim().ToUpperInvariant();
        BillingPeriodMonths = command.BillingPeriodMonths;
        VehicleLimit = command.VehicleLimit;
        Active = command.Active;
    }

    /// <summary>
    ///     Gets the unique plan identifier.
    /// </summary>
    public PlanId Id { get; private set; }

    /// <summary>
    ///     Gets the commercial name of the plan.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the plan description.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    ///     Gets the reference price of the plan.
    /// </summary>
    public decimal ReferencePrice { get; private set; }

    /// <summary>
    ///     Gets the currency code of the plan.
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    ///     Gets the billing period in months.
    /// </summary>
    public int? BillingPeriodMonths { get; private set; }

    /// <summary>
    ///     Gets the maximum number of allowed vehicles.
    /// </summary>
    public int? VehicleLimit { get; private set; }

    /// <summary>
    ///     Indicates whether the plan is active.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    ///     Indicates whether the plan can be selected
    ///     to activate a subscription.
    /// </summary>
    public bool IsAvailable =>
        Active && BillingPeriodMonths is > 0;
}
