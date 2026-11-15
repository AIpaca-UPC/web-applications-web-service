using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Repositories;

/// <summary>
///     Persistence contract for the <see cref="Vehicle" /> aggregate.
/// </summary>
public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    /// <summary>
    ///     Finds a vehicle by its strongly-typed identifier.
    /// </summary>
    /// <remarks>
    ///     The identifier of this aggregate is stored through a value converter, so the
    ///     <c>int</c> overload inherited from the base repository cannot be used for it.
    /// </remarks>
    /// <param name="id">The identifier of the vehicle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle, or <c>null</c> when no vehicle carries that identifier.</returns>
    Task<Vehicle?> FindByIdAsync(VehicleId id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds a vehicle by its registration plate.
    /// </summary>
    /// <param name="plate">The plate to look for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle, or <c>null</c> when no vehicle carries that plate.</returns>
    Task<Vehicle?> FindByLicensePlateAsync(LicensePlate plate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Determines whether a vehicle with the given plate is already registered.
    /// </summary>
    /// <param name="plate">The plate to look for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> when the plate is taken; otherwise <c>false</c>.</returns>
    Task<bool> ExistsByLicensePlateAsync(LicensePlate plate, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Finds a vehicle by its identifier without change tracking.
    /// </summary>
    /// <param name="id">The identifier of the vehicle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle, or <c>null</c> when no vehicle carries that identifier.</returns>
    Task<Vehicle?> FindByIdAsNoTrackingAsync(VehicleId id, CancellationToken cancellationToken = default);
}