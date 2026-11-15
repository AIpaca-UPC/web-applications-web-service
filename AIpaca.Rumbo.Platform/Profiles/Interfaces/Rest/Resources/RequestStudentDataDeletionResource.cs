namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Resource for requesting the deletion of a student's data
/// </summary>
public record RequestStudentDataDeletionResource(
    int StudentId,
    int RequestedByProfileId);
