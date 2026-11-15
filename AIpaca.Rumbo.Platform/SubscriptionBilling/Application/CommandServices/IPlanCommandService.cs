using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;

/// <summary>
///     Defines the application contract for plan commands.
/// </summary>
public interface IPlanCommandService
{
    /// <summary>
    ///     Handles the creation of a subscription plan.
    /// </summary>
    /// <param name="command">The creation command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result containing the created plan.</returns>
    Task<Result<Plan>> Handle(
        CreatePlanCommand command,
        CancellationToken cancellationToken = default);
}