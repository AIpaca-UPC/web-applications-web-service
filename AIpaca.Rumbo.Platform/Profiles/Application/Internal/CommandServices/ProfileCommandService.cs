using AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Errors;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Application.Internal.CommandServices;

/// <summary>
///     Profile command service.
/// </summary>
public class ProfileCommandService(
    IProfileRepository profileRepository,
    IUnitOfWork unitOfWork) : IProfileCommandService
{
    public async Task<Result<Profile>> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = new Profile(command);
        await profileRepository.AddAsync(profile, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Profile>.Success(profile);
    }
}
