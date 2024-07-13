using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Dtos.TeachingSchedule;

namespace TutorDemand.Data.Dtos.Reservation
{
    public class ReservationDetails
    {
        public Guid ReservationId { get; set; }
        public int PaidPrice { get; set; }
        public string ReservationStatus { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string TutorName { get; set; } = null!;
        
        public Guid TeachingScheduleId { get; set; }
        public TeachingScheduleDto TeachingSchedule { get; set; }
    }
}
