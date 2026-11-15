using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Queries;

public record GetAllDataDeletionRequestsByRequestedByProfileIdQuery(ProfileId RequestedByProfileId);
