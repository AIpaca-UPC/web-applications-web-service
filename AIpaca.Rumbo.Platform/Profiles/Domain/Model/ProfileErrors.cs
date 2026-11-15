using AIpaca.Rumbo.Platform.Shared.Domain.Model.Errors;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model;

public static class ProfileErrors
{
    public static readonly Error InvalidProfileData =
        Error.NotFound("Profiles.InvalidProfileData", "The profile data provided is invalid");
    
    public static readonly Error ProfileNotFound =
        Error.NotFound("Profiles.ProfileNotFound", "The specified profile was not found.");

    public static readonly Error StudentNotFound =
        Error.NotFound("Profiles.StudentNotFound", "The specified student was not found.");

    public static readonly Error DeletionRequestNotFound =
        Error.NotFound("Profiles.DeletionRequestNotFound", "The specified deletion request was not found.");

    public static readonly Error AccountAlreadyHasProfile =
        Error.Conflict("Profiles.AccountAlreadyHasProfile", "Account already has a profile.");

    public static readonly Error AuthorizedTutorRequired =
        Error.Failure("Profiles.AuthorizedTutorRequired", "Requester must be an authorized tutor of an active student.");
    
    public static readonly Error DeletionApprovalUnavailable =
        Error.Conflict("Profiles.DeletionApprovalUnavailable", "Approval unavailable: trip verification and personal-data erasure are not integrated.");

    public static readonly Error ConcurrentUpdate =
        Error.Conflict("Profiles.ConcurrentUpdate", "Concurrent update or database uniqueness conflict.");
}
