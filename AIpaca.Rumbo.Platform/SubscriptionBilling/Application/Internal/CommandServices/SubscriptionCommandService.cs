
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Application.Internal.CommandServices;

/// <summary>
///     Implements subscription commands using repositories
///     and the shared unit of work.
/// </summary>
/// <param name="planRepository">The plan repository.</param>
/// <param name="subscriptionRepository">The subscription repository.</param>
/// <param name="unitOfWork">The shared unit of work.</param>
public class SubscriptionCommandService(
    IPlanRepository planRepository,
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork) : ISubscriptionCommandService
{
    /// <summary>
    ///     Activates a subscription using a selected plan.
    ///     Prevents duplicate current subscriptions
    ///     and preserves existing subscription history.
    /// </summary>
    /// <param name="command">The activation command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The activation result.</returns>
    public async Task<Result<SubscriptionActivation>> Handle(
        ActivateSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.DriverProfileId) ||
            command.DriverProfileId.Trim().Length > 100 ||
            command.PlanId.Value <= 0 ||
            command.StartedAt == default)
        {
            return Result<SubscriptionActivation>.Failure(
                SubscriptionsError.InvalidSubscriptionData,
                "Valid driver, plan and start date are required.");
        }

        var utcNow = DateTimeOffset.UtcNow;
        var limaZone = TimeZoneInfo.FindSystemTimeZoneById(
            "America/Lima");

        var today = DateOnly.FromDateTime(
            TimeZoneInfo.ConvertTime(utcNow, limaZone).Date);

        if (command.StartedAt > today)
        {
            return Result<SubscriptionActivation>.Failure(
                SubscriptionsError.InvalidSubscriptionData,
                "The subscription cannot start in the future.");
        }

        var driverProfileId = command.DriverProfileId.Trim();

        var current = await subscriptionRepository
            .FindCurrentByDriverProfileIdAsync(
                driverProfileId,
                today,
                cancellationToken);

        if (current is not null)
        {
            if (current.PlanId == command.PlanId)
            {
                return Result<SubscriptionActivation>.Success(
                    new SubscriptionActivation(current, false));
            }

            return Result<SubscriptionActivation>.Failure(
                SubscriptionsError.SubscriptionAlreadyExists,
                "Change or cancel the current subscription before activating another plan.");
        }

        var plan = await planRepository.FindByIdAsync(
            command.PlanId,
            cancellationToken);

        if (plan is null)
        {
            return Result<SubscriptionActivation>.Failure(
                SubscriptionsError.PlanNotFound,
                $"No plan was found with ID {command.PlanId.Value}.");
        }

        if (!plan.IsAvailable ||
            plan.BillingPeriodMonths is not (>= 1 and <= 120))
        {
            return Result<SubscriptionActivation>.Failure(
                SubscriptionsError.PlanUnavailable,
                "The selected plan requires a valid billing period.");
        }

        var endDate = command.StartedAt.AddMonths(
            plan.BillingPeriodMonths.Value);

        if (endDate <= today)
        {
            return Result<SubscriptionActivation>.Failure(
                SubscriptionsError.InvalidSubscriptionData,
                "The selected start date would create an expired subscription.");
        }

        var normalizedCommand = command with
        {
            DriverProfileId = driverProfileId
        };

        var subscription = new Subscription(
            normalizedCommand,
            plan,
            utcNow);

        await subscriptionRepository.AddAsync(
            subscription,
            cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<SubscriptionActivation>.Success(
            new SubscriptionActivation(subscription, true));
    }
}
