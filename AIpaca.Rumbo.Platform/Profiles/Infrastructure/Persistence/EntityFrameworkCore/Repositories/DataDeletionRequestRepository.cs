using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class DataDeletionRequestRepository(AppDbContext context)
: BaseRepository<DataDeletionRequest>(context), IDataDeletionRequestRepository
{
    public async Task<DataDeletionRequest?> FindByIdAsync(DataDeletionRequestId id, CancellationToken cancellationToken)
    {
        return await Context.Set<DataDeletionRequest>()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<DataDeletionRequest?> FindByIdAsNoTrackingAsync(DataDeletionRequestId id, CancellationToken cancellationToken)
    {
        return await Context.Set<DataDeletionRequest>()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DataDeletionRequest>> FindAllByRequestedByProfileIdAsync(ProfileId requesterId, CancellationToken cancellationToken)
    {
        return await Context.Set<DataDeletionRequest>()
            .AsNoTracking()
            .Where(r => r.RequestedByTutorId == requesterId)
            .ToListAsync(cancellationToken);
    }
}
