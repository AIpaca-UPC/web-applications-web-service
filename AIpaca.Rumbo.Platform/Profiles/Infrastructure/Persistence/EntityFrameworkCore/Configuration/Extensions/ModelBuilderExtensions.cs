using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Entities;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace AIpaca.Rumbo.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProfilesRelationshipManagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Profile>().ToTable("profiles");
        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().HasIndex(p => p.AccountId).IsUnique();

        builder.Entity<Profile>().Property(p => p.Id)
            .HasConversion(new StronglyTypedIdConverter<ProfileId, int>(
                id => id.Value,
                value => new ProfileId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Profile>().Property(p => p.AccountId)
            .HasConversion(new StronglyTypedIdConverter<AccountId, int>(
                    id => id.Value,
                    value => new AccountId(value)))
            .IsRequired();
        
        builder.Entity<Profile>().OwnsOne(p => p.Name,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(p => p.FirstName)
                    .HasColumnName("FirstName").HasMaxLength(40).IsRequired();
                n.Property(p => p.LastName)
                    .HasColumnName("LastName").HasMaxLength(40).IsRequired();
            });
        
        builder.Entity<Profile>().OwnsOne(p => p.PhoneNumber,
            p => {
                p.WithOwner().HasForeignKey("Id");
                p.Property(p => p.Number)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(9);
            });
        
        
        
        builder.Entity<Student>().ToTable("students");
        builder.Entity<Student>().HasKey(s => s.Id);
        
        builder.Entity<Student>().Property(s => s.Id)
            .HasConversion(new StronglyTypedIdConverter<StudentId, int>(
                id => id.Value, 
                value => new StudentId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        builder.Entity<Student>().OwnsOne(s => s.Name, 
            name => 
            { 
                name.WithOwner().HasForeignKey("Id"); 
                name.Property(s => s.FirstName)
                    .HasColumnName("FirstName").HasMaxLength(40).IsRequired();
                name.Property(s => s.LastName)
                    .HasColumnName("LastName").HasMaxLength(40).IsRequired(); 
            });
        
        builder.Entity<Student>().Property(s => s.BirthDate)
            .IsRequired();

        builder.Entity<Student>().Property(s => s.Status)
            .HasConversion<string>();
        
       
        
        builder.Entity<TutorStudentRelationship>().ToTable("tutor_student_relationships");
        builder.Entity<TutorStudentRelationship>().HasKey(r => r.Id);

        builder.Entity<TutorStudentRelationship>().Property(r => r.Id)
            .HasConversion(new StronglyTypedIdConverter<TutorStudentRelationshipId, int>(
                id => id.Value,
                value => new TutorStudentRelationshipId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<TutorStudentRelationship>().Property(r => r.TutorProfileId)
            .HasConversion(new StronglyTypedIdConverter<ProfileId, int>(
                id => id.Value,
                value => new ProfileId(value)))
            .IsRequired();

        builder.Entity<TutorStudentRelationship>().Property(r => r.StudentId)
            .HasConversion(new StronglyTypedIdConverter<StudentId, int>(
                id => id.Value,
                value => new StudentId(value)))
            .IsRequired();

        builder.Entity<TutorStudentRelationship>().Property(r => r.RelationshipType)
            .HasConversion<string>();

        builder.Entity<TutorStudentRelationship>().Property(r => r.AuthorizationStatus)
            .HasConversion<string>();
        
        
        
        builder.Entity<DataDeletionRequest>().ToTable("data_deletion_requests");
        builder.Entity<DataDeletionRequest>().HasKey(r => r.Id);

        builder.Entity<DataDeletionRequest>().Property(r => r.Id)
            .HasConversion(new StronglyTypedIdConverter<DataDeletionRequestId, int>(
                id => id.Value,
                value => new DataDeletionRequestId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<DataDeletionRequest>().Property(r => r.StudentId)
            .HasConversion(new StronglyTypedIdConverter<StudentId, int>(
                id => id.Value,
                value => new StudentId(value)))
            .IsRequired();

        builder.Entity<DataDeletionRequest>().Property(r => r.RequestedByTutorId)
            .HasConversion(new StronglyTypedIdConverter<ProfileId, int>(
                id => id.Value,
                value => new ProfileId(value)))
            .IsRequired();

        builder.Entity<DataDeletionRequest>().Property(r => r.Status)
            .HasConversion<string>();
    }
}
