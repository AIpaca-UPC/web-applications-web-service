using AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Application.Internal.QueryServices;
/// <summary>
///     Student Query Service
/// </summary>
/// <param name="studentRepository"></param>
public class StudentQueryService(IStudentRepository studentRepository) : IStudentQueryService
{
    public async Task<IEnumerable<Student>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken)
    {
        return await studentRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<Student?> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
    {
        return await studentRepository.FindByIdAsNoTrackingAsync(query.StudentId, cancellationToken);
    }

    public async Task<IEnumerable<Student>> Handle(GetAllStudentsByTutorIdQuery query, CancellationToken cancellationToken)
    {
        return await studentRepository.FindAllByAuthorizedTutorProfileIdAsync(query.TutorId, cancellationToken);
    }
}
