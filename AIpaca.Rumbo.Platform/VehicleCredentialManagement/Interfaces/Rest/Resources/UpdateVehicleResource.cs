namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

/// <summary>
///     Resource for updating a vehicle and its declared documents.
/// </summary>
public record UpdateVehicleResource(
    string LicensePlate,
    string Brand,
    string Model,
    int Capacity,
    int Year,
    string Status,
    string DriverName,
    IReadOnlyCollection<CredentialDeclarationResource>? Credentials);