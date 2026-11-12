using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;

/// <summary>
///     Record the outcome a verifying service returned for one document of a vehicle.
/// </summary>
/// <param name="VehicleId">The vehicle holding the document.</param>
/// <param name="Type">The kind of document the outcome belongs to.</param>
/// <param name="Answered">
///     Whether the verifying service answered at all. When it did not, the document is left
///     pending instead of being given an outcome it did not produce.
/// </param>
/// <param name="Verified">Whether the service confirmed the document.</param>
/// <param name="Source">The service that produced the outcome.</param>
public record RegisterCredentialVerificationCommand(
    int VehicleId,
    ECredentialType Type,
    bool Answered,
    bool Verified,
    string Source
    );