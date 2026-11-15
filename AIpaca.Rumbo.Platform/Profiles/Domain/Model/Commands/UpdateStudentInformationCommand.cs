using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;

public record UpdateStudentInformationCommand(
    StudentId StudentId, 
    string FirstName, 
    string LastName, 
    DateOnly BirthDate,
    EStudentStatus Status);
