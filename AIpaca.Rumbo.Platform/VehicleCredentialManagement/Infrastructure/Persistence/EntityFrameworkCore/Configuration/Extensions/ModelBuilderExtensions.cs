using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity configuration for the Vehicle &amp; Credential Management bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Registers the entity configuration of this bounded context with the model builder.
    /// </summary>
    /// <param name="builder">The model builder of the database context.</param>
    public static void ApplyVehicleCredentialManagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Vehicle>().HasKey(v => v.Id);
        builder.Entity<Vehicle>().Property(v => v.Id)
            .HasConversion(new StronglyTypedIdConverter<VehicleId, int>(
                id => id.Value,
                value => new VehicleId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Vehicle>().Property(v => v.Brand).IsRequired().HasMaxLength(60);
        builder.Entity<Vehicle>().Property(v => v.Model).IsRequired().HasMaxLength(60);
        builder.Entity<Vehicle>().Property(v => v.DriverName).HasMaxLength(120);
        builder.Entity<Vehicle>().Property(v => v.Capacity).IsRequired();
        builder.Entity<Vehicle>().Property(v => v.Year).IsRequired();
        builder.Entity<Vehicle>().Property(v => v.Status).IsRequired();

        // The plate lives in the vehicle's own row; it has no identity of its own.
        builder.Entity<Vehicle>().OwnsOne(v => v.Plate, plate =>
        {
            plate.WithOwner().HasForeignKey("Id");
            plate.Property(p => p.Value)
                .HasColumnName("LicensePlate")
                .IsRequired()
                .HasMaxLength(10);
            plate.HasIndex(p => p.Value).IsUnique();
        });

        // The MySQL provider writes DateOnly into a DATE column but reads it back as DateTime,
        // so the value is mapped explicitly in both directions. Subscription & Billing needed
        // the same thing; when a third context does, the converter belongs in Shared.
        var nullableDateOnlyConverter = new ValueConverter<DateOnly?, DateTime?>(
            date => date == null ? null : date.Value.ToDateTime(TimeOnly.MinValue),
            dateTime => dateTime == null ? null : DateOnly.FromDateTime(dateTime.Value));

        // Same provider limitation, one type over: a DATETIME column carries no offset, so the
        // instant is stored in UTC and read back as a UTC offset rather than as a local time.
        var nullableInstantConverter = new ValueConverter<DateTimeOffset?, DateTime?>(
            value => value == null ? null : value.Value.UtcDateTime,
            value => value == null
                ? null
                : new DateTimeOffset(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)));

        // Credentials belong to the vehicle: they are written and read with it, and a
        // credential cannot exist without the vehicle that declared it.
        builder.Entity<Vehicle>().OwnsMany(v => v.Credentials, credential =>
        {
            credential.WithOwner().HasForeignKey("VehicleId");
            credential.Property<int>("Id");
            credential.HasKey("Id");
            credential.Property(c => c.Type).IsRequired();
            credential.Property(c => c.Number).IsRequired().HasMaxLength(40);
            credential.Property(c => c.ExpiresAt)
                .HasConversion(nullableDateOnlyConverter)
                .HasColumnType("date");
            credential.Property(c => c.VerificationStatus).IsRequired();
            credential.Property(c => c.VerificationSource).HasMaxLength(120);
            credential.Property(c => c.VerifiedAt)
                .HasConversion(nullableInstantConverter)
                .HasColumnType("datetime");
        });
    }
}