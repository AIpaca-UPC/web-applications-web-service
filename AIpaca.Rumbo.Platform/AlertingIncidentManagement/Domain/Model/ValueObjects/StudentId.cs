using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

/// <summary>
///  Represents a strongly-typed unique identifier for a student in the Alerting and Incident Management Bounded Context.
/// </summary>
/// <param name="Value">Represents the unique identifier of a student</param>
public readonly record struct StudentId(int Value) : IEntityId<int>
{
    public static implicit operator int(StudentId id) => id.Value;
    public static implicit operator StudentId(int value) => new(value);
    
    public override string ToString() => Value.ToString();
}
