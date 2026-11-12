namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Queries;

/// <summary>
///     Get one vehicle by its identifier.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
public record GetVehicleByIdQuery(int VehicleId);