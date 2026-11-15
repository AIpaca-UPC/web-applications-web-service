using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;

/// <summary>
/// Converts route application results into HTTP responses.
/// </summary>
public static class RoutesActionsResultAssembler
{
    /// <summary>
    /// Maps domain errors to HTTP status codes.
    /// </summary>
    private static int ToStatusCode(RoutesError error) => error switch
    {
        RoutesError.RouteNotFound => StatusCodes.Status404NotFound,
        RoutesError.StopNotFound => StatusCodes.Status404NotFound,

        RoutesError.RouteAlreadyPublished => StatusCodes.Status409Conflict,
        RoutesError.StudentAlreadyAssigned => StatusCodes.Status409Conflict,
        RoutesError.StopHasAssignments => StatusCodes.Status409Conflict,

        RoutesError.DatabaseError => StatusCodes.Status500InternalServerError,
        RoutesError.InternalServerError => StatusCodes.Status500InternalServerError,

        _ => StatusCodes.Status400BadRequest
    };

    /// <summary>
    /// Converts a successful or failed command into an HTTP response.
    /// </summary>
    public static IActionResult ToActionResultFromCommandResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess)
            return successAction(result.Value!);

        var error = result.Error as RoutesError?;
        var statusCode = error.HasValue
            ? ToStatusCode(error.Value)
            : StatusCodes.Status400BadRequest;

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            statusCode,
            result.Error,
            result.Message);
    }

    /// <summary>
    /// Returns a standardized bad request for invalid route input.
    /// </summary>
    public static IActionResult ToInvalidRequest(
        ControllerBase controller,
        ProblemDetailsFactory problemDetailsFactory,
        string message)
    {
        return problemDetailsFactory.CreateProblemDetails(
            controller,
            StatusCodes.Status400BadRequest,
            RoutesError.InvalidRouteData,
            message);
    }
}
