using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Commands;

public record MarkNotificationAsReadCommand(NotificationId NotificationId);