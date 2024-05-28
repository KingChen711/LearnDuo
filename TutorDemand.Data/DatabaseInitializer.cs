using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Enums;
using TutorDemand.Data.Utils;

namespace TutorDemand.Data
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
        Task TrySeedAsync();
        Task SeedAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly NET1704_221_5_TutorDemandContext _context;

        public DatabaseInitializer(NET1704_221_5_TutorDemandContext context)
        {
            _context = context;
        }


        //  Summary:
        //      Initialize Database 
        public async Task InitializeAsync()
        {
            try
            {
                // Check if database is not exist 
                if (!_context.Database.CanConnect())
                {
                    // Migration Database - Create database 
                    await _context.Database.MigrateAsync();
                }

                // Check if migrations have already been applied 
                var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();

                if (appliedMigrations.Any())
                {
                    Console.WriteLine("Migrations have already been applied. Skip migratons proccess.");
                    return;
                }

                Console.WriteLine("Database migrated successfully");
            }
            catch (Exception)
            {
                throw;
            }

        }

        //  Summary:
        //      Seeding Data
        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //  Summary:
        //      Seeding Data for several entities 
        public async Task TrySeedAsync()
        {
            try
            {
                await SeedSubjectsAsync();
                if (_context.Tutors.Any())
                {
                    Console.WriteLine("Data has been already seed. Skip seeding process.");
                    return;
                };

                Console.WriteLine("--> Seeding Data");

                // Tutors
                if (!_context.Tutors.Any()) await SeedTutorsAsync();
                // Subjects
                if (!_context.Subjects.Any()) await SeedSubjectsAsync();
                // Slots
                if (!_context.Slots.Any()) await SeedSlotsAsync();
                // Teaching Schedules
                if (!_context.TeachingSchedules.Any()) await SeedTeachingSchedulesAsync();
                // Customer (Only 1)
                if (!_context.Customers.Any()) await SeedCustomerAsync();
                // Reservation (UI Only)
                // Company

                Console.WriteLine("--> Seeding Data Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: ", ex.Message);
                throw;
            }
        }

        //  Summary:
        //      Seed subjects data
        private async Task SeedSubjectsAsync()
        {
            // List of subjects to seed
            var subjects = new List<Subject>
            {
                new Subject { Name = "ReactJs", SubjectCode = "RC201", SubjectId = Guid.NewGuid(), Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR4d-qz5zSkMYCbYyezUQ0MQmP1pcVG6dAAlX06SeXDcA&s" },
                new Subject { Name = "NestJs", SubjectCode = "NJ921", SubjectId = Guid.NewGuid(), Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRd9dnsXqD_YMxZ0cYV2TDOfVBncH9SSIl_3pgnIMhzzA&s" },
                new Subject { Name = "NodeJs", SubjectCode = "ND213" ,SubjectId = Guid.NewGuid(), Image = "https://maychusaigon.vn/wp-content/uploads/2023/06/dinh-nghia-nodejs-la-gi-maychusaigon.jpg" },
                new Subject { Name = "C#", SubjectCode = "CS101", SubjectId = Guid.NewGuid() },
                new Subject { Name = "Java", SubjectCode = "JV202", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" },
                new Subject { Name = "Python", SubjectCode = "PY303", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" },
                new Subject { Name = "Ruby", SubjectCode = "RB404", SubjectId = Guid.NewGuid() },
                new Subject { Name = "Go", SubjectCode = "GO505", SubjectId = Guid.NewGuid() },
                new Subject { Name = "Swift", SubjectCode = "SW606", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" },
                new Subject { Name = "Kotlin", SubjectCode = "KT707", SubjectId = Guid.NewGuid() },
                new Subject { Name = "Scala", SubjectCode = "SC808", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" },
                new Subject { Name = "Rust", SubjectCode = "RS909", SubjectId = Guid.NewGuid()},
                new Subject { Name = "PHP", SubjectCode = "PH101", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" },
                new Subject { Name = "PHP1", SubjectCode = "PH1012", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" },
                new Subject { Name = "PHP2", SubjectCode = "PH1014", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" },
                new Subject { Name = "PHP3", SubjectCode = "PH1013", SubjectId = Guid.NewGuid(), Image = "https://example.com/random-image" }
            };
            foreach (var subject in subjects)
            {
                if (await _context.Set<Subject>().AnyAsync(s => s.Name == subject.Name))
                {
                    continue; // Subject already exists, skip it
                }

                await _context.Set<Subject>().AddAsync(subject);
            }

            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding subject data");
        }

        //  Summary:
        //      Seed tutors data
        private async Task SeedTutorsAsync()
        {
            var tutorInfos = new List<(string email, string fullName)>
            {
                new ("levanbuoi@gmail.com", "Le Van Buoi"),
                new ("nguyenvana@gmail.com", "Nguyen Van A"),
                new ("tranthibanh@gmail.com", "Tran Thi Banh"),
                new ("lehoangcam@gmail.com", "Le Hoang Cam"),
                new ("phamthidung@gmail.com", "Pham Thi Dung"),
                new ("nguyenhuuem@gmail.com", "Nguyen Huu Em"),
                new ("phanvantai@gmail.com", "Phan Van Tai"),
                new ("doananhgia@gmail.com", "Doan Anh Gia"),
                new ("ngothihuyen@gmail.com", "Ngo Thi Huyen"),
                new ("vutuanhung@gmail.com", "Vu Tuan Hung")
            };

            var tutors = new List<Tutor>();
            var random = new Random();

            for (int i = 0; i < tutorInfos.Count; ++i)
            {
                tutors.Add(new Tutor
                {
                    TutorId = Guid.NewGuid(),
                    Fullname = tutorInfos[i].fullName,
                    Email = tutorInfos[i].email,
                    Phone = $"0{random.Next(100000000, 999999999)}",
                    Address = $"Address {i + 1}",
                    Gender = (i % 2 == 0) ? "Male" : "Female",
                    Dob = new DateOnly(1980 + i, random.Next(1, 13), random.Next(1, 29)),
                    Avatar = null,
                    CertificateImage = null,
                    IdentityCard = null
                });
            }

            await _context.AddRangeAsync(tutors);

            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding tutor data");
        }

        //  Summary:
        //      Seed slots data
        private async Task SeedSlotsAsync()
        {
            var slots = new List<Slot>();
            var times = new List<TimeSpan>()
            {
                new TimeSpan(7, 30, 0),
                new TimeSpan(9, 15, 0),
                new TimeSpan(12, 30, 0),
                new TimeSpan(15, 00, 0),
            };
            // Generate slot desc
            for (int i = 0; i < times.Count; ++i)
            {
                var slotEntity = new Slot();
                // By default slot duration is 2
                slotEntity.Duration = 2;
                slotEntity.Time = TimeOnly.FromTimeSpan(times[i]);

                // Generate slot desc
                var startTime = times[i].ToString(@"hh\:mm");
                var endTime = times[i]
                    .Add(TimeSpan.FromHours(slotEntity.Duration)).ToString(@"hh\:mm");

                // Assign slot desc
                slotEntity.SlotDesc = $"{startTime} - {endTime}";
                // Slot name
                slotEntity.SlotName = $"Slot {i + 1}";
                // UUID
                slotEntity.SlotId = Guid.NewGuid();

                slots.Add(slotEntity);
            }

            // Save to DB
            await _context.Slots.AddRangeAsync(slots);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding slot data");
        }

        //  Summary:
        //      Seed teaching schedules
        private async Task SeedTeachingSchedulesAsync()
        {
            var schedules = new List<TeachingSchedule>();

            var startDate = new DateOnly(2024, 5, 23);
            var password = "@Password123";

            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var meetRoomCode = TeachingScheduleHelper.GenerateMeetRoomCode();
                var endDate = startDate.AddDays(30); // Assump each schedule has 30 days

                schedules.Add(new()
                {
                    TeachingScheduleId = Guid.NewGuid(),
                    MeetRoomCode = meetRoomCode,
                    RoomPassword = password,
                    StartDate = startDate,
                    EndDate = endDate
                });
            }

            // Get all slots
            var slots = await _context.Slots.ToListAsync();
            // Get all subjects 
            var subjects = await _context.Subjects.ToListAsync();
            // Get all weekdays
            var weekdays = Enum.GetValues<Weekdays>();
            // Add schedule for tutor
            var tutors = await _context.Tutors.ToListAsync();
            for (int i = 0; i < schedules.Count; ++i)
            {
                // Assign tutor
                schedules[i].TutorId = tutors[i].TutorId;
                // Assign subject
                schedules[i].SubjectId = subjects[random.Next(subjects.Count)].SubjectId;
                // Assign slot
                schedules[i].SlotId = slots[random.Next(slots.Count)].SlotId;
                // Assign weekdays
                schedules[i].LearnDays = TeachingScheduleHelper.GenerateRandomWeekdays();
                // Assign price
                schedules[i].PaidPrice = random.Next(500000, 5000000); // Price from [500.000 - 5.000.000] VND
            }

            // Save to DB
            await _context.TeachingSchedules.AddRangeAsync(schedules);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding schedules data");
        }

        //  Summary:
        //      Seed customer 
        private async Task SeedCustomerAsync()
        {
            await _context.Customers.AddAsync(new Customer()
            {
                CustomerId = Guid.NewGuid(),
                Fullname = "Nguyen Van Teo",
                Dob = new DateOnly(1998, 10, 2),
                Email = "nguyenvanteo@gmail.com",
                Phone = "0767178991",
                Address = "E2a-7, Street D1, HCM City",
                Gender = nameof(Gender.Male)
            });

            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding customer data");
        }
    }
}
