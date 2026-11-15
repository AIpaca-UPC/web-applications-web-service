namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Queries;

/// <summary>
/// Represents a request to retrieve a route by its identifier.
/// </summary>
public record GetRouteByIdQuery(int RouteId);