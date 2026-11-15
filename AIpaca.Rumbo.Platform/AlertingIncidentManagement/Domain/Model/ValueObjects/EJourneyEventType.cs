namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

/// <summary>
/// Represents the type of journey event of the notification in the Alerting and Incident Management Bounded Context.
/// </summary>
public enum EJourneyEventType
{
    RoutePublished,
    TripPlanned,
    TripStarted,
    StopServed,
    Absent,
    StudentArrived, 
    TripCanceled,
    TripCompleted,
}