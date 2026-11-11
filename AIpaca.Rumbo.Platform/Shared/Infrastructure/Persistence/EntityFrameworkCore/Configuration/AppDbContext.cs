using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
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
        // builder.ApplyVehicleCredentialManagementConfiguration();

        // Profiles & Relationship Management Context
        // builder.ApplyProfilesRelationshipManagementConfiguration();

        // Route & Schedule Management Context
        // builder.ApplyRouteScheduleManagementConfiguration();

        // Trip Execution & Monitoring Context
        // builder.ApplyTripExecutionMonitoringConfiguration();

        // Alerting & Incident Management Context
        // builder.ApplyAlertingIncidentManagementConfiguration();

        // Subscription & Billing Context
        // builder.ApplySubscriptionBillingConfiguration();

        // Identity & Access Management Context
        // builder.ApplyIdentityAccessManagementConfiguration();

        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}
