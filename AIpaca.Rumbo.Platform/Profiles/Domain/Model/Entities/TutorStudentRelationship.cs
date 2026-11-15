using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Entities;

public class TutorStudentRelationship
{
    internal TutorStudentRelationship(AuthorizeTutorStudentRelationshipCommand command)
    {
        TutorProfileId = command.TutorProfileId;
        StudentId = command.StudentId;
        RelationshipType = command.RelationshipType;
        AuthorizationStatus = command.AuthorizationStatus;
    }

    public TutorStudentRelationshipId Id { get; private set; }
    
    public ProfileId TutorProfileId { get; private set; }
    public StudentId StudentId { get; private set; }
    public ERelationshipType RelationshipType { get; private set; }
    public EAuthorizationStatus AuthorizationStatus { get; private set; }
}
