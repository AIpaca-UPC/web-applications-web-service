using AIpaca.Rumbo.Platform.Shared.Domain.Model.Entities;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;

public partial class NotificationAudit : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}