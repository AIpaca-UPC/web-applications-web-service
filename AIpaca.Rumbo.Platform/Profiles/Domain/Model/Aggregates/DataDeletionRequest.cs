using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;

public partial class DataDeletionRequest : AggregateRoot
{
    private DataDeletionRequest() {}

    public DataDeletionRequest(RequestStudentDataDeletionCommand command)
    {
        StudentId = command.StudentId;
        RequestedByTutorId = command.RequestedByProfileId;
        Status = EDeletionRequestStatus.Pending;
    }

    public DataDeletionRequestId Id { get; private set; }
    
    public StudentId StudentId { get; private set; }
    public ProfileId RequestedByTutorId { get; private set; }
    public EDeletionRequestStatus Status { get; private set; }
}
