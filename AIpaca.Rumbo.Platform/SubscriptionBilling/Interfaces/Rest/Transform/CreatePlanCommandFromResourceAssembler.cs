using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;

/// <summary>
///     Converts a REST request resource into a plan command.
/// </summary>
public static class CreatePlanCommandFromResourceAssembler
{
    /// <summary>
    ///     Creates a command from the supplied resource.
    /// </summary>
    /// <param name="resource">The creation resource.</param>
    /// <returns>The create plan command.</returns>
    public static CreatePlanCommand ToCommandFromResource(
        CreatePlanResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new CreatePlanCommand(
            resource.Name,
            resource.Description ?? string.Empty,
            resource.ReferencePrice,
            resource.Currency,
            resource.BillingPeriodMonths,
            resource.VehicleLimit,
            resource.Active);
    }
}