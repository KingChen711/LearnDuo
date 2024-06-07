using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Dtos.Reservation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public async Task<IActionResult> OnGet(int? currentPage = 1)
        {
            const int PageSize = 5;
            CurrentPage = currentPage is not null ? currentPage.Value : 1;

            var result = _reservationService.GetAll();
            if (result.Status == 1)
            {
                var allReservations = (List<Reservation>)result.Data;
                TotalPages = (int)Math.Ceiling(allReservations.Count / (double)PageSize);

                var paginatedReservations = allReservations
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                foreach (var reservation in paginatedReservations)
                {
                    var teachingSchedule = await _teachingScheduleBusiness.FindOne(ts => ts.TeachingScheduleId.Equals(reservation.TeachingScheduleId));
                    var teachingScheduleData = (TeachingSchedule)teachingSchedule.Data!;
                    var tutor = await _tutorBusiness.FindOneAsync(x => x.TutorId.Equals(teachingScheduleData.TutorId));
                    var subject = await _subjectBusiness.GetByIdAsync(((TeachingSchedule)teachingSchedule.Data!).SubjectId);

                    var reservationDetail = new ReservationDetailDTO()
                    {
                        Id = reservation.ReservationId,
                        CreatedAt = reservation.CreatedAt,
                        PaidPrice = reservation.PaidPrice,
                        PaymentMethod = reservation.PaymentMethod,
                        PaymentStatus = reservation.PaymentStatus,
                        ReservationStatus = reservation.ReservationStatus,
                        TutorName = ((Tutor)tutor.Data!).Fullname,
                        SubjectName = ((Subject)subject.Data!).Name
                    };

                    Reservations.Add(reservationDetail);
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
