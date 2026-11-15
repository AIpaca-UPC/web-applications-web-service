using System.ComponentModel.DataAnnotations;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

/// <summary>
///     Request resource for activating a driver subscription.
/// </summary>
/// <param name="DriverProfileId">
///     The driver profile identifier.
/// </param>
/// <param name="PlanId">The selected plan identifier.</param>
/// <param name="StartedAt">The subscription start date.</param>
public record ActivateSubscriptionResource(
    [Required]
    [StringLength(100, MinimumLength = 1)]
    string DriverProfileId,

    [Range(1, int.MaxValue)]
    int PlanId,

    DateOnly StartedAt
);