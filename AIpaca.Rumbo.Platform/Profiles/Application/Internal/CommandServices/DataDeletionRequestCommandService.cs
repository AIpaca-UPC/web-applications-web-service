using AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Application.Internal.CommandServices;

/// <summary>
///     Data deletion request command service.
/// </summary>
public class DataDeletionRequestCommandService(
    IDataDeletionRequestRepository dataDeletionRequestRepository,
    IProfileRepository profileRepository,
    IStudentRepository studentRepository,
    IUnitOfWork unitOfWork) : IDataDeletionRequestCommandService
{
    public async Task<Result<DataDeletionRequest>> Handle(RequestStudentDataDeletionCommand command, CancellationToken cancellationToken)
    {
        var dataDeletionRequest = new DataDeletionRequest(command);
        await dataDeletionRequestRepository.AddAsync(dataDeletionRequest, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<DataDeletionRequest>.Success(dataDeletionRequest);
    }
}
