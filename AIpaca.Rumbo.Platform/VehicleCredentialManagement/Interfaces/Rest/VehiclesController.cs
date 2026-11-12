using AIpaca.Rumbo.Platform.Resources.Errors;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Resources;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Vehicles")]
public class VehiclesController(
    IVehicleCommandService vehicleCommandService,
    IVehicleQueryService vehicleQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [EndpointSummary("Get all vehicles")]
    [EndpointDescription("Get every vehicle enrolled in the service, with its declared documents")]
    [ProducesResponseType(typeof(IEnumerable<VehicleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
    {
        var vehicles = await vehicleQueryService.Handle(new GetAllVehiclesQuery(), cancellationToken);
        return Ok(vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{vehicleId:int}")]
    [EndpointSummary("Get a vehicle by Id")]
    [EndpointDescription("Get a vehicle by its unique identifier, with its declared documents")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleById(int vehicleId, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleQueryService.Handle(new GetVehicleByIdQuery(vehicleId), cancellationToken);

        return VehiclesActionsResultAssembler.ToActionResultFromGetVehicleByIdResult(
            this, vehicle, _errorLocalizer, _problemDetailsFactory,
            found => Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(found)));
    }

    [HttpPost]
    [EndpointSummary("Register a vehicle")]
    [EndpointDescription("Register a vehicle together with the documents declared for it")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateVehicle(
        CreateVehicleResource resource, CancellationToken cancellationToken)
    {
        var command = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return VehiclesActionsResultAssembler.ToActionResultFromCommandResult(
            this, result, _problemDetailsFactory,
            created => CreatedAtAction(nameof(GetVehicleById), new { vehicleId = (int)created.Id },
                VehicleResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPut("{vehicleId:int}")]
    [EndpointSummary("Update a vehicle")]
    [EndpointDescription("Update a vehicle and the documents declared for it")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateVehicle(
        int vehicleId, UpdateVehicleResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(vehicleId, resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return VehiclesActionsResultAssembler.ToActionResultFromCommandResult(
            this, result, _problemDetailsFactory,
            updated => Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpPost("{vehicleId:int}/credential-verifications")]
    [EndpointSummary("Record a credential verification")]
    [EndpointDescription(
        "Record the outcome a verifying service returned for one document. When the service " +
        "did not answer, the document is left pending instead of being declared verified")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterCredentialVerification(
        int vehicleId, RegisterCredentialVerificationResource resource, CancellationToken cancellationToken)
    {
        var command = RegisterCredentialVerificationCommandFromResourceAssembler
            .ToCommandFromResource(vehicleId, resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return VehiclesActionsResultAssembler.ToActionResultFromCommandResult(
            this, result, _problemDetailsFactory,
            updated => Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }
}