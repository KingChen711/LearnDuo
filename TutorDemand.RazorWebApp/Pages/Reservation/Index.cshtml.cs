using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Dtos.Reservation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Utils;
using TutorDemand.RazorWebApp.Pages.Reservation.Models;

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
        public string SearchResultMessage { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public ReservationFilter Filter { get; set; }
        [BindProperty(SupportsGet = true)] public string SearchQuery { get; set; }

        public async Task<IActionResult> OnGet(string? input, int? currentPage = 1)
        {
            const int PageSize = 5;
            CurrentPage = currentPage is not null ? currentPage.Value : 1;

            if (string.IsNullOrEmpty(input))
            {
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
                        var teachingSchedule = await _teachingScheduleBusiness.FindOneAsync(ts =>
                            ts.TeachingScheduleId.Equals(reservation.TeachingScheduleId));
                        var teachingScheduleData = (TeachingSchedule)teachingSchedule.Data!;
                        var tutor = await _tutorBusiness.FindOneAsync(x =>
                            x.TutorId.Equals(teachingScheduleData.TutorId));
                        var subject =
                            await _subjectBusiness.GetByIdAsync(((TeachingSchedule)teachingSchedule.Data!).SubjectId);

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
            }
            else
            {
                var searchingSchedule = await _teachingScheduleBusiness.GetWithConditionAsync(
                    ts => ts.Subject.Name.Contains(input) || ts.Tutor.Fullname.Contains(input), null, "Tutor,Subject");
                if (searchingSchedule.Data is null)
                {
                    SearchResultMessage = "Không có kết quả tìm kiếm phù hợp.";
                    return Page();
                }

                var searchingScheduleData = ((List<TeachingSchedule>)searchingSchedule.Data!).Distinct();
                foreach (var teachingData in searchingScheduleData)
                {
                    var searchingReservation =
                        await _reservationService.GetWithConditionAysnc(x =>
                            x.TeachingScheduleId.Equals(teachingData.TeachingScheduleId));

                    if (searchingReservation.Data is null)
                    {
                        continue;
                    }

                    foreach (var result in (List<Reservation>)searchingReservation.Data!)
                    {
                        var reservationDetail = new ReservationDetailDTO()
                        {
                            Id = result.ReservationId,
                            CreatedAt = result.CreatedAt,
                            PaidPrice = result.PaidPrice,
                            PaymentMethod = result.PaymentMethod,
                            PaymentStatus = result.PaymentStatus,
                            ReservationStatus = result.ReservationStatus,
                            TutorName = teachingData.Tutor.Fullname,
                            SubjectName = teachingData.Subject.Name
                        };
                        Reservations.Add(reservationDetail);
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            const int PageSize = 5;
            CurrentPage = 1;

            var filteredReservations = await _reservationService.GetWithConditionAysnc(reservation =>
                    (!Filter.PaidPrice.HasValue || reservation.PaidPrice == Filter.PaidPrice) &&
                    (!Filter.CreateAt.HasValue || reservation.CreatedAt.Date == Filter.CreateAt.Value.Date) &&
                    (string.IsNullOrEmpty(Filter.TutorName) ||
                     reservation.TeachingSchedule.Tutor.Fullname.Contains(Filter.TutorName)) &&
                    (string.IsNullOrEmpty(Filter.SubjectName) ||
                     reservation.TeachingSchedule.Subject.Name.Contains(Filter.SubjectName)), null, "Tutor,Subject"
            );
            if (filteredReservations.Data is null)
            {
                CurrentPage = 0;
                return Page();
            }

            var reservations = (List<Reservation>)filteredReservations.Data!;
            
            TotalPages = (int)Math.Ceiling(reservations.Count / (double)PageSize);

            var paginatedReservations = reservations
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var reservation in paginatedReservations)
            {
                var teachingSchedule = await _teachingScheduleBusiness.FindOneAsync(ts =>
                    ts.TeachingScheduleId.Equals(reservation.TeachingScheduleId));
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

            return Page();
        }
    }
}