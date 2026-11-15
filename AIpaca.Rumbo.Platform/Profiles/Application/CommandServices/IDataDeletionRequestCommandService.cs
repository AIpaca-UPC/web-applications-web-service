using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Shared.Application.Model;

namespace AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;

/// <summary>
///     Data deletion request command service
/// </summary>
public interface IDataDeletionRequestCommandService
{
    Task<Result<DataDeletionRequest>> Handle(RequestStudentDataDeletionCommand command, CancellationToken cancellationToken);
}
