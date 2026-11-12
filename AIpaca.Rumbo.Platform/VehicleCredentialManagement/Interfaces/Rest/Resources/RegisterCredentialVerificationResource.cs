namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;

/// <summary>
///     Resource for recording the outcome a verifying service returned for one document.
/// </summary>
/// <param name="Type">The kind of document: SOAT, CITV or DRIVER_LICENSE.</param>
/// <param name="Answered">
///     Whether the verifying service answered. When <c>false</c> the document is left pending.
/// </param>
/// <param name="Verified">Whether the service confirmed the document.</param>
/// <param name="Source">The service that produced the outcome.</param>
public record RegisterCredentialVerificationResource(
    string Type,
    bool Answered,
    bool Verified,
    string Source);