using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming CreateProfileResource to CreateProfileCommand
/// </summary>
public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        if (resource == null)
            throw new ArgumentException(nameof(resource),
                "CreateProfileResource cannot be null when converting to command");
        return new CreateProfileCommand(
            resource.AccountId,
            resource.FirstName,
            resource.LastName,
            resource.PhoneNumber,
            Enum.Parse<EProfileType>(resource.ProfileType, true)
            );
    }
}
