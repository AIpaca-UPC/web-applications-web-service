using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.Resources.Errors;
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using Microsoft.Extensions.Localization;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;

public static class ProfilesActionsResultAssembler
{
    private static int ToStatusCodeFromProfilesError(EProfileErrors error)
    {
        return error switch
        {
            EProfileErrors.ProfileNotFound => StatusCodes.Status404NotFound,
            EProfileErrors.StudentNotFound => StatusCodes.Status404NotFound,
            EProfileErrors.DeletionRequestNotFound => StatusCodes.Status404NotFound,
            EProfileErrors.AccountAlreadyHasProfile => StatusCodes.Status409Conflict,
            EProfileErrors.AuthorizedTutorRequired => StatusCodes.Status403Forbidden,
            EProfileErrors.DeletionApprovalUnavailable => StatusCodes.Status409Conflict,
            EProfileErrors.ConcurrentUpdate => StatusCodes.Status409Conflict,
            EProfileErrors.InvalidProfileData => StatusCodes.Status400BadRequest,
            EProfileErrors.OperationCancelled => StatusCodes.Status409Conflict,
            EProfileErrors.DatabaseError => StatusCodes.Status500InternalServerError,
            EProfileErrors.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }
    
    // --- Specific Assembler Methods ---

    public static IActionResult ToActionResultFromCreateProfileResult(
        ControllerBase controller,
        Result<Profile> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Profile, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        
        var statusCode = ToStatusCodeFromProfilesError((EProfileErrors)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetProfileByIdResult(
        ControllerBase controller,
        Profile? profile,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Profile, IActionResult> successAction)
    {
        if (profile is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromProfilesError(EProfileErrors.ProfileNotFound),
                EProfileErrors.ProfileNotFound,
                errorLocalizer[nameof(EProfileErrors.ProfileNotFound)]);
        return successAction(profile);
    }

    public static IActionResult ToActionResultFromGetProfileByAccountIdResult(
        ControllerBase controller,
        Profile? profile,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Profile, IActionResult> successAction)
    {
        if (profile is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromProfilesError(EProfileErrors.ProfileNotFound),
                EProfileErrors.ProfileNotFound,
                errorLocalizer[nameof(EProfileErrors.ProfileNotFound)]);
        return successAction(profile);
    }

    public static IActionResult ToActionResultFromRegisterStudentResult(
        ControllerBase controller,
        Result<Student> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Student, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        
        var statusCode = ToStatusCodeFromProfilesError((EProfileErrors)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetStudentByIdResult(
        ControllerBase controller,
        Student? student,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Student, IActionResult> successAction)
    {
        if (student is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromProfilesError(EProfileErrors.StudentNotFound),
                EProfileErrors.StudentNotFound,
                errorLocalizer[nameof(EProfileErrors.StudentNotFound)]);
        return successAction(student);
    }

    public static IActionResult ToActionResultFromRequestStudentDataDeletionResult(
        ControllerBase controller,
        Result<DataDeletionRequest> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<DataDeletionRequest, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        
        var statusCode = ToStatusCodeFromProfilesError((EProfileErrors)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetDataDeletionRequestByIdResult(
        ControllerBase controller,
        DataDeletionRequest? dataDeletionRequest,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<DataDeletionRequest, IActionResult> successAction)
    {
        if (dataDeletionRequest is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromProfilesError(EProfileErrors.DeletionRequestNotFound),
                EProfileErrors.DeletionRequestNotFound,
                errorLocalizer[nameof(EProfileErrors.DeletionRequestNotFound)]);
        return successAction(dataDeletionRequest);
    }
}
