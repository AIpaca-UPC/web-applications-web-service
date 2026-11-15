namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to delete an existing route.
/// </summary>
public record DeleteRouteCommand(int RouteId);