using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Configuration;

public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder
            .Property(e => e.TutorId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(e => e.TutorId)
            .IsUnique();

        builder
            .HasIndex(e => e.Email)
            .IsUnique();
    }
}