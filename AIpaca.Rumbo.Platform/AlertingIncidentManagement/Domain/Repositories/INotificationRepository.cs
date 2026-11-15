using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<IEnumerable<Notification>> FindByProfileIdAsync(ProfileId profileId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Notification>> FindByStudentIdAsync(StudentId studentId, CancellationToken cancellationToken = default);
}