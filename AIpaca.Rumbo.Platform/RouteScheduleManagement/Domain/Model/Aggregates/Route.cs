using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate root responsible for route configuration,
/// ordered stops, weekly schedules and student assignments.
/// </summary>
public partial class Route : AggregateRoot
{
    private readonly List<RouteStop> _stops = [];
    private readonly List<RouteSchedule> _schedule = [];
    private readonly List<RouteAssignment> _assignments = [];

    /// <summary>
    /// Constructor required by Entity Framework Core.
    /// </summary>
    private Route()
    {
    }

    /// <summary>
    /// Creates a new configurable route.
    /// </summary>
    public Route(
        DriverProfileId driverProfileId,
        string name,
        ERouteShift shift,
        VehicleId? vehicleId,
        IEnumerable<RouteStop> stops,
        IEnumerable<RouteSchedule> schedule,
        bool scheduleConfirmed)
    {
        if (driverProfileId.Value < 1)
            throw new ArgumentOutOfRangeException(nameof(driverProfileId));

        DriverProfileId = driverProfileId;

        Code = $"R-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        Status = ERouteStatus.Configurable;

        UpdateInformation(name, shift, vehicleId);
        ReplaceStops(stops);
        UpdateSchedule(schedule, scheduleConfirmed);
    }

    // Aggregate identity and general information.
    public RouteId Id { get; private set; }

    public DriverProfileId DriverProfileId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Code { get; private set; } = string.Empty;

    public ERouteShift Shift { get; private set; }

    public ERouteStatus Status { get; private set; }

    public VehicleId? VehicleId { get; private set; }

    public TimeOnly? DepartureTime { get; private set; }

    public bool ScheduleConfirmed { get; private set; }

    // Child entities and weekly schedule.
    public IReadOnlyCollection<RouteStop> Stops =>
        _stops.AsReadOnly();

    public IReadOnlyCollection<RouteSchedule> Schedule =>
        _schedule.AsReadOnly();

    public IReadOnlyCollection<RouteAssignment> Assignments =>
        _assignments.AsReadOnly();

    /// <summary>
    /// Indicates whether the route is published.
    /// </summary>
    public bool IsPublished => Status == ERouteStatus.Published;

    /// <summary>
    /// Updates route information while the route is configurable.
    /// </summary>
    public void UpdateInformation(
        string name,
        ERouteShift shift,
        VehicleId? vehicleId)
    {
        RequireConfigurable();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Route name cannot be blank.",
                nameof(name));

        if (!Enum.IsDefined(shift))
            throw new ArgumentOutOfRangeException(nameof(shift));

        if (vehicleId.HasValue && vehicleId.Value.Value < 1)
            throw new ArgumentOutOfRangeException(nameof(vehicleId));

