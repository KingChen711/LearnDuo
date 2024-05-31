using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Configuration;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder
            .Property(e => e.SubjectId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(e => e.SubjectId)
            .IsUnique();

        builder
            .Property(s => s.Duration)
            .HasColumnType("decimal(18,2)");

        builder
            .Property(s => s.CostPrice)
            .HasColumnType("decimal(18,2)");
    }
}