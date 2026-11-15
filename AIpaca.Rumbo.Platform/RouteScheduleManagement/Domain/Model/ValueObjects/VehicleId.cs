using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

/// <summary>
/// References a vehicle assigned to a route.
/// </summary>
public readonly record struct VehicleId(int Value) : IEntityId<int>
{
    public static implicit operator int(VehicleId id) => id.Value;

    public static implicit operator VehicleId(int value) => new(value);

    public override string ToString() => Value.ToString();
}