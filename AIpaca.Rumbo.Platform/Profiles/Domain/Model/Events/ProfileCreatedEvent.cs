using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Model.Events;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Events;
/// <summary>
///     Domain event for when a profile is created
/// </summary>
/// <param name="ProfileId">The identifier of the created profile</param>
public record ProfileCreatedEvent(ProfileId ProfileId): IEvent;