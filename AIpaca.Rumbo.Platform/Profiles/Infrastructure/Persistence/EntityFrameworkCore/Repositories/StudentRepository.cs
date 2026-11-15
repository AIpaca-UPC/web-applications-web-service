using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Entities;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StudentRepository(AppDbContext context)
: BaseRepository<Student>(context), IStudentRepository
{
    public async Task<Student?> FindByIdAsync(StudentId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Student>()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Student?> FindByIdAsNoTrackingAsync(StudentId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Student>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Student>> FindAllByIdsAsync(IEnumerable<StudentId> ids, CancellationToken cancellationToken)
    {
        var studentIds = ids.ToList();
        return await Context.Set<Student>()
            .AsNoTracking()
            .Where(s => studentIds.Contains(s.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Student>> FindAllByAuthorizedTutorProfileIdAsync(ProfileId tutorId, CancellationToken cancellationToken)
    {
        var relationships = Context.Set<TutorStudentRelationship>();
        return await Context.Set<Student>()
            .AsNoTracking()
            .Where(s => relationships.Any(r => r.StudentId == s.Id &&
                                               r.TutorProfileId == tutorId &&
                                               r.AuthorizationStatus == EAuthorizationStatus.Authorized))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Student>> FindAllByTutorProfileIdAsync(ProfileId tutorId, CancellationToken cancellationToken)
    {
        var relationships = Context.Set<TutorStudentRelationship>();
        return await Context.Set<Student>()
            .AsNoTracking()
            .Where(s => relationships.Any(r => r.StudentId == s.Id && r.TutorProfileId == tutorId))
            .ToListAsync(cancellationToken);
    }
}
