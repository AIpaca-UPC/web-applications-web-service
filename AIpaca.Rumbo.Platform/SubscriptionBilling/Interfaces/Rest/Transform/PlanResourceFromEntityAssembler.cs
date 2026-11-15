using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Interfaces.Rest.Transform;

/// <summary>
///     Converts a Plan aggregate into a REST response resource.
/// </summary>
public static class PlanResourceFromEntityAssembler
{
    /// <summary>
    ///     Creates a response resource from a plan aggregate.
    /// </summary>
    /// <param name="entity">The plan aggregate.</param>
    /// <returns>The plan response resource.</returns>
    public static PlanResource ToResourceFromEntity(Plan entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new PlanResource(
            entity.Id.Value,
            entity.Name,
            entity.Description,
            entity.ReferencePrice,
            entity.Currency,
            entity.BillingPeriodMonths,
            entity.VehicleLimit,
            entity.Active);
    }
}