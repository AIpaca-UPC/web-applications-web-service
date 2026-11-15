using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;

/// <summary>
///   Repository interface for managing Profile entities
/// </summary>
public interface IProfileRepository : IBaseRepository<Profile>
{
    /// <summary>
    ///    Find a profile by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the profile.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Profile"/> if found, otherwise, null</returns>
    Task<Profile?> FindByIdAsync(ProfileId id, CancellationToken cancellationToken);
    
    /// <summary>
    ///   Find a profile by its unique identifier without change tracking.
    /// </summary>
    /// <param name="id">The identifier of the profile.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Profile"/> if found, otherwise, null</returns>
    Task<Profile?> FindByIdAsNoTrackingAsync(ProfileId id, CancellationToken cancellationToken);
    
    /// <summary>
    ///     Find a profile by its account identifier.
    /// </summary>
    /// <param name="accountId">The identifier of the profile's associated account.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Profile"/> if found, otherwise, null</returns>
    Task<Profile?> FindByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken);
    
    /// <summary>
    ///     Check if a profile exists by its account identifier.
    /// </summary>
    /// <param name="accountId">The identifier of the profile's associated account.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if found, otherwise, null</returns>
    Task<bool> ExistsByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken);
}
