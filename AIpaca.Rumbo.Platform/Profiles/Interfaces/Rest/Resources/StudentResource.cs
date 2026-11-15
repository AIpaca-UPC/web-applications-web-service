namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Student resource for Rest API
/// </summary>
public record StudentResource(int Id, string FullName, DateOnly BirthDate, string SchoolName, string Status);
