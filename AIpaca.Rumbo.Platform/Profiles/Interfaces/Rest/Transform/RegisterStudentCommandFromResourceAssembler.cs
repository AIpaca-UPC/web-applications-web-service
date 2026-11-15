using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming RegisterStudentResource to RegisterStudentCommand
/// </summary>
public static class RegisterStudentCommandFromResourceAssembler
{
    public static RegisterStudentCommand ToCommandFromResource(RegisterStudentResource resource)
    {
        if (resource == null)
            throw new ArgumentException(nameof(resource),
                "RegisterStudentResource cannot be null when converting to command");
        return new RegisterStudentCommand(
            resource.FirstName,
            resource.LastName,
            resource.BirthDate,
            resource.SchoolName
            );
    }
}
