using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TutorDemand.Business;
using TutorDemand.Data.Dtos.Reservation;

namespace TutorDemand.Pages.Reservations
{
    public class IndexModel : PageModel
    {
        private readonly IReservationBusiness _reservationService;
        private readonly ITutorBusiness _tutorBusiness;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;


        //cusid, teachingscheduleid => tutor Id, mon hoc
            
            //getall Reservation => 
            
            //Ui => tutorId, mon hoc, va cac thuoc tinh con lai cua reservation
        
        public IndexModel(IReservationBusiness reservationService, ITutorBusiness tutorBusiness,
            ITeachingScheduleBusiness teachingScheduleBusiness
        )
        {
            _reservationService = reservationService;
            _tutorBusiness = tutorBusiness;
            _teachingScheduleBusiness = teachingScheduleBusiness;
        }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public async Task<IActionResult> OnGet()
        {
            var result = _reservationService.GetAll();
            if (result.Status == 1)
            {
                Reservations = (List<Reservation>)result.Data;
                foreach (var reservation in Reservations)
                {
                    var teachingSchedule = await _teachingScheduleBusiness.FindOne(ts
                        => ts.TeachingScheduleId.Equals(reservation.TeachingScheduleId));
                    var tutor = await _tutorBusiness.FindOneAsync(x =>
                        x.TutorId.Equals(teachingSchedule.TeachingScheduleId));
                    var responseReservation = new ReservationDetailDTO()
                    {
                        CreatedAt = reservation.CreatedAt,
                        PaidPrice =  reservation.PaidPrice,
                        PaymentMethod = reservation.PaymentMethod,
                        PaymentStatus = reservation.PaymentMethod,
                        ReservationStatus = reservation.ReservationStatus,
                        SubjectName =
                    };
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