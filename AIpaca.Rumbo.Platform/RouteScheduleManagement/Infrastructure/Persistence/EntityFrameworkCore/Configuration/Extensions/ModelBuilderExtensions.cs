using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Route = AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Model.Aggregates.Route;

namespace AIpaca.Rumbo.Platform.RouteScheduleManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Entity Framework Core configuration for the
/// Route &amp; Schedule Management bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Registers the Route aggregate and its owned collections
    /// in the application database model.
    /// </summary>
    public static void ApplyRouteScheduleManagementConfiguration(
        this ModelBuilder builder)
    {
        // Strongly typed identifiers are stored as integers.
        var routeIdConverter =
            new StronglyTypedIdConverter<RouteId, int>(
                id => id.Value,
                value => new RouteId(value));

        var driverIdConverter =
            new StronglyTypedIdConverter<DriverProfileId, int>(
                id => id.Value,
                value => new DriverProfileId(value));

        var vehicleIdConverter =
            new StronglyTypedIdConverter<VehicleId, int>(
                id => id.Value,
                value => new VehicleId(value));

        var studentIdConverter =
            new StronglyTypedIdConverter<StudentId, int>(
                id => id.Value,
                value => new StudentId(value));

        // Store TimeOnly values using MySQL TIME.
        var timeConverter = new ValueConverter<TimeOnly, TimeSpan>(
            time => time.ToTimeSpan(),
            value => TimeOnly.FromTimeSpan(value));

        builder.Entity<Route>(route =>
        {
            route.ToTable("Routes");

            // Aggregate primary key.
            route.HasKey(r => r.Id);

            route.Property(r => r.Id)
                .HasConversion(routeIdConverter)
                .ValueGeneratedOnAdd();

            // External bounded context references.
            route.Property(r => r.DriverProfileId)
                .HasConversion(driverIdConverter)
                .IsRequired();

            route.Property(r => r.VehicleId)
                .HasConversion(vehicleIdConverter);

            // Route configuration.
            route.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(120);

            route.Property(r => r.Code)
                .IsRequired()
                .HasMaxLength(20);

            route.HasIndex(r => r.Code).IsUnique();

            route.Property(r => r.Shift).IsRequired();

            route.Property(r => r.Status).IsRequired();

            route.Property(r => r.ScheduleConfirmed).IsRequired();

            route.Property(r => r.DepartureTime)
                .HasConversion(timeConverter)
                .HasColumnType("time");

            // IsPublished is calculated from Status.
            route.Ignore(r => r.IsPublished);

            // Stops belong exclusively to a route.
            route.OwnsMany(r => r.Stops, stops =>
            {
                stops.ToTable("RouteStops");

                stops.WithOwner().HasForeignKey("RouteId");

                stops.Property<RouteId>("RouteId")
                    .HasConversion(routeIdConverter);

                stops.HasKey(s => s.Id);

                stops.Property(s => s.Id)
                    .ValueGeneratedOnAdd();

                stops.Property(s => s.Address)
                    .IsRequired()
                    .HasMaxLength(250);

                stops.Property(s => s.Position).IsRequired();
            });

            // Weekly schedules belong to the route.
            route.OwnsMany(r => r.Schedule, schedules =>
            {
                schedules.ToTable("RouteSchedules");

                schedules.WithOwner().HasForeignKey("RouteId");

                schedules.Property<RouteId>("RouteId")
                    .HasConversion(routeIdConverter);

                // Shadow key: RouteSchedule is a value object.
                schedules.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                schedules.HasKey("Id");

                schedules.Property(s => s.DayOfWeek)
                    .IsRequired();

                schedules.Property(s => s.DepartureTime)
                    .HasConversion(timeConverter)
                    .HasColumnType("time")
                    .IsRequired();
            });

            // Student-to-stop assignments belong to the route.
            route.OwnsMany(r => r.Assignments, assignments =>
            {
                assignments.ToTable("RouteAssignments");

                assignments.WithOwner().HasForeignKey("RouteId");

                assignments.Property<RouteId>("RouteId")
                    .HasConversion(routeIdConverter);

                assignments.HasKey(a => a.Id);

                assignments.Property(a => a.Id)
                    .ValueGeneratedOnAdd();

                assignments.Property(a => a.StopId)
                    .IsRequired();

                assignments.Property(a => a.StudentId)
                    .HasConversion(studentIdConverter)
                    .IsRequired();
            });

            // Access the actual lists, not their read-only wrappers.
            route.Navigation(r => r.Stops)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            route.Navigation(r => r.Schedule)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            route.Navigation(r => r.Assignments)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
