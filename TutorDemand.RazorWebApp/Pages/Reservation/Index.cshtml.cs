using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Dtos.Reservation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TutorDemand.Pages.Reservations
{
    public class IndexModel : PageModel
    {
        private readonly IReservationBusiness _reservationService;
        private readonly ITutorBusiness _tutorBusiness;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
        private readonly ISubjectBusiness _subjectBusiness;

        public IndexModel(IReservationBusiness reservationService, ITutorBusiness tutorBusiness,
            ITeachingScheduleBusiness teachingScheduleBusiness, ISubjectBusiness subjectBusiness)
        {
            _reservationService = reservationService;
            _tutorBusiness = tutorBusiness;
            _teachingScheduleBusiness = teachingScheduleBusiness;
            _subjectBusiness = subjectBusiness;
        }

        public List<ReservationDetailDTO> Reservations { get; set; } = new List<ReservationDetailDTO>();

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGet(int currentPage = 1)
        {
            CurrentPage = currentPage;
            const int pageSize = 5;

            var result = _reservationService.GetAll();
            if (result.Status == 1)
            {
                var reservations = (List<Reservation>)result.Data;
                TotalPages = (int)Math.Ceiling(reservations.Count / (double)pageSize);
                
                var paginatedReservations = reservations
                    .Skip((CurrentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                foreach (var reservation in paginatedReservations)
                {
                    var teachingSchedule = await _teachingScheduleBusiness.FindOne(ts
                        => ts.TeachingScheduleId.Equals(reservation.TeachingScheduleId));
                    if (teachingSchedule is null)
                    {
                        return RedirectToPage("/Error");
                    }

                    var listTutor = await _tutorBusiness.FindOneAsync(x =>
                        x.TutorId.Equals(teachingSchedule.TutorId));
                    
                    var tutorData = (Tutor)listTutor.Data!;
                    if (tutorData is null)
                    {
                        return RedirectToPage("/Error");
                    }
;
                    var subject = await _subjectBusiness.GetByIdAsync(teachingSchedule.SubjectId);
                    var subjectData = (Subject)subject.Data;

                    var responseReservation = new ReservationDetailDTO()
                    {
                        Id = reservation.ReservationId,
                        CreatedAt = reservation.CreatedAt,
                        PaidPrice = reservation.PaidPrice,
                        PaymentMethod = reservation.PaymentMethod,
                        PaymentStatus = reservation.PaymentStatus,
                        ReservationStatus = reservation.ReservationStatus,
                        TutorName = tutorData.Fullname,
                        SubjectName = subjectData.Name
                    };

                    Reservations.Add(responseReservation);
                }
            }
            else
            {
                return RedirectToPage("Error");
            }

            return Page();
        }
    }
}
