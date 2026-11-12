using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;

/// <summary>
///     A document declared by the driver when registering or updating a vehicle.
/// </summary>
/// <param name="Type">The kind of document.</param>
/// <param name="Number">The document number.</param>
/// <param name="ExpiresAt">The date the document stops being valid.</param>
public record CredentialDeclaration(ECredentialType Type, string Number, DateOnly? ExpiresAt);

/// <summary>
///     Register a vehicle together with the documents declared for it.
/// </summary>
public record CreateVehicleCommand(
    string LicensePlate,
    string Brand,
    string Model,
    int Capacity,
    int Year,
    EVehicleStatus Status,
    string DriverName,
    IReadOnlyCollection<CredentialDeclaration> Credentials
    );