using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;

public record AuthorizeTutorStudentRelationshipCommand(
    ProfileId TutorProfileId, 
    StudentId StudentId,
    ERelationshipType RelationshipType,
    EAuthorizationStatus AuthorizationStatus);
