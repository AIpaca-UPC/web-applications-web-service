using AIpaca.Rumbo.Platform.Shared.Domain.Model.Entities;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;

public partial class VehicleAudit : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}