using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Commands;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.CommandServices;

/// <summary>
/// Handles the commands of the Alerting & Incident Management bounded context.
/// </summary>
public interface INotificationCommandService
{
    Task<Result<Notification>> Handle(CreateNotificationCommand command, CancellationToken cancellationToken);

    Task<Result<Notification>> Handle(DeleteNotificationCommand command, CancellationToken cancellationToken);

    Task<Result<Notification>> Handle(MarkNotificationAsReadCommand command, CancellationToken cancellationToken);
}