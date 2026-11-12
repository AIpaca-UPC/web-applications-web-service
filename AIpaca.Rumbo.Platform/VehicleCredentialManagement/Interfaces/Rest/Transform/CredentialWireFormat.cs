using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Transform;

/// <summary>
///     Translates between the enumerations of the domain and the literals of the REST contract.
/// </summary>
/// <remarks>
///     The contract uses upper case literals such as <c>DRIVER_LICENSE</c>, which the clients
///     already consume. Keeping the translation here means the naming conventions of C# never
///     leak into the API, and renaming an enumeration member does not break a client.
/// </remarks>
public static class CredentialWireFormat
{
    public static string ToLiteral(ECredentialType type) => type switch
    {
        ECredentialType.Soat => "SOAT",
        ECredentialType.Citv => "CITV",
        ECredentialType.DriverLicense => "DRIVER_LICENSE",
        _ => type.ToString().ToUpperInvariant()
    };

    public static string ToLiteral(EVerificationStatus status) => status switch
    {
        EVerificationStatus.Pending => "PENDING",
        EVerificationStatus.Verified => "VERIFIED",
        EVerificationStatus.Failed => "FAILED",
        _ => status.ToString().ToUpperInvariant()
    };

    public static string ToLiteral(ECredentialDisplayStatus status) => status switch
    {
        ECredentialDisplayStatus.Pending => "PENDING",
        ECredentialDisplayStatus.Verified => "VERIFIED",
        ECredentialDisplayStatus.Failed => "FAILED",
        ECredentialDisplayStatus.Expired => "EXPIRED",
        _ => status.ToString().ToUpperInvariant()
    };

    public static string ToLiteral(EVehicleStatus status) => status.ToString().ToUpperInvariant();

    public static ECredentialType ToCredentialType(string value) => value?.Trim().ToUpperInvariant() switch
    {
        "SOAT" => ECredentialType.Soat,
        "CITV" => ECredentialType.Citv,
        "DRIVER_LICENSE" => ECredentialType.DriverLicense,
        _ => throw new ArgumentException($"Unknown credential type '{value}'.", nameof(value))
    };

    public static EVehicleStatus ToVehicleStatus(string value) => value?.Trim().ToUpperInvariant() switch
    {
        "ACTIVE" => EVehicleStatus.Active,
        "INACTIVE" => EVehicleStatus.Inactive,
        "MAINTENANCE" => EVehicleStatus.Maintenance,
        _ => throw new ArgumentException($"Unknown vehicle status '{value}'.", nameof(value))
    };
}