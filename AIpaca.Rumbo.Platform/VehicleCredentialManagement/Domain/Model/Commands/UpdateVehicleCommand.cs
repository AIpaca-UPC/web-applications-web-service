using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;

/// <summary>
///     Update a vehicle and the documents declared for it.
/// </summary>
public record UpdateVehicleCommand(
    int VehicleId,
    string LicensePlate,
    string Brand,
    string Model,
    int Capacity,
    int Year,
    EVehicleStatus Status,
    string DriverName,
    IReadOnlyCollection<CredentialDeclaration> Credentials
    );