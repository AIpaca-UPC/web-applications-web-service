
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.AlertingIncidentManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAlertingIncidentManagementDefaultConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Notification>().HasKey(n => n.Id);
        builder.Entity<Notification>().Property(n => n.Id)
            .HasConversion(new StronglyTypedIdConverter<NotificationId, int>(
                id => id.Value,
                value => new NotificationId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        builder.Entity<Notification>().Property(n => n.Category).IsRequired();
        builder.Entity<Notification>().Property(n => n.Type).IsRequired();
        builder.Entity<Notification>().Property(n => n.Priority).IsRequired();
        builder.Entity<Notification>().Property(n => n.Title).IsRequired().HasMaxLength(120);
        builder.Entity<Notification>().Property(n => n.Message).IsRequired().HasMaxLength(1000);
        builder.Entity<Notification>().Property(n => n.ReadAt);


        builder.Entity<Notification>().Property(n => n.RouteId)
            .HasConversion(new StronglyTypedIdConverter<RouteId, int>(
                id => id.Value,
                value => new RouteId(value)))
            .IsRequired();

    
        builder.Entity<Notification>().Property(n => n.TripId)
            .HasConversion(new StronglyTypedIdConverter<TripId, int>(
                id => id.Value,
                value => new TripId(value)))
            .IsRequired();


        builder.Entity<Notification>().Property(n => n.StudentId)
            .HasConversion(new StronglyTypedIdConverter<StudentId, int>(
                id => id.Value,
                value => new StudentId(value)))
            .IsRequired();

 
        builder.Entity<Notification>().Property(n => n.RecipientProfileId)
            .HasConversion(new StronglyTypedIdConverter<ProfileId, int>(
                id => id.Value,
                value => new ProfileId(value)))
            .IsRequired();


        builder.Entity<Notification>().Property(n => n.JourneyEventType)
            .IsRequired(false);
    }
}
