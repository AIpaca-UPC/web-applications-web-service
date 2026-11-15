using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier of a data deletion request in the Profiles bounded context.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct DataDeletionRequestId(int Value) : IEntityId<int>
{
    public static implicit operator int(DataDeletionRequestId id) => id.Value;
    public static implicit operator DataDeletionRequestId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
