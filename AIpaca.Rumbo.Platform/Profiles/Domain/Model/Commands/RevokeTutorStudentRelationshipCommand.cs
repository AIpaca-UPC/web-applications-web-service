using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;

public record RevokeTutorStudentRelationshipCommand(
    ProfileId TutorProfileId, 
    StudentId StudentId);
