using AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;
using AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;
using AIpaca.Rumbo.Platform.Resources.Errors;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Data Deletion Requests")]
public class DataDeletionRequestsController(
    IDataDeletionRequestCommandService dataDeletionRequestCommandService,
    IDataDeletionRequestQueryService dataDeletionRequestQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
: ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet("{dataDeletionRequestId:int}")]
    [EndpointSummary("Get a data deletion request by Id")]
    [EndpointDescription("Get a data deletion request by Id its unique identifier")]
    [ProducesResponseType(typeof(DataDeletionRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDataDeletionRequestById(int dataDeletionRequestId, CancellationToken cancellationToken)
    {
        var getDataDeletionRequestByIdQuery = new GetDataDeletionRequestByIdQuery(dataDeletionRequestId);
        var dataDeletionRequest = await dataDeletionRequestQueryService.Handle(getDataDeletionRequestByIdQuery, cancellationToken);

        return ProfilesActionsResultAssembler.ToActionResultFromGetDataDeletionRequestByIdResult(
            this, dataDeletionRequest, _errorLocalizer, _problemDetailsFactory,
            foundDataDeletionRequest => Ok(DataDeletionRequestResourceFromEntityAssembler.ToResourceFromEntity(foundDataDeletionRequest)));
    }
    
    [HttpGet("requesters/{requestedByProfileId:int}")]
    [EndpointSummary("Get all data deletion requests by requester Id")]
    [EndpointDescription("Get all data deletion requests made by a tutor profile")]
    [ProducesResponseType(typeof(IEnumerable<DataDeletionRequestResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDataDeletionRequestsByRequestedByProfileId(int requestedByProfileId,
        CancellationToken cancellationToken)
    {
        var getAllDataDeletionRequestsByRequestedByProfileIdQuery =
            new GetAllDataDeletionRequestsByRequestedByProfileIdQuery(requestedByProfileId);
        var dataDeletionRequests = await dataDeletionRequestQueryService.Handle(
            getAllDataDeletionRequestsByRequestedByProfileIdQuery, cancellationToken);
        var dataDeletionRequestResources =
            dataDeletionRequests.Select(DataDeletionRequestResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(dataDeletionRequestResources);
    }
    
    [HttpPost]
    [EndpointSummary("Request student data deletion")]
    [EndpointDescription("Request student data deletion")]
    [ProducesResponseType(typeof(DataDeletionRequestResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RequestStudentDataDeletion(RequestStudentDataDeletionResource resource,
        CancellationToken cancellationToken)
    {
        var requestStudentDataDeletionCommand =
            RequestStudentDataDeletionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await dataDeletionRequestCommandService.Handle(requestStudentDataDeletionCommand, cancellationToken);
        
        return ProfilesActionsResultAssembler.ToActionResultFromRequestStudentDataDeletionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdDataDeletionRequest => CreatedAtAction(nameof(GetDataDeletionRequestById),
                new { dataDeletionRequestId = createdDataDeletionRequest.Id }, 
                DataDeletionRequestResourceFromEntityAssembler.ToResourceFromEntity(createdDataDeletionRequest)));
    }
}
