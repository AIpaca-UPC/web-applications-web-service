using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Transform;

/// <summary>
///     Transforms a <see cref="Vehicle" /> aggregate into the resource the API presents.
/// </summary>
public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new VehicleResource(
            entity.Id,
            entity.LicensePlate,
            entity.Brand,
            entity.Model,
            entity.Capacity,
            entity.Year,
            CredentialWireFormat.ToLiteral(entity.Status),
            entity.DriverName,
            entity.HasAllCredentialsVerified,
            entity.Credentials.Select(ToResourceFromCredential).ToList());
    }

    private static CredentialResource ToResourceFromCredential(Credential credential)
    {
        return new CredentialResource(
            CredentialWireFormat.ToLiteral(credential.Type),
            credential.Number,
            credential.ExpiresAt,
            CredentialWireFormat.ToLiteral(credential.VerificationStatus),
            credential.VerificationSource,
            credential.VerifiedAt,
            CredentialWireFormat.ToLiteral(credential.DisplayStatus));
    }
}