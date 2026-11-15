using System.ComponentModel.DataAnnotations;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

/// <summary>
///     Request resource for creating a subscription plan.
/// </summary>
/// <param name="Name">The plan name.</param>
/// <param name="Description">The plan description.</param>
/// <param name="ReferencePrice">The reference price.</param>
/// <param name="Currency">The currency code.</param>
/// <param name="BillingPeriodMonths">The billing period in months.</param>
/// <param name="VehicleLimit">The maximum number of vehicles.</param>
/// <param name="Active">Whether the plan is active.</param>
public record CreatePlanResource(
    [Required]
    [StringLength(100, MinimumLength = 1)]
    string Name,

    [StringLength(500)]
    string? Description,

    [Range(typeof(decimal), "0", "99999999.99")]
    decimal ReferencePrice,

    [Required]
    [RegularExpression("^[A-Za-z]{3}$")]
    string Currency,

    [Range(1, int.MaxValue)]
    int? BillingPeriodMonths,

    [Range(1, int.MaxValue)]
    int? VehicleLimit,

    bool Active
);