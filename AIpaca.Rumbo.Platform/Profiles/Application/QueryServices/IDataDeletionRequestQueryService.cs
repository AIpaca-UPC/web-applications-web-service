using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;

namespace AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;

/// <summary>
///     Data deletion request query service
/// </summary>
public interface IDataDeletionRequestQueryService
{
    Task<DataDeletionRequest?> Handle(GetDataDeletionRequestByIdQuery query, CancellationToken cancellationToken);
    
    Task<IEnumerable<DataDeletionRequest>> Handle(GetAllDataDeletionRequestsByRequestedByProfileIdQuery query, CancellationToken cancellationToken);
}
