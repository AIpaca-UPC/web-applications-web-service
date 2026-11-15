namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Queries;

/// <summary>
/// Represents a request to retrieve routes assigned to a driver.
/// </summary>
public record GetAllRoutesByDriverProfileIdQuery(int DriverProfileId);