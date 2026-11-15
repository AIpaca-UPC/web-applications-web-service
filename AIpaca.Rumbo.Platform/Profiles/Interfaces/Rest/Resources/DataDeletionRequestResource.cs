namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Data deletion request resource for Rest API
/// </summary>
public record DataDeletionRequestResource(
    int Id,
    int StudentId, 
    int RequestedByTutorId, 
    string Status);
