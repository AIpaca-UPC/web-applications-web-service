namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

/// <summary>
/// Represents the unique identifier for a notification.
/// </summary>
/// <param name="Value">The unique identifier for the notification.</param>
public readonly record struct NotificationId(int Value)
{
    public static implicit operator int(NotificationId id) => id.Value;
    public static implicit operator NotificationId(int value) => new(value);
    
    public override string ToString() => Value.ToString();
}