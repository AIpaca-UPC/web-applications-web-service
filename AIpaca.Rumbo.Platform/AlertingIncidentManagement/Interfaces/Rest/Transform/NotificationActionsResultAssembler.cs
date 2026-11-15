using AIpaca.Rumbo.Platform.Resources.Errors;
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Transform;

/// <summary>
/// Turns the outcome of the application layer into an HTTP response.
/// </summary>
public static class NotificationsActionsResultAssembler
{
    private static int ToStatusCodeFromNotificationsError(AlertingError error) => error switch
    {
        AlertingError.NotificationNotFound => StatusCodes.Status404NotFound,
        AlertingError.InvalidNotificationData => StatusCodes.Status400BadRequest,
        AlertingError.OperationCancelled => StatusCodes.Status409Conflict,
        AlertingError.DatabaseError => StatusCodes.Status500InternalServerError,
        AlertingError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    /// <summary>
    /// Turns the result of any command of this context into an HTTP response.
    /// </summary>
    public static IActionResult ToActionResultFromCommandResult(
        ControllerBase controller,
        Result<Notification> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Notification, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromNotificationsError((AlertingError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>
    /// Turns the outcome of a lookup by identifier into an HTTP response.
    /// </summary>
    public static IActionResult ToActionResultFromGetNotificationByIdResult(
        ControllerBase controller,
        Notification? notification,
        IStringLocalizer errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Notification, IActionResult> successAction)
    {
        if (notification is not null) return successAction(notification);

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromNotificationsError(AlertingError.NotificationNotFound),
            AlertingError.NotificationNotFound,
            errorLocalizer[nameof(AlertingError.NotificationNotFound)]);
    }
}