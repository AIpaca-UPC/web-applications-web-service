using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Transform;

/// <summary>
/// Translates between the enumerations of the domain and the literals of the REST contract.
/// </summary>
/// <remarks>
/// The contract uses upper case literals. Keeping the translation here means the naming conventions
/// of C# never leak into the API, and renaming an enumeration member does not break a client.
/// </remarks>
public static class NotificationWireFormat
{
    public static string ToLiteral(ENotificationCategory category) => category.ToString().ToUpperInvariant();

    public static string ToLiteral(ENotificationType type) => type.ToString().ToUpperInvariant();

    public static string ToLiteral(ENotificationPriority priority) => priority.ToString().ToUpperInvariant();

    public static string ToLiteral(EJourneyEventType eventType) => eventType.ToString().ToUpperInvariant();

    public static ENotificationCategory ToCategory(string value) => value?.Trim().ToUpperInvariant() switch
    {
        "GENERAL" => ENotificationCategory.General,
        "STUDENT_JOURNEY" => ENotificationCategory.StudentJourney,
        _ => throw new ArgumentException($"Unknown notification category '{value}'.", nameof(value))
    };

    public static ENotificationType ToType(string value) => value?.Trim().ToUpperInvariant() switch
    {
        "INFORMATION" => ENotificationType.Information,
        "WARNING" => ENotificationType.Warning,
        "SUCCESS" => ENotificationType.Success,
        "IMPORTANT" => ENotificationType.Important,
        _ => throw new ArgumentException($"Unknown notification type '{value}'.", nameof(value))
    };

    public static ENotificationPriority ToPriority(string value) => value?.Trim().ToUpperInvariant() switch
    {
        "REGULAR" => ENotificationPriority.Regular,
        "CRITICAL" => ENotificationPriority.Critical,
        _ => throw new ArgumentException($"Unknown notification priority '{value}'.", nameof(value))
    };

    public static EJourneyEventType ToJourneyEventType(string value) => value?.Trim().ToUpperInvariant() switch
    {
        "STUDENT_ABSENT" => EJourneyEventType.Absent,
        "ROUTE_PUBLISHED" => EJourneyEventType.RoutePublished,
        "STOP_SERVED" => EJourneyEventType.StopServed,
        "STUDENT_ARRIVED" => EJourneyEventType.StudentArrived,
        "TRIP_PLANNED" => EJourneyEventType.TripPlanned,
        "TRIP_STARTED" => EJourneyEventType.TripStarted,
        "TRIP_CANCELED" => EJourneyEventType.TripCanceled,
        "TRIP_COMPLETED" => EJourneyEventType.TripCompleted,
        _ => throw new ArgumentException($"Unknown journey event type '{value}'.", nameof(value))
    };
}