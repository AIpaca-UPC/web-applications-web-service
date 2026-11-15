namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Commands;

/// <summary>
/// Represents a request to publish a configured route.
/// </summary>
public record PublishRouteCommand(int RouteId);