namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///   Represents a relationship type in the Profiles bounded context.
/// </summary>
public enum ERelationshipType
{
    Mother = 0,
    Father = 1,
    LegalGuardian = 2,
    Other = 3
}
