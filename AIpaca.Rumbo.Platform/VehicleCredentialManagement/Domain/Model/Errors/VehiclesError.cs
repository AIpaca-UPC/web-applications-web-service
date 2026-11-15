namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Errors;

/// <summary>
///     Errors the Vehicle &amp; Credential Management bounded context can report.
/// </summary>
public enum VehiclesError
{
    None,
    VehicleNotFound,
    LicensePlateAlreadyRegistered,
    InvalidVehicleData,
    CredentialNotDeclared,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}