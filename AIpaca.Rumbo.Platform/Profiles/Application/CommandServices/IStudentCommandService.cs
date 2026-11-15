using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Shared.Application.Model;

namespace AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;

/// <summary>
///     Student command service
/// </summary>
public interface IStudentCommandService
{
    Task<Result<Student>> Handle(RegisterStudentCommand command, CancellationToken cancellationToken);
}
