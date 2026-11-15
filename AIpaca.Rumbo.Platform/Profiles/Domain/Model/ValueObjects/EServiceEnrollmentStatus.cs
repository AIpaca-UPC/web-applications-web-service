namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
///   Represents a service enrollment status in the Profiles bounded context.
/// </summary>
public enum EServiceEnrollmentStatus
{
    Pending = 0,
    Accepted = 1,
    Cancelled = 2,
    Ended = 3 
}
