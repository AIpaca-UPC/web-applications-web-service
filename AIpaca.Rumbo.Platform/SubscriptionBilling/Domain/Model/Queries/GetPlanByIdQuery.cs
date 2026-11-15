namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a subscription plan by its identifier.
/// </summary>
/// <param name="PlanId">The unique plan identifier.</param>
public record GetPlanByIdQuery(int PlanId);