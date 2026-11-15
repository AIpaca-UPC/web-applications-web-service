using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier of a service enrollment in the Profiles bounded context.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct ServiceEnrollmentId(int Value) : IEntityId<int>
{
    public static implicit operator int(ServiceEnrollmentId id) => id.Value;
    public static implicit operator ServiceEnrollmentId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
