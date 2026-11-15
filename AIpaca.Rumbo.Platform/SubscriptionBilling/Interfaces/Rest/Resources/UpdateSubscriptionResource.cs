using System.ComponentModel.DataAnnotations;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

/// <summary>
///     Request resource for updating a driver's subscription.
/// </summary>
/// <param name="DriverProfileId">
///     The subscribing driver profile identifier.
/// </param>
/// <param name="PlanId">
///     The plan identifier selected by the client.
/// </param>
/// <param name="Status">
///     The requested subscription status.
/// </param>
public record UpdateSubscriptionResource(
    [Required]
    [StringLength(100, MinimumLength = 1)]
    string DriverProfileId,

    [Range(1, int.MaxValue)]
    int PlanId,

    [Required]
    [StringLength(20)]
    string Status
);