using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using AIpaca.Rumbo.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context for the Rumbo Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <summary>
    ///     Declares how the date types of the domain travel to and from the database.
    /// </summary>
    /// <remarks>
    ///     The MySQL provider accepts a DateOnly or a DateTimeOffset when writing but returns a
    ///     DateTime when reading, which Entity Framework cannot cast back. That is a trait of the
    ///     provider and not of any one bounded context, so the conversion is declared once here:
    ///     every context gets it, including the ones not written yet, and nobody has to rediscover
    ///     the limitation the day a stored date is first read.
    ///
    ///     The column types are kept as they already are, DATE and DATETIME, so this changes how
    ///     the values are read and written and nothing about the schema.
    /// </remarks>
    /// <param name="configurationBuilder">
    ///     The builder that configures conventions for this context.
    /// </param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        configurationBuilder.Properties<DateTimeOffset>()
            .HaveConversion<InstantConverter>()
            .HaveColumnType("datetime");
    }

    /// <summary>
    ///     On creating the database model
    /// </summary>
    /// <remarks>
    ///     Each Bounded Context contributes its own entity configuration through an extension
    ///     method declared in its Infrastructure layer. A line is uncommented here only once the
    ///     corresponding context exists, so the project always compiles while it grows.
    /// </remarks>
    /// <param name="builder">
    ///     The model builder for the database context
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Vehicle & Credential Management Context
        builder.ApplyVehicleCredentialManagementConfiguration();

        // Profiles & Relationship Management Context
        builder.ApplyProfilesRelationshipManagementConfiguration();

        // Route & Schedule Management Context
        builder.ApplyRouteScheduleManagementConfiguration();

        // Trip Execution & Monitoring Context
        // builder.ApplyTripExecutionMonitoringConfiguration();

        // Alerting & Incident Management Context
        builder.ApplyAlertingIncidentManagementDefaultConfiguration();

        // Subscription & Billing Context
        builder.ApplySubscriptionBillingConfiguration();

        // Identity & Access Management Context
        // builder.ApplyIdentityAccessManagementConfiguration();

        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}