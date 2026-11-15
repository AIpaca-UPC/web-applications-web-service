using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Queries;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.QueryServices;

/// <summary>
/// Handles the queries of the Alerting & Incident Management bounded context.
/// </summary>
public interface INotificationQueryService
{
    Task<IEnumerable<Notification>> Handle(GetAllNotificationsQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<Notification>> Handle(GetNotificationsByProfileIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<Notification>> Handle(GetNotificationsByStudentIdQuery query, CancellationToken cancellationToken);
}