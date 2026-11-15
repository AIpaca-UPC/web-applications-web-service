
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Resources;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Interfaces.Rest.Transform;

/// <summary>
/// Transforms route action request bodies into domain commands.
/// </summary>
public static class RouteActionCommandFromResourceAssembler
{
    /// <summary>
    /// Creates a command to update a route's weekly schedule.
    /// </summary>
    public static UpdateRouteScheduleCommand ToUpdateScheduleCommand(
        int routeId,
        UpdateRouteScheduleResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new UpdateRouteScheduleCommand(
            routeId,
            RouteWireFormat.ToSchedule(resource.Schedule),
            resource.Confirm);
    }

    /// <summary>
    /// Creates a command to assign a student to a stop.
    /// </summary>
    public static AssignStudentToRouteCommand ToAssignStudentCommand(
        int routeId,
        int stopId,
        AssignStudentResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new AssignStudentToRouteCommand(
            routeId,
            stopId,
            resource.StudentId);
    }

    /// <summary>
    /// Creates a command to replace route assignments.
    /// </summary>
    public static ReplaceRouteAssignmentsCommand ToReplaceAssignmentsCommand(
        int routeId,
        ReplaceRouteAssignmentsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        ArgumentNullException.ThrowIfNull(resource.Assignments);

        var assignments = resource.Assignments.Select(item =>
            new RouteAssignment(
                item.StopId,
                new StudentId(item.StudentId))).ToList();

        return new ReplaceRouteAssignmentsCommand(
            routeId,
            assignments);
    }
}
