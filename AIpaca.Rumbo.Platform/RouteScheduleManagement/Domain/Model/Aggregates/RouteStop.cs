namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;

/// <summary>
/// Represents an ordered stop belonging to a route.
/// </summary>
public class RouteStop
{
    private RouteStop()
    {
    }

    public RouteStop(string address, int position)
    {
        UpdateDetails(address, position);
    }

    /// <summary>
    /// Reconstructs an existing stop keeping its identifier.
    /// </summary>
    public RouteStop(int id, string address, int position)
        : this(address, position)
    {
        if (id < 1)
            throw new ArgumentOutOfRangeException(nameof(id));

        Id = id;
    }

    public int Id { get; private set; }

    public string Address { get; private set; } = string.Empty;

    public int Position { get; private set; }

    /// <summary>
    /// Updates the address and ordered position of a stop.
    /// </summary>
    internal void UpdateDetails(string address, int position)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException(
                "Stop address cannot be blank.", nameof(address));

        if (position < 1)
            throw new ArgumentOutOfRangeException(nameof(position));

        Address = address.Trim();
        Position = position;
    }
}