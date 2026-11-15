namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Errors;

public enum EProfileErrors
{
    None,
    InvalidProfileData,
    OperationCancelled,
    DatabaseError,
    InternalServerError,
    ProfileNotFound,
    StudentNotFound,
    DeletionRequestNotFound,
    AccountAlreadyHasProfile,
    AuthorizedTutorRequired,
    DeletionApprovalUnavailable,
    ConcurrentUpdate
}