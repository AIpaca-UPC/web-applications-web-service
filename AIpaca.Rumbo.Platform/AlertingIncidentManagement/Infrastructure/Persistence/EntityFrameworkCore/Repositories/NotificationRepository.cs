
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Repository implementation for the Notification aggregate root, using Entity Framework Core as the underlying persistence mechanism.
/// </summary>
/// <param name="context">The database context.</param>
public class NotificationRepository(AppDbContext context) : BaseRepository<Notification>(context), INotificationRepository
{
    /// <summary>
    ///     Finds all notifications addressed to a specific profile.
    /// </summary>
    /// <param name="profileId">The identifier of the recipient profile.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A collection of notifications addressed to the profile.</returns>
    public async Task<IEnumerable<Notification>> FindByProfileIdAsync(
        ProfileId profileId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Notification>()
            .Where(notification => notification.RecipientProfileId == profileId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Finds all notifications associated with a specific student.
    /// </summary>
    /// <param name="studentId">The identifier of the student.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A collection of notifications associated with the student.</returns>
    public async Task<IEnumerable<Notification>> FindByStudentIdAsync(
        StudentId studentId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Notification>()
            .Where(notification => notification.StudentId == studentId)
            .ToListAsync(cancellationToken);
    }
}
