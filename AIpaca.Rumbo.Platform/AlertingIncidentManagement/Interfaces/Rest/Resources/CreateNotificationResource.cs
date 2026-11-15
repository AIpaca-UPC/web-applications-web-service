namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Resources;

/// <summary>
/// A notification to be created, as received by the API.
/// </summary>
/// <param name="Category">The category of the notification.</param>
/// <param name="Type">The type of notification.</param>
/// <param name="Priority">The priority level of the notification.</param>
/// <param name="Title">The title of the notification.</param>
/// <param name="Message">The content of the notification.</param>
/// <param name="RouteId">The identifier of the associated route.</param>
/// <param name="TripId">The identifier of the associated trip.</param>
/// <param name="StudentId">The identifier of the associated student.</param>
/// <param name="RecipientProfileId">The identifier of the recipient profile.</param>
/// <param name="JourneyEventType">The type of journey event associated with the notification.</param>
public record CreateNotificationResource(
    string Category,
    string Type,
    string Priority,
    string Title,
    string Message,
    int RouteId,
    int TripId,
    int StudentId,
    int RecipientProfileId,
    string? JourneyEventType);