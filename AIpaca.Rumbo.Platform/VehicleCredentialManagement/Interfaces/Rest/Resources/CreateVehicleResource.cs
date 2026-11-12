namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

/// <summary>
///     A document declared by the driver when registering or updating a vehicle.
/// </summary>
/// <param name="Type">The kind of document: SOAT, CITV or DRIVER_LICENSE.</param>
/// <param name="Number">The document number.</param>
/// <param name="ExpiresAt">The date the document stops being valid.</param>
public record CredentialDeclarationResource(string Type, string Number, DateOnly? ExpiresAt);

/// <summary>
///     Resource for registering a vehicle together with its declared documents.
/// </summary>
public record CreateVehicleResource(string LicensePlate,
    string Brand,
    string Model,
    int Capacity,
    int Year,
    string Status,
    string DriverName,
    IReadOnlyCollection<CredentialDeclarationResource>? Credentials);