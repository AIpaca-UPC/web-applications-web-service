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
[Tags("Students")]
public class StudentsController(
    IStudentCommandService studentCommandService,
    IStudentQueryService studentQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
: ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet("{studentId:int}")]
    [EndpointSummary("Get a student by Id")]
    [EndpointDescription("Get a student by Id its unique identifier")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentById(int studentId, CancellationToken cancellationToken)
    {
        var getStudentByIdQuery = new GetStudentByIdQuery(studentId);
        var student = await studentQueryService.Handle(getStudentByIdQuery, cancellationToken);

        return ProfilesActionsResultAssembler.ToActionResultFromGetStudentByIdResult(
            this, student, _errorLocalizer, _problemDetailsFactory,
            foundStudent => Ok(StudentResourceFromEntityAssembler.ToResourceFromEntity(foundStudent)));
    }
    
    [HttpGet("tutors/{tutorId:int}")]
    [EndpointSummary("Get all students by tutor Id")]
    [EndpointDescription("Get all students for which the tutor has an authorized relationship")]
    [ProducesResponseType(typeof(IEnumerable<StudentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStudentsByTutorId(int tutorId, CancellationToken cancellationToken)
    {
        var getAllStudentsByTutorIdQuery = new GetAllStudentsByTutorIdQuery(tutorId);
        var students = await studentQueryService.Handle(getAllStudentsByTutorIdQuery, cancellationToken);
        var studentResources = students.Select(StudentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(studentResources);
    }
    
    [HttpPost]
    [EndpointSummary("Register student")]
    [EndpointDescription("Register student")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterStudent(RegisterStudentResource resource, CancellationToken cancellationToken)
    {
        var registerStudentCommand = RegisterStudentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await studentCommandService.Handle(registerStudentCommand, cancellationToken);
        
        return ProfilesActionsResultAssembler.ToActionResultFromRegisterStudentResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            registeredStudent => CreatedAtAction(nameof(GetStudentById), new { studentId = registeredStudent.Id }, 
                StudentResourceFromEntityAssembler.ToResourceFromEntity(registeredStudent)));
    }
}
