using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;
/// <summary>
///     Assembler responsible for transforming Profile entity to ProfileResource
/// </summary>
public static class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile entity)
    {
        if (entity == null)
            throw new ArgumentException(nameof(entity), 
                "Profile entity cannot be null when converting to resource");

        return new ProfileResource(
            entity.Id, 
            entity.AccountId,
            entity.FullName,
            entity.PhoneNumber.Number,
            entity.ProfileType.ToString());
    }
}
