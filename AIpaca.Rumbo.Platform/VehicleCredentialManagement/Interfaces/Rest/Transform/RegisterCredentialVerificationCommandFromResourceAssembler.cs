using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Transform;

/// <summary>
///     Transforms a <see cref="RegisterCredentialVerificationResource" /> into its command.
/// </summary>
public static class RegisterCredentialVerificationCommandFromResourceAssembler
{
    public static RegisterCredentialVerificationCommand ToCommandFromResource(
        int vehicleId, RegisterCredentialVerificationResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new RegisterCredentialVerificationCommand(
            vehicleId,
            CredentialWireFormat.ToCredentialType(resource.Type),
            resource.Answered,
            resource.Verified,
            resource.Source);
    }
}