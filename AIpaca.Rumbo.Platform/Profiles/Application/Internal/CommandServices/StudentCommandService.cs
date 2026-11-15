using AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Application.Internal.CommandServices;

/// <summary>
///     Student command service.
/// </summary>
public class StudentCommandService(
    IStudentRepository studentRepository,
    IUnitOfWork unitOfWork) : IStudentCommandService
{
    public async Task<Result<Student>> Handle(RegisterStudentCommand command, CancellationToken cancellationToken)
    {
        var student = new Student(command);
        await studentRepository.AddAsync(student, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Student>.Success(student);
    }
}
