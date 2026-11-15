namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Resource for creating a profile
/// </summary>
public record CreateProfileResource(
    int AccountId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string ProfileType);
