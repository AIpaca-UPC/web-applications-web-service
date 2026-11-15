namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

/// <summary>
///     Kind of document a vehicle or its driver must hold to operate the service.
/// </summary>
public enum ECredentialType
{
    Soat = 0,
    Citv = 1,
    DriverLicense = 2
}

/// <summary>
///     Outcome recorded for a credential after a verification attempt.
/// </summary>
public enum EVerificationStatus
{
    Pending = 0,
    Verified = 1,
    Failed = 2
}

/// <summary>
///     Status a credential is presented with, once its expiry date is taken into account.
/// </summary>
public enum ECredentialDisplayStatus
{
    Pending = 0,
    Verified = 1,
    Failed = 2,
    Expired = 3
}

/// <summary>
///     A document declared for a vehicle, together with the outcome of its verification.
/// </summary>
/// <remarks>
///     The platform records declared documents; it does not act as the transport authority.
///     A credential therefore never reaches <see cref="EVerificationStatus.Verified" /> on its
///     own: it does so only through <see cref="WithVerification" />, which demands a source.
/// </remarks>
/// <param name="Type">The kind of document.</param>
/// <param name="Number">The document number as declared.</param>
/// <param name="ExpiresAt">The date the document stops being valid, when declared.</param>
/// <param name="VerificationStatus">The outcome recorded for the document.</param>
/// <param name="VerificationSource">The service that produced the outcome.</param>
/// <param name="VerifiedAt">The moment the outcome was recorded.</param>
public record Credential(
    ECredentialType Type,
    string Number,
    DateOnly? ExpiresAt,
    EVerificationStatus VerificationStatus = EVerificationStatus.Pending,
    string VerificationSource = "",
    DateTimeOffset? VerifiedAt = null)
{
    public Credential() : this(ECredentialType.Soat, string.Empty, null)
    {
    }

    /// <summary>
    ///     Gets a value indicating whether the declared expiry date has already passed.
    /// </summary>
    public bool IsExpired =>
        ExpiresAt is not null && ExpiresAt.Value < DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    ///     Gets the status the credential is presented with.
    /// </summary>
    /// <remarks>
    ///     Expiry overrides verification: a document whose validity has lapsed is presented as
    ///     expired even when a successful verification was recorded earlier.
    /// </remarks>
    public ECredentialDisplayStatus DisplayStatus => IsExpired
        ? ECredentialDisplayStatus.Expired
        : (ECredentialDisplayStatus)VerificationStatus;

    /// <summary>
    ///     Returns a copy carrying the outcome of a verification attempt.
    /// </summary>
    /// <param name="verified">Whether the verifying service confirmed the document.</param>
    /// <param name="source">The service that produced the outcome.</param>
    /// <returns>A new credential; the current instance is left untouched.</returns>
    public Credential WithVerification(bool verified, string source) => this with
    {
        VerificationStatus = verified ? EVerificationStatus.Verified : EVerificationStatus.Failed,
        VerificationSource = source,
        VerifiedAt = DateTimeOffset.UtcNow
    };

    /// <summary>
    ///     Returns a copy with no verification outcome recorded.
    /// </summary>
    /// <remarks>
    ///     Used when the verifying service does not answer: the credential stays pending rather
    ///     than being presented as verified.
    /// </remarks>
    /// <returns>A new credential; the current instance is left untouched.</returns>
    public Credential AsPending() => this with
    {
        VerificationStatus = EVerificationStatus.Pending,
        VerificationSource = "",
        VerifiedAt = null
    };
}