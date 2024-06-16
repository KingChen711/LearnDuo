using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Dtos.Slot;

namespace TutorDemand.Data.Dtos.TeachingSchedule
{
    public class TeachingScheduleDto
    {
        public int Id { get; set; }

        public Guid TeachingScheduleId { get; set; }

        public Guid TutorId { get; set; }

        public Guid SubjectId { get; set; }

        public Guid SlotId { get; set; }

        public string MeetRoomCode { get; set; } = null!;

        public string RoomPassword { get; set; } = null!;

        public string LearnDays { get; set; } = null!; //Example: "Monday,Wednesday,Friday"

        public int PaidPrice { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public DateTime LastUpdated { get; set; }
        public DateTime CreationDate { get; set; }

        public SubjectDto Subject { get; set; } = null!;
        public TutorDto Tutor { get; set; } = null!;
        public SlotDto Slot { get; set; } = null!;
        //public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
