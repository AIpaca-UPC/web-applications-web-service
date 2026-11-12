namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

/// <summary>
///     A document declared for a vehicle, as presented by the API.
/// </summary>
/// <param name="Type">The kind of document: SOAT, CITV or DRIVER_LICENSE.</param>
/// <param name="Number">The document number as declared.</param>
/// <param name="ExpiresAt">The date the document stops being valid.</param>
/// <param name="VerificationStatus">The outcome recorded: PENDING, VERIFIED or FAILED.</param>
/// <param name="VerificationSource">The service that produced the outcome.</param>
/// <param name="VerifiedAt">The moment the outcome was recorded.</param>
/// <param name="DisplayStatus">
///     The status the document is presented with, once expiry is taken into account:
///     PENDING, VERIFIED, FAILED or EXPIRED.
/// </param>

public record CredentialResource(
    string Type,
    string Number,
    DateOnly? ExpiresAt,
    string VerificationStatus,
    string VerificationSource,
    DateTimeOffset? VerifiedAt,
    string DisplayStatus);

/// <summary>
///     A vehicle enrolled in the service, as presented by the API.
/// </summary>
public record VehicleResource(
    int Id,
    string LicensePlate,
    string Brand,
    string Model,
    int Capacity,
    int Year,
    string Status,
    string DriverName,
    bool HasAllCredentialsVerified,
    IReadOnlyCollection<CredentialResource> Credentials);