using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;

/// <summary>
///   Repository interface for managing Student entities.
/// </summary>
public interface IStudentRepository : IBaseRepository<Student>
{
    /// <summary>
    ///    Find a student by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the student.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Student"/> if found, otherwise, null</returns>
    Task<Student?> FindByIdAsync(StudentId id, CancellationToken cancellationToken);
    
    /// <summary>
    ///    Find a student by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the student.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Student"/> if found, otherwise, null</returns>
    Task<Student?> FindByIdAsNoTrackingAsync(StudentId id, CancellationToken cancellationToken);
    
    /// <summary>
    ///   Find all students by their unique identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the students.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of <see cref="Student"/> if found, otherwise, an empty collection</returns>
    Task<IEnumerable<Student>> FindAllByIdsAsync(IEnumerable<StudentId> ids, CancellationToken cancellationToken);
    
    /// <summary>
    ///  Find all students by their tutor profile identifier, ONLY if their relationship has been AUTHORIZED.
    /// </summary>
    /// <param name="tutorId">The identifier of the tutor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of <see cref="Student"/> if found, otherwise, an empty collection</returns>
    Task<IEnumerable<Student>> FindAllByAuthorizedTutorProfileIdAsync(ProfileId tutorId, CancellationToken cancellationToken);
    
    /// <summary>
    ///  Find all students by their tutor profile identifier.
    /// </summary>
    /// <param name="tutorId">The identifier of the tutor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of <see cref="Student"/> if found, otherwise, an empty collection</returns>
    Task<IEnumerable<Student>> FindAllByTutorProfileIdAsync(ProfileId tutorId, CancellationToken cancellationToken);
}
