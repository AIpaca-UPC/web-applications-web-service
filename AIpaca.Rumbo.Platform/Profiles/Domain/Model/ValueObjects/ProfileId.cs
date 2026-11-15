using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier of a profile in the Profiles bounded context.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct ProfileId(int Value) : IEntityId<int>
{
    public static implicit operator int(ProfileId id) => id.Value;
    public static implicit operator ProfileId(int value) => new(value);

    public override string ToString() => Value.ToString();
}