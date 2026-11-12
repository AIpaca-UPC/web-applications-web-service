using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Transform;

/// <summary>
///     Transforms a <see cref="CreateVehicleResource" /> into its command.
/// </summary>
public static class CreateVehicleCommandFromResourceAssembler
{
    public static CreateVehicleCommand ToCommandFromResource(CreateVehicleResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new CreateVehicleCommand(
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

/// <summary>
///     Transforms declared document resources into their domain counterparts.
/// </summary>
public static class CredentialDeclarationAssembler
{
    public static IReadOnlyCollection<CredentialDeclaration> ToDeclarations(
        IReadOnlyCollection<CredentialDeclarationResource>? resources)
    {
        return resources is null
            ? []
            : resources.Select(resource => new CredentialDeclaration(
                CredentialWireFormat.ToCredentialType(resource.Type),
                resource.Number,
                resource.ExpiresAt)).ToList();
    }
}