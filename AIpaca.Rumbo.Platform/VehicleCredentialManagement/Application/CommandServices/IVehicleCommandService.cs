using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.CommandServices;

/// <summary>
///     Handles the commands of the Vehicle &amp; Credential Management bounded context.
/// </summary>
public interface IVehicleCommandService
{
    Task<Result<Vehicle>> Handle(CreateVehicleCommand command, CancellationToken cancellationToken);

    Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken);

    Task<Result<Vehicle>> Handle(RegisterCredentialVerificationCommand command, CancellationToken cancellationToken);
}