
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core configuration for the
///     Subscription & Billing bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Registers the entity mappings for subscription
    ///     plans and driver subscriptions.
    /// </summary>
    /// <param name="builder">The database model builder.</param>
    public static void ApplySubscriptionBillingConfiguration(
        this ModelBuilder builder)
    {
        // Subscription Plan Aggregate
        builder.Entity<Plan>().ToTable("billing_plans");

        builder.Entity<Plan>().HasKey(plan => plan.Id);

        builder.Entity<Plan>().Property(plan => plan.Id)
            .HasConversion(
                new StronglyTypedIdConverter<PlanId, int>(
                    id => id.Value,
                    value => new PlanId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Plan>().Property(plan => plan.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<Plan>().HasIndex(plan => plan.Name)
            .IsUnique();

        builder.Entity<Plan>().Property(plan => plan.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Entity<Plan>().Property(plan => plan.ReferencePrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Entity<Plan>().Property(plan => plan.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Entity<Plan>().Property(plan => plan.BillingPeriodMonths);

        builder.Entity<Plan>().Property(plan => plan.VehicleLimit);

        builder.Entity<Plan>().Property(plan => plan.Active)
            .IsRequired();

        builder.Entity<Plan>().Ignore(plan => plan.IsAvailable);

        // Driver Subscription Aggregate
        builder.Entity<Subscription>().ToTable("billing_subscriptions");

        builder.Entity<Subscription>()
            .HasKey(subscription => subscription.Id);

        builder.Entity<Subscription>()
            .Property(subscription => subscription.Id)
            .HasConversion(
                new StronglyTypedIdConverter<SubscriptionId, int>(
                    id => id.Value,
                    value => new SubscriptionId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Subscription>()
            .Property(subscription => subscription.PlanId)
            .HasConversion(
                new StronglyTypedIdConverter<PlanId, int>(
                    id => id.Value,
                    value => new PlanId(value)))
            .IsRequired();

        builder.Entity<Subscription>()
            .HasOne<Plan>()
            .WithMany()
            .HasForeignKey(subscription => subscription.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Subscription>()
            .Property(subscription => subscription.DriverProfileId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<Subscription>()
            .HasIndex(subscription => subscription.DriverProfileId);

        builder.Entity<Subscription>()
            .Property(subscription => subscription.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Entity<Subscription>()
            .Property(subscription => subscription.StartedAt)
            .HasColumnType("date")
            .IsRequired();

        builder.Entity<Subscription>()
            .Property(subscription => subscription.CurrentPeriodStart)
            .HasColumnType("date")
            .IsRequired();

        builder.Entity<Subscription>()
            .Property(subscription => subscription.CurrentPeriodEnd)
            .HasColumnType("date")
            .IsRequired();

        builder.Entity<Subscription>()
            .Property(subscription => subscription.VehicleLimit);

        builder.Entity<Subscription>()
            .Property(subscription => subscription.AutoRenew)
            .IsRequired();

        // Store timestamps as UTC datetime values in MySQL.
        var utcTimestampConverter =
            new ValueConverter<DateTimeOffset, DateTime>(
                value => value.UtcDateTime,
                value => new DateTimeOffset(
                    DateTime.SpecifyKind(value, DateTimeKind.Utc)));

        builder.Entity<Subscription>()
            .Property(subscription => subscription.CreatedAt)
            .HasConversion(utcTimestampConverter)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Entity<Subscription>()
            .Property(subscription => subscription.PausedAt)
            .HasConversion(utcTimestampConverter)
            .HasColumnType("datetime(6)");

        builder.Entity<Subscription>()
            .Property(subscription => subscription.CancelledAt)
            .HasConversion(utcTimestampConverter)
            .HasColumnType("datetime(6)");
    }
}
