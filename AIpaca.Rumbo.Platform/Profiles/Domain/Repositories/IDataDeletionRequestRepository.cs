using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;

/// <summary>
///    Repository interface for managing data deletion requests within the Profiles bounded context.
/// </summary>
public interface IDataDeletionRequestRepository : IBaseRepository<DataDeletionRequest>
{
    /// <summary>
    ///     Find a data deletion request by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the data deletion request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="DataDeletionRequest"/> if found, otherwise, null.</returns>
    Task<DataDeletionRequest?> FindByIdAsync(DataDeletionRequestId id, CancellationToken cancellationToken);
    
    /// <summary>
    ///    Find a data deletion request by its unique identifier without change tracking.
    /// </summary>
    /// <param name="id">The identifier of the data deletion request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="DataDeletionRequest"/> if found, otherwise, null.</returns>
    Task<DataDeletionRequest?> FindByIdAsNoTrackingAsync(DataDeletionRequestId id, CancellationToken cancellationToken);
    
    /// <summary>
    ///    Find all data deletion requests made by a specific profile.
    /// </summary>
    /// <param name="requesterId">The identifier of the person who solicited the data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="DataDeletionRequest"/> if found, otherwise, null.</returns>
    Task<IEnumerable<DataDeletionRequest>> FindAllByRequestedByProfileIdAsync(ProfileId requesterId, CancellationToken cancellationToken);
}
