﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using TutorDemand.Data.Configuration;

namespace TutorDemand.Data.Entities;

public class NET1704_221_5_TutorDemandContext : DbContext
{
    public NET1704_221_5_TutorDemandContext()
    {
    }

    public NET1704_221_5_TutorDemandContext(DbContextOptions<NET1704_221_5_TutorDemandContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Slot> Slots { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<TeachingSchedule> TeachingSchedules { get; set; }
    public DbSet<Tutor> Tutors { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

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