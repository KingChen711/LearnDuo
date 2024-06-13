using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Configuration;

public class TeachingScheduleConfiguration : IEntityTypeConfiguration<TeachingSchedule>
{
    public void Configure(EntityTypeBuilder<TeachingSchedule> builder)
    {
        builder
            .Property(e => e.TeachingScheduleId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder
            .Property(e => e.CreationDate)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder
            .Property(e => e.LastUpdated)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAddOrUpdate();

        builder
            .HasIndex(e => e.TeachingScheduleId)
            .IsUnique();

        builder
            .HasOne(d => d.Slot)
            .WithMany(p => p.TeachingSchedules)
            .HasPrincipalKey(p => p.SlotId)
            .HasForeignKey(d => d.SlotId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(d => d.Subject)
            .WithMany(p => p.TeachingSchedules)
            .HasPrincipalKey(p => p.SubjectId)
            .HasForeignKey(d => d.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(d => d.Tutor)
            .WithMany(p => p.TeachingSchedules)
            .HasPrincipalKey(p => p.TutorId)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}