using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

/// <summary>
/// References a student assigned to a route stop.
/// </summary>
public readonly record struct StudentId(int Value) : IEntityId<int>
{
    public static implicit operator int(StudentId id) => id.Value;

    public static implicit operator StudentId(int value) => new(value);

    public override string ToString() => Value.ToString();
}