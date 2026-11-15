namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

/// <summary>
///     Registration plate of a vehicle.
/// </summary>
/// <remarks>
///     The plate is stored normalised in upper case and without surrounding blanks, so that
///     two declarations of the same plate written differently are treated as the same value.
/// </remarks>
/// <param name="Value">The plate as declared by the driver.</param>
public record LicensePlate(string Value)
{
    public LicensePlate() : this(string.Empty)
    {
    }

    public string Value { get; } = (Value ?? string.Empty).Trim().ToUpperInvariant();

    public override string ToString() => Value;
}