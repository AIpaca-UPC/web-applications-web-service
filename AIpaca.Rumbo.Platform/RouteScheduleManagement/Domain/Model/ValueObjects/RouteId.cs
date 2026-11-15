using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

/// <summary>
/// Represents the unique identifier of a route.
/// </summary>
public readonly record struct RouteId(int Value) : IEntityId<int>
{
    public static implicit operator int(RouteId id) => id.Value;

    public static implicit operator RouteId(int value) => new(value);

    public override string ToString() => Value.ToString();
}