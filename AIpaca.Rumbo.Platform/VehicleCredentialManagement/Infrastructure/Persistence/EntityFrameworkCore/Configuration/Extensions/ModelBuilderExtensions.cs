using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

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

        // Credentials belong to the vehicle: they are written and read with it, and a
        // credential cannot exist without the vehicle that declared it.
        builder.Entity<Vehicle>().OwnsMany(v => v.Credentials, credential =>
        {
            credential.WithOwner().HasForeignKey("VehicleId");
            credential.Property<int>("Id");
            credential.HasKey("Id");
            credential.Property(c => c.Type).IsRequired();
            credential.Property(c => c.Number).IsRequired().HasMaxLength(40);
            credential.Property(c => c.ExpiresAt);
            credential.Property(c => c.VerificationStatus).IsRequired();
            credential.Property(c => c.VerificationSource).HasMaxLength(120);
            credential.Property(c => c.VerifiedAt);
        });
    }
}