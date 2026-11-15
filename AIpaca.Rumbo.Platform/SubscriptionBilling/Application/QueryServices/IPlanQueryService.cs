using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Queries;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.QueryServices;

/// <summary>
///     Defines the application contract for plan queries.
/// </summary>
public interface IPlanQueryService
{
    /// <summary>
    ///     Retrieves all subscription plans.
    /// </summary>
    /// <param name="query">The catalog query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription plan collection.</returns>
    Task<IEnumerable<Plan>> Handle(
        GetAllPlansQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a subscription plan by its identifier.
    /// </summary>
    /// <param name="query">The plan identifier query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The plan if found; otherwise, null.</returns>
    Task<Plan?> Handle(
        GetPlanByIdQuery query,
        CancellationToken cancellationToken = default);
}