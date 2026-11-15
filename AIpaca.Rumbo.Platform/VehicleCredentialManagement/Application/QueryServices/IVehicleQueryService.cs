using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Queries;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.QueryServices;

/// <summary>
///     Answers the queries of the Vehicle &amp; Credential Management bounded context.
/// </summary>
public interface IVehicleQueryService
{
    Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query, CancellationToken cancellationToken);

    Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken);
}