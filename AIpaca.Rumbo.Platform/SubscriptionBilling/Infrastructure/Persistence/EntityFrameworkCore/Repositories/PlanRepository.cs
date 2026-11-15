
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Repository implementation for subscription plans,
///     using Entity Framework Core for persistence.
/// </summary>
/// <param name="context">The application database context.</param>
public class PlanRepository(AppDbContext context)
    : BaseRepository<Plan>(context), IPlanRepository
{
    /// <summary>
    ///     Finds a subscription plan by its identifier.
    /// </summary>
    /// <param name="id">The plan identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The plan if found; otherwise, null.</returns>
    public async Task<Plan?> FindByIdAsync(
        PlanId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Plan>()
            .FirstOrDefaultAsync(
                plan => plan.Id == id,
                cancellationToken);
    }

    /// <summary>
    ///     Finds a subscription plan without change tracking.
    /// </summary>
    /// <param name="id">The plan identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The plan if found; otherwise, null.</returns>
    public async Task<Plan?> FindByIdAsNoTrackingAsync(
        PlanId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Plan>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                plan => plan.Id == id,
                cancellationToken);
    }

    /// <summary>
    ///     Checks whether a subscription plan
    ///     with the given name already exists.
    /// </summary>
    /// <param name="name">The plan name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the name exists; otherwise, false.</returns>
    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return await Context.Set<Plan>()
            .AnyAsync(
                plan => plan.Name == normalizedName,
                cancellationToken);
    }
}
