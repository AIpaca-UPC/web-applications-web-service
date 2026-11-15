using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier of a vehicle in the Vehicle &amp; Credential
///     Management bounded context.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct VehicleId(int Value): IEntityId<int>
{
    public static implicit operator int(VehicleId id) => id.Value;
    public static implicit operator VehicleId(int value) => new(value);

    public override string ToString() => Value.ToString();
}