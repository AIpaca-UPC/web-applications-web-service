using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

/// <summary>
///  Represents a strongly-typed unique identifier of a trip on the Alerting and Incident Management Bounded Context.
/// </summary>
/// <param name="Value">Represents the unique identifier of a trip</param>
public readonly record struct TripId(int Value) : IEntityId<int>
{
    public static implicit operator int(TripId id) => id.Value;
    public static implicit operator TripId(int value) => new(value);
    
    public override string ToString() => Value.ToString();
}