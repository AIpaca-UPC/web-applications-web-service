namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Resource for registering a student
/// </summary>
public record RegisterStudentResource(
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string SchoolName);
