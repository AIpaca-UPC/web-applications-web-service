using AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Application.Internal.QueryServices;
/// <summary>
///     Data Deletion Request Query Service
/// </summary>
/// <param name="dataDeletionRequestRepository"></param>
public class DataDeletionRequestQueryService(IDataDeletionRequestRepository dataDeletionRequestRepository)
    : IDataDeletionRequestQueryService
{
    public async Task<DataDeletionRequest?> Handle(GetDataDeletionRequestByIdQuery query, CancellationToken cancellationToken)
    {
        return await dataDeletionRequestRepository.FindByIdAsNoTrackingAsync(query.DeletionRequestId, cancellationToken);
    }

    public async Task<IEnumerable<DataDeletionRequest>> Handle(GetAllDataDeletionRequestsByRequestedByProfileIdQuery query, CancellationToken cancellationToken)
    {
        return await dataDeletionRequestRepository.FindAllByRequestedByProfileIdAsync(query.RequestedByProfileId, cancellationToken);
    }
}
