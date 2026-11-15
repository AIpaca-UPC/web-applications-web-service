using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;

namespace AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;

/// <summary>
///     Profile query service
/// </summary>
public interface IProfileQueryService
{
    Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken);
    
    Task<Profile?> Handle(GetProfileByAccountIdQuery query, CancellationToken cancellationToken);
}
