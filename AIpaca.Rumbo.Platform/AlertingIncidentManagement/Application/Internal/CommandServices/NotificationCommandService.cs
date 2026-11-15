using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.Internal.CommandServices;

/// <summary>
/// Default implementation of the <see cref="INotificationCommandService"/> contract.
/// </summary>
/// <param name="notificationRepository">The notification repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
public class NotificationCommandService(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : INotificationCommandService
{
    /// <summary>
    /// Handles the <see cref="CreateNotificationCommand"/> command.
    /// </summary>
    public async Task<Result<Notification>> Handle(
        CreateNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var notification = new Notification(command);

        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Notification>.Success(notification);
    }

    /// <summary>
    /// Handles the <see cref="DeleteNotificationCommand"/> command.
    /// </summary>
    public async Task<Result<Notification>> Handle(
        DeleteNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.FindByIdAsync(
            new NotificationId(command.Id), cancellationToken);

        if (notification is null)
            return Result<Notification>.Failure(
                AlertingError.NotificationNotFound,
                $"No notification was found with identifier {command.Id}.");

        notificationRepository.Remove(notification);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Notification>.Success(notification);
    }

    /// <summary>
    /// Handles the <see cref="MarkNotificationAsReadCommand"/> command.
    /// </summary>
    public async Task<Result<Notification>> Handle(
        MarkNotificationAsReadCommand command,
        CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.FindByIdAsync(
            new NotificationId(command.NotificationId), cancellationToken);

        if (notification is null)
            return Result<Notification>.Failure(
                AlertingError.NotificationNotFound,
                $"No notification was found with identifier {command.NotificationId}.");

        notification.MarkAsRead();

        notificationRepository.Update(notification);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Notification>.Success(notification);
    }
}