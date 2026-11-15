using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;

public class DataDeletionRequest : AggregateRoot
{
    private DataDeletionRequest()
    {
    }

    public DataDeletionRequest(RequestStudentDataDeletionCommand command) : this(command, DateTimeOffset.UtcNow)
    {
    }

    public DataDeletionRequest(RequestStudentDataDeletionCommand command, DateTimeOffset requestedAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.StudentId.Value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.RequestedByProfileId.Value);

        StudentId = command.StudentId;
        RequestedByProfileId = command.RequestedByProfileId;
        Status = EDeletionRequestStatus.Pending;
        RequestedAt = requestedAt;
    }

    public DataDeletionRequestId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public ProfileId RequestedByProfileId { get; private set; }
    public EDeletionRequestStatus Status { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public string? ResolutionNote { get; private set; }

    public void Approve(string note, DateTimeOffset at) => Resolve(EDeletionRequestStatus.Approved, note, at);

    public void Reject(string note, DateTimeOffset at) => Resolve(EDeletionRequestStatus.Rejected, note, at);

    private void Resolve(EDeletionRequestStatus status, string note, DateTimeOffset at)
    {
        if (Status != EDeletionRequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be resolved.");

        ArgumentException.ThrowIfNullOrWhiteSpace(note);
        if (note.Length > 500)
            throw new ArgumentException("Resolution note cannot exceed 500 characters.");
        if (at < RequestedAt)
            throw new ArgumentException("Resolution cannot precede request.");

        Status = status;
        ResolutionNote = note;
        ResolvedAt = at;
    }
}
