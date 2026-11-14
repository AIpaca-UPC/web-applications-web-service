using AIpaca.Rumbo.Platform.Shared.Domain.Model.Errors;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Errors;

public class AlertingErrors
{
    public static readonly Error NotificationCreationFailed = 
        Error.Failure("AlertingIncidentManagement.NotificationCreationFailed", "An error occurred while trying to create the notification.");
    
}