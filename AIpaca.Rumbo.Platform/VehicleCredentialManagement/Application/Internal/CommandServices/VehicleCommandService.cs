using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.Internal.CommandServices;

/// <summary>
///    Default implementation of the <see cref="IVehicleCommandService" /> contract.
/// </summary>
/// <param name="vehicleRepository"> The vehicle repository. </param>
/// <param name="unitOfWork"> The unit of work. </param>
public class VehicleCommandService(IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IVehicleCommandService 
{
    /// <summary>
    ///   Handles the <see cref="CreateVehicleCommand" /> command.
    /// </summary>
    /// <param name="command"> The command to handle. </param>
    /// <param name="cancellationToken"> The cancellation token. </param>
    /// <returns></returns>
    public async Task<Result<Vehicle>> Handle(CreateVehicleCommand command, CancellationToken cancellationToken)
    {
        var plate = new LicensePlate(command.LicensePlate);

        if (await vehicleRepository.ExistsByLicensePlateAsync(plate, cancellationToken))
            return Result<Vehicle>.Failure(
                VehiclesError.LicensePlateAlreadyRegistered,
                $"A vehicle with plate {plate.Value} is already registered.");

        var vehicle = new Vehicle(command);
        await vehicleRepository.AddAsync(vehicle, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Vehicle>.Success(vehicle);
    }

    /// <summary>
    ///   Handles the <see cref="UpdateVehicleCommand" /> command.
    /// </summary>
    /// <param name="command"> The command to handle. </param>
    /// <param name="cancellationToken"> The cancellation token. </param>
    /// <returns></returns>
    public async Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(new VehicleId(command.VehicleId), cancellationToken);
        if (vehicle is null)
            return Result<Vehicle>.Failure(
                VehiclesError.VehicleNotFound,
                $"No vehicle was found with identifier {command.VehicleId}.");

        var plate = new LicensePlate(command.LicensePlate);

        // The plate may stay the same; it only clashes when it now belongs to another vehicle.
        if (vehicle.Plate.Value != plate.Value &&
            await vehicleRepository.ExistsByLicensePlateAsync(plate, cancellationToken))
            return Result<Vehicle>.Failure(
                VehiclesError.LicensePlateAlreadyRegistered,
                $"A vehicle with plate {plate.Value} is already registered.");

        vehicle.Update(command);
        vehicleRepository.Update(vehicle);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Vehicle>.Success(vehicle);
    }

    /// <summary>
    ///  Handles the <see cref="RegisterCredentialVerificationCommand" /> command.
    /// </summary>
    /// <param name="command"> The command to handle. </param>
    /// <param name="cancellationToken"> The cancellation token. </param>
    /// <returns></returns>
    public async Task<Result<Vehicle>> Handle(
        RegisterCredentialVerificationCommand command, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(new VehicleId(command.VehicleId), cancellationToken);
        if (vehicle is null)
            return Result<Vehicle>.Failure(
                VehiclesError.VehicleNotFound,
                $"No vehicle was found with identifier {command.VehicleId}.");

        if (!vehicle.RegisterCredentialVerification(command))
            return Result<Vehicle>.Failure(
                VehiclesError.CredentialNotDeclared,
                $"No credential of kind {command.Type} was declared for this vehicle.");

        vehicleRepository.Update(vehicle);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Vehicle>.Success(vehicle);
    }
}