        Name = name.Trim();
        Shift = shift;
        VehicleId = vehicleId;
    }

    /// <summary>
    /// Replaces and reorders route stops while preserving
    /// the identifiers of existing stops.
    /// </summary>
    public void ReplaceStops(IEnumerable<RouteStop> proposed)
    {
        RequireConfigurable();
        ArgumentNullException.ThrowIfNull(proposed);

        var incoming = proposed.ToList();

        var current = _stops
            .Where(stop => stop.Id > 0)
            .ToDictionary(stop => stop.Id);

        var retainedIds = new HashSet<int>();

        // Validate the replacement before modifying the aggregate.
        foreach (var stop in incoming)
        {
            if (stop is null)
                throw new ArgumentException(
                    "Stops cannot contain null.");

            if (string.IsNullOrWhiteSpace(stop.Address))
                throw new ArgumentException(
                    "Stop address cannot be blank.");

            if (stop.Id == 0)
                continue;

            if (!current.ContainsKey(stop.Id))
                throw new InvalidOperationException(
                    $"Stop {stop.Id} is not part of this route.");

            if (!retainedIds.Add(stop.Id))
                throw new InvalidOperationException(
                    $"Duplicate stop ID {stop.Id}.");
        }

        // Stops with assigned students cannot be removed
        // before their assignments are cleared.
        if (_assignments.Any(
                assignment => !retainedIds.Contains(assignment.StopId)))
            throw new InvalidOperationException(
                "Cannot remove a stop with assigned students.");

        var replacement = new List<RouteStop>();

        for (var i = 0; i < incoming.Count; i++)
        {
            var stop = incoming[i];
            var position = i + 1;

            if (stop.Id > 0)
            {
                var existing = current[stop.Id];

                existing.UpdateDetails(stop.Address, position);

                replacement.Add(existing);
            }
            else
            {
                replacement.Add(
                    new RouteStop(stop.Address, position));
            }
        }

        _stops.Clear();
        _stops.AddRange(replacement);
    }

    /// <summary>
    /// Updates the weekly schedule of a configurable route.
    /// </summary>
    public void UpdateSchedule(
        IEnumerable<RouteSchedule> proposed,
        bool confirmed)
    {
        RequireConfigurable();
        ArgumentNullException.ThrowIfNull(proposed);

        var rows = proposed.ToList();
        var days = new HashSet<DayOfWeek>();

        foreach (var row in rows)
        {
            if (row is null)
                throw new ArgumentException(
                    "Schedule cannot contain null.");

            if (!Enum.IsDefined(row.DayOfWeek))
                throw new ArgumentException(
                    "Invalid schedule day.");

            if (!days.Add(row.DayOfWeek))
                throw new InvalidOperationException(
                    "Duplicate schedule day.");
        }

        if (confirmed && rows.Count == 0)
            throw new InvalidOperationException(
                "Confirmed schedule cannot be empty.");

        _schedule.Clear();
        _schedule.AddRange(rows);

        ScheduleConfirmed = confirmed;

        DepartureTime = rows.Count > 0
            ? rows[0].DepartureTime
            : null;
    }

    /// <summary>
    /// Assigns a student to an existing route stop.
    /// </summary>
    public void AssignStudent(
        int stopId,
        StudentId studentId)
    {
        RequireConfigurable();

        if (studentId.Value < 1)
            throw new ArgumentOutOfRangeException(nameof(studentId));

        if (!HasStop(stopId))
            throw new InvalidOperationException(
                "Stop is not part of this route.");

        if (IsStudentAssigned(studentId))
            throw new InvalidOperationException(
                "Student is already assigned to this route.");

        if (_assignments.Any(
                assignment => assignment.StopId == stopId))
            throw new InvalidOperationException(
                "Stop already has an assigned student.");

        _assignments.Add(
            new RouteAssignment(stopId, studentId));
    }

    /// <summary>
    /// Removes the student assignment from an existing stop.
    /// Does not remove the stop or the student entity.
    /// </summary>
    /// <param name="stopId">Identifier of the route stop.</param>
    public void UnassignStudent(int stopId)
    {
        RequireConfigurable();

        if (!HasStop(stopId))
            throw new InvalidOperationException(
                "Stop is not part of this route.");

        _assignments.RemoveAll(
            assignment => assignment.StopId == stopId);
    }

    /// <summary>
    /// Replaces student assignments while preserving
    /// existing assignment identifiers when possible.
    /// </summary>
    public void ReplaceAssignments(
        IEnumerable<RouteAssignment> proposed)
    {
        RequireConfigurable();
        ArgumentNullException.ThrowIfNull(proposed);

        var incoming = proposed.ToList();
        var students = new HashSet<StudentId>();
        var stops = new HashSet<int>();

        foreach (var assignment in incoming)
        {
            if (assignment is null)
                throw new ArgumentException(
                    "Assignments cannot contain null.");

            if (!HasStop(assignment.StopId))
                throw new InvalidOperationException(
                    "Assigned stop is not part of this route.");

            if (assignment.StudentId.Value < 1)
                throw new ArgumentException(
                    "Invalid student identifier.");

            if (!students.Add(assignment.StudentId))
                throw new InvalidOperationException(
                    "Student is assigned more than once.");

            if (!stops.Add(assignment.StopId))
                throw new InvalidOperationException(
                    "Stop cannot have more than one assigned student.");
        }

        var replacement = new List<RouteAssignment>();

        foreach (var assignment in incoming)
        {
            var existing = FindAssignment(assignment.StudentId);

            if (existing is not null)
            {
                existing.MoveToStop(assignment.StopId);
                replacement.Add(existing);
            }
            else
            {
                replacement.Add(
                    new RouteAssignment(
                        assignment.StopId,
                        assignment.StudentId));
            }
        }

        _assignments.Clear();
        _assignments.AddRange(replacement);
    }

    /// <summary>
    /// Publishes a route when its configuration is complete.
    /// </summary>
    public void Publish()
    {
        if (IsPublished)
            throw new InvalidOperationException(
                "Route has already been published.");

        if (_stops.Count == 0 ||
            !ScheduleConfirmed ||
            _schedule.Count == 0 ||
            _assignments.Count == 0)
            throw new InvalidOperationException(
                "Publishing requires stops, a confirmed schedule " +
                "and at least one assigned student.");

        if (VehicleId is null)
            throw new InvalidOperationException(
                "Publishing requires a vehicle.");

        if (_assignments.Any(
                assignment => !HasStop(assignment.StopId)))
            throw new InvalidOperationException(
                "Assignments must reference existing stops.");

        Status = ERouteStatus.Published;
    }

    /// <summary>
    /// Determines whether a stop belongs to the route.
    /// </summary>
    public bool HasStop(int stopId) =>
        stopId > 0 &&
        _stops.Any(stop => stop.Id == stopId);

    /// <summary>
    /// Determines whether a student is assigned to this route.
    /// </summary>
    public bool IsStudentAssigned(StudentId studentId) =>
        _assignments.Any(
            assignment => assignment.StudentId == studentId);

    /// <summary>
    /// Determines whether the route operates on a given day.
    /// </summary>
    public bool OperatesOn(DayOfWeek day) =>
        ScheduleConfirmed &&
        _schedule.Any(row => row.DayOfWeek == day);

    /// <summary>
    /// Finds an existing assignment by student identifier.
    /// </summary>
    public RouteAssignment? FindAssignment(StudentId studentId) =>
        _assignments.FirstOrDefault(
            assignment => assignment.StudentId == studentId);

    /// <summary>
    /// Ensures that the route has not been published.
    /// </summary>
    private void RequireConfigurable()
    {
        if (IsPublished)
            throw new InvalidOperationException(
                "Published routes cannot be reconfigured.");
    }
}
