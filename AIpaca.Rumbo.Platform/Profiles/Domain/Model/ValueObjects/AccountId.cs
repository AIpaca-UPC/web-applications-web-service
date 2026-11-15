using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier for an account within the system.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct AccountId(int Value) : IEntityId<int>
{
    public static implicit operator int(AccountId id) => id.Value;
    public static implicit operator AccountId(int value) => new(value);

    public override string ToString() => Value.ToString();
}