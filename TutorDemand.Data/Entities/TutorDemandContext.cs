using Microsoft.EntityFrameworkCore;
using TutorDemand.Data.Configuration;

namespace TutorDemand.Data.Entities;

public class TutorDemandContext : DbContext
{
    public TutorDemandContext(DbContextOptions<TutorDemandContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Slot> Slots { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<TeachingSchedule> TeachingSchedules { get; set; }
    public DbSet<Tutor> Tutors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        modelBuilder.ApplyConfiguration(new SlotConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        modelBuilder.ApplyConfiguration(new TeachingScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new TutorConfiguration());
    }

}