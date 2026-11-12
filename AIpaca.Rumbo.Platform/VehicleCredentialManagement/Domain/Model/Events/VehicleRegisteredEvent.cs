using AIpaca.Rumbo.Platform.Shared.Domain.Model.Events;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Events;

/// <summary>
///     Raised when a vehicle is enrolled in the service.
/// </summary>
/// <param name="VehicleId">The identifier of the registered vehicle.</param>
/// <param name="Plate">The registration plate of the vehicle.</param>
public record VehicleRegisteredEvent(VehicleId VehicleId, LicensePlate Plate) : IEvent;