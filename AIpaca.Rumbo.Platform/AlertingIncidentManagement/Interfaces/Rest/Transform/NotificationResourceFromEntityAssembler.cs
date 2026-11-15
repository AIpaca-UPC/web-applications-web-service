using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Transform;

/// <summary>
/// Transforms a <see cref="Notification"/> aggregate into the resource the API presents.
/// </summary>
public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Notification entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new NotificationResource(
            entity.Id.Value,
            NotificationWireFormat.ToLiteral(entity.Category),
            NotificationWireFormat.ToLiteral(entity.Type),
            NotificationWireFormat.ToLiteral(entity.Priority),
            entity.Title,
            entity.Message,
            entity.ReadAt,
            entity.RouteId.Value,
            entity.TripId.Value,
            entity.StudentId.Value,
            entity.RecipientProfileId.Value,
            entity.JourneyEventType is null
                ? null
                : NotificationWireFormat.ToLiteral(entity.JourneyEventType.Value));
    }
}