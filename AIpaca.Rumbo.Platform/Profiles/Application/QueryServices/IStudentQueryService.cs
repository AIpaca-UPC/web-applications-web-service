using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;

namespace AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;

/// <summary>
///     Student query service
/// </summary>
public interface IStudentQueryService
{
    Task<Student?> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken);
    
    Task<IEnumerable<Student>> Handle(GetAllStudentsByTutorIdQuery query, CancellationToken cancellationToken);
}
