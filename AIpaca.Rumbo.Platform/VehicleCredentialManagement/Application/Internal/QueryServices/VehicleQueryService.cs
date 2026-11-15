using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.Internal.QueryServices;

/// <summary>
///   Default implementation of the <see cref="IVehicleQueryService" /> contract.
/// </summary>
/// <param name="vehicleRepository">The repository for accessing vehicle data.</param>
public class VehicleQueryService(IVehicleRepository vehicleRepository) : IVehicleQueryService
{
    /// <summary>
    ///  Handles the <see cref="GetAllVehiclesQuery" /> query.
    /// </summary>
    /// <param name="query">The query for retrieving all vehicles.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of vehicles.</returns>
    public async Task<IEnumerable<Vehicle>> Handle(
        GetAllVehiclesQuery query, CancellationToken cancellationToken)
    {
        return await vehicleRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    /// <summary>
    ///   Handles the <see cref="GetVehicleByIdQuery" /> query.
    /// </summary>
    /// <param name="query">The query for retrieving a vehicle by its ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle if found; otherwise, null.</returns>
    public async Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken)
    {
        return await vehicleRepository.FindByIdAsNoTrackingAsync(
            new VehicleId(query.VehicleId), cancellationToken);
    }
}