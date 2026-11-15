using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

/// <summary>
/// Identifies the driver responsible for a route.
/// </summary>
public readonly record struct DriverProfileId(int Value) : IEntityId<int>
{
    public static implicit operator int(DriverProfileId id) => id.Value;

    public static implicit operator DriverProfileId(int value) => new(value);

    public override string ToString() => Value.ToString();
}