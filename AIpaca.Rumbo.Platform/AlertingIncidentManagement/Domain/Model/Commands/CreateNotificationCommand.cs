using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Commands;

public record CreateNotificationCommand(
    ENotificationCategory Category, 
    ENotificationType Type, 
    ENotificationPriority Priority, 
    string Title, 
    string Message, 
    EJourneyEventType? JourneyEventType, 
    int RouteId, 
    int TripId, 
    int StudentId, 
    int RecipientProfileId);
    