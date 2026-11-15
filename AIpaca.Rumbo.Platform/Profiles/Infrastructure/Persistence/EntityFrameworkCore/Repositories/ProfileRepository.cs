using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProfileRepository(AppDbContext context)
: BaseRepository<Profile>(context), IProfileRepository
{
    public async Task<Profile?> FindByIdAsync(ProfileId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Profile>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Profile?> FindByIdAsNoTrackingAsync(ProfileId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Profile>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Profile?> FindByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>()
            .FirstOrDefaultAsync(p => p.AccountId == accountId, cancellationToken);
    }

    public async Task<bool> ExistsByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>()
            .AnyAsync(p => p.AccountId == accountId, cancellationToken);
    }
}
