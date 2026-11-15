using AIpaca.Rumbo.Platform.Resources.Errors;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Resources;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Notifications")]
public class NotificationsController(
    INotificationCommandService notificationCommandService,
    INotificationQueryService notificationQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [EndpointSummary("Get all notifications")]
    [EndpointDescription("Get every notification registered in the service")]
    [ProducesResponseType(typeof(IEnumerable<NotificationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNotifications(CancellationToken cancellationToken)
    {
        var notifications = await notificationQueryService.Handle(
            new GetAllNotificationsQuery(), cancellationToken);

        return Ok(notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("profile/{profileId:int}")]
    [EndpointSummary("Get notifications by profile")]
    [EndpointDescription("Get every notification addressed to a specific profile")]
    [ProducesResponseType(typeof(IEnumerable<NotificationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationsByProfileId(
        int profileId, CancellationToken cancellationToken)
    {
        var notifications = await notificationQueryService.Handle(
            new GetNotificationsByProfileIdQuery(profileId), cancellationToken);

        return Ok(notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("student/{studentId:int}")]
    [EndpointSummary("Get notifications by student")]
    [EndpointDescription("Get every notification associated with a specific student")]
    [ProducesResponseType(typeof(IEnumerable<NotificationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationsByStudentId(
        int studentId, CancellationToken cancellationToken)
    {
        var notifications = await notificationQueryService.Handle(
            new GetNotificationsByStudentIdQuery(studentId), cancellationToken);

        return Ok(notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("Create a notification")]
    [EndpointDescription("Create a notification for a specific profile and student")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNotification(
        CreateNotificationResource resource, CancellationToken cancellationToken)
    {
        var command = CreateNotificationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await notificationCommandService.Handle(command, cancellationToken);

        return NotificationsActionsResultAssembler.ToActionResultFromCommandResult(
            this, result, _problemDetailsFactory,
            created => CreatedAtAction(
                nameof(GetNotificationsByProfileId),
                new { profileId = created.RecipientProfileId.Value },
                NotificationResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPatch("{notificationId:int}/read")]
    [EndpointSummary("Mark a notification as read")]
    [EndpointDescription("Mark a specific notification as read")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkNotificationAsRead(
        int notificationId, CancellationToken cancellationToken)
    {
        var command = new MarkNotificationAsReadCommand(notificationId);
        var result = await notificationCommandService.Handle(command, cancellationToken);

        return NotificationsActionsResultAssembler.ToActionResultFromCommandResult(
            this, result, _problemDetailsFactory,
            updated => Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }
}