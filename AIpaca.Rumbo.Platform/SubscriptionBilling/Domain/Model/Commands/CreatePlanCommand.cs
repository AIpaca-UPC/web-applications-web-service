namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;

/// <summary>
///     Command to create a new subscription plan
///     with its commercial conditions.
/// </summary>
/// <param name="Name">The name of the plan.</param>
/// <param name="Description">The plan description.</param>
/// <param name="ReferencePrice">The reference price.</param>
/// <param name="Currency">The currency code.</param>
/// <param name="BillingPeriodMonths">
///     The billing period expressed in months.
/// </param>
/// <param name="VehicleLimit">
///     The maximum number of vehicles allowed.
/// </param>
/// <param name="Active">
///     Indicates whether the plan is available.
/// </param>
public record CreatePlanCommand(
    string Name,
    string Description,
    decimal ReferencePrice,
    string Currency,
    int? BillingPeriodMonths,
    int? VehicleLimit,
    bool Active
);