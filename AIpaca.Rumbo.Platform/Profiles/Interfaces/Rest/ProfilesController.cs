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
[Tags("Profiles")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
: ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet("{profileId:int}")]
    [EndpointSummary("Get a profile by Id")]
    [EndpointDescription("Get a profile by Id its unique identifier")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileById(int profileId, CancellationToken cancellationToken)
    {
        var getProfileByIdQuery = new GetProfileByIdQuery(profileId);
        var profile = await profileQueryService.Handle(getProfileByIdQuery, cancellationToken);

        return ProfilesActionsResultAssembler.ToActionResultFromGetProfileByIdResult(
            this, profile, _errorLocalizer, _problemDetailsFactory,
            foundProfile => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(foundProfile)));
    }
    
    [HttpGet("accounts/{accountId:int}")]
    [EndpointSummary("Get a profile by account Id")]
    [EndpointDescription("Get a profile by the unique identifier of its associated account")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileByAccountId(int accountId, CancellationToken cancellationToken)
    {
        var getProfileByAccountIdQuery = new GetProfileByAccountIdQuery(accountId);
        var profile = await profileQueryService.Handle(getProfileByAccountIdQuery, cancellationToken);

        return ProfilesActionsResultAssembler.ToActionResultFromGetProfileByAccountIdResult(
            this, profile, _errorLocalizer, _problemDetailsFactory,
            foundProfile => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(foundProfile)));
    }
    
    [HttpPost]
    [EndpointSummary("Create profile")]
    [EndpointDescription("Create profile")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProfile(CreateProfileResource resource, CancellationToken cancellationToken)
    {
        var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await profileCommandService.Handle(createProfileCommand, cancellationToken);
        
        return ProfilesActionsResultAssembler.ToActionResultFromCreateProfileResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdProfile => CreatedAtAction(nameof(GetProfileById), new { profileId = createdProfile.Id }, 
                ProfileResourceFromEntityAssembler.ToResourceFromEntity(createdProfile)));
    }
}
