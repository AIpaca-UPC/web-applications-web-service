namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Profile resource for Rest API
/// </summary>
public record ProfileResource(
    int Id,
    int AccountId, 
    string FullName,
    string PhoneNumber, 
    string ProfileType);
