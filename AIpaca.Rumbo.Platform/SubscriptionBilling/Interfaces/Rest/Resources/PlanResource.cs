namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

/// <summary>
///     Represents a subscription plan returned by the REST API.
/// </summary>
/// <param name="Id">The plan identifier.</param>
/// <param name="Name">The plan name.</param>
/// <param name="Description">The plan description.</param>
/// <param name="ReferencePrice">The reference price.</param>
/// <param name="Currency">The currency code.</param>
/// <param name="BillingPeriodMonths">The billing period in months.</param>
/// <param name="VehicleLimit">The maximum number of vehicles.</param>
/// <param name="Active">Whether the plan is active.</param>
public record PlanResource(
    int Id,
    string Name,
    string Description,
    decimal ReferencePrice,
    string Currency,
    int? BillingPeriodMonths,
    int? VehicleLimit,
    bool Active
);