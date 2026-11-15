using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.Internal.CommandServices;

/// <summary>
///     Implements commands for managing subscription plans.
/// </summary>
/// <param name="planRepository">The plan repository.</param>
/// <param name="unitOfWork">The shared unit of work.</param>
public class PlanCommandService(
    IPlanRepository planRepository,
    IUnitOfWork unitOfWork) : IPlanCommandService
{
    /// <summary>
    ///     Validates and creates a subscription plan.
    /// </summary>
    /// <param name="command">The creation command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the creation operation.</returns>
    public async Task<Result<Plan>> Handle(
        CreatePlanCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name) ||
            command.Name.Trim().Length > 100 ||
            command.Description?.Length > 500 ||
            command.ReferencePrice < 0 ||
            command.ReferencePrice > 99999999.99m ||
            decimal.Round(command.ReferencePrice, 2) != command.ReferencePrice ||
            string.IsNullOrWhiteSpace(command.Currency) ||
            command.Currency.Trim().Length != 3 ||
            !command.Currency.Trim().All(char.IsAsciiLetter) ||
            command.BillingPeriodMonths is <= 0 ||
            command.VehicleLimit is <= 0)
        {
            return Result<Plan>.Failure(
                PlansError.InvalidPlanData,
                "The supplied plan information is invalid.");
        }

        var normalizedName = command.Name.Trim();

        if (await planRepository.ExistsByNameAsync(
                normalizedName, cancellationToken))
        {
            return Result<Plan>.Failure(
                PlansError.PlanAlreadyExists,
                $"A plan named '{normalizedName}' already exists.");
        }

        var plan = new Plan(command);

        await planRepository.AddAsync(plan, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Plan>.Success(plan);
    }
}