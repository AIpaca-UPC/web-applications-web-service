using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;

public record RequestStudentDataDeletionCommand(
    StudentId StudentId, 
    ProfileId RequestedByProfileId);
