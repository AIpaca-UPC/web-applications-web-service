using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Shared.Application.Model;

namespace AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;

/// <summary>
///     Profile command service
/// </summary>
public interface IProfileCommandService
{
    Task<Result<Profile>> Handle(CreateProfileCommand command, CancellationToken cancellationToken);
}
