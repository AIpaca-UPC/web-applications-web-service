using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming RequestStudentDataDeletionResource to RequestStudentDataDeletionCommand
/// </summary>
public static class RequestStudentDataDeletionCommandFromResourceAssembler
{
    public static RequestStudentDataDeletionCommand ToCommandFromResource(RequestStudentDataDeletionResource resource)
    {
        if (resource == null)
            throw new ArgumentException(nameof(resource),
                "RequestStudentDataDeletionResource cannot be null when converting to command");
        return new RequestStudentDataDeletionCommand(
            resource.StudentId,
            resource.RequestedByProfileId
            );
    }
}
