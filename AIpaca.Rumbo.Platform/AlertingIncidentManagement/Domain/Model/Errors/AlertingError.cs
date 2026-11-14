namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Errors;

public enum AlertingError
{
    NotificationNotFound,
    InvalidNotificationData,
    NotificationAlreadyRead,
    NotificationRecipientNotFound,
    NotificationCreationFailed,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}