using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;

public partial class Notification : AggregateRoot
{
    public NotificationId Id { get; }
    public ENotificationCategory Category { get; }
    public ENotificationType Type { get; }
    public ENotificationPriority Priority { get; }
    public String Title { get; }
    public String Message { get;  }
    
    public DateTimeOffset? ReadAt { get; set; }
    public RouteId RouteId { get; }
    public TripId TripId { get; }
    public StudentId StudentId { get; }
    public ProfileId RecipientProfileId { get;  }
    public EJourneyEventType? JourneyEventType { get; }

    public Notification()
    {
        Category = ENotificationCategory.General;
        Type = ENotificationType.Information;
        Priority = ENotificationPriority.Regular;
        Title = String.Empty;
        Message = String.Empty;
    }

    public Notification(CreateNotificationCommand command)
    {
        Category = command.Category;
        Type = command.Type;
        Priority = command.Priority;
        Title = command.Title;
        Message = command.Message;
        JourneyEventType = command.JourneyEventType;
        RouteId = new RouteId(command.RouteId);
        TripId = new TripId(command.TripId);
        StudentId = new StudentId(command.StudentId);
        RecipientProfileId = new ProfileId(command.RecipientProfileId);
    }
    
    public void MarkAsRead()
    {
        this.ReadAt ??= DateTimeOffset.UtcNow;
    }

    public bool IsRead()
    {
        return ReadAt.HasValue;
    }
    
    public bool IsCritical()
    {
        return Priority == ENotificationPriority.Critical;
    }
    
    public bool IsJourneyNotification()
    {
        return Category == ENotificationCategory.StudentJourney;
    }
}