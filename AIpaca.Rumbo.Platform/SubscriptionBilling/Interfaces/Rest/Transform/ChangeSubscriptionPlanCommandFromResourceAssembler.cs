using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;

/// <summary>
///     Converts a subscription update resource
///     into a change plan command.
/// </summary>
public static class ChangeSubscriptionPlanCommandFromResourceAssembler
{
    /// <summary>
    ///     Creates a command using the subscription identifier
    ///     and the requested plan information.
    /// </summary>
    /// <param name="subscriptionId">
    ///     The subscription identifier from the route.
    /// </param>
    /// <param name="resource">
    ///     The subscription update resource.
    /// </param>
    /// <returns>
    ///     The command when the status is supported;
    ///     otherwise, null.
    /// </returns>
    public static ChangeSubscriptionPlanCommand? ToCommandFromResource(
        int subscriptionId,
        UpdateSubscriptionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        ESubscriptionStatus? status = resource.Status?
                .Trim()
                .ToUpperInvariant() switch
            {
                "ACTIVE" => ESubscriptionStatus.Active,
                "PAUSED" => ESubscriptionStatus.Paused,
                _ => null
            };

        if (status is null)
            return null;

        return new ChangeSubscriptionPlanCommand(
            new SubscriptionId(subscriptionId),
            resource.DriverProfileId,
            new PlanId(resource.PlanId),
            status.Value);
    }
}