﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly DefaultData _defaultData;

        public DatabaseInitializer(NET1704_221_5_TutorDemandContext context,
            IOptionsMonitor<DefaultData> monitor)
        {
            _context = context;
            _defaultData = monitor.CurrentValue;
        }


        //  Summary:
        //      Initialize Database 
        public async Task InitializeAsync()
        {
            try
            {
                // Check if database is not exist 
                if (!await _context.Database.CanConnectAsync())
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
                }

                ;

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
                if (!_context.Reservations.Any()) await SeedReservationAsync();
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
                new Subject { Name = "ReactJs", SubjectCode = "RC201", SubjectId = Guid.NewGuid(), Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR4d-qz5zSkMYCbYyezUQ0MQmP1pcVG6dAAlX06SeXDcA&s",
                    Duration = 2, CostPrice = 499000, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(100),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 2000},

                new Subject { Name = "NestJs", SubjectCode = "NJ921", SubjectId = Guid.NewGuid(), Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRd9dnsXqD_YMxZ0cYV2TDOfVBncH9SSIl_3pgnIMhzzA&s", Duration = 2.5m, CostPrice = 350000, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(100),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 300},

                new Subject { Name = "NodeJs", SubjectCode = "ND213" ,SubjectId = Guid.NewGuid(), Image = "https://maychusaigon.vn/wp-content/uploads/2023/06/dinh-nghia-nodejs-la-gi-maychusaigon.jpg",
                    Duration = 2.5m, CostPrice = 550000, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 400 },

                new Subject { Name = "C#", SubjectCode = "CS101", SubjectId = Guid.NewGuid(), Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR7AJibMVALCqjy755vLcIillzm4ZTowt2ZvA&s",
                    Duration = 2.5m, CostPrice = 750000, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 550 },

                new Subject { Name = "Java", SubjectCode = "JV202", SubjectId = Guid.NewGuid(), Image = "https://www.devopsschool.com/blog/wp-content/uploads/2022/03/java_logo_icon_168609.png",
                    Duration = 3m, CostPrice = 850000, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(90),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 3000 },

                new Subject { Name = "Python", SubjectCode = "PY303", SubjectId = Guid.NewGuid(),
                    Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/Python-logo-notext.svg/1200px-Python-logo-notext.svg.png",
                    Duration = 3.5m, CostPrice = 450000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(60),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 4000 },

                new Subject { Name = "Ruby", SubjectCode = "RB404", SubjectId = Guid.NewGuid(),
                    Image = "https://www.freecodecamp.org/news/content/images/2021/10/golang.png",
                    Duration = 3.5m, CostPrice = 420000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(62),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 300 },

                new Subject { Name = "Go", SubjectCode = "GO505", SubjectId = Guid.NewGuid(),
                    Image = "https://www.freecodecamp.org/news/content/images/2021/10/golang.png",
                    Duration = 3m, CostPrice = 380000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(75),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 200 },

                new Subject { Name = "Swift", SubjectCode = "SW606", SubjectId = Guid.NewGuid(),
                    Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS1ukkbQX89KTE6TGuvPkrYaOJLWY4CPL9ETw&s",
                    Duration = 2.5m, CostPrice = 242000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(100),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 400},

                new Subject { Name = "Kotlin", SubjectCode = "KT707", SubjectId = Guid.NewGuid(),
                    Image = "https://media.licdn.com/dms/image/D4D12AQGRQ1BBEKdHEg/article-cover_image-shrink_720_1280/0/1702452656552?e=2147483647&v=beta&t=AG_dHioDuELsbuN00lkjoiXm_UkBIr5e7BDGhWr7ExMs",
                    Duration = 3.2m, CostPrice = 387000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(45),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 100 },

                new Subject { Name = "Scala", SubjectCode = "SC808", SubjectId = Guid.NewGuid(),
                    Image = "https://media.licdn.com/dms/image/D4E12AQHy35CpyMDMGg/article-cover_image-shrink_600_2000/0/1707415179126?e=2147483647&v=beta&t=NFWWYE2VhRPKdSGP1vxEFwbAol3PO_not72SxSCz498",
                    Duration = 1.5m, CostPrice = 657000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(25),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 100 },

                new Subject { Name = "Rust", SubjectCode = "RS909", SubjectId = Guid.NewGuid(),
                    Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTuANBqjF223L3XDy5JftheuJ_7jgzFJPvM5Q&s",
                    Duration = 1.5m, CostPrice = 567000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(120),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 200},

                new Subject { Name = "PHP", SubjectCode = "PH101", SubjectId = Guid.NewGuid(),
                    Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQwDJhSqTGNDdevfolscq5WA_nNItAro0_cfg&s",
                    Duration = 2m, CostPrice = 494000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(90),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 1500 },

                new Subject { Name = "PHP1", SubjectCode = "PH1012", SubjectId = Guid.NewGuid(),
                    Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQwDJhSqTGNDdevfolscq5WA_nNItAro0_cfg&s",
                    Duration = 2m, CostPrice = 494000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(90),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 100 },

                new Subject { Name = "PHP2", SubjectCode = "PH1014", SubjectId = Guid.NewGuid(),
                    Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQwDJhSqTGNDdevfolscq5WA_nNItAro0_cfg&s",
                    Duration = 2m, CostPrice = 494000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(90),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 100 },

                new Subject { Name = "PHP3", SubjectCode = "PH1013", SubjectId = Guid.NewGuid(),
                    Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQwDJhSqTGNDdevfolscq5WA_nNItAro0_cfg&s",
                    Duration = 2m, CostPrice = 494000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(90),
                    Description = _defaultData.SubjectDescription,
                    EnrolledCapacity = 100 }
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
            var tutorInfos = new List<(string email, string fullName, string avartar)>
            {
                new ("levanbuoi@gmail.com", "Le Van Buoi", "https://img.freepik.com/premium-vector/avatar-male-teacher-glasses-business-suit_768258-1808.jpg"),
                new ("nguyenvana@gmail.com", "Nguyen Van A", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQdkiAF2n_jshJN4JuA1FPEdtZcrhBgw4jdXD2XP9ULzrDi6QJodpUYsw3mYIZPLH9sBwE&usqp=CAU"),
                new ("tranthibanh@gmail.com", "Tran Thi Banh", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRVMnhq_Y5JXGF-uYXDRdO4p4qXvXbdexjGaA&s"),
                new ("lehoangcam@gmail.com", "Le Hoang Cam", "https://i.pinimg.com/474x/3b/3f/a1/3b3fa1a2db40f8f2610b9cd691cfe8e2.jpg"),
                new ("phamthidung@gmail.com", "Pham Thi Dung", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5jxRHDvFfLTIenODQrMJjvvo0Gv3ePU6gtZO-rNZKUowkA-rwtuslOEwPhGugPkzwa9c&usqp=CAU"),
                new ("nguyenhuuem@gmail.com", "Nguyen Huu Em", "https://img.freepik.com/free-photo/view-3d-male-teacher_23-2150710020.jpg"),
                new ("phanvantai@gmail.com", "Phan Van Tai", "https://img.freepik.com/premium-photo/teacher-man-avatar-icon-illustration-vector-style_131965-957.jpg"),
                new ("doananhgia@gmail.com", "Doan Anh Gia", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRNy1ChXTDxBC6jRuVW3p7dZOrJliOYu8k5UD0GX2vBaIoVCXavR1c0yDkaVD_--8zlUBw&usqp=CAU"),
                new ("ngothihuyen@gmail.com", "Ngo Thi Huyen", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS1wSLVyOVAdsRFqZ49efRZtyyjvn-Xye2D4g&s"),
                new ("vutuanhung@gmail.com", "Vu Tuan Hung", "https://png.pngtree.com/png-clipart/20201225/ourmid/pngtree-original-hand-drawn-illustration-teacher-avatar-png-image_2604471.jpg")
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
                    Avatar = tutorInfos[i].avartar,
                    CertificateImage = null,
                    IdentityCard = null
                });
            }

            await _context.AddRangeAsync(tutors);

            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding tutor data");
        }

        private async Task SeedReservationAsync()
        {
            var customer = await _context.Customers.FirstOrDefaultAsync();
            var tutor = await _context.Tutors.ToListAsync();
            var teachingSchedule = await _context.TeachingSchedules.ToListAsync();

            List<Reservation> reservations = new List<Reservation>()
            {
                new()
                {
                    ReservationId = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
                    TeachingScheduleId = teachingSchedule[0].TeachingScheduleId,
                    PaidPrice = 100,
                    CreatedAt = DateTime.Parse("10-10-2022"),
                    ReservationStatus = nameof(ReservationStatus.Completed),
                    PaymentMethod = nameof(PaymentMethod.Cash),
                    PaymentStatus = nameof(PaymentStatus.Paid)
                },
                new()
                {
                    ReservationId = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
                    TeachingScheduleId = teachingSchedule[1].TeachingScheduleId,
                    PaidPrice = 300,
                    CreatedAt = DateTime.Parse("05-20-2024"),
                    ReservationStatus = nameof(ReservationStatus.Processing),
                    PaymentMethod = nameof(PaymentMethod.DebitCard),
                    PaymentStatus = nameof(PaymentStatus.Paid)
                },
                new()
                {
                    ReservationId = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
                    TeachingScheduleId = teachingSchedule[2].TeachingScheduleId,
                    PaidPrice = 500,
                    CreatedAt = DateTime.Now,
                    ReservationStatus = nameof(ReservationStatus.Processing),
                    PaymentMethod = nameof(PaymentMethod.CreditCard),
                    PaymentStatus = nameof(PaymentStatus.Unpaid)
                }
            };
            await _context.Reservations.AddRangeAsync(reservations);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding Reservation");
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
            var endDate = startDate.AddDays(30); // Assump each schedule has 30 days
            var password = "@Password123";

            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var meetRoomCode = TeachingScheduleHelper.GenerateMeetRoomCode();

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
                // schedules[i].PaidPrice = random.Next(500000, 5000000); // Price from [500.000 - 5.000.000] VND
            }

            // Add more tutor for python subject
            var python = await _context.Subjects.FirstOrDefaultAsync(x => x.Name.Equals("Python"));

            if (!schedules.Select(x => x.SubjectId).Contains(python!.SubjectId))
            {
                var meetRoomCode = TeachingScheduleHelper.GenerateMeetRoomCode();

                schedules.Add(new TeachingSchedule
                {
                    TeachingScheduleId = Guid.NewGuid(),
                    SubjectId = python!.SubjectId,
                    TutorId = tutors[random.Next(tutors.Count)].TutorId,
                    MeetRoomCode = meetRoomCode,
                    RoomPassword = password,
                    StartDate = startDate,
                    EndDate = endDate,
                    // Assign slot
                    SlotId = slots[random.Next(slots.Count)].SlotId,
                    // Assign weekdays
                    LearnDays = TeachingScheduleHelper.GenerateRandomWeekdays(),
                    // Assign price
                    // PaidPrice = random.Next(500000, 5000000) // Price from [500.000 - 5.000.000] VND
                });
            }

            var pythonSchedule = schedules.Where(x => x.SubjectId == python!.SubjectId).FirstOrDefault();

            for (int i = 0; i < 6; ++i)
            {
                var meetRoomCode = TeachingScheduleHelper.GenerateMeetRoomCode();

                var randomTutor = tutors[random.Next(tutors.Count)];

                var isExistTutor = schedules
                    .Where(x => x.SubjectId == python.SubjectId)
                    .Select(x => x.TutorId)
                    .Contains(randomTutor.TutorId);

                if (!isExistTutor)
                {
                    schedules.Add(new TeachingSchedule
                    {
                        TeachingScheduleId = Guid.NewGuid(),
                        SubjectId = python!.SubjectId,
                        TutorId = randomTutor.TutorId,
                        MeetRoomCode = meetRoomCode,
                        RoomPassword = password,
                        StartDate = startDate,
                        EndDate = endDate,
                        // Assign slot
                        SlotId = slots[random.Next(slots.Count)].SlotId,
                        // Assign weekdays
                        LearnDays = TeachingScheduleHelper.GenerateRandomWeekdays(),
                        // Assign price
                        // PaidPrice = random.Next(500000, 5000000) // Price from [500.000 - 5.000.000] VND
                    });
                }
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
                Password = "123456",
                Email = "nguyenvanteo@gmail.com",
                Phone = "0767178991",
                Address = "E2a-7, Street D1, HCM City",
                Gender = nameof(Gender.Male),
            });

            var result = await _context.SaveChangesAsync() > 0;
            if (!result) Console.WriteLine("Something went wrong when seeding customer data");
        }
    }
}