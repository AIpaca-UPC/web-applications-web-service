using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Transform;

/// <summary>
/// Transforms a <see cref="CreateNotificationResource"/> into its command.
/// </summary>
public static class CreateNotificationCommandFromResourceAssembler
{
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new CreateNotificationCommand(
            NotificationWireFormat.ToCategory(resource.Category),
            NotificationWireFormat.ToType(resource.Type),
            NotificationWireFormat.ToPriority(resource.Priority),
            resource.Title,
            resource.Message,
            resource.JourneyEventType is null
                ? null
                : NotificationWireFormat.ToJourneyEventType(resource.JourneyEventType),
            resource.RouteId,
            resource.TripId,
            resource.StudentId,
            resource.RecipientProfileId
            );
    }
}