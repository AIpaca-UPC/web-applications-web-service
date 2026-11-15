using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;

/// <summary>
///     Converts a REST resource into a command
///     for changing a subscription status.
/// </summary>
public static class ChangeSubscriptionStatusCommandFromResourceAssembler
{
    /// <summary>
    ///     Creates a status change command from an HTTP request.
    /// </summary>
    /// <param name="subscriptionId">
    ///     The subscription identifier from the route.
    /// </param>
    /// <param name="resource">
    ///     The subscription update resource.
    /// </param>
    /// <returns>
    ///     The command when the status is valid; otherwise, null.
    /// </returns>
    public static ChangeSubscriptionStatusCommand? ToCommandFromResource(
        int subscriptionId,
        UpdateSubscriptionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        if (string.IsNullOrWhiteSpace(resource.Status) ||
            !Enum.TryParse<ESubscriptionStatus>(
                resource.Status,
                true,
                out var status))
        {
            return null;
        }

        if (status is not (
            ESubscriptionStatus.Active or ESubscriptionStatus.Paused))
        {
            return null;
        }

        return new ChangeSubscriptionStatusCommand(
            new SubscriptionId(subscriptionId),
            resource.DriverProfileId,
            status);
    }
}