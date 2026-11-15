namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;

/// <summary>
///  Represents a strongly-typed unique identifier of a profile on the Alerting and Incident Management Bounded Context.
/// </summary>
/// <param name="Value">Represents the unique identifier of a profile</param>
public readonly record struct ProfileId(int Value)
{
    public static implicit operator int(ProfileId id) => id.Value;
    public static implicit operator ProfileId(int value) => new(value);
    
    public override string ToString() => Value.ToString();
}