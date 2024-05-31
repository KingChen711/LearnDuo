using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Enums;

namespace TutorDemand.Data.Dtos.Tutor
{
    public class TutorDto
    {
        public int Id { get; set; }

        public Guid TutorId { get; set; }

        public string Fullname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public DateOnly? Dob { get; set; }

        public string? Avatar { get; set; }

        public string? CertificateImage { get; set; }

        public string? IdentityCard { get; set; }


        //public ICollection<TeachingSchedule> TeachingSchedules { get; set; } = [];
    }
}
