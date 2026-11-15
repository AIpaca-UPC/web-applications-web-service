using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

/// <summary>
///  Represents a strongly-typed unique identifier for a route in the Alerting and Incident Management Bounded Context.
/// </summary>
/// <param name="Value">Represents the unique identifier of a route</param>
public readonly record struct RouteId(int Value) : IEntityId<int>
{
    public static implicit operator int(RouteId id) => id.Value;
    public static implicit operator RouteId(int value) => new(value);
    
    public override string ToString() => Value.ToString();
}