namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Errors;

public enum EProfileErrors
{
    None, 
    ProfileNotFound,
    PhoneNumberAlreadyRegistered,
    InvalidProfileData,
    DeletionRequestAlreadyExists,
    StudentIsInactive,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}