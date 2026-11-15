using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.Internal.QueryServices;

/// <summary>
/// Default implementation of the <see cref="INotificationQueryService"/> contract.
/// </summary>
/// <param name="notificationRepository">The repository for accessing notification data.</param>
public class NotificationQueryService(INotificationRepository notificationRepository)
    : INotificationQueryService
{
    /// <summary>
    /// Handles the <see cref="GetAllNotificationsQuery"/> query.
    /// </summary>
    public async Task<IEnumerable<Notification>> Handle(
        GetAllNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        return await notificationRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    /// <summary>
    /// Handles the <see cref="GetNotificationsByProfileIdQuery"/> query.
    /// </summary>
    public async Task<IEnumerable<Notification>> Handle(
        GetNotificationsByProfileIdQuery query,
        CancellationToken cancellationToken)
    {
        return await notificationRepository.FindByProfileIdAsync(
            new ProfileId(query.ProfileId), cancellationToken);
    }

    /// <summary>
    /// Handles the <see cref="GetNotificationsByStudentIdQuery"/> query.
    /// </summary>
    public async Task<IEnumerable<Notification>> Handle(
        GetNotificationsByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        return await notificationRepository.FindByStudentIdAsync(
            new StudentId(query.StudentId), cancellationToken);
    }
}