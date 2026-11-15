using AIpaca.Rumbo.Platform.Shared.Domain.Model.Entities;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;

public partial class Student : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}