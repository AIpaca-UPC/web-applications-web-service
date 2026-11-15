
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Repository implementation for driver subscriptions,
///     using Entity Framework Core for persistence.
/// </summary>
/// <param name="context">The application database context.</param>
public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    /// <summary>
    ///     Finds a subscription by its identifier.
    /// </summary>
    /// <param name="id">The subscription identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription if found; otherwise, null.</returns>
    public async Task<Subscription?> FindByIdAsync(
        SubscriptionId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(
                subscription => subscription.Id == id,
                cancellationToken);
    }

    /// <summary>
    ///     Finds a subscription without change tracking.
    /// </summary>
    /// <param name="id">The subscription identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The subscription if found; otherwise, null.</returns>
    public async Task<Subscription?> FindByIdAsNoTrackingAsync(
        SubscriptionId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                subscription => subscription.Id == id,
                cancellationToken);
    }

    /// <summary>
    ///     Retrieves the subscription history of a driver,
    ///     ordered from newest to oldest.
    /// </summary>
    /// <param name="driverProfileId">The driver profile identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The driver's subscriptions.</returns>
    public async Task<IEnumerable<Subscription>>
        FindByDriverProfileIdAsNoTrackingAsync(
            string driverProfileId,
            CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .AsNoTracking()
            .Where(subscription =>
                subscription.DriverProfileId == driverProfileId)
            .OrderByDescending(subscription =>
                subscription.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Finds an active or paused subscription
    ///     within its valid billing period.
    /// </summary>
    /// <param name="driverProfileId">The driver profile identifier.</param>
    /// <param name="today">The current date.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The current subscription if found; otherwise, null.</returns>
    public async Task<Subscription?>
        FindCurrentByDriverProfileIdAsync(
            string driverProfileId,
            DateOnly today,
            CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(
                subscription =>
                    subscription.DriverProfileId == driverProfileId &&
                    subscription.StartedAt <= today &&
                    subscription.CurrentPeriodEnd > today &&
                    (subscription.Status == ESubscriptionStatus.Active ||
                     subscription.Status == ESubscriptionStatus.Paused),
                cancellationToken);
    }

    /// <summary>
    ///     Finds the most recent subscription
    ///     for a driver and a selected plan.
    /// </summary>
    /// <param name="driverProfileId">The driver profile identifier.</param>
    /// <param name="planId">The selected plan identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The latest subscription if found; otherwise, null.</returns>
    public async Task<Subscription?>
        FindLatestByDriverAndPlanAsync(
            string driverProfileId,
            PlanId planId,
            CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .AsNoTracking()
            .Where(subscription =>
                subscription.DriverProfileId == driverProfileId &&
                subscription.PlanId == planId)
            .OrderByDescending(subscription =>
                subscription.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
