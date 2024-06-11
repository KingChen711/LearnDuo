using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
