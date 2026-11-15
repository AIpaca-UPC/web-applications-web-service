using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique identifier for a tutor-student relationship within the system.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct TutorStudentRelationshipId(int Value) : IEntityId<int>
{
    public static implicit operator int(TutorStudentRelationshipId id) => id.Value;
    public static implicit operator TutorStudentRelationshipId(int value) => new(value);

    public override string ToString() => Value.ToString();
}