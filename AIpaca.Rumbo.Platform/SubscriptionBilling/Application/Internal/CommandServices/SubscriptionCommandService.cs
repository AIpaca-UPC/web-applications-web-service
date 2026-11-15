
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
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
    
    /// <summary>
    ///     Returns the current date in the America/Lima time zone.
    /// </summary>
    /// <returns>The current date in Lima.</returns>
    private static DateOnly TodayInLima()
    {
        var zone = TimeZoneInfo.FindSystemTimeZoneById(
            "America/Lima");

        var localNow = TimeZoneInfo.ConvertTime(
            DateTimeOffset.UtcNow, zone);

        return DateOnly.FromDateTime(localNow.Date);
    }

    /// <summary>
    ///     Pauses or resumes an existing subscription.
    /// </summary>
    /// <param name="command">The status change command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated subscription or a domain error.</returns>
    public async Task<Result<Subscription>> Handle(
        ChangeSubscriptionStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.SubscriptionId.Value <= 0 ||
            string.IsNullOrWhiteSpace(command.DriverProfileId) ||
            command.DriverProfileId.Trim().Length > 100 ||
            command.Status is not (
                ESubscriptionStatus.Active or ESubscriptionStatus.Paused))
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.InvalidSubscriptionData,
                "Valid subscription, driver and status are required.");
        }

        var subscription = await subscriptionRepository.FindByIdAsync(
            command.SubscriptionId, cancellationToken);

        if (subscription is null ||
            subscription.DriverProfileId != command.DriverProfileId.Trim())
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.SubscriptionNotFound,
                "The requested subscription was not found.");
        }

        var today = TodayInLima();

        if (subscription.StatusOn(today) == command.Status)
            return Result<Subscription>.Success(subscription);

        try
        {
            if (command.Status == ESubscriptionStatus.Paused)
            {
                subscription.Pause(today, DateTimeOffset.UtcNow);
            }
            else
            {
                subscription.Resume(today);
            }
        }
        catch (InvalidOperationException exception)
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.SubscriptionStatusConflict,
                exception.Message);
        }

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Subscription>.Success(subscription);
    }

    /// <summary>
    ///     Cancels an existing subscription while retaining
    ///     its historical information.
    /// </summary>
    /// <param name="command">The cancellation command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The cancelled subscription or a domain error.</returns>
    public async Task<Result<Subscription>> Handle(
        CancelSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.SubscriptionId.Value <= 0 ||
            string.IsNullOrWhiteSpace(command.DriverProfileId) ||
            command.DriverProfileId.Trim().Length > 100)
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.InvalidSubscriptionData,
                "Valid subscription and driver identifiers are required.");
        }

        var subscription = await subscriptionRepository.FindByIdAsync(
            command.SubscriptionId, cancellationToken);

        if (subscription is null ||
            subscription.DriverProfileId != command.DriverProfileId.Trim())
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.SubscriptionNotFound,
                "The requested subscription was not found.");
        }

        if (subscription.Status == ESubscriptionStatus.Cancelled)
            return Result<Subscription>.Success(subscription);

        try
        {
            subscription.Cancel(TodayInLima(), DateTimeOffset.UtcNow);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.SubscriptionStatusConflict,
                exception.Message);
        }

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Subscription>.Success(subscription);
    }

    /// <summary>
    ///     Changes the plan of a current subscription.
    ///     Cancels the previous subscription and creates
    ///     a replacement while preserving subscription history.
    /// </summary>
    /// <param name="command">The change plan command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     The newly created subscription or a business error.
    /// </returns>
    public async Task<Result<Subscription>> Handle(
        ChangeSubscriptionPlanCommand command,
        CancellationToken cancellationToken = default)
    {
        // Validate the requested identifiers and status.
        if (command.SubscriptionId.Value <= 0 ||
            command.NewPlanId.Value <= 0 ||
            string.IsNullOrWhiteSpace(command.DriverProfileId) ||
            command.DriverProfileId.Trim().Length > 100 ||
            command.RequestedStatus is not (
                ESubscriptionStatus.Active or ESubscriptionStatus.Paused))
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.InvalidSubscriptionData,
                "Valid subscription, driver, plan and status are required.");
        }

        var driverProfileId = command.DriverProfileId.Trim();
        var today = TodayInLima();

        // Retrieve the current subscription with change tracking.
        var currentSubscription =
            await subscriptionRepository.FindByIdAsync(
                command.SubscriptionId,
                cancellationToken);

        if (currentSubscription is null ||
            currentSubscription.DriverProfileId != driverProfileId)
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.SubscriptionNotFound,
                "The requested subscription was not found.");
        }

        // Only current active or paused subscriptions can change plans.
        var currentStatus = currentSubscription.StatusOn(today);

        if (currentStatus is not (
                ESubscriptionStatus.Active or ESubscriptionStatus.Paused))
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.SubscriptionStatusConflict,
                "Only active or paused subscriptions can change plans.");
        }

        // Changing to the same plan is not a plan transition.
        if (currentSubscription.PlanId == command.NewPlanId)
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.InvalidSubscriptionData,
                "The selected plan is already assigned to this subscription.");
        }

        // Verify that no other current subscription exists.
        var existingCurrent = await subscriptionRepository
            .FindCurrentByDriverProfileIdAsync(
                driverProfileId,
                today,
                cancellationToken);

        if (existingCurrent is not null &&
            existingCurrent.Id != currentSubscription.Id)
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.SubscriptionAlreadyExists,
                "The driver has another current subscription.");
        }

        // Retrieve and validate the new plan.
        var newPlan = await planRepository.FindByIdAsync(
            command.NewPlanId,
            cancellationToken);

        if (newPlan is null)
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.PlanNotFound,
                $"No plan was found with ID {command.NewPlanId.Value}.");
        }

        if (!newPlan.IsAvailable ||
            newPlan.BillingPeriodMonths is not (>= 1 and <= 120))
        {
            return Result<Subscription>.Failure(
                SubscriptionsError.PlanUnavailable,
                "The selected plan is not available for subscription.");
        }

        // The replacement begins a new billing period today.
        var utcNow = DateTimeOffset.UtcNow;

        var activationCommand = new ActivateSubscriptionCommand(
            driverProfileId,
            newPlan.Id,
            today);

        var newSubscription = new Subscription(
            activationCommand,
            newPlan,
            utcNow);

        // Apply the status selected by the driver.
        if (command.RequestedStatus == ESubscriptionStatus.Paused)
        {
            newSubscription.Pause(today, utcNow);
        }

        // Preserve the previous subscription as a cancelled record.
        currentSubscription.Cancel(today, utcNow);

        // Register both changes in the same unit of work.
        await subscriptionRepository.AddAsync(
            newSubscription,
            cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Subscription>.Success(newSubscription);
    }

}
