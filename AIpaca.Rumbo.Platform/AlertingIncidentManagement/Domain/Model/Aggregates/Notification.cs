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
    public DateTimeOffset ReadAt { get;  }
    public RouteId RouteId { get; }
    public TripId TripId { get; }
    public StudentId StudentId { get; }
    public ProfileId RecipientProfileId { get;  }
    
    
    
    
    

}