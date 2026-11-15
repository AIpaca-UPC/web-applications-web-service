using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;
/// <summary>
///     Assembler responsible for transforming DataDeletionRequest entity to DataDeletionRequestResource
/// </summary>
public static class DataDeletionRequestResourceFromEntityAssembler
{
    public static DataDeletionRequestResource ToResourceFromEntity(DataDeletionRequest entity)
    {
        if (entity == null)
            throw new ArgumentException(nameof(entity),
                "DataDeletionRequest entity cannot be null when converting to resource");

        return new DataDeletionRequestResource(
            entity.Id, 
            entity.StudentId, 
            entity.RequestedByTutorId,
            entity.Status.ToString());
    }
}
