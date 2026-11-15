namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///     Phone number value object.
/// </summary>
/// <param name="Number"></param>
public record PhoneNumber(string Number)
{
    public PhoneNumber() : this(string.Empty) {}
}