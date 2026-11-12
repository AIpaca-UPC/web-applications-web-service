using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Repositories;

/// <summary>
///    Repository implementation for the Vehicle aggregate root, using Entity Framework Core as the underlying persistence mechanism.
/// </summary>
/// <param name="context">The database context.</param>
public class VehicleRepository(AppDbContext context) : BaseRepository<Vehicle>(context), IVehicleRepository
{
    /// <summary>
    ///     Function to find a vehicle by its unique identifier. Returns null if no vehicle with the given id exists.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to find.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The vehicle if found, otherwise null.</returns>
    
    public async Task<Vehicle?> FindByIdAsync(VehicleId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .FirstOrDefaultAsync(vehicle => vehicle.Id == id, cancellationToken);
    }
    
    /// <summary>
    ///    Function to find a vehicle by its license plate. Returns null if no vehicle with the given license plate exists.
    /// </summary>
    /// <param name="plate">The license plate of the vehicle to find.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The vehicle if found, otherwise null.</returns>
    public async Task<Vehicle?> FindByLicensePlateAsync(
        LicensePlate plate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .FirstOrDefaultAsync(vehicle => vehicle.Plate.Value == plate.Value, cancellationToken);
    }

    
    /// <summary>
    ///    Function to check if a vehicle with the given license plate exists in the database.
    /// </summary>
    /// <param name="plate">The license plate of the vehicle to check.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>true if the vehicle exists, otherwise false.</returns>
    public async Task<bool> ExistsByLicensePlateAsync(
        LicensePlate plate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .AnyAsync(vehicle => vehicle.Plate.Value == plate.Value, cancellationToken);
    }
    
    /// <summary>
    ///    Finds a vehicle by its identifier without change tracking.
    /// </summary>
    /// <param name="id">The identifier of the vehicle to find.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The vehicle if found, otherwise null.</returns>
    public async Task<Vehicle?> FindByIdAsNoTrackingAsync(
        VehicleId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .AsNoTracking()
            .FirstOrDefaultAsync(vehicle => vehicle.Id == id, cancellationToken);
    }
}