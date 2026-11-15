using AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Application.Internal.QueryServices;
/// <summary>
///     Profile Query Service
/// </summary>
/// <param name="profileRepository"></param>
public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken)
    {
        return await profileRepository.FindByIdAsNoTrackingAsync(query.ProfileId, cancellationToken);
    }

    public async Task<Profile?> Handle(GetProfileByAccountIdQuery query, CancellationToken cancellationToken)
    {
        return await profileRepository.FindByAccountIdAsync(query.AccountId, cancellationToken);
    }
}
