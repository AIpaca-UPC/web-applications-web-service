using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;

public record RegisterStudentCommand(
    string FirstName, 
    string LastName, 
    DateOnly BirthDate, 
    string SchoolName);
