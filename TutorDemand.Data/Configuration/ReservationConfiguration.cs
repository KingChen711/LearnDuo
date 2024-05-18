using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Configuration;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder
            .Property(e => e.ReservationId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(e => e.ReservationId)
            .IsUnique();

        builder
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder
            .HasOne(d => d.Customer)
            .WithMany(p => p.Reservations)
            .HasPrincipalKey(p => p.CustomerId)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.TeachingSchedule)
            .WithMany(p => p.Reservations)
            .HasPrincipalKey(p => p.TeachingScheduleId)
            .HasForeignKey(d => d.TeachingScheduleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}