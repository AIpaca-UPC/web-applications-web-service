using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.Profiles.Interfaces.Rest.Transform;
/// <summary>
///     Assembler responsible for transforming Student entity to StudentResource
/// </summary>
public static class StudentResourceFromEntityAssembler
{
    public static StudentResource ToResourceFromEntity(Student entity)
    {
        if (entity == null)
            throw new ArgumentException(nameof(entity), "Student entity cannot be null when converting to resource");

        return new StudentResource(entity.Id, entity.FullName, entity.BirthDate, entity.SchoolName,
            entity.Status.ToString());
    }
}
