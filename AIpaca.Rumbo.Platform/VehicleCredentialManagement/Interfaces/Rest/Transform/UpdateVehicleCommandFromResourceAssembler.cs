using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Transform;

/// <summary>
///     Transforms an <see cref="UpdateVehicleResource" /> into its command.
/// </summary>
public class UpdateVehicleCommandFromResourceAssembler
{
    public static UpdateVehicleCommand ToCommandFromResource(int vehicleId, UpdateVehicleResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new UpdateVehicleCommand(
            vehicleId,
            resource.LicensePlate,
            resource.Brand,
            resource.Model,
            resource.Capacity,
            resource.Year,
            CredentialWireFormat.ToVehicleStatus(resource.Status),
            resource.DriverName,
            CredentialDeclarationAssembler.ToDeclarations(resource.Credentials));
    }
}