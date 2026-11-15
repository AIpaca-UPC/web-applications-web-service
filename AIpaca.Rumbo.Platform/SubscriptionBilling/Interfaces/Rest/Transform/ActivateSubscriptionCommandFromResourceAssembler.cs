using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;

/// <summary>
///     Transforms a subscription activation resource
///     into an application command.
/// </summary>
public static class ActivateSubscriptionCommandFromResourceAssembler
{
    /// <summary>
    ///     Converts the incoming resource into a command.
    /// </summary>
    /// <param name="resource">The activation request.</param>
    /// <returns>The subscription activation command.</returns>
    public static ActivateSubscriptionCommand ToCommandFromResource(
        ActivateSubscriptionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new ActivateSubscriptionCommand(
            resource.DriverProfileId,
            new PlanId(resource.PlanId),
            resource.StartedAt);
    }
}