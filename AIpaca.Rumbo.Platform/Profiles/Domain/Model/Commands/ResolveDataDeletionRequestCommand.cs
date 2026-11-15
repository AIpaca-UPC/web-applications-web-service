using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;

public record ResolveDataDeletionRequestCommand(
    DataDeletionRequestId DeletionRequestId, 
    EDeletionRequestStatus Status, 
    string ResolutionNote);
